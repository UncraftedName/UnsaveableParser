using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using SaveParser.Utils;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

// this will be removed, just here as a placeholder
namespace SaveParser.Parser {
	
	public static class Structs {

		public static void DefaultAppendToWriter<T>(this ref T str, IIndentedWriter iw) where T : struct {
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++) {
				iw.Append($"{fields[i].Name}: {fields[i].GetValue(str)}");
				if (i != fields.Length - 1)
					iw.AppendLine();
			}
		}


		public static void DefaultAppendToWriterOneLine<T>(this ref T str, IIndentedWriter iw) where T : struct {
			FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
			iw.Append("{");
			for (int i = 0; i < fields.Length; i++) {
				iw.Append($"{fields[i].Name}: {fields[i].GetValue(str)}");
				if (i != fields.Length - 1)
					iw.Append(", ");
			}
			iw.Append("}");
		}
		

		[StructLayout(LayoutKind.Explicit, Size = 32)]
		public struct BaseClientSections {
			
			[FieldOffset(0)]public int EntitySize;
			[FieldOffset(4)]public int HeaderSize;
			[FieldOffset(8)]public int DecalSize;
			[FieldOffset(12)]public int MusicSize;
			[FieldOffset(16)]public int SymbolSize;
			[FieldOffset(20)]public int DecalCount;
			[FieldOffset(24)]public int MusicCount;
			[FieldOffset(28)]public int SymbolCount;


			public int SumBytes() {
				return EntitySize + HeaderSize + DecalSize + SymbolSize + MusicSize;
			}
		}

		[StructLayout(LayoutKind.Explicit, Size = 8)]
		public readonly struct SaveFileHeaderTag : IEquatable<SaveFileHeaderTag> {
			
			public static readonly SaveFileHeaderTag CurrentSavefileHeaderTag = new SaveFileHeaderTag(1447119958, 0x073);
			
			[FieldOffset(0)]public readonly int Id;
			[FieldOffset(4)]public readonly int Version;


			public SaveFileHeaderTag(int id, int version) {
				Id = id;
				Version = version;
			}
			
			
			public override bool Equals(object? obj) {
				return obj is SaveFileHeaderTag other && Equals(other);
			}


			public bool Equals(SaveFileHeaderTag other) {
				return Id == other.Id && Version == other.Version;
			}


			public override int GetHashCode() {
				return HashCode.Combine(Id, Version);
			}


			public static bool operator ==(SaveFileHeaderTag left, SaveFileHeaderTag right) {
				return left.Equals(right);
			}


			public static bool operator !=(SaveFileHeaderTag left, SaveFileHeaderTag right) {
				return !left.Equals(right);
			}
		}
	}
}