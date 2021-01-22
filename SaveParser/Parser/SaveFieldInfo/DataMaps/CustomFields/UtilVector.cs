using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class UtilVector<T> : ParsedSaveField, IReadOnlyList<T> {

		public readonly T[] Array;
		public readonly TypeDesc ElemDesc;


		private UtilVector(T[] array, TypeDesc elemDesc, TypeDesc desc) : base(desc) {
			Array = array;
			ElemDesc = elemDesc;
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append("UtilVector<");
			iw.Append(ElemDesc.FieldType == FieldType.EMBEDDED ? ElemDesc.EmbeddedMap!.ClassName : ElemDesc.TypeString);
			iw.Append($">[{Array.Length}] ");
			iw.Append($"{Desc.Name}");
			if (Array.Length > 0) {
				iw.Append(": ");
				if (Array is IPretty[] ia)
					EnumerablePrettyWriteHelper(ia, iw);
				else
					iw.Append(Array.SequenceToString());
			}
		}
		
		/* This pretty much exactly what the game does. A datamap is created dynamically which contains only a single
		 * description for each element of this vector. This solution means I only have to do a single cast.
		 * Custom params:
		 * [0] - the field type of each element
		 * [1] - the custom read function of each element (if applicable)
		 * [2] - the embedded map of each element (if applicable)
		 * */
		public static UtilVector<T> Restore(TypeDesc vecDesc, SaveInfo info, ref ByteStreamReader bsr) {
			
			object[] @params = vecDesc.CustomParams!;
			
			DataMap? embMap = @params[2] is string s ? info.SDataMapLookup[s] : null;

			int count = bsr.ReadSInt();
			
			TypeDesc elemDesc = new TypeDesc(
				name: "elems",
				flags: DescFlags.FTYPEDESC_SAVE,
				fieldType: (FieldType)@params[0],
				customReadFunc: (CustomReadFunc?)@params[1],
				numElements: (ushort)(embMap == null ? count : 1))
			{
				EmbeddedMap = embMap
			};
			
			// Sometimes the count is one but the read result does not have an element. I'm not sure if this is
			// intended or not but by default that will cause an exception.
			
			DataMap vecMap = new DataMap(embMap == null ? "elems" : "uv", new[] {elemDesc});
			T[] res;
			if (embMap == null && count > 1) {
				ParsedDataMap mapReadResult = bsr.ReadDataMap(vecMap, info);
				res = (ParsedSaveField<T[]>)mapReadResult.ParsedFields.Single().Value;
			} else { // if the field type is embedded then the elements are read one by one
				res = new T[count];
				for (int i = 0; i < count; i++) {
					ParsedDataMap mapReadResult = bsr.ReadDataMap(vecMap, info);
					res[i] = (ParsedSaveField<T>)mapReadResult.ParsedFields.Single().Value;
				}
			}
			return new UtilVector<T>(res, elemDesc, vecDesc);
		}


		// called like "UtilVector<ParsedDataMap>.RestoreEmbedded"
		public static UtilVector<ParsedDataMap> RestoreEmbedded(string vecName, string elemMapName, SaveInfo info, ref ByteStreamReader bsr) {
			object?[] customParams = {FieldType.EMBEDDED, null, elemMapName};
			TypeDesc vecDesc = new TypeDesc(vecName, DescFlags.FTYPEDESC_SAVE, Restore, customParams);
			return UtilVector<ParsedDataMap>.Restore(vecDesc, info, ref bsr);
		}
		
		
		public override bool Equals(ParsedSaveField? other) {
			if (other == null || !(other is UtilVector<T> otherVec))
				return false;
			return Equals(Desc, otherVec.Desc) && Array.SequenceEqual(otherVec.Array);
		}


		public IEnumerator<T> GetEnumerator() {
			return ((IEnumerable<T>)Array).GetEnumerator();
		}


		IEnumerator IEnumerable.GetEnumerator() {
			return Array.GetEnumerator();
		}


		public int Count => Array.Length;
		public T this[int index] => Array[index];
	}
}