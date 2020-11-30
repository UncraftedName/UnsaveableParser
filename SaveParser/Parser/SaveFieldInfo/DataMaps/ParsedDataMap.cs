using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using SaveParser.Utils;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps {
	
	public class ParsedDataMap : AppendableClass {

		public readonly DataMap DataMap;
		private readonly OrderedDictionary<string, ParsedSaveField> _thisFields;
		private ParsedDataMap? _baseParsedMap;
		private OrderedDictionary<string, ParsedSaveField>? _combinedFields;
		public IReadOnlyDictionary<string, ParsedSaveField> ParsedFields => _combinedFields ?? _thisFields;

		private int _capacity;


		public ParsedDataMap(DataMap dataMap, int savedFields) {
			DataMap = dataMap;
			_capacity = savedFields;
			_thisFields = new OrderedDictionary<string, ParsedSaveField>();
		}


		public void AddSaveField(ParsedSaveField field) {
			if (_thisFields.Count == _capacity)
				throw new ConstraintException($"{nameof(ParsedDataMap)}: too many fields added ({_capacity})");
			if (!_thisFields.TryAdd(field.Desc.Name, field)) {
				/* Volvo has added two of some fields to the datadescs, so the same field will
				 * get read twice. Not a problem for valve because it actually sets the field in the struct/class, but
				 * a problem for me since I add the fields to a dict. So I just make sure the values of the duplicate
				 * fields are equal (which they always should be).
				 */
				if (Equals(_thisFields[field.Desc.Name].FieldAsObj, field.FieldAsObj))
					_capacity--; // we're not gonna be adding the same field again
				else
					throw new ConstraintException($"two duplicate keys with different values for key \"{field.Desc.Name}\"");
			}
		}


		private bool _baseAdded;
		
		public void AddBaseParsedMap(ParsedDataMap dataMap) {
			Debug.Assert(!_baseAdded);
			_baseAdded = true;
			_baseParsedMap = dataMap;
			_combinedFields = new OrderedDictionary<string, ParsedSaveField>((IOrderedDictionary<string, ParsedSaveField>)_baseParsedMap.ParsedFields);
			// here we have the same duplicate key problem as above, but now it's with A.field == B.field where A : B
			foreach (var (key, value) in _thisFields) {
				if (!_combinedFields.TryAdd(key, value) && !Equals(_combinedFields[key].FieldAsObj, value.FieldAsObj)) {
					// formatting is dumb, too bad!
					throw new ConstraintException($"duplicate field names, {DataMap.Name}::{{{_combinedFields[key].ToString()}}} does not match {dataMap.DataMap.Name}::{{{value.ToString()}}}");
				}
			}
		}
		
		public ParsedSaveField<T> GetField<T>(string name) {
			return (ParsedSaveField<T>)ParsedFields[name];
		}


		public T GetCustomTypeField<T>(string name) where T : ParsedSaveField {
			return (T)ParsedFields[name];
		}
		
		
		public bool TryGetField<T>(string name, out ParsedSaveField<T>? field) {
			if (ParsedFields.TryGetValue(name, out ParsedSaveField? tmp)) {
				field = (ParsedSaveField<T>)tmp;
				return true;
			}
			field = null;
			return false;
		}
		
		
		public bool TryGetCustomField<T>(string name, out T? field) where T : ParsedSaveField {
			if (ParsedFields.TryGetValue(name, out ParsedSaveField? tmp)) {
				field = (T)tmp;
				return true;
			}
			field = null;
			return false;
		}


		// if T is a ref type then you should be doing T? = GetValueOrDefault<T>
		public ParsedSaveField<T> GetFieldOrDefault<T>(string name) {
			if (ParsedFields.TryGetValue(name, out ParsedSaveField? saveField))
				return ((ParsedSaveField<T>)saveField)!;
			return new ParsedSaveField<T>(default!, DataMap.FieldDict.GetValueOrDefault(name)!);
		}


		public T GetCustomTypeFieldOrDefault<T>(string name) where T : ParsedSaveField, new() {
			if (ParsedFields.TryGetValue(name, out ParsedSaveField? saveField))
				return (T)saveField;
			return (T)Activator.CreateInstance(typeof(T), DataMap.FieldDict.GetValueOrDefault(name))!;
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{DataMap.Name}:");
			EnumerableAppendHelper(_thisFields.Values, iw);
			var nextBase = _baseParsedMap;
			while (nextBase != null && nextBase._thisFields.Count == 0)
				nextBase = nextBase._baseParsedMap;
			if (nextBase != null) {
				iw.Append("\nfields inherited from ");
				nextBase.AppendToWriter(iw);
			}
		}
	}
}