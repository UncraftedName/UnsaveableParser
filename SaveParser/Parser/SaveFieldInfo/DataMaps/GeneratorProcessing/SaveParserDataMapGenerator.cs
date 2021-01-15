using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils;
using static SaveParser.Parser.SaveFieldInfo.DescFlags;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing {

	// creates the global datamap dictionary used by the parser
	public class SaveParserDataMapGenerator : IDataMapInfoGeneratorHandler {
		
		private readonly HashSet<string> _emptyRoots;
		private readonly List<(DataMap map, string baseClass)> _unresolvedBaseClasses;
		private readonly Dictionary<string, string> _proxies; // name, base class
		private readonly List<(string, TypeDesc)> _unresolvedEmbeddedFields;
		private DataMap? _curMap;
		private (string name, string baseClass)? _curProxy;
		private bool _finished;

		private readonly Dictionary<string, DataMap> _maps;
		public IReadOnlyDictionary<string, DataMap> CompleteDataMapCollection { // after iteration, this is the final result
			get {
				if (_finished)
					return _maps;
				throw new ConstraintException("not notified of finished iteration");
			}
		}
		public DataMapGeneratorInfo GenInfo {get;}


		public SaveParserDataMapGenerator(DataMapGeneratorInfo info) {
			GenInfo = info;
			_maps = new Dictionary<string, DataMap>(100);
			_emptyRoots = new HashSet<string>();
			_proxies = new Dictionary<string, string>(50);
			_unresolvedBaseClasses = new List<(DataMap map, string baseClass)>();
			_unresolvedEmbeddedFields = new List<(string, TypeDesc)>();
			_curMap = null;
			_curProxy = null;
			_finished = false;
		}
		

		// it's time to link all the pieces together
		public void OnFinishedIterationOfInfoGenerators() {

			// go through all proxies, if the key does not exist in the complete collection then add it
			foreach ((string name, string baseClass) in _proxies) {
				// If there exists any proxies/links A->B->C, then we want A->C.
				// For example, consider the datamaps with this inheritance pattern - A:B, B:C, C:D, & D:E.
				// But imagine that A, B, and C are just proxies. So when you look for A or B, you really mean D.
				string actualBase = baseClass;
				while (_proxies.TryGetValue(actualBase, out string? nextBase))
					actualBase = nextBase;
				
				if (_maps.TryGetValue(name, out DataMap? existingMap)) {
					if (existingMap.Name != actualBase)
						throw new ConstraintException($"\"{name}\" already present in as \"{existingMap.Name}\", tried to overwrite with \"{actualBase}\"");
					continue;
				}
				_maps.Add(name, _maps[actualBase]);
			}
			
			// resolve base classes
			foreach ((DataMap map, string baseClass) in _unresolvedBaseClasses) {
				try {
					string actual = _proxies.GetValueOrDefault(baseClass, baseClass)!;
					if (!_emptyRoots.Contains(actual))
						map.BaseMap = _maps[actual];
				} catch (Exception e) {
					throw new Exception($"no base class called \"{baseClass}\" found", e);
				}
			}
			
			// resolve embedded fields
			foreach ((string embeddedMapName, TypeDesc desc) in _unresolvedEmbeddedFields) {
				try {
					string actual = _proxies.GetValueOrDefault(embeddedMapName, embeddedMapName)!;
					desc.EmbeddedMap = _maps[actual];
				} catch (Exception e) {
					throw new Exception($"no map for embedded field \"{desc}\" called \"{embeddedMapName}\"", e);
				}
			}

			_finished = true;
		}


		private void CheckFinish() {
			if (_finished)
				throw new ConstraintException("already notified of finished iteration");
		}
		
		
		private void AddFieldPrivate(TypeDesc td) {
			CheckFinish();
			if (_curMap == null || _curProxy != null)
				throw new NullReferenceException("attempted to add a field to a proxy");
			_curMap.FieldDictInternal.Add(td.Name, td);
		}


		public void BeginDataMap(string className, string? baseClass = null) {
			CheckFinish();
			_curProxy = null;
			_maps.Add(className, _curMap = new DataMap(className));
			if (baseClass != null)
				_unresolvedBaseClasses.Add((_curMap, baseClass));
		}


		public void DataMapProxy(string name, string baseClass) {
			CheckFinish();
			_curMap = null;
			_proxies.Add(name, baseClass);
			_curProxy = (name, baseClass);
			
		}


		public void LinkNamesToMap(params string[] proxies) {
			if (_curMap != null) {
				foreach (string s in proxies)
					_maps.Add(s, _curMap);
			} else {
				foreach (string s in proxies)
					_proxies.Add(s, _curProxy!.Value.name);
			}
		}


		public void DefineRootClassNoMap(string name) {
			_emptyRoots.Add(name);
		}


		public void DefineField(string name, FieldType fieldType, ushort count = 1, DescFlags flags = FTYPEDESC_SAVE) {
			AddFieldPrivate(new TypeDesc(name, fieldType, flags, count));
		}


		public void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1) {
			AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_INPUT, count, inputName));
		}


		public void DefineOutput(string name, string outputName) {
			var td = new TypeDesc(name, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_OUTPUT, EventsSave.Restore, outputName: outputName);
			AddFieldPrivate(td);
			_curMap!.InputFuncsInternal.Add(new OutputDataMapFunc(td, name, outputName));
		}


		public void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1, DescFlags flags = FTYPEDESC_SAVE | FTYPEDESC_KEY) {
			AddFieldPrivate(new TypeDesc(name, fieldType, flags, count, mapName: mapName));
		}


		public void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType, ushort count = 1) {
			AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY, count, mapName: mapName, inputName: inputName));
		}


		public void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType) {
			_curMap!.InputFuncsInternal.Add(new InputDataMapFunc(inputFunc, inputName, fieldType));
		}


		public void DefineFunction(string name, FunctionType functionType) {
			if (functionType == FunctionType.NORMAL)
				AddFieldPrivate(new TypeDesc(name, FUNCTION));
			_curMap!.AdditionalFuncsInternal.Add((name, functionType));
		}


		public void DefineCustomField(string name, CustomReadFunc customReadFunc, object?[]? customParams = null, DescFlags flags = FTYPEDESC_SAVE) {
			AddFieldPrivate(new TypeDesc(name, flags, customReadFunc, customParams));
		}


		public void DefineEmbeddedField(string name, string embeddedMap, ushort count = 1) {
			var td = new TypeDesc(name, EMBEDDED, FTYPEDESC_SAVE, count);
			AddFieldPrivate(td);
			_unresolvedEmbeddedFields.Add((embeddedMap, td));
		}


		public void DefineVector(string name, string elementType, DescFlags vecFlags = FTYPEDESC_SAVE) {
			DefineCustomField(name, UtilVector<ParsedDataMap>.Restore, new object?[] {EMBEDDED, null, elementType}, vecFlags);
		}

		
		// stores previously generated read functions for a given field type
		private static readonly Dictionary<FieldType, CustomReadFunc> VecFuncList = new Dictionary<FieldType, CustomReadFunc>();
		

		public void DefineVector(string name, FieldType elemFieldType, DescFlags vecFlags = FTYPEDESC_SAVE, CustomReadFunc? elemReadFunc = null) {
			// check if we've already created this read func
			if (!VecFuncList.TryGetValue(elemFieldType, out CustomReadFunc? vecReadFunc)) {
				// if not, create it using the power of god, anime, and reflection
				vecReadFunc =
					(CustomReadFunc)typeof(UtilVector<>).MakeGenericType(
							TypeDesc.GetNetTypeFromFieldType(elemFieldType))
						.GetMethods(BindingFlags.Static | BindingFlags.Public)
						.Single(info => info.Name == nameof(UtilVector<object>.Restore) // dummy type
										&& ParserUtils.IsMethodCompatibleWithDelegate<CustomReadFunc>(info))
						.CreateDelegate(typeof(CustomReadFunc));
				VecFuncList.Add(elemFieldType, vecReadFunc);
			}
			object?[] customParams = {elemFieldType, elemReadFunc, null};
			AddFieldPrivate(new TypeDesc(name, vecFlags, vecReadFunc, customParams));
		}

		
		// same idea as for the vector
		private static readonly List<(FieldType keyType, FieldType valType, CustomReadFunc readFunc)> UtilMapFuncList = new List<(FieldType, FieldType, CustomReadFunc)>();
		

		public void DefineUtilMap(
			string name,
			FieldType keyType,
			FieldType valType,
			string? embeddedKeyName,
			string? embeddedValName,
			DescFlags utlMapFlags,
			CustomReadFunc? keyReadFunc,
			CustomReadFunc? valReadFunc)
		{
			CustomReadFunc? utlMapReadFunc = UtilMapFuncList.Find(tuple => tuple.keyType == keyType && tuple.valType == valType).readFunc;
			if (utlMapReadFunc == null!) {
				utlMapReadFunc =
					(CustomReadFunc)typeof(UtilMap<,>).MakeGenericType(
							keyType == EMBEDDED ? typeof(ParsedDataMap) : TypeDesc.GetNetTypeFromFieldType(keyType),
							valType == EMBEDDED ? typeof(ParsedDataMap) : TypeDesc.GetNetTypeFromFieldType(valType))
						.GetMethods(BindingFlags.Static | BindingFlags.Public)
						.Single(info => info.Name == nameof(UtilMap<object,object>.Restore)
										&& ParserUtils.IsMethodCompatibleWithDelegate<CustomReadFunc>(info))
						.CreateDelegate(typeof(CustomReadFunc));
				UtilMapFuncList.Add((keyType, valType, utlMapReadFunc));
			}
			object?[] customParams = {keyType, keyReadFunc, embeddedKeyName, valType, valReadFunc, embeddedValName};
			AddFieldPrivate(new TypeDesc(name, utlMapFlags, utlMapReadFunc, customParams));
		}


		public void DefinePlaceholderEmbeddedField(string name) {
#if DEBUG
			AddFieldPrivate(new TypeDesc(name));
#endif
		}
	}
}