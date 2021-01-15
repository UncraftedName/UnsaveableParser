using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class UtilMap<TK, TV> : ParsedSaveField,
		IReadOnlyList<KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>>,
		IEnumerable<KeyValuePair<TK, TV>>
	{
		
		public readonly TypeDesc KeyDesc, ValDesc;
		public readonly KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>[] Elements;


		public UtilMap(
			TypeDesc desc,
			KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>[] elements,
			TypeDesc keyDesc,
			TypeDesc valDesc)
			: base(desc)
		{
			KeyDesc = keyDesc;
			ValDesc = valDesc;
			Elements = elements;
		}


		public override bool Equals(ParsedSaveField? other) {
			if (other == null || !(other is UtilMap<TK, TV> otherUtlMap))
				return false;
			return Equals(KeyDesc, otherUtlMap.KeyDesc) &&
				   Equals(ValDesc, otherUtlMap.ValDesc) &&
				   Elements.SequenceEqual(otherUtlMap.Elements);
		}
		
		
		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append("UtilMap<");
			iw.Append(KeyDesc.FieldType == EMBEDDED ? KeyDesc.EmbeddedMap!.Name : KeyDesc.TypeString);
			iw.Append(",");
			iw.Append(ValDesc.FieldType == EMBEDDED ? ValDesc.EmbeddedMap!.Name : ValDesc.TypeString);
			iw.Append($">[{Elements.Length}] {Desc.Name}" + (Elements.Length > 0 ? ":" : ""));
			if (Elements.Length > 0) {
				iw.FutureIndent++;
				for (int i = 0; i < Elements.Length; i++) {
					var (key, value) = Elements[i];
					iw.Append("\n{");
					key.PrettyWriteWithCustomPad(iw);
					iw.Append(",");
					if (ValDesc.FieldType == EMBEDDED)
						iw.AppendLine();
					value.PrettyWriteWithCustomPad(iw);
					iw.Append("}");
					if (i < Elements.Length - 1)
						iw.Append(",");
				}
				iw.FutureIndent--;
			}
		}


		/* This pretty much exactly what the game does. A datamap is created dynamically which contains a description
		 * for each key/value of the map.
		 * Custom params:
		 * [0] - the field type of each key
		 * [1] - the custom read function of each key (if applicable)
		 * [2] - the embedded map of each key (if applicable)
		 * [3] - the field type of each value
		 * [4] - the custom read function of each value (if applicable)
		 * [5] - the embedded map of each value (if applicable)
		 * */
		public static UtilMap<TK, TV> Restore(TypeDesc mapDesc, SaveInfo info, ref ByteStreamReader bsr) {
			object[] @params = mapDesc.CustomParams!;
			DataMap? embKeyMap = @params[2] is string sk ? info.SDataMapLookup[sk] : null;
			DataMap? embValMap = @params[5] is string sv ? info.SDataMapLookup[sv] : null;
			
			TypeDesc keyDesc = new TypeDesc(
				name: "K",
				flags: DescFlags.FTYPEDESC_SAVE,
				fieldType: (FieldType)@params[0],
				customReadFunc: (CustomReadFunc?)@params[1],
				numElements: 1)
			{
				EmbeddedMap = embKeyMap
			};
			TypeDesc valDesc = new TypeDesc(
				name: "T", // std::map<Key, T>
				flags: DescFlags.FTYPEDESC_SAVE,
				fieldType: (FieldType)@params[3],
				customReadFunc: (CustomReadFunc?)@params[4],
				numElements: 1)
			{
				EmbeddedMap = embValMap
			};
			DataMap vecMap = new DataMap("um", new[] {keyDesc, valDesc});
			
			bsr.StartBlock(info);
			int count = bsr.ReadSInt();
			var res = new KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>[count];
			for (int i = 0; i < count; i++) {
				ParsedDataMap readResult = bsr.ReadDataMap(vecMap, info);
				var k = (ParsedSaveField<TK>)readResult.ParsedFields["K"];
				var v = (ParsedSaveField<TV>)readResult.ParsedFields["T"];
				res[i] = new KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>(k, v);
			}
			bsr.EndBlock(info);
			return new UtilMap<TK, TV>(mapDesc, res, keyDesc, valDesc);
		}

		// this was confusing but convenient for UtilVector, but here it's really stupid
		
		public static UtilMap<ParsedDataMap, TV> RestoreEmbeddedKey(
			string utlMapName,
			SaveInfo info,
			ref ByteStreamReader bsr,
			string? keyMapName,
			FieldType valType,
			CustomReadFunc? valReadFunc = null)
		{
			object?[] customParams = {EMBEDDED, null, keyMapName, valType, valReadFunc, null};
			TypeDesc utlMapDesc = new TypeDesc(utlMapName, DescFlags.FTYPEDESC_SAVE, Restore, customParams);
			return UtilMap<ParsedDataMap, TV>.Restore(utlMapDesc, info, ref bsr);
		}


		public static UtilMap<TK, ParsedDataMap> RestoreEmbeddedValue(
			string utlMapName,
			SaveInfo info,
			ref ByteStreamReader bsr,
			string? valMapName,
			FieldType keyType,
			CustomReadFunc? keyReadFunc = null)
		{
			object?[] customParams = {keyType, keyReadFunc, null, EMBEDDED, null, valMapName};
			TypeDesc utlMapDesc = new TypeDesc(utlMapName, DescFlags.FTYPEDESC_SAVE, Restore, customParams);
			return UtilMap<TK, ParsedDataMap>.Restore(utlMapDesc, info, ref bsr);
		}


		public static UtilMap<ParsedDataMap, ParsedDataMap> RestoreEmbedded(
			string utlMapName,
			SaveInfo info,
			ref ByteStreamReader bsr,
			string? keyMapName,
			string? valMapName)
		{
			object?[] customParams = {EMBEDDED, null, keyMapName, EMBEDDED, null, valMapName};
			TypeDesc utlMapDesc = new TypeDesc(utlMapName, DescFlags.FTYPEDESC_SAVE, Restore, customParams);
			return UtilMap<ParsedDataMap, ParsedDataMap>.Restore(utlMapDesc, info, ref bsr);
		}
		
		
		public IEnumerable<TK> Keys => Elements.Select(pair => pair.Key.Field);

		public IEnumerable<TV> Values => Elements.Select(pair => pair.Value.Field);
		
		public int Count => Elements.Length;

		IEnumerator<KeyValuePair<TK, TV>> IEnumerable<KeyValuePair<TK, TV>>.GetEnumerator() {
			return Elements.Select(pair => new KeyValuePair<TK, TV>(pair.Key.Field, pair.Value.Field)).GetEnumerator();
		}
		
		public IEnumerator<KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>> GetEnumerator() {
			return ((IEnumerable<KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>>>)Elements).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return Elements.GetEnumerator();
		}

		public KeyValuePair<ParsedSaveField<TK>, ParsedSaveField<TV>> this[int index] => Elements[index];
	}
}