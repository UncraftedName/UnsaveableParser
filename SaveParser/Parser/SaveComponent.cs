using System.Diagnostics;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser {
	
	public abstract class SaveComponent : AppendableClass {
		
		public readonly SourceSave? SaveRef;
		protected SaveInfo SaveInfo => SaveRef!.SaveInfo;
		private int _absoluteOffsetStart;
		private int _absoluteOffsetEnd;
		public virtual BitStreamReader Reader =>
			SaveRef!.ReaderFromOffset(_absoluteOffsetStart, _absoluteOffsetEnd - _absoluteOffsetStart);


		protected SaveComponent(SourceSave? saveRef) {
			SaveRef = saveRef;
		}


		protected abstract void Parse(ref BitStreamReader bsr);
		

		public void ParseStream(ref BitStreamReader bsr) {
			_absoluteOffsetStart = bsr.AbsoluteBitIndex;
			_absoluteOffsetEnd = bsr.AbsoluteBitIndex + bsr.BitLength;
			Parse(ref bsr);
			_absoluteOffsetEnd = bsr.AbsoluteBitIndex;
		}


		public void ParseStream(BitStreamReader bsr) {
			ParseStream(ref bsr);
			if (bsr.BitsRemaining > 0)
				Debug.WriteLine($"{GetType().Name} didn't finish reading all bits! {bsr.BitsRemaining} left.");
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			ParserTextUtils.DefaultAppendToWriter(this, iw);
		}
	}
}