using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Iesi.Collections.Generic;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Diagnostics;
using System.IO;

namespace Jim.OCR.Algorithms {
    public class ShapeContext {
        /// <summary>
        /// 表示该形状上下文的对数极坐标直方图
        /// </summary>
        public LogPolar2Histogram Histogram { get; private set; }

        /// <summary>
        /// 通过一组规范化的距离/方向角来构造形状上下文
        /// </summary>
        /// <param name="vectors">Vector2的X为规范化到[0,1]的距离参数，Y为[0,2pi]的方向角参数。</param>
        public ShapeContext(Vector2[] vectors) {
            Histogram = new LogPolar2Histogram(vectors);
        }

        public int CostTo(ShapeContext other) {
            int cost = 0;
            for (int i = 0; i < LogPolar2Histogram.ThetaBin; ++i) {
                for (int j = 0; j < LogPolar2Histogram.RadiusBin; ++j) {
                    int a = this.Histogram[i, j], b = other.Histogram[i, j];
                    if (a == 0 && b == 0) continue;
                    cost += (a - b) * (a - b) / (a + b);
                }
            }
            return cost / 2;
        }

        /// <summary>
        /// 讲一组采样点集关系向量Vector2，其X为规范化到[0,1]的距离参数，Y为[0,2pi]的方向角参数。
        /// </summary>
        public static Vector2[][] Normalize(List<Point> sample, out double min, out double max) {
            min = double.MaxValue;
            max = double.MinValue;
            int n = sample.Count;
            Vector2[][] result = new Vector2[n][];
            int set = 0;
            foreach (var p1 in sample) {
                result[set] = new Vector2[n - 1];
                int pos = 0;
                foreach (var p2 in sample) {
                    if (p2 == p1) continue;
                    double x = p2.X - p1.X, y = p2.Y - p1.Y;
                    double r = Math.Log10(x * x + y * y),
                           t = Math.Atan2(y, x);
                    if (t < 0) t += Math.PI * 2;

                    if (r < min) min = r;
                    if (r > max) max = r;

                    result[set][pos++] = new Vector2(r, t);
                }
                set++;
            }
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n - 1; ++j) {
                    result[i][j].X = (result[i][j].X - min) / (max - min);
                }
            }

            return result;
        }

        public static Vector2[][] Normalize(List<Point> sample) {
            double min, max;
            return Normalize(sample, out min, out max);
        }
    }

    public class LogPolar2Histogram {
        public static int RadiusBin = 12;
        public static int ThetaBin = 5;

        private int[,] Histogram;

        public LogPolar2Histogram(Vector2[] vectors) {
            Histogram = (int[,])Array.CreateInstance(typeof(int), ThetaBin, RadiusBin);

            foreach (var v in vectors) {
                double r = v.X, t = v.Y;

                int i = (int)(t / (Math.PI * 2 / ThetaBin));
                if (i == ThetaBin) i--;
                int j = (int)(r * RadiusBin);
                if (j == RadiusBin) j--;

                ++Histogram[i, j];
            }
        }

        /// <summary>
        /// 将此直方图转换为图片
        /// </summary>
        public Image<Gray, Byte> ToImage() {
            int binWidth = 10, binHeight = 18;
            Image<Gray, Byte> img = new Image<Gray, byte>(RadiusBin * binWidth, ThetaBin * binHeight);

            double minCount = int.MaxValue, maxCount = int.MinValue;
            for (int i = 0; i < ThetaBin; ++i) {
                for (int j = 0; j < RadiusBin; ++j) {
                    int count = Histogram[i, j];
                    if (count < minCount) minCount = count;
                    if (count > maxCount) maxCount = count;
                }
            }

            for (int i = 0; i < ThetaBin; ++i) {
                for (int j = 0; j < RadiusBin; ++j) {
                    double color = 255 - (Histogram[ThetaBin - i - 1, j] - minCount) / (maxCount - minCount) * 255;
                    img.FillConvexPoly(new[]{
                        new Point((j+0)*binWidth, (i+0)*binHeight),   
                        new Point((j+1)*binWidth, (i+0)*binHeight),
                        new Point((j+1)*binWidth, (i+1)*binHeight),
                        new Point((j+0)*binWidth, (i+1)*binHeight)
                    }, new Gray(color));
                }
            }

            return img;
        }

        public int this[int i, int j] {
            get { return this.Histogram[i, j]; }
        }
    }

    public class ImageShapeContext {
        public ShapeContext[] ShapeContests { get; set; }
        public List<Point> Points { get; set; }

        public ImageShapeContext(List<Point> points) {
            ShapeContests = new ShapeContext[points.Count];
            Points = points;
        }
    }

    public class ShapeContextMatching {
        private ShapeContext[] scsa;
        private ShapeContext[] scsb;

        private int[,] cost;

        public ShapeContextMatching(ShapeContext[] scsa, ShapeContext[] scsb) {
            this.scsa = scsa;
            this.scsb = scsb;
        }

        public void BuildCostGraph() {
            int na = scsa.Length, nb = scsb.Length;
            int nn = na + nb;
            cost = new int[nn, nn];
            int INF = 10000000;
            for (int i = 0; i < nn; ++i) {
                for (int j = 0; j < nn; ++j) {
                    cost[i, j] = INF;
                }
            } for (int i = 0; i < na; ++i) {
                for (int j = 0; j < nb; ++j) {
                    cost[i, j + na] = cost[j + na, i] = scsa[i].CostTo(scsb[j]);
                }
            }

            using (StreamWriter sw = new StreamWriter(@"D:\Play Data\cost.txt")) {
                for (int i = 0; i < nn; ++i) {
                    for (int j = 0; j < nn; ++j) {
                        sw.Write("{0}\t", cost[i, j] == INF ? -1 : cost[i, j]);
                    }
                    sw.WriteLine();
                }
            }
        }

        public KM Match() {
            KM km = new KM(scsa.Length + scsb.Length, cost);
            km.Match(false);

            return km;
        }
    }
}
