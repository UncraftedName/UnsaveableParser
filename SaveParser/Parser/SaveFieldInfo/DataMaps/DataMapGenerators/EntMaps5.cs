// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public class EntMaps5 : DataMapGenerator {

		public const int RAGDOLL_MAX_ELEMENTS = 24;
		public const int VPHYSICS_MAX_OBJECT_LIST_COUNT = 1024;
		
		
		private void DefineRagdollElement(int i) {
			DefineField($"m_ragdoll.list[{i}].originParentSpace", VECTOR);
			DefinePhysPtr($"m_ragdoll.list[{i}].pObject");
			DefinePhysPtr($"m_ragdoll.list[{i}].pConstraint");
			DefineField($"m_ragdoll.list[{i}].parentIndex", INTEGER);
		}
		

		protected override void CreateDataMaps() {
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
			
			BeginDataMap("CSlideshowDisplay", "CBaseEntity");
			LinkNamesToMap("vgui_slideshow_display");
			DefineField("m_bEnabled", BOOLEAN);
			DefineKeyField("m_szDisplayText", "displaytext", CHARACTER, 128);
			DefineField("m_szSlideshowDirectory", CHARACTER, 128);
			DefineKeyField("m_String_tSlideshowDirectory", "directory", STRING);
			DefineField("m_chCurrentSlideLists", CHARACTER, 16);
			DefineKeyField("m_fMinSlideTime", "minslidetime", FLOAT);
			DefineKeyField("m_fMaxSlideTime", "maxslidetime", FLOAT);
			DefineKeyField("m_iCycleType", "cycletype", INTEGER);
			DefineKeyField("m_bNoListRepeats", "nolistrepeats", BOOLEAN);
			DefineKeyField("m_iScreenWidth", "width", INTEGER);
			DefineKeyField("m_iScreenHeight", "height", INTEGER);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("SetDisplayText", "InputSetDisplayText", STRING);
			DefineInputFunc("RemoveAllSlides", "InputRemoveAllSlides", VOID);
			DefineInputFunc("AddSlides", "InputAddSlides", STRING);
			DefineInputFunc("SetMinSlideTime", "InputSetMinSlideTime", FLOAT);
			DefineInputFunc("SetMaxSlideTime", "InputSetMaxSlideTime", FLOAT);
			DefineInputFunc("SetCycleType", "InputSetCycleType", INTEGER);
			DefineInputFunc("SetNoListRepeats", "InputSetNoListRepeats", BOOLEAN);
			
			BeginDataMap("CNeurotoxinCountdown", "CBaseEntity");
			LinkNamesToMap("vgui_neurotoxin_countdown");
			DefineField("m_bEnabled", BOOLEAN);
			DefineKeyField("m_iScreenWidth", "width", INTEGER);
			DefineKeyField("m_iScreenHeight", "height", INTEGER);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			
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
		}
	}
}