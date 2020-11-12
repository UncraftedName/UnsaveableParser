using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SaveParser.Utils {
	
	public static class ParserUtils {
		
		// https://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) {
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (T element in source)
				if (seenKeys.Add(keySelector(element)))
					yield return element;
		}
		
		
		// todo ugh this is fecking trash cuz of boxing
		public static unsafe void ByteSpanToStruct<T>(out T @struct, Span<byte> bytes) where T : struct {
			fixed (byte* ptr = bytes)
				@struct = Marshal.PtrToStructure<T>((IntPtr)ptr)!;
		}
		
		
		public class RefComparer<T> : IEqualityComparer<T> {
			public bool Equals([AllowNull]T x, [AllowNull]T y) => ReferenceEquals(x, y);
			public int GetHashCode([DisallowNull]T o) => RuntimeHelpers.GetHashCode(o!);
		}


		public static string ToString(this string?[] symbolTable) {
			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			bool first = true;
			foreach (string? t in symbolTable) {
				if (t != null) {
					if (!first) {
						first = false;
						sb.Append(", ");
					}
					sb.Append(t);
				}
			}
			sb.Append('}');
			return sb.ToString();
		}


		// regular "find" but starts at a given index and loops around if necessary
		public static T Find<T>(T[] arr, Predicate<T> predicate, ref int index) {
			foreach (T _ in arr) {
				if (predicate(arr[index]))
					return arr[index++];
				if (++index == arr.Length)
					index = 0;
			}
			return default!;
		}
		
		
		public static bool IsMethodCompatibleWithDelegate<T>(MethodInfo method) where T : class {
			Type delegateType = typeof(T);
			MethodInfo delegateSignature = delegateType.GetMethod("Invoke")!;

			if (!delegateSignature.ReturnType.IsAssignableFrom(method.ReturnType))
				return false;

			return delegateSignature
				.GetParameters()
				.Select(x => x.ParameterType)
				.SequenceEqual(method.GetParameters().Select(x => x.ParameterType));
		}
	}
}