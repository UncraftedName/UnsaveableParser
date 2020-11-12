using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SaveParser.Parser;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Parser.StateFile;
using SaveParser.Parser.StateFile.SaveStateData;
using SaveParser.Utils;

namespace Testing {

	public static class Program {
		public static void Main() {
			Console.WriteLine("AHHHHH");
			
			//Console.SetOut(new StreamWriter("something.txt"));

			//SourceSave a = new SourceSave(@"D:\Programming\SourceEngine2007-master\game\portal\SAVE\09-corner.sav");
			//SourceSave a = new SourceSave(@"B:\Games\SteamLibrary\steamapps\common\Portal 2\portal2\SAVE\76561198259411922\doorskip.sav");
			SourceSave a = new SourceSave(@"D:\Games\Portal Source\portal\SAVE\quick.sav");
			//SourceSave a = new SourceSave(@"D:\Games\Portal 3420\portal\SAVE\quick.sav");
			//SourceSave a = new SourceSave(@"C:\Users\UncraftedName\Downloads\04_tas_il.sav");
			//SourceSave a = new SourceSave(@"D:\Programming\Rider\Listsave\Listsave\bin\Debug\vault90.sav");
			a.Parse();

			using var w = new IndentedTextWriter(new FileStream("something.txt", FileMode.Create));
			a.AppendToWriter(w);
		}
	}
}