using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;
using static SaveParser.Parser.SaveFieldInfo.DescFlags;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps {
	
	public abstract class DataMapGenerator {

		// not meant to be accessed in inheriting classes
		internal readonly List<(string, DataMap)> UnlinkedBaseClasses = new List<(string, DataMap)>();
		internal readonly List<(string, TypeDesc)> UnlinkedEmbeddedMaps = new List<(string, TypeDesc)>();
		internal readonly Dictionary<string, string> Proxies = new Dictionary<string, string>();
		internal readonly List<DataMap> DataMaps = new List<DataMap>();
		internal readonly List<(string, string)> Links = new List<(string, string)>();
		internal readonly List<string> EmptyRoots = new List<string>();
		// temporary info for the datamap that is currently being constructed
		private string? _tmpName;
		private string? _tmpBaseClass;
		private bool _mapReady = false;
		internal readonly List<TypeDesc> TmpFields = new List<TypeDesc>();


		public void Generate() {
			CreateDataMaps();
			FinishDataMap();
		}


		protected void BeginDataMap(string className, string? baseClass = null) {
			FinishDataMap();
			_tmpName = className;
			_tmpBaseClass = baseClass;
			TmpFields.Clear();
			_mapReady = true;
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


		protected void DefineRootClassNoMap(string name) {
			EmptyRoots.Add(name);
		}


		protected void DefineFunction(string name)
			=> TmpFields.Add(new TypeDesc(name, FUNCTION,  FTYPEDESC_SAVE));


		// todo consider creating a special DefineEHandle where you can add what type of handle it is
		protected void DefineField(string name, FieldType fieldType, ushort count = 1)
			=> TmpFields.Add(new TypeDesc(name, fieldType, FTYPEDESC_SAVE, count));


		protected void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1) {
			TmpFields.Add(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_INPUT, count, inputName: inputName));
		}


		protected void DefineOutput(string name, string outputName)
			=> TmpFields.Add(new TypeDesc(name, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_OUTPUT, EventsSave.Restore, outputName: outputName));


		protected void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> TmpFields.Add(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY, count, mapName: mapName));
		
		
		protected void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType, ushort count = 1)
			=> TmpFields.Add(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_KEY, count, mapName: mapName, inputName: inputName));


		protected void DefineGlobalField(string name, FieldType fieldType, ushort count = 1) {
			TmpFields.Add(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL, count));
		}


		protected void DefineGlobalKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> TmpFields.Add(new TypeDesc(name, fieldType, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL | FTYPEDESC_KEY, count, mapName: mapName));
		
		
		// i don't think this is relevant for save files
		protected void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType) {}
		
		// i don't even know what these are for
		protected void DefineThinkFunc(string funcName) {}
		protected void DefineUseFunc(string funcName) {}


		// custom field requires a custom read function (I allow null as a placeholder)
		protected void DefineCustomField(
			string name,
			CustomReadFunc? customReadFunc,
			object?[]? customParams = null,
			DescFlags flags = FTYPEDESC_SAVE)
		{
			TmpFields.Add(new TypeDesc(name, flags, customReadFunc, customParams));
		}


		protected void DefineSoundPatch(string name) => DefineCustomField(name, SoundPatch.Restore);


		protected void DefinePhysPtr(string name) {
			// phys stuff is actually restored during the phys block handler, it's only queued for restore in the ents
			TmpFields.Add(new TypeDesc(name, FTYPEDESC_SAVE, VPhysPtrSave.QueueRestore));
		}


		// an embedded field simply means that the field should be read like a data map (recursively)
		protected void DefineEmbeddedField(
			string name,
			string embeddedMap,
			ushort count = 1)
		{
			var td = new TypeDesc(name,EMBEDDED, FTYPEDESC_SAVE, count);
			TmpFields.Add(td);
			UnlinkedEmbeddedMaps.Add((embeddedMap, td));
		}


		protected void DefineEmbeddedVector(string name, string elementType)
			=> DefineCustomField(name, UtilVector<ParsedDataMap>.Restore, new object?[] {EMBEDDED, null, elementType});


		protected void DefineGlobalEmbeddedVector(string name, string elementType)
			=> DefineCustomField(name, UtilVector<ParsedDataMap>.Restore, new object?[] {EMBEDDED, null, elementType}, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL);


		// Vec stuff = stuff belonging to the vector field.
		// Elem stuff = stuff belonging to the elements of the vector.
		protected void DefineVector(
			string name,
			FieldType elemFieldType,
			DescFlags vecFlags = FTYPEDESC_SAVE,
			CustomReadFunc? elemReadFunc = null)
		{
			CustomReadFunc vecReadFunc = (CustomReadFunc)typeof(UtilVector<>).MakeGenericType(TypeDesc.GetNetTypeFromFieldType(elemFieldType))
				.GetMethods(BindingFlags.Static | BindingFlags.Public)
				.Single(info => info.Name == "Restore" && ParserUtils.IsMethodCompatibleWithDelegate<CustomReadFunc>(info))!
				.CreateDelegate(typeof(CustomReadFunc));
			
			object?[] customParams = {elemFieldType, elemReadFunc, null}; // see UtilVector to see how these are used
			TmpFields.Add(new TypeDesc(name, vecFlags, vecReadFunc, customParams));
		}


		private void FinishDataMap() {
			if (_mapReady) {
				var map = new DataMap(_tmpName!, TmpFields);
				DataMaps.Add(map);
				if (_tmpBaseClass != null)
					UnlinkedBaseClasses.Add((_tmpBaseClass, map));
			}
		}

		
		protected void DefineMaterialIndexDataOps(string name) {
			static ParsedSaveField MatReadFunc(TypeDesc desc, SaveInfo info, ref BitStreamReader bsr)
				=> new ParsedSaveField<MaterialIndexStr>((MaterialIndexStr)bsr.ReadStringOfLength(bsr.ReadSInt()), desc);
			TmpFields.Add(new TypeDesc(name, FTYPEDESC_SAVE, MatReadFunc));
		}
		

		protected abstract void CreateDataMaps();
	}
}