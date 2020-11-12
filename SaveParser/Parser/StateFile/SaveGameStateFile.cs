using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Parser.StateFile.SaveStateData;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.StateFile {
	
	public class SaveGameStateFile : EmbeddedStateFile {
		
		public int Version;
		public UtilVector<ParsedDataMap> BlockHeadersInfo;
		public List<SaveGameHeader> SaveGameHeaders;
		public ParsedDataMap SaveHeader;
		public ParsedDataMap[]? AdjacencyList, LightStyleList;
		public List<SaveStateBlock> Blocks;


		public SaveGameStateFile(SourceSave saveRef, CharArray name) : base(saveRef, name) {}


		protected override void Parse(ref BitStreamReader bsr) {
			base.Parse(ref bsr);
			Version = bsr.ReadSInt();
			int nBytesSymbols = bsr.ReadSInt();
			int nSymbols = bsr.ReadSInt();
			int nBytesDataHeaders = bsr.ReadSInt();
			int nBytesData = bsr.ReadSInt();
			SaveInfo.ParseContext.CurrentSymbolTable = bsr.ReadSymbolTable(nSymbols, nBytesSymbols)!;
			
			int @base = bsr.CurrentByteIndex;
			
			int sizeHeaders = bsr.ReadSInt();
			int sizeBodies = bsr.ReadSInt();

			BlockHeadersInfo = UtilVector<ParsedDataMap>.RestoreEmbedded("BlockHeadersInfo", "SaveRestoreBlockHeader_t", SaveInfo, ref bsr);

			SaveGameHeaders = new List<SaveGameHeader>(BlockHeadersInfo.Count);
			foreach (ParsedDataMap headerInfo in BlockHeadersInfo) {
				int loc = headerInfo.GetField<int>("locHeader");
				if (loc == -1)
					continue;
				bsr.CurrentByteIndex = @base + loc;
				SaveGameHeader? sgh = SaveGameHeader.CreateForCategory(SaveRef, headerInfo);
				if (sgh == null)
					continue;
				sgh.ParseStream(ref bsr);
				SaveGameHeaders.Add(sgh);
			}

			//@base = bsr.CurrentByteIndex = @base + sizeHeaders;
			BitStreamReader entBaseBsr = bsr.Split(); // entity data is offset from this location

			SaveHeader = bsr.ReadDataMap("Save Header", SaveInfo);
			
			// todo something about this?
			SaveInfo.BaseTime = SaveHeader.GetFieldOrDefault<Time>("time");
			//SaveInfo.LandmarkPos // todo gotten from adjacency list?
			
			int connections = SaveHeader.GetFieldOrDefault<int>("connectionCount"); // to other maps?
			int lightStyles = SaveHeader.GetFieldOrDefault<int>("lightStyleCount");
			
			if (connections > 0) {
				AdjacencyList = new ParsedDataMap[connections];
				for (int i = 0; i < connections; i++)
					AdjacencyList[i] = bsr.ReadDataMap("ADJACENCY", SaveInfo);
			}
			if (lightStyles > 0) {
				LightStyleList = new ParsedDataMap[lightStyles];
				for (int i = 0; i < lightStyles; i++)
					LightStyleList[i] = bsr.ReadDataMap("LIGHTSTYLE", SaveInfo);
			}

			@base = bsr.CurrentByteIndex;
			
			Blocks = new List<SaveStateBlock>(SaveGameHeaders.Count);

			foreach (SaveGameHeader header in SaveGameHeaders) {
				if (header.DataHeader.GetFieldOrDefault<int>("locBody") == -1)
					continue;
				bsr.CurrentByteIndex = @base;
				if (!SaveStateBlock.CreateFromHeader(SaveRef!, header, out SaveStateBlock blockHandler)) {
					SaveInfo.AddError($"{nameof(SaveStateBlock)} not created from header: \"{header.Category}\"");
					continue;
				}
				blockHandler.ParseStream(ref entBaseBsr);
				Blocks.Add(blockHandler);
			}
			
			bsr.CurrentByteIndex = @base + sizeBodies;
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append(Name);
			iw.FutureIndent++;
			iw.AppendLine($"\nversion: {Version}");
			iw.AppendLine($"ID: {Id}");
			BlockHeadersInfo.AppendToWriter(iw);
			iw.Append($"\n{SaveGameHeaders.Count} headers:");
			EnumerableAppendHelper(SaveGameHeaders, iw, false);
			SaveHeader.AppendToWriter(iw);
			iw.AppendLine();
			if (AdjacencyList != null) {
				iw.Append("adjacents: ");
				EnumerableAppendHelper(AdjacencyList, iw, false);
			}
			if (LightStyleList != null) {
				iw.Append("light styles: ");
				EnumerableAppendHelper(LightStyleList, iw, false);
			}
			iw.Append($"{Blocks.Count} blocks:");
			EnumerableAppendHelper(Blocks, iw); // add last field bool
			iw.FutureIndent--;
		}
	}
}