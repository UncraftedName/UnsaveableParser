using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo.DataMaps.Generators;
using static SaveParser.Parser.SaveFieldInfo.DescFlags;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing {
	
	/// <summary>
	/// An IDataMapInfoGeneratorHandler will be called by a DataMapInfoGenerator to create some sort of data structure
	/// containing info about the datamaps used in games or to examine information about them.
	/// </summary>
	public interface IDataMapInfoGeneratorHandler {
		
		/// <summary>
		/// The default list of info generators to iterate over for a given handler.
		/// </summary>
		public static readonly IEnumerable<DataMapInfoGenerator> DefaultInfoGenerators = new DataMapInfoGenerator[] {
			new AI_Maps(),
			new EntMaps1(),
			new EntMaps2(),
			new EntMaps3(),
			new EntMaps4(),
			new EntMaps5(),
			new EnvMaps(),
			new LogicMaps(),
			new PhysConstraintMaps(),
			new StateFileDataMaps(),
			new TriggerMaps(),
			new VPhysicsMaps(),
			new VGuiMaps(),
			new P2SpecificMaps()
		};
		
		
		/// <summary>
		/// Iterates over all provided generators with the given handler.
		/// </summary>
		/// <param name="handler">The handler to run the generator on.</param>
		/// <param name="generatorInstances">Optionally provided generators; if null, the default list is used.
		/// Order does not matter.</param>
		public static void IterateAllGenerators(IDataMapInfoGeneratorHandler handler,
			IEnumerable<DataMapInfoGenerator>? generatorInstances = null)
		{
			generatorInstances ??= DefaultInfoGenerators;
			foreach (DataMapInfoGenerator infoGenerator in generatorInstances)
				infoGenerator.GenerateWithHandler(handler);
			handler.OnFinishedIterationOfInfoGenerators();
		}
		
		
		public DataMapGeneratorInfo GenInfo {get;}


		// there are no more maps, this method should be where all the actual structure creation or whatever is done
		public void OnFinishedIterationOfInfoGenerators();
		
		
		// declares that there exists some datamaps for class "className<T>" with name "dataMapName"
		public void DeclareTemplatedClass(string className, string dataMapName);
		
		
		// A:B, A:B<T>, A<T>:B, or A<T1>:B<T2>
		// There exists datamaps for templated classes. However, even valve haven't automated the process of creating
		// datamaps for these, and they are coded in manually. With the way that it was coded in, you can have some
		// class "ClassName<T>" but the datamap will just have some name "CustomName" for all types "T". A consequence
		// of the fact that I use dictionaries to store all datamaps is that I cannot have multiple maps with the same
		// name. Therefore, you cannot look up a datamap with the name "CustomName", you have to lookup "ClassName<T>".
		// The methods used in the rest of the class do not explicitly accept templated types, as that would make things
		// too complicated for my small brain. If I have to implement that at some point, there will be lots of crying.
		public void BeginDataMap(string name, string? templateType, string? baseName, string? baseTemplateType);


		// Same as above, but name & templateType are just a proxy to some other map.
		// Suppose C : B & B : A, but B is a class without any fields. In the code you can still see that B exists, but
		// valve might not have even made a datamap for it (since it has no fields). So this is a way to effectively
		// create a "blank" class that will be skipped over while parsing maps recursively. Except there won't actually
		// be a map - any references to this class and will be changed to be straight to the base class.
		// (Maps with no fields can still exist, this is more of a macro).
		public void DataMapProxy(string name, string? templateType, string baseName, string? baseTemplateType);


		// similar to a proxy, except that proxies are are actual classes in the game code, this is just "another name"
		public void LinkNamesToMap(params string[] proxies);
		
		
		// link names to any map, not just the current one
		public void LinkedNamesToOtherMap(string mapName, string[] proxies);


		// use if a class with a datamap inherits from one without
		public void DefineRootClassNoMap(string className, string? templateName);


		public void DefineField(string name, FieldType fieldType, ushort count = 1, DescFlags flags = FTYPEDESC_SAVE);


		public void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1);


		// 1 or more events that fire when a condition is met (trigger touched, button pressed, etc.)
		public void DefineOutput(string name, string outputName);


		public void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1,
			DescFlags flags = FTYPEDESC_SAVE);
		
		
		// some fields have a key and a input... I can't handle them separately so here they are together
		public void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType,
			ushort count = 1);
		
		
		// probably not relevant for save files - a function that can be fired
		public void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType);
		
		
		// not sure what these are for, a "normal" type is a name of a function, the others I haven't seen in save files
		public void DefineFunction(string name, FunctionType functionType);


		public void DefineCustomField(string name, CustomReadFunc customReadFunc, object?[]? customParams = null,
			DescFlags flags = FTYPEDESC_SAVE);


		// an object within an object - points to another datamap and reads it recursively
		public void DefineEmbeddedField(string name, string embeddedMap, ushort count = 1);


		// vector of embedded fields
		public void DefineVector(string name, string elementType, DescFlags vecFlags = FTYPEDESC_SAVE);


		// vector of fields
		public void DefineVector(
			string name,
			FieldType elemFieldType,
			DescFlags vecFlags = FTYPEDESC_SAVE,
			CustomReadFunc? elemReadFunc = null);


		public void DefineUtilMap(
			string name,
			FieldType keyType,
			FieldType valType,
			string? embeddedKeyName,
			string? embeddedValName,
			DescFlags utlMapFlags,
			CustomReadFunc? keyReadFunc,
			CustomReadFunc? valReadFunc);
		
		
		// use when you don't know the embedded type, should only be used for debugging
		public void DefinePlaceholderEmbeddedField(string name);
	}
}