using System;
using System.Collections.Generic;
using System.Numerics;
using SaveParser.Parser.SaveFieldInfo.DataMaps;
using SaveParser.Utils;
using SaveParser.Utils.BitStreams;
using static SaveParser.Parser.SaveFieldInfo.FieldType;

namespace SaveParser.Parser.SaveFieldInfo {
	
	public class TypeDesc {
		
		public readonly FieldType FieldType;
		public readonly string Name;
		public readonly string? InputName, OutputName, MapName;
		public readonly ushort NumElements;
		public readonly DescFlags Flags;

		internal readonly CustomReadFunc? CustomReadFunc;
		internal readonly object?[]? CustomParams; // anything special needed for the custom read to work
		public DataMap? EmbeddedMap; // set in global map creation

		private string? _typeString;
		public string TypeString => _typeString ??= EvaluateTypeString();


		// custom field constructor
		public TypeDesc(
			string name,
			DescFlags flags,
			CustomReadFunc? customReadFunc,
			object?[]? customParams = null,
			string? outputName = null)
		{
			Name = name;
			FieldType = CUSTOM;
			NumElements = 1;
			Flags = flags;
			CustomReadFunc = customReadFunc;
			CustomParams = customParams;
			OutputName = outputName;
		}


		// everything else
		public TypeDesc(
			string name,
			FieldType fieldType,
			DescFlags flags = DescFlags.FTYPEDESC_SAVE,
			ushort numElements = 1,
			string? inputName = null,
			string? outputName = null,
			string? mapName = null,
			CustomReadFunc? customReadFunc = null,
			object?[]? customParams = null)
		{
			FieldType = fieldType;
			Name = name;
			InputName = inputName;
			OutputName = outputName;
			MapName = mapName;
			NumElements = numElements;
			Flags = flags;
			CustomReadFunc = customReadFunc;
			CustomParams = customParams;
		}


		internal ParsedSaveField? InvokeCustomReadFunc(ref BitStreamReader bsr, SaveInfo info)
			=> CustomReadFunc!(this, info, ref bsr);
		
		
		public static Type GetNetTypeFromFieldType(FieldType fieldType) {
			return fieldType switch {
				BYTE                 => typeof(byte),
				FLOAT                => typeof(float),
				STRING               => typeof(string),
				VECTOR               => typeof(Vector3),
				QUATERNION           => typeof(Vector4),
				INTEGER              => typeof(int),
				BOOLEAN              => typeof(bool),
				SHORT                => typeof(short),
				CHARACTER            => typeof(CharArray),
				COLOR32              => typeof(Color32),
				EHANDLE              => typeof(Ehandle),
				EDICT                => typeof(Edict),
				POSITION_VECTOR      => typeof(Vector3),
				TIME                 => typeof(Time),
				TICK                 => typeof(Tick),
				MODELNAME            => typeof(ModelName),
				SOUNDNAME            => typeof(SoundName),
				VMATRIX              => typeof(VMatrix),
				VMATRIX_WORLDSPACE   => typeof(VMatrix),
				MATRIX3X4_WORLDSPACE => typeof(Matrix3X4),
				INTERVAL             => typeof(Interval),
				MODELINDEX           => typeof(ModelIndex),
				MATERIALINDEX        => typeof(MaterialIndex),
				VECTOR2D             => typeof(Vector2),
				CLASSPTR             => typeof(ClassPtr),
				FUNCTION             => typeof(Func),
				_ => throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null)
			};
		}


		public override string ToString() => $"{TypeString} {Name}";
		
		private static readonly Dictionary<FieldType, string> TypeNames = new Dictionary<FieldType, string>();


		private string EvaluateTypeString() {
			if (!TypeNames.TryGetValue(FieldType, out string? prefix)) {
				try {
					prefix = FieldType switch {
						CHARACTER => "char",
						FLOAT     => "float",
						BOOLEAN   => "bool",
						_ => GetNetTypeFromFieldType(FieldType).Name.ToLower()
					};
				} catch (Exception) {
					prefix = FieldType.ToString().ToLower();
				}
				TypeNames[FieldType] = prefix;
			}
			return NumElements > 1 ? $"{prefix}[{NumElements}]" : prefix;
		}


		public static int FieldInstByteSize(FieldType fieldType) {
			return fieldType switch {
				BYTE => 1,
				FLOAT => 4,
				VECTOR => 12,
				QUATERNION => 16,
				INTEGER => 4,
				BOOLEAN => 1,
				SHORT => 2,
				CHARACTER => 1,
				COLOR32 => 4,
				CLASSPTR => 4,
				EHANDLE => 4,
				EDICT => 4,
				POSITION_VECTOR => 12,
				TIME => 4,
				TICK => 4,
				INPUT => 4,
				FUNCTION => 4,
				VMATRIX => 64,
				VMATRIX_WORLDSPACE => 64,
				MATRIX3X4_WORLDSPACE => 48,
				INTERVAL => 8,
				MODELINDEX => 4,
				MATERIALINDEX => 4,
				VECTOR2D => 8,
				_ => throw new ArgumentOutOfRangeException(nameof(fieldType), fieldType, null)
			};
		}
	}
	
	
	// ReSharper disable InconsistentNaming
	// ReSharper disable IdentifierTypo
	// ReSharper disable CommentTypo
	
	
	public enum FieldType {
		BYTE = -1, // same as char, but I actually care about the specific data type
		
		VOID = 0,        // No type or value
		FLOAT,           // Any floating point value
		STRING,          // A string ID (return from ALLOC_STRING)
		VECTOR,          // Any vector, QAngle, or AngularImpulse
		QUATERNION,      // A quaternion
		INTEGER,         // Any integer or enum
		BOOLEAN,         // boolean, implemented as an int, I may use this as a hint for compression
		SHORT,           // 2 byte integer
		CHARACTER,       // a byte
		COLOR32,         // 8-bit per channel r,g,b,a (32bit color)
		EMBEDDED,        // an embedded object with a datadesc, recursively traverse and embedded class/structure based on an additional typedescription
		CUSTOM,          // special type that contains function pointers to it's read/write/parse functions
		
		CLASSPTR,        // CBaseEntity *
		EHANDLE,         // Entity handle
		EDICT,           // edict_t *
		
		POSITION_VECTOR, // A world coordinate (these are fixed up across level transitions automagically)
		TIME,            // a floating point time (these are fixed up automatically too!)
		TICK,            // an integer tick count( fixed up similarly to time)
		MODELNAME,       // Engine string that is a model name (needs precache)
		SOUNDNAME,       // Engine string that is a sound name (needs precache)
		
		INPUT,           // a list of inputted data fields (all derived from CMultiInputVar)
		FUNCTION,        // A class function pointer (Think, Use, etc)
		
		VMATRIX,         // a vmatrix (output coords are NOT worldspace)
		
		// NOTE: Use float arrays for local transformations that don't need to be fixed up.
		VMATRIX_WORLDSPACE,   // A VMatrix that maps some local space to world space (translation is fixed up on level transitions)
		MATRIX3X4_WORLDSPACE, // matrix3x4_t that maps some local space to world space (translation is fixed up on level transitions)
		
		INTERVAL,      // a start and range floating point interval ( e.g., 3.2->3.6 == 3.2 and 0.4 )
		MODELINDEX,    // a model index
		MATERIALINDEX, // a material index (using the material precache string table)
		
		VECTOR2D,      // 2 floats
	}


	[Flags]
	public enum DescFlags {
		FTYPEDESC_NONE,
		FTYPEDESC_GLOBAL            = 0x0001,        // This field is masked for global entity save/restore
		FTYPEDESC_SAVE              = 0x0002,        // This field is saved to disk
		FTYPEDESC_KEY               = 0x0004,        // This field can be requested and written to by string name at load time
		FTYPEDESC_INPUT             = 0x0008,        // This field can be written to by string name at run time, and a function called
		FTYPEDESC_OUTPUT            = 0x0010,        // This field propogates it's value to all targets whenever it changes
		FTYPEDESC_FUNCTIONTABLE     = 0x0020,        // This is a table entry for a member function pointer
		FTYPEDESC_PTR               = 0x0040,        // This field is a pointer, not an embedded object
		FTYPEDESC_OVERRIDE          = 0x0080,        // The field is an override for one in a base class (only used by prediction system for now)

		// Flags used by other systems (e.g., prediction system)
		FTYPEDESC_INSENDTABLE       = 0x0100,        // This field is present in a network SendTable
		FTYPEDESC_PRIVATE           = 0x0200,        // The field is local to the client or server only (not referenced by prediction code and not replicated by networking)
		FTYPEDESC_NOERRORCHECK      = 0x0400,        // The field is part of the prediction typedescription, but doesn't get compared when checking for errors

		FTYPEDESC_MODELINDEX        = 0x0800,        // The field is a model index (used for debugging output)

		FTYPEDESC_INDEX             = 0x1000,        // The field is an index into file data, used for byteswapping. 

		// These flags apply to C_BasePlayer derived objects only
		FTYPEDESC_VIEW_OTHER_PLAYER = 0x2000,        // By default you can only view fields on the local player (yourself), 
		                                             // but if this is set, then we allow you to see fields on other players
		FTYPEDESC_VIEW_OWN_TEAM     = 0x4000,        // Only show this data if the player is on the same team as the local player
		FTYPEDESC_VIEW_NEVER        = 0x8000         // Never show this field to anyone, even the local player (unusual)
	}
}