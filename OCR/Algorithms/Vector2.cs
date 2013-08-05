using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Algorithms {
    public struct Vector2 : IEquatable<Vector2> {
        #region 属性
        private double _x, _y;
        public double X {
            get { return _x; }
            set { _x = value; }
        }
        public double Y {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        #region 构造
        public Vector2(double a, double b) {
            _x = a;
            _y = b;
        }
        #endregion

        #region 自身计算
        public double Mod() {
            return Math.Sqrt(X * X + Y * Y);
        }

        public double ModSqr() {
            return (X * X + Y * Y);
        }

        /// <summary>
        /// 角度
        /// </summary>
        public double Theta() {
            return Math.Atan2(Y, X);
        }
        #endregion

        #region IEquatable<Vector2>

        public bool Equals(Vector2 other) {
            return this == other;
        }

        #endregion

        #region 运算符
        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2 operator *(Vector2 a, double k) {
            return new Vector2(a.X * k, a.Y * k);
        }
        public static Vector2 operator *(double k, Vector2 a) {
            return new Vector2(a.X * k, a.Y * k);
        }
        public static Vector2 operator /(Vector2 a, double k) {
            return new Vector2(a.X / k, a.Y / k);
        }
        public static bool operator ==(Vector2 a, Vector2 b) {
            return (a.X == b.X && a.Y == b.Y);
        }
        public static bool operator !=(Vector2 a, Vector2 b) {
            return (a.X != b.X || a.Y != b.Y);
        }
        #endregion

        #region Object重写
        public override bool Equals(object obj) {
            if (Object.ReferenceEquals(obj, null)) return false;
            if (obj is Vector2) return this == (Vector2)obj;
            return false;
        }
        public override int GetHashCode() {
            return (X.GetHashCode() ^ Y.GetHashCode());
        }
        public override string ToString() {
            return String.Format("<{0}, {1}>", X, Y);
        }
        #endregion

        #region 其他
        public static double Distance(Vector2 a, Vector2 b) {
            return (a - b).Mod();
        }
        public static double DistanceSqr(Vector2 a, Vector2 b) {
            return (a - b).ModSqr();
        }
        public static Vector2 Zero {
            get { return new Vector2(0, 0); }
        }
        public static Vector2 Max {
            get { return new Vector2(double.MaxValue, double.MaxValue); }
        }
        #endregion
    }
}
