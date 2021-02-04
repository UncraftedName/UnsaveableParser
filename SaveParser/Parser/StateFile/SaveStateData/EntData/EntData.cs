using System;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile.SaveStateData.EntData {

	// most entities are restored by just reading their map
	public class ParsedEntData : SaveComponent {
		
		public readonly ParsedDataMap HeaderInfo;
		public readonly DataMap ClassMap;
		public ParsedDataMap? ParsedFields;
		// the name of this ent in the ent header section, probably hammer name. e.g. "worldspawn" or "player"
		public string HeaderProxyName => HeaderInfo.GetField<string>("classname");
		
		
		public ParsedEntData(SourceSave saveRef, ParsedDataMap headerInfo, DataMap classMap) : base(saveRef) {
			HeaderInfo = headerInfo;
			ClassMap = classMap;
		}
		
		
		protected override void Parse(ref ByteStreamReader bsr) {
			try {
				ParsedFields = bsr.ReadDataMapRecursive(ClassMap, SaveInfo);
			} catch (Exception e) {
				SaveInfo.AddError($"exception while parsing map \"{ClassMap.ClassName}\": {e.Message}");
			}
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			PrettyWrite(iw, true);
		}


		internal void PrettyWrite(IPrettyWriter iw, bool showClassName) {
			if (ParsedFields == null) {
				iw.Append($"parsing fields failed for entity class: {ClassMap}");
			} else {
				if (showClassName)
					iw.Append($"({HeaderProxyName}) ");
				ParsedFields.PrettyWrite(iw);
			}
		}
	}


	public static class EntDataFactory {
		public static ParsedEntData CreateFromName(SourceSave saveRef, ParsedDataMap headerInfo, DataMap entMap) {
			// order of these checks matters
			if (entMap.InheritsFrom("CBasePlayer"))
				return new CBasePlayerEntData(saveRef, headerInfo, entMap);
			else if (entMap.InheritsFrom("CAI_BaseNPC"))
				return new CAI_BaseNpcEntData(saveRef, headerInfo, entMap);
			else if (entMap.InheritsFrom("CBaseEntity"))
				return new CBaseEntityParsedEntData(saveRef, headerInfo, entMap);
			else
				return new ParsedEntData(saveRef, headerInfo, entMap);
		}
	}
}