#pragma warning disable 8618
using System.Diagnostics;
using System.IO;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.StateFile;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser {
	
	public class SourceSave : SaveComponent {
		
		private BitStreamReader _privateReader;
		public override BitStreamReader Reader => _privateReader.FromBeginning();

		public new readonly SaveInfo SaveInfo;
		public SourceFileHeader SourceFileHeader;
		public ParsedDataMap GameHeader, Globals;
		public EmbeddedStateFile[] StateFiles;


		// i don't have a good way of getting the game from the file itself, so that will have to be passed in manually
		public SourceSave(byte[] bytes, Game game) : base(null) {
			_privateReader = new BitStreamReader(bytes);
			SaveInfo = new SaveInfo(game);
		}


		public SourceSave(string dir, Game game) : this(File.ReadAllBytes(dir), game) {
			SaveInfo.SaveDir = dir;
		}


		internal BitStreamReader ReaderFromOffset(int offset, int bitLength) {
			return new BitStreamReader(_privateReader.Data, bitLength, offset);
		}


		protected override void Parse(ref BitStreamReader bsr) {
			SourceFileHeader = new SourceFileHeader(this);
			SourceFileHeader.ParseStream(ref bsr);
			SaveInfo.ParseContext.CurrentSymbolTable = bsr.ReadSymbolTable(SourceFileHeader.TokenCount, SourceFileHeader.TokenTableSize)!;
			GameHeader = bsr.ReadDataMap("GameHeader", SaveInfo);
			Globals = bsr.ReadDataMap("GLOBAL", SaveInfo);
			StateFiles = new EmbeddedStateFile[bsr.ReadSInt()];
			
			for (int i = 0; i < StateFiles.Length; i++) {
				StateFiles[i] = EmbeddedStateFile.CreateFromName(this, bsr.ReadCharArray(260));
				int fileLength = bsr.ReadSInt();
				StateFiles[i].ParseStream(bsr.SplitAndSkip(fileLength << 3));
			}

			SaveInfo.Cleanup();
			Debug.Assert(bsr.BitsRemaining == 0);
		}
		
		
		public void Parse() {
			_privateReader.CurrentBitIndex = 0;
			Parse(ref _privateReader);
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append("");
			SourceFileHeader.AppendToWriter(iw);
			iw.AppendLine();
			GameHeader.AppendToWriter(iw);
			iw.AppendLine();
			Globals.AppendToWriter(iw);
			foreach (EmbeddedStateFile stateFile in StateFiles) {
				iw.AppendLine();
				stateFile.AppendToWriter(iw);
			}
			iw.Append("\n\n\nErrors:\n");
			foreach (string errorStr in SaveInfo.Errors)
				iw.AppendLine(errorStr);
		}
	}
}