using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SaveParser.Parser;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.StateFile.SaveStateData.EntData;

namespace SaveParser.Utils.ByteStreams {
	
	// some utility extensions of the reader so I can do brute force searches when the game code isn't enough
	public partial struct ByteStreamReader {
		
		
		[Conditional("DEBUG")]
		internal void DetermineDataMapHierarchy(SaveInfo info, string context, int offset) {
			List<string> classes = new List<string>();
			int tmp = AbsoluteByteIndex;
			AbsoluteByteIndex = offset;
			Console.Out.WriteLineColored($"Determining next datamaps hierarchy ({context})", ConsoleColor.Magenta);
			bool baseRead = false;
			for (int i = 0; i < 10; i++) {
				if (ReadSShort() != 4) {
					Console.WriteLine("first value not 4..");
					break;
				}
				string s = ReadSymbol(info)!;
				if (s == "AIExtendedSaveHeader_t") {
					// skip over the header stuff, this function should tell us if it inherits from CAI_BaseNPC anyway
					AbsoluteByteIndex -= 4;
					ReadDataMap("AIExtendedSaveHeader_t", info);
					StartBlock(info);
					var conditions = new CAI_BaseNpcEntData.ConditionStrings(info.SaveFile);
					conditions.ParseStream(ref this);
					EndBlock(info);
					StartBlock(info);
					var navigator = new CAI_NavigatorEntData(info.SaveFile);
					navigator.ParseStream(ref this);
					EndBlock(info);
					continue;
				} else {
					if (info.SDataMapLookup.TryGetValue(s, out DataMap? m) && m.BaseMap == null && !(baseRead ^= true))
						break;
					classes.Add(s);
				}
				int fieldsSaved = ReadSInt();
				for (int j = 0; j < fieldsSaved; j++) {
					StartBlock(info);
					SkipCurrentBlock(info);
				}
			}
			AbsoluteByteIndex = tmp;
			Console.WriteLine(((IEnumerable<string>)classes).Reverse().SequenceToString(" -> ", "", ""));
		}
	}
}