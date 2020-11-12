using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using SaveParser.Parser.SaveFieldInfo;

namespace SaveParser.Utils.BitStreams {
	
	public partial struct BitStreamReader {
		
		public readonly byte[] Data;
		public readonly int BitLength;
		public readonly int ByteLength => BitLength >> 3;
		public readonly int Start; // index of first readable bit
		public int AbsoluteBitIndex {get;private set;}
		private readonly int AbsoluteByteIndex => AbsoluteBitIndex >> 3;
		public int CurrentBitIndex {
			readonly get => AbsoluteBitIndex - Start;
			set => AbsoluteBitIndex = Start + value;
		}
		public int CurrentByteIndex {
			readonly get => CurrentBitIndex >> 3;
			set {
				if (!IsByteAligned)
					throw new ConstraintException("bad bit alignment");
				AbsoluteBitIndex = Start + (value << 3);
			}
		}

		public readonly int BitsRemaining => Start + BitLength - AbsoluteBitIndex;
		internal bool IsLittleEndian; // this doesn't work w/ big endian atm, probably won't try to fix it since it's not necessary
		private readonly byte CurrentByte => Data[AbsoluteByteIndex];  // same as Pointer / 8
		private readonly byte IndexInByte => (byte)(AbsoluteBitIndex & 0x07); // same as Pointer % 8
		private readonly byte RemainingBitMask => (byte)(0xff << IndexInByte); // mask to get remaining bits in this byte
		private readonly bool IsByteAligned => IndexInByte == 0;
		

		public BitStreamReader(byte[] data, bool isLittleEndian = true) : this(data, data.Length << 3, 0, isLittleEndian) {}


		public BitStreamReader(byte[] data, int bitLength, int start, bool isLittleEndian = true) {
			Data = data;
			BitLength = bitLength;
			AbsoluteBitIndex = Start = start;
			IsLittleEndian = isLittleEndian;
		}


		public BitStreamReader SplitAndSkip(int bits) {
			BitStreamReader ret = Split(bits);
			AbsoluteBitIndex += bits;
			return ret;
		}

		
		public readonly BitStreamReader Split() => Split(CurrentBitIndex, BitsRemaining);
		

		public readonly BitStreamReader Split(uint bitLength) => Split((int)bitLength);
		
		
		public readonly BitStreamReader Split(int bitLength) => Split(CurrentBitIndex, bitLength);
		
		
		public readonly BitStreamReader Split(int fromBitIndex, int bitCount) {
			if (bitCount < 0)
				throw new ArgumentOutOfRangeException(nameof(bitCount), $"{nameof(bitCount)} cannot be less than 0");
			if (fromBitIndex + bitCount > CurrentBitIndex + BitsRemaining)
				throw new ArgumentOutOfRangeException(nameof(bitCount),
					$"{BitsRemaining} bits remaining, attempted to create a substream with {fromBitIndex + bitCount - BitsRemaining} too many bits");
			return new BitStreamReader(Data, bitCount, Start + fromBitIndex, IsLittleEndian);
		}
		

		public readonly BitStreamReader FromBeginning() {
			return new BitStreamReader(Data, BitLength, Start, IsLittleEndian);
		}


		public readonly string ToBinaryString() {
			(byte[] bytes, int bitCount) = Split().ReadRemainingBits();
			if ((bitCount & 0x07) == 0)
				return ParserTextUtils.BytesToBinaryString(bytes);
			else
				return $"{ParserTextUtils.BytesToBinaryString(bytes[..^1])} {ParserTextUtils.ByteToBinaryString(bytes[^1], bitCount & 0x07)}".Trim();
		}


		public readonly string ToHexString(string separator = " ") {
			(byte[] bytes, int _) = Split().ReadRemainingBits();
			return ParserTextUtils.BytesToHexString(bytes, separator);
		}


		public override string ToString() {
			return $"{{start: {Start}, length: {BitLength}, abs index: {AbsoluteBitIndex} ({AbsoluteByteIndex}), rel: {CurrentBitIndex} ({CurrentByteIndex})}}";
			//return $"{{start: {Start}, bit length: {BitLength}, cur abs offset: {AbsoluteBitIndex} ({AbsoluteByteIndex}), relative: " + CurrentBitIndex + " (" + CurrentByteIndex + ")}";
		}
		
		
		public void ReadStruct<T>(out T @struct, int size) where T : struct {
			Span<byte> span = stackalloc byte[size];
			ReadBytesToSpan(span);
			ParserUtils.ByteSpanToStruct(out @struct, span);
		}


		public void SkipBytes(uint byteCount) => SkipBits(byteCount << 3);
		public void SkipBytes(int byteCount) => SkipBits(byteCount << 3);
		public void SkipBits(uint bitCount) => SkipBits((int)bitCount);


		public void SkipBits(int bitCount) {
			EnsureCapacity(bitCount);
			AbsoluteBitIndex += bitCount;
		}


		public void EnsureByteAlignment() {
			if (!IsByteAligned)
				AbsoluteBitIndex += 8 - IndexInByte;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private readonly void EnsureCapacity(long bitCount) {
			if (bitCount > BitsRemaining)
				throw new ArgumentOutOfRangeException(nameof(bitCount),
					$"{nameof(EnsureCapacity)} failed - {bitCount} bits were needed but only {BitsRemaining} were left");
		}
		
		
		private readonly byte BitMask(int bitCount) => BitMask(bitCount, IndexInByte);


		private readonly byte BitMask(int bitCount, int indexInByte) {
			return (byte)(RemainingBitMask & ~(0xff << (bitCount + indexInByte)));
		}


		public bool ReadBool() {
			EnsureCapacity(1);
			bool result = (CurrentByte & BitMask(1)) != 0;
			AbsoluteBitIndex++;
			return result;
		}
		

		public byte ReadByte() {
			EnsureCapacity(8);
			if (IsByteAligned) {
				byte result = CurrentByte;
				AbsoluteBitIndex += 8;
				return result;
			}
			int bitsToGetInNextByte = IndexInByte;
			int bitsToGetInCurrentByte = 8 - bitsToGetInNextByte;
			int output = (CurrentByte & RemainingBitMask) >> bitsToGetInNextByte;
			AbsoluteBitIndex += bitsToGetInCurrentByte;
			output |= (CurrentByte & BitMask(bitsToGetInNextByte)) << bitsToGetInCurrentByte;
			AbsoluteBitIndex += bitsToGetInNextByte;
			return (byte)output;
		}


		public byte PeekByte() {
			EnsureCapacity(8);
			if (IsByteAligned)
				return CurrentByte;
			int bitsInNext = IndexInByte;
			int bitsInCurrent = 8 - bitsInNext;
			return (byte)(((CurrentByte & RemainingBitMask) >> bitsInNext)
						  | ((Data[CurrentByteIndex + 1] & BitMask(bitsInNext, 0)) << bitsInCurrent));
		}


		public sbyte ReadSByte() => (sbyte)ReadByte();


		public byte[] ReadBytes(int byteCount) {
			byte[] result = new byte[byteCount];
			ReadBytesToSpan(result.AsSpan());
			return result;
		}
		
		
		public void ReadBytesToSpan(Span<byte> byteSpan) {
			if (byteSpan.Length == 0)
				return;
			EnsureCapacity(byteSpan.Length << 3);
			if (IsByteAligned) {
				Data.AsSpan(AbsoluteByteIndex, byteSpan.Length).CopyTo(byteSpan);
				AbsoluteBitIndex += byteSpan.Length << 3;
			} else {
				for (int i = 0; i < byteSpan.Length; i++) 
					byteSpan[i] = ReadByte();
			}
		}


		public IEnumerable<string> ReadNullSeparatedStrings(int byteLen) {
			var res = new LinkedList<string>();
			int finalIndex = AbsoluteBitIndex + (byteLen << 3);
			while (AbsoluteBitIndex < finalIndex) {
				if (ReadByte() != 0) {
					CurrentBitIndex -= 8;
					res.AddLast(ReadNullTerminatedString());
				}
			}
			return res;
		}


		public byte[] ReadBits(int bitCount) {
			byte[] result = new byte[(bitCount >> 3) + ((bitCount & 0x07) > 0 ? 1 : 0)];
			ReadBitsToSpan(result.AsSpan(), bitCount);
			return result;
		}


		// hey idiot! bytes are read least significant bit first!
		private void ReadBitsToSpan(Span<byte> byteSpan, int bitCount) {
			Span<byte> fullBytes = byteSpan.Slice(0, bitCount >> 3);
			ReadBytesToSpan(fullBytes);
			bitCount &= 0x07;
			if (bitCount > 0) {
				int lastByte;
				int bitsLeftInByte = 8 - IndexInByte;
				if (bitCount > bitsLeftInByte) {               // if remaining bits stretch over 2 bytes
					lastByte = CurrentByte & RemainingBitMask; // get bits from current byte
					lastByte >>= IndexInByte;
					AbsoluteBitIndex += bitsLeftInByte;
					bitCount -= bitsLeftInByte;
					lastByte |= (CurrentByte & BitMask(bitCount)) << bitsLeftInByte; // get bits from remaining byte
				} else {
					lastByte = (CurrentByte & BitMask(bitCount)) >> IndexInByte;
				}
				AbsoluteBitIndex += bitCount;
				byteSpan[fullBytes.Length] = (byte)lastByte;
			}
		}


		public (byte[] bytes, int bitCount) ReadRemainingBits() {
			int bitsRemaining = BitsRemaining;
			return (ReadBits(bitsRemaining), bitsRemaining);
		}


		public uint ReadBitsAsUInt(int bitCount) {
			if (bitCount > 32 || bitCount < 0)
				throw new ArgumentException("the number of bits requested must fit in an int", nameof(bitCount));
			EnsureCapacity(bitCount);
			Span<byte> bytes = stackalloc byte[4];
			bytes.Clear();
			ReadBitsToSpan(bytes, bitCount);
			if (BitConverter.IsLittleEndian ^ IsLittleEndian)
				bytes.Reverse();
			return BitConverter.ToUInt32(bytes);
		}


		public uint ReadBitsAsUInt(uint bitCount) => ReadBitsAsUInt((int)bitCount);


		public int ReadBitsAsSInt(int bitCount) {
			int res = (int)ReadBitsAsUInt(bitCount);
			if ((res & (1 << (bitCount - 1))) != 0) // sign extend, not necessary for primitive types
				res |= int.MaxValue << bitCount; // can use int here since the leftmost 0 will get shifted away anyway
			return res;
		}


		public int ReadBitsAsSInt(uint bitCount) => ReadBitsAsSInt((int)bitCount);


		public unsafe string ReadNullTerminatedString() {
			if (IsByteAligned) {
				string s;
				fixed (byte* bytePtr = Data)
					s = new string((sbyte*)&bytePtr[AbsoluteByteIndex]);
				AbsoluteBitIndex += (s.Length + 1) << 3;
				return s;
			}
			sbyte* strPtr = stackalloc sbyte[1024];
			int i = 0;
			do 
				strPtr[i] = ReadSByte();
			while (strPtr[i++] != 0);
			return new string(strPtr);
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
					AbsoluteBitIndex -= 8;
					tokens[i] = ReadNullTerminatedString();
				}
			}
			if (tmp + byteSize != CurrentByteIndex)
				throw new OverflowException($"the token list did not read the correct amount of bytes, {CurrentByteIndex - tmp - byteSize} bytes off");
			return tokens;
		}
		
		// I need to write these manually because I can't do Func<Span<byte>, T> cuz Span is a ref struct. 
		
		public uint ReadUInt() {
			Span<byte> span = stackalloc byte[sizeof(uint)];
			ReadBytesToSpan(span);
			if (BitConverter.IsLittleEndian ^ IsLittleEndian)
				span.Reverse();
			return BitConverter.ToUInt32(span);
		}


		public int ReadSInt() {
			Span<byte> span = stackalloc byte[sizeof(int)];
			ReadBytesToSpan(span);
			if (BitConverter.IsLittleEndian ^ IsLittleEndian)
				span.Reverse();
			return BitConverter.ToInt32(span);
		}
		
		
		public ushort ReadUShort() {
			Span<byte> span = stackalloc byte[sizeof(ushort)];
			ReadBytesToSpan(span);
			if (BitConverter.IsLittleEndian ^ IsLittleEndian)
				span.Reverse();
			return BitConverter.ToUInt16(span);
		}


		public short ReadSShort() {
			Span<byte> span = stackalloc byte[sizeof(short)];
			ReadBytesToSpan(span);
			if (BitConverter.IsLittleEndian ^ IsLittleEndian)
				span.Reverse();
			return BitConverter.ToInt16(span);
		}
		

		public float ReadFloat() {
			Span<byte> span = stackalloc byte[sizeof(float)];
			ReadBytesToSpan(span);
			if (BitConverter.IsLittleEndian ^ IsLittleEndian)
				span.Reverse();
			return BitConverter.ToSingle(span);
		}


		public void ReadVector3(out Vector3 vec3) {
			vec3.X = ReadFloat();
			vec3.Y = ReadFloat();
			vec3.Z = ReadFloat();
		}

		
		// for all 'IfExists' methods - read one bit, if it's set, read the desired field


		public byte? ReadByteIfExists() {
			return ReadBool() ? ReadByte() : (byte?) null;
		}
		
		
		public uint? ReadUIntIfExists() {
			return ReadBool() ? ReadUInt() : (uint?)null;
		}
		
		
		public ushort? ReadUShortIfExists() {
			return ReadBool() ? ReadUShort() : (ushort?)null;
		}
		

		public float? ReadFloatIfExists() {
			return ReadBool() ? ReadFloat() : (float?)null;
		}


		public byte[]? ReadBytesIfExists(int byteCount) {
			return ReadBool() ? ReadBytes(byteCount) : null;
		}


		public byte[]? ReadBitsIfExists(int bitCount) {
			return ReadBool() ? ReadBits(bitCount) : null;
		}


		public uint? ReadBitsAsUIntIfExists(int bitCount) {
			return ReadBool() ? ReadBitsAsUInt(bitCount) : (uint?)null;
		}


		public int? ReadBitsAsSIntIfExists(int bitCount) {
			return ReadBool() ? ReadBitsAsSInt(bitCount) : (int?)null;
		}


		public string? ReadStringIfExists() {
			return ReadBool() ? ReadNullTerminatedString() : null;
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