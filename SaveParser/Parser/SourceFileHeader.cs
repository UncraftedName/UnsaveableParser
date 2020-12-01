using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser {
	
	public class SourceFileHeader : SaveComponent {

		public string IdString = null!; // "JSAV"
		public int SaveVersion; // tag - 0x0073
		public int TokenTableFileTableOffset; // size
		public int TokenCount;
		public int TokenTableSize; // token size
		
		public SourceFileHeader(SourceSave saveRef) : base(saveRef) {}


		protected override void Parse(ref ByteStreamReader bsr) {
			IdString = bsr.ReadStringOfLength(4);
			SaveVersion = bsr.ReadSInt();
			TokenTableFileTableOffset = bsr.ReadSInt();
			TokenCount = bsr.ReadSInt();
			TokenTableSize = bsr.ReadSInt();
			// offset += table size?
		}
	}
}