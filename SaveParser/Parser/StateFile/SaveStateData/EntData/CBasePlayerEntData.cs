using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.StateFile.SaveStateData.EntData {
	
	public class CBasePlayerEntData : CBaseEntityParsedEntData { // todo any other classes along the way that do stuff?
		
		public CBasePlayerEntData(SourceSave saveRef, ParsedDataMap headerInfo, DataMap classMap)
			: base(saveRef, headerInfo, classMap) {}


		protected override void Parse(ref BitStreamReader bsr) {
			base.Parse(ref bsr);
			// todo if not landmark, set local origin to spawn
			//m_angRotation = p1.vangle;
			
			// if get flags & ducking, ,m_Local.m_bDucked = try, else false
		}
	}
}