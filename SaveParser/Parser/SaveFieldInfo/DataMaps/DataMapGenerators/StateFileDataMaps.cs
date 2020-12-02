// ReSharper disable All
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.DataMapGenerators {
	
	internal class StateFileDataMaps : DataMapGenerator {
		
		protected override void CreateDataMaps() {
			BeginDataMap("GameHeader");
			DefineField("mapName", CHARACTER, 32);
			DefineField("comment", CHARACTER, 80);
			DefineField("mapCount", INTEGER, 32);
			DefineField("originMapName", CHARACTER, 32);
			DefineField("landmark", CHARACTER, 256);
			
			BeginDataMap("CGlobalState");
			LinkNamesToMap("GLOBAL");
			DefineEmbeddedVector("m_list", "globalentity_t");
			
			BeginDataMap("globalentity_t");
			DefineField("name", STRING); // todo also adds to a special symbol table?
			DefineField("levelName", STRING);
			DefineField("state", INTEGER);
			DefineField("counter", INTEGER);

			BeginDataMap("entitytable_t");
			LinkNamesToMap("ETABLE");
			DefineField("id", INTEGER); // Ordinal ID of this entity (used for entity <--> pointer conversions)
			DefineField("edictindex", INTEGER); // saved for if the entity requires a certain edict number when restored (players, world)
			DefineField("saveentityindex", INTEGER); // the entity index the entity had at save time ( for fixing up client side entities )
			DefineField("location", INTEGER); // Offset from the base data of this entity
			DefineField("size", INTEGER);     // Byte size of this entity's data
			DefineField("flags", INTEGER); // This could be a short -- bit mask of transitions that this entity is in the PVS of
			DefineField("classname", STRING);
			DefineField("globalname", STRING);
			DefineField("landmarkModelSpace", VECTOR);
			DefineField("modelname", STRING);
			
			BeginDataMap("SAVE_HEADER");
			LinkNamesToMap("Save Header");
			DefineField("skillLevel", INTEGER);
			DefineField("connectionCount", INTEGER);
			DefineField("lightStyleCount", INTEGER);
			DefineField("mapVersion", INTEGER);
			if (Game == Game.PORTAL2)
				DefineField("time", TIME);
			else
				DefineField("time__USE_VCR_MODE", TIME); // I have no idea
			DefineField("mapName", CHARACTER, 32);
			DefineField("skyName", CHARACTER, 32);
			
			BeginDataMap("levellist_t");
			LinkNamesToMap("ADJACENCY");
			DefineField("mapName", CHARACTER, 32);
			DefineField("landmarkName", CHARACTER, 32);
			DefineField("pentLandmark", EDICT);
			DefineField("vecLandmarkOrigin", VECTOR);
			
			BeginDataMap("SAVELIGHTSTYLE");
			LinkNamesToMap("LIGHTSTYLE");
			DefineField("index", INTEGER);
			DefineField("style", CHARACTER, 64);
			
			BeginDataMap("decallist_t");
			LinkNamesToMap("DECALLIST");
			DefineField("position", VECTOR);
			DefineField("name", CHARACTER, 128);
			DefineField("entityIndex", SHORT);
			DefineField("depth", CHARACTER); // byte
			DefineField("flags", CHARACTER); // byte
			DefineField("impactPlaneNormal", VECTOR); // for moving decals across level transitions if they hit similar geometry
			
			BeginDataMap("musicsave_t");
			LinkNamesToMap("MUSICLIST");
			DefineField("songname", CHARACTER, 128);
			DefineField("sampleposition", INTEGER);
			DefineField("master_volume", SHORT);
			
			BeginDataMap("SaveRestoreBlockHeader_t");
			DefineField("szName", CHARACTER, 32);
			DefineField("locHeader", INTEGER);
			DefineField("locBody", INTEGER);

			BeginDataMap("CCollisionProperty");
			DefineGlobalField("m_vecMins", VECTOR);
			DefineGlobalField("m_vecMaxs", VECTOR);
			DefineKeyField("m_nSolidType", "solid", BYTE);
			DefineField("m_usSolidFlags", SHORT);
			DefineField("m_nSurroundType", BYTE);
			DefineField("m_flRadius", FLOAT);
			DefineField("m_triggerBloat", BYTE);
			DefineField("m_vecSpecifiedSurroundingMins", VECTOR);
			DefineField("m_vecSpecifiedSurroundingMaxs", VECTOR);
			DefineField("m_vecSurroundingMins", VECTOR);
			DefineField("m_vecSurroundingMaxs", VECTOR);
			
			BeginDataMap("ResponseContext_t");
			DefineField("m_iszName", STRING);
			DefineField("m_iszValue", STRING);
			DefineField("m_fExpirationTime", FLOAT);
			
			BeginDataMap("CServerNetworkProperty");
			DefineGlobalField("m_hParent", EHANDLE);
			
			BeginDataMap("CSoundEnvelope");
			DefineField("m_current", FLOAT);
			DefineField("m_target", FLOAT);
			DefineField("m_rate", FLOAT);
			DefineField("m_forceupdate", BOOLEAN);
			
			BeginDataMap("CCopyRecipientFilter");
			DefineField("m_Flags", INTEGER);
			DefineVector("m_Recipients", INTEGER);
			
			BeginDataMap("CSoundPatch");
			DefineEmbeddedField("m_pitch", "CSoundEnvelope");
			DefineEmbeddedField("m_volume", "CSoundEnvelope");
			DefineField("m_soundlevel", INTEGER);
			DefineField("m_shutdownTime", TIME);
			DefineField("m_flLastTime", TIME);
			DefineField("m_iszSoundName", STRING);
			DefineField("m_iszSoundScriptName", STRING);
			DefineField("m_hEnt", EHANDLE);
			DefineField("m_entityChannel", INTEGER);
			DefineField("m_flags", INTEGER);
			DefineField("m_baseFlags", INTEGER);
			DefineField("m_isPlaying", INTEGER);
			DefineField("m_flScriptVolume", FLOAT);
			DefineEmbeddedField("m_Filter", "CCopyRecipientFilter");
			DefineField("m_flCloseCaptionDuration", FLOAT);
			if (Game == Game.PORTAL2) {
				DefineField("m_hSoundScriptHash", INTEGER);
				DefineField("m_nSoundEntryVersion", INTEGER);
			}

			BeginDataMap("SoundCommand_t");
			DefineField("m_time", TIME);
			DefineField("m_deltaTime", FLOAT);
			DefineField("m_command", INTEGER);
			DefineField("m_value", FLOAT);
			
			BeginDataMap("thinkfunc_t");
			DefineField("m_pfnThink", CLASSPTR); // todo check
			DefineField("m_iszContext", STRING);
			DefineField("m_nNextThinkTick", INTEGER);
			DefineField("m_nLastThinkTick", INTEGER);
			
			BeginDataMap("AIExtendedSaveHeader_t");
			DefineField("version", SHORT);
			DefineField("flags", INTEGER);
			DefineField("szSchedule", CHARACTER, 128);
			DefineField("scheduleCrc", INTEGER);
			DefineField("szIdealSchedule", CHARACTER, 128);
			DefineField("szFailSchedule", CHARACTER, 128);
			DefineField("szSequence", CHARACTER, 128);
			
			// this fires a single input of a target after a specific delay
			BeginDataMap("CEventAction");
			LinkNamesToMap("EntityOutput");
			DefineField("m_iTarget", STRING);
			DefineField("m_iTargetInput", STRING);
			DefineField("m_iParameter", STRING);
			DefineField("m_flDelay", FLOAT);
			DefineField("m_nTimesToFire", INTEGER);
			DefineField("m_iIDStamp", INTEGER);
			
			BeginDataMap("ConceptHistory_t");
			DefineField("timeSpoken", TIME); // relative to server time
		}
	}
}