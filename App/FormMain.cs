using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jim.OCR;
using Jim.OCR.Algorithms;
using System.Diagnostics;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace App {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
            boxes = new[] { imageBox1, imageBox2, imageBox3, imageBox4, imageBox5, imageBox6 };
        }

        private ImageBox[] boxes;

        private void button1_Click(object sender, EventArgs e) {
            clearText();
            string path = @"D:\Play Data\毕业设计\测试数据\CSDN";
            string savepath = @"D:\Play Data\毕业设计\测试数据\CSDN-out";

            Stopwatch timer = new Stopwatch();
            foreach (var filename in Directory.GetFiles(path)) {
                string file = Path.GetFileNameWithoutExtension(filename);

                timer.Reset();
                timer.Start();
                Bitmap bmp = new Bitmap(filename);
                byte[] rgb = ImageProcessor.Bmp2Bytes(bmp);
                byte[] grey = new WeightedAverageGreyscale().Greyscale(rgb);
                byte[] b2 = new MaxVarianceBinarization().Binarization(grey, bmp.Width, bmp.Height);

                int it = 0;
                byte[] dn = new AnnealKMeansClusterDenoise().Denoise(b2, bmp.Width, bmp.Height, 1, out it);
                var img = ImageProcessor.Greyscale2Bmp(dn, bmp.Width, bmp.Height);
                timer.Stop();

                appendLine("文件：{0}，用时：{1}ms， 降噪迭代次数：{2}", file, timer.ElapsedMilliseconds, it);

                img.Save(Path.Combine(savepath, file + ".out.bmp"));
            }
        }

        private void clearText() {
            richTextBox1.Clear();
        }

        private delegate void SimpleDel();
        private void appendText(string fmt, params object[] vals) {
            richTextBox1.Invoke(new SimpleDel(() => { richTextBox1.AppendText(String.Format(fmt, vals)); }));
        }
        private void appendLine(string fmt, params object[] vals) {
            appendText(fmt, vals);
            appendText("\n");
        }

        private void button2_Click(object sender, EventArgs e) {
            openfile();
            gao();
        }

        MyTimer timer = new MyTimer();
        private void gao() {
            clearText();
            string filename = label1.Text;
            string path = Path.GetDirectoryName(filename);
            string file = Path.GetFileNameWithoutExtension(filename);
            string savepath = Path.Combine(path, "out");
            appendLine(filename);

            Image<Bgr, Byte> bmp = new Image<Bgr, byte>(filename);

            Image<Gray, Byte> gray = bmp.Convert<Gray, Byte>();

            double threshold = new MaxVarianceBinarization().CalculateThreshold(gray);
            appendLine("阈值：{0}", threshold);

            //Image<Gray, Byte> bi = gray.ThresholdBinaryInv(new Gray(threshold), new Gray(255));
            // Image<Gray, Byte> dn = bi.SmoothMedian(3);
            int it;
            //Stopwatch timer = Stopwatch.StartNew();
            //Image<Gray, Byte> dn = new AnnealKMeansClusterDenoise().Denoise(bi, 1, out it);
            //timer.Stop();
            //appendLine("降噪用时{0}ms，迭代次数{1}。", timer.ElapsedMilliseconds, it);

            //var con = dn.FindContours();
            //bmp.Draw(con, new Bgr(0, 0, 255), new Bgr(255, 0, 0), 1, 1);

            //var proj = Projecting.VerticalProject(dn);
            //var proj = bi.SmoothMedian(3);

            //var proj = dn - dn.Erode(1);//dn.Dilate(1) - bi;

            //var proj = gray.MorphologyEx(see, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_OPEN, 1);
            //var proj = 
            //showImage(imageBox1, bi);
            //showImage(imageBox2, dn);
            //showImage(imageBox1, gray);

            //showImage(imageBox1, gray);

            timer.Restart();
            var mask = gray.SmoothMask();
            showImage(imageBox1, mask);
            appendLine("掩模：{0}ms", timer.Stop());

            timer.Restart();
            double thr = new MaxVarianceBinarization().CalculateThreshold(mask);
            var thrd = mask.ThresholdBinaryInv(new Gray(thr), new Gray(255));
            showImage(imageBox2, thrd);
            appendLine("二值：{0}ms", timer.Stop());

            timer.Restart();
            var smooth = thrd.SmoothMedian(7);
            showImage(imageBox3, smooth);
            appendLine("中值滤波：{0}ms", timer.Stop());

            timer.Restart();
            var dm = new AnnealKMeansClusterDenoise().Denoise(thrd, 0, out it).SmoothMedian(5);
            showImage(imageBox4, dm);
            appendLine("区域联通：{0}ms", timer.Stop());

            #region 灰度分级连通
            //int level = 16;
            //var gray4 = mask.ToGrayN(level);
            //showImage(imageBox1, gray4);

            //timer.Restart();
            //var conns = gray4.Connect(level);
            //appendLine("分级：{0}ms", timer.Stop());

            //var mins = Utils.FindLocalMins(conns.Select(c => c.Domains.Count).ToArray());
            //var list = new List<ConnectLevel>();
            //for (int i = 1; i < mins.Length; ++i) {
            //    var cl = conns[mins[i]];
            //    if (cl.Domains.Count == 1) continue;
            //    if (cl.MeanArea < 10) continue;
            //    if (cl.Variance > 20000) continue;
            //    //if (mins[i] - mins[i - 1] == 1) {
            //    //    var lastcl = conns[mins[i] - 1];
            //    //    if (lastcl.Domains.Count != cl.Domains.Count)
            //    //        list.Add(cl);
            //    //} else {
            //    //    list.Add(cl);
            //    //}
            //    list.Add(cl);
            //}
            //list.ForEach(
            //    c => appendLine("Lv{0},C{1},MA{2:F2},VA{3:F4}", c.Level, c.Domains.Count, c.MeanArea, c.Variance));
            //timer.Restart();
            //var bestlevel = list.OrderByDescending(c => c.MeanArea).ThenBy(c => c.Domains.Count).First();
            //var colored = gray4.ColorLevel(bestlevel);
            //var see = new StructuringElementEx(4, 4, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
            //var bg = colored.Erode(10).Dilate(10);
            //showImage(imageBox2, colored - bg);
            //appendLine("染色处理{0}ms", timer.Stop());

            #endregion

            //double t2=new MaxVarianceBinarization().CalculateThreshold(mask);
            //var proj = mask.ThresholdBinaryInv(new Gray(t2), new Gray(255));
            //var akmcd = new AnnealKMeansClusterDenoise();
            //var t3 = new MaxVarianceBinarization().CalculateThreshold(mask);
            //var bi2 = mask.ThresholdBinaryInv(new Gray(t3), new Gray(255));
            //showImage(imageBox2, bi2);

            //var dn2 = akmcd.Denoise(bi2, 1, out it);
            //showImage(imageBox3, dn2);

            //var contour = ContourFinder.ColorContours(dn2, new Gray(0), new Gray(255));
            //showImage(imageBox5, contour);

            //var con = dn2.FindContours();
            //var con2 = dn2.Convert<Bgr, Byte>();
            //con2.Draw(con, new Bgr(0, 0, 255), new Bgr(0, 255, 0), 1, 1);
            //showImage(imageBox6, con2);

            //p1 = akmcd.Denoise(p1, 0, out it);
            //p2 = akmcd.Denoise(p2, 0, out it);
            //var p3 = p1.Xor(p2);
            //var p4 = p1 - p3;
            //var p5 = p2 - p3;
            //var p6 = p1 - p2;

            //var p4 = akmcd.Denoise(p1, 0, out it);
            //var p5 = akmcd.Denoise(p2, 0, out it);
            //var p6 = akmcd.Denoise(p3, 0, out it);

            //StructuringElementEx se = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);

            //dn = dn.MorphologyEx(se, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_OPEN, 1);

            //var canny = dn.Canny(new Gray(threshold), new Gray(threshold * 0.6));

            //showImage(imageBox1, bmp);
            //showImage(imageBox2, gray);
            //showImage(imageBox3, bi);
            //showImage(imageBox4, dn);
            //showImage(imageBox5, p1);
            //showImage(imageBox6, p2);
            //showImage(imageBox4, p4);
            //showImage(imageBox5, p5);
            //showImage(imageBox6, p6);
        }

        private double norm(double val, double min, double max, double scale) {
            return (val - min) / (max - min) * scale;
        }

        private void showImage(ImageBox pic, IImage img) {
            int zoom = 16;
            pic.Size = new Size(img.Size.Width * zoom / 10, img.Size.Height * zoom / 10);
            pic.Image = img;
        }

        #region 没用了
        /*

        public double CalcVar(int[] data) {
            int len = data.Length;
            if (len <= 1) return 0;
            if (len == 2) return (data[0] + data[1]) / 2;
            int min = data.Min(), max = data.Max();
            int[] histogram = new int[max + 1]; // 灰度统计直方图
            for (int i = 0; i < len; ++i) {
                histogram[data[i]]++;
            }
            double t = min, threshold = 0, step = 0.26;
            double tg = 0;
            while (t < max) {
                double sum1 = 0, sum2 = 0;
                int n1 = 0, n2 = 0;

                for (int i = min; i < t; ++i) {
                    // 统计A组，min~t
                    sum1 += histogram[i] * i;
                    n1 += histogram[i];
                }
                for (int i = (int)t; i <= max; ++i) {
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
                t += step;
            }
            threshold -= step;
            return threshold;
        }

        public int Connective = 4;
        public byte[] Denoise(byte[] data, int width, int height, int padding) {
            byte[,] arr = Utils.ByteArr1DTo2D(data, width, height);
            bool[,] vi = (bool[,])Array.CreateInstance(typeof(bool), height, width);

            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    vi[i, j] = false;
                }
            }

            List<Point> points = new List<Point>();
            List<int> areas = new List<int>();

            for (int i = padding; i < height - padding; ++i) {
                for (int j = padding; j < width - padding; ++j) {
                    if (vi[i, j]) continue;
                    if (arr[i, j] == 255) continue;
                    int sum = 0;
                    dfsSearch(arr, vi, i, j, width, height, padding, ref sum);
                    points.Add(new Point(j, i));
                    areas.Add(sum);
                }
            }

            if (points.Count > 1) {
                Vector2[] vs = areas.Select(i => new Vector2(i, 0)).ToArray();
                Stopwatch timer = new Stopwatch();
                timer.Start();
                // KMeansCluster result = KMeansClustering.Cluster(vs, 2);
                int it = 0;
                KMeansCluster result = KMeansClustering.AnnealCluster(vs, 2, out it);
                timer.Stop();
                appendLine("对{0}个数据分{1}堆，耗时{2}ms，迭代{3}次。", vs.Length, 2, timer.ElapsedMilliseconds, it);
                for (int i = 0; i < 2; ++i) {
                    appendLine("第{0}类，质心{1}。----------------", i, result[i].Centroid.A);
                    foreach (var v in result[i]) {
                        appendText("{0}, ", v.A);
                    }
                    appendLine("");
                }
                int bigcluster = (result[0].Centroid.ModSqr() > result[1].Centroid.ModSqr()) ? 0 : 1;
                for (int i = 0; i < points.Count; ++i) {
                    if (result.ClusterIndex[i] == bigcluster) {
                        dfsColor(arr, points[i].Y, points[i].X, width, height, 1);
                    }
                }
            } else {
                dfsColor(arr, points[0].Y, points[0].X, width, height, 1);
            }

            //double threshold = CalcVar(areas.ToArray());
            //for (int i = 0; i < points.Count; ++i) {
            //    if (areas[i] > threshold) {
            //        dfsColor(arr, points[i].Y, points[i].X, width, height, padding);
            //    }
            //}

            //for (int i = 0; i < points.Count; ++i) {
            //    if (areas[i] > threshold) {
            //        //dfsClear(arr, points[i].Y, points[i].X, width, height);
            //        dfsColor(arr, points[i].Y, points[i].X, width, height, padding);
            //    }
            //}

            //appendLine("最大类间方差降噪阈值：{0}", threshold);

            byte[] bytes = Utils.ByteArr2DTo1D(arr);
            return bytes;
        }

        private static readonly int[] dy8 = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private static readonly int[] dx8 = { -1, 0, 1, 1, 1, 0, -1, -1 };
        private static readonly int[] dy4 = { -1, 0, 1, 0 };
        private static readonly int[] dx4 = { 0, 1, 0, -1 };

        private void dfsColor(byte[,] arr, int y, int x, int width, int height, int padding) {
            //arr[y, x] = 255;
            arr[y, x] = 127;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < padding || ny >= height - padding) continue;
                if (nx < padding || nx >= width - padding) continue;

                if (arr[ny, nx] == 0) dfsColor(arr, ny, nx, width, height, padding);
            }
        }

        private void dfsClear(byte[,] arr, int y, int x, int width, int height) {
            arr[y, x] = 255;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < 0 || ny >= height) continue;
                if (nx < 0 || nx >= width) continue;

                if (arr[ny, nx] == 0) dfsClear(arr, ny, nx, width, height);
            }
        }

        private void dfsSearch(byte[,] arr, bool[,] vi, int y, int x, int width, int height, int padding, ref int sum) {
            if (vi[y, x]) return;
            vi[y, x] = true;
            ++sum;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < padding || ny >= height - padding) continue;
                if (nx < padding || nx >= width - padding) continue;

                if (arr[ny, nx] == 0) dfsSearch(arr, vi, ny, nx, width, height, padding, ref sum);
            }
        }
        */
        #endregion

        private void openfile() {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;

            label1.Text = ofd.FileName;
        }

        private void button3_Click(object sender, EventArgs e) {
            openfile();
        }

        private void button4_Click(object sender, EventArgs e) {
            gao();
        }
    }
}
