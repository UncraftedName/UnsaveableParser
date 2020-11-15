// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public class PhysConstraintMaps : DataMapGenerator {
		
		protected override void CreateDataMaps() {
			BeginDataMap("CPhysConstraint", "CLogicalEntity");
			DefinePhysPtr("m_pConstraint");
			DefineKeyField("m_nameSystem", "constraintsystem", STRING);
			DefineKeyField("m_nameAttach1", "attach1", STRING);
			DefineKeyField("m_nameAttach2", "attach2", STRING);
			DefineKeyField("m_breakSound", "breaksound", SOUNDNAME);
			DefineKeyField("m_forceLimit", "forcelimit", FLOAT);
			DefineKeyField("m_torqueLimit", "torquelimit", FLOAT);
			DefineKeyField("m_minTeleportDistance", "teleportfollowdistance", FLOAT);
			DefineOutput("m_OnBreak", "OnBreak");
			DefineInputFunc("Break", "InputBreak", VOID);
			DefineInputFunc("ConstraintBroken", "InputOnBreak", VOID);
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			
			DataMapProxy("CPhysBallSocket", "CPhysConstraint");
			LinkNamesToMap("phys_ballsocket");
			
			BeginDataMap("CPhysSlideContraint", "CPhysConstraint");
			LinkNamesToMap("phys_slideconstraint");
			DefineKeyField("m_axisEnd", "slideaxis", POSITION_VECTOR);
			DefineKeyField("m_slideFriction", "slidefriction", FLOAT);
			DefineKeyField("m_systemLoadScale", "systemloadscale", FLOAT);
			DefineInputFunc("SetVelocity", "InputSetVelocity", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_keyPoints[SimpleConstraintSoundProfile::kMIN_THRESHOLD]", "minSoundThreshold", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_keyPoints[SimpleConstraintSoundProfile::kMIN_FULL]", "maxSoundThreshold", FLOAT);
			DefineKeyField("m_soundInfo.m_iszTravelSoundFwd", "slidesoundfwd", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszTravelSoundBack", "slidesoundback", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[0]", "reversalsoundSmall", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[1]", "reversalsoundMedium", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[2]", "reversalsoundLarge", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[0]", "reversalsoundthresholdSmall", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[1]", "reversalsoundthresholdMedium", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[2]", "reversalsoundthresholdLarge", FLOAT);
			DefineThinkFunc("SoundThink");
			
			DataMapProxy("CPhysFixed", "CPhysConstraint");
			LinkNamesToMap("phys_constraint");

			BeginDataMap("CConstraintAnchor", "CPointEntity");
			LinkNamesToMap("info_constraint_anchor");
			DefineKeyField("m_massScale", "massScale", FLOAT);

			BeginDataMap("CPhysConstraintSystem", "CLogicalEntity");
			LinkNamesToMap("phys_constraintsystem");
			DefinePhysPtr("m_pMachine");
			DefineKeyField("m_additionalIterations", "additionaliterations", INTEGER);

			BeginDataMap("CPhysHinge", "CPhysConstraint");
			LinkNamesToMap("phys_hinge");
			DefineKeyField("m_hingeFriction", "hingefriction", FLOAT);
			DefineField("m_hinge.worldPosition", POSITION_VECTOR);
			DefineKeyField("m_hinge.worldAxisDirection", "hingeaxis", VECTOR);
			DefineKeyField("m_systemLoadScale", "systemloadscale", FLOAT);
			DefineInputFunc("SetAngularVelocity", "InputSetVelocity", FLOAT);
			DefineInputFunc("SetHingeFriction", "InputSetHingeFriction", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_keyPoints[SimpleConstraintSoundProfile::kMIN_THRESHOLD]", "minSoundThreshold", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_keyPoints[SimpleConstraintSoundProfile::kMIN_FULL]", "maxSoundThreshold", FLOAT);
			DefineKeyField("m_soundInfo.m_iszTravelSoundFwd", "slidesoundfwd", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszTravelSoundBack", "slidesoundback", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[0]", "reversalsoundSmall", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[1]", "reversalsoundMedium", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[2]", "reversalsoundLarge", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[0]", "reversalsoundthresholdSmall", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[1]", "reversalsoundthresholdMedium", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[2]", "reversalsoundthresholdLarge", FLOAT);
			DefineThinkFunc("SoundThink");
			
			BeginDataMap("CPhysPulley", "CPhysConstraint");
			LinkNamesToMap("phys_pulleyconstraint");
			DefineKeyField("m_position2", "position2", POSITION_VECTOR);
			DefineField("m_offset", VECTOR, 2);
			DefineKeyField("m_addLength", "addlength", FLOAT);
			DefineKeyField("m_gearRatio", "gearratio", FLOAT);
			
			BeginDataMap("CPhysLength", "CPhysConstraint");
			LinkNamesToMap("phys_lengthconstraint");
			DefineField("m_offset", VECTOR, 2);
			DefineKeyField("m_addLength", "addlength", FLOAT);
			DefineKeyField("m_minLength", "minlength", FLOAT);
			DefineKeyField("m_vecAttach", "attachpoint", POSITION_VECTOR);
			DefineField("m_totalLength", FLOAT);
			
			BeginDataMap("CRagdollConstraint", "CPhysConstraint");
			LinkNamesToMap("phys_ragdollconstraint");
			DefineKeyField("m_xmin", "xmin", FLOAT);
			DefineKeyField("m_xmax", "xmax", FLOAT);
			DefineKeyField("m_ymin", "ymin", FLOAT);
			DefineKeyField("m_ymax", "ymax", FLOAT);
			DefineKeyField("m_zmin", "zmin", FLOAT);
			DefineKeyField("m_zmax", "zmax", FLOAT);
			DefineKeyField("m_xfriction", "xfriction", FLOAT);
			DefineKeyField("m_yfriction", "yfriction", FLOAT);
			DefineKeyField("m_zfriction", "zfriction", FLOAT);

			BeginDataMap("CBreakable", "CBaseEntity");
			DefineField("m_Material", INTEGER);
			DefineKeyField("m_Explosion", "explosion", INTEGER);
			DefineKeyField("m_GibDir", "gibdir", VECTOR);
			DefineField("m_hBreaker", EHANDLE);
			DefineField("m_angle", FLOAT);
			DefineField("m_iszGibModel", STRING);
			DefineField("m_iszSpawnObject", STRING);
			DefineKeyField("m_ExplosionMagnitude", "explodemagnitude", INTEGER);
			DefineKeyField("m_flPressureDelay", "PressureDelay", FLOAT);
			DefineKeyField("m_iMinHealthDmg", "minhealthdmg", INTEGER);
			DefineField("m_bTookPhysicsDamage", BOOLEAN);
			DefineField("m_iszPropData", STRING);
			DefineInput("m_impactEnergyScale", "physdamagescale", FLOAT);
			DefineKeyField("m_PerformanceMode", "PerformanceMode", INTEGER);
			DefineInputFunc("Break", "InputBreak", VOID);
			DefineInputFunc("SetHealth", "InputSetHealth", INTEGER);
			DefineInputFunc("AddHealth", "InputAddHealth", INTEGER);
			DefineInputFunc("RemoveHealth", "InputRemoveHealth", INTEGER);
			DefineInputFunc("SetMass", "InputSetMass", FLOAT);
			DefineEntityFunc("BreakTouch");
			DefineThinkFunc("Die");
			DefineOutput("m_OnBreak", "OnBreak");
			DefineOutput("m_OnHealthChanged", "OnHealthChanged");
			DefineField("m_flDmgModBullet", FLOAT);
			DefineField("m_flDmgModClub", FLOAT);
			DefineField("m_flDmgModExplosive", FLOAT);
			DefineField("m_iszPhysicsDamageTableName", STRING);
			DefineField("m_iszBreakableModel", STRING);
			DefineField("m_iBreakableSkin", INTEGER);
			DefineField("m_iBreakableCount", INTEGER);
			DefineField("m_iMaxBreakableSize", INTEGER);
			DefineField("m_iszBasePropData", STRING);
			DefineField("m_iInteractions", INTEGER);
			DefineField("m_explodeRadius", FLOAT);
			DefineField("m_iszModelName", STRING);
			DefineField("m_hPhysicsAttacker", EHANDLE);
			DefineField("m_flLastPhysicsInfluenceTime", TIME);
			
			BeginDataMap("CPhysBox", "CBreakable");
			LinkNamesToMap("func_physbox");
			DefineKeyField("m_massScale", "massScale", FLOAT);
			DefineKeyField("m_damageType", "Damagetype", INTEGER);
			DefineKeyField("m_iszOverrideScript", "overridescript", STRING);
			DefineKeyField("m_damageToEnableMotion", "damagetoenablemotion", INTEGER);
			DefineKeyField("m_flForceToEnableMotion", "forcetoenablemotion", FLOAT);
			DefineKeyField("m_angPreferredCarryAngles", "preferredcarryangles", VECTOR);
			DefineKeyField("m_bNotSolidToWorld", "notsolid", BOOLEAN);
			DefineInputFunc("Wake", "InputWake", VOID);
			DefineInputFunc("Sleep", "InputSleep", VOID);
			DefineInputFunc("EnableMotion", "InputEnableMotion", VOID);
			DefineInputFunc("DisableMotion", "InputDisableMotion", VOID);
			DefineInputFunc("ForceDrop", "InputForceDrop", VOID);
			DefineInputFunc("DisableFloating", "InputDisableFloating", VOID);
			DefineEntityFunc("BreakTouch");
			DefineOutput("m_OnDamaged", "OnDamaged");
			DefineOutput("m_OnAwakened", "OnAwakened");
			DefineOutput("m_OnMotionEnabled", "OnMotionEnabled");
			DefineOutput("m_OnPhysGunPickup", "OnPhysGunPickup");
			DefineOutput("m_OnPhysGunPunt", "OnPhysGunPunt");
			DefineOutput("m_OnPhysGunOnlyPickup", "OnPhysGunOnlyPickup");
			DefineOutput("m_OnPhysGunDrop", "OnPhysGunDrop");
			DefineOutput("m_OnPlayerUse", "OnPlayerUse");
			
			BeginDataMap("CPhysicsProp", "CBreakableProp");
			LinkNamesToMap("physics_prop", "prop_physics", "prop_physics_override");
			//DefineINPUTFUNC("EnableMotion", VOID);
			//DefineINPUTFUNC("DisableMotion", VOID);
			//DefineINPUTFUNC("Wake", VOID);
			//DefineINPUTFUNC("Sleep", VOID);
			//DefineINPUTFUNC("DisableFloating", VOID);
			DefineField("m_bAwake", BOOLEAN);
			DefineKeyField("m_massScale", "massscale", FLOAT);
			DefineKeyField("m_inertiaScale", "inertiascale", FLOAT);
			DefineKeyField("m_damageType", "Damagetype", INTEGER);
			DefineKeyField("m_iszOverrideScript", "overridescript", STRING);
			DefineKeyField("m_damageToEnableMotion", "damagetoenablemotion", INTEGER);
			DefineKeyField("m_flForceToEnableMotion", "forcetoenablemotion", FLOAT);
			DefineOutput("m_OnAwakened", "OnAwakened");
			DefineOutput("m_MotionEnabled", "OnMotionEnabled");
			DefineOutput("m_OnPhysGunPickup", "OnPhysGunPickup");
			DefineOutput("m_OnPhysGunOnlyPickup", "OnPhysGunOnlyPickup");
			DefineOutput("m_OnPhysGunPunt", "OnPhysGunPunt");
			DefineOutput("m_OnPhysGunDrop", "OnPhysGunDrop");
			DefineOutput("m_OnPlayerUse", "OnPlayerUse");
			DefineOutput("m_OnPlayerPickup", "OnPlayerPickup");
			DefineOutput("m_OnOutOfWorld", "OnOutOfWorld");
			DefineField("m_bThrownByPlayer", BOOLEAN);
			DefineField("m_bFirstCollisionAfterLaunch", BOOLEAN);
			DefineThinkFunc("ClearFlagsThink");
			
			BeginDataMap("CPhysExplosion", "CPointEntity");
			LinkNamesToMap("env_physexplosion");
			DefineKeyField("m_damage", "magnitude", FLOAT);
			DefineKeyField("m_radius", "radius", FLOAT);
			DefineKeyField("m_targetEntityName", "targetentityname", STRING);
			DefineKeyField("m_flInnerRadius", "inner_radius", FLOAT);
			DefineInputFunc("Explode", "InputExplode", VOID);
			DefineOutput("m_OnPushedPlayer", "OnPushedPlayer");

			BeginDataMap("CBreakableProp", "CBaseProp");
			DefineKeyField("m_explodeDamage", "ExplodeDamage", FLOAT);
			DefineKeyField("m_explodeRadius", "ExplodeRadius", FLOAT);
			DefineKeyField("m_iMinHealthDmg", "minhealthdmg", INTEGER);
			DefineField("m_createTick", INTEGER);
			DefineField("m_hBreaker", EHANDLE);
			DefineKeyField("m_PerformanceMode", "PerformanceMode", INTEGER);
			DefineField("m_flDmgModBullet", FLOAT);
			DefineField("m_flDmgModClub", FLOAT);
			DefineField("m_flDmgModExplosive", FLOAT);
			DefineField("m_iszPhysicsDamageTableName", STRING);
			DefineField("m_iszBreakableModel", STRING);
			DefineField("m_iBreakableSkin", INTEGER);
			DefineField("m_iBreakableCount", INTEGER);
			DefineField("m_iMaxBreakableSize", INTEGER);
			DefineField("m_iszBasePropData", STRING);
			DefineField("m_iInteractions", INTEGER);
			DefineField("m_iNumBreakableChunks", INTEGER);
			DefineField("m_nPhysgunState", BYTE);
			DefineKeyField("m_iszPuntSound", "puntsound", STRING);
			DefineKeyField("m_flPressureDelay", "PressureDelay", FLOAT);
			DefineField("m_preferredCarryAngles", VECTOR);
			DefineField("m_flDefaultFadeScale", FLOAT);
			DefineField("m_bUsePuntSound", BOOLEAN);
			//DefineINPUTFUNC("Break", VOID);
			//DefineINPUTFUNC("SetHealth", INTEGER);
			//DefineINPUTFUNC("AddHealth", INTEGER);
			//DefineINPUTFUNC("RemoveHealth", INTEGER);
			DefineInput("m_impactEnergyScale", "physdamagescale", FLOAT);
			//DefineINPUTFUNC("EnablePhyscannonPickup", VOID);
			//DefineINPUTFUNC("DisablePhyscannonPickup", VOID);
			//DefineINPUTFUNC("EnablePuntSound", VOID);
			//DefineINPUTFUNC("DisablePuntSound", VOID);
			DefineOutput("m_OnBreak", "OnBreak");
			DefineOutput("m_OnHealthChanged", "OnHealthChanged");
			DefineOutput("m_OnTakeDamage", "OnTakeDamage");
			DefineOutput("m_OnPhysCannonDetach", "OnPhysCannonDetach");
			DefineOutput("m_OnPhysCannonAnimatePreStarted", "OnPhysCannonAnimatePreStarted");
			DefineOutput("m_OnPhysCannonAnimatePullStarted", "OnPhysCannonAnimatePullStarted");
			DefineOutput("m_OnPhysCannonAnimatePostStarted", "OnPhysCannonAnimatePostStarted");
			DefineOutput("m_OnPhysCannonPullAnimFinished", "OnPhysCannonPullAnimFinished");
			//DEFINE_THINKFUNC( BreakThink ),
			//DEFINE_THINKFUNC( AnimateThink ),
			//DEFINE_THINKFUNC( RampToDefaultFadeScale ),
			//DEFINE_ENTITYFUNC( BreakablePropTouch ),
			DefineField("m_hPhysicsAttacker", EHANDLE);
			DefineField("m_flLastPhysicsInfluenceTime", TIME);
			DefineField("m_bOriginalBlockLOS", BOOLEAN);
			DefineField("m_bBlockLOSSetByPropData", BOOLEAN);
			DefineField("m_bIsWalkableSetByPropData", BOOLEAN);
			DefineField("m_hLastAttacker", EHANDLE);
			DefineField("m_hFlareEnt", EHANDLE);
		}
	}
}