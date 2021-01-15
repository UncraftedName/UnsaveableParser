using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.Structs;

namespace SaveParser.Parser.StateFile {
	
	public class ClientStateFile : EmbeddedStateFile {
		
		public ClientStateFile(SourceSave saveRef, CharArray name) : base(saveRef, name) {}


		// CSaveRestore::RestoreClientState
		// id = 'V' 'A' 'L' 'V', version = 0x73 = 115, section header = 2
		protected override unsafe void Parse(ref ByteStreamReader bsr) {
			base.Parse(ref bsr);
			var version = bsr.ReadSInt();
			var magic = bsr.ReadSInt();
			// if ( magicnumber == SECTION_MAGIC_NUMBER )
			var sectionHeaderVersion = bsr.ReadSInt();
			bsr.ReadStruct(out BaseClientSections sections, sizeof(BaseClientSections));
			//var symbolTable = bsr.ReadSymbolTable(sections.SymbolCount, sections.SymbolSize);
			var symbolTable = bsr.ReadNullSeparatedStrings(sections.SymbolCount);
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append(Name);
			iw.FutureIndent++;
			iw.Append($"\nmws: {Id}");
			iw.FutureIndent--;
		}
	}
}