// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public sealed class EntMaps2 : DataMapGenerator {
		
		protected override void CreateDataMaps() {
			BeginDataMap("CSound");
			DefineField("m_hOwner", EHANDLE);
			DefineField("m_iVolume", INTEGER);
			DefineField("m_flOcclusionScale", FLOAT);
			DefineField("m_iType", INTEGER);
			DefineField("m_bNoExpirationTime", BOOLEAN);
			DefineField("m_flExpireTime", TIME);
			DefineField("m_iNext", SHORT);
			DefineField("m_ownerChannelIndex", INTEGER);
			DefineField("m_vecOrigin", POSITION_VECTOR);
			DefineField("m_bHasOwner", BOOLEAN);
			DefineField("m_hTarget", EHANDLE);
			
			BeginDataMap("CSoundEnt", "CBaseEntity");
			LinkNamesToMap("soundent");
			DefineField("m_iFreeSound", INTEGER);
			DefineField("m_iActiveSound", INTEGER);
			DefineField("m_cLastActiveSounds", INTEGER);
			DefineEmbeddedField("m_SoundPool", "CSound", Constants.MAX_WORLD_SOUNDS_SP);
			
			DataMapProxy("CServerOnlyEntity", "CBaseEntity");

			DataMapProxy("CLogicalEntity", "CServerOnlyEntity");
			
			BeginDataMap("template_t");
			DefineField("iTemplateIndex", INTEGER);
			DefineField("matEntityToTemplate", VMATRIX);
			
			BeginDataMap("CPointTemplate", "CLogicalEntity");
			LinkNamesToMap("point_template");
			DefineKeyField("m_iszTemplateEntityNames[0]", "Template01", STRING);
			DefineKeyField("m_iszTemplateEntityNames[1]", "Template02", STRING);
			DefineKeyField("m_iszTemplateEntityNames[2]", "Template03", STRING);
			DefineKeyField("m_iszTemplateEntityNames[3]", "Template04", STRING);
			DefineKeyField("m_iszTemplateEntityNames[4]", "Template05", STRING);
			DefineKeyField("m_iszTemplateEntityNames[5]", "Template06", STRING);
			DefineKeyField("m_iszTemplateEntityNames[6]", "Template07", STRING);
			DefineKeyField("m_iszTemplateEntityNames[7]", "Template08", STRING);
			DefineKeyField("m_iszTemplateEntityNames[8]", "Template09", STRING);
			DefineKeyField("m_iszTemplateEntityNames[9]", "Template10", STRING);
			DefineKeyField("m_iszTemplateEntityNames[10]", "Template11", STRING);
			DefineKeyField("m_iszTemplateEntityNames[11]", "Template12", STRING);
			DefineKeyField("m_iszTemplateEntityNames[12]", "Template13", STRING);
			DefineKeyField("m_iszTemplateEntityNames[13]", "Template14", STRING);
			DefineKeyField("m_iszTemplateEntityNames[14]", "Template15", STRING);
			DefineKeyField("m_iszTemplateEntityNames[15]", "Template16", STRING);
			DefineVector("m_hTemplateEntities", CLASSPTR);
			DefineEmbeddedVector("m_hTemplates", "template_t");
			//DefineINPUTFUNC("ForceSpawn", VOID);
			DefineOutput("m_pOutputOnSpawned", "OnEntitySpawned");
			
			DataMapProxy("CPointEntity", "CBaseEntity");
			LinkNamesToMap("info_landmark", "info_player_start", "info_teleport_destination", "info_target_helicopter_crash");
			
			BeginDataMap("CRopeKeyframe", "CBaseEntity");
			LinkNamesToMap("move_rope", "keyframe_rope");
			DefineField("m_RopeFlags", INTEGER);
			DefineKeyField("m_iNextLinkName", "NextKey", STRING);
			DefineKeyField("m_Slack", "Slack", INTEGER);
			DefineKeyField("m_Width", "Width", FLOAT);
			DefineKeyField("m_TextureScale", "TextureScale", FLOAT);
			DefineField("m_nSegments", INTEGER);
			DefineField("m_bConstrainBetweenEndpoints", BOOLEAN);
			DefineField("m_strRopeMaterialModel", STRING);
			DefineField("m_iRopeMaterialModelIndex", MODELINDEX);
			DefineKeyField("m_Subdiv", "Subdiv", INTEGER);
			DefineField("m_RopeLength", INTEGER);
			DefineField("m_fLockedPoints", INTEGER);
			DefineField("m_bCreatedFromMapFile", BOOLEAN);
			DefineKeyField("m_flScrollSpeed", "ScrollSpeed", FLOAT);
			DefineField("m_bStartPointValid", BOOLEAN);
			DefineField("m_bEndPointValid", BOOLEAN);
			DefineField("m_hStartPoint", EHANDLE);
			DefineField("m_hEndPoint", EHANDLE);
			DefineField("m_iStartAttachment", SHORT);
			DefineField("m_iEndAttachment", SHORT);
			//DefineINPUTFUNC("SetScrollSpeed", FLOAT);
			//DefineINPUTFUNC("SetForce", VECTOR);
			//DefineINPUTFUNC("Break", VOID);
			
			BeginDataMap("CFuncBrush", "CBaseEntity");
			LinkNamesToMap("func_brush");
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("Disable", VOID);
			//DefineINPUTFUNC("Toggle", VOID);
			DefineKeyField("m_iDisabled", "StartDisabled", INTEGER);
			DefineKeyField("m_iSolidity", "Solidity", INTEGER);
			DefineKeyField("m_bSolidBsp", "solidbsp", BOOLEAN);
			DefineKeyField("m_iszExcludedClass", "excludednpc", STRING);
			DefineKeyField("m_bInvertExclusion", "invert_exclusion", BOOLEAN);
			//DefineINPUTFUNC("SetExcluded", STRING);
			//DefineINPUTFUNC("SetInvert", BOOLEAN);
			
			DataMapProxy("CBaseProp", "CBaseAnimating");
			
			BeginDataMap("physfollower_t");
			DefineField("boneIndex", INTEGER);
			DefineField("hFollower", EHANDLE);
			
			BeginDataMap("CBoneFollowerManager");
			DefineGlobalField("m_iNumBones", INTEGER);
			DefineGlobalEmbeddedVector("m_physBones", "physfollower_t");
			
			BeginDataMap("CDynamicProp", "CBreakableProp");
			LinkNamesToMap("dynamic_prop", "prop_dynamic", "prop_dynamic_override");
			DefineKeyField("m_iszDefaultAnim", "DefaultAnim", STRING);
			DefineField("m_iGoalSequence", INTEGER);
			DefineField("m_iTransitionDirection", INTEGER);
			DefineKeyField("m_bRandomAnimator", "RandomAnimation", BOOLEAN);
			DefineField("m_flNextRandAnim", TIME);
			DefineKeyField("m_flMinRandAnimTime", "MinAnimTime", FLOAT);
			DefineKeyField("m_flMaxRandAnimTime", "MaxAnimTime", FLOAT);
			DefineKeyField("m_bStartDisabled", "StartDisabled", BOOLEAN);
			DefineField("m_bUseHitboxesForRenderBox", BOOLEAN);
			DefineField("m_nPendingSequence", SHORT);
			//DefineINPUTFUNC("SetAnimation", STRING);
			//DefineINPUTFUNC("SetDefaultAnimation", STRING);
			//DefineINPUTFUNC("TurnOn", VOID);
			//DefineINPUTFUNC("TurnOff", VOID);
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("Disable", VOID);
			//DefineINPUTFUNC("EnableCollision", VOID);
			//DefineINPUTFUNC("DisableCollision", VOID);
			//DefineINPUTFUNC("SetPlaybackRate", FLOAT);
			DefineOutput("m_pOutputAnimBegun", "OnAnimationBegun");
			DefineOutput("m_pOutputAnimOver", "OnAnimationDone");
			//DEFINE_THINKFUNC( AnimThink ),
			DefineEmbeddedField("m_BoneFollowerManager", "CBoneFollowerManager");
			
			BeginDataMap("CBaseToggle", "CBaseEntity");
			DefineField("m_toggle_state", INTEGER);
			DefineField("m_flMoveDistance", FLOAT);
			DefineField("m_flWait", FLOAT);
			DefineField("m_flLip", FLOAT);
			DefineField("m_vecPosition1", POSITION_VECTOR);
			DefineField("m_vecPosition2", POSITION_VECTOR);
			DefineField("m_vecMoveAng", VECTOR);
			DefineField("m_vecAngle1", VECTOR);
			DefineField("m_vecAngle2", VECTOR);
			DefineField("m_flHeight", FLOAT);
			DefineField("m_hActivator", EHANDLE);
			DefineField("m_vecFinalDest", POSITION_VECTOR);
			DefineField("m_vecFinalAngle", VECTOR);
			DefineField("m_sMaster", STRING);
			DefineField("m_movementType", INTEGER);
			
			BeginDataMap("CBaseDoor", "CBaseToggle");
			LinkNamesToMap("func_door", "func_water");
			DefineKeyField("m_vecMoveDir", "movedir", VECTOR);
			DefineField("m_bLockedSentence", CHARACTER);
			DefineField("m_bUnlockedSentence", CHARACTER);
			DefineKeyField("m_NoiseMoving", "noise1", SOUNDNAME);
			DefineKeyField("m_NoiseArrived", "noise2", SOUNDNAME);
			DefineKeyField("m_NoiseMovingClosed", "startclosesound", SOUNDNAME);
			DefineKeyField("m_NoiseArrivedClosed", "closesound", SOUNDNAME);
			DefineKeyField("m_ChainTarget", "chainstodoor", STRING);
			DefineKeyField("m_ls.sLockedSound", "locked_sound", SOUNDNAME);
			DefineKeyField("m_ls.sUnlockedSound", "unlocked_sound", SOUNDNAME);
			DefineField("m_bLocked", BOOLEAN);
			DefineKeyField("m_flWaveHeight", "WaveHeight", FLOAT);
			DefineKeyField("m_flBlockDamage", "dmg", FLOAT);
			DefineKeyField("m_eSpawnPosition", "spawnpos", INTEGER);
			DefineKeyField("m_bForceClosed", "forceclosed", BOOLEAN);
			DefineField("m_bDoorGroup", BOOLEAN);
			DefineKeyField("m_bLoopMoveSound", "loopmovesound", BOOLEAN);
			DefineKeyField("m_bIgnoreDebris", "ignoredebris", BOOLEAN);
			//DefineINPUTFUNC("Open", VOID);
			//DefineINPUTFUNC("Close", VOID);
			//DefineINPUTFUNC("Toggle", VOID);
			//DefineINPUTFUNC("Lock", VOID);
			//DefineINPUTFUNC("Unlock", VOID);
			//DefineINPUTFUNC("SetSpeed", FLOAT);
			//DefineINPUTFUNC("SetToggleState", FLOAT);
			DefineOutput("m_OnBlockedOpening", "OnBlockedOpening");
			DefineOutput("m_OnBlockedClosing", "OnBlockedClosing");
			DefineOutput("m_OnUnblockedOpening", "OnUnblockedOpening");
			DefineOutput("m_OnUnblockedClosing", "OnUnblockedClosing");
			DefineOutput("m_OnFullyClosed", "OnFullyClosed");
			DefineOutput("m_OnFullyOpen", "OnFullyOpen");
			DefineOutput("m_OnClose", "OnClose");
			DefineOutput("m_OnOpen", "OnOpen");
			DefineOutput("m_OnLockedUse", "OnLockedUse");
			DefineFunction("DoorTouch");
			DefineFunction("DoorGoUp");
			DefineFunction("DoorGoDown");
			DefineFunction("DoorHitTop");
			DefineFunction("DoorHitBottom");
			//DEFINE_THINKFUNC( MovingSoundThink ),
			//DEFINE_THINKFUNC( CloseAreaPortalsThink ),
			
			BeginDataMap("CLight", "CPointEntity");
			LinkNamesToMap("light");
			DefineField("m_iCurrentFade", CHARACTER);
			DefineField("m_iTargetFade", CHARACTER);
			DefineKeyField("m_iStyle", "style", INTEGER);
			DefineKeyField("m_iDefaultStyle", "defaultstyle", INTEGER);
			DefineKeyField("m_iszPattern", "pattern", STRING);
			DefineFunction("FadeThink");
			//DefineINPUTFUNC("SetPattern", STRING);
			//DefineINPUTFUNC("FadeToPattern", STRING);
			//DefineINPUTFUNC("Toggle", VOID);
			//DefineINPUTFUNC("TurnOn", VOID);
			//DefineINPUTFUNC("TurnOff", VOID);
			
			BeginDataMap("CMaterialModifyControl", "CBaseEntity");
			LinkNamesToMap("material_modify_control");
			DefineField("m_szMaterialName", CHARACTER, Constants.MATERIAL_MODIFY_STRING_SIZE);
			DefineField("m_szMaterialVar", CHARACTER, Constants.MATERIAL_MODIFY_STRING_SIZE);
			DefineField("m_szMaterialVarValue", CHARACTER, Constants.MATERIAL_MODIFY_STRING_SIZE);
			DefineField("m_iFrameStart", INTEGER);
			DefineField("m_iFrameEnd", INTEGER);
			DefineField("m_bWrap", BOOLEAN);
			DefineField("m_flFramerate", FLOAT);
			DefineField("m_bNewAnimCommandsSemaphore", BOOLEAN);
			DefineField("m_flFloatLerpStartValue", FLOAT);
			DefineField("m_flFloatLerpEndValue", FLOAT);
			DefineField("m_flFloatLerpTransitionTime", FLOAT);
			DefineField("m_nModifyMode", INTEGER);
			//DefineINPUTFUNC("SetMaterialVar", STRING);
			//DefineINPUTFUNC("SetMaterialVarToCurrentTime", VOID);
			//DefineINPUTFUNC("StartAnimSequence", STRING);
			//DefineINPUTFUNC("StartFloatLerp", STRING);
			
			BeginDataMap("CAmbientGeneric", "CPointEntity"); // a sound emitter used for one-shot and looping sounds.
			LinkNamesToMap("ambient_generic");
			DefineKeyField("m_iszSound", "message", SOUNDNAME);
			DefineKeyField("m_radius", "radius", FLOAT);
			DefineKeyField("m_sSourceEntName", "SourceEntityName", STRING);
			// DEFINE_FIELD( m_hSoundSource, EHANDLE ),
			// DEFINE_FIELD( m_nSoundSourceEntIndex, FIELD_INTERGER ),
			DefineField("m_flMaxRadius", FLOAT);
			DefineField("m_fActive", BOOLEAN);
			DefineField("m_fLooping", BOOLEAN);
			DefineField("m_iSoundLevel", INTEGER);
			DefineField("m_dpv", BYTE, 100); // this is actually a struct of dynpitchvol_t?
			DefineFunction("RampThink");
			//DefineINPUTFUNC("PlaySound", VOID);
			//DefineINPUTFUNC("StopSound", VOID);
			//DefineINPUTFUNC("ToggleSound", VOID);
			//DefineINPUTFUNC("Pitch", FLOAT);
			//DefineINPUTFUNC("Volume", FLOAT);
			//DefineINPUTFUNC("FadeIn", FLOAT);
			//DefineINPUTFUNC("FadeOut", FLOAT);
			
			BeginDataMap("CFuncAreaPortalBase", "CBaseEntity");
			DefineField("m_portalNumber", INTEGER);
			DefineKeyField("m_iPortalVersion", "PortalVersion", INTEGER);
			
			BeginDataMap("CAreaPortal", "CFuncAreaPortalBase");
			LinkNamesToMap("func_areaportal");
			DefineKeyField("m_portalNumber", "portalnumber", INTEGER);
			DefineField("m_state", INTEGER);
			//DefineINPUTFUNC("Open", VOID);
			//DefineINPUTFUNC("Close", VOID);
			//DefineINPUTFUNC("Toggle", VOID);
			//DefineINPUTFUNC("TurnOn", VOID);
			//DefineINPUTFUNC("TurnOff", VOID);
			
			BeginDataMap("CPhysicsShake");
			DefineField("m_force", VECTOR);
			
			BeginDataMap("vehicle_gear_t");
			DefineField("flMinSpeed", FLOAT);
			DefineField("flMaxSpeed", FLOAT);
			DefineField("flSpeedApproachFactor", FLOAT);
			
			BeginDataMap("vehicle_crashsound_t");
			DefineField("flMinSpeed", FLOAT);
			DefineField("flMinDeltaSpeed", FLOAT);
			DefineField("iszCrashSound", STRING);
			DefineField("gearLimit", INTEGER);
			
			BeginDataMap("vehiclesounds_t");
			DefineField("iszSound", STRING, Constants.VS_NUM_SOUNDS);
			DefineEmbeddedVector("pGears", "vehicle_gear_t");
			DefineEmbeddedVector("crashSounds", "vehicle_crashsound_t");
			DefineField("iszStateSounds", STRING, Constants.SS_NUM_STATES);
			DefineField("minStateTime", FLOAT, Constants.SS_NUM_STATES);
			
			BeginDataMap("CPassengerInfo");
			DefineField("m_hPassenger", EHANDLE);
			DefineField("m_strRoleName", STRING);
			DefineField("m_strSeatName", STRING);
			
			BeginDataMap("CBaseServerVehicle");
			DefineField("m_nNPCButtons", INTEGER);
			DefineField("m_nPrevNPCButtons", INTEGER);
			DefineField("m_flTurnDegrees", FLOAT);
			DefineField("m_flVehicleVolume", FLOAT);
			DefineEmbeddedField("m_vehicleSounds", "vehiclesounds_t");
			DefineField("m_iSoundGear", INTEGER);
			DefineField("m_flSpeedPercentage", FLOAT);
			DefineSoundPatch("m_pStateSound");
			DefineSoundPatch("m_pStateSoundFade");
			DefineField("m_soundState", INTEGER);
			DefineField("m_soundStateStartTime", TIME);
			DefineField("m_lastSpeed", FLOAT);
			DefineField("m_iCurrentExitAnim", INTEGER);
			DefineField("m_vecCurrentExitEndPoint", POSITION_VECTOR);
			DefineField("m_chPreviousTextureType", CHARACTER);
			DefineField("m_savedViewOffset", VECTOR);
			DefineField("m_hExitBlocker", EHANDLE);
			DefineEmbeddedVector("m_PassengerInfo", "CPassengerInfo");
			
			DataMapProxy("CChoreoGenericServerVehicle", "CBaseServerVehicle");
			
			BeginDataMap("vehicleview_t");
			DefineField("bClampEyeAngles", BOOLEAN);
			DefineField("flPitchCurveZero", FLOAT);
			DefineField("flPitchCurveLinear", FLOAT);
			DefineField("flRollCurveZero", FLOAT);
			DefineField("flRollCurveLinear", FLOAT);
			DefineField("flFOV", FLOAT);
			DefineField("flYawMin", FLOAT);
			DefineField("flYawMax", FLOAT);
			DefineField("flPitchMin", FLOAT);
			DefineField("flPitchMax", FLOAT);
			
			BeginDataMap("CPropVehicleChoreoGeneric", "CDynamicProp");
			LinkNamesToMap("prop_vehicle_choreo_generic");
			DefineInputFunc("Lock", "InputLock", VOID);
			DefineInputFunc("Unlock", "InputUnlock", VOID);
			DefineInputFunc("EnterVehicle", "InputEnterVehicle", VOID);
			DefineInputFunc("EnterVehicleImmediate", "InputEnterVehicleImmediate", VOID);
			DefineInputFunc("ExitVehicle", "InputExitVehicle", VOID);
			DefineInputFunc("Open", "InputOpen", VOID);
			DefineInputFunc("Close", "InputClose", VOID);
			DefineInputFunc("Viewlock", "InputViewlock", BOOLEAN);
			DefineEmbeddedField("m_ServerVehicle", "CChoreoGenericServerVehicle");
			DefineField("m_hPlayer", EHANDLE);
			DefineField("m_bEnterAnimOn", BOOLEAN);
			DefineField("m_bExitAnimOn", BOOLEAN);
			DefineField("m_bForcedExit", BOOLEAN);
			DefineField("m_vecEyeExitEndpoint", POSITION_VECTOR);
			DefineKeyField("m_vehicleScript", "vehiclescript", STRING);
			DefineKeyField("m_bLocked", "vehiclelocked", BOOLEAN);
			DefineKeyField("m_bIgnoreMoveParent", "ignoremoveparent", BOOLEAN);
			DefineKeyField("m_bIgnorePlayerCollisions", "ignoreplayer", BOOLEAN);
			DefineKeyField("m_bForcePlayerEyePoint", "useplayereyes", BOOLEAN);
			DefineOutput("m_playerOn", "PlayerOn");
			DefineOutput("m_playerOff", "PlayerOff");
			DefineOutput("m_OnOpen", "OnOpen");
			DefineOutput("m_OnClose", "OnClose");
			DefineEmbeddedField("m_vehicleView", "vehicleview_t");
			DefineEmbeddedField("m_savedVehicleView", "vehicleview_t");
		}
	}
}