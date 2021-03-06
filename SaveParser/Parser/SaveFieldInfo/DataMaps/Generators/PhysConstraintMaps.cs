// ReSharper disable All

using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public class PhysConstraintMaps : DataMapInfoGenerator {

		public const int MAX_NUM_PANELS = 16;
		
		
		protected override void GenerateDataMaps() {
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
			LinkNamesToMap("func_breakable");
			DefineField("m_Material", INTEGER);
			DefineKeyField("m_Explosion", "explosion", INTEGER);
			DefineKeyField("m_GibDir", "gibdir", VECTOR);
			DefineField("m_hBreaker", EHANDLE);
			if (Game != Game.PORTAL2)
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
			if (Game == Game.PORTAL2)
				DefineField("m_flDmgModFire", FLOAT);
			
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
			DefineInputFunc("EnableMotion", "InputEnableMotion", VOID);
			DefineInputFunc("DisableMotion", "InputDisableMotion", VOID);
			DefineInputFunc("Wake", "InputWake", VOID);
			DefineInputFunc("Sleep", "InputSleep", VOID);
			DefineInputFunc("DisableFloating", "InputDisableFloating", VOID);
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
			if (Game == Game.PORTAL2)
				DefineField("m_bAllowPortalFunnel", BOOLEAN);
			
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
			if (Game == Game.PORTAL2)
				DefineField("m_flDmgModFire", FLOAT);
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
			DefineInputFunc("Break", "InputBreak", VOID);
			DefineInputFunc("SetHealth", "InputSetHealth", INTEGER);
			DefineInputFunc("AddHealth", "InputAddHealth", INTEGER);
			DefineInputFunc("RemoveHealth", "InputRemoveHealth", INTEGER);
			DefineInput("m_impactEnergyScale", "physdamagescale", FLOAT);
			DefineInputFunc("EnablePhyscannonPickup", "InputEnablePhyscannonPickup", VOID);
			DefineInputFunc("DisablePhyscannonPickup", "InputDisablePhyscannonPickup", VOID);
			DefineInputFunc("EnablePuntSound", "InputEnablePuntSound", VOID);
			DefineInputFunc("DisablePuntSound", "InputDisablePuntSound", VOID);
			DefineOutput("m_OnBreak", "OnBreak");
			DefineOutput("m_OnHealthChanged", "OnHealthChanged");
			DefineOutput("m_OnTakeDamage", "OnTakeDamage");
			DefineOutput("m_OnPhysCannonDetach", "OnPhysCannonDetach");
			DefineOutput("m_OnPhysCannonAnimatePreStarted", "OnPhysCannonAnimatePreStarted");
			DefineOutput("m_OnPhysCannonAnimatePullStarted", "OnPhysCannonAnimatePullStarted");
			DefineOutput("m_OnPhysCannonAnimatePostStarted", "OnPhysCannonAnimatePostStarted");
			DefineOutput("m_OnPhysCannonPullAnimFinished", "OnPhysCannonPullAnimFinished");
			DefineThinkFunc("BreakThink");
			DefineThinkFunc("AnimateThink");
			DefineThinkFunc("RampToDefaultFadeScale");
			DefineEntityFunc("BreakablePropTouch");
			DefineField("m_hPhysicsAttacker", EHANDLE);
			DefineField("m_flLastPhysicsInfluenceTime", TIME);
			DefineField("m_bOriginalBlockLOS", BOOLEAN);
			DefineField("m_bBlockLOSSetByPropData", BOOLEAN);
			DefineField("m_bIsWalkableSetByPropData", BOOLEAN);
			DefineField("m_hLastAttacker", EHANDLE);
			DefineField("m_hFlareEnt", EHANDLE);
			
			BeginDataMap("CBreakableSurface", "CBreakable");
			LinkNamesToMap("func_breakable_surf");
			DefineKeyField("m_nSurfaceType", "surfacetype", INTEGER);
			DefineKeyField("m_nFragility", "fragility", INTEGER);
			DefineKeyField("m_vLLVertex", "lowerleft", VECTOR);
			DefineKeyField("m_vULVertex", "upperleft", VECTOR);
			DefineKeyField("m_vLRVertex", "lowerright", VECTOR);
			DefineKeyField("m_vURVertex", "upperright", VECTOR);
			DefineKeyField("m_nQuadError", "error", INTEGER);
			DefineField("m_nNumWide", INTEGER);
			DefineField("m_nNumHigh", INTEGER);
			DefineField("m_flPanelWidth", FLOAT);
			DefineField("m_flPanelHeight", FLOAT);
			DefineField("m_vNormal", VECTOR);
			DefineField("m_vCorner", POSITION_VECTOR);
			DefineField("m_bIsBroken", BOOLEAN);
			DefineField("m_nNumBrokenPanes", INTEGER);
			DefineField("m_flSupport", FLOAT, MAX_NUM_PANELS * MAX_NUM_PANELS);
			DefineField("m_RawPanelBitVec", BOOLEAN, MAX_NUM_PANELS * MAX_NUM_PANELS);
			DefineThinkFunc("BreakThink");
			DefineEntityFunc("SurfaceTouch");
			DefineInputFunc("Shatter", "InputShatter", VECTOR);
			
			BeginDataMap("CFuncVPhysicsClip", "CBaseEntity");
			LinkNamesToMap("func_clip_vphysics");
			DefineKeyField("m_iFilterName", "filtername", STRING);
			DefineField("m_hFilter", EHANDLE);
			DefineField("m_bDisabled", BOOLEAN);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			
			BeginDataMap("CPhysicsSpring", "CBaseEntity");
			LinkNamesToMap("phys_spring");
			DefinePhysPtr("m_pSpring");
			DefineKeyField("m_tempConstant", "constant", FLOAT);
			DefineKeyField("m_tempLength", "length", FLOAT);
			DefineKeyField("m_tempDamping", "damping", FLOAT);
			DefineKeyField("m_tempRelativeDamping", "relativedamping", FLOAT);
			DefineKeyField("m_nameAttachStart", "attach1", STRING);
			DefineKeyField("m_nameAttachEnd", "attach2", STRING);
			DefineField("m_start", POSITION_VECTOR);
			DefineKeyField("m_end", "springaxis", POSITION_VECTOR);
			DefineField("m_isLocal", BOOLEAN);
			DefineInputFunc("SetSpringConstant", "InputSetSpringConstant", FLOAT);
			DefineInputFunc("SetSpringLength", "InputSetSpringLength", FLOAT);
			DefineInputFunc("SetSpringDamping", "InputSetSpringDamping", FLOAT);
		}
	}
}