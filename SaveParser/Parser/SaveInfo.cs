using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;

namespace SaveParser.Parser {
	
	public class SaveInfo {
		
		internal ParseContext ParseContext;
		internal readonly List<string> Errors = new List<string>();
		internal readonly SourceSave SaveFile;
		
		public Vector3 LandmarkPos;
		public Time BaseTime;
		public readonly float TickInterval;
		public readonly Game Game;
		public string? SaveDir;
		// todo two separate map lookups makes me :(( (maybe use special generators for shared classes)
		public readonly IReadOnlyDictionary<string, DataMap> SDataMapLookup; // server
		public readonly IReadOnlyDictionary<string, DataMap> CDataMapLookup; // client, always null until I start using it
		
		// map name, parent name, fields
		internal readonly List<DeterminedDataMap> DeterminedDatamaps;


		public SaveInfo(SourceSave saveFile, Game game) {
			SaveFile = saveFile;
			ParseContext = new ParseContext(0);
			Game = game;
			TickInterval = game switch {
				Game.PORTAL1_3420 => 0.015f,
				Game.PORTAL2 => 1.0f / 60,
				_ => throw new ArgumentOutOfRangeException(nameof(game), game, "invalid game type")
			};
			DeterminedDatamaps = new List<DeterminedDataMap>();
			
			// should be last
			DataMapGeneratorInfo info = new DataMapGeneratorInfo(game, false);
			SaveParserDataMapGenerator handler = new SaveParserDataMapGenerator(info);
			IDataMapInfoGeneratorHandler.IterateAllGenerators(handler);
			SDataMapLookup = handler.CompleteDataMapCollection; // after iteration, this is the result we need
			// now do the exact same thing to get the client maps
			// info = new DataMapGeneratorInfo(game, true);
			// handler = new SaveParserDataMapGenerator(info);
			// IDataMapInfoGeneratorHandler.IterateAllGenerators(handler);
			// CDataMapLookup = handler.CompleteDataMapCollection;
		}


		public void Cleanup() {
			ParseContext = default;
		}


		public void AddError(string e, bool print = true) {
			if (print)
				Console.Out.WriteLineColored(e);
			Errors.Add(e);
		}
		
		
		public int TimeToTicks(float time) => (int)((0.5f + time) / TickInterval);


		[Conditional("DEBUG")]
		internal void PrintDeterminedDatamaps() {
			if (DeterminedDatamaps.Count == 0)
				return;
			Console.WriteLine("\nThese fields were determined for datamaps that don't don't exist in the project:");
			foreach ((var mapName, string? parentName, var fields) in DeterminedDatamaps.Select(m => m.Deconstruct())) {
				Console.Write($"\nBeginDataMap(\"{mapName}");
				Console.WriteLine(parentName == null ? ");" : $", \"{parentName}\");");
				foreach ((short byteSize, string fieldName) in fields) {
					Console.WriteLine($"DefineField(\"{fieldName}\", {byteSize});");
				}
			}
		}
	}


	internal struct ParseContext {
		
		public string?[]? CurrentSymbolTable;
		public readonly Stack<int> Blocks;
		public ParsedEntData? CurrentEntity;
		public readonly Queue<(ParsedEntData ent, TypeDesc typeDesc)> VPhysicsRestoreInfo;
		

		public ParseContext(int dummy) {
			CurrentSymbolTable = null;
			Blocks = new Stack<int>();
			CurrentEntity = null;
			VPhysicsRestoreInfo = new Queue<(ParsedEntData, TypeDesc)>();
		}
	}


	public class DataMapGeneratorInfo {
		
		public readonly Game Game;
		// these are here for consistency to game code, or because I'm not sure of their values
		public readonly bool IsDefHl1Dll = false;
		public readonly bool IsDefHl2Dll = false;
		public readonly bool IsDefInvasionDll = false;
		public readonly bool IsDefHl2Episodic = false;
		public bool IsDefPortal => Game == Game.PORTAL1_3420 || Game == Game.PORTAL2;
		public readonly bool IsXBox = false;
		public readonly bool IsDefClientDll;
		
		
		public DataMapGeneratorInfo(Game game, bool isClient) {
			Game = game;
			IsDefClientDll = isClient;
		}
	}


	// If your game isn't listed here, the save should™ still parse (at least without vphys parsing). You can use the
	// errors to deduce many of the field types, although you won't get detailed information such as if it's a keyfield.
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public enum Game {
		PORTAL1_3420,
		PORTAL2
	}


	// used for debugging when I come across some map that I haven't seen before
	internal class DeterminedDataMap {
		
		public readonly string MapName;
		public string? ParentName;
		public HashSet<(short byteSize, string fieldName)> Fields;
		
		
		public DeterminedDataMap(string mapName) {
			MapName = mapName;
			Fields = new HashSet<(short byteSize, string fieldName)>();
		}


		public (string, string?, HashSet<(short byteSize, string fieldName)>) Deconstruct() {
			return (MapName, ParentName, Fields);
		}
	}
}