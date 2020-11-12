using SaveParser.Utils;

namespace SaveParser.Parser.StateFile.SaveStateData {

	public abstract class SaveStateBlock : SaveComponent {

		public readonly SaveGameHeader DataHeader;
		
		
		protected SaveStateBlock(SourceSave? saveRef, SaveGameHeader dataHeader) : base(saveRef) {
			DataHeader = dataHeader;
		}
		
		
		public static bool CreateFromHeader(SourceSave saveRef, SaveGameHeader header, out SaveStateBlock block) {
			switch (header) {
				case ETableHeader eTableHeader:
					block = new EntitySaveStateBlock(saveRef, eTableHeader);
					return true;
				case PhysicsInfoHeader physHeader:
					block = new PhysSaveStateRestoreHandler(saveRef, physHeader);
					return true;
				default:
					block = null!;
					return false;
			}
		}
	}
	

	public abstract class SaveStateBlock<THeader> : SaveStateBlock where THeader : SaveGameHeader {

		protected new THeader DataHeader => (THeader)base.DataHeader;


		protected SaveStateBlock(SourceSave? saveRef, THeader dataHeader) : base(saveRef, dataHeader) {}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"category: {DataHeader.Category}");
		}
	}
}