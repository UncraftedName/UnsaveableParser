using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public class P2SpecificMaps : DataMapInfoGenerator {
		
		// lots of these have no code
		protected override void GenerateDataMaps() {
			if (Game != Game.PORTAL2)
				return;
			
			LinkedNamesToOtherMap("CPointEntity", "info_landmark_entry", "info_landmark_exit", "projected_entity_ambient_sound_proxy"); // the last one is just a guess
			
			#region ai/npc stuff
			
			// this is needed for the player class hierarchy, it's some confusing template spaghetti, thank you valve
			
			// there exists at least one class "IPaintableEntity<T>" with a datamap called "PaintableEntity"
			DeclareTemplatedClass("IPaintableEntity", "PaintableEntity");
			DefineField("m_iPaintPower", INTEGER);
			
			// create the datamap for the class "IPaintableEntity<CBasePlayer> : CBasePlayer" (which has the name "PaintableEntity")
			BeginTemplatedMap("IPaintableEntity", "CBasePlayer", "CBasePlayer", null);
			
			// CBaseMultiplayer : IPaintableEntity<CBasePlayer> (no datamap)
			DataMapProxyToTemplated("CBaseMultiplayerPlayer", null, "IPaintableEntity", "CBasePlayer");
			
			// CPaintableEntity<CBaseMultiplayerPlayer> : CBaseMultiplayerPlayer (no datamap)
			DataMapProxyToTemplated("CPaintableEntity", "CBaseMultiplayerPlayer", "CBaseMultiplayerPlayer", null);
			
			// PaintPowerUser<CPaintableEntity<CBaseMultiplayerPlayer>> : CPaintableEntity<CBaseMultiplayerPlayer> (no datamap), the player inherits from this
			DataMapProxyToTemplated("PaintPowerUser", "CPaintableEntity<CBaseMultiplayerPlayer>", "CPaintableEntity", "CBaseMultiplayerPlayer");

			BeginDataMap("CNPC_PersonalityCore", "CAI_PlayerAlly");
			LinkNamesToMap("npc_personality_core");
			DefineField("m_flNextIdleSoundTime", FLOAT);
			DefineField("m_iIdleOverrideSequence", INTEGER);
			DefineField("m_hProjectedTexture", EHANDLE);
			DefineField("m_bPickupEnabled", BOOLEAN);
			
			BeginDataMap("CPortalPlayerLocalData");
			DefineField("m_nLocatorEntityIndices", EHANDLE, 16);
			DefineField("m_StickNormal", VECTOR);
			DefineField("m_OldStickNormal", VECTOR);
			DefineField("m_Up", VECTOR);
			DefineField("m_StandHullMin", VECTOR);
			DefineField("m_StandHullMax", VECTOR);
			DefineField("m_DuckHullMin", VECTOR);
			DefineField("m_DuckHullMax", VECTOR);
			DefineField("m_CachedStandHullMinAttempt", VECTOR);
			DefineField("m_CachedStandHullMaxAttempt", VECTOR);
			DefineField("m_CachedDuckHullMinAttempt", VECTOR);
			DefineField("m_CachedDuckHullMaxAttempt", VECTOR);
			DefineField("m_vLocalUp", VECTOR);
			DefineField("m_PaintedPowerType", INTEGER); // todo paint type?
			DefineField("m_flAirInputScale", FLOAT);
			DefineField("m_flCurrentStickTime", FLOAT);
			DefineField("m_nStickCameraState", INTEGER);
			DefineField("m_bDoneStickInterp", BOOLEAN);
			DefineField("m_bDoneCorrectPitch", BOOLEAN);
			DefineField("m_InAirState", INTEGER);
			DefineField("m_vStickRotationAxis", VECTOR);
			DefineField("m_vEyeOffset", VECTOR);
			DefineField("m_fBouncedTime", FLOAT);
			
			BeginDataMap("PortalPlayerStatistics_t");
			DefineField("iNumPortalsPlaced", INTEGER);
			DefineField("iNumStepsTaken", INTEGER);
			DefineField("fNumSecondsTaken", FLOAT);
			DefineField("fDistanceTaken", FLOAT);
			
			DeclareTemplatedClass("CAI_Behavior", "AI_Behaviors");
			DefineField("CBaseEntity", EHANDLE);
			DefineKeyField("m_iClassname", "classname", STRING);
			
			DataMapProxyToTemplated("CAI_ComponentWithOuter", "CNPC_Wheatley_Boss", "CNPC_Wheatley_Boss", null);
			
			BeginTemplatedMap("CAI_Behavior", "CNPC_Wheatley_Boss", "CAI_ComponentWithOuter", "CNPC_Wheatley_Boss");
			
			DataMapProxyToTemplated("UNKNOWN_CLASS_NAME_FOR_WHEATLEY", null, "CAI_Behavior", "CNPC_Wheatley_Boss");
			LinkNamesToMap("npc_wheatley_boss");
			
			BeginDataMap("CNPC_Wheatley_Boss", "CAI_BaseActor");
			
			// IPaintableEntity<CNPC_FloorTurret> : CNPC_FloorTurret
			BeginTemplatedMap("IPaintableEntity", "CNPC_FloorTurret", "CNPC_FloorTurret", null);
				
			// PropPaintPowerUser<IPaintableEntity<CNPC_FloorTurret>> : IPaintableEntity<CNPC_FloorTurret>
			BeginTemplatedMap("PropPaintPowerUser", "IPaintableEntity<CNPC_FloorTurret>", "IPaintableEntity", "CNPC_FloorTurret");
				
			// CNPC_Portal_FloorTurret : PropPaintPowerUser<IPaintableEntity<CNPC_FloorTurret>>
			DataMapProxyToTemplated("CNPC_Portal_FloorTurret", null, "PropPaintPowerUser", "IPaintableEntity<CNPC_FloorTurret>");
			LinkNamesToMap("npc_portal_turret_floor");
			
			#endregion
			
			BeginDataMap("CBeamSpotlight", "CBaseEntity");
			LinkNamesToMap("beam_spotlight");
			DefineField("m_nHaloIndex", MODELINDEX);
			DefineField("m_bSpotlightOn", BOOLEAN);
			DefineField("m_bHasDynamicLight", BOOLEAN);
			DefineField("m_flRotationSpeed", FLOAT);
			DefineField("m_isRotating", BOOLEAN);
			DefineField("m_isReversed", BOOLEAN);
			DefineField("m_nRotationAxis", INTEGER);
			DefineKeyField("m_flmaxSpeed", "maxspeed", FLOAT);
			DefineKeyField("m_flSpotlightMaxLength", "SpotlightLength", FLOAT);
			DefineKeyField("m_flSpotlightGoalWidth", "SpotlightWidth", FLOAT);
			DefineKeyField("m_flHDRColorScale", "HDRColorScale", FLOAT);
			DefineInputFunc("LightOn", "InputTurnOn", VOID);
			DefineInputFunc("LightOff", "InputTurnOff", VOID);
			DefineInputFunc("Start", "InputStart", VOID);
			DefineInputFunc("Stop", "InputStop", VOID);
			DefineInputFunc("Reverse", "InputReverse", VOID);
			DefineOutput("m_OnOn", "OnLightOn");
			DefineOutput("m_OnOff", "OnLightOff");
			
			BeginDataMap("CPropUnderButton", "CPropButton");
			LinkNamesToMap("prop_under_button");
			
			BeginDataMap("CInfoPlacementHelper", "CBaseEntity");
			LinkNamesToMap("info_placement_helper");
			DefineField("m_flRadius", FLOAT);
			DefineField("m_bForcePlacement", BOOLEAN);
			DefineField("m_flDisableTime", FLOAT);
			if (Game == Game.PORTAL2)
				DefineField("m_bSnapToHelperAngles", BOOLEAN);
			
			BeginDataMap("CPropButton", "CBaseAnimating");
			LinkNamesToMap("prop_button");
			DefineThinkFunc("AnimateThink");
			DefineThinkFunc("TimerThink");
			DefineKeyField("m_flDelayBeforeReset", "Delay", FLOAT);
			DefineKeyField("m_bIsTimer", "IsTimer", BOOLEAN);
			DefineKeyField("m_bPreventFastReset", "PreventFastReset", BOOLEAN);
			DefineField("m_hActivator", EHANDLE);
			DefineField("m_bLocked", BOOLEAN);
			DefineField("m_bTimerCancelled", BOOLEAN);
			DefineField("m_flGoalTime", TIME);
			DefineField("m_UpSequence", INTEGER);
			DefineField("m_DownSequence", INTEGER);
			DefineField("m_IdleDownSequence", INTEGER);
			DefineField("m_IdleUpSequence", INTEGER);
			DefineInputFunc("Press", "InputPress", VOID);
			DefineInputFunc("Lock", "InputLock", VOID);
			DefineInputFunc("Unlock", "InputUnlock", VOID);
			DefineInputFunc("CancelPress", "InputCancelPress", VOID);
			DefineOutput("m_OnPressed", "OnPressed");
			DefineOutput("m_OnPressedOrange", "OnPressedOrange");
			DefineOutput("m_OnPressedBlue", "OnPressedBlue");
			DefineOutput("m_OnButtonReset", "OnButtonReset");
			
			BeginDataMap("CPropTestChamberDoor", "CBaseAnimating");
			LinkNamesToMap("prop_testchamber_door");
			DefineField("m_nSequenceOpen", INTEGER);
			DefineField("m_nSequenceOpenIdle", INTEGER);
			DefineField("m_nSequenceClose", INTEGER);
			DefineField("m_nSequenceCloseIdle", INTEGER);
			DefineField("m_bIsOpen", BOOLEAN);
			DefineField("m_bIsAnimating", BOOLEAN);
			DefineField("m_bIsLocked", BOOLEAN);
			DefineField("m_hAreaPortalWindow", EHANDLE);
			DefineKeyField("m_strAreaPortalWindowName", "AreaPortalWindow", STRING);
			DefineKeyField("m_bUseAreaPortalFade", "UseAreaPortalFade", BOOLEAN);
			DefineKeyField("m_flAreaPortalFadeStartDistance", "AreaPortalFadeStart", FLOAT);
			DefineKeyField("m_flAreaPortalFadeEndDistance", "AreaPortalFadeEnd", FLOAT);
			DefineThinkFunc("AnimateThink");
			DefineInputFunc("Open", "InputOpen", VOID);
			DefineInputFunc("Close", "InputClose", VOID);
			DefineInputFunc("Lock", "InputLock", VOID);
			DefineInputFunc("LockOpen", "InputLockOpen", VOID);
			DefineInputFunc("Unlock", "InputUnlock", VOID);
			DefineOutput("m_OnOpen", "OnOpen");
			DefineOutput("m_OnClose", "OnClose");
			DefineOutput("m_OnFullyClosed", "OnFullyClosed");
			DefineOutput("m_OnFullyOpen", "OnFullyOpen");
			DefineEmbeddedField("m_BoneFollowerManager", "CBoneFollowerManager");
			
			BeginDataMap("CPropFloorButton", "CDynamicProp");
			LinkNamesToMap("prop_floor_button");
			DefineThinkFunc("AnimateThink");
			DefineThinkFunc("PressingBoxHasSetteledThink");
			DefineField("m_UpSequence", INTEGER);
			DefineField("m_DownSequence", INTEGER);
			DefineField("m_hButtonTrigger", EHANDLE);
			DefineInputFunc("PressIn", "InputPressIn", VOID);
			DefineInputFunc("PressOut", "InputPressOut", VOID);
			DefineOutput("m_OnPressed", "OnPressed");
			DefineOutput("m_OnPressedOrange", "OnPressedOrange");
			DefineOutput("m_OnPressedBlue", "OnPressedBlue");
			DefineOutput("m_OnUnPressed", "OnUnPressed");
			
			BeginDataMap("CPropIndicatorPanel", "CBaseEntity");
			LinkNamesToMap("prop_indicator_panel");
			DefineField("m_strIndicatorLights", STRING);
			DefineField("m_hIndicatorPanel", EHANDLE);
			DefineField("m_bEnabled", BOOLEAN);
			DefineField("m_flTimerDuration", FLOAT);
			
			BeginDataMap("CBaseProjector", "CBaseAnimating");
			DefineField("m_hFirstChild", EHANDLE);
			DefineField("m_bEnabled", BOOLEAN);
			BeginDataMap("CPropWallProjector", "CBaseProjector");
			LinkNamesToMap("prop_wall_projector");
			DefineSoundPatch("m_pAmbientSound");
			DefineSoundPatch("m_pAmbientMusic");
			DefineField("m_hAmbientSoundProxy", EHANDLE);
			BeginDataMap("CLabIndicatorPanel", "CBaseEntity");
			LinkNamesToMap("vgui_indicator_panel");
			DefineField("m_bEnabled", BOOLEAN);
			DefineField("m_iPlayerPinged", INTEGER);
			DefineField("m_hScreen", EHANDLE);
			
			BeginDataMap("FizzlerMultiOriginSoundPlayer", "CBaseEntity");
			LinkNamesToMap("fizzler_multiorigin_sound_player");
			DefineSoundPatch("m_pSound");
			
			BeginDataMap("CBaseProjectedEntity", "CBaseEntity");
			DefineField("m_vecStartPoint", VECTOR);
			DefineField("m_vecEndPoint", VECTOR);
			DefineField("m_iMaxRemainingRecursions", INTEGER);
			DefineField("m_bCreatePlacementHelper", BOOLEAN);
			
			// IPaintableEntity<CBaseProjectedEntity> : CBaseProjectedEntity
			BeginTemplatedMap("IPaintableEntity", "CBaseProjectedEntity", "CBaseProjectedEntity", null);
			
			// CProjectedWallEntity : IPaintableEntity<CBaseProjectedEntity>
			BeginTemplatedMap("CProjectedWallEntity", null, "IPaintableEntity", "CBaseProjectedEntity");
			LinkNamesToMap("projected_wall_entity");
			DefineField("m_vWorldSpace_WallMins", VECTOR);
			DefineField("m_vWorldSpace_WallMaxs", VECTOR);
			DefineField("m_flLength", FLOAT);
			DefineField("m_flWidth", FLOAT);
			DefineField("m_flHeight", FLOAT);
			DefineField("m_bIsHorizontal", BOOLEAN);
			DefineField("m_flSegmentLength", FLOAT);
			DefineField("m_nNumSegments", INTEGER);
			
			BeginDataMap("CPortalStatsController", "CBaseEntity");
			LinkNamesToMap("portal_stats_controller");
			
			DeclareTemplatedClass("PropPaintPowerUser");
			DefineKeyField("m_PrePaintedPower", "PaintPower", INTEGER);
			DefineMaterialIndexDataOps("m_nOriginalMaterialIndex"); // seems to work, not 100% sure tho
			
			BeginTemplatedMap("IPaintableEntity", "CPhysicsProp", "CPhysicsProp", null);
			
			BeginTemplatedMap("PropPaintPowerUser", "IPaintableEntity<CPhysicsProp>", "IPaintableEntity", "CPhysicsProp");
			
			// there might be some classes missing in this hierarchy, but it'll just be proxies so whatever
			BeginTemplatedMap("CPropWeightedCube", null, "PropPaintPowerUser", "IPaintableEntity<CPhysicsProp>");
			LinkNamesToMap("prop_weighted_cube");
			DefineField("m_nCurrentPaintedType", INTEGER);
			DefineField("m_bNewSkins", BOOLEAN);
			DefineOutput("m_OnFizzled", "OnFizzled");
			DefineField("m_bTouchedByPlayer", BOOLEAN);
			DefineField("m_nCubeType", INTEGER);
			DefineField("m_pController", EHANDLE);
			
			BeginDataMap("CPointChangelevel", "CBaseEntity");
			LinkNamesToMap("point_changelevel");
			
			BeginDataMap("CProp_Portal", "CPortal_Base2D");
			LinkNamesToMap("prop_portal");
			DefineKeyField("m_iLinkageGroupID", "LinkageGroupID", CHARACTER);
			DefineKeyField("m_bActivated", "Activated", BOOLEAN);
			DefineKeyField("m_bOldActivatedState", "OldActivated", BOOLEAN);
			DefineKeyField("m_bIsPortal2", "PortalTwo", BOOLEAN);
			DefineField("m_NotifyOnPortalled", EHANDLE);
			DefineField("m_hFiredByPlayer", EHANDLE);
			DefineSoundPatch("m_pAmbientSound");
			DefineThinkFunc("DelayedPlacementThink");
			DefineInputFunc("SetActivatedState", "InputSetActivatedState", BOOLEAN);
			DefineInputFunc("Fizzle", "InputFizzle", VOID);
			DefineInputFunc("NewLocation", "InputNewLocation", STRING);
			DefineInputFunc("Resize", "InputResize", STRING);
			DefineInputFunc("SetLinkageGroupId", "InputSetLinkageGroupId", INTEGER);
			
			BeginDataMap("CPortal_Base2D", "CBaseAnimating");
			LinkNamesToMap("portal_base2D");
			DefineField("m_hLinkedPortal", EHANDLE);
			DefineField("m_matrixThisToLinked", VMATRIX);
			DefineKeyField("m_bActivated", "Activated", BOOLEAN);
			DefineKeyField("m_bOldActivatedState", "OldActivated", BOOLEAN);
			DefineKeyField("m_bIsPortal2", "PortalTwo", BOOLEAN);
			DefineField("m_vPrevForward", VECTOR);
			DefineField("m_hMicrophone", EHANDLE);
			DefineField("m_hSpeaker", EHANDLE);
			DefineField("m_bMicAndSpeakersLinkedToRemote", BOOLEAN);
			DefineField("m_vAudioOrigin", VECTOR);
			DefineField("m_vDelayedPosition", VECTOR);
			DefineField("m_qDelayedAngles", VECTOR);
			DefineField("m_iDelayedFailure", INTEGER);
			DefineField("m_vOldPosition", VECTOR);
			DefineField("m_qOldAngles", VECTOR);
			DefineField("m_hPlacedBy", EHANDLE);
			DefineKeyField("m_fNetworkHalfWidth", "HalfWidth", FLOAT);
			DefineKeyField("m_fNetworkHalfHeight", "HalfHeight", FLOAT);
			DefineField("m_bIsMobile", BOOLEAN);
			DefineField("m_bSharedEnvironmentConfiguration", BOOLEAN);
			DefineField("m_vPortalCorners", POSITION_VECTOR, 4);
			DefineVector("m_PortalEventListeners", EHANDLE);
			DefineThinkFunc("TestRestingSurfaceThink");
			DefineThinkFunc("DeactivatePortalNow");
			DefineOutput("m_OnPlacedSuccessfully", "OnPlacedSuccessfully");
			DefineOutput("m_OnEntityTeleportFromMe", "OnEntityTeleportFromMe");
			DefineOutput("m_OnPlayerTeleportFromMe", "OnPlayerTeleportFromMe");
			DefineOutput("m_OnEntityTeleportToMe", "OnEntityTeleportToMe");
			DefineOutput("m_OnPlayerTeleportToMe", "OnPlayerTeleportToMe");
			DefineField("m_vPortalSpawnLocation", VECTOR);
			
			BeginDataMap("CPortalButtonTrigger", "CBaseTrigger");
			LinkNamesToMap("trigger_portal_button");
			DefineField("m_pOwnerButton", CLASSPTR);
			
			BeginDataMap("CTriggerCatapult", "CBaseTrigger");
			LinkNamesToMap("trigger_catapult");
			DefineField("m_flPlayerVelocity", FLOAT);
			DefineField("m_flPhysicsVelocity", FLOAT);
			DefineField("m_strLaunchTarget", STRING);
			DefineField("m_bUseThresholdCheck", BOOLEAN);
			DefineField("m_bUseExactVelocity", BOOLEAN);
			DefineField("m_flLowerThreshold", FLOAT);
			DefineField("m_flUpperThreshold", FLOAT);
			DefineField("m_flAirControlSupressionTime", FLOAT);
			DefineField("m_hLaunchTarget", EHANDLE);
			DefineField("m_vecLaunchAngles", VECTOR);
			DefineField("m_bApplyAngularImpulse", BOOLEAN);
			DefineField("m_bOnlyVelocityCheck", BOOLEAN);
			DefineOutput("m_OnCatapulted", "OnCatapulted");
			
			BeginDataMap("CMovieDisplay", "CBaseEntity");
			LinkNamesToMap("vgui_movie_display");
			DefineField("m_bEnabled", BOOLEAN);
			DefineKeyField("m_szDisplayText", "displaytext", CHARACTER, 128);
			DefineField("m_szMovieFilename", CHARACTER, 128);
			DefineKeyField("m_strMovieFilename", "moviefilename", STRING);
			DefineField("m_szGroupName", CHARACTER, 128);
			DefineKeyField("m_strGroupName", "groupname", STRING); // Screens of the same group name will play the same movie at the same time
			DefineKeyField("m_iScreenWidth", "width", INTEGER);
			DefineKeyField("m_iScreenHeight", "height", INTEGER);
			DefineKeyField("m_bLooping", "looping", BOOLEAN);
			DefineKeyField("m_bStretchToFill", "stretch", BOOLEAN);
			DefineKeyField("m_bForcedSlave", "forcedslave", BOOLEAN);
			DefineKeyField("m_bForcePrecache", "forceprecache", BOOLEAN);
			DefineField("m_bUseCustomUVs", BOOLEAN);
			DefineField("m_flUMin", FLOAT);
			DefineField("m_flUMax", FLOAT);
			DefineField("m_flVMin", FLOAT);
			DefineField("m_flVMax", FLOAT);
			DefineField("m_bDoFullTransmit", BOOLEAN);
			DefineField("m_hScreen", EHANDLE);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("SetDisplayText", "InputSetDisplayText", STRING);
			DefineInputFunc("SetMovie", "InputSetMovie", STRING);
			DefineInputFunc("SetUseCustomUVs", "InputSetUseCustomUVs", BOOLEAN);
			DefineInputFunc("SetUMin", "InputSetUMin", FLOAT);
			DefineInputFunc("SetUMax", "InputSetUMax", FLOAT);
			DefineInputFunc("SetVMin", "InputSetVMin", FLOAT);
			DefineInputFunc("SetVMax", "InputSetVMax", FLOAT);
			DefineInputFunc("TakeOverAsMaster", "InputTakeOverAsMaster", VOID);
			
			BeginDataMap("CTriggerPaintCleanser", "CBaseTrigger");
			LinkNamesToMap("trigger_paint_cleanser");
			
			BeginDataMap("CProjectedTractorBeamEntity", "CBaseProjectedEntity");
			LinkNamesToMap("projected_tractor_beam_entity");
			DefineField("m_hTractorBeamTrigger", EHANDLE);
			
			BeginDataMap("CPropTractorBeamProjector", "CBaseProjector");
			LinkNamesToMap("prop_tractor_beam");
			DefineField("m_flLinearForce", FLOAT);
			DefineSoundPatch("m_sndMechanical");
			DefineSoundPatch("m_sndAmbientSound");
			DefineSoundPatch("m_sndAmbientMusic");
			DefineField("m_hAmbientSoundProxy", EHANDLE);
			DefineField("m_bUse128Model", BOOLEAN);
			
			BeginDataMap("CTrigger_TractorBeam", "CBaseVPhysicsTrigger");
			LinkNamesToMap("trigger_tractorbeam");
			DefinePhysPtr("m_pController");
			DefineField("m_linearLimit", FLOAT);
			DefineField("m_linearForce", FLOAT);
			DefineField("m_linearForceAngles", VECTOR);
			DefineSoundPatch("m_sndPlayerInBeam");
			DefineField("m_vStart", VECTOR);
			DefineField("m_vEnd", VECTOR);
			DefineField("m_flRadius", FLOAT);
			DefineField("m_hProxyEntity", EHANDLE);
			DefineField("m_bReversed", BOOLEAN);
			
			BeginDataMap("CPropMonsterBox", "CPhysicsProp");
			LinkNamesToMap("prop_monster_box");
			DefineField("m_flBoxSwitchSpeed", FLOAT);
			DefineField("m_flPushStrength", FLOAT);
			DefineField("m_nBodyGroups", INTEGER);
			DefineOutput("m_OnFizzled", "OnFizzled");
			DefineField("m_bIsABox", BOOLEAN);
			DefineField("m_bIsFlying", BOOLEAN);
			DefineField("m_bForcedAsBox", BOOLEAN);
			
			BeginDataMap("CCubeRotationController", "CPointEntity");
			LinkNamesToMap("cube_rotationcontroller");
			DefineField("m_bEnabled", BOOLEAN);
			DefineField("m_flSuspendTime", TIME);
			DefineField("m_worldGoalAxis", VECTOR);
			DefineField("m_localTestAxis", VECTOR);
			DefinePhysPtr("m_pController");
			DefineField("m_angularLimit", FLOAT);
			DefineField("m_pParent", CLASSPTR);
			
			BeginDataMap("CLaserCatcher", "CBaseAnimating");
			LinkNamesToMap("prop_laser_catcher");
			DefineField("m_pCatcherLaserTarget", EHANDLE);
			DefineField("m_PowerOnSequence", INTEGER);
			DefineField("m_iTargetAttachment", INTEGER);
			DefineOutput("m_OnPowered", "OnPowered");
			DefineOutput("m_OnUnpowered", "OnUnpowered");
			DefineField("m_iPowerState", INTEGER);
			
			BeginDataMap("CLaserRelay", "CLaserCatcher");
			LinkNamesToMap("prop_laser_relay");
			
			BeginDataMap("CPortalLaser", "CBaseAnimating");
			LinkNamesToMap("env_portal_laser");
			DefineField("m_vStartPoint", VECTOR);
			DefineField("m_vEndPoint", VECTOR);
			DefineField("m_bShouldSpark", BOOLEAN);
			DefineField("m_angPortalExitAngles", VECTOR);
			DefineField("m_bLaserOn", BOOLEAN);
			DefineField("m_pSoundProxy", CLASSPTR, 33);
			DefineField("m_pPlacementHelper", EHANDLE);
			DefineField("m_iLaserAttachment", INTEGER);
			DefineField("m_ModelName", STRING);
			DefineField("m_bAutoAimEnabled", BOOLEAN);
			DefineField("m_bStartOff", BOOLEAN);
			DefineField("m_angRotation", VECTOR);
			DefineField("m_nSimulationTick", INTEGER);
			DefineField("CLogicBranch", EHANDLE); // interesting name
			DefineOutput("m_OnTrue", "OnTrue");
			DefineOutput("m_OnFalse", "OnFalse");
			
			BeginDataMap("CPortalLaserTarget", "CBaseEntity");
			LinkNamesToMap("point_laser_target");
			DefineField("m_pCatcher", EHANDLE);
			DefineField("m_bTerminalPoint", BOOLEAN);
			DefineField("m_bPowered", BOOLEAN);
			
			DataMapProxy("CMoveableCamera", "CBaseEntity");
			
			BeginDataMap("CTriggerCameraMultiplayer", "CMoveableCamera");
			LinkNamesToMap("point_viewcontrol_multiplayer");
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("AddPlayer", "InputAddPlayer", VOID);
			DefineInputFunc("RemovePlayer", "InputRemovePlayer", VOID);
			DefineInputFunc("StartMovement", "InputStartMovement", VOID);
			DefineKeyField("m_fov", "fov", FLOAT);
			DefineKeyField("m_fovSpeed", "fov_rate", FLOAT);
			DefineKeyField("m_targetEntName", "target_entity", STRING);
			DefineKeyField("m_flInterpTime", "interp_time", FLOAT);
			DefineKeyField("m_nTeamNum", "target_team", INTEGER);
			
			BeginDataMap("CTriggerPlayerTeam", "CBaseTrigger");
			LinkNamesToMap("trigger_playerteam");
			DefineOutput("m_OnStartTouchOrangePlayer", "OnStartTouchOrangePlayer");
			DefineOutput("m_OnEndTouchOrangePlayer", "OnEndTouchOrangePlayer");
			DefineOutput("m_OnStartTouchBluePlayer", "OnStartTouchBluePlayer");
			DefineOutput("m_OnEndTouchBluePlayer", "OnEndTouchBluePlayer");
			DefineField("m_nTargetTeam", INTEGER);
			
			BeginDataMap("CLogicCoopManager", "CLogicalEntity");
			LinkNamesToMap("logic_coop_manager");
			DefineOutput("m_OnChangeToAllTrue", "OnChangeToAllTrue");
			DefineOutput("m_OnChangeToAnyTrue", "OnChangeToAnyTrue");
			DefineOutput("m_OnChangeToAllFalse", "OnChangeToAllFalse");
			DefineOutput("m_OnChangeToAnyFalse", "OnChangeToAnyFalse");
			DefineKeyField("m_bPlayerStateA", "DefaultPlayerStateA", BOOLEAN);
			DefineKeyField("m_bPlayerStateB", "DefaultPlayerStateB", BOOLEAN);
			DefineInputFunc("SetStateATrue", "InputSetStateATrue", INPUT);
			DefineInputFunc("SetStateAFalse", "InputSetStateAFalse", INPUT);
			DefineInputFunc("ToggleStateA", "InputToggleStateA", INPUT);
			DefineInputFunc("SetStateBTrue", "InputSetStateBTrue", INPUT);
			DefineInputFunc("SetStateBFalse", "InputSetStateBFalse", INPUT);
			DefineInputFunc("ToggleStateB", "InputToggleStateB", INPUT);
			
			BeginDataMap("CInfo_Coop_Spawn", "CBaseAnimating");
			LinkNamesToMap("info_coop_spawn");
			DefineField("m_bEnabled", BOOLEAN);
			DefineField("m_iStartingTeam", INTEGER);
			
			BeginDataMap("CLinkedPortalDoor", "CBaseAnimating"); // Non-animated linked portal door
			LinkNamesToMap("linked_portal_door");
			DefineInputFunc("SetPartner", "InputSetPartner", STRING);
			DefineInputFunc("Open", "InputOpen", VOID);
			DefineInputFunc("Close", "InputClose", VOID);
			DefineKeyField("m_nWidth", "width", INTEGER);
			DefineKeyField("m_nHeight", "height", INTEGER);
			DefineKeyField("m_szPartnerName", "partnername", STRING);
			DefineKeyField("m_bStartActive", "startactive", BOOLEAN);
			DefineField("m_bIsLinkedToPartner", BOOLEAN);
			DefineField("m_hPortal", EHANDLE);
			DefineField("m_hPartner", EHANDLE);
			DefineThinkFunc("DisableLinkageThink");
			DefineOutput("m_OnOpen", "OnOpen");
			DefineOutput("m_OnClose", "OnClose");
			DefineOutput("m_OnEntityTeleportFromMe", "OnEntityTeleportFromMe");
			DefineOutput("m_OnPlayerTeleportFromMe", "OnPlayerTeleportFromMe");
			DefineOutput("m_OnEntityTeleportToMe", "OnEntityTeleportToMe");
			DefineOutput("m_OnPlayerTeleportToMe", "OnPlayerTeleportToMe");
			
			BeginDataMap("CLogicPlayMovie", "CLogicalEntity");
			LinkNamesToMap("logic_playmovie");
			DefineKeyField("m_strMovieFilename", "MovieFilename", STRING);
			DefineKeyField("m_bAllowUserSkip", "allowskip", BOOLEAN);
			DefineKeyField("m_bLoopVideo", "loopvideo", BOOLEAN);
			DefineKeyField("m_bFadeInTime", "fadeintime", FLOAT);
			DefineInputFunc("PlayMovie", "InputPlayMovie", VOID);
			DefineInputFunc("PlayMovieForAllPlayers", "InputPlayMovieForAllPlayers", VOID);
			DefineInputFunc("PlayLevelTransitionMovie", "InputPlayLevelTransitionMovie", VOID);
			DefineInputFunc("FadeAllMovies", "InputFadeAllMovies", VOID);
			DefineInputFunc("__MovieFinished", "InputMovieFinished", VOID);
			DefineOutput("m_OnPlaybackFinished", "OnPlaybackFinished");
			
			BeginDataMap("CTriggerViewProxy", "CBaseEntity");
			LinkNamesToMap("point_viewproxy");
			DefineField("m_hPlayer", EHANDLE);
			DefineField("m_pProxy", CLASSPTR);
			DefineKeyField("m_nOffsetType", "offsettype", INTEGER);
			DefineField("m_vecInitialOffset", VECTOR);
			DefineField("m_flStartTime", FLOAT);
			DefineKeyField("m_sProxy", "proxy", STRING);
			DefineKeyField("m_sProxyAttachment", "proxyattachment", STRING);
			DefineKeyField("m_flTiltFraction", "tiltfraction", FLOAT);
			DefineKeyField("m_bUseFakeAcceleration", "usefakeacceleration", BOOLEAN);
			DefineKeyField("m_bSkewAccelerationForward", "skewaccelerationforward", BOOLEAN);
			DefineKeyField("m_flAccelerationScalar", "accelerationscalar", FLOAT);
			DefineKeyField("m_bEaseAnglesToCamera", "easeanglestocamera", BOOLEAN);
			DefineField("m_nParentAttachment", INTEGER);
			DefineField("m_state", INTEGER);
			DefineField("m_vecInitialPosition", VECTOR);
			DefineField("m_nPlayerButtons", INTEGER);
			DefineField("m_nOldTakeDamage", INTEGER);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("TeleportPlayerToProxy", "InputTeleportPlayerToProxy", VOID);
			DefineFunction("TranslateViewToProxy");
			
			BeginDataMap("CInfoGameEventProxy", "CPointEntity");
			LinkNamesToMap("info_game_event_proxy");
			DefineKeyField("m_iszEventName", "event_name", STRING);
			DefineKeyField("m_flRange", "range", FLOAT);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineInputFunc("GenerateGameEvent", "InputGenerateGameEvent", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			
			BeginDataMap("CEnvInstructorHint", "CBaseEntity");
			LinkNamesToMap("env_instructor_hint");
			DefineKeyField("m_iszReplace_Key", "hint_replace_key", STRING);
			DefineKeyField("m_iszHintTargetEntity", "hint_target", STRING);
			DefineKeyField("m_iTimeout", "hint_timeout", INTEGER);
			DefineKeyField("m_iszIcon_Onscreen", "hint_icon_onscreen", STRING);
			DefineKeyField("m_iszIcon_Offscreen", "hint_icon_offscreen", STRING);
			DefineKeyField("m_iszCaption", "hint_caption", STRING);
			DefineKeyField("m_iszActivatorCaption", "hint_activator_caption", STRING);
			DefineKeyField("m_Color", "hint_color", COLOR32);
			DefineKeyField("m_fIconOffset", "hint_icon_offset", FLOAT);
			DefineKeyField("m_fRange", "hint_range", FLOAT);
			DefineKeyField("m_iPulseOption", "hint_pulseoption", CHARACTER);
			DefineKeyField("m_iAlphaOption", "hint_alphaoption", CHARACTER);
			DefineKeyField("m_iShakeOption", "hint_shakeoption", CHARACTER);
			DefineKeyField("m_bStatic", "hint_static", BOOLEAN);
			DefineKeyField("m_bNoOffscreen", "hint_nooffscreen", BOOLEAN);
			DefineKeyField("m_bForceCaption", "hint_forcecaption", BOOLEAN);
			DefineKeyField("m_iszBinding", "hint_binding", STRING);
			DefineKeyField("m_iszGamepadBinding", "hint_gamepad_binding", STRING);
			DefineKeyField("m_bAllowNoDrawTarget", "hint_allow_nodraw_target", BOOLEAN);
			DefineKeyField("m_bLocalPlayerOnly", "hint_local_player_only", BOOLEAN);
			DefineInputFunc("ShowHint", "InputShowHint", STRING);
			DefineInputFunc("EndHint", "InputEndHint", VOID);
		}
	}
}