using System;
using System.Diagnostics.CodeAnalysis;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysicsConstraint;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysicsConstraint.ConstraintType;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysInterfaceId_t;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {

	public abstract class RestoredVPhysicsObject : ParsedSaveField {
		
		public int OldObj; // I think this is a pointer from before the save?
		
		protected RestoredVPhysicsObject(TypeDesc desc) : base(desc) {}
		

		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append(ParserTextUtils.CamelCaseToLowerSpaced(GetType().Name));
			iw.Append($" (old obj = {OldObj}):");
		}
	}
	
	
	public class PhysicsObject : RestoredVPhysicsObject {
		
		public ParsedDataMap ObjectTemplate;
		public ParsedDataMap? ControllerTemplate;

		public PhysicsObject(TypeDesc desc) : base(desc) {}
		
		
		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
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


	public class PhysicsConstraintGroup : RestoredVPhysicsObject {

		public ParsedDataMap GroupTemplate;
		
		public PhysicsConstraintGroup(TypeDesc desc) : base(desc) {}
		

		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			GroupTemplate.AppendToWriter(iw);
			iw.FutureIndent--;
		}
	}


	public class PhysicsConstraint : RestoredVPhysicsObject {

		public ConstraintType PhysicsConstraintType;
		public ParsedDataMap Header;
		public ParsedDataMap? Constraint;
		
		
		public PhysicsConstraint(TypeDesc desc) : base(desc) {}


		public override void AppendToWriter(IIndentedWriter iw) {
			base.AppendToWriter(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			Header.AppendToWriter(iw);
			if (Constraint != null) {
				iw.AppendLine();
				Constraint.AppendToWriter(iw);
			}
			iw.FutureIndent--;
		}


		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public enum ConstraintType {
			CONSTRAINT_UNKNOWN = 0,
			CONSTRAINT_RAGDOLL,
			CONSTRAINT_HINGE,
			CONSTRAINT_FIXED,
			CONSTRAINT_BALLSOCKET,
			CONSTRAINT_SLIDING,
			CONSTRAINT_PULLEY,
			CONSTRAINT_LENGTH,
		}
	}
	
	
	public static class CPhysicsEnvironment {

		// return type needs to exist to match with the CustomReadFunc delegate, but I'll always return null
		public static ParsedSaveField? QueueRestore(TypeDesc typeDesc, SaveInfo info, ref BitStreamReader bsr) {
			info.ParseContext.VPhysicsRestoreInfo!.Enqueue((info.ParseContext.CurrentEntity!, typeDesc));
			return null;
		}


		// same name as in game code - CPhysicsEnvironment::Restore
		public static ParsedSaveField? Restore(
			SaveInfo saveInfo,
			ParsedDataMap physHeader,
			(ParsedEntData ent, TypeDesc typeDesc) physRestoreInfo,
			ref BitStreamReader bsr)
		{
			var oldObj = bsr.ReadSInt();
			PhysInterfaceId_t physType = (PhysInterfaceId_t)physHeader.GetFieldOrDefault<int>("type").Field;
			var typeDesc = physRestoreInfo.typeDesc;
			
			switch (physType) {
				case PIID_IPHYSICSOBJECT:
					var ot = bsr.ReadDataMap("vphysics_save_cphysicsobject_t", saveInfo);
					ParsedDataMap? ct =
						ot.GetFieldOrDefault<bool>("hasShadowController")
							? bsr.ReadDataMap("vphysics_save_cshadowcontroller_t", saveInfo)
							: null;
					return new PhysicsObject(typeDesc) {OldObj = oldObj, ObjectTemplate = ot, ControllerTemplate = ct};
				case PIID_IPHYSICSFLUIDCONTROLLER:
					break;
				case PIID_IPHYSICSSPRING:
					break;
				case PIID_IPHYSICSCONSTRAINTGROUP:
					var gt = bsr.ReadDataMap("vphysics_save_cphysicsconstraintgroup_t", saveInfo);
					return new PhysicsConstraintGroup(typeDesc) {OldObj = oldObj, GroupTemplate = gt};
				case PIID_IPHYSICSCONSTRAINT:
					var header = bsr.ReadDataMap("vphysics_save_cphysicsconstraint_t", saveInfo);
					ConstraintType type = (ConstraintType)(int)header.GetFieldOrDefault<int>("constraintType");
					ParsedDataMap? constraint = null;
					if (type != CONSTRAINT_UNKNOWN
						&& header.ParsedFields.ContainsKey("pObjAttached")
						&& header.ParsedFields.ContainsKey("pObjReference"))
					{
						string datamapName = $"vphysics_save_{type.ToString().Replace("_", "").ToLower()}_t";
						constraint = bsr.ReadDataMap(datamapName, saveInfo);
					}
					return new PhysicsConstraint(typeDesc)
						{OldObj = oldObj, Header = header, PhysicsConstraintType = type, Constraint = constraint};
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
			return null;
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