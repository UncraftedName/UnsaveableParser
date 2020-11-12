using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using SaveParser.Parser;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using static SaveParser.Parser.Constants;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace SaveParser.Utils.BitStreams {
	
	public partial struct BitStreamReader {
		
		public string? ReadSymbol(SaveInfo info) => info.ParseContext.CurrentSymbolTable![ReadSShort()];


		private static bool TryGetMapFromString(string name, out DataMap? map) {
			if (!GlobalDataMapCollection.MapsByName.TryGetValue(name, out map)) {
				var caller = new StackTrace().GetFrame(2)!.GetMethod()!;
				Console.Out.WriteLineColored($"{caller.ReflectedType!.FullName}.{caller.Name} - datamap for \"{name}\" not found");
				return false;
			}
			return true;
		}


		public bool TryReadDataMapRecursive(string dataMapName, SaveInfo info, out ParsedDataMap? result) {
			if (TryGetMapFromString(dataMapName, out DataMap? map)) {
				try {
					result = ReadDataMapRecursive(map!, info);
					return true;
				} catch (Exception e) {
					info.AddError($"exception while reading map \"{dataMapName}\" recursively: {e.Message}");
				}
			}
			result = null;
			return false;
		}
		

		public ParsedDataMap ReadDataMapRecursive(DataMap map, SaveInfo info) {
			if (map.BaseMap == null)
				return ReadDataMap(map, info);
			ParsedDataMap baseFields = ReadDataMapRecursive(map.BaseMap, info);
			ParsedDataMap thisFields = ReadDataMap(map, info);
			thisFields.AddBaseParsedMap(baseFields);
			return thisFields;
		}


		public ParsedDataMap ReadDataMap(string dataMapName, SaveInfo info)
			=> ReadDataMap(GlobalDataMapCollection.MapsByName[dataMapName], info);


		public void StartBlock(SaveInfo info, out short byteSize, out string? symbol) {
			byteSize = ReadSShort();
			symbol = ReadSymbol(info);
			info.ParseContext.Blocks.Push(CurrentByteIndex + byteSize);
		}


		public void StartBlock(SaveInfo info, out string? symbol) => StartBlock(info, out _, out symbol);
		public void StartBlock(SaveInfo info, out short byteSize) => StartBlock(info, out byteSize, out _);
		public void StartBlock(SaveInfo info) => StartBlock(info, out _, out _);


		public void SkipCurrentBlock(SaveInfo info) {
			CurrentByteIndex = info.ParseContext.Blocks.Pop();
		}


		public void EndBlock(SaveInfo info) {
			int expected = info.ParseContext.Blocks.Pop();
			if (expected != CurrentByteIndex)
				throw new ConstraintException($"wrong amount of bytes read, expected = {expected}, current = {CurrentByteIndex}");
		}



		public ParsedDataMap ReadDataMap(DataMap map, SaveInfo info) {
			if (ReadSShort() != 4)
				throw new ConstraintException("first entry in data map should be 4");
			string sym = ReadSymbol(info)!;
			if (sym != map.Name) {
				GlobalDataMapCollection.MapsByName.TryGetValue(sym, out DataMap? cmpMap);
				if (cmpMap != map)
					throw new ArgumentException($"bad symbol, expected \"{map.Name}\" but read \"{sym}\"");
			}
			int fieldsSaved = ReadSInt();
			ParsedDataMap ret = new ParsedDataMap(map, fieldsSaved);
			for (int i = 0; i < fieldsSaved; i++) {
				StartBlock(info, out short byteSize, out string? s);
				map.FieldDict.TryGetValue(s!, out TypeDesc? curFieldDesc);
				bool skip = !ShouldReadField(curFieldDesc, out string? reason);
				if (!skip) {
					try {
						int index = AbsoluteByteIndex;
						var f = ReadSaveField(curFieldDesc!, info, byteSize);
						if (f != null) { // I allow returning null here for the vphys read
							f.ByteIndex = index;
							ret.AddSaveField(f);
						}
					} catch (Exception e) {
						info.AddError($"exception while reading field {s}: {e.Message}");
						skip = true;
					}
				} else {
					info.AddError($"skipping field \"{s}\" from datamap \"{map.Name}\", reason: {reason}");
				}
				if (skip)
					SkipCurrentBlock(info);
				else
					EndBlock(info);
			}
			return ret;
		}


		// all of these generic types must* stay consistent with TypeDesc.GetNetTypeFromFieldType
		public ParsedSaveField? ReadSaveField(TypeDesc desc, SaveInfo info, int bytesAvail) {
			switch (desc.FieldType) {
				case EMBEDDED:
					if (desc.NumElements == 1) {
						return new ParsedSaveField<ParsedDataMap>(ReadDataMapRecursive(desc.EmbeddedMap!, info), desc);
					} else {
						ParsedDataMap[] parsedMaps = new ParsedDataMap[desc.NumElements];
						for (int i = 0; i < desc.NumElements; i++)
							parsedMaps[i] = ReadDataMapRecursive(desc.EmbeddedMap!, info);
						return new ParsedSaveField<ParsedDataMap[]>(parsedMaps, desc, parsedMaps.Length);
					}
				case CUSTOM:
					return desc.InvokeCustomReadFunc(ref this, info); // this can return null
				case MODELNAME:
					if (desc.NumElements == 1)
						return new ParsedSaveField<ModelName>((ModelName)ReadStringOfLength(bytesAvail), desc);
					throw new NotImplementedException("reading multiple strings is not implemented");
				case SOUNDNAME:
					if (desc.NumElements == 1)
						return new ParsedSaveField<SoundName>((SoundName)ReadStringOfLength(bytesAvail), desc);
					throw new NotImplementedException("reading multiple strings is not implemented");
				case FUNCTION:
					if (desc.NumElements == 1)
						return new ParsedSaveField<Func>((Func)ReadStringOfLength(bytesAvail), desc);
					throw new NotImplementedException("reading multiple strings is not implemented");
				case STRING:
					if (desc.NumElements == 1)
						return new ParsedSaveField<string>(ReadStringOfLength(bytesAvail), desc);
					throw new NotImplementedException("reading multiple strings is not implemented");
				case MODELINDEX:
					if (desc.NumElements == 1)
						return new ParsedSaveField<ModelIndex>((ModelIndex)ReadStringOfLength(bytesAvail), desc);
					throw new NotImplementedException("reading multiple strings is not implemented");
				case MATERIALINDEX:
					return ReadSimple<int, MaterialIndex>(desc, bytesAvail, i => (MaterialIndex)i);
				case FLOAT:
					return ReadSimple<float>(desc, bytesAvail);
				case VECTOR:
					return ReadSimple<Vector3>(desc, bytesAvail);
				case POSITION_VECTOR:
					return ReadSimple<Vector3>(desc, bytesAvail, vec => vec + info.LandmarkPos);
				case QUATERNION:
					return ReadSimple<Vector4>(desc, bytesAvail);
				case EHANDLE:
					return ReadSimple<int, Ehandle>(desc, bytesAvail, i => (Ehandle)i);
				case CLASSPTR:
					return ReadSimple<int, ClassPtr>(desc, bytesAvail, i => (ClassPtr)i);
				case EDICT:
					return ReadSimple<int, Edict>(desc, bytesAvail, i => (Edict)i);
				case INTEGER:
					return ReadSimple<int>(desc, bytesAvail);
				case BOOLEAN:
					return ReadSimple<byte, bool>(desc, bytesAvail, b => b != 0);
				case SHORT:
					return ReadSimple<short>(desc, bytesAvail);
				case BYTE:
					return ReadSimple<byte>(desc, bytesAvail);
				case INTERVAL:
					return ReadSimple<Interval>(desc, bytesAvail);
				case CHARACTER:
					return desc.NumElements == 1 ? ReadSimple<byte, char>(desc, bytesAvail, b => (char)b)
						: new ParsedSaveField<CharArray>(ReadCharArray(bytesAvail), desc);
				case COLOR32:
					return ReadSimple<int, Color32>(desc, bytesAvail, i => (Color32)i);
				case TIME:
					return ReadSimple<float, Time>(desc, bytesAvail, f => {
						if (f == ZERO_TIME)
							return (Time)0;
						if (f != INVALID_TIME && f != FLT_MAX)
							return (Time)(f + info.BaseTime);
						return (Time)f;
					});
				case TICK:
					int baseTick = info.TimeToTicks(info.BaseTime + 0.1f);
					return ReadSimple<int, Tick>(desc, bytesAvail,
						i => (Tick)(i == TICK_NEVER_THINK_ENCODE ? TICK_NEVER_THINK : i + baseTick));
				case VMATRIX:
					return ReadSimple<VMatrix>(desc, bytesAvail);
				case VMATRIX_WORLDSPACE:
					return ReadSimple<VMatrix>(desc, bytesAvail, mat => {
						mat.ApplyTranslation(in info.LandmarkPos);
						return mat;
					});
				case MATRIX3X4_WORLDSPACE:
					return ReadSimple<Matrix3X4>(desc, bytesAvail, mat => {
						mat.GetColumn(3, out Vector3 vec);
						vec += info.LandmarkPos;
						mat.SetColumn(3, in vec);
						return mat;
					});
			}
			throw new NotImplementedException($"reading for {desc.FieldType} is not implemented");
		}


		private static bool ShouldReadField(TypeDesc? desc, out string? reason) { // reason for skip
			if (desc == null) {
				reason = "no appropriate field found";
				return false;
			}

			if ((desc.Flags & DescFlags.FTYPEDESC_SAVE) == 0) {
				reason = "this field should not be saved";
				return false;
			}

			/*if ((desc.Flags & DescFlags.FTYPEDESC_GLOBAL) != 0) { //  todo m_global ????
				reason = "something to do with globals idk";
				return false;
			}*/

			// for me
			if (desc.FieldType == CUSTOM && desc.CustomReadFunc == null) {
				reason = "this field requires a custom read function but it is not present";
				return false;
			}

			// i think this is equivalent to what the game does
			if (desc.FieldType == EMBEDDED && (desc.Flags & DescFlags.FTYPEDESC_PTR) != 0) {
				reason = "this field is embedded but is also a pointer?";
				return false;
			}

			reason = null;
			return true;
		}


		private ParsedSaveField ReadSimple<T>(TypeDesc desc, int bytesAvail, Func<T,T>? selectFunc = null) {
			ReadSimpleToOneOrMany(desc, bytesAvail, out int actualCount, out OneOrMany<T> res);
			if (selectFunc == null) {
				if (res.HasOnlyOne)
					return new ParsedSaveField<T>(res.Single, desc);
				else
					return new ParsedSaveField<T[]>(res.ToArray(), desc, actualCount);
			} else {
				if (res.HasOnlyOne)
					return new ParsedSaveField<T>(selectFunc(res.Single), desc);
				else
					return new ParsedSaveField<T[]>(res.Select(selectFunc).ToArray(), desc, actualCount);
			}
		}


		private ParsedSaveField ReadSimple<TIn, TOut>(TypeDesc desc, int bytesAvail, Func<TIn, TOut> mapFunc) {
			ReadSimpleToOneOrMany(desc, bytesAvail, out int actualCount, out OneOrMany<TIn> res);
			if (res.HasOnlyOne)
				return new ParsedSaveField<TOut>(mapFunc(res.Single), desc);
			else
				return new ParsedSaveField<TOut[]>(res.Select(mapFunc).ToArray(), desc, actualCount);
		}


		// a messy bit of code that reads either a single value or an array of values,
		// for "educational" purposes I decided to make this generic instead of doing every type by hand
		private void ReadSimpleToOneOrMany<T>(TypeDesc desc, int bytesAvail, out int actualCount, out OneOrMany<T> result) {
			if (OneOrMany<T>.SimpleReadFunc == null)
				throw new NotImplementedException($"reading for {desc.FieldType} not implemented");
			
			int desiredBytes = desc.NumElements * OneOrMany<T>.SizeOfType;
			int actualBytes;
			if (bytesAvail == 0) {
				actualBytes = desiredBytes;
			} else {
				Debug.Assert(bytesAvail % OneOrMany<T>.SizeOfType == 0, $"avail: {bytesAvail}, size of type: {OneOrMany<T>.SizeOfType}");
				actualBytes = Math.Min(desiredBytes, bytesAvail);
			}
			actualCount = actualBytes / OneOrMany<T>.SizeOfType;
			result = new OneOrMany<T>(actualCount, desc.NumElements == 1);
			for (int i = 0; i < actualCount; i++)
				result.AddValue(OneOrMany<T>.SimpleReadFunc!(ref this));
			
			if (actualBytes < bytesAvail)
				CurrentByteIndex += bytesAvail - actualBytes;
		}


		private struct OneOrMany<T> : IEnumerable<T> {
			
			public delegate T ReadFunc(ref BitStreamReader bsr);
			
			public static readonly int SizeOfType;
			public static readonly ReadFunc SimpleReadFunc;
			
			public readonly bool HasOnlyOne;
			public T Single;
			public readonly IEnumerable<T>? Many;


			// I have to use reflection to set the delegate otherwise the compiler screams about it being the wrong type 
			static OneOrMany() {
				if (typeof(T) == typeof(int)) {
					SetReadFunc(nameof(ReadIntWrap));
					SizeOfType = 4;
				} else if (typeof(T) == typeof(Vector3)) {
					SetReadFunc(nameof(ReadVec3Wrap));
					SizeOfType = 12;
				} else if (typeof(T) == typeof(Vector4)) {
					SetReadFunc(nameof(ReadVec4Wrap));
					SizeOfType = 16;
				} else if (typeof(T) == typeof(float)) {
					SetReadFunc(nameof(ReadFloatWrap));
					SizeOfType = 4;
				} else if (typeof(T) == typeof(byte)) {
					SetReadFunc(nameof(ReadByteWrap));
					SizeOfType = 1;
				} else if (typeof(T) == typeof(short)) {
					SetReadFunc(nameof(ReadShortWrap));
					SizeOfType = 2;
				} else if (typeof(T) == typeof(Matrix3X4)) {
					SetReadFunc(nameof(ReadMatrix3X4Wrap));
					SizeOfType = 48;
				} else if (typeof(T) == typeof(VMatrix)) {
					SetReadFunc(nameof(ReadMatrix4X4Wrap));
					SizeOfType = 64;
				} else if (typeof(T) == typeof(Interval)) {
					SetReadFunc(nameof(ReadIntervalWrap));
					SizeOfType = 8;
				} else {
					SizeOfType = -1;
					SimpleReadFunc = null!;
					Console.Out.WriteLineColored($"{typeof(OneOrMany<T>)}: read func not implemented");
				}
			}


			private static void SetReadFunc(string name) {
				var field = typeof(OneOrMany<T>).GetField(nameof(SimpleReadFunc), BindingFlags.Static | BindingFlags.Public);
				field!.SetValue(null, Delegate.CreateDelegate(typeof(ReadFunc), typeof(BitStreamReader), name));
			}
			

			public OneOrMany(int count, bool? hasOnlyOne = null) {
				if (hasOnlyOne.HasValue)
					HasOnlyOne = hasOnlyOne.Value;
				else
					HasOnlyOne = count == 1;
				Single = default!;
				Many = HasOnlyOne ? null : new List<T>(count);
			}


			public void AddValue(T val) {
				if (HasOnlyOne)
					Single = val;
				else
					((List<T>)Many!).Add(val); // values are only added while it's still a list
			}


			public IEnumerator<T> GetEnumerator() {
				Debug.Assert(!HasOnlyOne && Many != null);
				return Many.GetEnumerator();
			}


			IEnumerator IEnumerable.GetEnumerator() {
				return GetEnumerator();
			}
		}


		private static int ReadIntWrap(ref BitStreamReader bsr) => bsr.ReadSInt();
		private static short ReadShortWrap(ref BitStreamReader bsr) => bsr.ReadSShort();
		private static byte ReadByteWrap(ref BitStreamReader bsr) => bsr.ReadByte();
		private static Vector3 ReadVec3Wrap(ref BitStreamReader bsr) {
			bsr.ReadVector3(out Vector3 tmp);
			return tmp;
		}
		private static Vector4 ReadVec4Wrap(ref BitStreamReader bsr) => new Vector4(bsr.ReadFloat(), bsr.ReadFloat(), bsr.ReadFloat(), bsr.ReadFloat());
		private static float ReadFloatWrap(ref BitStreamReader bsr) => bsr.ReadFloat();
		private static Matrix3X4 ReadMatrix3X4Wrap(ref BitStreamReader bsr) => (Matrix3X4)bsr.ReadFloatMat(3, 4);
		private static VMatrix ReadMatrix4X4Wrap(ref BitStreamReader bsr) => (VMatrix)bsr.ReadFloatMat(4, 4);
		private static Interval ReadIntervalWrap(ref BitStreamReader bsr) => new Interval(bsr.ReadFloat(), bsr.ReadFloat());
	}
}