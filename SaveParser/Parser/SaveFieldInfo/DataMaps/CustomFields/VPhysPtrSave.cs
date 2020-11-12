using System;
using System.Diagnostics.CodeAnalysis;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysInterfaceId_t;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {
	
	public class PhysicsObject : ParsedSaveField {
		
		public ParsedDataMap ObjectTemplate;
		public ParsedDataMap? ControllerTemplate;
		
		
		public PhysicsObject(TypeDesc desc) : base(desc) {}
		
		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append("physics object:");
			iw.FutureIndent++;
			iw.AppendLine();
			ObjectTemplate.AppendToWriter(iw);
			if (ControllerTemplate != null) {
				iw.AppendLine();
				ControllerTemplate.AppendToWriter(iw);
			}
			iw.FutureIndent--;
		}
	}
	
	
	public class VPhysPtrSave : ParsedSaveField {
		
		public VPhysPtrSave(TypeDesc desc) : base(desc) {}
		
		
		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append("VPHYSPTR NOT IMPLEMENTED");
		}


		// return type needs to exist to match with the CustomReadFunc delegate
		public static VPhysPtrSave? QueueRestore(TypeDesc typeDesc, SaveInfo info, ref BitStreamReader bsr) {
			info.ParseContext.VPhysicsRestoreInfo!.Enqueue((info.ParseContext.CurrentEntity!, typeDesc));
			return null;
		}


		public static ParsedSaveField Restore(
			SaveInfo saveInfo,
			ParsedDataMap physHeader,
			(ParsedEntData ent, TypeDesc typeDesc) physRestoreInfo,
			PhysInterfaceId_t physType,
			ref BitStreamReader bsr)
		{
			var oldObj = bsr.ReadSInt();
			switch (physType) {
				case PIID_IPHYSICSOBJECT:
					var ot = bsr.ReadDataMap("vphysics_save_cphysicsobject_t", saveInfo);
					ParsedDataMap? ct =
						ot.GetFieldOrDefault<bool>("hasShadowController")
							? bsr.ReadDataMap("vphysics_save_cshadowcontroller_t", saveInfo)
							: null;
					return new PhysicsObject(physRestoreInfo.typeDesc) {ObjectTemplate = ot, ControllerTemplate = ct};
				case PIID_IPHYSICSFLUIDCONTROLLER:
					break;
				case PIID_IPHYSICSSPRING:
					break;
				case PIID_IPHYSICSCONSTRAINTGROUP:
					break;
				case PIID_IPHYSICSCONSTRAINT:
					break;
				case PIID_IPHYSICSSHADOWCONTROLLER:
					break;
				case PIID_IPHYSICSPLAYERCONTROLLER:
					break;
				case PIID_IPHYSICSMOTIONCONTROLLER:
					break;
				case PIID_IPHYSICSVEHICLECONTROLLER:
					break;
				case PIID_IPHYSICSGAMETRACE:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(physType), $"bad phys type: {physType}");
			}
			saveInfo.AddError($"phys parsing not implemented for {physType}");
			return null!;
		}
	}
	
	
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	[SuppressMessage("ReSharper", "IdentifierTypo")]
	public enum PhysInterfaceId_t {
		PIID_UNKNOWN,
		PIID_IPHYSICSOBJECT,
		PIID_IPHYSICSFLUIDCONTROLLER,
		PIID_IPHYSICSSPRING,
		PIID_IPHYSICSCONSTRAINTGROUP,
		PIID_IPHYSICSCONSTRAINT,
		PIID_IPHYSICSSHADOWCONTROLLER,
		PIID_IPHYSICSPLAYERCONTROLLER,
		PIID_IPHYSICSMOTIONCONTROLLER,
		PIID_IPHYSICSVEHICLECONTROLLER,
		PIID_IPHYSICSGAMETRACE
	}
}