// ReSharper disable All

using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public class EntMaps3 : DataMapInfoGenerator {
		
		public const int kMAXCONTROLPOINTS = 63;
		public const int kSERVERCONTROLLEDPOINTS = 4;
		public const int MAX_SCENE_FILENAME = 128;
		public const int MAX_PATH = 260;
		
		
		protected override void GenerateDataMaps() {
			if (Game == Game.PORTAL1_3420) {
				BeginDataMap("CProp_Portal", "CBaseAnimating");
				LinkNamesToMap("prop_portal");
				DefineField("m_hLinkedPortal", EHANDLE);
				DefineKeyField("m_iLinkageGroupID", "LinkageGroupID", CHARACTER);
				DefineField("m_matrixThisToLinked", VMATRIX);
				DefineKeyField("m_bActivated", "Activated", BOOLEAN);
				DefineKeyField("m_bIsPortal2", "PortalTwo", BOOLEAN);
				DefineField("m_vPrevForward", VECTOR);
				DefineField("m_hMicrophone", EHANDLE);
				DefineField("m_hSpeaker", EHANDLE);
				DefineSoundPatch("m_pAmbientSound");
				DefineField("m_vAudioOrigin", VECTOR);
				DefineField("m_vDelayedPosition", VECTOR);
				DefineField("m_qDelayedAngles", VECTOR);
				DefineField("m_iDelayedFailure", INTEGER);
				DefineField("m_hPlacedBy", EHANDLE);
				DefineField("m_bSharedEnvironmentConfiguration", BOOLEAN);
				DefineField("m_vPortalCorners", POSITION_VECTOR, 4);
				DefineThinkFunc("DelayedPlacementThink");
				DefineThinkFunc("TestRestingSurfaceThink");
				DefineThinkFunc("FizzleThink");
				DefineInputFunc("SetActivatedState", "InputSetActivatedState", BOOLEAN);
				DefineInputFunc("Fizzle", "InputFizzle", VOID);
				DefineInputFunc("NewLocation", "InputNewLocation", STRING);
				DefineOutput("m_OnPlacedSuccessfully", "OnPlacedSuccessfully");
			}

			BeginDataMap("CFuncPortalBumper", "CBaseEntity");
			LinkNamesToMap("func_portal_bumper");
			DefineField("m_bActive", BOOLEAN);
			DefineInputFunc("Deactivate", "InputDeactivate", VOID);
			DefineInputFunc("Activate", "InputActivate", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineFunction("IsActive");
			
			BeginDataMap("CFuncNoPortalVolume", "CBaseEntity");
			LinkNamesToMap("func_noportal_volume");
			DefineField("m_bActive", BOOLEAN);
			DefineField("m_iListIndex", INTEGER);
			DefineInputFunc("Deactivate", "InputDeactivate", VOID);
			DefineInputFunc("Activate", "InputActivate", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineFunction("GetIndex");
			DefineFunction("IsActive");
			
			BeginDataMap("CSceneEntity", "CPointEntity");
			LinkNamesToMap("scripted_scene", "logic_choreographed_scene");
			DefineKeyField("m_iszSceneFile", "SceneFile", STRING);
			DefineKeyField("m_iszResumeSceneFile", "ResumeSceneFile", STRING);
			DefineField("m_hWaitingForThisResumeScene", EHANDLE);
			DefineField("m_bWaitingForResumeScene", BOOLEAN);
			DefineKeyField("m_iszTarget1", "target1", STRING);
			DefineKeyField("m_iszTarget2", "target2", STRING);
			DefineKeyField("m_iszTarget3", "target3", STRING);
			DefineKeyField("m_iszTarget4", "target4", STRING);
			DefineKeyField("m_iszTarget5", "target5", STRING);
			DefineKeyField("m_iszTarget6", "target6", STRING);
			DefineKeyField("m_iszTarget7", "target7", STRING);
			DefineKeyField("m_iszTarget8", "target8", STRING);
			DefineKeyField("m_BusyActor", "busyactor", INTEGER);
			DefineField("m_hTarget1", EHANDLE);
			DefineField("m_hTarget2", EHANDLE);
			DefineField("m_hTarget3", EHANDLE);
			DefineField("m_hTarget4", EHANDLE);
			DefineField("m_hTarget5", EHANDLE);
			DefineField("m_hTarget6", EHANDLE);
			DefineField("m_hTarget7", EHANDLE);
			DefineField("m_hTarget8", EHANDLE);
			DefineField("m_bIsPlayingBack", BOOLEAN);
			DefineField("m_bPaused", BOOLEAN);
			DefineField("m_flCurrentTime", FLOAT);
			DefineField("m_flForceClientTime", FLOAT);
			DefineField("m_flFrameTime", FLOAT);
			DefineField("m_bCancelAtNextInterrupt", BOOLEAN);
			DefineField("m_fPitch", FLOAT);
			DefineField("m_bAutomated", BOOLEAN);
			DefineField("m_nAutomatedAction", INTEGER);
			DefineField("m_flAutomationDelay", FLOAT);
			DefineField("m_flAutomationTime", FLOAT);
			DefineField("m_bPausedViaInput", BOOLEAN);
			DefineField("m_bWaitingForActor", BOOLEAN);
			DefineField("m_bWaitingForInterrupt", BOOLEAN);
			DefineField("m_bInterruptedActorsScenes", BOOLEAN);
			DefineField("m_bBreakOnNonIdle", BOOLEAN);
			DefineVector("m_hActorList", EHANDLE);
			DefineVector("m_hRemoveActorList", EHANDLE);
			DefineField("m_nInterruptCount", INTEGER);
			DefineField("m_bInterrupted", BOOLEAN);
			DefineField("m_hInterruptScene", EHANDLE);
			DefineField("m_bCompletedEarly", BOOLEAN);
			DefineField("m_bInterruptSceneFinished", BOOLEAN);
			DefineField("m_bGenerated", BOOLEAN);
			DefineField("m_iszSoundName", STRING);
			DefineField("m_hActor", EHANDLE);
			DefineField("m_hActivator", EHANDLE);
			DefineVector("m_hNotifySceneCompletion", EHANDLE);
			DefineVector("m_hListManagers", EHANDLE);
			DefineField("m_bMultiplayer", BOOLEAN);
			DefineInputFunc("Start", "InputStartPlayback", VOID);
			DefineInputFunc("Pause", "InputPausePlayback", VOID);
			DefineInputFunc("Resume", "InputResumePlayback", VOID);
			DefineInputFunc("Cancel", "InputCancelPlayback", VOID);
			DefineInputFunc("CancelAtNextInterrupt", "InputCancelAtNextInterrupt", VOID);
			DefineInputFunc("PitchShift", "InputPitchShiftPlayback", FLOAT);
			DefineInputFunc("InterjectResponse", "InputInterjectResponse", STRING);
			DefineInputFunc("StopWaitingForActor", "InputStopWaitingForActor", VOID);
			DefineInputFunc("Trigger", "InputTriggerEvent", INTEGER);
			DefineKeyField("m_iPlayerDeathBehavior", "onplayerdeath", INTEGER);
			DefineInputFunc("ScriptPlayerDeath", "InputScriptPlayerDeath", VOID);
			DefineOutput("m_OnStart", "OnStart");
			DefineOutput("m_OnCompletion", "OnCompletion");
			DefineOutput("m_OnCanceled", "OnCanceled");
			DefineOutput("m_OnTrigger1", "OnTrigger1");
			DefineOutput("m_OnTrigger2", "OnTrigger2");
			DefineOutput("m_OnTrigger3", "OnTrigger3");
			DefineOutput("m_OnTrigger4", "OnTrigger4");
			DefineOutput("m_OnTrigger5", "OnTrigger5");
			DefineOutput("m_OnTrigger6", "OnTrigger6");
			DefineOutput("m_OnTrigger7", "OnTrigger7");
			DefineOutput("m_OnTrigger8", "OnTrigger8");
			DefineOutput("m_OnTrigger9", "OnTrigger9");
			DefineOutput("m_OnTrigger10", "OnTrigger10");
			DefineOutput("m_OnTrigger11", "OnTrigger11");
			DefineOutput("m_OnTrigger12", "OnTrigger12");
			DefineOutput("m_OnTrigger13", "OnTrigger13");
			DefineOutput("m_OnTrigger14", "OnTrigger14");
			DefineOutput("m_OnTrigger15", "OnTrigger15");
			DefineOutput("m_OnTrigger16", "OnTrigger16");
			
			BeginDataMap("CInstancedSceneEntity", "CSceneEntity");
			LinkNamesToMap("instanced_scripted_scene");
			DefineField("m_hOwner", EHANDLE);
			DefineField("m_bHadOwner", BOOLEAN);
			DefineField("m_flPostSpeakDelay", FLOAT);
			DefineField("m_flPreDelay", FLOAT);
			DefineField("m_szInstanceFilename", CHARACTER, MAX_SCENE_FILENAME);
			DefineField("m_bIsBackground", BOOLEAN);
			
			BeginDataMap("CBaseCombatWeapon", "CBaseAnimating");
			DefineField("m_flNextPrimaryAttack", TIME);
			DefineField("m_flNextSecondaryAttack", TIME);
			DefineField("m_flTimeWeaponIdle", TIME);
			DefineField("m_bInReload", BOOLEAN);
			DefineField("m_bFireOnEmpty", BOOLEAN);
			DefineField("m_hOwner", EHANDLE);
			DefineField("m_iState", INTEGER);
			DefineField("m_iszName", STRING);
			DefineField("m_iPrimaryAmmoType", INTEGER);
			DefineField("m_iSecondaryAmmoType", INTEGER);
			DefineField("m_iClip1", INTEGER);
			DefineField("m_iClip2", INTEGER);
			DefineField("m_bFiresUnderwater", BOOLEAN);
			DefineField("m_bAltFiresUnderwater", BOOLEAN);
			DefineField("m_fMinRange1", FLOAT);
			DefineField("m_fMinRange2", FLOAT);
			DefineField("m_fMaxRange1", FLOAT);
			DefineField("m_fMaxRange2", FLOAT);
			DefineField("m_iPrimaryAmmoCount", INTEGER);
			DefineField("m_iSecondaryAmmoCount", INTEGER);
			DefineField("m_nViewModelIndex", INTEGER);
			DefineField("m_nIdealSequence", INTEGER);
			DefineField("m_IdealActivity", INTEGER);
			DefineField("m_fFireDuration", FLOAT);
			DefineField("m_bReloadsSingly", BOOLEAN);
			DefineField("m_iSubType", INTEGER);
			DefineField("m_bRemoveable", BOOLEAN);
			DefineField("m_flUnlockTime", TIME);
			DefineField("m_hLocker", EHANDLE);
			DefinePhysPtr("m_pConstraint");
			DefineField("m_iReloadHudHintCount", INTEGER);
			DefineField("m_iAltFireHudHintCount", INTEGER);
			DefineField("m_bReloadHudHintDisplayed", BOOLEAN);
			DefineField("m_bAltFireHudHintDisplayed", BOOLEAN);
			DefineField("m_flHudHintPollTime", TIME);
			DefineField("m_flHudHintMinDisplayTime", TIME);
			DefineFunction("DefaultTouch");
			DefineFunction("FallThink");
			DefineFunction("Materialize");
			DefineFunction("AttemptToMaterialize");
			DefineFunction("DestroyItem");
			DefineFunction("SetPickupTouch");
			DefineFunction("HideThink");
			DefineInputFunc("HideWeapon", "InputHideWeapon", VOID);
			DefineOutput("m_OnPlayerUse", "OnPlayerUse");
			DefineOutput("m_OnPlayerPickup", "OnPlayerPickup");
			DefineOutput("m_OnNPCPickup", "OnNPCPickup");
			DefineOutput("m_OnCacheInteraction", "OnCacheInteraction");
			
			BeginDataMap("CWeaponPortalBase", "CBaseCombatWeapon");
			
			BeginDataMap("CBasePortalCombatWeapon", "CWeaponPortalBase");
			DefineField("m_bLowered", BOOLEAN);
			DefineField("m_flRaiseTime", TIME);
			DefineField("m_flHolsterTime", TIME);
			if (Game == Game.PORTAL2) {
				DefineField("m_flNextRepeatPrimaryAttack", FLOAT);
				DefineField("m_flNextRepeatSecondaryAttack", FLOAT);
			}
			
			BeginDataMap("CWeaponPortalgun", "CBasePortalCombatWeapon");
			LinkNamesToMap("weapon_portalgun");
			DefineKeyField("m_bCanFirePortal1", "CanFirePortal1", BOOLEAN);
			DefineKeyField("m_bCanFirePortal2", "CanFirePortal2", BOOLEAN);
			DefineField("m_iLastFiredPortal", INTEGER);
			DefineField("m_bOpenProngs", BOOLEAN);
			DefineField("m_fCanPlacePortal1OnThisSurface", FLOAT);
			DefineField("m_fCanPlacePortal2OnThisSurface", FLOAT);
			DefineField("m_fEffectsMaxSize1", FLOAT);
			DefineField("m_fEffectsMaxSize2", FLOAT);
			DefineField("m_EffectState", INTEGER);
			DefineField("m_iPortalLinkageGroupID", CHARACTER);
			DefineInputFunc("ChargePortal1", "InputChargePortal1", VOID);
			DefineInputFunc("ChargePortal2", "InputChargePortal2", VOID);
			DefineInputFunc("FirePortal1", "FirePortal1", VOID);
			DefineInputFunc("FirePortal2", "FirePortal2", VOID);
			DefineInputFunc("FirePortalDirection1", "FirePortalDirection1", VECTOR);
			DefineInputFunc("FirePortalDirection2", "FirePortalDirection2", VECTOR);
			DefineSoundPatch("m_pMiniGravHoldSound");
			DefineOutput("m_OnFiredPortal1", "OnFiredPortal1");
			DefineOutput("m_OnFiredPortal2", "OnFiredPortal2");
			DefineFunction("Think");
			if (Game == Game.PORTAL2) {
				DefineField("m_vecBluePortalPos", VECTOR);
				DefineField("m_vecOrangePortalPos", VECTOR);
				DefineField("m_bShowingPotatos", BOOLEAN);
			}
			
			BeginDataMap("CBoneFollower", "CBaseEntity");
			LinkNamesToMap("phys_bone_follower");
			DefineField("m_modelIndex", MODELINDEX);
			DefineField("m_solidIndex", INTEGER);
			DefineField("m_physicsBone", INTEGER);
			DefineField("m_hitGroup", INTEGER);
			
			BeginDataMap("CBaseViewModel", "CBaseAnimating");
			LinkNamesToMap("viewmodel");
			DefineField("m_hOwner", EHANDLE);
			DefineField("m_nViewModelIndex", INTEGER);
			DefineField("m_flTimeWeaponIdle", FLOAT);
			DefineField("m_nAnimationParity", INTEGER);
			DefineField("m_vecLastFacing", VECTOR);
			DefineField("m_hWeapon", EHANDLE);
			DefineVector("m_hScreens", EHANDLE);
			
			BeginDataMap("CColorCorrection", "CBaseEntity"); // shadow control entity
			LinkNamesToMap("color_correction");
			DefineThinkFunc("FadeInThink");
			DefineThinkFunc("FadeOutThink");
			DefineField("m_flCurWeight", FLOAT);
			DefineField("m_flTimeStartFadeIn", FLOAT);
			DefineField("m_flTimeStartFadeOut", FLOAT);
			DefineField("m_flStartFadeInWeight", FLOAT);
			DefineField("m_flStartFadeOutWeight", FLOAT);
			DefineKeyField("m_MinFalloff", "minfalloff", FLOAT);
			DefineKeyField("m_MaxFalloff", "maxfalloff", FLOAT);
			DefineKeyField("m_flMaxWeight", "maxweight", FLOAT);
			DefineKeyField("m_flFadeInDuration", "fadeInDuration", FLOAT);
			DefineKeyField("m_flFadeOutDuration", "fadeOutDuration", FLOAT);
			DefineKeyField("m_lookupFilename", "filename", STRING);
			DefineKeyField("m_bEnabled", "enabled", BOOLEAN);
			DefineKeyField("m_bStartDisabled", "StartDisabled", BOOLEAN);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("SetFadeInDuration", "InputSetFadeInDuration", FLOAT);
			DefineInputFunc("SetFadeOutDuration", "InputSetFadeOutDuration", FLOAT);
			
			BeginDataMap("CParticleSystem", "CBaseEntity");
			LinkNamesToMap("info_particle_system");
			DefineKeyField("m_bStartActive", "start_active", BOOLEAN);
			DefineField("m_bActive", BOOLEAN);
			DefineField("m_flStartTime", TIME);
			if (Game == Game.PORTAL2) {
				DefineField("m_vServerControlPoints", VECTOR, kSERVERCONTROLLEDPOINTS);
				DefineField("m_iServerControlPointAssignments", BYTE, kSERVERCONTROLLEDPOINTS);
				DefineKeyField("m_szSnapshotFileName", "snapshot_file", CHARACTER, MAX_PATH);
			}
			DefineKeyField("m_iszEffectName", "effect_name", STRING);
			DefineKeyField("m_iszControlPointNames[0]", "cpoint1", STRING);
			DefineKeyField("m_iszControlPointNames[1]", "cpoint2", STRING);
			DefineKeyField("m_iszControlPointNames[2]", "cpoint3", STRING);
			DefineKeyField("m_iszControlPointNames[3]", "cpoint4", STRING);
			DefineKeyField("m_iszControlPointNames[4]", "cpoint5", STRING);
			DefineKeyField("m_iszControlPointNames[5]", "cpoint6", STRING);
			DefineKeyField("m_iszControlPointNames[6]", "cpoint7", STRING);
			DefineKeyField("m_iszControlPointNames[7]", "cpoint8", STRING);
			DefineKeyField("m_iszControlPointNames[8]", "cpoint9", STRING);
			DefineKeyField("m_iszControlPointNames[9]", "cpoint10", STRING);
			DefineKeyField("m_iszControlPointNames[10]", "cpoint11", STRING);
			DefineKeyField("m_iszControlPointNames[11]", "cpoint12", STRING);
			DefineKeyField("m_iszControlPointNames[12]", "cpoint13", STRING);
			DefineKeyField("m_iszControlPointNames[13]", "cpoint14", STRING);
			DefineKeyField("m_iszControlPointNames[14]", "cpoint15", STRING);
			DefineKeyField("m_iszControlPointNames[15]", "cpoint16", STRING);
			DefineKeyField("m_iszControlPointNames[16]", "cpoint17", STRING);
			DefineKeyField("m_iszControlPointNames[17]", "cpoint18", STRING);
			DefineKeyField("m_iszControlPointNames[18]", "cpoint19", STRING);
			DefineKeyField("m_iszControlPointNames[19]", "cpoint20", STRING);
			DefineKeyField("m_iszControlPointNames[20]", "cpoint21", STRING);
			DefineKeyField("m_iszControlPointNames[21]", "cpoint22", STRING);
			DefineKeyField("m_iszControlPointNames[22]", "cpoint23", STRING);
			DefineKeyField("m_iszControlPointNames[23]", "cpoint24", STRING);
			DefineKeyField("m_iszControlPointNames[24]", "cpoint25", STRING);
			DefineKeyField("m_iszControlPointNames[25]", "cpoint26", STRING);
			DefineKeyField("m_iszControlPointNames[26]", "cpoint27", STRING);
			DefineKeyField("m_iszControlPointNames[27]", "cpoint28", STRING);
			DefineKeyField("m_iszControlPointNames[28]", "cpoint29", STRING);
			DefineKeyField("m_iszControlPointNames[29]", "cpoint30", STRING);
			DefineKeyField("m_iszControlPointNames[30]", "cpoint31", STRING);
			DefineKeyField("m_iszControlPointNames[31]", "cpoint32", STRING);
			DefineKeyField("m_iszControlPointNames[32]", "cpoint33", STRING);
			DefineKeyField("m_iszControlPointNames[33]", "cpoint34", STRING);
			DefineKeyField("m_iszControlPointNames[34]", "cpoint35", STRING);
			DefineKeyField("m_iszControlPointNames[35]", "cpoint36", STRING);
			DefineKeyField("m_iszControlPointNames[36]", "cpoint37", STRING);
			DefineKeyField("m_iszControlPointNames[37]", "cpoint38", STRING);
			DefineKeyField("m_iszControlPointNames[38]", "cpoint39", STRING);
			DefineKeyField("m_iszControlPointNames[39]", "cpoint40", STRING);
			DefineKeyField("m_iszControlPointNames[40]", "cpoint41", STRING);
			DefineKeyField("m_iszControlPointNames[41]", "cpoint42", STRING);
			DefineKeyField("m_iszControlPointNames[42]", "cpoint43", STRING);
			DefineKeyField("m_iszControlPointNames[43]", "cpoint44", STRING);
			DefineKeyField("m_iszControlPointNames[44]", "cpoint45", STRING);
			DefineKeyField("m_iszControlPointNames[45]", "cpoint46", STRING);
			DefineKeyField("m_iszControlPointNames[46]", "cpoint47", STRING);
			DefineKeyField("m_iszControlPointNames[47]", "cpoint48", STRING);
			DefineKeyField("m_iszControlPointNames[48]", "cpoint49", STRING);
			DefineKeyField("m_iszControlPointNames[49]", "cpoint50", STRING);
			DefineKeyField("m_iszControlPointNames[50]", "cpoint51", STRING);
			DefineKeyField("m_iszControlPointNames[51]", "cpoint52", STRING);
			DefineKeyField("m_iszControlPointNames[52]", "cpoint53", STRING);
			DefineKeyField("m_iszControlPointNames[53]", "cpoint54", STRING);
			DefineKeyField("m_iszControlPointNames[54]", "cpoint55", STRING);
			DefineKeyField("m_iszControlPointNames[55]", "cpoint56", STRING);
			DefineKeyField("m_iszControlPointNames[56]", "cpoint57", STRING);
			DefineKeyField("m_iszControlPointNames[57]", "cpoint58", STRING);
			DefineKeyField("m_iszControlPointNames[58]", "cpoint59", STRING);
			DefineKeyField("m_iszControlPointNames[59]", "cpoint60", STRING);
			DefineKeyField("m_iszControlPointNames[60]", "cpoint61", STRING);
			DefineKeyField("m_iszControlPointNames[61]", "cpoint62", STRING);
			DefineKeyField("m_iszControlPointNames[62]", "cpoint63", STRING);
			DefineKeyField("m_iControlPointParents[0]", "cpoint1_parent", CHARACTER);
			DefineKeyField("m_iControlPointParents[1]", "cpoint2_parent", CHARACTER);
			DefineKeyField("m_iControlPointParents[2]", "cpoint3_parent", CHARACTER);
			DefineKeyField("m_iControlPointParents[3]", "cpoint4_parent", CHARACTER);
			DefineKeyField("m_iControlPointParents[4]", "cpoint5_parent", CHARACTER);
			DefineKeyField("m_iControlPointParents[5]", "cpoint6_parent", CHARACTER);
			DefineKeyField("m_iControlPointParents[6]", "cpoint7_parent", CHARACTER);
			DefineField("m_hControlPointEnts", EHANDLE, kMAXCONTROLPOINTS);
			DefineInputFunc("Start", "InputStart", VOID);
			DefineInputFunc("Stop", "InputStop", VOID);
			if (Game == Game.PORTAL2) {
				DefineInputFunc("StopPlayEndCap", "InputStopEndCap", VOID);
				DefineInputFunc("DestroyImmediately", "InputDestroy", VOID);
			}
			DefineThinkFunc("StartParticleSystemThink");
			
			BeginDataMap("CPathTrack", "CPointEntity");
			LinkNamesToMap("path_track");
			DefineField("m_pnext", CLASSPTR);
			DefineField("m_pprevious", CLASSPTR);
			DefineField("m_paltpath", CLASSPTR);
			DefineKeyField("m_flRadius", "radius", FLOAT);
			DefineField("m_length", FLOAT);
			DefineKeyField("m_altName", "altpath", STRING);
			DefineKeyField("m_eOrientationType", "orientationtype", INTEGER);
			DefineInputFunc("InPass", "InputPass", VOID);
			DefineInputFunc("EnableAlternatePath", "InputEnableAlternatePath", VOID);
			DefineInputFunc("DisableAlternatePath", "InputDisableAlternatePath", VOID);
			DefineInputFunc("ToggleAlternatePath", "InputToggleAlternatePath", VOID);
			DefineInputFunc("EnablePath", "InputEnablePath", VOID);
			DefineInputFunc("DisablePath", "InputDisablePath", VOID);
			DefineInputFunc("TogglePath", "InputTogglePath", VOID);
			DefineOutput("m_OnPass", "OnPass");
			
			BeginDataMap("CShadowControl", "CBaseEntity");
			LinkNamesToMap("shadow_control");
			DefineInputAndKeyField("m_flShadowMaxDist", "distance", "SetDistance", FLOAT);
			DefineInputAndKeyField("m_bDisableShadows", "disableallshadows", "SetShadowsDisabled", BOOLEAN);
			DefineInput("m_shadowColor", "color", COLOR32);
			DefineInput("m_shadowDirection", "direction", VECTOR);
			if (Game == Game.PORTAL2)
				DefineInput("m_bEnableLocalLightShadows", "SetShadowsFromLocalLightsEnabled", BOOLEAN);
			DefineInputFunc("SetAngles", "InputSetAngles", STRING);
			
			BeginDataMap("CInfoLightingRelative", "CBaseEntity");
			LinkNamesToMap("info_lighting_relative");
			DefineKeyField("m_strLightingLandmark", "LightingLandmark", STRING);
			DefineField("m_hLightingLandmark", EHANDLE);
			
			BeginDataMap("CFuncTrackTrain", "CBaseEntity");
			LinkNamesToMap("func_tracktrain");
			DefineKeyField("m_length", "wheels", FLOAT);
			DefineKeyField("m_height", "height", FLOAT);
			DefineKeyField("m_maxSpeed", "startspeed", FLOAT);
			DefineKeyField("m_flBank", "bank", FLOAT);
			DefineKeyField("m_flBlockDamage", "dmg", FLOAT);
			DefineKeyField("m_iszSoundMove", "MoveSound", SOUNDNAME);
			DefineKeyField("m_iszSoundMovePing", "MovePingSound", SOUNDNAME);
			DefineKeyField("m_iszSoundStart", "StartSound", SOUNDNAME);
			DefineKeyField("m_iszSoundStop", "StopSound", SOUNDNAME);
			DefineKeyField("m_nMoveSoundMinPitch", "MoveSoundMinPitch", INTEGER);
			DefineKeyField("m_nMoveSoundMaxPitch", "MoveSoundMaxPitch", INTEGER);
			DefineKeyField("m_flMoveSoundMinTime", "MoveSoundMinTime", FLOAT);
			DefineKeyField("m_flMoveSoundMaxTime", "MoveSoundMaxTime", FLOAT);
			DefineField("m_flNextMoveSoundTime", TIME);
			DefineKeyField("m_eVelocityType", "velocitytype", INTEGER);
			DefineKeyField("m_eOrientationType", "orientationtype", INTEGER);
			DefineField("m_ppath", CLASSPTR);
			DefineField("m_dir", FLOAT);
			DefineField("m_controlMins", VECTOR);
			DefineField("m_controlMaxs", VECTOR);
			DefineField("m_flVolume", FLOAT);
			DefineField("m_oldSpeed", FLOAT);
			DefineField("m_bSoundPlaying", BOOLEAN);
			if (GenInfo.IsDefHl1Dll)
				DefineField("m_bOnTrackChange", BOOLEAN);
			if (Game == Game.PORTAL2) {
				DefineKeyField("m_bManualSpeedChanges", "ManualSpeedChanges", BOOLEAN);
				DefineKeyField("m_flAccelSpeed", "ManualAccelSpeed", FLOAT);
				DefineKeyField("m_flDecelSpeed", "ManualDecelSpeed", FLOAT);
			}
			DefineInputFunc("Stop", "InputStop", VOID);
			DefineInputFunc("StartForward", "InputStartForward", VOID);
			DefineInputFunc("StartBackward", "InputStartBackward", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("Resume", "InputResume", VOID);
			DefineInputFunc("Reverse", "InputReverse", VOID);
			DefineInputFunc("SetSpeed", "InputSetSpeed", FLOAT);
			DefineInputFunc("SetSpeedDir", "InputSetSpeedDir", FLOAT);
			DefineInputFunc("SetSpeedReal", "InputSetSpeedReal", FLOAT);
			if (Game == Game.PORTAL2) {
				DefineInputFunc("SetSpeedDirAccel", "InputSetSpeedDirAccel", FLOAT);
				DefineOutput("m_OnArrivedAtDestinationNode", "OnArrivedAtDestinationNode");
				DefineField("m_strPathTarget", STRING);
			}
			DefineOutput("m_OnStart", "OnStart");
			DefineOutput("m_OnNext", "OnNextPoint");
			DefineFunction("Next");
			DefineFunction("Find");
			DefineFunction("NearestPath");
			DefineFunction("DeadEnd");
			
			BeginDataMap("CInfoOverlayAccessor", "CPointEntity");
			LinkNamesToMap("info_overlay_accessor");
			DefineField("m_iOverlayID", INTEGER);
			
			BeginDataMap("CBaseFilter", "CLogicalEntity");
			LinkNamesToMap("filter_base");
			DefineKeyField("m_bNegated", "Negated", BOOLEAN);
			DefineInputFunc("TestActivator", "InputTestActivator", INPUT);
			DefineOutput("m_OnPass", "OnPass");
			DefineOutput("m_OnFail", "OnFail");
			
			BeginDataMap("CFilterName", "CBaseFilter");
			LinkNamesToMap("filter_activator_name");
			DefineKeyField("m_iFilterName", "filtername", STRING);
		}
	}
}