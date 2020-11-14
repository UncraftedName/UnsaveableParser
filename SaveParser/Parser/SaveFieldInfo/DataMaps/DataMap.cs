using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveParser.Utils;


namespace SaveParser.Parser.SaveFieldInfo.DataMaps {

	public class DataMap {

		public readonly string Name;
		public DataMap? BaseMap {get;internal set;}
		public Dictionary<string, TypeDesc> FieldDict {get;}
		public int TotalFieldCount => FieldDict.Count + (BaseMap?.TotalFieldCount ?? 0);
		public List<DataMapFunc> Functions {get;}


		// other fields are populated after initialization
		internal DataMap(string name, IEnumerable<TypeDesc>? dataDesc = null) {
			if (dataDesc == null) {
				FieldDict = new Dictionary<string, TypeDesc>();
			} else {
				try {
					FieldDict = dataDesc.ToDictionary(field => field.Name);
				} catch (Exception e) {
					Console.Out.WriteLineColored($"dictionary threw exception while creating datamap \"{name}\": {e.Message}");
					throw;
				}
			}
			Name = name;
			Functions = new List<DataMapFunc>();
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


	/// <summary>
	/// A function referenced by the map file or I/O events.
	/// </summary>
	public abstract class DataMapFunc {
		
		public readonly string InternalName; // C++ name
		public readonly string ExternalName; // map and I/O name


		public DataMapFunc(string internalName, string externalName) {
			InternalName = internalName;
			ExternalName = externalName;
		}
	}


	public class InputDataMapFunc : DataMapFunc {
		
		public readonly FieldType InputType;
		// I would add a type desc for a ref of INPUTNOTIFY fields, but those aren't used by the game.


		public InputDataMapFunc(string internalName, string externalName, FieldType inputType)
			: base(internalName, externalName)
		{
			InputType = inputType;
		}


		public override string ToString() {
			return $"input func {InputType} {ExternalName}";
		}
	}


	public class OutputDataMapFunc : DataMapFunc {

		public readonly TypeDesc OutputEvents;
		
		
		public OutputDataMapFunc(TypeDesc outputEvents, string internalName, string externalName)
			: base(internalName, externalName)
		{
			OutputEvents = outputEvents;
		}


		public override string ToString() {
			return $"output func {ExternalName}";
		}
	}
}