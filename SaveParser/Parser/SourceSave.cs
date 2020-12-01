#pragma warning disable 8618
using System.Diagnostics;
using System.IO;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.StateFile;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser {
	
	public class SourceSave : SaveComponent {
		
		private ByteStreamReader _privateReader;
		public override ByteStreamReader Reader => _privateReader.FromBeginning();

		public new readonly SaveInfo SaveInfo;
		public SourceFileHeader SourceFileHeader;
		public ParsedDataMap GameHeader, Globals;
		public EmbeddedStateFile[] StateFiles;


		// i don't have a good way of getting the game from the file itself, so that will have to be passed in manually
		public SourceSave(byte[] bytes, Game game) : base(null) {
			_privateReader = new ByteStreamReader(bytes);
			SaveInfo = new SaveInfo(game);
		}


		public SourceSave(string dir, Game game) : this(File.ReadAllBytes(dir), game) {
			SaveInfo.SaveDir = dir;
		}


		internal ByteStreamReader ReaderFromOffset(int offset, int size) {
			return new ByteStreamReader((byte[])_privateReader.Data, size, offset);
		}


		protected override void Parse(ref ByteStreamReader bsr) {
			SourceFileHeader = new SourceFileHeader(this);
			SourceFileHeader.ParseStream(ref bsr);
			SaveInfo.ParseContext.CurrentSymbolTable = bsr.ReadSymbolTable(SourceFileHeader.TokenCount, SourceFileHeader.TokenTableSize)!;
			GameHeader = bsr.ReadDataMap("GameHeader", SaveInfo);
			Globals = bsr.ReadDataMap("GLOBAL", SaveInfo);
			StateFiles = new EmbeddedStateFile[bsr.ReadSInt()];
			
			for (int i = 0; i < StateFiles.Length; i++) {
				StateFiles[i] = EmbeddedStateFile.CreateFromName(this, bsr.ReadCharArray(260));
				int fileLength = bsr.ReadSInt();
				StateFiles[i].ParseStream(bsr.SplitAndSkip(fileLength));
			}

			SaveInfo.Cleanup();
			Debug.Assert(bsr.BytesRemaining == 0);
		}
		
		
		public void Parse() {
			_privateReader.CurrentByteIndex = 0;
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