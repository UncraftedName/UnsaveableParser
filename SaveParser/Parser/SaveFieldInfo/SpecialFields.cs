using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using SaveParser.Utils;

namespace SaveParser.Parser.SaveFieldInfo {
	
	public readonly struct Func {

		public readonly string Str;
		public static implicit operator string(Func mn) => mn.Str;
		public static explicit operator Func(string s) => new Func(s);
		
		public Func(string s) {
			Str = s;
		}
		
		public override string ToString() {
			return Str;
		}
	}

	public readonly struct ClassPtr {
		public readonly int Index;
		public static implicit operator int(ClassPtr t) => t.Index;
		public static explicit operator ClassPtr(int i) => new ClassPtr(i);
		
		public ClassPtr(int index) {
			Index = index;
		}
		
		public override string ToString() {
			return $"index={Index}";
		}
	}
	
	public readonly struct Edict {
		public readonly int Index;
		public static implicit operator int(Edict t) => t.Index;
		public static explicit operator Edict(int i) => new Edict(i);
		
		public Edict(int index) {
			Index = index;
		}
		
		public override string ToString() {
			return $"index={Index}";
		}
	}
	
	public readonly struct Ehandle {
		public readonly int Index;
		public static implicit operator int(Ehandle t) => t.Index;
		public static explicit operator Ehandle(int i) => new Ehandle(i);
		
		public Ehandle(int index) {
			Index = index;
		}


		public static bool operator ==(Ehandle e1, Ehandle e2) {
			return e1.Index == e2.Index;
		}


		public static bool operator !=(Ehandle e1, Ehandle e2) {
			return !(e1 == e2);
		}


		public override string ToString() {
			return $"index={Index}";
		}
	}

	public readonly struct Tick {
		
		public readonly int Val;
		public static implicit operator int(Tick t) => t.Val;
		public static explicit operator Tick(int i) => new Tick(i);
		
		public Tick(int val) {
			Val = val;
		}
		
		public override string ToString() {
			return Val.ToString();
		}
	}


	public readonly struct Time {
		
		public readonly float Val;
		public static implicit operator float(Time t) => t.Val;
		public static explicit operator Time(float f) => new Time(f);
		
		public Time(float val) {
			Val = val;
		}
		
		public override string ToString() {
			return Val.ToString(CultureInfo.InvariantCulture);
		}
	}


	public readonly struct ModelName { // todo figure out what the hell stuff like *7 means

		public readonly string Str;
		public static implicit operator string(ModelName mn) => mn.Str;
		public static explicit operator ModelName(string s) => new ModelName(s);
		
		public ModelName(string s) {
			Str = s;
		}
		
		public override string ToString() {
			return Str;
		}
	}
	
	
	public readonly struct SoundName {

		public readonly string Str;
		public static implicit operator string(SoundName mn) => mn.Str;
		public static explicit operator SoundName(string s) => new SoundName(s);
		
		public SoundName(string s) {
			Str = s;
		}
		
		public override string ToString() {
			return Str;
		}
	}
	
	
	public readonly struct ModelIndex {
		
		public readonly string Str;
		public static implicit operator string(ModelIndex mn) => mn.Str;
		public static explicit operator ModelIndex(string s) => new ModelIndex(s);
		
		public ModelIndex(string s) {
			Str = s;
		}
		
		public override string ToString() {
			return Str;
		}
	}
	
	
	public readonly struct MaterialIndex { // I want to strangle valve
		
		public readonly int Val;
		public static implicit operator int(MaterialIndex t) => t.Val;
		public static explicit operator MaterialIndex(int i) => new MaterialIndex(i);
		
		public MaterialIndex(int val) {
			Val = val;
		}
		
		public override string ToString() {
			return Val.ToString();
		}
	}


	public readonly struct MaterialIndexStr {
		public readonly string Str;
		public static implicit operator string(MaterialIndexStr mn) => mn.Str;
		public static explicit operator MaterialIndexStr(string s) => new MaterialIndexStr(s);
		
		public MaterialIndexStr(string s) {
			Str = s;
		}
		
		public override string ToString() {
			return Str;
		}
	}
	
	
	public class VMatrix {
		
		public readonly float[,] Mat;
		public static implicit operator float[,](VMatrix mat) => mat.Mat;
		public static explicit operator VMatrix(float[,] mat) => new VMatrix(mat);
		
		public VMatrix(float[,] mat) {
			if (mat.Rank != 2 || mat.GetLength(0) != 4 || mat.GetLength(1) != 4)
				throw new ConstraintException($"expected float[4,4], got: {mat.GetType()}");
			Mat = mat;
		}


		public void ApplyTranslation(in Vector3 vec) {
			AssertMatSize();
			Mat[0,3] += vec.X;
			Mat[1,3] += vec.Y;
			Mat[2,3] += vec.Z;
		}
		
		
		private void AssertMatSize() {
			Debug.Assert(Mat.GetLength(0) == 4 && Mat.GetLength(1) == 4);
		}


		// todo some better way?
		public override string ToString() {
			return Enumerable.Range(0, 4).Select(i => Enumerable.Range(0, 4).Select(j => Mat[i, j]).SequenceToString()).SequenceToString();
		}
	}


	public class Matrix3X4 {
		
		public readonly float[,] Mat; // x = forward, y = left, z = up
		public static implicit operator float[,](Matrix3X4 mat) => mat.Mat;
		public static explicit operator Matrix3X4(float[,] mat) => new Matrix3X4(mat);


		public Matrix3X4(float[,] mat) {
			if (mat.Rank != 2 || mat.GetLength(0) != 3 || mat.GetLength(1) != 4)
				throw new ConstraintException($"expected float[3,4], got: {mat.GetType()}");
			Mat = mat;
		}


		public void GetColumn(int column, out Vector3 vec) { // todo check
			AssertMatSize();
			vec.X = Mat[0,column];
			vec.Y = Mat[1,column];
			vec.Z = Mat[2,column];
		}


		public void SetColumn(int column, in Vector3 vec) {
			AssertMatSize();
			Mat[0,column] = vec.X;
			Mat[1,column] = vec.Y;
			Mat[2,column] = vec.Z;
		}


		private void AssertMatSize() {
			Debug.Assert(Mat.GetLength(0) == 3 && Mat.GetLength(1) == 4);
		}
		
		
		// todo some better way?
		public override string ToString() {
			return Enumerable.Range(0, 3).Select(i => Enumerable.Range(0, 4).Select(j => Mat[i, j]).SequenceToString()).SequenceToString();
		}
	}


	public struct Color32 { // todo check

		public int Val;

		public static implicit operator int(Color32 c) => c.Val;
		public static explicit operator Color32(int i) => new Color32(i);

		public byte R {
			get => (byte)(Val >> 24);
			set => Val = (value << 24) | (Val & ~(0xFF << 24));
		}
		public byte G {
			get => (byte)(Val >> 16);
			set => Val = (value << 16) | (Val & ~(0xFF << 16));
		}
		public byte B {
			get => (byte)(Val >> 8);
			set => Val = (value << 8) | (Val & ~(0xFF << 8));
		}
		public byte A {
			get => (byte)Val;
			set => Val = value | (Val & ~0xFF);
		}

		public Color32(int val) {
			Val = val;
		}


		public override string ToString() {
			return $"(RGBA) {R} {G} {B} {A}";
		}
	}


	public struct Interval {
		
		public float Start, Range;
		
		public Interval(float start, float range) {
			Start = start;
			Range = range;
		}
		
		public override string ToString() {
			return $"[{Start} - {Start + Range}]";
		}
	}
}