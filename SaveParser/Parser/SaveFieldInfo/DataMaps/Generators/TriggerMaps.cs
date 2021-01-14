// ReSharper disable All

using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public class TriggerMaps : DataMapInfoGenerator {
		
		public const int cchMapNameMost = 32;
		
		
		protected override void GenerateDataMaps() {
			BeginDataMap("CTriggerGravity", "CBaseTrigger");
			LinkNamesToMap("trigger_gravity");
			DefineFunction("GravityTouch");

			DataMapProxy("CTriggerOnce", "CTriggerMultiple");
			LinkNamesToMap("trigger_once");
			
			BeginDataMap("CTriggerLook", "CTriggerOnce");
			LinkNamesToMap("trigger_look");
			DefineField("m_hLookTarget", EHANDLE);
			DefineField("m_flLookTimeTotal", FLOAT);
			DefineField("m_flLookTimeLast", TIME);
			DefineKeyField("m_flTimeoutDuration", "timeout", FLOAT);
			DefineField("m_bTimeoutFired", BOOLEAN);
			DefineField("m_hActivator", EHANDLE);
			DefineOutput("m_OnTimeout", "OnTimeout");
			DefineFunction("TimeoutThink");
			DefineInput("m_flFieldOfView", "FieldOfView", FLOAT);
			DefineInput("m_flLookTime", "LookTime", FLOAT);
			
			BeginDataMap("CChangeLevel", "CBaseTrigger");
			LinkNamesToMap("trigger_changelevel");
			DefineField("m_szMapName", CHARACTER, cchMapNameMost);
			DefineField("m_szLandmarkName", CHARACTER, cchMapNameMost);
			DefineFunction("TouchChangeLevel");
			//DefineINPUTFUNC("ChangeLevel", VOID);
			DefineOutput("m_OnChangeLevel", "OnChangeLevel");
			
			DataMapProxy("CTriggerVolume", "CPointEntity");
			LinkNamesToMap("trigger_transition");
			
			BeginDataMap("CTriggerMultiple", "CBaseTrigger");
			LinkNamesToMap("trigger_multiple");
			DefineFunction("MultiTouch");
			DefineFunction("MultiWaitOver");
			DefineOutput("m_OnTrigger", "OnTrigger");
			
			BeginDataMap("CTriggerPortalCleanser", "CBaseTrigger");
			LinkNamesToMap("trigger_portal_cleanser");
			DefineOutput("m_OnDissolve", "OnDissolve");
			DefineOutput("m_OnFizzle", "OnFizzle");
			DefineOutput("m_OnDissolveBox", "OnDissolveBox");
			if (Game == Game.PORTAL2) {
				DefineField("m_bVisible", BOOLEAN);
				DefineField("m_bUseScanline", BOOLEAN);
				DefineField("m_bPlayersPassTriggerFilters", BOOLEAN);
			}
			
			BeginDataMap("CBaseTrigger", "CBaseToggle");
			DefineKeyField("m_iFilterName", "filtername", STRING);
			DefineField("m_hFilter", EHANDLE);
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineVector("m_hTouchingEntities", EHANDLE);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			DefineInputFunc("TouchTest", "InputTouchTest", VOID);
			DefineInputFunc("StartTouch", "InputStartTouch", VOID);
			DefineInputFunc("EndTouch", "InputEndTouch", VOID);
			DefineOutput("m_OnStartTouch", "OnStartTouch");
			DefineOutput("m_OnStartTouchAll", "OnStartTouchAll");
			DefineOutput("m_OnEndTouch", "OnEndTouch");
			DefineOutput("m_OnEndTouchAll", "OnEndTouchAll");
			DefineOutput("m_OnTouching", "OnTouching");
			DefineOutput("m_OnNotTouching", "OnNotTouching");
			
			BeginDataMap("CBaseVPhysicsTrigger", "CBaseEntity"); // uses vphysics to compute touch events, doesn't do per-frame Touch call
			DefineKeyField("m_bDisabled", "StartDisabled", BOOLEAN);
			DefineKeyField("m_iFilterName", "filtername", STRING);
			DefineField("m_hFilter", EHANDLE);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineInputFunc("Toggle", "InputToggle", VOID);
			
			BeginDataMap("CTriggerVPhysicsMotion", "CBaseVPhysicsTrigger");
			LinkNamesToMap("trigger_vphysics_motion");
			DefinePhysPtr("m_pController");
			if (!GenInfo.IsXBox)
				DefineEmbeddedField("m_ParticleTrail", "EntityParticleTrailInfo_t");
			DefineInput("m_gravityScale", "SetGravityScale", FLOAT);
			DefineInput("m_addAirDensity", "SetAdditionalAirDensity", FLOAT);
			DefineInput("m_linearLimit", "SetVelocityLimit", FLOAT);
			DefineInput("m_linearLimitDelta", "SetVelocityLimitDelta", FLOAT);
			DefineField("m_linearLimitTime", FLOAT);
			DefineField("m_linearLimitStart", TIME);
			DefineField("m_linearLimitStartTime", TIME);
			DefineInput("m_linearScale", "SetVelocityScale", FLOAT);
			DefineInput("m_angularLimit", "SetAngVelocityLimit", FLOAT);
			DefineInput("m_angularScale", "SetAngVelocityScale", FLOAT);
			DefineInput("m_linearForce", "SetLinearForce", FLOAT);
			DefineInput("m_linearForceAngles", "SetLinearForceAngles", VECTOR);
			DefineInputFunc("SetVelocityLimitTime", "InputSetVelocityLimitTime", STRING);
			
			BeginDataMap("CTriggerHurt", "CBaseTrigger");
			LinkNamesToMap("trigger_hurt");
			DefineFunction("RadiationThink");
			DefineFunction("HurtThink");
			DefineField("m_flOriginalDamage", FLOAT);
			DefineInputAndKeyField("m_flDamage", "damage", "SetDamage", FLOAT);
			DefineKeyField("m_flDamageCap", "damagecap", FLOAT);
			DefineKeyField("m_bitsDamageInflict", "damagetype", INTEGER);
			DefineKeyField("m_damageModel", "damagemodel", INTEGER);
			DefineKeyField("m_bNoDmgForce", "nodmgforce", BOOLEAN);
			DefineField("m_flLastDmgTime", TIME);
			DefineField("m_flDmgResetTime", TIME);
			DefineVector("m_hurtEntities", EHANDLE);
			DefineOutput("m_OnHurt", "OnHurt");
			DefineOutput("m_OnHurtPlayer", "OnHurtPlayer");
			
			BeginDataMap("CTriggerPush", "CBaseTrigger");
			LinkNamesToMap("trigger_push");
			DefineKeyField("m_vecPushDir", "pushdir", VECTOR);
			DefineKeyField("m_flAlternateTicksFix", "alternateticksfix", FLOAT);
			
			BeginDataMap("CTriggerSave", "CBaseTrigger");
			LinkNamesToMap("trigger_autosave");
			DefineKeyField("m_bForceNewLevelUnit", "NewLevelUnit", BOOLEAN);
			DefineKeyField("m_minHitPoints", "MinimumHitPoints", INTEGER);
			DefineKeyField("m_fDangerousTimer", "DangerousTimer", FLOAT);
			
			BeginDataMap("CTriggerRemove", "CBaseTrigger");
			LinkNamesToMap("trigger_remove");
			DefineOutput("m_OnRemove", "OnRemove");
			
			BeginDataMap("CTriggerCamera", "CBaseEntity");
			LinkNamesToMap("point_viewcontrol");
			DefineField("m_hPlayer", EHANDLE);
			DefineField("m_hTarget", EHANDLE);
			DefineField("m_pPath", CLASSPTR);
			DefineField("m_sPath", STRING);
			DefineField("m_flWait", FLOAT);
			DefineField("m_flReturnTime", TIME);
			DefineField("m_flStopTime", TIME);
			DefineField("m_moveDistance", FLOAT);
			DefineField("m_targetSpeed", FLOAT);
			DefineField("m_initialSpeed", FLOAT);
			DefineField("m_acceleration", FLOAT);
			DefineField("m_deceleration", FLOAT);
			DefineField("m_state", INTEGER);
			DefineField("m_vecMoveDir", VECTOR);
			DefineKeyField("m_iszTargetAttachment", "targetattachment", STRING);
			DefineField("m_iAttachmentIndex", INTEGER);
			DefineField("m_bSnapToGoal", BOOLEAN);
			if (GenInfo.IsDefHl2Episodic) {
				DefineKeyField("m_bInterpolatePosition", "interpolatepositiontoplayer", BOOLEAN);
				DefineField("m_vStartPos", VECTOR);
				DefineField("m_vEndPos", VECTOR);
				DefineField("m_flInterpStartTime", TIME);
			}
			DefineField("m_nPlayerButtons", INTEGER);
			DefineField("m_nOldTakeDamage", INTEGER);
			DefineInputFunc("Enable", "InputEnable", VOID);
			DefineInputFunc("Disable", "InputDisable", VOID);
			DefineFunction("FollowTarget");
			DefineOutput("m_OnEndFollow", "OnEndFollow");
			
			BeginDataMap("CTriggerTeleport", "CBaseTrigger");
			LinkNamesToMap("trigger_teleport");
			DefineKeyField("m_iLandmark", "landmark", STRING);
			
			BeginDataMap("CPointTeleport", "CBaseEntity");
			LinkNamesToMap("point_teleport");
			DefineField("m_vSaveOrigin", VECTOR);
			DefineField("m_vSaveAngles", VECTOR);
			DefineInputFunc("Teleport", "InputTeleport", VOID);
			if (Game == Game.PORTAL2) {
				DefineInputFunc("TeleportEntity", "InputTeleportEntity", STRING);
				DefineInputFunc("TeleportToCurrentPos", "InputTeleportToCurrentPos", VOID);
			}
		}
	}
}