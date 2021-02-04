using System;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class SoundPatch : ParsedSaveField {

		public readonly (ParsedDataMap patch, ParsedDataMap?[]? commands)[] Patches;


		public SoundPatch(TypeDesc desc, (ParsedDataMap patch, ParsedDataMap?[]? commands)[] patches) : base(desc) {
			Patches = patches;
		}


		public override bool Equals(ParsedSaveField? other) {
			if (other == null || !(other is SoundPatch otherPatch))
				return false;
			if (Patches.Length != otherPatch.Patches.Length)
				return false;
			for (int i = 0; i < Patches.Length; i++) {
				(ParsedDataMap? patchA, ParsedDataMap?[]? commandsA) = Patches[i];
				(ParsedDataMap? patchB, ParsedDataMap?[]? commandsB) = otherPatch.Patches[i];
				if (!Equals(patchA, patchB))
					return false;
				if (!ParserUtils.NullableSequenceEquals(commandsA, commandsB))
					return false;
			}
			return true;
		}
		
		
		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append($"{Patches.Length} sound patch");
			if (Patches.Length != 1)
				iw.Append("es");
			iw.Append($" {Desc.Name}");
			if (Patches.Length > 0) {
				iw.Append(":");
				iw.FutureIndent++;
				foreach ((ParsedDataMap patch, ParsedDataMap?[]? commands) in Patches) {
					iw.AppendLine();
					patch.PrettyWrite(iw);
					if (commands != null) {
						iw.Append($"\n{commands.Length} command");
						if (commands.Length != 1)
							iw.Append("s");
						if (commands.Length > 0) {
							iw.Append(":");
#pragma warning disable 8631
							EnumerablePrettyWriteHelper(commands, iw);
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
				ParsedDataMap?[]? commands = null;
				if (patch!.GetFieldOrDefault<int>("m_isPlaying") != 0) {
					bsr.StartBlock(info);
					var commandCount = bsr.ReadSInt();
					commands = new ParsedDataMap?[commandCount];
					for (int j = 0; j < commandCount; j++) {
						bsr.StartBlock(info);
						bsr.TryReadDataMapRecursive("SoundCommand_t", info, out commands[j]);
						bsr.EndBlock(info);
					}
					bsr.EndBlock(info);
				}
				patches[i] = (patch, commands);
			}
			try {
				bsr.EndBlock(info);
			} catch (Exception e) {
				// I've only seen this happen with m_sndPlayerInBeam, but it seems to work fine other than not ending on the correct byte
				info.AddError($"exception while parsing {nameof(SoundPatch)} for field \"{typeDesc.Name}\": {e.Message}"); 
			}
			return new SoundPatch(typeDesc, patches);
		}
	}
}