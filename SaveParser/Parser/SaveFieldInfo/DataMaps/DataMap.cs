using System;
using System.Collections.Generic;
using System.Linq;
using SaveParser.Utils;


namespace SaveParser.Parser.SaveFieldInfo.DataMaps {

	public class DataMap : IEquatable<DataMap> {

		public string DataMapName {get;internal set;} // resolved after all maps are processed
		private readonly string? _className;
		// you can always lookup this map by using its class name, but maybe not its datamap name
		public string ClassName => _className ?? DataMapName;
		
		public DataMap? BaseMap {get;internal set;}  // resolved after all maps are processed
		internal readonly Dictionary<string, TypeDesc> FieldDictInternal;
		public IReadOnlyDictionary<string, TypeDesc> FieldDict => FieldDictInternal;
		public int TotalFieldCount => FieldDictInternal.Count + (BaseMap?.TotalFieldCount ?? 0);
		internal readonly List<DataMapFunc> InputFuncsInternal;
		public IReadOnlyList<DataMapFunc> InputFuncs => InputFuncsInternal;
		internal readonly List<(string name, FunctionType type)> AdditionalFuncsInternal;
		public IReadOnlyList<(string name, FunctionType type)> AdditionalFuncs => AdditionalFuncsInternal;


		// used during the global datamap creation
		public DataMap(string className) {
			_className = DataMapName = className; // datamap name may be overwritten later
			FieldDictInternal = new Dictionary<string, TypeDesc>();
			InputFuncsInternal = new List<DataMapFunc>();
			AdditionalFuncsInternal = new List<(string, FunctionType)>();
		}
		
		
		// used by custom fields where the datamaps may be generated dynamically
		public DataMap(string dataMapName, IEnumerable<TypeDesc> dataDesc) {
			try {
				FieldDictInternal = dataDesc.ToDictionary(field => field.Name);
			} catch (Exception e) {
				Console.Out.WriteLineColored($"dictionary threw exception while creating datamap \"{dataMapName}\": {e.Message}");
				throw;
			}
			_className = null;
			DataMapName = dataMapName;
			InputFuncsInternal = new List<DataMapFunc>();
			AdditionalFuncsInternal = new List<(string, FunctionType)>();
		}


		public bool InheritsFrom(string name) => DataMapName == name || (BaseMap != null && BaseMap.InheritsFrom(name));


		public override string ToString() {
			return BaseMap == null
				? $"datamap {{{ClassName}, {FieldDictInternal.Count} fields}}"
				: $"datamap {{{ClassName}, {FieldDictInternal.Count} fields ({TotalFieldCount} total)}}";
		}


		public bool Equals(DataMap? other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return DataMapName == other.DataMapName
				   && Equals(BaseMap, other.BaseMap)
				   && FieldDictInternal.Count == other.FieldDictInternal.Count && !FieldDictInternal.Except(other.FieldDictInternal).Any()
				   && InputFuncs.SequenceEqual(other.InputFuncs); // todo
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
		
		public readonly string InternalName; // C++ dataMapName
		public readonly string ExternalName; // hammer and I/O name


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


	public enum FunctionType {
		NORMAL,
		THINK,
		ENTITY,
		USE
	}
}