using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile {


	public abstract class SaveGameHeader : SaveComponent {
		
		public readonly ParsedDataMap DataHeader;
		public string Name => (CharArray)DataHeader.GetField<CharArray>("szName");
		

		protected SaveGameHeader(SourceSave? saveRef, ParsedDataMap dataHeader) : base(saveRef) {
			DataHeader = dataHeader;
		}


		public static SaveGameHeader? CreateFromHeaderInfo(SourceSave? saveRef, ParsedDataMap dataHeader) {
			CharArray name = dataHeader.GetField<CharArray>("szName");
			switch (name) {
				case "Entities":
					return new ETableHeader(saveRef, dataHeader);
				case "Physics":
					return new PhysicsInfoHeader(saveRef, dataHeader);
				case "AI":
				case "Templates":
				case "ResponseSystem":
				case "Commentary":
				case "EventQueue":
				case "Achievement":
					return new VersionableSaveGameHeader(saveRef, dataHeader);
				default:
					saveRef!.SaveInfo.AddError($"unknown header name: {name}");
					return null;
				// todo VScriptServer and maybe PaintDatabase (will have to ree)
			}
		}

		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"name: {Name}, ");
		}
	}


	public class ETableHeader : SaveGameHeader {

		public ParsedDataMap[] EntHeaders;
		

		public ETableHeader(SourceSave? saveRef, ParsedDataMap dataHeader) : base(saveRef, dataHeader) {}


		protected override void Parse(ref ByteStreamReader bsr) {
			int nEntities = bsr.ReadSInt();
			EntHeaders = new ParsedDataMap[nEntities];
			for (int i = 0; i < nEntities; i++)
				EntHeaders[i] = bsr.ReadDataMap("ETABLE", SaveInfo);
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.Append($"{EntHeaders.Length} entity headers:");
			iw.FutureIndent++;
			foreach (ParsedDataMap entHeader in EntHeaders) {
				iw.AppendLine();
				entHeader.AppendToWriter(iw);
			}
			iw.FutureIndent--;
		}
	}


	public class VersionableSaveGameHeader : SaveGameHeader {

		public int Version;
		
		public VersionableSaveGameHeader(SourceSave? saveRef, ParsedDataMap dataHeader) : base(saveRef, dataHeader) {}
		
		protected override void Parse(ref ByteStreamReader bsr) {
			Version = bsr.ReadSShort();
		}
		
		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.Append($"version: {Version}");
		}
	}


	public class PhysicsInfoHeader : VersionableSaveGameHeader {
		
		public ParsedDataMap PhysHeader;
		
		public PhysicsInfoHeader(SourceSave? saveRef, ParsedDataMap dataHeader) : base(saveRef, dataHeader) {}
		
		
		protected override void Parse(ref ByteStreamReader bsr) {
			base.Parse(ref bsr);
			PhysHeader = bsr.ReadDataMap("PhysBlockHeader_t", SaveInfo);
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			PhysHeader.AppendToWriter(iw);
			iw.FutureIndent--;
		}
	}
}