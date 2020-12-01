using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile {
	
	public abstract class EmbeddedStateFile : SaveComponent {

		public CharArray Name;
		public CharArray Id;


		public EmbeddedStateFile(SourceSave saveRef, CharArray name) : base(saveRef) {
			Name = name;
		}


		protected override void Parse(ref ByteStreamReader bsr) {
			Id = bsr.ReadCharArray(4);
		}


		public static EmbeddedStateFile CreateFromName(SourceSave saveRef, CharArray fileName) {
			return fileName.Str.Substring(fileName.Str.Length - 3, 3) switch {
				"hl1" => new SaveGameStateFile(saveRef, fileName),
				"hl2" => new ClientStateFile(saveRef, fileName),
				"hl3" => new EntityPatchFile(saveRef, fileName),
				_ => new UnknownStateFile(saveRef, fileName)
			};
		}
	}


	public sealed class UnknownStateFile : EmbeddedStateFile {
		
		public UnknownStateFile(SourceSave saveRef, CharArray name) : base(saveRef, name) {}

		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{Name}, {Reader.Size} bytes");
		}
	}
}