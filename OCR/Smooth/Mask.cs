using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;
using System.Drawing;
using Jim.OCR.Algorithms;

namespace Jim.OCR.Smooth {
    /// <summary>
    /// 掩模平滑，用掩模对图像进行平滑，通常于平滑和增大对比度，提高降噪效果。
    /// </summary>
    [ProcessorName("掩模平滑")]
    [ProcessorDescription("用掩模对图像进行平滑，通常于平滑和增大对比度，提高降噪效果。")]
    public class Mask : IImageProcessor {
        public enum MaskType {
           /* Mask1, */Mask9, Mask17
        }

        /// <summary>
        /// 掩模类型。（默认Mask17）
        /// </summary>
        [DefaultValue(MaskType.Mask17)]
        [Description("掩模类型。")]
        public MaskType Type { get; set; }

        public Mask() {
            Type = MaskType.Mask17;
        }

        #region mask1
        private static readonly int[,] mask1 = new int[5, 5]{
            {-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1},
            {-1,-1,24,-1,-1},
            {-1,-1,-1,-1,-1},
            {-1,-1,-1,-1,-1}
        };
        #endregion

        #region mask9
        private static readonly string[] mask9 = new string[]{
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
        };
        #endregion

        #region mask17
        private static readonly string[] mask17 = new string[]{
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

        private List<Point[]> getMasks() {
            var masks = new List<Point[]>();
            var maskDef = this.Type == MaskType.Mask17 ? mask17 : mask9;
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
            return masks;
        }

        private Image<Gray, Byte> smoothmask1(Image<Gray, Byte> src) {
            int width = src.Width, height = src.Height;
            var dst = new Image<Gray, Byte>(width, height);
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    // 使用1个24掩模
                    double gray = 0;
                    for (int i = -2; i <= 2; ++i) {
                        for (int j = -2; j <= 2; ++j) {
                            int ny = y + i;
                            //if (ny < 0 || ny >= height - 2) continue;
                            if (ny < 0) ny = 0;
                            if (ny > height - 3) ny = height - 3;
                            int nx = x + j;
                            //if (nx < 0 || nx >= width - 2) continue;
                            if (nx < 0) nx = 0;
                            if (nx > width - 3) nx = width - 3;
                            gray += src[ny, nx].Intensity * mask1[i + 2, j + 2];
                        }
                    }
                    dst[y, x] = new Gray(gray > 0 ? gray : 0);
                }
            }

            return dst;
        }

        private Image<Gray, Byte> smoothmaskmany(Image<Gray, Byte> src) {
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
            var masks = getMasks();
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    // 分别使用n掩模
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
            Image<Gray, Byte> dst = new Image<Gray, byte>(width, height);
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    dst[y, x] = new Gray(256 * (gray[y, x] - min) / (max - min));
                }
            }

            return dst;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
          /*  if (this.Type == MaskType.Mask1) {
                return smoothmask1(src);
            } else {*/
                return smoothmaskmany(src);
          /*  }*/
        }
    }
}
