using System;
using System.Collections;
using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo {


	public abstract class ParsedSaveField : PrettyClass, IEquatable<ParsedSaveField> {

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
		
		
		public override void PrettyWrite(IPrettyWriter iw) => AppendWithCustomPad(iw, 9);


		// a wee bit excessive
		public void AppendWithCustomPad(IPrettyWriter iw, int padCount = 0) {
			if (Field == null) {
				iw.Append("null");
				return;
			}
			if (Desc.FieldType == EMBEDDED) {
				iw.Append($"{Desc.EmbeddedMap!.Name}");
				if (ElemCount == 1) {
					if (!(Field is ParsedDataMap emMap))
						throw new Exception($"Unexpected type for embedded field: {Field!.GetType()}");
					iw.Append($" {Desc.Name}:");
					EnumerablePrettyWriteHelper(emMap.ParsedFields.Values, iw);
				} else {
					iw.Append($"[{ElemCount}] {Desc.Name}:");
					EnumerablePrettyWriteHelper(Field as IEnumerable<ParsedDataMap>, iw);
				}
				return;
			}
			iw.Append($"{Desc.TypeString.PadRight(padCount)} {Desc.Name}: ");
			if (Desc.FieldType == CUSTOM) {
				iw.Append(ElemCount == 1
					? Field.ToString() :
					((IEnumerable)Field).SequenceToString());
				return;
			}
			if (ElemCount == 1) {
				if (Field is IPretty ap)
					ap.PrettyWrite(iw);
				else
					iw.Append(Field.ToString()!);
			} else {
				if (Field is CharArray chrArr)
					iw.Append(chrArr);
				else if (Field is IEnumerable ie2)
					iw.Append(ie2.SequenceToString());
				else
					throw new Exception($"field is type \"{Field.GetType()}\", cannot convert to string");
			}
		}
	}
}