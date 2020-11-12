using System;

namespace SaveParser.Utils {
	
	// represents a c-style string like char[60] which is null-terminated
	public readonly struct CharArray : IAppendable {

		private readonly int _length; // this is equal to or longer than the string length
		public readonly string Str;

		public static implicit operator string(CharArray ca) => ca.Str;


		public CharArray(byte[] bytes) {
			_length = bytes.Length;
			Str = ParserTextUtils.ByteArrayAsString(bytes);
		}


		public byte[] AsByteArray() {
			return ParserTextUtils.StringAsByteArray(Str, _length);
		}


		public override string ToString() {
			return Str;
		}


		public void AppendToWriter(IIndentedWriter iw) {
			iw.Append(Str);
		}


		public override bool Equals(object? obj) {
			return obj is CharArray otherChrArr && otherChrArr.Str == Str && otherChrArr._length == _length;
		}


		public override int GetHashCode() {
			return HashCode.Combine(Str, _length);
		}
	}
}