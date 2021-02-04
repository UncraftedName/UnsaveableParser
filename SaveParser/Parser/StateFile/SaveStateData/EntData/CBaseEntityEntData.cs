using System.Numerics;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile.SaveStateData.EntData {
	
	public class CBaseEntityParsedEntData : ParsedEntData {
		
		public CBaseEntityParsedEntData(SourceSave saveRef, ParsedDataMap headerInfo, DataMap classMap)
			: base(saveRef, headerInfo, classMap) {}


		protected override void Parse(ref ByteStreamReader bsr) {
			base.Parse(ref bsr);
			if (ParsedFields != null) {
				Vector3 parentSpaceOffset = default; // todo modelSpaceOffset
				if (!ParsedFields.ParsedFields.ContainsKey("m_pParent"))
					parentSpaceOffset += SaveInfo.LandmarkPos; // parent is the world
				var origin = ParsedFields.GetFieldOrDefault<Vector3>("m_vecAbsOrigin");
				Matrix3X4? coordFrame = ParsedFields.GetFieldOrDefault<Matrix3X4>("m_rgflCoordinateFrame");
				coordFrame?.SetColumn(3, in origin.Field);
				origin.Field += parentSpaceOffset;
			}
		}
	}
}