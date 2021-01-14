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
		public static readonly IReadOnlyList<DataMapInfoGenerator> DefaultInfoGenerators = new DataMapInfoGenerator[] {
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
			new VGuiMaps()
		};
		
		
		/// <summary>
		/// Iterates over all provided generators with the given handler.
		/// </summary>
		/// <param name="handler">The handler to run the generator on.</param>
		/// <param name="generatorInstances">Optionally provided generators; if null, the default list is used. </param>
		public static void IterateAllGenerators(IDataMapInfoGeneratorHandler handler,
			IEnumerable<DataMapInfoGenerator>? generatorInstances = null)
		{
			generatorInstances ??= DefaultInfoGenerators;
			foreach (DataMapInfoGenerator infoGenerator in generatorInstances)
				infoGenerator.GenerateWithHandler(handler);
			handler.OnFinishedIterationOfInfoGenerators();
		}
		
		
		public DataMapGeneratorInfo GenInfo {get;}


		public void OnFinishedIterationOfInfoGenerators();
		

		// represents the beginning of a class/struct
		public void BeginDataMap(string className, string? baseClass = null);
		
		
		// Suppose C : B & B : A, but B is a class without any fields. In the code you can still see that B exists, but
		// valve might not have even made a datamap for it (since it has no fields). So this is a way to effectively
		// create a "blank" class that will be skipped dover while parsing maps recursively. Except there won't actually
		// be a map - I will simply find any references to this class and change them to be straight to the base class.
		// (Maps with no fields can still exist, this is more of a macro).
		public void DataMapProxy(string name, string baseClass);


		// any datamap names that refer to classes should be added here
		public void LinkNamesToMap(params string[] proxies);


		// use if a class with a datamap inherits from one without
		public void DefineRootClassNoMap(string name);


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