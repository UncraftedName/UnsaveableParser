using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class SoundPatch : ParsedSaveField {

		public (ParsedDataMap patch, ParsedDataMap?[]? commands)[] Patches;
		public override object FieldAsObj => Patches;
		

		public SoundPatch(TypeDesc desc) : base(desc) {}
		
		
		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append($"{Patches.Length} sound patch");
			if (Patches.Length != 1)
				iw.Append("es");
			if (Patches.Length > 0) {
				iw.Append(":");
				iw.FutureIndent++;
				foreach ((ParsedDataMap patch, ParsedDataMap?[]? commands) in Patches) {
					iw.AppendLine();
					patch.AppendToWriter(iw);
					if (commands != null) {
						iw.Append($"\n{commands.Length} command");
						if (commands.Length != 1)
							iw.Append("s");
						if (commands.Length > 0) {
							iw.Append(":");
#pragma warning disable 8631
							EnumerableAppendHelper(commands, iw);
#pragma warning restore 8631
						}
					}
				}
				iw.FutureIndent--;
			}
		}
		

		public static SoundPatch Restore(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr) {
			bsr.StartBlock(info);
			var patches = new (ParsedDataMap patch, ParsedDataMap?[]? commands)[1]; // always 1?
			for (int i = 0; i < patches.Length; i++) {
				bsr.StartBlock(info);
				bsr.TryReadDataMapRecursive("CSoundPatch", info, out var patch);
				bsr.EndBlock(info);
				if (patch!.GetFieldOrDefault<int>("m_isPlaying") != 0) {
					bsr.StartBlock(info);
					var commandCount = bsr.ReadSInt();
					ParsedDataMap?[] commands = new ParsedDataMap?[commandCount];
					for (int j = 0; j < commandCount; j++) {
						bsr.StartBlock(info);
						bsr.TryReadDataMapRecursive("SoundCommand_t", info, out commands[j]);
						bsr.EndBlock(info);
					}
					bsr.EndBlock(info);
					patches[i] = (patch, commands);
				}
			}
			bsr.EndBlock(info);
			return new SoundPatch(typeDesc) {Patches = patches};
		}
	}
}