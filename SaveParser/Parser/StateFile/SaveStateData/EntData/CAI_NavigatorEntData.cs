using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile.SaveStateData.EntData {
	
	public class CAI_NavigatorEntData : ParsedEntData {

		public short Version;
		public UtilVector<ParsedDataMap> MinPathArray;
		
		
		public CAI_NavigatorEntData(SourceSave saveRef)
			: base(saveRef, null!, saveRef.SaveInfo.SDataMapLookup["CAI_Navigator"]) {}


		protected override void Parse(ref ByteStreamReader bsr) {
			Version = bsr.ReadSShort();
			MinPathArray = UtilVector<ParsedDataMap>.RestoreEmbedded("minPathName", "AI_WayPoint_t", SaveInfo, ref bsr);
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{ClassMap.Name} (version {Version}):");
			iw.FutureIndent++;
			iw.AppendLine();
			MinPathArray.AppendToWriter(iw);
			iw.FutureIndent--;
		}
	}
}