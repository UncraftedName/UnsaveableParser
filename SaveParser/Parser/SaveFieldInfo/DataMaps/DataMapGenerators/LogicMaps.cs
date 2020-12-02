// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public sealed class LogicMaps : DataMapGenerator {

		public const int MAX_LOGIC_CASES = 16;
		
		
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
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("EnableRefire", "InputEnableRefire", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("Trigger", "InputTrigger", VOID);
			DefineInputFunc("CancelPending", "InputCancelPending", VOID);
			DefineOutput("m_OnTrigger", "OnTrigger");
			DefineOutput("m_OnSpawn", "OnSpawn");
			
			BeginDataMap("CLogicAutosave","CLogicalEntity");
			LinkNamesToMap("logic_autosave");
			DefineKeyField("m_bForceNewLevelUnit", "NewLevelUnit", BOOLEAN);
			DefineKeyField("m_minHitPoints", "MinimumHitPoints", INTEGER);
			DefineKeyField("m_minHitPointsToCommit", "MinHitPointsToCommit", INTEGER);
			DefineInputFunc("Save", "InputSave", VOID);
			DefineInputFunc("SaveDangerous", "InputSaveDangerous", FLOAT);
			DefineInputFunc("SetMinHitpointsThreshold", "InputSetMinHitpointsThreshold", INTEGER);
			
			BeginDataMap("CMathCounter", "CLogicalEntity");
			LinkNamesToMap("math_counter");
			DefineField("m_bHitMax", BOOLEAN);
			DefineField("m_bHitMin", BOOLEAN);
			DefineKeyField("m_flMin", "min", FLOAT);
			DefineKeyField("m_flMax", "max", FLOAT);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineInputFunc("Add", "InputAdd", FLOAT);
			DefineInputFunc("Divide", "InputDivide", FLOAT);
			DefineInputFunc("Multiply", "InputMultiply", FLOAT);
			DefineInputFunc("SetValue", "InputSetValue", FLOAT);
			DefineInputFunc("SetValueNoFire", "InputSetValueNoFire", FLOAT);
			DefineInputFunc("Subtract", "InputSubtract", FLOAT);
			DefineInputFunc("SetHitMax", "InputSetHitMax", FLOAT);
			DefineInputFunc("SetHitMin", "InputSetHitMin", FLOAT);
			DefineInputFunc("GetValue", "InputGetValue", VOID);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
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
			
			BeginDataMap("CPhysConvert", "CLogicalEntity");
			LinkNamesToMap("phys_convert");
			DefineKeyField("m_swapModel", "swapmodel", STRING);
			DefineKeyField("m_flMassOverride", "massoverride", FLOAT);
			DefineInputFunc("ConvertTarget", "InputConvertTarget", VOID);
			DefineOutput("m_OnConvert", "OnConvert");
			
			// compares a single string input to up to 16 case values, firing the corresponding output or default
			BeginDataMap("CLogicCase", "CLogicalEntity");
			LinkNamesToMap("logic_case");
			DefineKeyField("m_nCase[0]", "Case01", STRING);
			DefineKeyField("m_nCase[1]", "Case02", STRING);
			DefineKeyField("m_nCase[2]", "Case03", STRING);
			DefineKeyField("m_nCase[3]", "Case04", STRING);
			DefineKeyField("m_nCase[4]", "Case05", STRING);
			DefineKeyField("m_nCase[5]", "Case06", STRING);
			DefineKeyField("m_nCase[6]", "Case07", STRING);
			DefineKeyField("m_nCase[7]", "Case08", STRING);
			DefineKeyField("m_nCase[8]", "Case09", STRING);
			DefineKeyField("m_nCase[9]", "Case10", STRING);
			DefineKeyField("m_nCase[10]", "Case11", STRING);
			DefineKeyField("m_nCase[11]", "Case12", STRING);
			DefineKeyField("m_nCase[12]", "Case13", STRING);
			DefineKeyField("m_nCase[13]", "Case14", STRING);
			DefineKeyField("m_nCase[14]", "Case15", STRING);
			DefineKeyField("m_nCase[15]", "Case16", STRING);
			DefineField("m_nShuffleCases", INTEGER);
			DefineField("m_nLastShuffleCase", INTEGER);
			DefineField("m_uchShuffleCaseMap", CHARACTER, MAX_LOGIC_CASES);
			DefineInputFunc("InValue", "InputValue", INPUT);
			DefineInputFunc("PickRandom", "InputPickRandom", VOID);
			DefineInputFunc("PickRandomShuffle", "InputPickRandomShuffle", VOID);
			DefineOutput("m_OnCase[0]", "OnCase01");
			DefineOutput("m_OnCase[1]", "OnCase02");
			DefineOutput("m_OnCase[2]", "OnCase03");
			DefineOutput("m_OnCase[3]", "OnCase04");
			DefineOutput("m_OnCase[4]", "OnCase05");
			DefineOutput("m_OnCase[5]", "OnCase06");
			DefineOutput("m_OnCase[6]", "OnCase07");
			DefineOutput("m_OnCase[7]", "OnCase08");
			DefineOutput("m_OnCase[8]", "OnCase09");
			DefineOutput("m_OnCase[9]", "OnCase10");
			DefineOutput("m_OnCase[10]", "OnCase11");
			DefineOutput("m_OnCase[11]", "OnCase12");
			DefineOutput("m_OnCase[12]", "OnCase13");
			DefineOutput("m_OnCase[13]", "OnCase14");
			DefineOutput("m_OnCase[14]", "OnCase15");
			DefineOutput("m_OnCase[15]", "OnCase16");
			DefineOutput("m_OnDefault", "OnDefault");
			
			BeginDataMap("CLogicBranch", "CLogicalEntity");
			LinkNamesToMap("logic_branch");
			DefineKeyField("m_bInValue", "InitialValue", BOOLEAN);
			DefineVector("m_Listeners", EHANDLE);
			DefineInputFunc("SetValue", "InputSetValue", BOOLEAN);
			DefineInputFunc("SetValueTest", "InputSetValueTest", BOOLEAN);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("ToggleTest", "InputToggleTest", VOID);
			DefineInputFunc("Test", "InputTest", VOID);
			DefineOutput("m_OnTrue", "OnTrue");
			DefineOutput("m_OnFalse", "OnFalse");
			
			BeginDataMap("CLogicScript", "CPointEntity");
			LinkNamesToMap("logic_script");
			DefineKeyField("m_iszGroupMembers[0]", "Group00", STRING);
			DefineKeyField("m_iszGroupMembers[1]", "Group01", STRING);
			DefineKeyField("m_iszGroupMembers[2]", "Group02", STRING);
			DefineKeyField("m_iszGroupMembers[3]", "Group03", STRING);
			DefineKeyField("m_iszGroupMembers[4]", "Group04", STRING);
			DefineKeyField("m_iszGroupMembers[5]", "Group05", STRING);
			DefineKeyField("m_iszGroupMembers[6]", "Group06", STRING);
			DefineKeyField("m_iszGroupMembers[7]", "Group07", STRING);
			DefineKeyField("m_iszGroupMembers[8]", "Group08", STRING);
			DefineKeyField("m_iszGroupMembers[9]", "Group09", STRING);
			DefineKeyField("m_iszGroupMembers[10]", "Group10", STRING);
			DefineKeyField("m_iszGroupMembers[11]", "Group11", STRING);
			DefineKeyField("m_iszGroupMembers[12]", "Group12", STRING);
			DefineKeyField("m_iszGroupMembers[13]", "Group13", STRING);
			DefineKeyField("m_iszGroupMembers[14]", "Group14", STRING);
			DefineKeyField("m_iszGroupMembers[15]", "Group16", STRING);
			
			BeginDataMap("CPointServerCommand", "CPointEntity");
			LinkNamesToMap("point_servercommand");
			DefineInputFunc("Command", "InputCommand", STRING);

			{
				void DefineProxyRelayInputFunc(int i)
					=> DefineInputFunc($"OnProxyRelay{i}", $"InputProxyRelay{i}", STRING);
				
				void DefineProxyRelayOutput(int i)
					=> DefineOutput($"m_OnProxyRelay{i}", $"OnProxyRelay{i}");

				BeginDataMap("CFuncInstanceIoProxy", "CBaseEntity");
				LinkNamesToMap("func_instance_io_proxy");
				for (int i = 0; i < (Game == Game.PORTAL2 ? 19 : 16); i++) {
					DefineProxyRelayInputFunc(i+1);
					DefineProxyRelayOutput(i+1);
				}
			}
			
			BeginDataMap("CLogicBranchList", "CLogicalEntity");
			LinkNamesToMap("logic_branch_listener");
			DefineKeyField("m_nLogicBranchNames[0]", "Branch01", STRING);
			DefineKeyField("m_nLogicBranchNames[1]", "Branch02", STRING);
			DefineKeyField("m_nLogicBranchNames[2]", "Branch03", STRING);
			DefineKeyField("m_nLogicBranchNames[3]", "Branch04", STRING);
			DefineKeyField("m_nLogicBranchNames[4]", "Branch05", STRING);
			DefineKeyField("m_nLogicBranchNames[5]", "Branch06", STRING);
			DefineKeyField("m_nLogicBranchNames[6]", "Branch07", STRING);
			DefineKeyField("m_nLogicBranchNames[7]", "Branch08", STRING);
			DefineKeyField("m_nLogicBranchNames[8]", "Branch09", STRING);
			DefineKeyField("m_nLogicBranchNames[9]", "Branch10", STRING);
			DefineKeyField("m_nLogicBranchNames[10]", "Branch11", STRING);
			DefineKeyField("m_nLogicBranchNames[11]", "Branch12", STRING);
			DefineKeyField("m_nLogicBranchNames[12]", "Branch13", STRING);
			DefineKeyField("m_nLogicBranchNames[13]", "Branch14", STRING);
			DefineKeyField("m_nLogicBranchNames[14]", "Branch15", STRING);
			DefineKeyField("m_nLogicBranchNames[15]", "Branch16", STRING);
			DefineVector("m_LogicBranchList", EHANDLE);
			DefineField("m_eLastState", INTEGER);
			DefineInputFunc("Test", "InputTest", INPUT);
			DefineInputFunc("_OnLogicBranchChanged", "Input_OnLogicBranchChanged", INPUT);
			DefineInputFunc("_OnLogicBranchRemoved", "Input_OnLogicBranchRemoved", INPUT);
			DefineOutput("m_OnAllTrue", "OnAllTrue");
			DefineOutput("m_OnAllFalse", "OnAllFalse");
			DefineOutput("m_OnMixed", "OnMixed");
			
			BeginDataMap("CRuleEntity", "CBaseEntity");
			DefineKeyField("m_iszMaster", "master", STRING);
			
			BeginDataMap("CRulePointEntity", "CRuleEntity");
			DefineField("m_Score", INTEGER);
				
			BeginDataMap("CGameText", "CRulePointEntity");
			LinkNamesToMap("game_text");
			DefineKeyField("m_iszMessage", "message", STRING);
			DefineKeyField("m_textParms.channel", "channel", INTEGER);
			DefineKeyField("m_textParms.x", "x", FLOAT);
			DefineKeyField("m_textParms.y", "y", FLOAT);
			DefineKeyField("m_textParms.effect", "effect", INTEGER);
			DefineKeyField("m_textParms.fadeinTime", "fadein", FLOAT);
			DefineKeyField("m_textParms.fadeoutTime", "fadeout", FLOAT);
			DefineKeyField("m_textParms.holdTime", "holdtime", FLOAT);
			DefineKeyField("m_textParms.fxTime", "fxtime", FLOAT);
			DefineField("m_textParms", BYTE, 40); // todo this is actually a hudtextparms_t struct
			DefineInputFunc("Display", "InputDisplay", VOID);
			if (Game == Game.PORTAL2)
				DefineInputFunc("SetText", "InputSetText", STRING);
		}
	}
}