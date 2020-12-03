using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SaveParser.Utils.ByteStreams {
	
	public partial struct ByteStreamReader {
		
		private readonly byte[] _data;
		public readonly IReadOnlyList<byte> Data => _data;
		public readonly int Size;
		public readonly int Start;
		public int AbsoluteByteIndex {get;private set;}
		public int CurrentByteIndex {
			readonly get => AbsoluteByteIndex - Start;
			set => AbsoluteByteIndex = Start + value;
		}
		public readonly int BytesRemaining => Start + Size - AbsoluteByteIndex;


		public ByteStreamReader(byte[] data) : this(data, data.Length, 0) {}


		public ByteStreamReader(byte[] data, int size, int start) {
			_data = data;
			Size = size;
			AbsoluteByteIndex = Start = start;
		}


		public ByteStreamReader SplitAndSkip(int bytes) {
			ByteStreamReader ret = Split(bytes);
			AbsoluteByteIndex += bytes;
			return ret;
		}

		
		public readonly ByteStreamReader Split() => Split(CurrentByteIndex, BytesRemaining);
		

		public readonly ByteStreamReader Split(uint bitLength) => Split((int)bitLength);
		
		
		public readonly ByteStreamReader Split(int bitLength) => Split(CurrentByteIndex, bitLength);
		
		
		public readonly ByteStreamReader Split(int newStart, int byteCount) {
			if (byteCount < 0)
				throw new ArgumentOutOfRangeException(nameof(byteCount), $"{nameof(byteCount)} cannot be less than 0");
			if (newStart + byteCount > CurrentByteIndex + BytesRemaining)
				throw new ArgumentOutOfRangeException(nameof(byteCount),
					$"{BytesRemaining} bytes remaining, attempted to create a substream with {newStart + byteCount - BytesRemaining} too many bytes");
			return new ByteStreamReader(_data, byteCount, Start + newStart);
		}
		

		public readonly ByteStreamReader FromBeginning() {
			return new ByteStreamReader(_data, Size, Start);
		}


		public override string ToString() {
			return $"{{start: {Start}, size: {Size}, abs: {AbsoluteByteIndex}, rel: {CurrentByteIndex}}}";
		}
		
		
		public void ReadStruct<T>(out T @struct, int size) where T : struct {
			Span<byte> span = stackalloc byte[size];
			ReadBytesToSpan(span);
			ParserUtils.ByteSpanToStruct(out @struct, span);
		}


		public void SkipBytes(uint byteCount) => SkipBytes((int)byteCount);


		public void SkipBytes(int byteCount) {
			EnsureCapacity(byteCount);
			AbsoluteByteIndex += byteCount;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void EnsureCapacity(int byteCount) {
			if (byteCount > BytesRemaining)
				throw new ArgumentOutOfRangeException(nameof(byteCount),
					$"{nameof(EnsureCapacity)} failed - {byteCount} bytes were needed but only {BytesRemaining} were left");
		}


		public bool ReadBool() {
			EnsureCapacity(1);
			return _data[AbsoluteByteIndex++] != 0;
		}
		

		public byte ReadByte() {
			EnsureCapacity(1);
			return _data[AbsoluteByteIndex++];
		}


		public byte PeekByte() {
			EnsureCapacity(1);
			return _data[AbsoluteByteIndex];
		}


		public sbyte ReadSByte() => (sbyte)ReadByte();


		public byte[] ReadBytes(int byteCount) {
			byte[] result = new byte[byteCount];
			ReadBytesToSpan(result);
			return result;
		}


		public void ReadBytesToSpan(Span<byte> byteSpan) {
			EnsureCapacity(byteSpan.Length);
			_data.AsSpan(AbsoluteByteIndex, byteSpan.Length).CopyTo(byteSpan);
			AbsoluteByteIndex += byteSpan.Length;
		}


		public IEnumerable<string> ReadNullSeparatedStrings(int byteLen) {
			var res = new LinkedList<string>();
			int limit = AbsoluteByteIndex + byteLen;
			while (AbsoluteByteIndex < limit) {
				if (ReadByte() != 0) {
					AbsoluteByteIndex--;
					res.AddLast(ReadNullTerminatedString());
				}
			}
			return res;
		}


		public (byte[] bytes, int byteCount) ReadRemainingBytes() {
			int bytesRemaining = BytesRemaining;
			return (ReadBytes(bytesRemaining), bytesRemaining);
		}


		public unsafe string ReadNullTerminatedString() {
			string s;
			fixed (byte* bytePtr = _data)
				s = new string((sbyte*)&bytePtr[AbsoluteByteIndex]);
			AbsoluteByteIndex += s.Length + 1;
			return s;
		}


		public string ReadStringOfLength(int strLength) {
			if (strLength < 0)
				throw new ArgumentException("bro that's not supposed to be negative", nameof(strLength));
			
			Span<byte> bytes = strLength < 1000 
				? stackalloc byte[strLength + 1] 
				: new byte[strLength + 1];
			
			ReadBytesToSpan(bytes.Slice(0, strLength));
			bytes[strLength] = 0; // I would assume that in this case the string might not be null-terminated.
			unsafe {
				fixed(byte* strPtr = bytes)
					return new string((sbyte*)strPtr);
			}
		}


		public string ReadStringOfLength(uint strLength) => ReadStringOfLength((int)strLength);


		public CharArray ReadCharArray(int byteCount) {
			return new CharArray(ReadBytes(byteCount));
		}


		public string?[]? ReadSymbolTable(int tokenCount, int byteSize) {
			if (byteSize <= 0)
				return null;
			int tmp = CurrentByteIndex;
			string?[] tokens = new string?[tokenCount];
			for (int i = 0; i < tokenCount; i++) {
				if (ReadByte() != 0) {
					AbsoluteByteIndex--;
					tokens[i] = ReadNullTerminatedString();
				}
			}
			if (tmp + byteSize != CurrentByteIndex)
				throw new OverflowException($"the token list did not read the correct amount of bytes, {CurrentByteIndex - tmp - byteSize} bytes off");
			return tokens;
		}
		
		// I need to write these manually because I can't do Func<Span<byte>, T> cuz Span is a ref struct.


		private static void AssertCorrectByteAlignment() => Debug.Assert(BitConverter.IsLittleEndian);
		

		public int ReadSInt() => (int)ReadUInt();
		
		
		public uint ReadUInt() {
			AssertCorrectByteAlignment();
			var tmp = BitConverter.ToUInt32(_data, AbsoluteByteIndex);
			AbsoluteByteIndex += sizeof(uint);
			return tmp;
		}


		public short ReadSShort() => (short)ReadUShort();


		public ushort ReadUShort() {
			AssertCorrectByteAlignment();
			var tmp = BitConverter.ToUInt16(_data, AbsoluteByteIndex);
			AbsoluteByteIndex += sizeof(ushort);
			return tmp;
		}


		public float ReadFloat() {
			AssertCorrectByteAlignment();
			var tmp = BitConverter.ToSingle(_data, AbsoluteByteIndex);
			AbsoluteByteIndex += sizeof(float);
			return tmp;
		}


		public void ReadVector3(out Vector3 vec3) {
			vec3.X = ReadFloat();
			vec3.Y = ReadFloat();
			vec3.Z = ReadFloat();
		}


		private float[,] ReadFloatMat(int rows, int cols) {
			float[,] mat = new float[rows,cols];
			for (int r = 0; r < rows; r++)
			for (int c = 0; c < cols; c++)
				mat[r, c] = ReadFloat();
			return mat;
		}
	}
}