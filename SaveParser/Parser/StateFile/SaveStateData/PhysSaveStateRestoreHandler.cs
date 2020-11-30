using System;
using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.StateFile.SaveStateData {
	
	public class PhysSaveStateRestoreHandler : SaveStateBlock<PhysicsInfoHeader> {

		public List<(ParsedDataMap header, ParsedSaveField[]? objects)> PhysObjects;
		
		
		public PhysSaveStateRestoreHandler(SourceSave? saveRef, PhysicsInfoHeader dataHeader) : base(saveRef, dataHeader) {}
		
		
		protected override void Parse(ref BitStreamReader bsr) {
			var queue = SaveInfo.ParseContext.VPhysicsRestoreInfo;
			PhysObjects = new List<(ParsedDataMap header, ParsedSaveField[]? objects)>(queue.Count);
#if DEBUG_WITH_PHYS
			try {
				while (queue.TryDequeue(out var physRestoreInfo)) {
					var header = bsr.ReadDataMap("PhysObjectHeader_t", SaveInfo);
					ParsedSaveField[]? objects = null;
					bsr.StartBlock(SaveInfo);
					if (header.GetFieldOrDefault<Ehandle>("hEntity") != (Ehandle)(-1)) { // not sure if -1
						int count = header.GetFieldOrDefault<int>("nObjects");
						objects = new ParsedSaveField[count];
						for (int i = 0; i < count; i++) {
							bsr.StartBlock(SaveInfo);
							var physObj = CPhysicsEnvironment.Restore(SaveInfo, header, physRestoreInfo, ref bsr);
							if (physObj == null) {
								bsr.SkipCurrentBlock(SaveInfo);
							} else {
								objects[i] = physObj;
								bsr.EndBlock(SaveInfo);
							}
						}
					}
					PhysObjects.Add((header, objects));
					bsr.EndBlock(SaveInfo);
				}
			} catch (Exception e) {
				SaveInfo.AddError($"phys parsing failed, aborting. msg: {e.Message}");
				queue.Clear();
			}
#else
			SaveInfo.AddError($"parsing skipped for {queue.Count} phys objects");
			queue.Clear();
#endif
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{PhysObjects.Count} physics descriptions:");
			iw.FutureIndent++;
			foreach ((ParsedDataMap header, ParsedSaveField[]? objects) in PhysObjects) {
				iw.AppendLine();
				header.AppendToWriter(iw);
				if (objects != null) {
					iw.Append($"\n{objects.Length} phys object{(objects.Length == 1 ? "" : "s")}");
					if (objects.Length > 0) {
						iw.Append(":");
						EnumerableAppendHelper(objects, iw);
					}
				}
			}
			iw.FutureIndent--;
		}
	}
}