// ReSharper disable All
using SaveParser.Parser.SaveFieldInfo.DataMaps.CustomFields;
using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public sealed class EntMaps1 : DataMapInfoGenerator {
		
		public const int NUM_BONECTRLS = 4;
		public const int NUM_POSEPAREMETERS = 24;
		public const int NUM_AUDIO_LOCAL_SOUNDS = 8;
		public const int MAX_AREA_STATE_BYTES = 32;
		public const int MAX_AREA_PORTAL_STATE_BYTES = 24;
		public const int MAX_PLACE_NAME_LENGTH = 18;
		public const int CDMG_TIMEBASED = 8;
		public const int MAX_VIEWMODELS = 2;
		public const int MAX_PLAYER_NAME_LENGTH = 32;
		public const int MAX_NETWORKID_LENGTH = 64;
		public const int MAX_ITEMS = 5;
		public const int CSUITPLAYLIST = 4;
		public const int CSUITNOREPEAT = 32;
		public const int MAX_AMMO_SLOTS = 32;
		public const int MAX_WEAPONS = 48;
		public const int MAXSTUDIOFLEXCTRL = 96;
		public const int MAX_POWERUPS = 4;


		protected override void GenerateDataMaps() {
			BeginDataMap("CBaseEntity");
			DefineKeyField("m_iClassname", "classname", STRING);
			DefineGlobalKeyField("m_iGlobalname", "globalname", STRING);
			DefineKeyField("m_iParent", "parentname", STRING);
			DefineKeyField("m_iHammerID", "hammerid", INTEGER);
			DefineKeyField("m_flSpeed", "speed", FLOAT);
			DefineKeyField("m_nRenderFX", "renderfx", BYTE);
			DefineKeyField("m_nRenderMode", "rendermode", BYTE);
			DefineField("m_flPrevAnimTime", TIME);
			DefineField("m_flAnimTime", TIME);
			DefineField("m_flSimulationTime", TIME);
			DefineField("m_nLastThinkTick", TICK);
			DefineKeyField("m_nNextThinkTick", "nextthink", TICK);
			DefineKeyField("m_fEffects", "effects", INTEGER);
			DefineKeyField("m_clrRender", "rendercolor", COLOR32);
			DefineGlobalKeyField("m_nModelIndex", "modelindex", SHORT);
			DefineField("touchStamp", INTEGER);
			DefineCustomField("m_aThinkFunctions", ThinkContexts.Restore);
			DefineVector("m_ResponseContexts", "ResponseContext_t");
			DefineKeyField("m_iszResponseContext", "ResponseContext", STRING);
			DefineField("m_pfnThink", FUNCTION);
			DefineField("m_pfnTouch", FUNCTION);
			DefineField("m_pfnUse", FUNCTION);
			DefineField("m_pfnBlocked", FUNCTION);
			DefineField("m_pfnMoveDone", FUNCTION);
			DefineField("m_lifeState", BYTE);
			DefineField("m_takedamage", BYTE);
			DefineKeyField("m_iMaxHealth", "max_health", INTEGER);
			DefineKeyField("m_iHealth", "health", INTEGER);
			DefineKeyField("m_target", "target", STRING);
			DefineKeyField("m_iszDamageFilterName", "damagefilter", STRING);
			DefineField("m_hDamageFilter", EHANDLE);
			DefineField("m_debugOverlays", INTEGER);
			DefineGlobalField("m_pParent", EHANDLE);
			DefineField("m_iParentAttachment", BYTE);
			DefineGlobalField("m_hMoveParent", EHANDLE);
			DefineGlobalField("m_hMoveChild", EHANDLE);
			DefineGlobalField("m_hMovePeer", EHANDLE);
			DefineField("m_iEFlags", INTEGER);
			DefineField("m_iName", STRING);
			DefineEmbeddedField("m_Collision", "CCollisionProperty");
			DefineEmbeddedField("m_Network", "CServerNetworkProperty");
			DefineField("m_MoveType", BYTE);
			DefineField("m_MoveCollide", BYTE);
			DefineField("m_hOwnerEntity", EHANDLE);
			if (Game == Game.PORTAL2)
				DefineKeyField("m_CollisionGroup", "CollisionGroup", INTEGER);
			else
				DefineField("m_CollisionGroup", INTEGER);
			DefinePhysPtr("m_pPhysicsObject");
			DefineField("m_flElasticity", FLOAT);
			DefineKeyField("m_flShadowCastDistance", "shadowcastdist", FLOAT);
			DefineField("m_flDesiredShadowCastDistance", FLOAT);
			DefineInput("m_iInitialTeamNum", "TeamNum", INTEGER);
			DefineField("m_iTeamNum", INTEGER);
			DefineField("m_hGroundEntity", EHANDLE);
			DefineField("m_flGroundChangeTime", TIME);
			DefineGlobalKeyField("m_ModelName", "model", MODELNAME);
			DefineKeyField("m_vecBaseVelocity", "basevelocity", VECTOR);
			DefineField("m_vecAbsVelocity", VECTOR);
			DefineKeyField("m_vecAngVelocity", "avelocity", VECTOR);
			DefineField("m_rgflCoordinateFrame", MATRIX3X4_WORLDSPACE);
			DefineKeyField("m_nWaterLevel", "waterlevel", CHARACTER);
			DefineField("m_nWaterType", BYTE);
			DefineField("m_pBlocker", EHANDLE);
			DefineKeyField("m_flGravity", "gravity", FLOAT);
			DefineKeyField("m_flFriction", "friction", FLOAT);
			DefineKeyField("m_flLocalTime", "ltime", FLOAT);
			DefineField("m_flVPhysicsUpdateLocalTime", FLOAT);
			DefineField("m_flMoveDoneTime", FLOAT);
			DefineField("m_vecAbsOrigin", POSITION_VECTOR);
			DefineKeyField("m_vecVelocity", "velocity", VECTOR);
			DefineKeyField("m_iTextureFrameIndex", "texframeindex", CHARACTER);
			DefineField("m_bSimulatedEveryTick", BOOLEAN);
			DefineField("m_bAnimatedEveryTick", BOOLEAN);
			DefineField("m_bAlternateSorting", BOOLEAN);
			DefineKeyField("m_spawnflags", "spawnflags", INTEGER);
			DefineField("m_nTransmitStateOwnedCounter", BYTE);
			DefineField("m_angAbsRotation", VECTOR);
			DefineField("m_vecOrigin", VECTOR);
			DefineField("m_angRotation", VECTOR);
			DefineKeyField("m_vecViewOffset", "view_ofs", VECTOR);
			DefineField("m_fFlags", INTEGER);
			DefineField("m_nSimulationTick", TICK);
			DefineField("m_flNavIgnoreUntilTime", TIME);
			DefineInputFunc("SetTeam", "InputSetTeam", INTEGER);
			DefineInputFunc("Kill", "InputKill", VOID);
			DefineInputFunc("KillHierarchy", "InputKillHierarchy", VOID);
			DefineInputFunc("Use", "InputUse", VOID);
			DefineInputFunc("Alpha", "InputAlpha", INTEGER);
			DefineInputFunc("AlternativeSorting", "InputAlternativeSorting", BOOLEAN);
			DefineInputFunc("Color", "InputColor", COLOR32);
			DefineInputFunc("SetParent", "InputSetParent", STRING);
			DefineInputFunc("SetParentAttachment", "InputSetParentAttachment", STRING);
			DefineInputFunc("SetParentAttachmentMaintainOffset", "InputSetParentAttachmentMaintainOffset", STRING);
			DefineInputFunc("ClearParent", "InputClearParent", VOID);
			DefineInputFunc("SetDamageFilter", "InputSetDamageFilter", STRING);
			DefineInputFunc("EnableDamageForces", "InputEnableDamageForces", VOID);
			DefineInputFunc("DisableDamageForces", "InputDisableDamageForces", VOID);
			if (Game != Game.PORTAL2)
				DefineInputFunc("DispatchEffect", "InputDispatchEffect", STRING);
			DefineInputFunc("DispatchResponse", "InputDispatchResponse", STRING);
			DefineInputFunc("AddContext", "InputAddContext", STRING);
			DefineInputFunc("RemoveContext", "InputRemoveContext", STRING);
			DefineInputFunc("ClearContext", "InputClearContext", STRING);
			DefineInputFunc("DisableShadow", "InputDisableShadow", VOID);
			DefineInputFunc("EnableShadow", "InputEnableShadow", VOID);
			DefineInputFunc("AddOutput", "InputAddOutput", STRING);
			DefineInputFunc("FireUser1", "InputFireUser1", STRING);
			DefineInputFunc("FireUser2", "InputFireUser2", STRING);
			DefineInputFunc("FireUser3", "InputFireUser3", STRING);
			DefineInputFunc("FireUser4", "InputFireUser4", STRING);
			DefineOutput("m_OnUser1", "OnUser1");
			DefineOutput("m_OnUser2", "OnUser2");
			DefineOutput("m_OnUser3", "OnUser3");
			DefineOutput("m_OnUser4", "OnUser4");
			DefineFunction("SUB_Remove");
			DefineFunction("SUB_DoNothing");
			DefineFunction("SUB_StartFadeOut");
			DefineFunction("SUB_StartFadeOutInstant");
			DefineFunction("SUB_FadeOut");
			DefineFunction("SUB_Vanish");
			DefineFunction("SUB_CallUseToggle");
			DefineThinkFunc("ShadowCastDistThink");
			DefineField("m_hEffectEntity", EHANDLE);
			if (Game == Game.PORTAL2) {
				DefineKeyField("m_nMinCPULevel", "mincpulevel", BYTE);
				DefineKeyField("m_nMaxCPULevel", "maxcpulevel", BYTE);
				DefineKeyField("m_nMinGPULevel", "mingpulevel", BYTE);
				DefineKeyField("m_nMaxGPULevel", "maxgpulevel", BYTE);
				DefineField("m_iszScriptId", STRING);
				DefineKeyField("m_iszVScripts", "vscripts", STRING);
				DefineKeyField("m_iszScriptThinkFunction", "thinkfunction", STRING);
				DefineField("m_flCreateTime", TIME);
				DefineField("m_iSignifierName", STRING);
				DefineField("m_flDmgModFire", FLOAT);
				DefineKeyField("m_AIAddOn", "addon", STRING);
				DefineField("m_bClientSideRagdoll", BOOLEAN);
				DefineInput("m_fadeMinDist", "fademindist", FLOAT);
				DefineInput("m_fadeMaxDist", "fademaxdist", FLOAT);
				DefineKeyField("m_flFadeScale", "fadescale", FLOAT);
				DefineInputFunc("RunScriptFile", "InputRunScriptFile", STRING);
				DefineInputFunc("RunScriptCode", "InputRunScript", STRING);
				DefineInputFunc("CallScriptFunction", "InputCallScriptFunction", STRING);
				DefineThinkFunc("FrictionRevertThink");
				DefineThinkFunc("ScriptThink");
				DefineKeyField("m_bLagCompensate", "LagCompensate", BOOLEAN);
				DefineField("m_iObjectCapsCache", INTEGER);
			}

			BeginDataMap("CWorld", "CBaseEntity");
			LinkNamesToMap("worldspawn");
			DefineField("m_flWaveHeight", FLOAT);
			DefineKeyField("m_iszChapterTitle", "chaptertitle", STRING);
			DefineKeyField("m_bStartDark", "startdark", BOOLEAN);
			DefineKeyField("m_bDisplayTitle", "gametitle", BOOLEAN);
			DefineField("m_WorldMins", VECTOR);
			DefineField("m_WorldMaxs", VECTOR);
			if (GenInfo.IsXBox) {
				DefineKeyField("m_flMaxOccludeeArea", "maxoccludeearea_x360", FLOAT);
				DefineKeyField("m_flMinOccluderArea", "minoccluderarea_x360", FLOAT);
			} else {
				DefineKeyField("m_flMaxOccludeeArea", "maxoccludeearea", FLOAT);
				DefineKeyField("m_flMinOccluderArea", "minoccluderarea", FLOAT);
			}
			DefineKeyField("m_flMaxPropScreenSpaceWidth", "maxpropscreenwidth", FLOAT);
			DefineKeyField("m_flMinPropScreenSpaceWidth", "minpropscreenwidth", FLOAT);
			DefineKeyField("m_iszDetailSpriteMaterial", "detailmaterial", STRING);
			DefineKeyField("m_bColdWorld", "coldworld", BOOLEAN);
			if (Game == Game.PORTAL2)
				DefineField("m_nMaxBlobCount", INTEGER);
			
			BeginDataMap("CAnimationLayer");
			DefineField("m_fFlags", INTEGER);
			DefineField("m_bSequenceFinished", BOOLEAN);
			DefineField("m_bLooping", BOOLEAN);
			DefineField("m_nSequence", INTEGER);
			DefineField("m_flCycle", FLOAT);
			DefineField("m_flPrevCycle", FLOAT);
			DefineField("m_flPlaybackRate", FLOAT);
			DefineField("m_flWeight", FLOAT);
			DefineField("m_flBlendIn", FLOAT);
			DefineField("m_flBlendOut", FLOAT);
			DefineField("m_flKillRate", FLOAT);
			DefineField("m_flKillDelay", FLOAT);
			DefineCustomField("m_nActivity", ActivityData.Restore);
			DefineField("m_nPriority", INTEGER);
			DefineField("m_nOrder", INTEGER);
			DefineField("m_flLastEventCheck", FLOAT);
			DefineField("m_flLastAccess", TIME);
			DefineField("m_flLayerAnimtime", FLOAT);
			DefineField("m_flLayerFadeOuttime", FLOAT);
			
			BeginDataMap("CBaseAnimating", "CBaseEntity");
			DefineField("m_flGroundSpeed", FLOAT);
			DefineField("m_flLastEventCheck", TIME);
			DefineField("m_bSequenceFinished", BOOLEAN);
			DefineField("m_bSequenceLoops", BOOLEAN);
			DefineInput("m_nSkin", "skin", INTEGER);
			DefineInputAndKeyField("m_nBody", "body", "SetBodyGroup", INTEGER);
			DefineKeyField("m_nHitboxSet", "hitboxset", INTEGER);
			DefineKeyField("m_nSequence", "sequence", INTEGER);
			DefineField("m_flPoseParameter", FLOAT, NUM_POSEPAREMETERS);
			DefineField("m_flEncodedController", FLOAT, NUM_BONECTRLS);
			DefineKeyField("m_flPlaybackRate", "playbackrate", FLOAT);
			DefineKeyField("m_flCycle", "cycle", FLOAT);
			DefineField("m_pIk", BOOLEAN); // custom field name "bHasIK", but that's too much effort
			DefineField("m_iIKCounter", INTEGER);
			DefineField("m_bClientSideAnimation", BOOLEAN);
			DefineField("m_bClientSideFrameReset", BOOLEAN);
			DefineField("m_nNewSequenceParity", INTEGER);
			DefineField("m_nResetEventsParity", INTEGER);
			DefineField("m_nMuzzleFlashParity", BYTE);
			DefineKeyField("m_iszLightingOriginRelative", "LightingOriginHack", STRING);
			DefineKeyField("m_iszLightingOrigin", "LightingOrigin", STRING);
			DefineField("m_hLightingOrigin", EHANDLE);
			DefineField("m_hLightingOriginRelative", EHANDLE);
			if (Game == Game.PORTAL2)
				DefineKeyField("m_flModelScale", "ModelScale", FLOAT);
			else
				DefineField("m_flModelWidthScale", FLOAT);
			DefineField("m_flDissolveStartTime", TIME);
			DefineInputFunc("Ignite", "InputIgnite", VOID);
			DefineInputFunc("IgniteLifetime", "InputIgniteLifetime", FLOAT);
			if (Game == Game.PORTAL2) {
				DefineInputFunc("IgniteNumHitboxFires", "InputIgnite", INTEGER);
				DefineInputFunc("IgniteHitboxFireScale", "InputIgnite", FLOAT);
			} else {
				DefineInputFunc("IgniteNumHitboxFires", "InputIgniteNumHitboxFires", INTEGER);
				DefineInputFunc("IgniteHitboxFireScale", "InputIgniteHitboxFireScale", FLOAT);
			}
			DefineInputFunc("BecomeRagdoll", "InputBecomeRagdoll", VOID);
			DefineInputFunc("SetLightingOriginHack", "InputSetLightingOriginRelative", STRING);
			DefineInputFunc("SetLightingOrigin", "InputSetLightingOrigin", STRING);
			DefineOutput("m_OnIgnite", "OnIgnite");
			DefineInput("m_fadeMinDist", "fademindist", FLOAT);
			DefineInput("m_fadeMaxDist", "fademaxdist", FLOAT);
			DefineKeyField("m_flFadeScale", "fadescale", FLOAT);
			DefineField("m_fBoneCacheFlags", SHORT);
			if (Game == Game.PORTAL2)
				DefineField("m_flFrozenThawRate", FLOAT);
			
			BeginDataMap("CBaseAnimatingOverlay", "CBaseAnimating");
			DefineVector("m_AnimOverlay", "CAnimationLayer");
			
			BeginDataMap("CBaseFlex", "CBaseAnimatingOverlay"); // Animated characters who have vertex flex capability (e.g., facial expressions)
			DefineField("m_flexWeight", FLOAT, MAXSTUDIOFLEXCTRL);
			DefineField("m_viewtarget", POSITION_VECTOR);
			DefineField("m_flAllowResponsesEndTime", TIME);
			DefineField("m_flLastFlexAnimationTime", TIME);
			if (GenInfo.IsDefHl2Dll) {
				DefineField("m_vecLean", VECTOR);
				DefineField("m_vecShift", VECTOR);
			}
			
			BeginDataMap("Relationship_t");
			DefineField("entity", EHANDLE);
			DefineField("classType", INTEGER); // todo enum types (Class_T)
			DefineField("disposition", INTEGER); // todo enum types (Disposition_t)
			DefineField("priority", INTEGER);
			
			BeginDataMap("CBaseCombatCharacter", "CBaseFlex");
			if (GenInfo.IsDefInvasionDll) {
				DefineField("m_iPowerups", INTEGER);
				DefineField("m_flPowerupAttemptTimes", TIME, MAX_POWERUPS);
				DefineField("m_flPowerupEndTimes", TIME, MAX_POWERUPS);
				DefineField("m_flFractionalBoost", FLOAT);
			}
			DefineField("m_flNextAttack", TIME);
			DefineField("m_eHull", INTEGER);
			DefineField("m_bloodColor", INTEGER);
			DefineField("m_iDamageCount", INTEGER);
			DefineField("m_flFieldOfView", FLOAT);
			DefineField("m_HackedGunPos", VECTOR);
			DefineKeyField("m_RelationshipString", "Relationship", STRING);
			DefineField("m_LastHitGroup", INTEGER);
			DefineField("m_flDamageAccumulator", FLOAT);
			DefineInput("m_impactEnergyScale", "physdamagescale", FLOAT);
			DefineField("m_CurrentWeaponProficiency", INTEGER);
			DefineVector("m_Relationship", "Relationship_t");
			DefineField("m_iAmmo", INTEGER, MAX_AMMO_SLOTS);
			DefineField("m_hMyWeapons", EHANDLE, MAX_WEAPONS);
			DefineField("m_hActiveWeapon", EHANDLE);
			DefineField("m_bForceServerRagdoll", BOOLEAN);
			DefineField("m_bPreventWeaponPickup", BOOLEAN);
			DefineInputFunc("KilledNPC", "InputKilledNPC", VOID);
			
			BeginDataMap("CPlayerState");
			DefineField("v_angle", VECTOR);
			DefineField("deadflag", BOOLEAN);
			
			BeginDataMap("CBasePlayer", "CBaseCombatCharacter");
			DefineEmbeddedField("m_Local", "CPlayerLocalData");
			DefineVector("m_hTriggerSoundscapeList", EHANDLE);
			DefineEmbeddedField("pl", "CPlayerState");
			DefineField("m_StuckLast", INTEGER);
			DefineField("m_nButtons", INTEGER);
			DefineField("m_afButtonLast", INTEGER);
			DefineField("m_afButtonPressed", INTEGER);
			DefineField("m_afButtonReleased", INTEGER);
			DefineField("m_afButtonDisabled", INTEGER);
			DefineField("m_afButtonForced", INTEGER);
			DefineField("m_iFOV", INTEGER);
			DefineField("m_iFOVStart", INTEGER);
			DefineField("m_flFOVTime", TIME);
			DefineField("m_iDefaultFOV", INTEGER);
			DefineField("m_flVehicleViewFOV", FLOAT);
			DefineField("m_iObserverMode", INTEGER);
			DefineField("m_iObserverLastMode", INTEGER);
			DefineField("m_hObserverTarget", EHANDLE);
			DefineField("m_bForcedObserverMode", BOOLEAN);
			DefineField("m_szAnimExtension", CHARACTER, 32);
			DefineField("m_nUpdateRate", INTEGER);
			DefineField("m_fLerpTime", FLOAT);
			DefineField("m_bLagCompensation", BOOLEAN);
			DefineField("m_bPredictWeapons", BOOLEAN);
			DefineField("m_vecAdditionalPVSOrigin", POSITION_VECTOR);
			DefineField("m_vecCameraPVSOrigin", POSITION_VECTOR);
			DefineField("m_hUseEntity", EHANDLE);
			DefineField("m_iTrain", INTEGER);
			DefineField("m_iRespawnFrames", FLOAT);
			DefineField("m_afPhysicsFlags", INTEGER);
			DefineField("m_hVehicle", EHANDLE);
			DefineField("m_szNetworkIDString", CHARACTER, MAX_NETWORKID_LENGTH);
			DefineField("m_oldOrigin", POSITION_VECTOR);
			DefineField("m_vecSmoothedVelocity", VECTOR);
			DefineField("m_iTargetVolume", INTEGER);
			DefineField("m_rgItems", INTEGER, MAX_ITEMS);
			if (Game != Game.PORTAL2) {
				DefineField("m_flSwimTime", TIME);
				DefineField("m_flDuckTime", TIME);
				DefineField("m_flDuckJumpTime", TIME);
			}
			DefineField("m_flSuitUpdate", TIME);
			DefineField("m_rgSuitPlayList", INTEGER, CSUITPLAYLIST);
			DefineField("m_iSuitPlayNext", INTEGER);
			DefineField("m_rgiSuitNoRepeat", INTEGER, CSUITNOREPEAT);   // suit sentence no repeat list
			DefineField("m_rgflSuitNoRepeatTime", TIME, CSUITNOREPEAT); // how long to wait before allowing repeat
			DefineField("m_bPauseBonusProgress", BOOLEAN);
			DefineField("m_iBonusProgress", INTEGER);
			DefineField("m_iBonusChallenge", INTEGER);
			DefineField("m_lastDamageAmount", INTEGER);
			DefineField("m_tbdPrev", TIME);
			DefineField("m_flStepSoundTime", FLOAT);
			DefineField("m_szNetname", CHARACTER, MAX_PLAYER_NAME_LENGTH);
			DefineField("m_idrowndmg", INTEGER);
			DefineField("m_idrownrestored", INTEGER);
			DefineField("m_nPoisonDmg", INTEGER);
			DefineField("m_nPoisonRestored", INTEGER);
			DefineField("m_bitsHUDDamage", INTEGER);
			DefineField("m_fInitHUD", BOOLEAN);
			DefineField("m_flDeathTime", TIME);
			DefineField("m_flDeathAnimTime", TIME);
			DefineField("m_iFrags", INTEGER);
			DefineField("m_iDeaths", INTEGER);
			DefineField("m_bAllowInstantSpawn", BOOLEAN);
			DefineField("m_flNextDecalTime", TIME);
			DefineField("m_ArmorValue", INTEGER);
			DefineField("m_DmgOrigin", VECTOR);
			DefineField("m_DmgTake", FLOAT);
			DefineField("m_DmgSave", FLOAT);
			DefineField("m_AirFinished", TIME);
			DefineField("m_PainFinished", TIME);
			DefineField("m_iPlayerLocked", INTEGER);
			DefineField("m_hViewModel", EHANDLE, MAX_VIEWMODELS);
			DefineField("m_flMaxspeed", FLOAT);
			DefineField("m_flWaterJumpTime", TIME);
			DefineField("m_vecWaterJumpVel", VECTOR);
			DefineField("m_nImpulse", INTEGER);
			DefineField("m_flSwimSoundTime", TIME);
			DefineField("m_vecLadderNormal", VECTOR);
			DefineField("m_flFlashTime", TIME);
			DefineField("m_nDrownDmgRate", INTEGER);
			DefineField("m_iSuicideCustomKillFlags", INTEGER);
			DefineField("m_bitsDamageType", INTEGER);
			DefineField("m_rgbTimeBasedDamage", BYTE, CDMG_TIMEBASED);
			DefineField("m_fLastPlayerTalkTime", FLOAT);
			DefineField("m_hLastWeapon", EHANDLE);
			DefineField("m_flOldPlayerZ", FLOAT);
			DefineField("m_flOldPlayerViewOffsetZ", FLOAT);
			DefineField("m_bPlayerUnderwater", BOOLEAN);
			DefineField("m_hViewEntity", EHANDLE);
			DefineField("m_hConstraintEntity", EHANDLE);
			DefineField("m_vecConstraintCenter", VECTOR);
			DefineField("m_flConstraintRadius", FLOAT);
			DefineField("m_flConstraintWidth", FLOAT);
			DefineField("m_flConstraintSpeedFactor", FLOAT);
			if (Game == Game.PORTAL2)
				DefineField("m_bConstraintPastRadius", BOOLEAN);
			DefineField("m_hZoomOwner", EHANDLE);
			DefineField("m_flLaggedMovementValue", FLOAT);
			DefineField("m_vNewVPhysicsPosition", VECTOR);
			DefineField("m_vNewVPhysicsVelocity", VECTOR);
			DefineField("m_bSinglePlayerGameEnding", BOOLEAN);
			DefineField("m_szLastPlaceName", CHARACTER, MAX_PLACE_NAME_LENGTH);
			DefineField("m_autoKickDisabled", BOOLEAN);
			DefineFunction("PlayerDeathThink");
			DefineInputFunc("SetHealth", "InputSetHealth", INTEGER);
			DefineInputFunc("SetHUDVisibility", "InputSetHUDVisibility", BOOLEAN);
			DefineInputFunc("SetFogController", "InputSetFogController", STRING);
			DefineField("m_nNumCrouches", INTEGER);
			DefineField("m_bDuckToggled", BOOLEAN);
			DefineField("m_flForwardMove", FLOAT);
			DefineField("m_flSideMove", FLOAT);
			DefineField("m_vecPreviouslyPredictedOrigin", POSITION_VECTOR);
			DefineField("m_nNumCrateHudHints", INTEGER);
			if (Game == Game.PORTAL2) {
				DefineField("m_hPostProcessCtrl", EHANDLE);
				DefineField("m_hColorCorrectionCtrl", EHANDLE);
				DefineEmbeddedField("m_PlayerFog", "fogplayerparams_t");
				DefineField("m_bDropEnabled", BOOLEAN);
				DefineField("m_bDuckEnabled", BOOLEAN);
				DefineField("m_fTimeLastHurt", FLOAT);
			}
			
			BeginDataMap("CHL2PlayerLocalData");
			DefineField("m_flSuitPower", FLOAT);
			DefineField("m_bZooming", BOOLEAN);
			DefineField("m_bitsActiveDevices", INTEGER);
			DefineField("m_iSquadMemberCount", INTEGER);
			DefineField("m_iSquadMedicCount", INTEGER);
			DefineField("m_fSquadInFollowMode", BOOLEAN);
			DefineField("m_bWeaponLowered", BOOLEAN);
			DefineField("m_bDisplayReticle", BOOLEAN);
			DefineField("m_bStickyAutoAim", BOOLEAN);
			if (GenInfo.IsDefHl2Episodic) {
				DefineField("m_flFlashBattery", FLOAT);
				DefineField("m_vecLocatorOrigin", POSITION_VECTOR);
			}
			DefineField("m_flFlashBattery", FLOAT);             // hl2 episodic
			DefineField("m_vecLocatorOrigin", POSITION_VECTOR); // hl2 episodic
			DefineField("m_hLadder", EHANDLE);
			DefineEmbeddedField("m_LadderMove", "LadderMove_t");

			if (Game != Game.PORTAL2) {
				BeginDataMap("CHL2_Player", "CBasePlayer");
				DefineField("m_nControlClass", INTEGER);
				DefineEmbeddedField("m_HL2Local", "CHL2PlayerLocalData");
				DefineField("m_bSprintEnabled", BOOLEAN);
				DefineField("m_flTimeAllSuitDevicesOff", TIME);
				DefineField("m_fIsSprinting", BOOLEAN);
				DefineField("m_fIsWalking", BOOLEAN);
				DefineField("m_vecMissPositions", POSITION_VECTOR, 16);
				DefineField("m_nNumMissPositions", INTEGER);
				DefineEmbeddedField("m_CommanderUpdateTimer", "CSimpleSimTimer");
				DefineField("m_QueuedCommand", INTEGER);
				DefineField("m_flTimeIgnoreFallDamage", TIME);
				DefineField("m_bIgnoreFallDamageResetAfterImpact", BOOLEAN);
				DefineField("m_flSuitPowerLoad", FLOAT);
				DefineField("m_flIdleTime", TIME);
				DefineField("m_flMoveTime", TIME);
				DefineField("m_flLastDamageTime", TIME);
				DefineField("m_flTargetFindTime", TIME);
				DefineField("m_flAdmireGlovesAnimTime", TIME);
				DefineField("m_flNextFlashlightCheckTime", TIME);
				DefineField("m_flFlashlightPowerDrainScale", FLOAT);
				DefineField("m_bFlashlightDisabled", BOOLEAN);
				DefineField("m_bUseCappedPhysicsDamageTable", BOOLEAN);
				DefineField("m_hLockedAutoAimEntity", EHANDLE);
				DefineEmbeddedField("m_LowerWeaponTimer", "CSimpleSimTimer");
				DefineEmbeddedField("m_AutoaimTimer", "CSimpleSimTimer");
				DefineInputFunc("IgnoreFallDamage", "InputIgnoreFallDamage", FLOAT);
				DefineInputFunc("IgnoreFallDamageWithoutReset", "InputIgnoreFallDamageWithoutReset", FLOAT);
				DefineInputFunc("OnSquadMemberKilled", "OnSquadMemberKilled", VOID);
				DefineInputFunc("DisableFlashlight", "InputDisableFlashlight", VOID);
				DefineInputFunc("EnableFlashlight", "InputEnableFlashlight", VOID);
				DefineInputFunc("ForceDropPhysObjects", "InputForceDropPhysObjects", VOID);
				DefineSoundPatch("m_sndLeeches");
				DefineSoundPatch("m_sndWaterSplashes");
				DefineField("m_flArmorReductionTime", TIME);
				DefineField("m_iArmorReductionFrom", INTEGER);
				DefineField("m_flTimeUseSuspended", TIME);
				DefineField("m_hLocatorTargetEntity", EHANDLE);
				DefineField("m_flTimeNextLadderHint", TIME);
			}

			BeginDataMap("fogparams_t");
			DefineField("dirPrimary", VECTOR);
			DefineField("colorPrimary", COLOR32);
			DefineField("colorSecondary", COLOR32);
			DefineField("colorPrimaryLerpTo", COLOR32);
			DefineField("colorSecondaryLerpTo", COLOR32);
			DefineField("start", FLOAT);
			DefineField("end", FLOAT);
			DefineField("farz", FLOAT);
			DefineField("maxdensity", FLOAT);
			DefineField("startLerpTo", FLOAT);
			DefineField("endLerpTo", FLOAT);
			DefineField("maxdensityLerpTo", FLOAT);
			DefineField("lerptime", FLOAT);
			DefineField("duration", FLOAT);
			DefineField("enable", BOOLEAN);
			DefineField("blend", BOOLEAN);
			DefineField("HDRColorScale", FLOAT);
			
			BeginDataMap("sky3dparams_t");
			DefineField("scale", INTEGER);
			DefineField("origin", VECTOR);
			DefineField("area", INTEGER);
			DefineEmbeddedField("fog", "fogparams_t");
			
			BeginDataMap("audioparams_t");
			DefineField("localSound", VECTOR, NUM_AUDIO_LOCAL_SOUNDS);
			DefineField("soundscapeIndex", INTEGER); // index of the current soundscape from soundscape.txt
			DefineField("localBits", INTEGER);       // if bits 0,1,2,3 are set then position 0,1,2,3 are valid/used
			DefineField("entIndex", INTEGER);        // the entity setting the soundscape
			DefineField("ent", EHANDLE); // todo is really ehandle?
			
			BeginDataMap("fogplayerparams_t");
			DefineField("m_hCtrl", EHANDLE); // todo check this
			DefineField("m_flTransitionTime", FLOAT);
			DefineField("m_OldColor", COLOR32);
			DefineField("m_flOldStart", FLOAT);
			DefineField("m_flOldEnd", FLOAT);
			DefineField("m_flOldMaxDensity", FLOAT);
			DefineField("m_flOldHDRColorScale", FLOAT);
			DefineField("m_flOldFarZ", FLOAT);
			DefineField("m_NewColor", COLOR32);
			DefineField("m_flNewStart", FLOAT);
			DefineField("m_flNewEnd", FLOAT);
			DefineField("m_flNewMaxDensity", FLOAT);
			DefineField("m_flNewHDRColorScale", FLOAT);
			DefineField("m_flNewFarZ", FLOAT);
			
			BeginDataMap("CPlayerLocalData");
			DefineField("m_chAreaBits", BYTE, MAX_AREA_STATE_BYTES);
			DefineField("m_chAreaPortalBits", BYTE, MAX_AREA_STATE_BYTES);
			DefineField("m_iHideHUD", INTEGER);
			DefineField("m_flFOVRate", FLOAT);
			DefineField("m_vecOverViewpoint", VECTOR);
			DefineField("m_bDucked", BOOLEAN);
			DefineField("m_bDucking", BOOLEAN);
			DefineField("m_bInDuckJump", BOOLEAN);
			DefineField("m_flDucktime", TIME);
			DefineField("m_flDuckJumpTime", TIME);
			DefineField("m_flJumpTime", TIME);
			DefineField("m_nStepside", INTEGER);
			DefineField("m_flFallVelocity", FLOAT);
			DefineField("m_nOldButtons", INTEGER);
			DefineField("m_vecPunchAngle", VECTOR);
			DefineField("m_vecPunchAngleVel", VECTOR);
			DefineField("m_bDrawViewmodel", BOOLEAN);
			DefineField("m_bWearingSuit", BOOLEAN);
			DefineField("m_bPoisoned", BOOLEAN);
			DefineField("m_flStepSize", FLOAT);
			DefineField("m_bAllowAutoMovement", BOOLEAN);
			DefineEmbeddedField("m_skybox3d", "sky3dparams_t");
			DefineEmbeddedField("m_PlayerFog", "fogplayerparams_t");
			DefineEmbeddedField("m_fog", "fogparams_t");
			DefineEmbeddedField("m_audio", "audioparams_t");
			
			if (Game == Game.PORTAL2) {
				// CPortal_Player : PaintPowerUser<CPaintableEntity<CBaseMultiplayerPlayer>>
				BeginTemplatedMap("CPortal_Player", null, "PaintPowerUser", "CPaintableEntity<CBaseMultiplayerPlayer>");
			} else {
				BeginDataMap("CPortal_Player", "CHL2_Player");
			}
			LinkNamesToMap("player");
			DefineSoundPatch("m_pWooshSound");
			DefineField("m_bHeldObjectOnOppositeSideOfPortal", BOOLEAN);
			DefineField("m_pHeldObjectPortal", EHANDLE);
			DefineField("m_bIntersectingPortalPlane", BOOLEAN);
			DefineField("m_bStuckOnPortalCollisionObject", BOOLEAN);
			DefineField("m_fTimeLastHurt", TIME);
			if (Game != Game.PORTAL2) {
				DefineField("m_StatsThisLevel.iNumPortalsPlaced", INTEGER);
				DefineField("m_StatsThisLevel.iNumStepsTaken", INTEGER);
				DefineField("m_StatsThisLevel.fNumSecondsTaken", FLOAT);
			}
			DefineField("m_fTimeLastNumSecondsUpdate", TIME);
			DefineField("m_iNumCamerasDetatched", INTEGER);
			DefineField("m_bPitchReorientation", BOOLEAN);
			DefineField("m_bIsRegenerating", BOOLEAN);
			DefineField("m_fNeuroToxinDamageTime", TIME);
			DefineField("m_hPortalEnvironment", EHANDLE);
			DefineField("m_flExpressionLoopTime", TIME);
			DefineField("m_iszExpressionScene", STRING);
			DefineField("m_hExpressionSceneEnt", EHANDLE);
			DefineField("m_vecTotalBulletForce", VECTOR);
			DefineField("m_bSilentDropAndPickup", BOOLEAN);
			DefineField("m_hRagdoll", EHANDLE);
			DefineField("m_angEyeAngles", VECTOR);
			DefineField("m_iPlayerSoundType", INTEGER);
			DefineField("m_qPrePortalledViewAngles", VECTOR);
			DefineField("m_bFixEyeAnglesFromPortalling", BOOLEAN);
			DefineField("m_matLastPortalled", VMATRIX_WORLDSPACE);
			DefineField("m_vWorldSpaceCenterHolder", POSITION_VECTOR);
			DefineField("m_hSurroundingLiquidPortal", EHANDLE);
			DefineEmbeddedField("m_pExpresser", "CAI_Expresser"); // todo check
			if (Game == Game.PORTAL2) {
				DefineField("m_iSpawnInterpCounter", INTEGER);
				DefineField("m_flMotionBlurAmount", FLOAT);
				DefineField("m_flTimeLastTouchedGround", FLOAT);
				DefineField("m_bPotatos", BOOLEAN);
				DefineField("m_PlayerGunType", INTEGER);
				DefineField("m_flHullHeight", FLOAT);
				DefineEmbeddedField("m_PortalLocal", "CPortalPlayerLocalData");
				DefineEmbeddedField("m_StatsThisLevel", "PortalPlayerStatistics_t");
				DefineField("m_flUseKeyCooldownTime", FLOAT);
				DefineField("m_flUseKeyStartTime", FLOAT);
				DefineField("m_flAutoGrabLockOutTime", FLOAT);
			}

			BeginDataMap("LadderMove_t");
			DefineField("m_bForceLadderMove", BOOLEAN);
			DefineField("m_bForceMount", BOOLEAN);
			DefineField("m_flStartTime", FLOAT);
			DefineField("m_flArriveTime", FLOAT);
			DefineField("m_vecGoalPosition", VECTOR);
			DefineField("m_vecStartPosition", VECTOR);
			DefineField("m_hForceLadder", EHANDLE);
			DefineField("m_hReservedSpot", EHANDLE);
			
			BeginDataMap("CPortalRagdoll", "CBaseEntity");
			LinkNamesToMap("portal_ragdoll");
			DefineField("m_vecRagdollOrigin", POSITION_VECTOR);
			DefineField("m_hPlayer", EHANDLE);
			DefineField("m_vecRagdollVelocity", VECTOR);
			
			BeginDataMap("CRevertSaved", "CPointEntity");
			LinkNamesToMap("player_loadsaved");
			if (GenInfo.IsDefHl1Dll) {
				DefineKeyField("m_iszMessage", "message", STRING);
				DefineKeyField("m_messageTime", "messagetime", FLOAT);
				DefineFunction("MessageThink");
			}
			DefineKeyField("m_loadTime", "loadtime", FLOAT);
			DefineKeyField("m_Duration", "duration", FLOAT);
			DefineKeyField("m_HoldTime", "holdtime", FLOAT);
			DefineInputFunc("Reload", "InputReload", VOID);
			DefineFunction("LoadThink");
			
			BeginDataMap("CFuncMonitor", "CFuncBrush");
			LinkNamesToMap("func_monitor");
			DefineField("m_hInfoCameraLink", EHANDLE);
			DefineInputFunc("SetCamera", "InputSetCamera", STRING);
			
			BeginDataMap("CPointCamera", "CBaseEntity");
			LinkNamesToMap("point_camera");
			DefineKeyField("m_FOV", "FOV", FLOAT);
			DefineKeyField("m_Resolution", "resolution", FLOAT);
			DefineKeyField("m_bFogEnable", "fogEnable", BOOLEAN);
			DefineKeyField("m_FogColor", "fogColor", COLOR32);
			DefineKeyField("m_flFogStart", "fogStart", FLOAT);
			DefineKeyField("m_flFogEnd", "fogEnd", FLOAT);
			DefineKeyField("m_flFogMaxDensity", "fogMaxDensity", FLOAT);
			DefineKeyField("m_bUseScreenAspectRatio", "UseScreenAspectRatio", BOOLEAN);
			DefineField("m_bActive", BOOLEAN);
			DefineField("m_bIsOn", BOOLEAN);
			DefineField("m_TargetFOV", FLOAT);
			DefineField("m_DegreesPerSecond", FLOAT);
			DefineFunction("ChangeFOVThink");
			DefineInputFunc("ChangeFOV", "InputChangeFOV", STRING);
			DefineInputFunc("SetOnAndTurnOthersOff", "InputSetOnAndTurnOthersOff", VOID);
			DefineInputFunc("SetOn", "InputSetOn", VOID);
			DefineInputFunc("SetOff", "InputSetOff", VOID);
			
			BeginDataMap("CFuncWall","CBaseEntity");
			LinkNamesToMap("func_wall");
			DefineField("m_nState", INTEGER);
		}
	}
}