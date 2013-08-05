using System;

namespace Jim.OCR.Mathematics {
    /// <summary>
    /// 复数类，基于double
    /// </summary>
    public struct Complex : ICloneable, IComparable<Complex> {
        #region 属性

        private double re;
        private double im;

        /// <summary>
        /// 实部
        /// </summary>
        public double Re { get { return re; } set { re = value; } }

        /// <summary>
        /// 虚部
        /// </summary>
        public double Im { get { return im; } set { im = value; } }

        #endregion

        #region 构造

        public Complex(double real, double imaginary) {
            re = real;
            im = imaginary;
        }

        public Complex(Complex c) {
            re = c.Re;
            im = c.Im;
        }

        /// <summary>
        /// 通过极坐标构造
        /// </summary>
        /// <param name="modulus">模</param>
        /// <param name="argument">弧度</param>
        public static Complex FromModulusArgument(double modulus, double argument) {
            double re = modulus * Math.Cos(argument),
                   im = modulus * Math.Sin(argument);
            return new Complex(re, im);
        }

        #endregion

        #region ICloneable

        object ICloneable.Clone() {
            return this.Clone();
        }

        public Complex Clone() {
            return new Complex(this);
        }

        #endregion

        #region 数学运算

        /// <summary>
        /// 求模
        /// </summary>
        public double GetMod() {
            double x = Re, y = Im;
            return Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// 模的平方
        /// </summary>
        public double GetModSqr() {
            double x = Re, y = Im;
            return x * x + y * y;
        }

        /// <summary>
        /// 圆心角（弧度）
        /// </summary>
        public double GetArgument() {
            return Math.Atan2(Im, Re);
        }

        /// <summary>
        /// 标准化
        /// </summary>
        public Complex Normalize() {
            double mod = this.GetMod();
            if (mod == 0) {
                throw new DivideByZeroException("Can not normalize a complex number that is zero.");
            }
            double re = Re / mod,
                   im = Im / mod;
            return new Complex(re, im);
        }

        #endregion

        #region 类型转换

        /// <summary>
        /// double转复数，虚部为0。
        /// Convert from a single precision real number to a complex number
        /// </summary>
        public static explicit operator Complex(double d) {
            return new Complex(d, 0);
        }

        /// <summary>
        /// 复数转double，忽略虚部。
        /// </summary>
        public static explicit operator double(Complex c) {
            return c.Re;
        }

        #endregion

        #region 运算符重载

        public static bool operator ==(Complex a, Complex b) {
            return (a.Re == b.Re) && (a.Im == b.Im);
        }

        public static bool operator !=(Complex a, Complex b) {
            return (a.Re != b.Re) || (a.Im != b.Im);
        }

        public static Complex operator -(Complex a) {
            return new Complex(-a.Re, -a.Im);
        }

        public static Complex operator +(Complex a, double f) {
            return new Complex(a.Re + f, a.Im);
        }

        public static Complex operator +(double f, Complex a) {
            return new Complex(a.Re + f, a.Im);
        }

        public static Complex operator +(Complex a, Complex b) {
            return new Complex(a.Re + b.Re, a.Im + b.Im);
        }

        public static Complex operator -(Complex a, double f) {
            return new Complex(a.Re - f, a.Im);
        }

        public static Complex operator -(double f, Complex a) {
            return new Complex(f - a.Re, 0 - a.Im);
        }
        public static Complex operator -(Complex a, Complex b) {
            return new Complex(a.Re - b.Re, a.Im - b.Im);
        }

        public static Complex operator *(Complex a, double f) {
            return new Complex(a.Re * f, a.Im * f);
        }

        public static Complex operator *(double f, Complex a) {
            return new Complex(a.Re * f, a.Im * f);
        }

        /// <summary>
        /// Multiply two complex numbers together
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex operator *(Complex a, Complex b) {
            // (x + yi)(u + vi) = (xu - yv) + (xv + yu)i. 
            double x = a.Re, y = a.Im,
                   u = b.Re, v = b.Im;

            return new Complex(x * u - y * v, x * v + y * u);
        }

        public static Complex operator /(Complex a, double f) {
            if (f == 0) {
                throw new DivideByZeroException();
            }
            return new Complex(a.Re / f, a.Im / f);
        }

        public static Complex operator /(Complex a, Complex b) {
            double x = a.Re, y = a.Im,
                   u = b.Re, v = b.Im;
            double denom = u * u + v * v;

            if (denom == 0) {
                throw new DivideByZeroException();
            }

            double m = (x * u + y * v) / denom,
                   n = (y * u - x * v) / denom;

            return new Complex(m, n);
        }

        #endregion

        #region Object重写

        public override int GetHashCode() {
            return (Re.GetHashCode() ^ Im.GetHashCode());
        }

        public override bool Equals(object o) {
            if (o is Complex) {
                Complex c = (Complex)o;
                return (this == c);
            }
            return false;
        }

        public override string ToString() {
            return String.Format("({0}, {1}i)", Re, Im);
        }

        #endregion

        #region IComparable

        public int CompareTo(Complex o) {
            if (o == null) {
                return 1;  // null sorts before current
            }
            return this.GetMod().CompareTo(o.GetMod());
        }

        #endregion

        #region 其他

        /// <summary>
        /// 比较两者是否在容忍度内相等
        /// </summary>
        public static bool IsEqual(Complex a, Complex b, double tolerance) {
            return (Math.Abs(a.Re - b.Re) < tolerance) &&
                   (Math.Abs(a.Im - b.Im) < tolerance);

        }

        public static Complex Zero {
            get { return new Complex(0, 0); }
        }

        public static Complex I {
            get { return new Complex(0, 1); }
        }

        public static Complex MaxValue {
            get { return new Complex(double.MaxValue, double.MaxValue); }
        }

        public static Complex MinValue {
            get { return new Complex(double.MinValue, double.MinValue); }
        }

        #endregion
    }
}
