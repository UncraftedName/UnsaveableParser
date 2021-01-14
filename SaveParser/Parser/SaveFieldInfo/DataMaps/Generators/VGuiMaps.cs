// ReSharper disable All

using SaveParser.Parser.SaveFieldInfo.DataMaps.GeneratorProcessing;
using SaveParser.Utils.ByteStreams;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo.DataMaps.Generators {
	
	public class VGuiMaps : DataMapInfoGenerator {

		private static ParsedSaveField VGuiScreenStringOps(TypeDesc desc, SaveInfo info, ref ByteStreamReader bsr)
			=> new ParsedSaveField<string>(bsr.ReadStringOfLength(bsr.ReadSInt()), desc);
		

		protected override void GenerateDataMaps() {
			BeginDataMap("CVguiScreen", "CBaseEntity");
			LinkNamesToMap("vgui_screen", "vgui_screen_team", "CVGuiScreen"); // not sure if the last one is a typo
			DefineCustomField("m_nPanelName", VGuiScreenStringOps);
			DefineField("m_nAttachmentIndex", INTEGER);
			DefineField("m_fScreenFlags", INTEGER);
			DefineKeyField("m_flWidth", "width", FLOAT);
			DefineKeyField("m_flHeight", "height", FLOAT);
			DefineKeyField("m_strOverlayMaterial", "overlaymaterial", STRING);
			DefineField("m_hPlayerOwner", EHANDLE);
			DefineInputFunc("SetActive", "InputSetActive", VOID);
			DefineInputFunc("SetInactive", "InputSetInactive", VOID);
			
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
			
			if (Game == Game.PORTAL2) {
				BeginDataMap("CMovieDisplay", "CBaseEntity");
				LinkNamesToMap("vgui_movie_display");
				DefineField("m_bEnabled", BOOLEAN);
				DefineKeyField("m_szDisplayText", "displaytext", CHARACTER, 128);
				DefineField("m_szMovieFilename", CHARACTER, 128);
				DefineKeyField("m_strMovieFilename", "moviefilename", STRING);
				DefineField("m_szGroupName", CHARACTER, 128);
				DefineKeyField("m_strGroupName", "groupname", STRING); // Screens of the same group name will play the same movie at the same time
				DefineKeyField("m_iScreenWidth", "width", INTEGER);
				DefineKeyField("m_iScreenHeight", "height", INTEGER);
				DefineKeyField("m_bLooping", "looping", BOOLEAN);
				DefineField("m_bDoFullTransmit", BOOLEAN);
				DefineField("m_hScreen", EHANDLE);
				DefineInputFunc("Disable", "InputDisable", VOID);
				DefineInputFunc("Enable", "InputEnable", VOID);
				DefineInputFunc("SetDisplayText", "InputSetDisplayText", STRING);
				DefineField("m_bStretchToFill", BOOLEAN);
				DefineField("m_bForcedSlave", BOOLEAN);
				DefineField("m_flUMax", FLOAT);
				DefineField("m_flVMax", FLOAT);
			}
		}
	}
}