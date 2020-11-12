using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class ThinkContexts : ParsedSaveField {

		public (ParsedDataMap map, Func? func)[] Contexts;
		public override object FieldAsObj => Contexts;


		private ThinkContexts(TypeDesc desc) : base(desc) {}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{Contexts.Length} think contexts:");
			iw.FutureIndent++;
			foreach ((ParsedDataMap? m, Func? f) in Contexts) {
				iw.AppendLine();
				iw.AppendLine(f.HasValue ? $"think function: {f.Value}" : "no think function");
				m.AppendToWriter(iw);
			}
			iw.FutureIndent--;
		}


		public static ThinkContexts Restore(TypeDesc typeDesc, SaveInfo info, ref BitStreamReader bsr) {
			var pUtlVector = UtilVector<ParsedDataMap>.RestoreEmbedded("pUtlVector", "thinkfunc_t", info, ref bsr);
			
			(ParsedDataMap map, Func? func)[] contexts = new (ParsedDataMap, Func?)[pUtlVector.Count];
			
			bsr.StartBlock(info);
			// get the owner of this entity
			for (int i = 0; i < contexts.Length; i++) {
				contexts[i].map = pUtlVector[i];
				bool hasFunc = bsr.ReadByte() != 0;
				if (hasFunc) {
					bsr.StartBlock(info, out short size);
					contexts[i].func = (Func)bsr.ReadStringOfLength(size);
					bsr.EndBlock(info);
				}
			}
			bsr.EndBlock(info);
			
			return new ThinkContexts(typeDesc) {Contexts = contexts};
		}
	}
}