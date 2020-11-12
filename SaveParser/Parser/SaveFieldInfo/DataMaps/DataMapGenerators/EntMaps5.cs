// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public class EntMaps5 : DataMapGenerator {
		
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
			DefineField("m_uchShuffleCaseMap", CHARACTER, Constants.MAX_LOGIC_CASES);
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
		}
	}
}