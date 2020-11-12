// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public sealed class LogicMaps : DataMapGenerator {
		
		protected override void CreateDataMaps() {
			BeginDataMap("CLogicAuto", "CBaseEntity");
			LinkNamesToMap("logic_auto");
			DefineKeyField("m_globalstate", "globalstate", STRING);
			DefineOutput("m_OnMapSpawn", "OnMapSpawn");
			DefineOutput("m_OnNewGame", "OnNewGame");
			DefineOutput("m_OnLoadGame", "OnLoadGame");
			DefineOutput("m_OnMapTransition", "OnMapTransition");
			DefineOutput("m_OnBackgroundMap", "OnBackgroundMap");
			DefineOutput("m_OnMultiNewMap", "OnMultiNewMap");
			DefineOutput("m_OnMultiNewRound", "OnMultiNewRound");
			
			BeginDataMap("CSimpleSimTimer");
			DefineField("m_next", TIME);

			BeginDataMap("CSimTimer", "CSimpleSimTimer");
			DefineField("m_interval", FLOAT);

			BeginDataMap("CRandSimTimer", "CSimpleSimTimer");
			DefineField("m_minInterval", FLOAT);
			DefineField("m_maxInterval", FLOAT);

			BeginDataMap("CStopwatchBase", "CSimpleSimTimer");
			DefineField("m_fIsRunning", BOOLEAN);

			BeginDataMap("CStopwatch", "CStopwatchBase");
			DefineField("m_interval", FLOAT);

			BeginDataMap("CRandStopwatch", "CStopwatchBase");
			DefineField("m_minInterval", FLOAT);
			DefineField("m_maxInterval", FLOAT);
			
			BeginDataMap("CLogicRelay", "CLogicalEntity");
			LinkNamesToMap("logic_relay");
			DefineField("m_bWaitForRefire", BOOLEAN);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("EnableRefire", VOID);
			//DefineINPUTFUNC("Disable", VOID);
			//DefineINPUTFUNC("Toggle", VOID);
			//DefineINPUTFUNC("Trigger", VOID);
			//DefineINPUTFUNC("CancelPending", VOID);
			DefineOutput("m_OnTrigger", "OnTrigger");
			DefineOutput("m_OnSpawn", "OnSpawn");
			
			BeginDataMap("CLogicAutosave","CLogicalEntity");
			LinkNamesToMap("logic_autosave");
			DefineKeyField("m_bForceNewLevelUnit", "NewLevelUnit", BOOLEAN);
			DefineKeyField("m_minHitPoints", "MinimumHitPoints", INTEGER);
			DefineKeyField("m_minHitPointsToCommit", "MinHitPointsToCommit", INTEGER);
			//DefineINPUTFUNC("Save", VOID);
			//DefineINPUTFUNC("SaveDangerous", FLOAT);
			//DefineINPUTFUNC("SetMinHitpointsThreshold", INTEGER);
			
			BeginDataMap("CMathCounter", "CLogicalEntity");
			LinkNamesToMap("math_counter");
			DefineField("m_bHitMax", BOOLEAN);
			DefineField("m_bHitMin", BOOLEAN);
			DefineKeyField("m_flMin", "min", FLOAT);
			DefineKeyField("m_flMax", "max", FLOAT);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			//DefineINPUTFUNC("Add", FLOAT);
			//DefineINPUTFUNC("Divide", FLOAT);
			//DefineINPUTFUNC("Multiply", FLOAT);
			//DefineINPUTFUNC("SetValue", FLOAT);
			//DefineINPUTFUNC("SetValueNoFire", FLOAT);
			//DefineINPUTFUNC("Subtract", FLOAT);
			//DefineINPUTFUNC("SetHitMax", FLOAT);
			//DefineINPUTFUNC("SetHitMin", FLOAT);
			//DefineINPUTFUNC("GetValue", VOID);
			//DefineINPUTFUNC("Enable", VOID);
			//DefineINPUTFUNC("Disable", VOID);
			DefineOutput("m_OutValue", "OutValue");
			DefineOutput("m_OnHitMin", "OnHitMin");
			DefineOutput("m_OnHitMax", "OnHitMax");
			DefineOutput("m_OnGetValue", "OnGetValue");
			
			// compares a float to a predefined value, firing output to indicate the result
			BeginDataMap("CLogicCompare", "CLogicalEntity");
			LinkNamesToMap("logic_compare");
			DefineKeyField("m_flCompareValue", "CompareValue", FLOAT);
			DefineKeyField("m_flInValue", "InitialValue", FLOAT);
			DefineInputFunc("SetValue", "InputSetValue", FLOAT);
			DefineInputFunc("SetValueCompare", "InputSetValueCompare", FLOAT);
			DefineInputFunc("SetCompareValue", "InputSetCompareValue", FLOAT);
			DefineInputFunc("Compare", "InputCompare", VOID);
			DefineOutput("m_OnEqualTo", "OnEqualTo");
			DefineOutput("m_OnNotEqualTo", "OnNotEqualTo");
			DefineOutput("m_OnGreaterThan", "OnGreaterThan");
			DefineOutput("m_OnLessThan", "OnLessThan");
			
			BeginDataMap("CSkyCamera", "CLogicalEntity");
			LinkNamesToMap("sky_camera");
			DefineKeyField("m_skyboxData.scale", "scale", INTEGER);
			DefineField("m_skyboxData.origin", VECTOR);
			DefineField("m_skyboxData.area", INTEGER);
			DefineKeyField("m_bUseAngles", "use_angles", BOOLEAN);
			DefineKeyField("m_skyboxData.fog.enable", "fogenable", BOOLEAN);
			DefineKeyField("m_skyboxData.fog.blend", "fogblend", BOOLEAN);
			DefineKeyField("m_skyboxData.fog.dirPrimary", "fogdir", VECTOR);
			DefineKeyField("m_skyboxData.fog.colorPrimary", "fogcolor", COLOR32);
			DefineKeyField("m_skyboxData.fog.colorSecondary", "fogcolor2", COLOR32);
			DefineKeyField("m_skyboxData.fog.start", "fogstart", FLOAT);
			DefineKeyField("m_skyboxData.fog.end", "fogend", FLOAT);
			DefineKeyField("m_skyboxData.fog.maxdensity", "fogmaxdensity", FLOAT);
			
			// see logic_achievement.cpp for a string list of (some?) of the achievements
			BeginDataMap("CLogicAchievement", "CLogicalEntity");
			LinkNamesToMap("logic_achievement");
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineKeyField("m_iszAchievementEventID", "AchievementEvent", STRING);
			DefineInputFunc("FireEvent", "InputFireEvent", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineOutput("m_OnFired", "OnFired");
			
			// relays I/O from player to world and vice versa
			BeginDataMap("CLogicPlayerProxy", "CLogicalEntity");
			LinkNamesToMap("logic_playerproxy");
			DefineOutput("m_OnFlashlightOn", "OnFlashlightOn");
			DefineOutput("m_OnFlashlightOff", "OnFlashlightOff");
			DefineOutput("m_RequestedPlayerHealth", "PlayerHealth");
			DefineOutput("m_PlayerHasAmmo", "PlayerHasAmmo");
			DefineOutput("m_PlayerHasNoAmmo", "PlayerHasNoAmmo");
			DefineOutput("m_PlayerDied", "PlayerDied");
			DefineOutput("m_PlayerMissedAR2AltFire", "PlayerMissedAR2AltFire");
			DefineInputFunc("RequestPlayerHealth", "InputRequestPlayerHealth", VOID);
			DefineInputFunc("SetFlashlightSlowDrain", "InputSetFlashlightSlowDrain", VOID);
			DefineInputFunc("SetFlashlightNormalDrain", "InputSetFlashlightNormalDrain", VOID);
			DefineInputFunc("SetPlayerHealth", "InputSetPlayerHealth", INTEGER);
			DefineInputFunc("RequestAmmoState", "InputRequestAmmoState", VOID);
			DefineInputFunc("LowerWeapon", "InputLowerWeapon", VOID);
			DefineInputFunc("EnableCappedPhysicsDamage", "InputEnableCappedPhysicsDamage", VOID);
			DefineInputFunc("DisableCappedPhysicsDamage", "InputDisableCappedPhysicsDamage", VOID);
			DefineInputFunc("SetLocatorTargetEntity", "InputSetLocatorTargetEntity", STRING);
			DefineField("m_hPlayer", EHANDLE);
		}
	}
}