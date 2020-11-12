using System;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.StateFile {


	public abstract class SaveGameHeader : SaveComponent {
		
		public readonly ParsedDataMap DataHeader;
		public readonly string Category;

		protected SaveGameHeader(SourceSave? saveRef, ParsedDataMap dataHeader, string category) : base(saveRef) {
			DataHeader = dataHeader;
			Category = category;
		}


		public static SaveGameHeader? CreateForCategory(SourceSave? saveRef, ParsedDataMap dataHeader) {
			CharArray category = dataHeader.GetField<CharArray>("szName");
			switch (category) {
				case "Entities":
					return new ETableHeader(saveRef, dataHeader, category);
				case "Physics":
					return new PhysicsInfoHeader(saveRef, dataHeader, category);
				case "AI":
				case "Templates":
				case "ResponseSystem":
				case "Commentary":
				case "EventQueue":
				case "Achievement":
					return new VersionableSaveGameHeader(saveRef, dataHeader, category);
				default:
					saveRef!.SaveInfo.AddError($"unknown header category: {category}");
					return null;
			}
		}

		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"category: {Category}, ");
		}
	}


	public class ETableHeader : SaveGameHeader {

		public ParsedDataMap[] EntHeaders;
		

		public ETableHeader(SourceSave? saveRef, ParsedDataMap dataHeader, string category)
			: base(saveRef, dataHeader, category) {}


		protected override void Parse(ref BitStreamReader bsr) {
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
		
		public VersionableSaveGameHeader(SourceSave? saveRef, ParsedDataMap dataHeader, string category)
			: base(saveRef, dataHeader, category) {}
		
		protected override void Parse(ref BitStreamReader bsr) {
			Version = bsr.ReadSShort();
		}
		
		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.Append($"version: {Version}");
		}
	}


	public class PhysicsInfoHeader : VersionableSaveGameHeader {
		
		public ParsedDataMap PhysHeader;
		
		public PhysicsInfoHeader(SourceSave? saveRef, ParsedDataMap dataHeader, string category)
			: base(saveRef, dataHeader, category) {}
		
		
		protected override void Parse(ref BitStreamReader bsr) {
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