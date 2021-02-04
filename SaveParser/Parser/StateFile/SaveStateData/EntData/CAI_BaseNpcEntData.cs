// ReSharper disable All

using System.Collections.Generic;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;

namespace SaveParser.Parser.StateFile.SaveStateData.EntData {
	
	public class CAI_BaseNpcEntData : CBaseEntityParsedEntData {

		private const short AI_EXTENDED_SAVE_HEADER_FIRST_VERSION_WITH_CONDITIONS = 2;
		private const short AI_EXTENDED_SAVE_HEADER_FIRST_VERSION_WITH_NAVIGATOR_SAVE = 5;

		public ParsedDataMap ExtendedHeader;
		public ConditionStrings? Conditions;
		public CAI_NavigatorEntData? Navigator;
		
		
		public CAI_BaseNpcEntData(SourceSave saveRef, ParsedDataMap headerInfo, DataMap classMap)
			: base(saveRef, headerInfo, classMap) {}


		protected override void Parse(ref ByteStreamReader bsr) {
			ExtendedHeader = bsr.ReadDataMap("AIExtendedSaveHeader_t", SaveInfo);
			
			if (ExtendedHeader.GetFieldOrDefault<short>("version")
				>= AI_EXTENDED_SAVE_HEADER_FIRST_VERSION_WITH_CONDITIONS)
			{
				bsr.StartBlock(SaveInfo);
				Conditions = new ConditionStrings(SaveRef);
				Conditions.ParseStream(ref bsr);
				bsr.EndBlock(SaveInfo);
			}
			
			if (ExtendedHeader.GetFieldOrDefault<short>("version")
				>= AI_EXTENDED_SAVE_HEADER_FIRST_VERSION_WITH_NAVIGATOR_SAVE)
			{
				bsr.StartBlock(SaveInfo);
				Navigator = new CAI_NavigatorEntData(SaveRef!);
				Navigator.ParseStream(ref bsr);
				bsr.EndBlock(SaveInfo);
			}
			
			base.Parse(ref bsr);
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append($"({HeaderProxyName}) {ClassMap.ClassName}:");
			iw.FutureIndent++;
			iw.Append("\nextra data for CAI_BaseNPC:");
			iw.FutureIndent++;
			iw.AppendLine();
			ExtendedHeader.PrettyWrite(iw);
			iw.AppendLine();
			if (Conditions != null) {
				Conditions.PrettyWrite(iw);
				iw.AppendLine();
			}
			if (Navigator != null) {
				Navigator.PrettyWrite(iw);
				iw.AppendLine();
			}
			iw.FutureIndent--;
			base.PrettyWrite(iw, false);
			iw.FutureIndent--;
		}
		
		
		public class ConditionStrings : SaveComponent {

			public List<string> Conditions, CustomInterruptConditions, ConditionsPreIgnore, IgnoreConditions;
			
			
			public ConditionStrings(SourceSave? saveRef) : base(saveRef) {}
			
			
			protected override void Parse(ref ByteStreamReader bsr) {
				Conditions = new List<string>();
				CustomInterruptConditions = new List<string>();
				ConditionsPreIgnore = new List<string>();
				IgnoreConditions = new List<string>();
				foreach (List<string> condList in new[] {Conditions, CustomInterruptConditions, ConditionsPreIgnore, IgnoreConditions}) {
					for (;;) {
						if (bsr.ReadByte() != 0) {
							bsr.CurrentByteIndex--;
							condList.Add(bsr.ReadNullTerminatedString());
						} else {
							break;
						}
					}
				}
			}


			public override void PrettyWrite(IPrettyWriter iw) {
				iw.AppendLine($"m_Conditions: {Conditions.SequenceToString()}");
				iw.AppendLine($"m_CustomInterruptConditions: {CustomInterruptConditions.SequenceToString()}");
				iw.AppendLine($"m_ConditionsPreIgnore: {ConditionsPreIgnore.SequenceToString()}");
				iw.Append($"m_IgnoreConditions: {IgnoreConditions.SequenceToString()}");
			}
		}
	}
}