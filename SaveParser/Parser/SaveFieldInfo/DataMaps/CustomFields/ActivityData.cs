using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
		
	public class ActivityData : ParsedSaveField {

		private const uint ActivityFileTag = 0x80800000;
		public int? Index;
		public string? Name;
			
		public ActivityData(TypeDesc desc) : base(desc) {}
			

		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append("CActivityData:");
			iw.FutureIndent++;
			iw.AppendLine();
			iw.Append(Index.HasValue ? $"activity index: {Index.Value}" : $"name: {Name!}");
			iw.FutureIndent--;
		}


		internal static ActivityData Restore(TypeDesc typeDesc, SaveInfo info, ref BitStreamReader bsr) {
			uint len = bsr.ReadUInt();
			return (len & 0xFFFF0000) != ActivityFileTag
				? new ActivityData(typeDesc) {Index = (int)len}
				: new ActivityData(typeDesc) {Name = bsr.ReadStringOfLength(len & 0xFFFF)};
		}
	}
}