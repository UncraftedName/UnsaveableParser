// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public sealed class EnvMaps : DataMapGenerator {

		public const int MAX_WAKE_POINTS = 16;
		public const int MAX_PATH = 260;
		
		
		protected override void CreateDataMaps() {
			BeginDataMap("CSteamJet", "CBaseParticleEntity");
			LinkNamesToMap("env_steam", "env_steamjet");
			DefineKeyField("m_StartSize", "StartSize", FLOAT);
			DefineKeyField("m_EndSize", "EndSize", FLOAT);
			DefineKeyField("m_InitialState", "InitialState", BOOLEAN);
			DefineKeyField("m_nType", "Type", INTEGER);
			DefineKeyField("m_flRollSpeed", "RollSpeed", FLOAT);
			DefineField("m_bEmit", INTEGER);
			DefineField("m_bFaceLeft", BOOLEAN);
			DefineInput("m_JetLength", "JetLength", FLOAT);
			DefineInput("m_SpreadSpeed", "SpreadSpeed", FLOAT);
			DefineInput("m_Speed", "Speed", FLOAT);
			DefineInput("m_Rate", "Rate", FLOAT);
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			
			BeginDataMap("CEnvSpark", "CPointEntity");
			LinkNamesToMap("env_spark");
			DefineKeyField("m_flDelay", "MaxDelay", FLOAT);
			DefineField("m_nGlowSpriteIndex", INTEGER);
			DefineKeyField("m_nMagnitude", "Magnitude", INTEGER);
			DefineKeyField("m_nTrailLength", "TrailLength", INTEGER);
			DefineFunction("SparkThink");
			DefineInputFunc("StartSpark", "InputStartSpark", VOID);
			DefineInputFunc("StopSpark", "InputStopSpark", VOID);
			DefineInputFunc("ToggleSpark", "InputToggleSpark", VOID);
			DefineInputFunc("SparkOnce", "InputSparkOnce", VOID);
			DefineOutput("m_OnSpark", "OnSpark");
			
			BeginDataMap("CEnvExplosion", "CPointEntity");
			LinkNamesToMap("env_explosion");
			DefineKeyField("m_iMagnitude", "iMagnitude", INTEGER);
			DefineKeyField("m_iRadiusOverride", "iRadiusOverride", INTEGER);
			DefineField("m_spriteScale", INTEGER);
			DefineKeyField("m_flDamageForce", "DamageForce", FLOAT);
			DefineField("m_iszFireballSprite", STRING);
			DefineField("m_sFireballSprite", SHORT);
			DefineField("m_hInflictor", EHANDLE);
			DefineField("m_iCustomDamageType", INTEGER);
			DefineField("m_iClassIgnore", INTEGER);
			DefineKeyField("m_hEntityIgnore", "ignoredEntity", EHANDLE);
			DefineThinkFunc("Smoke");
			DefineInputFunc("Explode", "InputExplode", VOID);
			
			BeginDataMap("CEnvEntityMaker", "CPointEntity");
			LinkNamesToMap("env_entity_maker");
			DefineField("m_hCurrentInstance", EHANDLE);
			DefineField("m_hCurrentBlocker", EHANDLE);
			DefineField("m_vecBlockerOrigin", VECTOR);
			DefineKeyField("m_iszTemplate", "EntityTemplate", STRING);
			DefineKeyField("m_angPostSpawnDirection", "PostSpawnDirection", VECTOR);
			DefineKeyField("m_flPostSpawnDirectionVariance", "PostSpawnDirectionVariance", FLOAT);
			DefineKeyField("m_flPostSpawnSpeed", "PostSpawnSpeed", FLOAT);
			DefineKeyField("m_bPostSpawnUseAngles", "PostSpawnInheritAngles", BOOLEAN);
			DefineOutput("m_pOutputOnSpawned", "OnEntitySpawned");
			DefineOutput("m_pOutputOnFailedSpawn", "OnEntityFailedSpawn");
			DefineInputFunc("ForceSpawn", "InputForceSpawn", VOID);
			DefineInputFunc("ForceSpawnAtEntityOrigin", "InputForceSpawnAtEntityOrigin", STRING);
			DefineThinkFunc("CheckSpawnThink");
			
			DataMapProxy("CTextureToggle", "CPointEntity");
			LinkNamesToMap("env_texturetoggle");
			DefineInputFunc("IncrementTextureIndex", "InputIncrementBrushTexIndex", VOID);
			DefineInputFunc("SetTextureIndex", "InputSetBrushTexIndex", INTEGER);
			
			BeginDataMap("CEnvSoundscape", "CPointEntity");
			LinkNamesToMap("env_soundscape");
			DefineKeyField("m_flRadius", "radius", FLOAT);
			DefineField("m_soundscapeName", STRING);
			DefineField("m_hProxySoundscape", EHANDLE);
			DefineKeyField("m_positionNames[0]", "position0", STRING);
			DefineKeyField("m_positionNames[1]", "position1", STRING);
			DefineKeyField("m_positionNames[2]", "position2", STRING);
			DefineKeyField("m_positionNames[3]", "position3", STRING);
			DefineKeyField("m_positionNames[4]", "position4", STRING);
			DefineKeyField("m_positionNames[5]", "position5", STRING);
			DefineKeyField("m_positionNames[6]", "position6", STRING);
			DefineKeyField("m_positionNames[7]", "position7", STRING);
			DefineVector("m_hPlayersInPVS", EHANDLE);
			DefineField("m_flNextUpdatePlayersInPVS", TIME);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("ToggleEnabled", "InputToggleEnabled", VOID);
			DefineOutput("m_OnPlay", "OnPlay");
			
			BeginDataMap("CEnvShake", "CPointEntity");
			LinkNamesToMap("env_shake");
			DefineKeyField("m_Amplitude", "amplitude", FLOAT);
			DefineKeyField("m_Frequency", "frequency", FLOAT);
			DefineKeyField("m_Duration", "duration", FLOAT);
			DefineKeyField("m_Radius", "radius", FLOAT);
			DefineField("m_stopTime", TIME);
			DefineField("m_nextShake", TIME);
			DefineField("m_currentAmp", FLOAT);
			DefineField("m_maxForce", VECTOR);
			DefinePhysPtr("m_pShakeController");
			DefineEmbeddedField("m_shakeCallback", "CPhysicsShake");
			DefineInputFunc("StartShake", "InputStartShake", VOID);
			DefineInputFunc("StopShake", "InputStopShake", VOID);
			DefineInputFunc("Amplitude", "InputAmplitude", FLOAT);
			DefineInputFunc("Frequency", "InputFrequency", FLOAT);
			
			BeginDataMap("CSpeaker", "CPointEntity");
			LinkNamesToMap("env_speaker");
			DefineKeyField("m_delayMin", "delaymin", FLOAT);
			DefineKeyField("m_delayMax", "delaymax", FLOAT);
			DefineKeyField("m_iszRuleScriptFile", "rulescript", STRING);
			DefineKeyField("m_iszConcept", "concept", STRING);
			DefineFunction("SpeakerThink");
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			
			BeginDataMap("CEnvMicrophone", "CPointEntity");
			LinkNamesToMap("env_microphone");
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineField("m_hMeasureTarget", EHANDLE);
			DefineKeyField("m_nSoundMask", "SoundMask", INTEGER);
			DefineKeyField("m_flSensitivity", "Sensitivity", FLOAT);
			DefineKeyField("m_flSmoothFactor", "SmoothFactor", FLOAT);
			DefineKeyField("m_iszSpeakerName", "SpeakerName", STRING);
			DefineKeyField("m_iszListenFilter", "ListenFilter", STRING);
			DefineField("m_hListenFilter", EHANDLE);
			DefineField("m_hSpeaker", EHANDLE);
			DefineKeyField("m_iSpeakerDSPPreset", "speaker_dsp_preset", INTEGER);
			DefineKeyField("m_flMaxRange", "MaxRange", FLOAT);
			DefineField("m_szLastSound", CHARACTER, 256);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("SetSpeakerName", "InputSetSpeakerName", STRING);
			DefineOutput("m_SoundLevel", "SoundLevel");
			DefineOutput("m_OnRoutedSound", "OnRoutedSound");
			DefineOutput("m_OnHeardSound", "OnHeardSound");
			
			BeginDataMap("CEnvTonemapController", "CPointEntity"); // controls player's tonemap
			LinkNamesToMap("env_tonemap_controller");
			DefineField("m_flBlendTonemapStart", FLOAT);
			DefineField("m_flBlendTonemapEnd", FLOAT);
			DefineField("m_flBlendEndTime", TIME);
			DefineField("m_flBlendStartTime", TIME);
			DefineField("m_bUseCustomAutoExposureMin", BOOLEAN);
			DefineField("m_bUseCustomAutoExposureMax", BOOLEAN);
			DefineField("m_flCustomAutoExposureMin", FLOAT);
			DefineField("m_flCustomAutoExposureMax", FLOAT);
			DefineField("m_flCustomBloomScale", FLOAT);
			DefineField("m_flCustomBloomScaleMinimum", FLOAT);
			DefineField("m_bUseCustomBloomScale", BOOLEAN);
			if (Game == Game.PORTAL2) {
				DefineField("m_flBloomExponent", FLOAT);
				DefineField("m_flBloomSaturation", FLOAT);
			}
			DefineThinkFunc("UpdateTonemapScaleBlend");
			DefineInputFunc("SetTonemapScale", "InputSetTonemapScale", FLOAT);
			DefineInputFunc("BlendTonemapScale", "InputBlendTonemapScale", STRING);
			DefineInputFunc("SetTonemapRate", "InputSetTonemapRate", FLOAT);
			DefineInputFunc("SetAutoExposureMin", "InputSetAutoExposureMin", FLOAT);
			DefineInputFunc("SetAutoExposureMax", "InputSetAutoExposureMax", FLOAT);
			DefineInputFunc("UseDefaultAutoExposure", "InputUseDefaultAutoExposure", VOID);
			DefineInputFunc("UseDefaultBloomScale", "InputUseDefaultBloomScale", VOID);
			DefineInputFunc("SetBloomScale", "InputSetBloomScale", FLOAT);
			DefineInputFunc("SetBloomScaleRange", "InputSetBloomScaleRange", FLOAT);
			if (Game == Game.PORTAL2) {
				DefineInputFunc("SetBloomExponent", "InputSetBloomExponent", FLOAT);
				DefineInputFunc("SetBloomSaturation", "InputSetBloomSaturation", FLOAT);
			}
			
			BeginDataMap("CCitadelEnergyCore", "CBaseEntity"); // charges up and then releases energy from its position
			LinkNamesToMap("env_citadel_energy_core");
			DefineKeyField("m_flScale", "scale", FLOAT);
			DefineField("m_nState", INTEGER);
			DefineField("m_flDuration", FLOAT);
			DefineField("m_flStartTime", TIME);
			DefineInputFunc("StartCharge", "InputStartCharge", FLOAT);
			DefineInputFunc("StartDischarge", "InputStartDischarge", VOID);
			DefineInputFunc("Stop", "InputStop", FLOAT);
			
			BeginDataMap("CSprite", "CBaseEntity");
			LinkNamesToMap("env_sprite");
			DefineField("m_flLastTime", TIME);
			DefineField("m_flMaxFrame", FLOAT);
			DefineField("m_hAttachedToEntity", EHANDLE);
			DefineField("m_nAttachment", INTEGER);
			DefineField("m_flDieTime", TIME);
			DefineField("m_nBrightness", INTEGER);
			DefineField("m_flBrightnessTime", FLOAT);
			DefineInputAndKeyField("m_flSpriteScale", "scale", "SetScale", FLOAT);
			DefineKeyField("m_flSpriteFramerate", "framerate", FLOAT);
			DefineKeyField("m_flFrame", "frame", FLOAT);
			if (GenInfo.IsDefPortal) {
				DefineField("m_bDrawInMainRender", BOOLEAN);
				DefineField("m_bDrawInPortalRender", BOOLEAN);
			}
			DefineKeyField("m_flHDRColorScale", "HDRColorScale", FLOAT);
			DefineKeyField("m_flGlowProxySize", "GlowProxySize", FLOAT);
			DefineField("m_flScaleTime", FLOAT);
			DefineField("m_flStartScale", FLOAT);
			DefineField("m_flDestScale", FLOAT);
			DefineField("m_flScaleTimeStart", TIME);
			DefineField("m_nStartBrightness", INTEGER);
			DefineField("m_nDestBrightness", INTEGER);
			DefineField("m_flBrightnessTimeStart", TIME);
			DefineField("m_bWorldSpaceScale", BOOLEAN);
			DefineFunction("AnimateThink");
			DefineFunction("ExpandThink");
			DefineFunction("AnimateUntilDead");
			DefineFunction("BeginFadeOutThink");
			DefineInputFunc("HideSprite", "InputHideSprite", VOID);
			DefineInputFunc("ShowSprite", "InputShowSprite", VOID);
			DefineInputFunc("ToggleSprite", "InputToggleSprite", VOID);
			DefineInputFunc("ColorRedValue", "InputColorRedValue", FLOAT);
			DefineInputFunc("ColorGreenValue", "InputColorGreenValue", FLOAT);
			DefineInputFunc("ColorBlueValue", "InputColorBlueValue", FLOAT);
			
			BeginDataMap("CEnvBeam", "CBeam");
			LinkNamesToMap("env_beam");
			DefineField("m_active", INTEGER);
			DefineField("m_spriteTexture", INTEGER);
			DefineKeyField("m_iszStartEntity", "LightningStart", STRING);
			DefineKeyField("m_iszEndEntity", "LightningEnd", STRING);
			DefineKeyField("m_life", "life", FLOAT);
			DefineKeyField("m_boltWidth", "BoltWidth", FLOAT);
			DefineKeyField("m_noiseAmplitude", "NoiseAmplitude", FLOAT);
			DefineKeyField("m_speed", "TextureScroll", INTEGER);
			DefineKeyField("m_restrike", "StrikeTime", FLOAT);
			DefineKeyField("m_iszSpriteName", "texture", STRING);
			DefineKeyField("m_frameStart", "framestart", INTEGER);
			DefineKeyField("m_radius", "Radius", FLOAT);
			DefineKeyField("m_TouchType", "TouchType", INTEGER);
			DefineKeyField("m_iFilterName", "filtername", STRING);
			DefineKeyField("m_iszDecal", "decalname", STRING);
			DefineField("m_hFilter", EHANDLE);
			DefineFunction("StrikeThink");
			DefineFunction("UpdateThink");
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("StrikeOnce", "InputStrikeOnce", VOID);
			DefineOutput("m_OnTouchedByEntity", "OnTouchedByEntity");
			
			BeginDataMap("CEnvFade", "CLogicalEntity");
			LinkNamesToMap("env_fade");
			DefineKeyField("m_Duration", "duration", FLOAT);
			DefineKeyField("m_HoldTime", "holdtime", FLOAT);
			DefineInputFunc("Fade", "InputFade", VOID);
			DefineOutput("m_OnBeginFade", "OnBeginFade");
			if (Game == Game.PORTAL2)
				DefineField("m_flReverseFadeDuration", FLOAT);
			
			BeginDataMap("TrailPoint_t");
			DefineField("m_vecScreenPos", POSITION_VECTOR); // if screen space trails, then just a vector
			DefineField("m_flDieTime", TIME);
			DefineField("m_flTexCoord", FLOAT);
			DefineField("m_flWidthVariance", FLOAT);
			
			BeginDataMap("CSpriteTrail", "CSprite");
			LinkNamesToMap("env_spritetrail");
			DefineKeyField("m_flLifeTime", "lifetime", FLOAT);
			DefineKeyField("m_flStartWidth", "startwidth", FLOAT);
			DefineKeyField("m_flEndWidth", "endwidth", FLOAT);
			DefineKeyField("m_iszSpriteName", "spritename", STRING);
			DefineKeyField("m_bAnimate", "animate", BOOLEAN);
			DefineField("m_flStartWidthVariance", FLOAT);
			DefineField("m_flTextureRes", FLOAT);
			DefineField("m_flMinFadeLength", FLOAT);
			DefineField("m_vecSkyboxOrigin", POSITION_VECTOR);
			DefineField("m_flSkyboxScale", FLOAT);
			if (GenInfo.IsDefClientDll) {
				DefineEmbeddedField("m_vecSteps", "TrailPoint_t", MAX_WAKE_POINTS);
				DefineField("m_nFirstStep", INTEGER);
				DefineField("m_nStepCount", INTEGER);
				DefineField("m_flUpdateTime", TIME);
				DefineField("m_vecPrevSkyboxOrigin", POSITION_VECTOR);
				DefineField("m_flPrevSkyboxScale", FLOAT);
				DefineField("m_vecRenderMins", VECTOR);
				DefineField("m_vecRenderMaxs", VECTOR);
			}
			
			BeginDataMap("CPortalCredits", "CPointEntity");
			LinkNamesToMap("env_portal_credits");
			DefineInputFunc("RollCredits", "InputRollCredits", VOID);
			DefineInputFunc("RollOutroCredits", "InputRollOutroCredits", VOID);
			DefineInputFunc("RollPortalOutroCredits", "InputRollPortalOutroCredits", VOID);
			DefineInputFunc("ShowLogo", "InputShowLogo", VOID);
			DefineInputFunc("SetLogoLength", "InputSetLogoLength", FLOAT);
			DefineOutput("m_OnCreditsDone", "OnCreditsDone");
			DefineField("m_bRolledOutroCredits", BOOLEAN);
			DefineField("m_flLogoLength", FLOAT);
			
			BeginDataMap("CFogController", "CBaseEntity");
			LinkNamesToMap("env_fog_controller");
			DefineInputFunc("SetStartDist", "InputSetStartDist", FLOAT);
			DefineInputFunc("SetEndDist", "InputSetEndDist", FLOAT);
			DefineInputFunc("SetMaxDensity", "InputSetMaxDensity", FLOAT);
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			DefineInputFunc("SetColor", "InputSetColor", COLOR32);
			DefineInputFunc("SetColorSecondary", "InputSetColorSecondary", COLOR32);
			DefineInputFunc("SetFarZ", "InputSetFarZ", INTEGER);
			DefineInputFunc("SetAngles", "InputSetAngles", STRING);
			DefineInputFunc("SetColorLerpTo", "InputSetColorLerpTo", COLOR32);
			DefineInputFunc("SetColorSecondaryLerpTo", "InputSetColorSecondaryLerpTo", COLOR32);
			DefineInputFunc("SetStartDistLerpTo", "InputSetStartDistLerpTo", FLOAT);
			DefineInputFunc("SetEndDistLerpTo", "InputSetEndDistLerpTo", FLOAT);
			DefineInputFunc("StartFogTransition", "InputStartFogTransition", VOID);
			DefineKeyField("m_bUseAngles", "use_angles", BOOLEAN);
			DefineKeyField("m_fog.colorPrimary", "fogcolor", COLOR32);
			DefineKeyField("m_fog.colorSecondary", "fogcolor2", COLOR32);
			DefineKeyField("m_fog.dirPrimary", "fogdir", VECTOR);
			DefineKeyField("m_fog.enable", "fogenable", BOOLEAN);
			DefineKeyField("m_fog.blend", "fogblend", BOOLEAN);
			DefineKeyField("m_fog.start", "fogstart", FLOAT);
			DefineKeyField("m_fog.end", "fogend", FLOAT);
			DefineKeyField("m_fog.maxdensity", "fogmaxdensity", FLOAT);
			DefineKeyField("m_fog.farz", "farz", FLOAT);
			DefineKeyField("m_fog.duration", "foglerptime", FLOAT);
			if (Game == Game.PORTAL2)
				DefineKeyField("m_fog.HDRColorScale", "HDRColorScale", FLOAT);
			DefineThinkFunc("SetLerpValues");
			DefineField("m_iChangedVariables", INTEGER);
			DefineField("m_fog.lerptime", TIME);
			DefineField("m_fog.colorPrimaryLerpTo", COLOR32);
			DefineField("m_fog.colorSecondaryLerpTo", COLOR32);
			DefineField("m_fog.startLerpTo", FLOAT);
			DefineField("m_fog.endLerpTo", FLOAT);
			if (Game == Game.PORTAL2)
				DefineField("m_fog.maxdensityLerpTo", FLOAT);

			BeginDataMap("CSmokeStackLightInfo");
			DefineField("m_vPos", POSITION_VECTOR);
			DefineField("m_vColor", VECTOR);
			DefineField("m_flIntensity", FLOAT);
			
			BeginDataMap("CSmokeStack", "CBaseParticleEntity");
			LinkNamesToMap("env_smokestack");
			DefineKeyField("m_StartSize", "StartSize", FLOAT);
			DefineKeyField("m_EndSize", "EndSize", FLOAT);
			DefineKeyField("m_InitialState", "InitialState", BOOLEAN);
			DefineKeyField("m_flBaseSpread", "BaseSpread", FLOAT);
			DefineKeyField("m_flTwist", "Twist", FLOAT);
			DefineKeyField("m_flRollSpeed", "Roll", FLOAT);
			DefineField("m_strMaterialModel", STRING);
			DefineField("m_iMaterialModel", INTEGER);
			DefineEmbeddedField("m_AmbientLight", "CSmokeStackLightInfo");
			DefineEmbeddedField("m_DirLight", "CSmokeStackLightInfo");
			DefineKeyField("m_WindAngle", "WindAngle", INTEGER);
			DefineKeyField("m_WindSpeed", "WindSpeed", INTEGER);
			DefineField("m_vWind", VECTOR);
			DefineField("m_bEmit", INTEGER);
			DefineInput("m_JetLength", "JetLength", FLOAT);
			DefineInput("m_SpreadSpeed", "SpreadSpeed", FLOAT);
			DefineInput("m_Speed", "Speed", FLOAT);
			DefineInput("m_Rate", "Rate", FLOAT);
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			
			BeginDataMap("CFire", "CBaseEntity");
			LinkNamesToMap("env_fire");
			DefineField("m_hEffect", EHANDLE);
			DefineField("m_hOwner", EHANDLE);
			DefineKeyField("m_nFireType", "firetype", INTEGER);
			DefineField("m_flFuel", FLOAT);
			DefineField("m_flDamageTime", TIME);
			DefineField("m_lastDamage", TIME);
			DefineKeyField("m_flFireSize", "firesize", FLOAT);
			DefineKeyField("m_flHeatLevel", "ignitionpoint", FLOAT);
			DefineField("m_flHeatAbsorb", FLOAT);
			DefineKeyField("m_flDamageScale", "damagescale", FLOAT);
			DefineField("m_flMaxHeat", FLOAT);
			DefineKeyField("m_flAttackTime", "fireattack", FLOAT);
			DefineField("m_bEnabled", BOOLEAN);
			DefineKeyField("m_bStartDisabled", "StartDisabled", BOOLEAN);
			DefineField("m_bDidActivate", BOOLEAN);
			DefineFunction("BurnThink");
			DefineFunction("GoOutThink");
			DefineInputFunc("StartFire", "InputStartFire", VOID);
			DefineInputFunc("Extinguish", "InputExtinguish", FLOAT);
			DefineInputFunc("ExtinguishTemporary", "InputExtinguishTemporary", FLOAT);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineOutput("m_OnIgnited", "OnIgnited");
			DefineOutput("m_OnExtinguished", "OnExtinguished");
			
			BeginDataMap("CSun", "CBaseEntity");
			LinkNamesToMap("env_sun");
			DefineField("m_vDirection", VECTOR);
			DefineKeyField("m_bUseAngles", "use_angles", INTEGER);
			DefineKeyField("m_flPitch", "pitch", FLOAT);
			DefineKeyField("m_flYaw", "angle", FLOAT);
			DefineKeyField("m_nSize", "size", INTEGER);
			DefineKeyField("m_clrOverlay", "overlaycolor", COLOR32);
			DefineKeyField("m_nOverlaySize", "overlaysize", INTEGER);
			DefineKeyField("m_strMaterial", "material", STRING);
			DefineKeyField("m_strOverlayMaterial", "overlaymaterial", STRING);
			DefineField("m_bOn", BOOLEAN);
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			DefineInputFunc("SetColor", "InputSetColor", COLOR32);
			DefineKeyField("m_flHDRColorScale", "HDRColorScale", FLOAT);
			
			BeginDataMap("CGibShooter", "CBaseEntity");
			LinkNamesToMap("gibshooter");
			DefineKeyField("m_iGibs", "m_iGibs", INTEGER);
			DefineKeyField("m_flGibVelocity", "m_flVelocity", FLOAT);
			DefineKeyField("m_flVariance", "m_flVariance", FLOAT);
			DefineKeyField("m_flGibLife", "m_flGibLife", FLOAT);
			DefineKeyField("m_nSimulationType", "Simulation", INTEGER);
			DefineKeyField("m_flDelay", "delay", FLOAT);
			DefineKeyField("m_angGibRotation", "gibangles", VECTOR);
			DefineKeyField("m_flGibAngVelocity", "gibanglevelocity", FLOAT);
			DefineField("m_bIsSprite", BOOLEAN);
			DefineField("m_iGibCapacity", INTEGER);
			DefineField("m_iGibMaterial", INTEGER);
			DefineField("m_iGibModelIndex", INTEGER);
			DefineField("m_nMaxGibModelFrame", INTEGER);
			DefineKeyField("m_iszLightingOrigin", "LightingOrigin", STRING);
			DefineKeyField("m_bNoGibShadows", "nogibshadows", BOOLEAN);
			DefineInputFunc("Shoot", "InputShoot", VOID);
			DefineFunction("ShootThink");
			
			BeginDataMap("CEnvShooter", "CGibShooter");
			LinkNamesToMap("env_shooter");
			DefineKeyField("m_nSkin", "skin", INTEGER);
			DefineKeyField("m_flGibScale", "scale", FLOAT);
			DefineKeyField("m_flGibGravityScale", "gibgravityscale", FLOAT);
			
			BeginDataMap("CBaseFire", "CBaseEntity");
			DefineField("m_flStartScale", FLOAT);
			DefineField("m_flScale", FLOAT);
			DefineField("m_flScaleTime", TIME);
			DefineField("m_nFlags", INTEGER);

			BeginDataMap("CFireSmoke", "CBaseFire");
			LinkNamesToMap("_firesmoke");
			DefineField("m_flStartScale", FLOAT);
			DefineField("m_flScale", FLOAT);
			DefineField("m_flScaleTime", FLOAT); // this collides with TIME m_flScaleTime from CBaseFire
			DefineField("m_nFlags", INTEGER);
			DefineField("m_nFlameModelIndex", MODELINDEX);
			DefineField("m_nFlameFromAboveModelIndex", MODELINDEX);
			
			BeginDataMap("CEnvHudHint", "CPointEntity");
			LinkNamesToMap("env_hudhint");
			DefineKeyField("m_iszMessage", "message", STRING);
			DefineInputFunc("ShowHudHint", "InputShowHudHint", VOID);
			DefineInputFunc("HideHudHint", "InputHideHudHint", VOID);
			
			BeginDataMap("CEnvSplash", "CPointEntity");
			LinkNamesToMap("env_splash");
			DefineKeyField("m_flScale", "scale", FLOAT);
			DefineInputFunc("Splash", "InputSplash", VOID);
			
			BeginDataMap("CFuncSmokeVolume", "CBaseParticleEntity");
			LinkNamesToMap("func_smokevolume");
			DefineKeyField("m_Color1", "Color1", COLOR32);
			DefineKeyField("m_Color2", "Color2", COLOR32);
			DefineKeyField("m_String_tMaterialName", "Material", STRING);
			DefineKeyField("m_ParticleDrawWidth", "ParticleDrawWidth", FLOAT);
			DefineKeyField("m_ParticleSpacingDistance", "ParticleSpacingDistance", FLOAT);
			DefineKeyField("m_DensityRampSpeed", "DensityRampSpeed", FLOAT);
			DefineInputAndKeyField("m_RotationSpeed", "RotationSpeed", "SetRotationSpeed", FLOAT);
			DefineInputAndKeyField("m_MovementSpeed", "MovementSpeed", "SetMovementSpeed", FLOAT);
			DefineInputAndKeyField("m_Density", "Density", "SetDensity", FLOAT);
			
			BeginDataMap("CEnvProjectedTexture", "CPointEntity");
			LinkNamesToMap("env_projectedtexture");
			DefineField("m_hTargetEntity", EHANDLE);
			DefineField("m_bState", BOOLEAN);
			DefineKeyField("m_flLightFOV", "lightfov", FLOAT);
			DefineKeyField("m_bEnableShadows", "enableshadows", BOOLEAN);
			if (Game == Game.PORTAL2)
				DefineKeyField("m_bSimpleProjection", "simpleprojection", BOOLEAN);
			DefineKeyField("m_bLightOnlyTarget", "lightonlytarget", BOOLEAN);
			DefineKeyField("m_bLightWorld", "lightworld", BOOLEAN);
			DefineKeyField("m_bCameraSpace", "cameraspace", BOOLEAN);
			DefineKeyField("m_flAmbient", "ambient", FLOAT);
			DefineKeyField("m_SpotlightTextureName", "texturename", CHARACTER, MAX_PATH);
			DefineKeyField("m_nSpotlightTextureFrame", "textureframe", INTEGER);
			DefineKeyField("m_flNearZ", "nearz", FLOAT);
			DefineKeyField("m_flFarZ", "farz", FLOAT);
			DefineKeyField("m_nShadowQuality", "shadowquality", INTEGER);
			if (Game == Game.PORTAL2) {
				DefineKeyField("m_flBrightnessScale", "brightnessscale", FLOAT);
				DefineField("m_LightColor", COLOR32);
				DefineKeyField("m_flColorTransitionTime", "colortransitiontime", FLOAT);
				DefineKeyField("m_flProjectionSize", "projection_size", FLOAT);
				DefineKeyField("m_flRotation", "projection_rotation", FLOAT);
			} else {
				DefineField("m_LinearFloatLightColor", VECTOR);
			}
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			if (Game == Game.PORTAL2) {
				DefineInputFunc("AlwaysUpdateOn", "InputAlwaysUpdateOn", VOID);
				DefineInputFunc("AlwaysUpdateOff", "InputAlwaysUpdateOff", VOID);
			}
			DefineInputFunc("FOV", "InputSetFOV", FLOAT);
			DefineInputFunc("Target", "InputSetTarget", EHANDLE);
			DefineInputFunc("CameraSpace", "InputSetCameraSpace", BOOLEAN);
			DefineInputFunc("LightOnlyTarget", "InputSetLightOnlyTarget", BOOLEAN);
			DefineInputFunc("LightWorld", "InputSetLightWorld", BOOLEAN);
			DefineInputFunc("EnableShadows", "InputSetEnableShadows", BOOLEAN);
			if (Game == Game.PORTAL2)
				DefineInputFunc("LightColor", "InputSetLightColor", COLOR32);
			DefineInputFunc("Ambient", "InputSetAmbient", FLOAT);
			DefineInputFunc("SpotlightTexture", "InputSetSpotlightTexture", STRING);
			DefineThinkFunc("InitialThink");
			if (Game == Game.PORTAL2) {
				DefineField("m_bAlwaysUpdate", BOOLEAN);
				DefineField("m_iStyle", INTEGER);
			}
			
			BeginDataMap("CLightGlow", "CBaseEntity");
			LinkNamesToMap("env_lightglow");
			DefineKeyField("m_nVerticalSize", "VerticalGlowSize", INTEGER);
			DefineKeyField("m_nHorizontalSize", "HorizontalGlowSize", INTEGER);
			DefineKeyField("m_nMinDist", "MinDist", INTEGER);
			DefineKeyField("m_nMaxDist", "MaxDist", INTEGER);
			DefineKeyField("m_nOuterMaxDist", "OuterMaxDist", INTEGER);
			DefineKeyField("m_flGlowProxySize", "GlowProxySize", FLOAT);
			DefineKeyField("m_flHDRColorScale", "HDRColorScale", FLOAT);
			DefineInputFunc("Color", "InputColor", COLOR32);
		}
	}
}