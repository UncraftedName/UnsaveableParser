// ReSharper disable All

using System;

namespace SaveParser.Parser {
	public static class Constants {
		public const int MaxEdictBits = 11;
		public const int MaxEdicts = 1 << MaxEdictBits;
		public const int NetworkedEHandleSerialNumBits = 10;
		public const int NetworkedEHandleBits = MaxEdictBits + NetworkedEHandleSerialNumBits;
		public const uint NullEHandle = (1 << NetworkedEHandleBits) - 1; // INVALID_NETWORKED_EHANDLE_VALUE
		public const float FLT_MAX = 3.402823466e+38f;
		public const float ZERO_TIME = FLT_MAX * 0.5f;
		public const float INVALID_TIME = FLT_MAX * -1.0f;
		public const int MaxClients = 1; // depends on the game
		public const int TICK_NEVER_THINK = -1;
		public const int TICK_NEVER_THINK_ENCODE = int.MaxValue - 3;
		public const int MAXSTUDIOFLEXCTRL = 96;
		public const int MAX_AMMO_SLOTS = 32;
		public const int MAX_WEAPONS = 48;
		public const int MAX_NETWORKID_LENGTH = 64;
		public const int MAX_ITEMS = 5;
		public const int CSUITPLAYLIST = 4;
		public const int CSUITNOREPEAT = 32;
		public const int MAX_PLAYER_NAME_LENGTH = 32;
		public const int MAX_VIEWMODELS = 2;
		public const int CDMG_TIMEBASED = 8;
		public const int MAX_PLACE_NAME_LENGTH = 18;
		public const int MAX_AREA_STATE_BYTES = 32;
		public const int MAX_AREA_PORTAL_STATE_BYTES = 24;
		public const int NUM_AUDIO_LOCAL_SOUNDS = 8;
		public const int NUM_POSEPAREMETERS = 24;
		public const int NUM_BONECTRLS = 4;
		public const int MAX_WORLD_SOUNDS_SP = 64;
		public const int MATERIAL_MODIFY_STRING_SIZE = 255;
		public const int SNPCINT_NUM_PHASES = 3;
		public const int MAX_SCENE_FILENAME = 128;
		public const int kMAXCONTROLPOINTS = 63;
		public const int cchMapNameMost = 32;
		public const int VEHICLE_MAX_AXLE_COUNT = 4;
		public const int VEHICLE_MAX_WHEEL_COUNT = 2 * VEHICLE_MAX_AXLE_COUNT;
		public const int VEHICLE_MAX_GEAR_COUNT = 6;
		public const int SECURITY_CAMERA_NUM_ROPES = 2;
		public const int MAX_BEAM_ENTS = 10;
		public const int PORTAL_FLOOR_TURRET_NUM_ROPES = 4;
		public const int MAX_LOGIC_CASES = 16;
		public const int VS_NUM_SOUNDS = 9;
		public const int SS_NUM_STATES = 20;
	}
}