using System.Data;
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using static SaveParser.Parser.SaveFieldInfo.DescFlags;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing {
	
	/// <summary>
	/// A class that calls a bunch of pre-determined methods that describe the structure of in-game datamaps.
	/// </summary>
	public abstract class DataMapInfoGenerator {
		
		public IDataMapInfoGeneratorHandler Handler {get;private set;}
		protected DataMapGeneratorInfo GenInfo => Handler.GenInfo;
		protected Game Game => GenInfo.Game;


		public void GenerateWithHandler(IDataMapInfoGeneratorHandler handler) {
			if (!handler.GetType().IsClass)
				throw new ConstraintException($"{nameof(handler)} must be a reference type");
			Handler = handler;
			GenerateDataMaps();
		}


		protected abstract void GenerateDataMaps();


		protected void BeginDataMap(string className, string? baseClass = null)
			=> Handler.BeginDataMap(className, baseClass);


		protected void DataMapProxy(string name, string baseClass)
			=> Handler.DataMapProxy(name, baseClass);


		protected void LinkNamesToMap(params string[] proxies)
			=> Handler.LinkNamesToMap(proxies);


		protected void DefineRootClassNoMap(string name)
			=> Handler.DefineRootClassNoMap(name);


		protected void DefineField(string name, FieldType fieldType, ushort count = 1)
			=> Handler.DefineField(name, fieldType, count);


		protected void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1)
			=> Handler.DefineInput(name, inputName, fieldType, count);


		protected void DefineOutput(string name, string outputName)
			=> Handler.DefineOutput(name, outputName);


		protected void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> Handler.DefineKeyField(name, mapName, fieldType, count);


		protected void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType,
			ushort count = 1)
			=> Handler.DefineInputAndKeyField(name, mapName, inputName, fieldType);


		protected void DefineGlobalField(string name, FieldType fieldType, ushort count = 1)
			=> Handler.DefineField(name, fieldType, count, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL);


		protected void DefineGlobalKeyField(string name, string mapName, FieldType fieldType, ushort count = 1)
			=> Handler.DefineKeyField(name, mapName, fieldType, count, FTYPEDESC_SAVE | FTYPEDESC_GLOBAL);


		protected void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType)
			=> Handler.DefineInputFunc(inputName, inputFunc, fieldType);


		protected void DefineFunction(string name)
			=> Handler.DefineFunction(name, FunctionType.NORMAL);


		protected void DefineThinkFunc(string name)
			=> Handler.DefineFunction(name, FunctionType.THINK);


		protected void DefineEntityFunc(string name)
			=> Handler.DefineFunction(name, FunctionType.ENTITY);


		protected void DefineUseFunc(string name)
			=> Handler.DefineFunction(name, FunctionType.USE);


		protected void DefineCustomField(
			string name,
			CustomReadFunc customReadFunc,
			object?[]? customParams = null,
			DescFlags flags = FTYPEDESC_SAVE)
			=> Handler.DefineCustomField(name, customReadFunc, customParams, flags);


		protected void DefineSoundPatch(string name)
			=> Handler.DefineCustomField(name, SoundPatch.Restore);


		// phys stuff is actually restored during the phys block handler, it's only queued for restore in the ents
		protected void DefinePhysPtr(string name)
			=> Handler.DefineCustomField(name, CPhysicsEnvironment.QueueRestore);


		// an embedded field simply means that the field should be read like a data map (recursively)
		protected void DefineEmbeddedField(string name, string embeddedMap, ushort count = 1)
			=> Handler.DefineEmbeddedField(name, embeddedMap, count);


		protected void DefineVector(string name, string elementType, DescFlags vecFlags = FTYPEDESC_SAVE)
			=> Handler.DefineVector(name, elementType, vecFlags);
		

		protected void DefineVector(
			string name,
			FieldType elemFieldType,
			DescFlags vecFlags = FTYPEDESC_SAVE,
			CustomReadFunc? elemReadFunc = null)
			=> Handler.DefineVector(name, elemFieldType, vecFlags, elemReadFunc);


		protected void DefineUtilMap(
			string name,
			FieldType keyType,
			FieldType valType,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? keyReadFunc = null,
			CustomReadFunc? valReadFunc = null)
			=> Handler.DefineUtilMap(name, keyType, valType, null, null, utlMapFlags, 
				keyReadFunc, valReadFunc);


		protected void DefineUtilMap(
			string name,
			string embeddedKeyName,
			FieldType valType,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? valReadFunc = null)
			=> Handler.DefineUtilMap(name, EMBEDDED, valType, embeddedKeyName, null, utlMapFlags, 
				null, valReadFunc);


		protected void DefineUtilMap(
			string name,
			FieldType keyType,
			string embeddedValName,
			DescFlags utlMapFlags = FTYPEDESC_SAVE,
			CustomReadFunc? keyReadFunc = null) 
			=> Handler.DefineUtilMap(name, keyType, EMBEDDED, null, embeddedValName, utlMapFlags, 
				keyReadFunc, null);


		protected void DefineUtilMap(
			string name,
			string embeddedKeyName,
			string embeddedValName,
			DescFlags utlMapFlags = FTYPEDESC_SAVE)
			=> Handler.DefineUtilMap(name, EMBEDDED, EMBEDDED, embeddedKeyName, embeddedValName, 
				utlMapFlags, null, null);

		
		protected void DefinePlaceholderEmbeddedField(string name) {
#if DEBUG
			Handler.DefinePlaceholderEmbeddedField(name);
#endif
		}
	}
}