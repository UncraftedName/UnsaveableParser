using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;


namespace SaveParser.Parser.SaveFieldInfo.DataMaps {
	
	public delegate ParsedSaveField? CustomReadFunc(TypeDesc typeDesc, SaveInfo info, ref BitStreamReader bsr);

	public class DataMap {

		public readonly string Name;
		public DataMap? BaseMap {get;internal set;} // set in the global map creation
		public Dictionary<string, TypeDesc> FieldDict {get;}
		public int TotalFieldCount => FieldDict.Count + (BaseMap?.TotalFieldCount ?? 0);


		// base map linked later
		internal DataMap(string name, IEnumerable<TypeDesc> dataDesc) {
			try {
				FieldDict = dataDesc.ToDictionary(field => field.Name);
			} catch (Exception e) {
				Console.Out.WriteLineColored($"dictionary threw exception while creating datamap \"{name}\": {e.Message}");
				throw;
			}
			Name = name;
		}


		public bool InheritsFrom(string name) => Name == name || (BaseMap != null && BaseMap.InheritsFrom(name));


		public override string ToString() {
			StringBuilder sb = new StringBuilder($"{{{Name}, {FieldDict.Count} fields");
			if (BaseMap != null)
				sb.Append($" ({TotalFieldCount} total)");
			sb.Append('}');
			return sb.ToString();
		}
	}
}