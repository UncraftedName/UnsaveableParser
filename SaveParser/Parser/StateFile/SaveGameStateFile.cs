using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Parser.StateFile.SaveStateData;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile {
	
	public class SaveGameStateFile : EmbeddedStateFile {
		
		public int Version;
		public UtilVector<ParsedDataMap> BlockHeadersInfo;
		public List<SaveGameHeader> SaveGameHeaders;
		public ParsedDataMap SaveHeader;
		public ParsedDataMap[]? AdjacencyList, LightStyleList;
		public List<SaveStateBlock> Blocks;


		public SaveGameStateFile(SourceSave saveRef, CharArray name) : base(saveRef, name) {}


		protected override void Parse(ref ByteStreamReader bsr) {
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

			// info about the header block
			BlockHeadersInfo = UtilVector<ParsedDataMap>.RestoreEmbedded("BlockHeadersInfo", "SaveRestoreBlockHeader_t", SaveInfo, ref bsr);

			SaveGameHeaders = new List<SaveGameHeader>(BlockHeadersInfo.Count);
			// read the headers one by one
			foreach (ParsedDataMap headerInfo in BlockHeadersInfo) {
				int loc = headerInfo.GetField<int>("locHeader");
				if (loc == -1)
					continue;
				bsr.CurrentByteIndex = @base + loc;
				SaveGameHeader? sgh = SaveGameHeader.CreateFromHeaderInfo(SaveRef, headerInfo);
				if (sgh == null)
					continue;
				sgh.ParseStream(ref bsr);
				SaveGameHeaders.Add(sgh);
			}

			//@base = bsr.CurrentByteIndex = @base + sizeHeaders;
			ByteStreamReader bodyBase = bsr.Split(); // block data is offset from this location

			SaveHeader = bsr.ReadDataMap("Save Header", SaveInfo);
			
			SaveInfo.BaseTime = SaveHeader.GetFieldOrDefault<Time>("time__USE_VCR_MODE");
			//SaveInfo.LandmarkPos // todo gotten from adjacency list?

			@base = bsr.CurrentByteIndex;
			
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

			// now read the actual blocks of data based off of information from the headers
			foreach (SaveGameHeader header in SaveGameHeaders) {
				int loc = header.DataHeader.GetFieldOrDefault<int>("locBody");
				if (loc == -1)
					continue;
				//bodyBase.CurrentByteIndex = loc;
				//bsr.CurrentByteIndex = @base;
				if (!SaveStateBlock.CreateFromHeader(SaveRef!, header, out SaveStateBlock blockHandler)) {
					SaveInfo.AddError($"{nameof(SaveStateBlock)} not created from header: \"{header.Name}\"");
					continue;
				}
				blockHandler.ParseStream(ref bodyBase);
				Blocks.Add(blockHandler);
			}
			
			//bsr.CurrentByteIndex = @base + sizeBodies;
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