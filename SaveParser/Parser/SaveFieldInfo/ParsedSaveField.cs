using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;

namespace SaveParser.Parser.SaveFieldInfo {


	public abstract class ParsedSaveField : AppendableClass, IEquatable<ParsedSaveField> {
		
		protected static readonly Exception InvalidCastE = new InvalidCastException($"{typeof(ParsedSaveField)}: bad cast");

		public readonly TypeDesc Desc;
		public int ByteIndex {get; private set;}


		protected ParsedSaveField(TypeDesc desc) {
			Desc = desc;
			ByteIndex = -1;
		}


		internal void SetIndex(int index) {
			ByteIndex = index;
		}
		
		
		public abstract bool Equals(ParsedSaveField? other);


#pragma warning disable 659
		public override bool Equals(object? obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((ParsedSaveField)obj);
		}
#pragma warning restore 659
	}


	public class ParsedSaveField<T> : ParsedSaveField {
		
		public T Field;
		public static implicit operator T(ParsedSaveField<T> p) => p.Field;
		public readonly int ElemCount;


		public ParsedSaveField(T field, TypeDesc desc, int elemCount = 1) : base(desc) {
			Field = field;
			ElemCount = elemCount;
		}
		
		
		public override bool Equals(ParsedSaveField? other) { // todo test
			if (other == null || !(other is ParsedSaveField<T> otherParsedField))
				return false;
			if (Field == null)
				return otherParsedField.Field == null;
			else if (otherParsedField.Field == null)
				return false;
			if (ElemCount != otherParsedField.ElemCount || !Equals(Desc, otherParsedField.Desc))
				return false;
			if (ElemCount == 1) {
				return Field.Equals(otherParsedField.Field);
			} else {
				var a = ((IEnumerable)Field).GetEnumerator();
				var b = ((IEnumerable)otherParsedField.Field).GetEnumerator();
				while (a.MoveNext())
					if (!(b.MoveNext() && Equals(a.Current, b.Current)))
						return false;
				return !b.MoveNext();
			}
		}


		private static bool IsOverride(MethodInfo m) {
			return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
		}
		
		private static readonly HashSet<Type> _validEnumerableTypes = new HashSet<Type>();
		
		
		// a wee bit silly
		public override void AppendToWriter(IIndentedWriter iw) {
			if (Field == null) {
				iw.Append("null");
				return;
			}
			switch (Desc.FieldType) {
				case FieldType.EMBEDDED:
					iw.Append($"{Desc.EmbeddedMap!.Name}");
					if (ElemCount == 1) {
						if (!(Field is ParsedDataMap emMap))
							throw new Exception($"Unexpected type for embedded field: {Field!.GetType()}");
						iw.Append($" {Desc.Name}:");
						EnumerableAppendHelper(emMap.ParsedFields.Values, iw);
					} else {
						iw.Append($"[{ElemCount}] {Desc.Name}:");
						EnumerableAppendHelper(Field as IEnumerable<ParsedDataMap>, iw);
					}
					return;
				case FieldType.CUSTOM:
					iw.Append($"{Desc}: "); //todo custom spacing doesn't always work
					break;
				default:
					iw.Append($"{Desc.TypeString.PadRight(12)} {Desc.Name}: ");
					break;
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