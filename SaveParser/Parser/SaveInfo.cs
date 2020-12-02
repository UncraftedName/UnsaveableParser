using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;

namespace SaveParser.Parser {
	
	public class SaveInfo {
		
		internal ParseContext ParseContext;
		internal readonly List<string> Errors = new List<string>();
		
		public Vector3 LandmarkPos;
		public Time BaseTime;
		public readonly float TickInterval;
		public readonly Game Game;
		public string? SaveDir;
		// todo two separate map lookups makes me :(( (maybe use special generators for shared classes)
		public readonly IReadOnlyDictionary<string, DataMap> SDataMapLookup; // server
		public readonly IReadOnlyDictionary<string, DataMap> CDataMapLookup; // client


		public SaveInfo(Game game) {
			ParseContext = new ParseContext(0);
			Game = game;
			TickInterval = game switch {
				Game.PORTAL1_3420 => 0.015f,
				Game.PORTAL2 => 1.0f / 60,
				_ => throw new ArgumentOutOfRangeException(nameof(game), game, "invalid game type")
			};
			// should be last
			SDataMapLookup = GlobalDataMapGenerator.GetDataMapList(this, false);
			CDataMapLookup = GlobalDataMapGenerator.GetDataMapList(this, true);
		}


		public void Cleanup() {
			ParseContext = default;
		}


		public void AddError(string e) {
			Console.Out.WriteLineColored(e);
			Errors.Add(e);
		}
		
		
		public int TimeToTicks(float time) => (int)((0.5f + time) / TickInterval);


		internal DataMapGeneratorInfo CreateGenInfo(bool client) {
			return new DataMapGeneratorInfo(Game, client);
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


	internal class DataMapGeneratorInfo {
		
		public readonly Game Game;
		// these are here for consistency to game code, or because I'm not sure of their values
		public readonly bool IsDefHl1Dll = false;
		public readonly bool IsDefHl2Dll = false;
		public readonly bool IsDefInvasionDll = false;
		public readonly bool IsDefHl2Episodic = false;
		public bool IsDefPortal => Game == Game.PORTAL1_3420 || Game == Game.PORTAL2;
		public readonly bool IsXBox = false;
		public readonly bool IsDefClientDll;
		
		
		public DataMapGeneratorInfo(Game game, bool isDefClientDll) {
			Game = game;
			IsDefClientDll = isDefClientDll;
		}
	}


	// If your game isn't listed here, the save should™ still parse (at least without vphys parsing). You can use the
	// errors to deduce many of the field types, although you won't get detailed information such as if it's a keyfield.
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public enum Game {
		PORTAL1_3420,
		PORTAL2
	}
}