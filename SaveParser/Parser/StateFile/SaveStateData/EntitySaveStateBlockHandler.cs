using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile.SaveStateData {
	
	public class EntitySaveStateBlock : SaveStateBlock<ETableHeader> {

		public ParsedEntData?[] EntData;
		
		public EntitySaveStateBlock(SourceSave? saveRef, ETableHeader dataHeader) : base(saveRef, dataHeader) {}
		
		private static readonly HashSet<string> Searched = new HashSet<string>(); // for debugging
		
		
		protected override void Parse(ref ByteStreamReader bsr) {
			EntData = new ParsedEntData[DataHeader.EntHeaders.Length];
			for (int i = 0; i < DataHeader.EntHeaders.Length; i++) {
				ParsedDataMap entHeader = DataHeader.EntHeaders[i];
				var edictIndex = entHeader.GetFieldOrDefault<int>("edictindex");
				bsr.CurrentByteIndex = entHeader.GetFieldOrDefault<int>("location");
				string? className = entHeader.GetFieldOrDefault<string>("classname");
				if (className == null) // todo
					continue;
				if (!SaveInfo.SDataMapLookup.TryGetValue(className, out DataMap? entMap)) {
					string s = $"{nameof(EntitySaveStateBlock)}.{nameof(Parse)} - datamap for \"{className}\" not found";
					if (bsr.ReadSShort() == 4 && Searched.Add(s)) {
						bsr.DetermineDataMapHierarchy(SaveInfo,
							$"{nameof(EntitySaveStateBlock)}.{nameof(Parse)} - datamap for \"{className}\" not found",
							bsr.AbsoluteByteIndex - 2);
					}
					SaveInfo.AddError(s
#if DEBUG
						,false
#endif
						);
					continue;
				}
				EntData[i] = EntDataFactory.CreateFromName(SaveRef!, entHeader, entMap);
				SaveInfo.ParseContext.CurrentEntity = EntData[i];
				EntData[i]!.ParseStream(ref bsr);
			}
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.Append($"\n{EntData.Length} entities:");
#pragma warning disable 8631
			EnumerableAppendHelper(EntData, iw, enumerate: true);
#pragma warning restore 8631
		}
	}
}