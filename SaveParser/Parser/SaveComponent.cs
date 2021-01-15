using System.Diagnostics;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser {
	
	public abstract class SaveComponent : PrettyClass {
		
		public readonly SourceSave? SaveRef;
		protected SaveInfo SaveInfo => SaveRef!.SaveInfo;
		private int _absoluteOffsetStart;
		private int _absoluteOffsetEnd;
		public virtual ByteStreamReader Reader =>
			SaveRef!.ReaderFromOffset(_absoluteOffsetStart, _absoluteOffsetEnd - _absoluteOffsetStart);


		protected SaveComponent(SourceSave? saveRef) {
			SaveRef = saveRef;
		}


		protected abstract void Parse(ref ByteStreamReader bsr);
		

		public void ParseStream(ref ByteStreamReader bsr) {
			_absoluteOffsetStart = bsr.AbsoluteByteIndex;
			_absoluteOffsetEnd = bsr.AbsoluteByteIndex + bsr.Size;
			Parse(ref bsr);
			_absoluteOffsetEnd = bsr.AbsoluteByteIndex;
		}


		public void ParseStream(ByteStreamReader bsr) {
			ParseStream(ref bsr);
			if (bsr.BytesRemaining > 0)
				Debug.WriteLine($"{GetType().Name} didn't finish reading all bytes! {bsr.BytesRemaining} left.");
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			ParserTextUtils.DefaultPrettyWrite(this, iw);
		}
	}
}