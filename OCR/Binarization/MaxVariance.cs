using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Web.Script.Serialization;
using System.ComponentModel;

namespace Jim.OCR.Binarization {
    /// <summary>
    /// 最大类间方差法二值化（针对全图）
    /// </summary>
    [ProcessorName("最大类间方差法二值化")]
    [ProcessorDescription("对图像所有点的灰度进行统计，求出直方图双峰间的最佳点作为阈值。")]
    public class MaxVariance : IImageProcessor {
        /// <summary>
        /// 是否进行反色。若源图前景色是深色，则应该反色。总之目标图形应该是黑底白字。
        /// </summary>
        [DefaultValue(true)]
        [Description("是否进行反色。若源图前景色是深色，则应该反色。总之目标图形应该是黑底白字。")]
        public bool Inverse { get; set; }

        [ScriptIgnore]
        [Browsable(false)]
        public string test = "应该不会被序列化吧";

        public MaxVariance() {
            Inverse = true;
        }

        private static double CalculateThreshold(Image<Gray, Byte> img) {
            int len = img.Height * img.Width;
            int[] histogram = new int[256]; // 灰度统计直方图
            int min = int.MaxValue, max = int.MinValue;
            for (int i = 0; i < img.Height; ++i) {
                for (int j = 0; j < img.Width; ++j) {
                    int b = (int)img[i, j].Intensity;
                    if (b == 255) continue;
                    histogram[b]++;
                    if (b < min) min = b;
                    if (b > max) max = b;
                }
            }
            int t = min, threshold = 0;
            double tg = 0;
            while (t < max) {
                double sum1 = 0, sum2 = 0;
                int n1 = 0, n2 = 0;

                for (int i = min; i <= t; ++i) {
                    // 统计A组，min~t
                    sum1 += histogram[i] * i;
                    n1 += histogram[i];
                }
                for (int i = t + 1; i <= max; ++i) {
                    // 统计B组，t~max
                    sum2 += histogram[i] * i;
                    n2 += histogram[i];
                }

                double mean1 = sum1 / n1, mean2 = sum2 / n2, w1 = (double)n1 / len, w2 = (double)n2 / len;
                double g = w1 * w2 * (mean1 - mean2) * (mean1 - mean2);
                if (g > tg) {
                    tg = g;
                    threshold = t;
                }
                ++t;
            }
            return threshold;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            var threshold = new Gray(CalculateThreshold(src));
            if (Inverse) return src.ThresholdBinaryInv(threshold, new Gray(255));
            else return src.ThresholdBinary(threshold, new Gray(255));
        }
    }
}