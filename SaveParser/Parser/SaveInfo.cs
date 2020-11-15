using System;
using System.Collections.Generic;
using System.Numerics;
using SaveParser.Parser.SaveFieldInfo;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;

namespace SaveParser.Parser {
	
	public class SaveInfo {
		
		internal ParseContext ParseContext;
		internal readonly List<string> Errors = new List<string>();
		
		public Vector3 LandmarkPos;
		public Time BaseTime;
		public float TickInterval = 0.015f; // todo

		public SaveInfo() {
			ParseContext = new ParseContext(0);
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
}