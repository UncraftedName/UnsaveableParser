using System;
using System.Diagnostics.CodeAnalysis;
using SaveParser.Parser.StateFile.SaveStateData.EntData;
using SaveParser.Utils;
using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysicsConstraint;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysicsConstraint.ConstraintType;
using static SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields.PhysInterfaceId_t;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields {

	public abstract class RestoredVPhysicsObject : ParsedSaveField {
		
		public readonly int OldObj; // I think this is a pointer from before the save?


		protected RestoredVPhysicsObject(TypeDesc desc, int oldObj) : base(desc) {
			OldObj = oldObj;
		}
		

		public override void PrettyWrite(IPrettyWriter iw) {
			iw.Append(ParserTextUtils.CamelCaseToLowerSpaced(GetType().Name));
			iw.Append($" (old obj = {OldObj}):");
		}


		public abstract bool Equals(RestoredVPhysicsObject resPhysObj);


		public override bool Equals(ParsedSaveField? other) {
			if (other == null || !(other is RestoredVPhysicsObject vphysObj) || OldObj != vphysObj.OldObj)
				return false;
			return Equals(vphysObj);
		}
	}
	
	
	public class PhysicsObject : RestoredVPhysicsObject {
		
		public readonly ParsedDataMap ObjectTemplate;
		public readonly ParsedDataMap? ControllerTemplate;


		public PhysicsObject(TypeDesc desc, int oldObj, ParsedDataMap objTemplate, ParsedDataMap? controllerTemplate)
			: base(desc, oldObj)
		{
			ObjectTemplate = objTemplate;
			ControllerTemplate = controllerTemplate;
		}
		
		
		public override void PrettyWrite(IPrettyWriter iw) {
			base.PrettyWrite(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			ObjectTemplate.PrettyWrite(iw);
			if (ControllerTemplate != null) {
				iw.AppendLine();
				ControllerTemplate.PrettyWrite(iw);
			}
			iw.FutureIndent--;
		}


		public override bool Equals(RestoredVPhysicsObject resPhysObj) {
			if (!(resPhysObj is PhysicsObject physObj))
				return false;
			return Equals(ObjectTemplate,physObj.ObjectTemplate) &&
				   Equals(ControllerTemplate, physObj.ControllerTemplate);
		}
	}


	public class PhysicsConstraintGroup : RestoredVPhysicsObject {

		public readonly ParsedDataMap GroupTemplate;


		public PhysicsConstraintGroup(TypeDesc desc, int oldObj, ParsedDataMap groupTemplate) : base(desc, oldObj) {
			GroupTemplate = groupTemplate;
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			base.PrettyWrite(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			GroupTemplate.PrettyWrite(iw);
			iw.FutureIndent--;
		}


		public override bool Equals(RestoredVPhysicsObject resPhysObj) {
			if (!(resPhysObj is PhysicsConstraintGroup restoredConstraintGroup))
				return false;
			return Equals(GroupTemplate, restoredConstraintGroup.GroupTemplate);
		}
	}


	public class PhysicsConstraint : RestoredVPhysicsObject {

		public readonly ConstraintType PhysicsConstraintType;
		public readonly ParsedDataMap Header;
		public readonly ParsedDataMap? Constraint;


		public PhysicsConstraint(
			TypeDesc desc,
			int oldObj,
			ConstraintType physicsConstraintType,
			ParsedDataMap header,
			ParsedDataMap? constraint)
			: base(desc, oldObj)
		{
			PhysicsConstraintType = physicsConstraintType;
			Header = header;
			Constraint = constraint;
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			base.PrettyWrite(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			Header.PrettyWrite(iw);
			if (Constraint != null) {
				iw.AppendLine();
				Constraint.PrettyWrite(iw);
			}
			iw.FutureIndent--;
		}


		public override bool Equals(RestoredVPhysicsObject resPhysObj) {
			if (!(resPhysObj is PhysicsConstraint restoredConstraint))
				return false;
			return PhysicsConstraintType == restoredConstraint.PhysicsConstraintType &&
				   Equals(Header, restoredConstraint.Header) &&
				   Equals(Constraint, restoredConstraint.Constraint);
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


	public class PhysicsMotionController : RestoredVPhysicsObject {
		
		public readonly ParsedDataMap MotionController;


		public PhysicsMotionController(TypeDesc desc, int oldObj, ParsedDataMap motionController) : base(desc, oldObj) {
			MotionController = motionController;
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			base.PrettyWrite(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			MotionController.PrettyWrite(iw);
			iw.FutureIndent--;
		}


		public override bool Equals(RestoredVPhysicsObject resPhysObj) {
			if (!(resPhysObj is PhysicsMotionController restoredMotionController))
				return false;
			return Equals(MotionController, restoredMotionController.MotionController);
		}
	}


	public class PhysicsSpring : RestoredVPhysicsObject {

		public readonly ParsedDataMap Spring;


		public PhysicsSpring(TypeDesc desc, int oldObj, ParsedDataMap spring) : base(desc, oldObj) {
			Spring = spring;
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			base.PrettyWrite(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			Spring.PrettyWrite(iw);
			iw.FutureIndent--;
		}


		public override bool Equals(RestoredVPhysicsObject resPhysObj) {
			if (!(resPhysObj is PhysicsSpring restoredSpring))
				return false;
			return Equals(Spring, restoredSpring.Spring);
		}
	}


	public class PhysicsVehicleController : RestoredVPhysicsObject {

		public readonly ParsedDataMap Controller;


		public PhysicsVehicleController(TypeDesc desc, int oldObj, ParsedDataMap controller) : base(desc, oldObj) {
			Controller = controller;
		}


		public override void PrettyWrite(IPrettyWriter iw) {
			base.PrettyWrite(iw);
			iw.FutureIndent++;
			iw.AppendLine();
			Controller.PrettyWrite(iw);
			iw.FutureIndent--;
		}


		public override bool Equals(RestoredVPhysicsObject resPhysObj) {
			if (!(resPhysObj is PhysicsVehicleController restoredVehController))
				return false;
			return Equals(Controller, restoredVehController.Controller);
		}
	}
	
	
	public static class CPhysicsEnvironment {

		// return type needs to exist to match with the CustomReadFunc delegate, but I'll always return null
		public static ParsedSaveField? QueueRestore(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr) {
			info.ParseContext.VPhysicsRestoreInfo!.Enqueue((info.ParseContext.CurrentEntity!, typeDesc));
			return null;
		}


		// same name as in game code - CPhysicsEnvironment::Restore
		public static ParsedSaveField? Restore(
			SaveInfo saveInfo,
			ParsedDataMap physHeader,
			(ParsedEntData ent, TypeDesc typeDesc) physRestoreInfo,
			ref ByteStreamReader bsr)
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
					return new PhysicsObject(typeDesc, oldObj, ot, ct);
				case PIID_IPHYSICSFLUIDCONTROLLER:
					break;
				case PIID_IPHYSICSSPRING:
					var s = bsr.ReadDataMap("vphysics_save_cphysicsspring_t", saveInfo);
					return new PhysicsSpring(typeDesc, oldObj, s);
				case PIID_IPHYSICSCONSTRAINTGROUP:
					var gt = bsr.ReadDataMap("vphysics_save_cphysicsconstraintgroup_t", saveInfo);
					return new PhysicsConstraintGroup(typeDesc, oldObj, gt);
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
					return new PhysicsConstraint(typeDesc, oldObj, type, header, constraint);
				case PIID_IPHYSICSSHADOWCONTROLLER:
					break;
				case PIID_IPHYSICSPLAYERCONTROLLER:
					break;
				case PIID_IPHYSICSMOTIONCONTROLLER:
					var mc = bsr.ReadDataMap("vphysics_save_motioncontroller_t", saveInfo);
					return new PhysicsMotionController(typeDesc, oldObj, mc);
				case PIID_IPHYSICSVEHICLECONTROLLER:
					var vc = bsr.ReadDataMap("vphysics_save_cvehiclecontroller_t", saveInfo);
					return new PhysicsVehicleController(typeDesc, oldObj, vc);
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