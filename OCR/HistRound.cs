using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Jim.OCR {
    public static class HistRound {
        public static Image<Gray, Byte> Equalize(this Image<Gray, Byte> src, int m, int n) {
            int[] hist_old = new int[n];
            int grade = 256 / n;
            for (int y = 0; y < src.Height; ++y) {
                for (int x = 0; x < src.Width; ++x) {
                    int g = 256 - grade - (int)src[y, x].Intensity;
                    hist_old[g / grade]++;
                }
            }
            double sum = hist_old.Sum();
            var p = hist_old.Select(h => h / sum).ToArray();
            int[] index = new int[n];
            index[0] = 0;
            for (int i = 1; i < n; ++i) {
                p[i] += p[i - 1];
                index[i] = (int)Math.Round(p[i] * (m - 1));
            }

            double[] newgraylevel = new double[m];
            double newgrade = 255.0 / m;
            for (int i = 0; i < m; ++i) {
                newgraylevel[i] = 255 - newgrade - i * newgrade;
            }

            var dst = new Image<Gray, Byte>(src.Width, src.Height);
            for (int y = 0; y < src.Height; ++y) {
                for (int x = 0; x < src.Width; ++x) {
                    int g = 256 - grade - (int)src[y, x].Intensity;
                    int level = g / grade;
                    dst[y, x] = new Gray(newgraylevel[index[level]]);
                }
            }
            return dst;
        }
    }
}
