// ReSharper disable All

using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public class EntMaps5 : DataMapInfoGenerator {

		public const int RAGDOLL_MAX_ELEMENTS = 24;
		public const int VPHYSICS_MAX_OBJECT_LIST_COUNT = 1024;
		
		
		private void DefineRagdollElement(int i) {
			DefineField($"m_ragdoll.list[{i}].originParentSpace", VECTOR);
			DefinePhysPtr($"m_ragdoll.list[{i}].pObject");
			DefinePhysPtr($"m_ragdoll.list[{i}].pConstraint");
			DefineField($"m_ragdoll.list[{i}].parentIndex", INTEGER);
		}
		

		protected override void GenerateDataMaps() {
			BeginDataMap("CBaseButton", "CBaseToggle");
			DefineKeyField("m_vecMoveDir", "movedir", VECTOR);
			DefineField("m_fStayPushed", BOOLEAN);
			DefineField("m_fRotating", BOOLEAN);
			DefineField("m_bLockedSound", CHARACTER);
			DefineField("m_bLockedSentence", CHARACTER);
			DefineField("m_bUnlockedSound", CHARACTER);
			DefineField("m_bUnlockedSentence", CHARACTER);
			DefineField("m_bLocked", BOOLEAN);
			DefineField("m_sNoise", SOUNDNAME);
			DefineField("m_flUseLockedTime", TIME);
			DefineField("m_bSolidBsp", BOOLEAN);
			DefineKeyField("m_sounds", "sounds", INTEGER);
			DefineFunction("ButtonTouch");
			DefineFunction("ButtonSpark");
			DefineFunction("TriggerAndWait");
			DefineFunction("ButtonReturn");
			DefineFunction("ButtonBackHome");
			DefineFunction("ButtonUse");
			DefineInputFunc("Lock", "InputLock", VOID);
			DefineInputFunc("Unlock", "InputUnlock", VOID);
			DefineInputFunc("Press", "InputPress", VOID);
			DefineInputFunc("PressIn", "InputPressIn", VOID);
			DefineInputFunc("PressOut", "InputPressOut", VOID);
			DefineOutput("m_OnDamaged", "OnDamaged");
			DefineOutput("m_OnPressed", "OnPressed");
			DefineOutput("m_OnUseLocked", "OnUseLocked");
			DefineOutput("m_OnIn", "OnIn");
			DefineOutput("m_OnOut", "OnOut");
			
			DataMapProxy("CRotButton", "CBaseButton");
			LinkNamesToMap("func_rot_button");
			
			BeginDataMap("CFuncRotating", "CBaseEntity");
			LinkNamesToMap("func_rotating");
			DefineField("m_vecMoveAng", VECTOR);
			DefineField("m_flFanFriction", FLOAT);
			DefineField("m_flAttenuation", FLOAT);
			DefineField("m_flVolume", FLOAT);
			DefineField("m_flTargetSpeed", FLOAT);
			DefineKeyField("m_flMaxSpeed", "maxspeed", FLOAT);
			DefineKeyField("m_flBlockDamage", "dmg", FLOAT);
			DefineKeyField("m_NoiseRunning", "message", SOUNDNAME);
			DefineField("m_bReversed", BOOLEAN);
			DefineField("m_angStart", VECTOR);
			DefineField("m_bStopAtStartPos", BOOLEAN);
			DefineKeyField("m_bSolidBsp", "solidbsp", BOOLEAN);
			DefineFunction("SpinUpMove");
			DefineFunction("SpinDownMove");
			DefineFunction("HurtTouch");
			DefineFunction("RotatingUse");
			DefineFunction("RotateMove");
			DefineFunction("ReverseMove");
			DefineInputFunc("SetSpeed", "InputSetSpeed", FLOAT);
			DefineInputFunc("Start", "InputStart", VOID);
			DefineInputFunc("Stop", "InputStop", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("Reverse", "InputReverse", VOID);
			DefineInputFunc("StartForward", "InputStartForward", VOID);
			DefineInputFunc("StartBackward", "InputStartBackward", VOID);
			DefineInputFunc("StopAtStartPos", "InputStopAtStartPos", VOID);
			
			DataMapProxy("CBaseParticleEntity", "CBaseEntity");
			
			BeginDataMap("CRotDoor", "CBaseDoor");
			LinkNamesToMap("func_door_rotating");
			DefineKeyField("m_bSolidBsp", "solidbsp", BOOLEAN);
			
			BeginDataMap("CFilterClass", "CBaseFilter");
			LinkNamesToMap("filter_activator_class");
			DefineKeyField("m_iFilterClass", "filterclass", STRING);
			
			BeginDataMap("locksound_t");
			DefineField("sLockedSound", STRING);
			DefineField("sLockedSentence", STRING);
			DefineField("sUnlockedSound", STRING);
			DefineField("sUnlockedSentence", STRING);
			DefineField("iLockedSentence", INTEGER);
			DefineField("iUnlockedSentence", INTEGER);
			DefineField("flwaitSound", FLOAT);
			DefineField("flwaitSentence", FLOAT);
			DefineField("bEOFLocked", CHARACTER);
			DefineField("bEOFUnlocked", CHARACTER);
			
			BeginDataMap("CBasePropDoor", "CDynamicProp");
			DefineKeyField("m_nHardwareType", "hardware", INTEGER);
			DefineKeyField("m_flAutoReturnDelay", "returndelay", FLOAT);
			DefineField("m_hActivator", EHANDLE);
			DefineKeyField("m_SoundMoving", "soundmoveoverride", SOUNDNAME);
			DefineKeyField("m_SoundOpen", "soundopenoverride", SOUNDNAME);
			DefineKeyField("m_SoundClose", "soundcloseoverride", SOUNDNAME);
			DefineKeyField("m_ls.sLockedSound", "soundlockedoverride", SOUNDNAME);
			DefineKeyField("m_ls.sUnlockedSound", "soundunlockedoverride", SOUNDNAME);
			DefineKeyField("m_SlaveName", "slavename", STRING);
			DefineField("m_bLocked", BOOLEAN);
			DefineKeyField("m_bForceClosed", "forceclosed", BOOLEAN);
			DefineField("m_eDoorState", INTEGER);
			DefineField("m_hMaster", EHANDLE);
			DefineField("m_hBlocker", EHANDLE);
			DefineField("m_bFirstBlocked", BOOLEAN);
			DefineInputFunc("Open", "InputOpen", VOID);
			DefineInputFunc("OpenAwayFrom", "InputOpenAwayFrom", STRING);
			DefineInputFunc("Close", "InputClose", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("Lock", "InputLock", VOID);
			DefineInputFunc("Unlock", "InputUnlock", VOID);
			DefineOutput("m_OnBlockedOpening", "OnBlockedOpening");
			DefineOutput("m_OnBlockedClosing", "OnBlockedClosing");
			DefineOutput("m_OnUnblockedOpening", "OnUnblockedOpening");
			DefineOutput("m_OnUnblockedClosing", "OnUnblockedClosing");
			DefineOutput("m_OnFullyClosed", "OnFullyClosed");
			DefineOutput("m_OnFullyOpen", "OnFullyOpen");
			DefineOutput("m_OnClose", "OnClose");
			DefineOutput("m_OnOpen", "OnOpen");
			DefineOutput("m_OnLockedUse", "OnLockedUse");
			DefineEmbeddedField("m_ls", "locksound_t");
			DefineThinkFunc("DoorOpenMoveDone");
			DefineThinkFunc("DoorCloseMoveDone");
			DefineThinkFunc("DoorAutoCloseThink");
			
			BeginDataMap("CPropDoorRotating", "CBasePropDoor");
			LinkNamesToMap("prop_door_rotating");
			DefineKeyField("m_eSpawnPosition", "spawnpos", INTEGER);
			DefineKeyField("m_eOpenDirection", "opendir", INTEGER);
			DefineKeyField("m_vecAxis", "axis", VECTOR);
			DefineKeyField("m_flDistance", "distance", FLOAT);
			DefineKeyField("m_angRotationAjar", "ajarangles", VECTOR);
			DefineField("m_angRotationClosed", VECTOR);
			DefineField("m_angRotationOpenForward", VECTOR);
			DefineField("m_angRotationOpenBack", VECTOR);
			DefineField("m_angGoal", VECTOR);
			DefineField("m_hDoorBlocker", EHANDLE);
			DefineInputFunc("SetRotationDistance", "InputSetRotationDistance", FLOAT);
			DefineInputFunc("SetSpeed", "InputSetSpeed", FLOAT);
			
			BeginDataMap("CTimerEntity", "CLogicalEntity");
			LinkNamesToMap("logic_timer");
			DefineKeyField("m_iDisabled", "StartDisabled", INTEGER);
			DefineKeyField("m_flRefireTime", "RefireTime", FLOAT);
			DefineField("m_bUpDownState", BOOLEAN);
			DefineInputFunc("RefireTime", "InputRefireTime", FLOAT);
			DefineInputFunc("FireTimer", "InputFireTimer", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("AddToTimer", "InputAddToTimer", FLOAT);
			DefineInputFunc("ResetTimer", "InputResetTimer", VOID);
			DefineInputFunc("SubtractFromTimer", "InputSubtractFromTimer", FLOAT);
			DefineInput("m_iUseRandomTime", "UseRandomTime", INTEGER);
			DefineInput("m_flLowerRandomBound", "LowerRandomBound", FLOAT);
			DefineInput("m_flUpperRandomBound", "UpperRandomBound", FLOAT);
			DefineOutput("m_OnTimer", "OnTimer");
			DefineOutput("m_OnTimerHigh", "OnTimerHigh");
			DefineOutput("m_OnTimerLow", "OnTimerLow");
			
			BeginDataMap("CWaterLODControl", "CBaseEntity");
			LinkNamesToMap("water_lod_control");
			DefineInputAndKeyField("m_flCheapWaterStartDistance", "cheapwaterstartdistance", "SetCheapWaterStartDistance", FLOAT);
			DefineInputAndKeyField("m_flCheapWaterEndDistance", "cheapwaterenddistance", "SetCheapWaterEndDistance", FLOAT);
			
			BeginDataMap("CPathCorner", "CPointEntity");
			LinkNamesToMap("path_corner");
			DefineKeyField("m_flWait", "wait", FLOAT);
			DefineInputFunc("SetNextPathCorner", "InputSetNextPathCorner", STRING);
			DefineInputFunc("InPass", "InputInPass", VOID);
			DefineOutput("m_OnPass", "OnPass");
			
			BeginDataMap("CDinosaurSignal", "CBaseEntity");
			LinkNamesToMap("updateitem1");
			DefineField("m_szSoundName", CHARACTER, 128);
			DefineField("m_flOuterRadius", FLOAT);
			DefineField("m_flInnerRadius", FLOAT);
			DefineField("m_nSignalID", INTEGER);
			
			BeginDataMap("CPortal_Dinosaur", "CPhysicsProp");
			LinkNamesToMap("updateitem2");
			DefineField("m_hDinosaur_Signal", EHANDLE);
			DefineField("m_bAlreadyDiscovered", BOOLEAN);
			
			BeginDataMap("CRagdollProp", "CBaseAnimating");
			LinkNamesToMap("prop_ragdoll", "physics_prop_ragdoll");
			DefineField("m_ragdoll.boneIndex", INTEGER, RAGDOLL_MAX_ELEMENTS);
			DefineField("m_ragPos", POSITION_VECTOR, RAGDOLL_MAX_ELEMENTS);
			DefineField("m_ragAngles", VECTOR, RAGDOLL_MAX_ELEMENTS);
			DefineKeyField("m_anglesOverrideString", "angleOverride", STRING);
			DefineField("m_lastUpdateTickCount", INTEGER);
			DefineField("m_allAsleep", BOOLEAN);
			DefineField("m_hDamageEntity", EHANDLE);
			DefineField("m_hKiller", EHANDLE);
			DefineKeyField("m_bStartDisabled", "StartDisabled", BOOLEAN);
			DefineInputFunc("StartRagdollBoogie", "InputStartRadgollBoogie", VOID);
			DefineInputFunc("EnableMotion", "InputEnableMotion", VOID);
			DefineInputFunc("DisableMotion", "InputDisableMotion", VOID);
			DefineInputFunc("Enable", "InputTurnOn", VOID);
			DefineInputFunc("Disable", "InputTurnOff", VOID);
			DefineInputFunc("FadeAndRemove", "InputFadeAndRemove", FLOAT);
			DefineField("m_hUnragdoll", EHANDLE);
			DefineField("m_bFirstCollisionAfterLaunch", BOOLEAN);
			DefineField("m_flBlendWeight", FLOAT);
			DefineField("m_nOverlaySequence", INTEGER);
			DefineField("m_ragdollMins", VECTOR, RAGDOLL_MAX_ELEMENTS);
			DefineField("m_ragdollMaxs", VECTOR, RAGDOLL_MAX_ELEMENTS);
			DefineField("m_hPhysicsAttacker", EHANDLE);
			DefineField("m_flLastPhysicsInfluenceTime", TIME);
			DefineField("m_flFadeOutStartTime", TIME);
			DefineField("m_flFadeTime", FLOAT);
			DefineField("m_strSourceClassName", STRING);
			DefineField("m_bHasBeenPhysgunned", BOOLEAN);
			DefineThinkFunc("SetDebrisThink");
			DefineThinkFunc("ClearFlagsThink");
			DefineThinkFunc("FadeOutThink");
			DefineField("m_ragdoll.listCount", INTEGER);
			DefineField("m_ragdoll.allowStretch", BOOLEAN);
			DefinePhysPtr("m_ragdoll.pGroup");
			DefineField("m_flDefaultFadeScale", FLOAT);
			for (int i = 0; i < 24; i++)
				DefineRagdollElement(i);
			
			DataMapProxy("CFuncIllusionary", "CBaseEntity"); // A simple entity that looks solid but lets you walk through it.
			LinkNamesToMap("func_illusionary");
			
			DataMapProxy("CEntityBlocker", "CBaseEntity");
			LinkNamesToMap("entity_blocker");
			
			BeginDataMap("FilterDamageType", "CBaseFilter");
			LinkNamesToMap("filter_damage_type");
			DefineKeyField("m_iDamageType", "damagetype", INTEGER); // enum?
			
			DataMapProxy("CSimplePhysicsProp", "CBaseProp");
			LinkNamesToMap("simple_physics_prop");
			
			BeginDataMap("game_shadowcontrol_params_t");
			DefineField("targetPosition", POSITION_VECTOR);
			DefineField("targetRotation", VECTOR);
			DefineField("maxAngular", FLOAT);
			DefineField("maxDampAngular", FLOAT);
			DefineField("maxSpeed", FLOAT);
			DefineField("maxDampSpeed", FLOAT);
			DefineField("dampFactor", FLOAT);
			DefineField("teleportDistance", FLOAT);
			
			BeginDataMap("CGrabController");
			DefineEmbeddedField("m_shadow", "game_shadowcontrol_params_t");
			DefineField("m_timeToArrive", FLOAT);
			DefineField("m_errorTime", FLOAT);
			DefineField("m_error", FLOAT);
			DefineField("m_contactAmount", FLOAT);
			DefineField("m_savedRotDamping", FLOAT, VPHYSICS_MAX_OBJECT_LIST_COUNT);
			DefineField("m_savedMass", FLOAT, VPHYSICS_MAX_OBJECT_LIST_COUNT);
			DefineField("m_flLoadWeight", FLOAT);
			DefineField("m_bCarriedEntityBlocksLOS", BOOLEAN);
			DefineField("m_bIgnoreRelativePitch", BOOLEAN);
			DefineField("m_attachedEntity", EHANDLE);
			DefineField("m_angleAlignment", FLOAT);
			DefineField("m_vecPreferredCarryAngles", VECTOR);
			DefineField("m_bHasPreferredCarryAngles", BOOLEAN);
			DefineField("m_flDistanceOffset", FLOAT);
			DefineField("m_attachedAnglesPlayerSpace", VECTOR);
			DefineField("m_attachedPositionObjectSpace", VECTOR);
			DefineField("m_bAllowObjectOverhead", BOOLEAN);
			
			BeginDataMap("CPlayerPickupController", "CBaseEntity");
			LinkNamesToMap("player_pickup");
			DefineEmbeddedField("m_grabController", "CGrabController");
			DefinePhysPtr("m_grabController.m_controller");
			DefineField("m_pPlayer", CLASSPTR);
			
			BeginDataMap("CFuncTank", "CBaseEntity");
			DefineKeyField("m_yawRate", "yawrate", FLOAT);
			DefineKeyField("m_yawRange", "yawrange", FLOAT);
			DefineKeyField("m_yawTolerance", "yawtolerance", FLOAT);
			DefineKeyField("m_pitchRate", "pitchrate", FLOAT);
			DefineKeyField("m_pitchRange", "pitchrange", FLOAT);
			DefineKeyField("m_pitchTolerance", "pitchtolerance", FLOAT);
			DefineKeyField("m_fireRate", "firerate", FLOAT);
			DefineField("m_fireTime", TIME);
			DefineKeyField("m_persist", "persistence", FLOAT);
			DefineKeyField("m_persist2", "persistence2", FLOAT);
			DefineKeyField("m_minRange", "minRange", FLOAT);
			DefineKeyField("m_maxRange", "maxRange", FLOAT);
			DefineField("m_flMinRange2", FLOAT);
			DefineField("m_flMaxRange2", FLOAT);
			DefineKeyField("m_iAmmoCount", "ammo_count", INTEGER);
			DefineKeyField("m_spriteScale", "spritescale", FLOAT);
			DefineKeyField("m_iszSpriteSmoke", "spritesmoke", STRING);
			DefineKeyField("m_iszSpriteFlash", "spriteflash", STRING);
			DefineKeyField("m_bulletType", "bullet", INTEGER);
			DefineField("m_nBulletCount", INTEGER);
			DefineKeyField("m_spread", "firespread", INTEGER);
			DefineKeyField("m_iBulletDamage", "bullet_damage", INTEGER);
			DefineKeyField("m_iBulletDamageVsPlayer", "bullet_damage_vs_player", INTEGER);
			DefineKeyField("m_iszMaster", "master", STRING);
			if (GenInfo.IsDefHl2Episodic) {
				DefineKeyField("m_iszAmmoType", "ammotype", STRING);
				DefineField("m_iAmmoType", INTEGER);
			} else {
				DefineField("m_iSmallAmmoType", INTEGER);
				DefineField("m_iMediumAmmoType", INTEGER);
				DefineField("m_iLargeAmmoType", INTEGER);
			}
			DefineKeyField("m_soundStartRotate", "rotatestartsound", SOUNDNAME);
			DefineKeyField("m_soundStopRotate", "rotatestopsound", SOUNDNAME);
			DefineKeyField("m_soundLoopRotate", "rotatesound", SOUNDNAME);
			DefineKeyField("m_flPlayerGracePeriod", "playergraceperiod", FLOAT);
			DefineKeyField("m_flIgnoreGraceUpto", "ignoregraceupto", FLOAT);
			DefineKeyField("m_flPlayerLockTimeBeforeFire", "playerlocktimebeforefire", FLOAT);
			DefineField("m_flLastSawNonPlayer", TIME);
			DefineField("m_yawCenter", FLOAT);
			DefineField("m_yawCenterWorld", FLOAT);
			DefineField("m_pitchCenter", FLOAT);
			DefineField("m_pitchCenterWorld", FLOAT);
			DefineField("m_fireLast", TIME);
			DefineField("m_lastSightTime", TIME);
			DefineField("m_barrelPos", VECTOR);
			DefineField("m_sightOrigin", POSITION_VECTOR);
			DefineField("m_hFuncTankTarget", EHANDLE);
			DefineField("m_hController", EHANDLE);
			DefineField("m_vecControllerUsePos", VECTOR);
			DefineField("m_flNextAttack", TIME);
			DefineField("m_targetEntityName", STRING);
			DefineField("m_hTarget", EHANDLE);
			DefineField("m_vTargetPosition", POSITION_VECTOR);
			DefineField("m_vecNPCIdleTarget", POSITION_VECTOR);
			DefineField("m_persist2burst", FLOAT);
			DefineField("m_hControlVolume", EHANDLE);
			DefineKeyField("m_iszControlVolume", "control_volume", STRING);
			DefineField("m_flNextControllerSearch", TIME);
			DefineField("m_bShouldFindNPCs", BOOLEAN);
			DefineField("m_bNPCInRoute", BOOLEAN);
			DefineKeyField("m_iszNPCManPoint", "npc_man_point", STRING);
			DefineField("m_bReadyToFire", BOOLEAN);
			DefineKeyField("m_bPerformLeading", "LeadTarget", BOOLEAN);
			DefineField("m_flStartLeadFactor", FLOAT);
			DefineField("m_flStartLeadFactorTime", TIME);
			DefineField("m_flNextLeadFactor", FLOAT);
			DefineField("m_flNextLeadFactorTime", TIME);
			DefineKeyField("m_iszBaseAttachment", "gun_base_attach", STRING);
			DefineKeyField("m_iszBarrelAttachment", "gun_barrel_attach", STRING);
			DefineKeyField("m_iszYawPoseParam", "gun_yaw_pose_param", STRING);
			DefineKeyField("m_iszPitchPoseParam", "gun_pitch_pose_param", STRING);
			DefineKeyField("m_flYawPoseCenter", "gun_yaw_pose_center", FLOAT);
			DefineKeyField("m_flPitchPoseCenter", "gun_pitch_pose_center", FLOAT);
			DefineField("m_bUsePoseParameters", BOOLEAN);
			DefineKeyField("m_iEffectHandling", "effecthandling", INTEGER);
			DefineInputFunc("Activate", "InputActivate", VOID);
			DefineInputFunc("Deactivate", "InputDeactivate", VOID);
			DefineInputFunc("SetFireRate", "InputSetFireRate", FLOAT);
			DefineInputFunc("SetDamage", "InputSetDamage", INTEGER);
			DefineInputFunc("SetTargetPosition", "InputSetTargetPosition", VECTOR);
			DefineInputFunc("SetTargetDir", "InputSetTargetDir", VECTOR);
			DefineInputFunc("SetTargetEntityName", "InputSetTargetEntityName", STRING);
			DefineInputFunc("SetTargetEntity", "InputSetTargetEntity", EHANDLE);
			DefineInputFunc("ClearTargetEntity", "InputClearTargetEntity", VOID);
			DefineInputFunc("FindNPCToManTank", "InputFindNPCToManTank", STRING);
			DefineInputFunc("StopFindingNPCs", "InputStopFindingNPCs", VOID);
			DefineInputFunc("StartFindingNPCs", "InputStartFindingNPCs", VOID);
			DefineInputFunc("ForceNPCOff", "InputForceNPCOff", VOID);
			DefineInputFunc("SetMaxRange", "InputSetMaxRange", FLOAT);
			DefineOutput("m_OnFire", "OnFire");
			DefineOutput("m_OnLoseTarget", "OnLoseTarget");
			DefineOutput("m_OnAquireTarget", "OnAquireTarget");
			DefineOutput("m_OnAmmoDepleted", "OnAmmoDepleted");
			DefineOutput("m_OnGotController", "OnGotController");
			DefineOutput("m_OnLostController", "OnLostController");
			DefineOutput("m_OnGotPlayerController", "OnGotPlayerController");
			DefineOutput("m_OnLostPlayerController", "OnLostPlayerController");
			DefineOutput("m_OnReadyToFire", "OnReadyToFire");
			
			DataMapProxy("CFuncDataGun", "CFuncTank");
			LinkNamesToMap("func_tank");
			
			BeginDataMap("CFuncAreaPortalWindow", "CFuncAreaPortalBase");
			LinkNamesToMap("func_areaportalwindow");
			DefineKeyField("m_portalNumber", "portalnumber", INTEGER);
			DefineKeyField("m_flFadeStartDist", "FadeStartDist", FLOAT);
			DefineKeyField("m_flFadeDist", "FadeDist", FLOAT);
			DefineKeyField("m_flTranslucencyLimit", "TranslucencyLimit", FLOAT);
			DefineKeyField("m_iBackgroundBModelName", "BackgroundBModel", STRING);
			DefineInputFunc("SetFadeStartDistance", "InputSetFadeStartDistance", FLOAT);
			DefineInputFunc("SetFadeEndDistance", "InputSetFadeEndDistance", FLOAT);
		}
	}
}