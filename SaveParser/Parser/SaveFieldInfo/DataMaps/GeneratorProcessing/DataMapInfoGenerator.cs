using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.SaveFieldInfo.DescFlags;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing {
	
	/// <summary>
	/// A class that calls a bunch of pre-determined methods that describe the structure of in-game datamaps.
	/// </summary>
	public abstract class DataMapInfoGenerator {

		private IDataMapInfoGeneratorHandler _handler;
		protected DataMapGeneratorInfo GenInfo => _handler.GenInfo;
		protected Game Game => GenInfo.Game;


		public void GenerateWithHandler(IDataMapInfoGeneratorHandler handler) {
			lock (handler) {
				_handler = handler;
				GenerateDataMaps();
			}
		}


		protected abstract void GenerateDataMaps();


		protected void BeginDataMap(string dataMapName, string? baseName = null)
			=> BeginTemplatedMap(dataMapName, null, baseName, null);


		protected void DeclareTemplatedClass(string className, string? dataMapName = null)
			=> _handler.DeclareTemplatedClass(className, dataMapName ?? className);


		protected void BeginTemplatedMap(string name, string? templateType, string? baseName, string? baseTemplateType) {
			_handler.BeginDataMap(name, templateType, baseName, baseTemplateType);
		}


		protected void DataMapProxy(string name, string baseClass)
			=> _handler.DataMapProxy(name, null, baseClass, null);


		protected void DataMapProxyToTemplated(string name, string? templateType, string baseName, string? baseTemplateType)
			=> _handler.DataMapProxy(name, templateType, baseName, baseTemplateType);


		// sets up proxies for the most recent map/proxy
		protected void LinkNamesToMap(params string[] proxies)
			=> _handler.LinkNamesToMap(proxies);


		protected void LinkedNamesToOtherMap(string mapName, params string[] proxies)
			=> _handler.LinkedNamesToOtherMap(mapName, proxies);


		protected void DefineRootClassNoMap(string className, string? templateName = null)
			=> _handler.DefineRootClassNoMap(className, templateName);


		protected void DefineField(string name, FieldType fieldType, ushort count = 1)
			=> _handler.DefineField(name, fieldType, count);


		protected void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1)
			=> _handler.DefineInput(name, inputName, fieldType, count);


		// 1 or more events that fire when a condition is met (trigger touched, button pressed, etc.)
		protected void DefineOutput(string name, string outputName)
			=> _handler.DefineOutput(name, outputName);


		protected void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> _handler.DefineKeyField(name, mapName, fieldType, count);


		protected void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType,
			ushort count = 1)
			=> _handler.DefineInputAndKeyField(name, mapName, inputName, fieldType);


		protected void DefineGlobalField(string name, FieldType fieldType, ushort count = 1)
			=> _handler.DefineField(name, fieldType, count, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL);


		protected void DefineGlobalKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> _handler.DefineKeyField(name, mapName, fieldType, count, FTYPEDESC_SAVE | FTYPEDESC_KEY | FTYPEDESC_GLOBAL);


		// probably not relevant for save files - a function that can be fired
		protected void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType)
			=> _handler.DefineInputFunc(inputName, inputFunc, fieldType);


		protected void DefineFunction(string name)
			=> _handler.DefineFunction(name, FunctionType.NORMAL);


		protected void DefineThinkFunc(string name)
			=> _handler.DefineFunction(name, FunctionType.THINK);


		protected void DefineEntityFunc(string name)
			=> _handler.DefineFunction(name, FunctionType.ENTITY);


		protected void DefineUseFunc(string name)
			=> _handler.DefineFunction(name, FunctionType.USE);


		protected void DefineCustomField(
			string name,
			CustomReadFunc customReadFunc,
			object?[]? customParams = null,
			DescFlags flags = FTYPEDESC_SAVE)
			=> _handler.DefineCustomField(name, customReadFunc, customParams, flags);


		protected void DefineSoundPatch(string name)
			=> _handler.DefineCustomField(name, SoundPatch.Restore);


		// phys stuff is actually restored during the phys block handler, it's only queued for restore in the ents
		protected void DefinePhysPtr(string name)
			=> _handler.DefineCustomField(name, CPhysicsEnvironment.QueueRestore);
		

		protected void DefineMaterialIndexDataOps(string name) {
			static ParsedSaveField MatReadFunc(TypeDesc desc, SaveInfo info, ref ByteStreamReader bsr)
				=> new ParsedSaveField<MaterialIndexStr>((MaterialIndexStr)bsr.ReadStringOfLength(bsr.ReadSInt()), desc);
			DefineCustomField(name, MatReadFunc);
		}


		// an embedded field simply means that the field should be read like a data map (recursively)
		protected void DefineEmbeddedField(string name, string embeddedMap, ushort count = 1)
			=> _handler.DefineEmbeddedField(name, embeddedMap, count);


		// vector of embedded fields
		protected void DefineVector(string name, string elementType, DescFlags vecFlags = FTYPEDESC_SAVE)
			=> _handler.DefineVector(name, elementType, vecFlags);
		

		// vector of fields
		protected void DefineVector(
			string name,
			FieldType elemFieldType,
			DescFlags vecFlags = FTYPEDESC_SAVE,
			CustomReadFunc? elemReadFunc = null)
			=> _handler.DefineVector(name, elemFieldType, vecFlags, elemReadFunc);


		protected void DefineUtilMap(
			string name,
			FieldType keyType,
			FieldType valType,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? keyReadFunc = null,
			CustomReadFunc? valReadFunc = null)
			=> _handler.DefineUtilMap(name, keyType, valType, null, null, utlMapFlags, 
				keyReadFunc, valReadFunc);


		protected void DefineUtilMap(
			string name,
			string embeddedKeyName,
			FieldType valType,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? valReadFunc = null)
			=> _handler.DefineUtilMap(name, EMBEDDED, valType, embeddedKeyName, null, utlMapFlags, 
				null, valReadFunc);


		protected void DefineUtilMap(
			string name,
			FieldType keyType,
			string embeddedValName,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? keyReadFunc = null) 
			=> _handler.DefineUtilMap(name, keyType, EMBEDDED, null, embeddedValName, utlMapFlags, 
				keyReadFunc, null);


		protected void DefineUtilMap(
			string name,
			string embeddedKeyName,
			string embeddedValName,
			DescFlags utlMapFlags = FTYPEDESC_SAVE)
			=> _handler.DefineUtilMap(name, EMBEDDED, EMBEDDED, embeddedKeyName, embeddedValName, 
				utlMapFlags, null, null);

		
		protected void DefinePlaceholderEmbeddedField(string name) {
#if DEBUG
			_handler.DefinePlaceholderEmbeddedField(name);
#endif
		}
	}
}