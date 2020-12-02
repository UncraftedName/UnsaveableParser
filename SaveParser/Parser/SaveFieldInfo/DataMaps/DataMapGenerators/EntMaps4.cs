// ReSharper disable All

using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public class EntMaps4 : DataMapGenerator {
		
		public const int MAX_BEAM_ENTS = 10;
		
		
		protected override void CreateDataMaps() {
			DataMapProxy("CInfoTarget", "CPointEntity");
			LinkNamesToMap("info_target");
			
			BeginDataMap("CFuncCombineBallSpawner", "CBaseEntity");
			DefineKeyField("m_nBallCount", "ballcount", INTEGER);
			DefineKeyField("m_flMinSpeed", "minspeed", FLOAT);
			DefineKeyField("m_flMaxSpeed", "maxspeed", FLOAT);
			DefineKeyField("m_flBallRadius", "ballradius", FLOAT);
			DefineKeyField("m_flBallRespawnTime", "ballrespawntime", FLOAT);
			DefineField("m_flRadius", FLOAT);
			DefineField("m_nBallsRemainingInField", INTEGER);
			DefineField("m_bEnabled", BOOLEAN);
			DefineVector("m_BallRespawnTime", TIME);
			DefineField("m_flDisableTime", TIME);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineOutput("m_OnBallGrabbed", "OnBallGrabbed");
			DefineOutput("m_OnBallReinserted", "OnBallReinserted");
			DefineOutput("m_OnBallHitTopSide", "OnBallHitTopSide");
			DefineOutput("m_OnBallHitBottomSide", "OnBallHitBottomSide");
			DefineOutput("m_OnLastBallGrabbed", "OnLastBallGrabbed");
			DefineOutput("m_OnFirstBallReinserted", "OnFirstBallReinserted");
			DefineThinkFunc("BallThink");
			DefineEntityFunc("GrabBallTouch");
			
			BeginDataMap("CPointCombineBallLauncher", "CFuncCombineBallSpawner");
			DefineKeyField("m_flConeDegrees", "launchconenoise", FLOAT);
			DefineKeyField("m_iszBullseyeName", "bullseyename", STRING);
			DefineKeyField("m_iBounces", "maxballbounces", INTEGER);
			DefineInputFunc("LaunchBall", "InputLaunchBall", VOID);
			
			BeginDataMap("CEnergyBallLauncher", "CPointCombineBallLauncher");
			LinkNamesToMap("point_energy_ball_launcher");
			DefineKeyField("m_fBallLifetime", "BallLifetime", FLOAT);
			DefineKeyField("m_fMinBallLifeAfterPortal", "MinLifeAfterPortal", FLOAT);
			DefineOutput("m_OnPostSpawnBall", "OnPostSpawnBall");
			
			BeginDataMap("CPointSpotlight", "CPointEntity");
			LinkNamesToMap("point_spotlight");
			DefineField("m_flSpotlightCurLength", FLOAT);
			DefineField("m_bSpotlightOn", BOOLEAN);
			DefineField("m_bEfficientSpotlight", BOOLEAN);
			DefineField("m_vSpotlightTargetPos", POSITION_VECTOR);
			DefineField("m_vSpotlightCurrentPos", POSITION_VECTOR);
			DefineField("m_vSpotlightDir", VECTOR);
			DefineField("m_nHaloSprite", INTEGER);
			DefineKeyField("m_flSpotlightMaxLength", "SpotlightLength", FLOAT);
			DefineKeyField("m_flSpotlightGoalWidth", "SpotlightWidth", FLOAT);
			DefineKeyField("m_flHDRColorScale", "HDRColorScale", FLOAT);
			DefineKeyField("m_nMinDXLevel", "mindxlevel", INTEGER);
			DefineInputFunc("LightOn", "InputLightOn", VOID);
			DefineInputFunc("LightOff", "InputLightOff", VOID);
			DefineOutput("m_OnOn", "OnLightOn");
			DefineOutput("m_OnOff", "OnLightOff");
			DefineThinkFunc("SpotlightThink");

			BeginDataMap("CFuncPortalDetector", "CBaseEntity");
			LinkNamesToMap("func_portal_detector");
			DefineField("m_bActive", BOOLEAN);
			DefineKeyField("m_iLinkageGroupID", "LinkageGroupID", INTEGER);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineOutput("m_OnStartTouchPortal1", "OnStartTouchPortal1");
			DefineOutput("m_OnStartTouchPortal2", "OnStartTouchPortal2");
			DefineOutput("m_OnStartTouchLinkedPortal", "OnStartTouchLinkedPortal");
			DefineOutput("m_OnStartTouchBothLinkedPortals", "OnStartTouchBothLinkedPortals");
			DefineFunction("IsActive");
			if (Game == Game.PORTAL2) {
				DefineOutput("m_OnStartTouchPortal", "OnStartTouchPortal");
				DefineOutput("m_OnEndTouchPortal", "OnEndTouchPortal");
			}

			BeginDataMap("CBeam", "CBaseEntity");
			DefineField("m_nHaloIndex", MODELINDEX);
			DefineField("m_nBeamType", INTEGER);
			DefineField("m_nBeamFlags", INTEGER);
			DefineField("m_nNumBeamEnts", INTEGER);
			DefineField("m_hAttachEntity", EHANDLE, MAX_BEAM_ENTS);
			DefineField("m_nAttachIndex", INTEGER, MAX_BEAM_ENTS);
			DefineField("m_nMinDXLevel", INTEGER);
			DefineField("m_fWidth", FLOAT);
			DefineField("m_fEndWidth", FLOAT);
			DefineField("m_fFadeLength", FLOAT);
			DefineField("m_fHaloScale", FLOAT);
			DefineField("m_fAmplitude", FLOAT);
			DefineField("m_fStartFrame", FLOAT);
			DefineField("m_flFrameRate", FLOAT);
			DefineField("m_flFrame", FLOAT);
			DefineKeyField("m_flHDRColorScale", "HDRColorScale", FLOAT);
			DefineKeyField("m_flDamage", "damage", FLOAT);
			DefineField("m_flFireTime", TIME);
			DefineField("m_vecEndPos", POSITION_VECTOR);
			DefineField("m_hEndEntity", EHANDLE);
			DefineKeyField("m_nDissolveType", "dissolvetype", INTEGER);
			if (GenInfo.IsDefPortal) {
				DefineField("m_bDrawInMainRender", BOOLEAN);
				DefineField("m_bDrawInPortalRender", BOOLEAN);
			}
			DefineInputFunc("Width", "InputWidth", FLOAT);
			DefineInputFunc("Noise", "InputNoise", FLOAT);
			DefineInputFunc("ColorRedValue", "InputColorRedValue", FLOAT);
			DefineInputFunc("ColorGreenValue", "InputColorGreenValue", FLOAT);
			DefineInputFunc("ColorBlueValue", "InputColorBlueValue", FLOAT);
			DefineInput("m_fSpeed", "ScrollSpeed", FLOAT);
			
			BeginDataMap("CFilterCombineBall", "CBaseFilter");
			LinkNamesToMap("filter_combineball_type");
			DefineKeyField("m_iBallType", "balltype", INTEGER);
			
			BeginDataMap("CPropPortalStatsDisplay", "CBaseAnimating");
			LinkNamesToMap("prop_portal_stats_display");
			DefineField("m_bEnabled", BOOLEAN);
			DefineField("m_iNumPortalsPlaced", INTEGER);
			DefineField("m_iNumStepsTaken", INTEGER);
			DefineField("m_fNumSecondsTaken", FLOAT);
			DefineField("m_iBronzeObjective", INTEGER);
			DefineField("m_iSilverObjective", INTEGER);
			DefineField("m_iGoldObjective", INTEGER);
			DefineField("szChallengeFileName", CHARACTER, 128);
			DefineField("szChallengeMapName", CHARACTER, 32);
			DefineField("szChallengeName", CHARACTER, 32);
			DefineField("m_iDisplayObjective", INTEGER);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("UpdateStats", "InputUpdateStats", VOID);
			DefineInputFunc("ResetPlayerStats", "InputResetPlayerStats", VOID);
			DefineOutput("m_OnMetBronzeObjective", "OnMetBronzeObjective");
			DefineOutput("m_OnMetSilverObjective", "OnMetSilverObjective");
			DefineOutput("m_OnMetGoldObjective", "OnMetGoldObjective");
			DefineOutput("m_OnFailedAllObjectives", "OnFailedAllObjectives");
			
			BeginDataMap("EntityParticleTrailInfo_t");
			DefineKeyField("m_strMaterialName", "ParticleTrailMaterial", STRING);
			DefineKeyField("m_flLifetime", "ParticleTrailLifetime", FLOAT);
			DefineKeyField("m_flStartSize", "ParticleTrailStartSize", FLOAT);
			DefineKeyField("m_flEndSize", "ParticleTrailEndSize", FLOAT);
			
			BeginDataMap("CPointClientCommand", "CPointEntity");
			LinkNamesToMap("point_clientcommand");
			DefineInputFunc("Command", "InputCommand", STRING);
			
			BeginDataMap("CPointBonusMapsAccessor", "CPointEntity");
			LinkNamesToMap("point_bonusmaps_accessor");
			DefineKeyField("m_String_tFileName", "filename", STRING);
			DefineKeyField("m_String_tMapName", "mapname", STRING);
			DefineInputFunc("Unlock", "InputUnlock", VOID);
			DefineInputFunc("Complete", "InputComplete", VOID);
			DefineInputFunc("Save", "InputSave", VOID);
			
			BeginDataMap("CPropCombineBall", "CBaseAnimating");
			LinkNamesToMap("prop_combine_ball");
			DefineField("m_flLastBounceTime", TIME);
			DefineField("m_flRadius", FLOAT);
			DefineField("m_nState", BYTE);
			DefineField("m_pGlowTrail", CLASSPTR);
			DefineSoundPatch("m_pHoldingSound");
			DefineField("m_bFiredGrabbedOutput", BOOLEAN);
			DefineField("m_bEmit", BOOLEAN);
			DefineField("m_bHeld", BOOLEAN);
			DefineField("m_bLaunched", BOOLEAN);
			DefineField("m_bStruckEntity", BOOLEAN);
			DefineField("m_bWeaponLaunched", BOOLEAN);
			DefineField("m_bForward", BOOLEAN);
			DefineField("m_flSpeed", FLOAT);
			DefineField("m_flNextDamageTime", TIME);
			DefineField("m_flLastCaptureTime", TIME);
			DefineField("m_bCaptureInProgress", BOOLEAN);
			DefineField("m_nBounceCount", INTEGER);
			DefineField("m_nMaxBounces", INTEGER);
			DefineField("m_bBounceDie", BOOLEAN);
			DefineField("m_hSpawner", EHANDLE);
			DefineThinkFunc("ExplodeThink");
			DefineThinkFunc("WhizSoundThink");
			DefineThinkFunc("DieThink");
			DefineThinkFunc("DissolveThink");
			DefineThinkFunc("DissolveRampSoundThink");
			DefineThinkFunc("AnimThink");
			DefineThinkFunc("CaptureBySpawner");
			DefineInputFunc("Explode", "InputExplode", VOID);
			DefineInputFunc("FadeAndRespawn", "InputFadeAndRespawn", VOID);
			DefineInputFunc("Kill", "InputKill", VOID);
			DefineInputFunc("Socketed", "InputSocketed", VOID);
			
			BeginDataMap("CPropEnergyBall", "CPropCombineBall");
			LinkNamesToMap("prop_energy_ball");
			DefineField("m_hTouchedPortal", EHANDLE);
			DefineField("m_bTouchingPortal1", BOOLEAN);
			DefineField("m_bTouchingPortal2", BOOLEAN);
			DefineField("m_vLastKnownDirection", VECTOR);
			DefineField("m_fMinLifeAfterPortal", FLOAT);
			DefineField("m_bIsInfiniteLife", BOOLEAN);
			DefineField("m_fTimeTillDeath", FLOAT);
			DefineSoundPatch("m_pAmbientSound");
			DefineThinkFunc("Think");
			
			BeginDataMap("CFuncPortalOrientation", "CBaseEntity");
			LinkNamesToMap("func_portal_orientation");
			DefineField("m_iListIndex", INTEGER);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineKeyField("m_bMatchLinkedAngles", "MatchLinkedAngles", BOOLEAN);
			DefineKeyField("m_vecAnglesToFace", "AnglesToFace", VECTOR);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
		}
	}
}