using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib
{
    public class Vector3 : IEquatable<Vector3>
    {
        public double X, Y, Z;
        public double Volume { get { return X * Y * Z; } }
        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vector3()
        {

        }
        public Vector3(Vector2 value)
        {
            X = value.X;
            Y = 0;
            Z = value.Y;
        }
        public Vector3(double value)
        {
            X = value;
            Y = value;
            Z = value;
        }
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 Clone()
        {
            return (Vector3)this.MemberwiseClone();
        }
        /*public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }*/
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.X * b, a.Y * b, a.Z * b);
        }
        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }
        public bool Equals(Vector3 other)
        {
            return other.X == this.X && other.Y == this.Y && other.Z == this.Z;
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector3)
                return Equals((Vector3)obj);
            return false;
        }
        public double DistanceTo(Vector3 other)
        {
            return Math.Sqrt((other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y) + (other.Z - Z) * (other.Z - Z)); ;
        }
        public double DistanceSquaredTo(Vector3 other)
        {
            return (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y) + (other.Z - Z) * (other.Z - Z);
        }
        public Vector3 Truncate()
        {
            return new Vector3((int)X, (int)Y, (int)Z);
        }
        public Vector3 Round(int decimals)
        {
            return new Vector3(Math.Round(X, decimals), Math.Round(Y, decimals), Math.Round(Z, decimals));
        }
        public Vector3 Normalize()
        {
            Vector3 result = Clone();
            double len = result.Length;
            result.X /= len;
            result.Y /= len;
            result.Z /= len;
            return result;
        }
        public Vector3 FixToBlock()
        {
            Vector3 result = Clone();
            result.X = Math.Round(result.X + 0.5, 0);
            result.X -= 0.5;
            result.Z = Math.Round(result.Z + 0.5, 0);
            result.Z -= 0.5;
            return result;
        }
        public Vector3 Floor()
        {
            return new Vector3((int)X, (int)Y, (int)Z);
        }
        public override string ToString()
        {
            return "(" + Math.Round(X, 2) + "," + Math.Round(Y, 2) + "," + Math.Round(Z, 2) + ")";
        }

        public static Vector3 Zero
        {
            get { return new Vector3(0); }
        }
        public static Vector3 Up
        {
            get { return new Vector3(0, 1, 0); }
        }
        public static Vector3 Down
        {
            get { return new Vector3(0, -1, 0); }
        }
        public static Vector3 Left
        {
            get { return new Vector3(-1, 0, 0); }
        }
        public static Vector3 Right
        {
            get { return new Vector3(1, 0, 0); }
        }
        public static Vector3 Forward
        {
            get { return new Vector3(0, 0, 1); }
        }
        public static Vector3 Backward
        {
            get { return new Vector3(0, 0, -1); }
        }
        public static Vector3 North
        {
            get { return new Vector3(0, 0, -1); }
        }
        public static Vector3 South
        {
            get { return new Vector3(0, 0, 1); }
        }
        public static Vector3 East
        {
            get { return new Vector3(1, 0, 0); }
        }
        public static Vector3 West
        {
            get { return new Vector3(-1, 0, 0); }
        }
    }
    public class Vector2 : IEquatable<Vector2>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2()
        {
            X = Y = 0;
        }
        public Vector2(int value)
        {
            X = Y = value;
        }
        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Clone()
        {
            return (Vector2)this.MemberwiseClone();
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2 operator *(Vector2 a, int b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }
        public static Vector2 operator /(Vector2 a, int b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }
        public static Vector2 Zero
        {
            get { return new Vector2(0); }
        }
        public bool Equals(Vector2 other)
        {
            return other.X == this.X && other.Y == this.Y;
        }
        public override bool Equals(object obj)
        {
 	         if (obj is Vector2)
                 return Equals((Vector2)obj);
            return false;
        }
        public double DistanceTo(Vector2 other)
        {
            return Math.Sqrt((other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y)); ;
        }
        public double DistanceSquaredTo(Vector2 other)
        {
            return (other.X - X) * (other.X - X) + (other.Y - Y) * (other.Y - Y);
        }
    }
}
