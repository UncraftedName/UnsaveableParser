using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class ConceptHistories : ParsedSaveField {

		public readonly (string conceptName, ParsedDataMap history, ParsedDataMap? response)[] Histories;


		public ConceptHistories(
			TypeDesc desc,
			(string conceptName, ParsedDataMap history, ParsedDataMap? response)[] histories)
			: base(desc)
		{
			Histories = histories;
		}


		public override bool Equals(ParsedSaveField? other) {
			if (other == null || !(other is ConceptHistories otherHistories))
				return false;
			return Equals(Histories, otherHistories.Histories); // todo check if value tuple does this right
		}
		
		
		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{Histories.Length} concept histories:");
			iw.FutureIndent++;
			foreach ((string conceptName, ParsedDataMap history, ParsedDataMap? response) in Histories) {
				iw.Append($"\n{conceptName}:");
				iw.FutureIndent++;
				iw.AppendLine();
				history.AppendToWriter(iw);
				iw.AppendLine();
				if (response == null)
					iw.Append("no response");
				else
					response.AppendToWriter(iw);
				iw.FutureIndent--;
			}
			iw.FutureIndent--;
		}


		public static ConceptHistories Restore(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr) {
			int count = bsr.ReadSInt();
			var histories = new (string conceptName, ParsedDataMap history, ParsedDataMap? response)[count];
			for (int i = 0; i < count; i++) {
				bsr.StartBlock(info);
				histories[i].conceptName = bsr.ReadNullTerminatedString();
				histories[i].history = bsr.ReadDataMap("ConceptHistory_t", info);
				if (bsr.ReadByte() != 0)
					histories[i].response = bsr.ReadDataMap("AI_Response", info);
				bsr.EndBlock(info);
			}
			return new ConceptHistories(typeDesc, histories);
		}
	}
}