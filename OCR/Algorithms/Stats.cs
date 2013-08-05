using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Algorithms {
    /// <summary>
    /// 统计
    /// </summary>
    public static class Stats {
        public static double[] CalcMeansAndVariance(double[] values) {
            double means = values.Average();

            double variance = values.Sum(val => val * val - means * means) / values.Length;

            return new double[2] { means, variance };
        }
    }
}
