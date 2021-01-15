using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
		
	public class ActivityData : ParsedSaveField {

		private const uint ActivityFileTag = 0x80800000;
		public readonly int? Index;
		public readonly string? Name;
		

		public ActivityData(TypeDesc desc, int index) : base(desc) {
			Index = index;
		}


		public ActivityData(TypeDesc desc, string name) : base(desc) {
			Name = name;
		}


		public override bool Equals(ParsedSaveField? other) {
			if (other == null || !(other is ActivityData otherActivity))
				return false;
			return Index == otherActivity.Index && Equals(Name, otherActivity.Name);
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append("CActivityData:");
			iw.FutureIndent++;
			iw.AppendLine();
			iw.Append(Index.HasValue ? $"activity index: {Index.Value}" : $"name: {Name!}");
			iw.FutureIndent--;
		}


		internal static ActivityData Restore(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr) {
			uint len = bsr.ReadUInt();
			return (len & 0xFFFF0000) != ActivityFileTag
				? new ActivityData(typeDesc, (int)len)
				: new ActivityData(typeDesc, bsr.ReadStringOfLength(len & 0xFFFF));
		}
	}
}