// ReSharper disable All
using SaveParser.Utils.BitStreams;
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
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("Disable", VOID);
			DefineOutput("m_OnBallGrabbed", "OnBallGrabbed");
			DefineOutput("m_OnBallReinserted", "OnBallReinserted");
			DefineOutput("m_OnBallHitTopSide", "OnBallHitTopSide");
			DefineOutput("m_OnBallHitBottomSide", "OnBallHitBottomSide");
			DefineOutput("m_OnLastBallGrabbed", "OnLastBallGrabbed");
			DefineOutput("m_OnFirstBallReinserted", "OnFirstBallReinserted");
			//DEFINE_THINKFUNC( BallThink ),
			//DEFINE_ENTITYFUNC( GrabBallTouch ),
			
			BeginDataMap("CPointCombineBallLauncher", "CFuncCombineBallSpawner");
			DefineKeyField("m_flConeDegrees", "launchconenoise", FLOAT);
			DefineKeyField("m_iszBullseyeName", "bullseyename", STRING);
			DefineKeyField("m_iBounces", "maxballbounces", INTEGER);
			//DefineINPUTFUNC("LaunchBall", VOID);
			
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
			//DefineINPUTFUNC("LightOn", VOID);
			//DefineINPUTFUNC("LightOff", VOID);
			DefineOutput("m_OnOn", "OnLightOn");
			DefineOutput("m_OnOff", "OnLightOff");
			//DEFINE_THINKFUNC( SpotlightThink ),

			BeginDataMap("CFuncPortalDetector", "CBaseEntity");
			LinkNamesToMap("func_portal_detector");
			DefineField("m_bActive", BOOLEAN);
			DefineKeyField("m_iLinkageGroupID", "LinkageGroupID", INTEGER);
			//DefineINPUTFUNC("Disable", VOID);
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("Toggle", VOID);
			DefineOutput("m_OnStartTouchPortal1", "OnStartTouchPortal1");
			DefineOutput("m_OnStartTouchPortal2", "OnStartTouchPortal2");
			DefineOutput("m_OnStartTouchLinkedPortal", "OnStartTouchLinkedPortal");
			DefineOutput("m_OnStartTouchBothLinkedPortals", "OnStartTouchBothLinkedPortals");
			DefineFunction("IsActive");
			
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
			DefineField("m_bDrawInMainRender", BOOLEAN);
			DefineField("m_bDrawInPortalRender", BOOLEAN);
			//DefineINPUTFUNC("Width", FLOAT);
			//DefineINPUTFUNC("Noise", FLOAT);
			//DefineINPUTFUNC("ColorRedValue", FLOAT);
			//DefineINPUTFUNC("ColorGreenValue", FLOAT);
			//DefineINPUTFUNC("ColorBlueValue", FLOAT);
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
			//DefineINPUTFUNC("Disable", VOID);
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("UpdateStats", VOID);
			//DefineINPUTFUNC("ResetPlayerStats", VOID);
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
			//DEFINE_INPUTFUNC( FIELD_STRING, "Command", InputCommand ),
			
			BeginDataMap("CPointBonusMapsAccessor", "CPointEntity");
			LinkNamesToMap("point_bonusmaps_accessor");
			DefineKeyField("m_String_tFileName", "filename", STRING);
			DefineKeyField("m_String_tMapName", "mapname", STRING);
			//DefineINPUTFUNC("Unlock", VOID);
			//DefineINPUTFUNC("Complete", VOID);
			//DefineINPUTFUNC("Save", VOID);
			
			BeginDataMap("CVguiScreen", "CBaseEntity");
			LinkNamesToMap("vgui_screen", "vgui_screen_team", "CVGuiScreen"); // not sure if the last one is a typo
			DefineCustomField("m_nPanelName", VGuiScreenStringOps);
			DefineField("m_nAttachmentIndex", INTEGER);
			DefineField("m_fScreenFlags", INTEGER);
			DefineKeyField("m_flWidth", "width", FLOAT);
			DefineKeyField("m_flHeight", "height", FLOAT);
			DefineKeyField("m_strOverlayMaterial", "overlaymaterial", STRING);
			DefineField("m_hPlayerOwner", EHANDLE);
			//DefineINPUTFUNC("SetActive", VOID);
			//DefineINPUTFUNC("SetInactive", VOID);
			
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
			//DEFINE_THINKFUNC( ExplodeThink ),
			//DEFINE_THINKFUNC( WhizSoundThink ),
			//DEFINE_THINKFUNC( DieThink ),
			//DEFINE_THINKFUNC( DissolveThink ),
			//DEFINE_THINKFUNC( DissolveRampSoundThink ),
			//DEFINE_THINKFUNC( AnimThink ),
			//DEFINE_THINKFUNC( CaptureBySpawner ),
			//DefineINPUTFUNC("Explode", VOID);
			//DefineINPUTFUNC("FadeAndRespawn", VOID);
			//DefineINPUTFUNC("Kill", VOID);
			//DefineINPUTFUNC("Socketed", VOID);
			
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
		
		
		private static ParsedSaveField VGuiScreenStringOps(TypeDesc desc, SaveInfo info, ref BitStreamReader bsr)
			=> new ParsedSaveField<string>(bsr.ReadStringOfLength(bsr.ReadSInt()), desc);
	}
}