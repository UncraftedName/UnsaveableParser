using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaveParser.Utils;


namespace SaveParser.Parser.SaveFieldInfo.DataMaps {

	public class DataMap : IEquatable<DataMap> {

		public readonly string Name;
		public DataMap? BaseMap {get;internal set;}
		internal readonly Dictionary<string, TypeDesc> FieldDictInternal;
		public IReadOnlyDictionary<string, TypeDesc> FieldDict => FieldDictInternal;
		public int TotalFieldCount => FieldDictInternal.Count + (BaseMap?.TotalFieldCount ?? 0);
		public readonly List<DataMapFunc> FunctionsInternal;
		public IReadOnlyList<DataMapFunc> Functions => FunctionsInternal;


		// other fields are populated after initialization
		internal DataMap(string name, IEnumerable<TypeDesc>? dataDesc = null) {
			if (dataDesc == null) {
				FieldDictInternal = new Dictionary<string, TypeDesc>();
			} else {
				try {
					FieldDictInternal = dataDesc.ToDictionary(field => field.Name);
				} catch (Exception e) {
					Console.Out.WriteLineColored($"dictionary threw exception while creating datamap \"{name}\": {e.Message}");
					throw;
				}
			}
			Name = name;
			FunctionsInternal = new List<DataMapFunc>();
		}


		public bool InheritsFrom(string name) => Name == name || (BaseMap != null && BaseMap.InheritsFrom(name));


		public override string ToString() {
			StringBuilder sb = new StringBuilder($"{{{Name}, {FieldDictInternal.Count} fields");
			if (BaseMap != null)
				sb.Append($" ({TotalFieldCount} total)");
			sb.Append('}');
			return sb.ToString();
		}


		public bool Equals(DataMap? other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Name == other.Name
				   && Equals(BaseMap, other.BaseMap)
				   && FieldDictInternal.Count == other.FieldDictInternal.Count && !FieldDictInternal.Except(other.FieldDictInternal).Any()
				   && Functions.SequenceEqual(other.Functions);
		}


#pragma warning disable 659
		public override bool Equals(object? obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((DataMap)obj);
		}
#pragma warning restore 659
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