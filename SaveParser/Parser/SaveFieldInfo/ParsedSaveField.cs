using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;

namespace SaveParser.Parser.SaveFieldInfo {


	// NOTE: every class that inherits should have an explicit ctor(TypeDesc), see ParsedDataMap.GetCustomTypeFieldOrDefault
	public abstract class ParsedSaveField : AppendableClass {
		
		protected static readonly Exception InvalidCastE = new InvalidCastException($"{typeof(ParsedSaveField)}: bad cast");

		public virtual object? FieldAsObj => throw new Exception("not sure how you called this");
		public readonly TypeDesc Desc;
		public readonly int ElemCount;
		public int ByteIndex;


		protected ParsedSaveField(TypeDesc desc, int elemCount = 1) {
			Desc = desc;
			ElemCount = elemCount;
			ByteIndex = -1;
		}
	}


	public class ParsedSaveField<T> : ParsedSaveField {
		
		public T Field;
		public static implicit operator T(ParsedSaveField<T> p) => p.Field;

		public override object FieldAsObj => Field!;

		public ParsedSaveField(T field, TypeDesc desc, int elemCount = 1) : base(desc, elemCount) {
			Field = field;
		}
		
		// do not remove, see note at ParsedSaveField
		public ParsedSaveField(TypeDesc desc) : this (default!, desc) {}
		
		
		private static bool IsOverride(MethodInfo m) {
			return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
		}
		
		private static readonly HashSet<Type> _validEnumerableTypes = new HashSet<Type>();
		
		
		// a wee bit silly
		public override void AppendToWriter(IIndentedWriter iw) {
			switch (Desc.FieldType) {
				case FieldType.EMBEDDED: {
					iw.Append($"{Desc.EmbeddedMap!.Name}");
					if (ElemCount == 1) {
						if (!(Field is ParsedDataMap))
							throw new Exception($"Unexpected type for embedded field: {Field!.GetType()}");
						iw.Append($" {Desc.Name}:");
						EnumerableAppendHelper(((ParsedDataMap)FieldAsObj).FieldsDict.Values, iw);
					} else {
						iw.Append($"[{ElemCount}] {Desc.Name}:");
						EnumerableAppendHelper((FieldAsObj as IEnumerable<ParsedDataMap>)!, iw);
					}
					return;
				}
				case FieldType.CUSTOM:
					iw.Append($"{Desc}: ");
					break;
				default:
					iw.Append($"{Desc.TypeString.PadRight(12)} {Desc.Name}: ");
					break;
			}
			if (Field == null) {
				iw.Append("null");
				return;
			}
			if (ElemCount == 1) {
				if (Field is IAppendable ap)
					ap.AppendToWriter(iw);
				else if (Field is IEnumerable<IAppendable> || typeof(IAppendable).IsAssignableFrom(Field!.GetType().GetElementType()))
					EnumerableAppendHelper((IEnumerable<IAppendable>)Field, iw);
				else
					iw.Append(Field!.ToString()!);
			} else {

				if (Field is CharArray chrArr) {
					iw.Append(chrArr);
					return;
				}

				// todo
				if (Field is IEnumerable ie && ie.GetType() != typeof(IEnumerable)) { // wacky huh?
					Type t = ie.GetType();
					if (!_validEnumerableTypes.Contains(t)) {
						iw.Append(ie.SequenceToString());
						return;
					}
					Type elemType = null!;
					if (t.IsArray)
						elemType = t.GetElementType()!;
					if (IsOverride(elemType.GetMethod("ToString", new Type[0])!))
						_validEnumerableTypes.Add(t);
					//var a = typeof(IEnumerable<>).IsAssignableFrom(t);
					//if (t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				}



				switch (Field) {
					case IEnumerable<float> f:
						iw.Append(f.SequenceToString());
						break;
					case IEnumerable<int> i:
						iw.Append(i.SequenceToString());
						break;
					case IEnumerable<short> s:
						iw.Append(s.SequenceToString());
						break;
					case IEnumerable<byte> b:
						iw.Append(b.SequenceToString());
						break;
					case IEnumerable<bool> b:
						iw.Append(b.SequenceToString());
						break;
					case CharArray ca:
						iw.Append(ca);
						break;
					case IEnumerable<Vector2> v:
						iw.Append(v.SequenceToString());
						break;
					case IEnumerable<Vector3> v:
						iw.Append(v.SequenceToString());
						break;
					case IEnumerable<Vector4> v:
						iw.Append(v.SequenceToString());
						break;
					case IEnumerable<Ehandle> e:
						iw.Append(e.SequenceToString());
						break;
					case IEnumerable<Tick> t:
						iw.Append(t.SequenceToString());
						break;
					case IEnumerable<Time> t:
						iw.Append(t.SequenceToString());
						break;
					case IEnumerable<ModelName> t:
						iw.Append(t.SequenceToString());
						break;
					case IEnumerable<SoundName> t:
						iw.Append(t.SequenceToString());
						break;
					case IEnumerable<ModelIndex> t:
						iw.Append(t.SequenceToString());
						break;
					case IEnumerable<MaterialIndex> t:
						iw.Append(t.SequenceToString());
						break;
					case IEnumerable<Color32> c:
						iw.Append(c.SequenceToString());
						break;
					case IEnumerable<Interval> i:
						iw.Append(i.SequenceToString());
						break;
					default:
						iw.Append($"UNIMPLEMENTED TYPE FOR TOSTRING(): {Field!.GetType()}");
						break;
				}
			}
		}
	}
}