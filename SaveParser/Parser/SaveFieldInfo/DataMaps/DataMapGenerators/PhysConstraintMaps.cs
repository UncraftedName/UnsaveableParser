// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	public class PhysConstraintMaps : DataMapGenerator {
		
		protected override void CreateDataMaps() {
			BeginDataMap("CPhysConstraint", "CLogicalEntity");
			DefinePhysPtr("m_pConstraint");
			DefineKeyField("m_nameSystem", "constraintsystem", STRING);
			DefineKeyField("m_nameAttach1", "attach1", STRING);
			DefineKeyField("m_nameAttach2", "attach2", STRING);
			DefineKeyField("m_breakSound", "breaksound", SOUNDNAME);
			DefineKeyField("m_forceLimit", "forcelimit", FLOAT);
			DefineKeyField("m_torqueLimit", "torquelimit", FLOAT);
			DefineKeyField("m_minTeleportDistance", "teleportfollowdistance", FLOAT);
			DefineOutput("m_OnBreak", "OnBreak");
			DefineInputFunc("Break", "InputBreak", VOID);
			DefineInputFunc("ConstraintBroken", "InputOnBreak", VOID);
			DefineInputFunc("TurnOn", "InputTurnOn", VOID);
			DefineInputFunc("TurnOff", "InputTurnOff", VOID);
			
			DataMapProxy("CPhysBallSocket", "CPhysConstraint");
			LinkNamesToMap("phys_ballsocket");
			
			BeginDataMap("CPhysSlideContraint", "CPhysConstraint");
			LinkNamesToMap("phys_slideconstraint");
			DefineKeyField("m_axisEnd", "slideaxis", POSITION_VECTOR);
			DefineKeyField("m_slideFriction", "slidefriction", FLOAT);
			DefineKeyField("m_systemLoadScale", "systemloadscale", FLOAT);
			DefineInputFunc("SetVelocity", "InputSetVelocity", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_keyPoints[SimpleConstraintSoundProfile::kMIN_THRESHOLD]", "minSoundThreshold", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_keyPoints[SimpleConstraintSoundProfile::kMIN_FULL]", "maxSoundThreshold", FLOAT);
			DefineKeyField("m_soundInfo.m_iszTravelSoundFwd", "slidesoundfwd", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszTravelSoundBack", "slidesoundback", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[0]", "reversalsoundSmall", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[1]", "reversalsoundMedium", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_iszReversalSounds[2]", "reversalsoundLarge", SOUNDNAME);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[0]", "reversalsoundthresholdSmall", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[1]", "reversalsoundthresholdMedium", FLOAT);
			DefineKeyField("m_soundInfo.m_soundProfile.m_reversalSoundThresholds[2]", "reversalsoundthresholdLarge", FLOAT);
			DefineThinkFunc("SoundThink");
			
			BeginDataMap("CPhysHinge", "CPhysConstraint");
		}
	}
}