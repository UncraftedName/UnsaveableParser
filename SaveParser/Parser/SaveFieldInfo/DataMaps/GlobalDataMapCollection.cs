using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps {
	
	public static class GlobalDataMapCollection {
		
		public static readonly Dictionary<string, DataMap> MapsByName;


		static GlobalDataMapCollection() {
			MapsByName = new Dictionary<string, DataMap>();
			
			List<(string, DataMap)> unlinkedBaseClasses = new List<(string baseClass, DataMap map)>();
			List<(string, TypeDesc)> unlinkedEmbeddedMaps = new List<(string mapName, TypeDesc desc)>();
			Dictionary<string, string> proxies = new Dictionary<string, string>();
			HashSet<string> emptyRoots = new HashSet<string>();

			// iterate through all generators
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(DataMapGenerator).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
				.ToList()
				.ForEach(type => {
					// run the generator, this will populate its fields with datamap info
					DataMapGenerator gen = (DataMapGenerator)Activator.CreateInstance(type)!;
					gen.Generate();
					
					emptyRoots.UnionWith(gen.EmptyRoots);
					
					// iterate through all generators' datamaps and add them to dictionaries
					foreach (DataMap map in gen.DataMaps) {
						try {
							MapsByName.Add(map.Name, map);
						} catch (Exception) {
							Debug.WriteLine($"{nameof(GlobalDataMapCollection)}: entry already exists for map: \"{map.Name}\"");
							throw;
						}
					}

					// If any maps/fields reference other classes (like embedded fields), just add it to a collection
					// and don't do anything now.
					foreach ((string baseClass, DataMap map) in gen.UnlinkedBaseClasses)
						unlinkedBaseClasses.Add((baseClass, map));

					foreach ((string embeddedMapName, TypeDesc desc) in gen.UnlinkedEmbeddedMaps)
						unlinkedEmbeddedMaps.Add((embeddedMapName, desc));

					foreach ((string linkName, string mapName) in gen.Links) // a link behaves the same as a proxy
						gen.Proxies.Add(linkName, mapName);

					// if there exists any A->B->C->D, make A->D
					// since A, B, C, & D might all be in different classes, A->D is added to the 'global' proxies 
					foreach ((string name, string baseClass) in gen.Proxies) {
						string b = baseClass;
						while (gen.Proxies.TryGetValue(b, out string? actualBase) || proxies.TryGetValue(b, out actualBase))
							b = actualBase;
						proxies[name] = b;
					}

					// go through all proxies, if the value does not exist in the map collection then add it
					foreach ((string key, string value) in proxies) {
						if (MapsByName.TryGetValue(key, out DataMap? existingMap)) {
							if (existingMap.Name != value) {
								throw new ConstraintException(
									$"{nameof(GlobalDataMapCollection)}: \"{key}\" already present in dict as \"{existingMap.Name}\", tried to overwrite with \"{value}\"");
							}
							continue;
						}
						MapsByName.Add(key, MapsByName[value]);
					}
				});
			
			// now that we have a dictionary of all datamaps, we can resolve the references
			
			foreach ((string baseClass, DataMap map) in unlinkedBaseClasses) {
				try {
					string actual = proxies.GetValueOrDefault(baseClass, baseClass)!;
					if (!emptyRoots.Contains(actual))
						map.BaseMap = MapsByName[actual];
				} catch (Exception e) {
					throw new Exception($"{nameof(GlobalDataMapCollection)}: no base class called \"{baseClass}\" found", e);
				}
			}

			foreach ((string embeddedMapName, TypeDesc desc) in unlinkedEmbeddedMaps) {
				try {
					string actual = proxies.GetValueOrDefault(embeddedMapName, embeddedMapName)!;
					desc.EmbeddedMap = MapsByName[actual];
				} catch (Exception e) {
					throw new Exception($"{nameof(GlobalDataMapCollection)}: no class for embedded field \"{desc}\" called \"{embeddedMapName}\" found", e);
				}
			}
		}
	}
}