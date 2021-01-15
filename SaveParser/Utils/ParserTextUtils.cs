using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SaveParser.Parser;

namespace SaveParser.Utils {
	
	public static class ParserTextUtils {

		public static string BytesToBinaryString(IEnumerable<byte> bytes) {
			return string.Join(" ", bytes
				.Select(b => Convert.ToString(b, 2)).ToList()
				.Select(s => s.PadLeft(8, '0')));
		}


		public static string ByteToBinaryString(byte b, int bitCount = 8) {
			return Convert.ToString(b, 2).PadLeft(8, '0').Substring(0, bitCount);
		}


		public static string BytesToHexString(IEnumerable<byte> bytes, string separator = " ") {
			return string.Join(separator, bytes
				.Select(b => Convert.ToString(b, 16)).ToList()
				.Select(s => s.PadLeft(2, '0')));
		}


		// https://stackoverflow.com/questions/18781027/regex-camel-case-to-underscore-ignore-first-occurrence
		public static string CamelCaseToUnderscore(string str) {
			return string.Concat(str.Select((x,i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToUpper();
		}


		public static string CamelCaseToLowerSpaced(string str) {
			return string.Concat(str.Select((x,i) => i > 0 && char.IsUpper(x) ? " " + x : x.ToString())).ToLower();
		}


		/*public static string UnderscoreToCamelCase(string str) {
			string[] sections = str.Split('_');
			if (sections.Length == 1)
				return str;
			StringBuilder sb = new StringBuilder(str.Length);
			foreach (string section in sections) {
				sb.Append(section[0].ToString().ToUpper());
				sb.Append(section.Substring(1));
			}
			return sb.ToString();
		}*/


		public static byte[] StringAsByteArray(string s, int length) { // fills the unused indices with '\0'
			byte[] result = new byte[length];
			Array.Copy(Encoding.ASCII.GetBytes(s), result, s.Length);
			return result;
		}


		public static unsafe string ByteArrayAsString(byte[] bytes) { // cuts off all '\0' from the end of the array
			fixed (byte* bytePtr = bytes)
				return new string((sbyte*)bytePtr);
		}
		

		public static string SequenceToString(this IEnumerable enumerable, string separator = ", ", string start = "[", string end = "]") {
			StringBuilder builder = new StringBuilder(start);
			bool containsElements = false;
			foreach (var x in enumerable) {
				builder.Append($"{x}{separator}");
				containsElements = true;
			}
			if (containsElements)
				builder.Remove(builder.Length - separator.Length, separator.Length);
			builder.Append(end);
			return builder.ToString();
		}


		public static string SharedPathSubstring(string s1, string s2) {
			int furthestSlash = 0;
			for (int i = 0; i < s1.Length && i < s2.Length && s1[i] == s2[i]; i++) {
				if (s1[i] == '/' || s1[i] == '\\')
					furthestSlash = i;
			}
			return s1.Substring(0, furthestSlash);
		}


		internal static void WriteLineColored(this TextWriter tw, string message, ConsoleColor cc = ConsoleColor.Red) {
			if (tw != Console.Out)
				throw new ArgumentException("text writer was not Console.Out");
			var tmp = Console.ForegroundColor;
			Console.ForegroundColor = cc;
			tw.WriteLine(message);
			Console.ForegroundColor = tmp;
		}
		
		
		// base types come first
		private class TypeComparer : IComparer<Type?> {
			
			public static readonly TypeComparer Inst = new TypeComparer();
			
			public int Compare(Type? x, Type? y) {
				if (x == y || x == null || y == null)
					return 0;
				else if (x.IsSubclassOf(y))
					return 1;
				else if (y.IsSubclassOf(x))
					return -1;
				return 0;
			}
		}


		private static readonly Dictionary<Type, FieldInfo[]> FieldLookup = new Dictionary<Type, FieldInfo[]>();
		

		public static void DefaultPrettyWrite(object? obj, IPrettyWriter iw) {
			if (obj == null) {
				iw.Append("null");
				return;
			}
			Type t = obj.GetType();
			if (!FieldLookup.TryGetValue(t, out FieldInfo[]? fields)) {
				fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
					.Where(f => f.FieldType != typeof(SourceSave)) // prevent infinite recursion
					.OrderBy(f => f.DeclaringType, TypeComparer.Inst).ToArray();
				FieldLookup.Add(t, fields);
			}
			for (int i = 0; i < fields.Length; i++) {
				iw.Append($"{fields[i].Name}: ");
				var fieldObj = fields[i].GetValue(obj);
				if (fieldObj == null) {
					iw.Append("null");
				} else {
					switch (fieldObj) {
						case IPretty pretty:
							iw.FutureIndent++;
							iw.AppendLine();
							pretty.PrettyWrite(iw);
							iw.FutureIndent--;
							break;
						case string s:
							iw.Append('"' + s.Replace("\n", "\\n").Replace("\"", "\\\"") + '"');
							break;
						case IEnumerable enumerable:
							iw.Append(enumerable.SequenceToString());
							break;
						default:
							iw.Append(fieldObj.ToString());
							break;
					}
				}
				if (i != fields.Length - 1)
					iw.AppendLine();
			}
		}
	}
}