using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Mathematics {
    /// <summary>
    /// 复数数学运算
    /// </summary>
    public static class ComplexMath {
        #region 常数

        /// <summary>
        /// 二分之根号二
        /// </summary>
        private static readonly double _halfOfSqrt2 = Math.Sqrt(2.0) * 0.5;

        #endregion

        #region 算术运算

        /// <summary>
        /// 复数平方根
        /// </summary>
        public static Complex Sqrt(Complex c) {
            double x = c.Re, y = c.Im, mod = c.GetMod();
            int sign = y < 0 ? -1 : 1;
            double re = _halfOfSqrt2 * Math.Sqrt(mod + x),
                   im = _halfOfSqrt2 * sign * Math.Sqrt(mod - x);
            return new Complex(re, im);
        }

        /// <summary>
        /// 复数的幂
        /// </summary>
        public static Complex Pow(Complex c, double exp) {
            double mod = Math.Pow(c.GetModSqr(), exp * 0.5),
                   arg = c.GetArgument() * exp;
            double re = mod * Math.Cos(arg),
                   im = mod * Math.Sin(arg);
            return new Complex(re, im);
        }

        #endregion
    }
}
