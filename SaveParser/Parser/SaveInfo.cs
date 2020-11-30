using System;
using System.Collections.Generic;
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
		public Game Game;
		public string? SaveDir;
		public readonly IReadOnlyDictionary<string, DataMap> DataMapLookup;

		
		public SaveInfo(Game game) {
			ParseContext = new ParseContext(0);
			Game = game;
			TickInterval = game switch {
				Game.Portal13420 => 0.015f,
				Game.Portal2 => 1.0f / 60,
				_ => throw new ArgumentOutOfRangeException(nameof(game), game, "invalid game type")
			};
			// should be last
			DataMapLookup = GlobalDataMapGenerator.GetDataMapList(this);
		}


		public void Cleanup() {
			ParseContext = default;
		}


		public void AddError(string e) {
			Console.Out.WriteLineColored(e);
			Errors.Add(e);
		}
		
		
		public int TimeToTicks(float time) => (int)((0.5f + time) / TickInterval);
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


	// if your game doesn't exist here, the save should™ still parse
	public enum Game {
		Portal13420,
		Portal2
	}
}