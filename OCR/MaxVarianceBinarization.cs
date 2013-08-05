using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Jim.OCR {
    /// <summary>
    /// 最大类间方差法二值化（针对全图）
    /// </summary>
    public class MaxVarianceBinarization : IBinarization {
        public byte[] Binarization(byte[] grey, int width, int height) {
            int ttt;
            return Binarization(grey, width, height, out ttt);
        }

        public byte[] Binarization(byte[] grey, int width, int height, out int th) {
            int len = grey.Length;
            int[] histogram = new int[256]; // 灰度统计直方图
            int min = int.MaxValue, max = int.MinValue;
            for (int i = 0; i < len; ++i) {
                int b = grey[i];
                if (b == 255) continue;
                histogram[b]++;
                if (b < min) min = b;
                if (b > max) max = b;
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

            //Console.WriteLine("{0}/{1},Threshold:{2}", min, max, threshold);
            byte[] result = new byte[grey.Length];
            for (int i = 0; i < grey.Length; ++i) {
                result[i] = (byte)(grey[i] < threshold ? 255 : 0);
            }

            th = threshold;
            return result;
        }

        public double CalculateThreshold(Image<Gray, Byte> img) {
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
    }
}
