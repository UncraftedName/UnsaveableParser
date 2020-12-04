using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils;
using static SaveParser.Parser.SaveFieldInfo.DescFlags;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps {
	
	public abstract class DataMapGenerator {
		
		// possibly useful fields during datamap generation
		internal DataMapGeneratorInfo GenInfo;
		protected Game Game => GenInfo.Game;

		// not meant to be accessed in inheriting classes
		internal readonly List<(string, DataMap)> UnlinkedBaseClasses = new List<(string, DataMap)>();
		internal readonly List<(string, TypeDesc)> UnlinkedEmbeddedMaps = new List<(string, TypeDesc)>();
		internal readonly Dictionary<string, string> Proxies = new Dictionary<string, string>();
		internal readonly List<DataMap> DataMaps = new List<DataMap>();
		internal readonly List<(string, string)> Links = new List<(string, string)>();
		internal readonly List<string> EmptyRoots = new List<string>();
		
		// temporary info for the datamap that is currently being constructed
		private string? _tmpName; // name of last class (might just be a proxy)
		private string? _tmpBaseClass;
		private bool _mapReady; // little botch to make the proxies work
		private DataMap CurMap => DataMaps[^1];


		public void Clear() {
			UnlinkedBaseClasses.Clear();
			UnlinkedEmbeddedMaps.Clear();
			Proxies.Clear();
			DataMaps.Clear();
			Links.Clear();
			EmptyRoots.Clear();
			_mapReady = false;
			_tmpName = _tmpBaseClass = null;
		}
		
		
		public void Generate() { // clear before calling
			CreateDataMaps();
			FinishDataMap();
		}


		private void AddFieldPrivate(TypeDesc td)
			=> CurMap.FieldDictInternal.Add(td.Name, td);


		protected void BeginDataMap(string className, string? baseClass = null) {
			FinishDataMap();
			_tmpName = className;
			_tmpBaseClass = baseClass;
			_mapReady = true;
			DataMaps.Add(new DataMap(_tmpName));
		}


		// Suppose C : B & B : A, but B is a class without any fields. In the code you can still see that B exists, but
		// valve might not have even made a datamap for it (since it has no fields). So this is a way to effectively
		// create a "blank" class that will be skipped over while parsing maps recursively. Except there won't actually
		// be a map - I will simply find any references to this class and change them to be straight to the base class.
		// (Maps with no fields can still exist, this is more of a macro).
		protected void DataMapProxy(string name, string baseClass) {
			FinishDataMap();
			Proxies.Add(name, baseClass);
			_tmpName = name;
			_mapReady = false;
		}


		// any datamap names that refer to classes should be added here
		protected void LinkNamesToMap(params string[] proxies) {
			if (_tmpName == null)
				throw new Exception("no map to link to");
			foreach (string proxy in proxies)
				Links.Add((proxy, _tmpName!));
		}


		protected void DefineRootClassNoMap(string name)
			=> EmptyRoots.Add(name);


		// todo consider creating a special DefineEHandle where you can add what type of handle it is
		protected void DefineField(string name, FieldType fieldType, ushort count = 1)
			=> AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE, count));


		protected void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1)
			=> AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_INPUT, count, inputName));


		protected void DefineOutput(string name, string outputName) {
			var td = new TypeDesc(name, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_OUTPUT, EventsSave.Restore, outputName: outputName);
			AddFieldPrivate(td);
			CurMap.InputFuncsInternal.Add(new OutputDataMapFunc(td, name, outputName));
		}


		protected void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY, count, mapName: mapName));
		
		
		// some fields have a key and a input... I can't handle them separately so here they are together
		protected void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType, ushort count = 1)
			=> AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY, count, mapName: mapName, inputName: inputName));


		protected void DefineGlobalField(string name, FieldType fieldType, ushort count = 1)
			=> AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL, count));


		protected void DefineGlobalKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> AddFieldPrivate(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL | FTYPEDESC_KEY, count, mapName: mapName));
		
		
		// not relevant for save files, but i'll save these anyways
		protected void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType) {
			CurMap.InputFuncsInternal.Add(new InputDataMapFunc(inputFunc, inputName, fieldType));
		}
		

		protected void DefineFunction(string name) {
			AddFieldPrivate(new TypeDesc(name, FUNCTION));
			CurMap.AdditionalFuncsInternal.Add((name, FunctionType.NORMAL));
		}

		// I don't know what these are for - they seem to be implemented similarly to DEFINE_FUNCTION.
		// They might be used as inputs in game, but I haven't seen them come up in save files...
		
		protected void DefineThinkFunc(string funcName) {
			CurMap.AdditionalFuncsInternal.Add((funcName, FunctionType.THINK));
		}


		protected void DefineEntityFunc(string funcName) {
			CurMap.AdditionalFuncsInternal.Add((funcName, FunctionType.ENTITY));
		}


		protected void DefineUseFunc(string funcName) {
			CurMap.AdditionalFuncsInternal.Add((funcName, FunctionType.USE));
		}


		protected void DefineCustomField(
			string name,
			CustomReadFunc customReadFunc,
			object?[]? customParams = null,
			DescFlags flags = FTYPEDESC_SAVE)
		{
			AddFieldPrivate(new TypeDesc(name, flags, customReadFunc, customParams));
		}


		protected void DefineSoundPatch(string name) => DefineCustomField(name, SoundPatch.Restore);


		protected void DefinePhysPtr(string name) {
			// phys stuff is actually restored during the phys block handler, it's only queued for restore in the ents
			AddFieldPrivate(new TypeDesc(name, FTYPEDESC_SAVE, CPhysicsEnvironment.QueueRestore));
		}


		// an embedded field simply means that the field should be read like a data map (recursively)
		protected void DefineEmbeddedField(
			string name,
			string embeddedMap,
			ushort count = 1)
		{
			var td = new TypeDesc(name,EMBEDDED, FTYPEDESC_SAVE, count);
			AddFieldPrivate(td);
			UnlinkedEmbeddedMaps.Add((embeddedMap, td));
		}


		protected void DefineVector(string name, string elementType, DescFlags vecFlags = FTYPEDESC_SAVE)
			=> DefineCustomField(name, UtilVector<ParsedDataMap>.Restore, new object?[] {EMBEDDED, null, elementType}, vecFlags);


		private static readonly Dictionary<FieldType, CustomReadFunc> VecFuncList
			= new Dictionary<FieldType, CustomReadFunc>();

		protected void DefineVector(
			string name,
			FieldType elemFieldType,
			DescFlags vecFlags = FTYPEDESC_SAVE,
			CustomReadFunc? elemReadFunc = null)
		{
			if (!VecFuncList.TryGetValue(elemFieldType, out CustomReadFunc? vecReadFunc)) {
				vecReadFunc =
					(CustomReadFunc)typeof(UtilVector<>).MakeGenericType(
							TypeDesc.GetNetTypeFromFieldType(elemFieldType))
						.GetMethods(BindingFlags.Static | BindingFlags.Public)
						.Single(info => info.Name == nameof(UtilVector<object>.Restore) 
										&& ParserUtils.IsMethodCompatibleWithDelegate<CustomReadFunc>(info))
						.CreateDelegate(typeof(CustomReadFunc));
				VecFuncList.Add(elemFieldType, vecReadFunc);
			}
			object?[] customParams = {elemFieldType, elemReadFunc, null};
			AddFieldPrivate(new TypeDesc(name, vecFlags, vecReadFunc, customParams));
		}


		protected void DefineUtilMap(
			string name,
			FieldType keyType,
			FieldType valType,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? keyReadFunc = null,
			CustomReadFunc? valReadFunc = null)
			=> DefineUtilMapPrivate(name, keyType, valType, null, null, utlMapFlags, keyReadFunc, valReadFunc);


		protected void DefineUtilMap(
			string name,
			string embeddedKeyName,
			FieldType valType,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? valReadFunc = null)
			=> DefineUtilMapPrivate(name, EMBEDDED, valType, embeddedKeyName, null, utlMapFlags, null, valReadFunc);


		protected void DefineUtilMap(
			string name,
			FieldType keyType,
			string embeddedValName,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? keyReadFunc = null) 
			=> DefineUtilMapPrivate(name, keyType, EMBEDDED, null, embeddedValName, utlMapFlags, keyReadFunc, null);


		protected void DefineUtilMap(
			string name,
			string embeddedKeyName,
			string embeddedValName,
			DescFlags utlMapFlags = FTYPEDESC_SAVE)
			=> DefineUtilMapPrivate(name, EMBEDDED, EMBEDDED, embeddedKeyName, embeddedValName, utlMapFlags, null, null);


		private static readonly List<(FieldType keyType, FieldType valType, CustomReadFunc readFunc)> UtilMapFuncList
			= new List<(FieldType, FieldType, CustomReadFunc)>();


		private void DefineUtilMapPrivate(
			string name,
			FieldType keyType,
			FieldType valType,
			string? embeddedKeyName,
			string? embeddedValName,
			DescFlags utlMapFlags,
			CustomReadFunc? keyReadFunc,
			CustomReadFunc? valReadFunc)
		{
			// check to see if a read func has already been created for this key/val combo
			CustomReadFunc? utlMapReadFunc = UtilMapFuncList.Find(tuple => tuple.keyType == keyType && tuple.valType == valType).readFunc;
			if (utlMapReadFunc == null!) { // if not, make it with the wonders of reflection!!
				utlMapReadFunc =
					(CustomReadFunc)typeof(UtilMap<,>).MakeGenericType(
							keyType == EMBEDDED ? typeof(ParsedDataMap) : TypeDesc.GetNetTypeFromFieldType(keyType),
							valType == EMBEDDED ? typeof(ParsedDataMap) : TypeDesc.GetNetTypeFromFieldType(valType))
						.GetMethods(BindingFlags.Static | BindingFlags.Public)
						.Single(info => info.Name == nameof(UtilMap<object,object>.Restore) 
										&& ParserUtils.IsMethodCompatibleWithDelegate<CustomReadFunc>(info))
						.CreateDelegate(typeof(CustomReadFunc));
				// add it to the list so we (hopefully) don't have to do that ^ everytime
				UtilMapFuncList.Add((keyType, valType, utlMapReadFunc));
			}
			object?[] customParams = {keyType, keyReadFunc, embeddedKeyName, valType, valReadFunc, embeddedValName};
			AddFieldPrivate(new TypeDesc(name, utlMapFlags, utlMapReadFunc, customParams));
		}

		
		// use for embedded fields where you don't know the embedded type
		protected void DefinePlaceholder(string name) {
#if DEBUG
			AddFieldPrivate(new TypeDesc(name));
#endif
		}


		private void FinishDataMap() {
			if (_mapReady && _tmpBaseClass != null)
				UnlinkedBaseClasses.Add((_tmpBaseClass, CurMap));
		}
		

		protected abstract void CreateDataMaps();
	}
}