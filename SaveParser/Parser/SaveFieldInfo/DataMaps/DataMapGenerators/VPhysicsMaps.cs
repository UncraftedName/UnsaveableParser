// ReSharper disable All

using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public class VPhysicsMaps : DataMapGenerator {

		public const int VEHICLE_MAX_AXLE_COUNT = 4;
		public const int VEHICLE_MAX_WHEEL_COUNT = 2 * VEHICLE_MAX_AXLE_COUNT;
		public const int VEHICLE_MAX_GEAR_COUNT = 6;
		

		// todo this returns fields that don't format the ToString with spaces - "custom materialIndex: metal"
		protected void DefineMaterialIndexDataOps(string name) {
			static ParsedSaveField MatReadFunc(TypeDesc desc, SaveInfo info, ref ByteStreamReader bsr)
				=> new ParsedSaveField<MaterialIndexStr>((MaterialIndexStr)bsr.ReadStringOfLength(bsr.ReadSInt()), desc);
			DefineCustomField(name, MatReadFunc);
		}
		
		
		// this is different from PhysPtr, here the datamap is read right away (since we're in the vphys section)
		// todo, the custom read func will just return a ref to another vphys object (i think)
		private void DefineVPhysPtr(string name) {
			static ParsedSaveField ReadFunc(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr)
				=> new ParsedSaveField<int>(bsr.ReadSInt(), typeDesc);
			DefineCustomField(name, ReadFunc);
		}

		
		// todo link like above
		private void DefineVPhysPtrArray(string name, int count) {
			static ParsedSaveField ReadFunc(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr) {
				int[] arr = new int[(int)typeDesc.CustomParams![0]!];
				for (int i = 0; i < arr.Length; i++)
					arr[i] = bsr.ReadSInt();
				return new ParsedSaveField<int[]>(arr, typeDesc, arr.Length);
			}
			DefineCustomField(name, ReadFunc, new object?[] {count});
		}


		// todo link like above
		private void DefineVPhysPtrVector(string name) {
			static ParsedSaveField ReadFunc(TypeDesc typeDesc, SaveInfo info, ref ByteStreamReader bsr) {
				int count = bsr.ReadSInt();
				int[] arr = new int[count];
				for (int i = 0; i < arr.Length; i++)
					arr[i] = bsr.ReadSInt();
				return new ParsedSaveField<int[]>(arr, typeDesc, arr.Length);
			}
			DefineCustomField(name, ReadFunc);
		}
		
		
		protected override void CreateDataMaps() { // todo redo the vehicles part
			BeginDataMap("PhysBlockHeader_t");
			DefineField("nSaved", INTEGER);
			DefineField("pWorldObject", INTEGER); // this is a pointer saved for remapping
			
			BeginDataMap("PhysObjectHeader_t");
			DefineField("type", INTEGER);
			DefineField("hEntity", EHANDLE);
			DefineField("fieldName", STRING);
			DefineField("nObjects", INTEGER);
			DefineField("modelName", STRING);
			DefineField("bbox.mins", VECTOR);
			DefineField("bbox.maxs", VECTOR);
			DefineField("sphere.radius", FLOAT);
			DefineField("iCollide", INTEGER);
			
			BeginDataMap("vphysics_save_cphysicsobject_t");
			DefineField("sphereRadius", FLOAT);
			DefineField("isStatic", BOOLEAN);
			DefineField("collisionEnabled", BOOLEAN);
			DefineField("gravityEnabled", BOOLEAN);
			DefineField("dragEnabled", BOOLEAN);
			DefineField("motionEnabled", BOOLEAN);
			DefineField("isAsleep", BOOLEAN);
			DefineField("isTrigger", BOOLEAN);
			DefineField("asleepSinceCreation", BOOLEAN);
			DefineField("hasTouchedDynamic", BOOLEAN);
			DefineMaterialIndexDataOps("materialIndex");
			DefineField("mass", FLOAT);
			DefineField("rotInertia", VECTOR);
			DefineField("speedDamping", FLOAT);
			DefineField("rotSpeedDamping", FLOAT);
			DefineField("massCenterOverride", VECTOR);
			DefineField("callbacks", INTEGER);
			DefineField("gameFlags", INTEGER);
			DefineField("contentsMask", INTEGER);
			DefineField("volume", FLOAT);
			DefineField("dragCoefficient", FLOAT);
			DefineField("angDragCoefficient", FLOAT);
			DefineField("hasShadowController", BOOLEAN);
			DefineField("origin", POSITION_VECTOR);
			DefineField("angles", VECTOR);
			DefineField("velocity", VECTOR);
			DefineField("angVelocity", VECTOR);
			DefineField("collideType", SHORT);
			DefineField("gameIndex", SHORT);
			DefineField("hingeAxis", INTEGER);
			
			BeginDataMap("vphysics_save_cshadowcontroller_t");
			DefineField("secondsToArrival", FLOAT);
			DefineField("saveRot.k", FLOAT, 3);
			DefineField("savedRI.k", FLOAT, 3);
			DefineField("currentSpeed.k", FLOAT, 3);
			DefineField("savedMass", FLOAT);
			DefineField("savedFlags", INTEGER); // todo
			DefineMaterialIndexDataOps("savedMaterial");
			DefineField("enable", BOOLEAN);
			DefineField("allowPhysicsMovement", BOOLEAN);
			DefineField("allowPhysicsRotation", BOOLEAN);
			DefineField("isPhysicallyControlled", BOOLEAN);
			DefineEmbeddedField("shadowParams", "vphysics_save_shadowcontrolparams_t"); // embedded override?
			
			BeginDataMap("vphysics_save_shadowcontrolparams_t");
			DefineField("targetPosition", POSITION_VECTOR);
			DefineField("targetRotation", VECTOR);
			DefineField("maxSpeed", FLOAT);
			DefineField("maxDampSpeed", FLOAT);
			DefineField("maxAngular", FLOAT);
			DefineField("maxDampAngular", FLOAT);
			DefineField("dampFactor", FLOAT);
			DefineField("teleportDistance", FLOAT);
			
			BeginDataMap("vphysics_save_cphysicsspring_t");
			DefineField("constant", FLOAT);
			DefineField("naturalLength", FLOAT);
			DefineField("damping", FLOAT);
			DefineField("relativeDamping", FLOAT);
			DefineField("startPosition", VECTOR); // relative
			DefineField("endPosition", VECTOR); // relative
			DefineField("useLocalPositions", BOOLEAN);
			DefineField("onlyStretch", BOOLEAN);
			DefineVPhysPtr("pObjStart");
			DefineVPhysPtr("pObjEnd");
			
			BeginDataMap("vphysics_save_cphysicsconstraintgroup_t");
			DefineField("isActive", BOOLEAN);
			DefineField("additionalIterations", INTEGER);
			DefineField("minErrorTicks", INTEGER);
			DefineField("errorTolerance", FLOAT);

			BeginDataMap("vphysics_save_cphysicsconstraint_t");
			DefineField("constraintType", INTEGER);
			DefineVPhysPtr("pGroup");
			DefineVPhysPtr("pObjReference");
			DefineVPhysPtr("pObjAttached");
			
			BeginDataMap("vphysics_save_motioncontroller_t");
			DefineVPhysPtrVector("m_objectList");
			DefineField("m_nPriority", INTEGER);
			
			BeginDataMap("vehicle_bodyparams_t");
			DefineField("massCenterOverride", VECTOR); // 0 = no override
			DefineField("massOverride", FLOAT);        // 0 = no override
			DefineField("addGravity", FLOAT);          // keeps car down
			DefineField("tiltForce", FLOAT);           // keeps car down when not on float ground
			DefineField("tiltFoceHeight", FLOAT);      // tilt force pulls relative to center of mass
			DefineField("counterTorqueFactor", FLOAT);
			DefineField("keepUprightTorque", FLOAT);
			DefineField("maxAngularVelocity", FLOAT);
			
			BeginDataMap("vehicle_wheelparams_t");
			DefineField("radius", FLOAT);
			DefineField("mass", FLOAT);
			DefineField("inertia", FLOAT);
			DefineField("damping", FLOAT);       // usually 0
			DefineField("rotdamping", FLOAT);    // usually 0
			DefineField("frictionScale", FLOAT); // 1.5 front, 1.8 rear
			DefineMaterialIndexDataOps("materialIndex");
			DefineMaterialIndexDataOps("brakeMaterialIndex");
			DefineMaterialIndexDataOps("skidMaterialIndex");
			DefineField("springAdditionalLength", FLOAT); // 0 means the spring is at it's rest length
			
			BeginDataMap("vehicle_suspensionparams_t");
			DefineField("springConstant", FLOAT);
			DefineField("springDamping", FLOAT);
			DefineField("stabilizerConstant", FLOAT);
			DefineField("springDampingCompression", FLOAT);
			DefineField("maxBodyForce", FLOAT);
			
			BeginDataMap("vehicle_axleparams_t");// one per pair of wheels, raytrace & wheel data here b/c jetski uses both
			DefineField("offset", VECTOR);               // center of this axle in vehicle object space
			DefineField("wheelOffset", VECTOR);          // offset to wheel (assume other wheel is symmetric)
			DefineField("raytraceCenterOffset", VECTOR); // offset to center of axle for raytrace data
			DefineField("raytraceOFfset", VECTOR);
			DefineEmbeddedField("wheels", "vehicle_wheelparams_t");
			DefineEmbeddedField("suspension", "vehicle_suspensionparams_t");
			DefineField("torqueFactor", FLOAT); // [0-1], e.g. 0,1 for rear wheel drive, 0.5,0.5 for 4 wheel drive
			DefineField("brakeFactor", FLOAT);  // [0-1]
			
			BeginDataMap("vehicle_engineparams_t");
			DefineField("horsePower", FLOAT);
			DefineField("maxSpeed", FLOAT);
			DefineField("maxRevSpeed", FLOAT);
			DefineField("maxRPM", FLOAT); // redline limit
			DefineField("axleRatio", FLOAT); // ratio of engine rev to axle rev
			DefineField("throttleTime", FLOAT); // time to reach full throttle in seconds
			DefineField("gearCount", INTEGER);
			DefineField("gearRatio", FLOAT, VEHICLE_MAX_GEAR_COUNT);
			DefineField("shiftUpRPM", FLOAT); // max rpm to switch to a higher gear
			DefineField("shiftDownRPM", FLOAT); // min rmp to switch to a lower gear
			DefineField("boostForce", FLOAT);
			DefineField("boostDuration", FLOAT);
			DefineField("boostDelay", FLOAT);
			DefineField("boostMaxSpeed", FLOAT);
			DefineField("autobrakeSpeedGain", FLOAT);
			DefineField("autobrakeSpeedFactor", FLOAT);
			DefineField("torqueBoost", BOOLEAN);
			DefineField("isAutoTransmission", BOOLEAN);
			
			BeginDataMap("vehicle_steeringparams_t");
			DefineField("degreesSlow", FLOAT);                 // angle in degrees of steering at slow speed
			DefineField("degreesFast", FLOAT);                 // angle in degrees of steering at fast speed
			DefineField("degreesBoost", FLOAT);                // angle in degrees of steering at fast speed
			DefineField("steeringRateSlow", FLOAT);            // this is the speed the wheels are steered when the vehicle is slow
			DefineField("steeringRateFast", FLOAT);            // this is the speed the wheels are steered when the vehicle is "fast"
			DefineField("steeringRestRateSlow", FLOAT);        // this is the speed at which the wheels move toward their resting state (straight ahead) at slow speed
			DefineField("steeringRestRateFast", FLOAT);        // this is the speed at which the wheels move toward their resting state (straight ahead) at fast speed
			DefineField("speedSlow", FLOAT);                   // this is the max speed of "slow"
			DefineField("speedFast", FLOAT);                   // this is the min speed of "fast"
			DefineField("turnThrottleReduceSlow", FLOAT);      // this is the amount of throttle reduction to apply at the maximum steering angle
			DefineField("turnThrottleReduceFast", FLOAT);      // this is the amount of throttle reduction to apply at the maximum steering angle
			DefineField("brakeSteeringRateFactor", FLOAT);     // this scales the steering rate when the brake/handbrake is down
			DefineField("powerSlideAccel", FLOAT);             // this scales the steering rest rate when the throttle is down
			DefineField("boostSteeringRestRateFactor", FLOAT); // scale of speed to acceleration
			DefineField("boostSteeringRateFactor", FLOAT);     // this scales the steering rest rate when boosting
			DefineField("steeringExponent", FLOAT);            // this makes the steering response non-linear.  The steering function is linear, then raised to this power
			DefineField("isSkidAllowed", BOOLEAN);
			DefineField("dustCloud", BOOLEAN);
			
			BeginDataMap("vehicleparams_t");
			DefineField("axleCount", INTEGER);
			DefineField("wheelsPerAxle", INTEGER);
			DefineEmbeddedField("body", "vehicle_bodyparams_t");
			DefineEmbeddedField("axles", "vehicle_axleparams_t", VEHICLE_MAX_AXLE_COUNT);
			DefineEmbeddedField("engine", "vehicle_engineparams_t");
			DefineEmbeddedField("steering", "vehicle_steeringparams_t");
			
			BeginDataMap("vehicle_operatingparams_t");
			DefineField("speed", FLOAT);
			DefineField("engineRPM", FLOAT);
			DefineField("gear", INTEGER);
			DefineField("boostDelay", FLOAT);
			DefineField("boostTimeLeft", INTEGER);
			DefineField("skidSpeed", FLOAT);
			DefineMaterialIndexDataOps("skidMaterial");
			DefineField("steeringAngle", FLOAT);
			DefineField("wheelsNotInContact", INTEGER);
			DefineField("wheelsInContact", INTEGER);
			DefineField("isTorqueBoosting", BOOLEAN);
			
			BeginDataMap("vphysics_save_cvehiclecontroller_t");
			DefineVPhysPtr("m_pCarBody");
			DefineField("m_wheelCount", INTEGER);
			DefineEmbeddedField("m_vehicleData", "vehicleparams_t");
			DefineEmbeddedField("m_currentState", "vehicle_operatingparams_t");
			DefineField("m_bodyMass", FLOAT);
			DefineField("m_totalWheelMass", FLOAT);
			DefineField("m_gravityLength", FLOAT);
			DefineField("m_torqueScale", FLOAT);
			DefineVPhysPtrArray("m_pWheels", VEHICLE_MAX_WHEEL_COUNT);
			DefineField("m_wheelPosition_Bs", VECTOR, VEHICLE_MAX_WHEEL_COUNT);
			DefineField("m_tracePosition_Bs", VECTOR, VEHICLE_MAX_WHEEL_COUNT);
			DefineField("m_vehicleFlags", INTEGER);
			DefineField("m_nTireType", INTEGER);
			DefineField("m_nVehicleType", INTEGER);
			DefineField("m_bTraceData", BOOLEAN);
			DefineField("m_bOccupied", BOOLEAN);
			DefineField("m_bEngineDisable", BOOLEAN);
			DefineField("m_flVelocity", FLOAT, 3);
			
			BeginDataMap("vphysics_save_constraintbreakable_t");
			DefineField("strength", FLOAT);               // strength of constraint [0,1]
			DefineField("forceLimit", FLOAT);             // constraint force limit to break (0 means never break)
			DefineField("torqueLimit", FLOAT);            // constraint torque limit to break (0 means never break)
			DefineField("bodyMassScale", FLOAT, 2); // scale applied to mass of reference/attached object before solving constraint
			DefineField("isActive", BOOLEAN);
			
			BeginDataMap("vphysics_save_constraintfixed_t");
			DefineField("attachedRefXform", MATRIX3X4_WORLDSPACE);
			DefineEmbeddedField("constraint", "vphysics_save_constraintbreakable_t");
			
			BeginDataMap("vphysics_save_constraintaxislimit_t");
			DefineField("minRotation", FLOAT);
			DefineField("maxRotation", FLOAT);
			DefineField("angularVelocity", FLOAT);
			DefineField("torque", FLOAT);
			
			BeginDataMap("vphysics_save_constrainthinge_t");
			DefineField("worldPosition", POSITION_VECTOR);
			DefineField("worldAxisDirection", VECTOR);
			DefineEmbeddedField("constraint", "vphysics_save_constraintbreakable_t");
			DefineEmbeddedField("hingeAxis", "vphysics_save_constraintaxislimit_t");
			
			BeginDataMap("vphysics_save_constraintballsocket_t");
			DefineField("constraintPosition", VECTOR, 2);
			DefineEmbeddedField("constraint", "vphysics_save_constraintbreakable_t");
		}
	}
}