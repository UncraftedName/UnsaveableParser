using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	// a collection of I/O fire events
	public class EventsSave : ParsedSaveField, IReadOnlyList<ParsedDataMap> {

		public ParsedSaveField? Value;
		public ParsedDataMap[]? Events;
		
		
		public EventsSave(TypeDesc desc) : base(desc) {}
		
		
		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{Events!.Length} {Desc.Name} output");
			if (Events.Length != 1)
				iw.Append("s");
			if (Value != null || Events.Length > 0) {
				iw.Append(":");
				if (Value != null) {
					iw.AppendLine();
					Value.AppendToWriter(iw);
				}
				if (Events.Length > 0)
					EnumerableAppendHelper(Events!, iw);
			}
		}


		public static EventsSave Restore(TypeDesc typeDesc, SaveInfo info, ref BitStreamReader bsr) {
			
			int count = bsr.ReadSInt();
			
			// inline version of reading embedded field of type CBaseEntityOutput (it only contains 1 field)
			
			if (bsr.ReadSShort() != 4)
				throw new ConstraintException("first entry in data map should be 4");
			string? mapSym = bsr.ReadSymbol(info);
			if (mapSym != "Value")
				throw new ConstraintException($"bad symbol, expected \"Value\" but read \"{mapSym}\"");
			int fieldsSaved = bsr.ReadSInt();
			ParsedSaveField? psf = null;
			if (fieldsSaved == 1) {
				bsr.StartBlock(info, out string? sym);
				if (sym != "m_Value")
					throw new ConstraintException($"bad symbol, expected \"m_Value\" but read \"{sym}\"");
				FieldType type = (FieldType)bsr.ReadSInt();
				string? s = FieldNameFromType(type);
				if (s != null) {
					TypeDesc t = new TypeDesc(s, type);
					DataMap m = new DataMap("m_Value", new [] {t});
					var pm = bsr.ReadDataMap(m, info);
					if (pm.FieldsDict.Any())
						psf = pm.FieldsDict.Single().Value;
				}
				bsr.EndBlock(info);
			} else if (fieldsSaved != 0) {
				throw new ConstraintException($"expected 0 fields, got {fieldsSaved}");
			}
			
			ParsedDataMap[] events = new ParsedDataMap[count];
			for (int i = 0; i < count; i++)
				events[i] = bsr.ReadDataMap("EntityOutput", info);

			return new EventsSave(typeDesc) {Value = psf, Events = events};
		}


		private static string? FieldNameFromType(FieldType type) {
			return type switch {
				FLOAT           => "flVal",
				INTEGER         => "iVal",
				COLOR32         => "rgbaVal",
				EHANDLE         => "eVal",
				STRING          => "iszVal",
				BOOLEAN         => "bVal",
				VECTOR          => "vecSave",
				POSITION_VECTOR => "vecSave",
				VOID            => null,
				_ => throw new ArgumentException($"bad field type while parsing {nameof(EventsSave)}")
			};
		}


		public IEnumerator<ParsedDataMap> GetEnumerator() {
			return ((IEnumerable<ParsedDataMap>)Events!).GetEnumerator();
		}


		IEnumerator IEnumerable.GetEnumerator() {
			return Events!.GetEnumerator();
		}


		public int Count => Events!.Length;

		public ParsedDataMap this[int index] => Events![index];
	}
}