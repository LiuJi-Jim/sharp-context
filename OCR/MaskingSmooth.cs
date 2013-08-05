using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using Jim.OCR.Algorithms;
using System.Drawing;

namespace Jim.OCR {
    public static class MaskingSmooth {
        /// <summary>
        /// 掩模
        /// </summary>
        private static readonly int[,] mask = new int[5, 5]{
            {-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1},
            {-1,-1,24,-1,-1},
            {-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1}
        };

        private static readonly List<Point[]> masks = new List<Point[]>();

        #region 定义9个掩模 已废弃，改成17个
        /*
        private static readonly string[] maskDef = new string[]{
             "....."
            +".###."
            +".###."
            +".###."
            +".....",
             "....."
            +"##..."
            +"###.."
            +"##..."
            +".....",
             ".###."
            +".###."
            +"..#.."
            +"....."
            +".....",
             "....."
            +"...##"
            +"..###"
            +"...##"
            +".....",
             "....."
            +"....."
            +"..#.."
            +".###."
            +".###.",
             "##..."
            +"###.."
            +".##.."
            +"....."
            +".....",
             "...##"
            +"..###"
            +"..##."
            +"....."
            +".....",
             "....."
            +"....."
            +"..##."
            +"..###"
            +"...##",
             "....."
            +"....."
            +".##.."
            +"###.."
            +"##..."
        };*/

        #endregion

        #region 定义新的17个掩模

        private static readonly string[] maskDef = new string[]{
             "....."
            +"..#.."
            +".###."
            +"..#.."
            +".....",
             ".##.."
            +".##.."
            +"..#.."
            +"....."
            +".....",
             "....."
            +"...##"
            +"..###"
            +"....."
            +".....",
             "....."
            +"....."
            +"..#.."
            +"..##."
            +"..##.",
             "....."
            +"....."
            +"###.."
            +"##..."
            +".....",
             "..##."
            +"..##."
            +"..#.."
            +"....."
            +".....",
             "....."
            +"....."
            +"..###"
            +"...##"
            +".....",
             "....."
            +"....."
            +"..#.."
            +".##.."
            +".##..",
             "....."
            +"##..."
            +"###.."
            +"....."
            +".....",
             "#...."
            +"##..."
            +".##.."
            +"....."
            +".....",
             "...##"
            +"..##."
            +"..#.."
            +"....."
            +".....",
             "....."
            +"....."
            +"..##."
            +"...##"
            +"....#",
             "....."
            +"....."
            +"..#.."
            +".##.."
            +"##...",
             "##..."
            +".##.."
            +"..#.."
            +"....."
            +".....",
             "....#"
            +"...##"
            +"..##."
            +"....."
            +".....",
             "....."
            +"....."
            +"..#.."
            +"..##."
            +"...##",
             "....."
            +"....."
            +".##.."
            +"##..."
            +"#....",
        };

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        static MaskingSmooth() {
            foreach (string m in maskDef) {
                var tmpList = new List<Point>();
                for (int i = 0; i < 5; ++i) {
                    for (int j = 0; j < 5; ++j) {
                        char c = m[i * 5 + j];
                        if (c == '#') {
                            tmpList.Add(new Point(j - 2, i - 2));
                        }
                    }
                }
                masks.Add(tmpList.ToArray());
            }
        }

        public static Image<Gray, Byte> SmoothMask(this Image<Gray, Byte> src, int iteration) {
            for (int i = 0; i < iteration; ++i) src = src.SmoothMask();
            return src;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Image<Gray, Byte> SmoothMask(this Image<Gray, Byte> src) {
            Image<Gray, Byte> dst = src.Clone();
            int width = src.Width, height = src.Height;
            int[] grayCounts = new int[256];
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    int g = (int)Math.Ceiling(src[y, x].Intensity);
                    ++grayCounts[g];
                }
            }
            double[,] gray = new double[height, width];
            double min = double.MaxValue, max = double.MinValue;
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    // 使用1个24掩模
                    //for (int i = -2; i <= 2; ++i) {
                    //    for (int j = -2; j <= 2; ++j) {
                    //        int ny = y + i;
                    //        //if (ny < 0 || ny >= height - 2) continue;
                    //        if (ny < 0) ny = 0;
                    //        if (ny >= height - 2) ny = height - 3;
                    //        int nx = x + j;
                    //        //if (nx < 0 || nx >= width - 2) continue;
                    //        if (nx < 0) nx = 0;
                    //        if (nx >= width - 2) nx = width - 3;
                    //        gray[y, x] += src[ny, nx].Intensity * mask[i + 2, j + 2];
                    //    }
                    //}

                    // 分别使用9个掩模
                    List<double[]> stats = new List<double[]>();
                    foreach (var list in masks) {
                        List<double> grays = new List<double>();
                        foreach (var off in list) {
                            int ny = y + off.Y,
                                nx = x + off.X;
                            if (ny < 0) ny = 0;
                            if (ny >= height - 2) ny = height - 3;
                            if (nx < 0) nx = 0;
                            if (nx >= width - 2) nx = width - 3;
                            grays.Add(src[ny, nx].Intensity);
                        }
                        double[] stat = Stats.CalcMeansAndVariance(grays.ToArray());
                        double sum = 0;
                        double graySum = 0;
                        foreach (double g in grays) {
                            int gc = grayCounts[(int)Math.Ceiling(g)];
                            sum += gc;
                            graySum += g * gc;
                        }
                        stat[0] = graySum / sum;
                        stats.Add(stat);
                    }
                    double[] best = stats.OrderBy(val => val[1]).First();
                    gray[y, x] = best[0];

                    if (gray[y, x] < min) min = gray[y, x];
                    if (gray[y, x] > max) max = gray[y, x];
                }
            }
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    dst[y, x] = new Gray(256 * (gray[y, x] - min) / (max - min));
                }
            }


            return dst;
        }
    }
}
