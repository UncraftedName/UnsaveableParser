using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SaveParser.Utils;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing {
	
	/// <summary>
	/// Can be used to generate a datamap hierarchy of the classes/structs that the game uses.
	/// </summary>
	/// <example>
	/// <code>
	/// DataMapHierarchyGenerator hierarchyGenerator = new DataMapHierarchyGenerator(new DataMapGeneratorInfo(Game.PORTAL1_3420, false));
	/// IDataMapInfoGeneratorHandler.IterateAllGenerators(hierarchyGenerator);
	/// Console.WriteLine(hierarchyGenerator.ToString("| "));
	/// </code>
	/// </example>
	public class DataMapHierarchyGenerator : PrettyClass, IDataMapInfoGeneratorHandler {

		private class LowerStringComparer : IComparer<string>, IEqualityComparer<string> {
			
			public int Compare(string? x, string? y) {
				Debug.Assert(x != null && y != null);
				return string.Compare(x.ToLower(), y.ToLower(), StringComparison.Ordinal);
			}


			public bool Equals(string? x, string? y) {
				return Compare(x, y) == 0;
			}


			public int GetHashCode(string obj) {
				return obj.ToLower().GetHashCode();
			}
		}
		

		public class TreeNode : PrettyClass {
			
			public readonly string Name;
			public readonly bool HasDataMap;
			public readonly SortedSet<string> LinkedNames;
			public readonly SortedDictionary<string, TreeNode> Children;
			
			public TreeNode(string name, bool hasDataMap, SortedSet<string> linkedNames) {
				Name = name;
				HasDataMap = hasDataMap;
				LinkedNames = linkedNames;
				Children = new SortedDictionary<string, TreeNode>(new LowerStringComparer());
			}


			public override void PrettyWrite(IPrettyWriter iw) {
				iw.Append(HasDataMap ? "" : "*");
				iw.Append(Name);
				if (LinkedNames.Any())
					iw.Append(" " + LinkedNames.SequenceToString(start: "(", end: ")"));
				if (Children.Any())
					EnumerablePrettyWriteHelper(Children.Values, iw);
			}
		}


		private class ClassNode {
			
			public readonly string Name;
			public readonly string? BaseClass;
			public readonly bool HasDataMap;
			public readonly SortedSet<string> LinkedNames;
			
			public ClassNode(string name, string? baseClass, bool hasDataMap) {
				Name = name;
				BaseClass = baseClass;
				HasDataMap = hasDataMap;
				LinkedNames = new SortedSet<string>(new LowerStringComparer());
			}


			public override string ToString() {
				return Name;
			}
		}
		
		
		public DataMapGeneratorInfo GenInfo {get;}
		public SortedDictionary<string, TreeNode> Roots;
		private readonly Dictionary<string, ClassNode> _classNodes;


		public DataMapHierarchyGenerator(DataMapGeneratorInfo genInfo) {
			GenInfo = genInfo;
			_classNodes = new Dictionary<string, ClassNode>();
		}


		private TreeNode IterateAndAddNodes(ClassNode classNode) {
			SortedDictionary<string, TreeNode> insertionLevel = classNode.BaseClass == null
				? Roots : IterateAndAddNodes(_classNodes[classNode.BaseClass]).Children;
			
			if (insertionLevel.TryGetValue(classNode.Name, out TreeNode? treeNode))
				return treeNode;
			treeNode = new TreeNode(classNode.Name, classNode.HasDataMap, classNode.LinkedNames);
			insertionLevel[classNode.Name] = treeNode;
			return treeNode;
		}


		public void OnFinishedIterationOfInfoGenerators() {
			Roots = new SortedDictionary<string, TreeNode>(new LowerStringComparer());
			foreach (ClassNode classNode in _classNodes.Values)
				IterateAndAddNodes(classNode);
		}


		public void BeginDataMap(string className, string? baseClass = null) {
			_classNodes.Add(className, new ClassNode(className, baseClass, true));
		}


		public void DataMapProxy(string name, string baseClass) {
			_classNodes.Add(name, new ClassNode(name, baseClass, false));
		}


		public void LinkNamesToMap(params string[] proxies) {
			foreach (string proxy in proxies)
				_classNodes.Last().Value.LinkedNames.Add(proxy);
		}


		public void DefineRootClassNoMap(string name) {
			DataMapProxy(name, null!);
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			int i = 0;
			foreach (TreeNode treeNode in Roots.Values) {
				if (i++ != 0)
					iw.AppendLine();
				treeNode.PrettyWrite(iw);
			}
		}


		// do nothing for fields
		
		
		public void DefineField(string name, FieldType fieldType, ushort count = 1, DescFlags flags = DescFlags.FTYPEDESC_SAVE) {}
		public void DefineInput(string name, string inputName, FieldType fieldType, ushort count = 1) {}
		public void DefineOutput(string name, string outputName) {}
		public void DefineKeyField(string name, string mapName, FieldType fieldType, ushort count = 1, DescFlags flags = DescFlags.FTYPEDESC_SAVE) {}
		public void DefineInputAndKeyField(string name, string mapName, string inputName, FieldType fieldType, ushort count = 1) {}
		public void DefineInputFunc(string inputName, string inputFunc, FieldType fieldType) {}
		public void DefineFunction(string name, FunctionType functionType) {}
		public void DefineCustomField(string name, CustomReadFunc customReadFunc, object?[]? customParams = null, DescFlags flags = DescFlags.FTYPEDESC_SAVE) {}
		public void DefineEmbeddedField(string name, string embeddedMap, ushort count = 1) {}
		public void DefineVector(string name, string elementType, DescFlags vecFlags = DescFlags.FTYPEDESC_SAVE) {}
		public void DefineVector(string name, FieldType elemFieldType, DescFlags vecFlags = DescFlags.FTYPEDESC_SAVE, CustomReadFunc? elemReadFunc = null) {}
		public void DefineUtilMap(string name, FieldType keyType, FieldType valType, string? embeddedKeyName, string? embeddedValName, DescFlags utlMapFlags, CustomReadFunc? keyReadFunc, CustomReadFunc? valReadFunc) {}
		public void DefinePlaceholderEmbeddedField(string name) {}
	}
}