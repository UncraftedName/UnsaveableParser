using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps {
	
	public static class GlobalDataMapGenerator {

		// Any generators included here will be used to create the full global datamap list. I originally used
		// reflection to get all the types, but I figured a direct approach might be better.
		private static readonly DataMapGenerator[] GeneratorInstances = {
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
			new VPhysicsMaps()
		};

		private static readonly Dictionary<Game, Dictionary<string, DataMap>> GeneratedGlobalMaps
			= new Dictionary<Game, Dictionary<string, DataMap>>();


		[MethodImpl(MethodImplOptions.Synchronized)]
		public static IReadOnlyDictionary<string, DataMap> GetDataMapList(SaveInfo info) {
			// check if the datamaps have already been generated for a previous save file
			if (!GeneratedGlobalMaps.TryGetValue(info.Game, out var res)) {
				res = GenerateMaps(info);
				GeneratedGlobalMaps.Add(info.Game, res);
			}
			return res;
		}


		private static Dictionary<string, DataMap> GenerateMaps(SaveInfo info) {
			
			Dictionary<string, DataMap> globalMaps = new Dictionary<string, DataMap>();
			
			List<(string, DataMap)> unlinkedBaseClasses = new List<(string baseClass, DataMap map)>();
			List<(string, TypeDesc)> unlinkedEmbeddedMaps = new List<(string mapName, TypeDesc desc)>();
			Dictionary<string, string> proxies = new Dictionary<string, string>();
			HashSet<string> emptyRoots = new HashSet<string>();
			
			foreach (DataMapGenerator gen in GeneratorInstances) {
				gen.SaveInfo = info;
				gen.Generate();
					
				emptyRoots.UnionWith(gen.EmptyRoots);
					
				// iterate through all the generator datamaps and add them to the global dict
				foreach (DataMap map in gen.DataMaps) {
					try {
						globalMaps.Add(map.Name, map);
					} catch (Exception e) {
						throw new ConstraintException($"entry already exists for datamap: \"{map.Name}\"", e);
					}
				}

				// If any maps/fields reference other classes (like embedded fields), just add it to a collection
				// and don't do anything else for now - these will be resolved later.
				foreach ((string baseClass, DataMap map) in gen.UnlinkedBaseClasses)
					unlinkedBaseClasses.Add((baseClass, map));

				foreach ((string embeddedMapName, TypeDesc desc) in gen.UnlinkedEmbeddedMaps)
					unlinkedEmbeddedMaps.Add((embeddedMapName, desc));

				foreach ((string linkName, string mapName) in gen.Links) // a link behaves the same as a proxy here
					gen.Proxies.Add(linkName, mapName);

				// If there exists any proxies/links A->B->C->D, then create a direct proxy A->D.
				// For example, consider the datamaps with this inheritance pattern - A:B, B:C, C:D, & D:E.
				// But imagine that A, B, and C are just proxies. So when you look for A or B, you really mean D.
				foreach ((string name, string baseClass) in gen.Proxies) {
					string b = baseClass;
					while (gen.Proxies.TryGetValue(b, out string? actualBase) || proxies.TryGetValue(b, out actualBase))
						b = actualBase;
					proxies[name] = b;
				}

				// go through all proxies, if the value does not exist in the global datamaps then add it
				foreach ((string key, string value) in proxies) {
					if (globalMaps.TryGetValue(key, out DataMap? existingMap)) {
						if (existingMap.Name != value) {
							throw new ConstraintException($"\"{key}\" already present in as \"{existingMap.Name}\"" +
														  $", tried to overwrite with \"{value}\"");
						}
						continue;
					}
					globalMaps.Add(key, globalMaps[value]);
				}
			}
			
			// now that we have all the datamaps, we can resolve the references for base classes and embedded maps
			
			foreach ((string baseClass, DataMap map) in unlinkedBaseClasses) {
				try {
					string actual = proxies.GetValueOrDefault(baseClass, baseClass)!;
					if (!emptyRoots.Contains(actual))
						map.BaseMap = globalMaps[actual];
				} catch (Exception e) {
					throw new Exception($"no base class called \"{baseClass}\" found", e);
				}
			}

			foreach ((string embeddedMapName, TypeDesc desc) in unlinkedEmbeddedMaps) {
				try {
					string actual = proxies.GetValueOrDefault(embeddedMapName, embeddedMapName)!;
					desc.EmbeddedMap = globalMaps[actual];
				} catch (Exception e) {
					throw new Exception($"no map for embedded field \"{desc}\" called \"{embeddedMapName}\"", e);
				}
			}

			return globalMaps;
		}
	}
}