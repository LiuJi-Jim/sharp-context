using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exocortex.DSP;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using Jim.OCR;
using System.Net;
using Emgu.CV.Structure;
using Emgu.CV;
using Iesi.Collections.Generic;
using Jim.OCR.Algorithms;
using dnAnalytics.LinearAlgebra;
using Jim.OCR.ShapeContext2D;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Web.Script.Serialization;

namespace Test {
    public class Program {
        static Random rand = new Random();
        public static void Main(string[] args) {
            dbg.AutoFlush = true;

            //DownloadImage();
            //ProcessImage();
            //CreateSample();
            //km();

            //openMNIST();
            //analyzeMNIST();
            //testMNIST();
            //testLSW("Arial");
            //testLSW("Verdana");
            //testLSW("Comic Sans MS");
            //testSC();

            //var pa = readPoint(@"D:\Play Data\x1.txt");
            //var pb = readPoint(@"D:\Play Data\y2a.txt");
            //var pa = readPoint(@"D:\Play Data\x1-noise.txt");
            //var pb = readPoint(@"D:\Play Data\y2a-noise.txt");
            //var pa = readPoint(@"D:\Play Data\x1-outliers.txt");
            //var pb = readPoint(@"D:\Play Data\y2a-outliers.txt");
            //var pa = readPoint2(@"D:\Play Data\毕业设计\测试数据\样本\!Sample-Arial-A.txt");
            //var pb = readPoint2(@"D:\Play Data\毕业设计\测试数据\样本\!Sample-Comic Sans MS-A.txt");

            //var sc = new Jim.OCR.ShapeContext2D.ShapeContext(pa, pb);
            //sc.IterMatchTPS();
            //var v = MatrixUtils.Gaussian(4).Reshape(2, 8);
            //Console.WriteLine(v);
            //Console.WriteLine();
            //Console.WriteLine(v.GradientX());
            //Console.WriteLine();
            //Console.WriteLine(v.GradientY());
            //return;

            //string file = @"D:\Play Data\grayfile-27.txt";
            //using (Image<Gray, Byte> img = new Image<Gray, byte>(28, 28)) {
            //    double[] grays = File.ReadAllText(file).Split('\t').Select(s => double.Parse(s)).ToArray();
            //    Console.WriteLine(grays.Length);
            //    for (int i = 0; i < 28; ++i) {
            //        for (int j = 0; j < 28; ++j) {
            //            Console.Write("{0},", grays[i * 28 + j]);
            //        }
            //        Console.WriteLine();
            //    }
            //}
            //var mmr =
            //    new dnAnalytics.LinearAlgebra.IO.MatlabMatrixReader(
            //        @"D:\Play Data\sc_demo\digit_100_train_easy.mat");
            //var train_data = mmr.ReadMatrix("train_data");
            //var label_train = mmr.ReadMatrix("label_train");
            //Console.WriteLine(train_data.GetSubMatrix(0, 1, 0, train_data.Columns).Reshape(28, 28));
            //for (int i = 0; i < train_data.Rows; ++i) {
            //    using (Image<Gray, Byte> img = new Image<Gray, byte>(28, 28)) {
            //        for (int y = 0; y < 28; ++y) {
            //            for (int x = 0; x < 28; ++x) {
            //                img[y, x] = new Gray(train_data[i, y * 28 + x] * 256);
            //            }
            //        }
            //        img.Save(String.Format(@"D:\Play Data\sc_demo_easy\{0:D2}-{1}.bmp", i, (int)label_train[i, 0]));
            //    }
            //}
            //return;
            var x = new DenseMatrix(4, 5);
            for (int i = 0; i < 4; ++i) {
                for (int j = 0; j < 5; ++j)
                    x[i, j] = rand.Next(50);
            }


            //return;
            //File.Delete(@"D:\Play Data\test.txt");


            //var sc = new Jim.OCR.ShapeContext2D.ShapeContext(
            //    //@"D:\Play Data\train\00022-9.bmp",
            //    //@"D:\Play Data\train\00033-9.bmp");
            //    //@"D:\Play Data\test\00030-3.bmp",
            //    //@"D:\Play Data\test\00032-3.bmp");
            //    @"D:\Play Data\sc_demo_easy\65-5.bmp",
            //    @"D:\Play Data\sc_demo_easy\83-5.bmp");
            //    //@"D:\Play Data\sc_demo_easy\25-9.bmp",
            //    //@"D:\Play Data\sc_demo_easy\26-9.bmp");
            ////@"D:\Play Data\logo-1.jpg",
            ////@"D:\Play Data\logo-4.jpg");
            //sc.display_flag = true;
            //sc.debug_flag = true;
            //sc.timer_flag = false;
            //sc.showScale = 6;
            //sc.matchScale = 2.5;
            //sc.n_iter = 6;
            //sc.MatchFile();

            //var sc = new Jim.OCR.ShapeContext2D.ShapeContext();
            //sc.showScale = 4;
            //sc.matchScale = 1;
            //sc.n_iter = 5;
            //sc.display_flag = true;
            //sc.debug_flag = true;
            //sc.timer_flag = false;
            //sc.MatchChar('A', 'A');

            //testMNIST2();


            //computeTemplateSC(0,1000);
            //computeTemplateGSC(10000, 10000);

            //fastPruningWithRSC();
            //fastPruningWithGRSC();

            //clusterSC();
            //quantizeSC();
            //fastPruningWithShapeme();

            //clusterGSC();
            //quantizeGSC();
            //fastPruningWithGShapeme();
            //testCluter();
            //testCluterMNIST();
            //testCAPTCHA();
            //createTemplate();
            //for (int i = 0; i < 10; ++i) {
            //GenerateImage();
            //}

            //IImageProcessor ip = new Jim.OCR.Binarization.MaxVariance();
            //var ser = new JavaScriptSerializer();
            //Console.WriteLine(ser.Serialize(ip));

            //string ppp = @"D:\Play Data\fail2.bmp";
            //Image<Gray, Byte> src = new Image<Bgr, Byte>(ppp).Convert<Gray, Byte>();
            //Image<Gray, Byte> dst = new Image<Gray, byte>(src.Width, src.Height).Not();
            //for (int j = 0; j < src.Width; ++j) {
            //    for (int i1 = 0,i2=0; i1 < src.Height; ++i1) {
            //        if (src[i1, j].Intensity == 255) dst[i2++, j] = new Gray(0);
            //    }
            //}
            //string fn = Path.GetFileNameWithoutExtension(ppp);
            //dst.Save(Path.Combine(@"D:\Play Data\", fn + ".proj.bmp"));

            
            
            
            
            
            //new OnlyRSC().Run();
            //new OnlyShapeme().Run();
            //new RSCandKM().Run();
            //new ShapemeandKM().Run();

            //for(int i=0; i<10; ++i)
            //{
            //    GenerateImage();
            //}

            //new MySplit().Split();
            //new MySplit().Rename();
            //new TestCaptcha().RunShapmeme();
            //new TestCaptcha().RunRSC();
            new Sampler().Run();
        }

        private static void GenerateImage() {
            int width = 210, height = 70;
            string familyName = "Arial";
            string text = "";
            string temp = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            for (int i = 0; i < 4; ++i) {
                text += temp[rand.Next(temp.Length)];
            }
            // Create a new 32-bit bitmap image.

            Bitmap bitmap = new Bitmap(
              width,
              height,
              PixelFormat.Format32bppArgb);

            // Create a graphics object for drawing.

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, width, height);

            // Fill in the background.

            HatchBrush hatchBrush = new HatchBrush(
                HatchStyle.Shingle,
                //Color.LightGray,
                //Color.White);
                Color.FromArgb(rand.Next(224, 256), rand.Next(224, 256), rand.Next(224, 256)),
                Color.FromArgb(rand.Next(192, 224), rand.Next(192, 224), rand.Next(192, 224)));
            g.FillRectangle(hatchBrush, rect);

            // Set up the text font.

            float fontSize = rect.Height;
            Font font = new Font(familyName, fontSize, FontStyle.Bold);
            SizeF size = g.MeasureString(text, font);
            // Adjust the font size until the text fits within the image.

            do {
                fontSize--;
                font = new Font(
                  familyName,
                  fontSize,
                  FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            // Set up the text format.

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Create a path using the text and warp it randomly.
            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, rect, format);
            float v = 10;
            PointF[] points ={
                        new PointF(rand.Next(rect.Width)/v-20,rand.Next(rect.Height)/v),
                        new PointF(rect.Width - rand.Next(rect.Width)/v-20,rand.Next(rect.Height)/v),
                        new PointF(rand.Next(rect.Width)/v-20,rect.Height - rand.Next(rect.Height)/v),
                        new PointF(rect.Width - rand.Next(rect.Width)/v-20,rect.Height - rand.Next(rect.Height)/v)
                    };
            for (int i = 0; i < 1; ++i) {
                System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
                matrix.Translate(0F, 0F);
                path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            }
            // Draw the text.


            hatchBrush = new HatchBrush(
              HatchStyle.LargeConfetti,
                //Color.DarkGray,
                //Color.Black);
                Color.FromArgb(rand.Next(64, 128), rand.Next(64, 128), rand.Next(64, 128)),
                Color.FromArgb(rand.Next(0, 64), rand.Next(0, 64), rand.Next(0, 64)));
            g.FillPath(hatchBrush, path);

            // Add some random noise.

            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++) {
                int x = rand.Next(rect.Width);
                int y = rand.Next(rect.Height);
                int w = rand.Next(m / 50);
                int h = rand.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Clean up.

            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();

            for (int i = 0; i < 1; ++i)
                bitmap = wave(bitmap);

            // Set the image.
            bitmap.Save(Path.Combine(@"D:\Play Data\my_captcha", text + ".jpg"), ImageFormat.Jpeg);
        }

        private static Bitmap wave(Bitmap src) {
            Bitmap dst1 = new Bitmap(src);
            bool bXDir = true;
            double dMultValue = 7;
            double dPhase = rand.NextDouble() * Math.PI * 2;
            double dBaseAxisLen = src.Height;
            for (int i = 0; i < src.Width; i++) {
                for (int j = 0; j < src.Height; j++) {
                    double dx = 0;
                    dx = bXDir ? (2 * Math.PI * (double)j) / dBaseAxisLen : (2 * Math.PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = i + (int)(dy * dMultValue);
                    nOldY = j;

                    System.Drawing.Color color = src.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < src.Width
                        && nOldY >= 0 && nOldY < src.Height) {
                        dst1.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            Bitmap dst2 = new Bitmap(dst1);
            for (int i = 0; i < src.Width; i++) {
                for (int j = 0; j < src.Height; j++) {
                    double dx = 0;
                    dx = (2 * Math.PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = i;
                    nOldY = j + (int)(dy * dMultValue);

                    System.Drawing.Color color = dst1.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < dst1.Width
                        && nOldY >= 0 && nOldY < dst1.Height) {
                        dst2.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return dst2;
        }

        private static void createTemplate() {
            List<string> list = new List<string>();
            List<string> fonts = new List<string>();
            list.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(c => c.ToString()));
            //list.AddRange("abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(c => c.ToString()));
            list.AddRange("0123456789".ToCharArray().Select(c => c.ToString()));
            fonts.AddRange(new string[]{
                "微软雅黑","宋体","Arial","Verdana","Tahoma","Times New Roman",
                "Calibri","Cambria","Comic Sans MS","Lucida Console","Consolas","Courier New",
                "Gulim","Gungsuh","MS Gothic","方正舒体","华文新魏","Berlin Sans FB","Segoe Print"
            });
            int count = 0;
            string path = @"D:\Play Data\my_template";
            fonts.ForEach(f => {
                if (f == "") return;
                Font font = new Font(f, 48);
                list.ForEach(c => {
                    using (Bitmap bmp = new Bitmap(100, 100)) {
                        using (Graphics g = Graphics.FromImage(bmp)) {
                            g.Clear(Color.Black);
                            g.DrawString(c, font, Brushes.White, 0, 0);
                        }
                        string file = Path.Combine(path, String.Format("{0:D5}-{1}.jpg", count++, c));
                        bmp.Save(file, ImageFormat.Jpeg);
                    }
                });
                Console.WriteLine(font);
            });
        }

        private static void testCluter() {
            int ss = 100, sq = 100, k = 10, m = 10; // 采样数，G取前k，S取前m
            int randcount = 5, randtake = 10, randselect = 3;
            var timer = new MyTimer();
            var templatefiles = Directory.GetFiles(@"D:\Play Data\字符模板");
            int templatecount = templatefiles.Length;
            var templateChars = new string[templatecount];
            var templatePoints = new Vector2[templatecount][];
            //var templatesc = new double[templatecount][,];
            var templatesc = new double[templatecount][][];
            #region 计算模板的SC
            for (int f = 0; f < templatecount; ++f) {
                string file = templatefiles[f], filename = Path.GetFileNameWithoutExtension(file);
                using (Image<Gray, Byte> img = new Image<Gray, byte>(file)) {
                    var samples = getEdge(img).Sample(ss);
                    templatesc[f] = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC2(samples);
                    templatePoints[f] = samples.Select(p => new Vector2(p.X, p.Y)).ToArray();
                }
                templateChars[f] = filename;
                Console.Write(filename);
            }
            Console.WriteLine();
            Debug("模板读取完成");
            #endregion

            #region 处理字符
            foreach (string file in Directory.GetFiles(@"D:\Play Data\字符")) {
                string filename = Path.GetFileNameWithoutExtension(file);
                if (filename != "AB") continue;
                timer.Restart();
                Image<Bgr, Byte> img = new Image<Bgr, Byte>(file);
                var samples = getEdge(img.Convert<Gray, Byte>()).Sample(sq);
                //double[,] SCQ = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC(samples);
                double[][] SCQ = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC2(samples);
                var Q_Points = samples.Select(p => new Vector2(p.X, p.Y)).ToArray();
                var mmg = img.Convert<Bgr, Byte>();
                Graphics g = Graphics.FromImage(mmg.Bitmap);
                Q_Points.ToList().ForEach(v => { mmg[(int)v.Y, (int)v.X] = new Bgr(0, 0, 255); });

                var point_distance = new ValueIndexPair<double>[sq][];
                #region 计算采样点之间的距离
                for (int i = 0; i < sq; ++i) {
                    double xi = Q_Points[i].X, yi = Q_Points[i].Y;
                    point_distance[i] = new ValueIndexPair<double>[sq];
                    for (int j = 0; j < sq; ++j) {
                        double xj = Q_Points[j].X, yj = Q_Points[j].Y;
                        point_distance[i][j] = new ValueIndexPair<double> {
                            Value = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj)),
                            Index = j
                        };
                    }
                    Array.Sort(point_distance[i], (a, b) => a.Value.CompareTo(b.Value));
                }
                #endregion
                var randpoints = new int[randcount][];
                #region 随机取randcount个点，并在其周围randtake个点中取randselect个
                for (int i = 0; i < randcount; ++i) {
                    int pi = rand.Next(Q_Points.Length);
                    var p = Q_Points[pi];
                    mmg.Draw(new CircleF(new PointF((float)p.X, (float)p.Y), 2), new Bgr(255, 0, 0), 1);

                    randpoints[i] = new int[randselect];
                    bool[] vi = Utils.InitArray<bool>(randtake, false);
                    for (int cnt = 0; cnt < randselect; ) {
                        int rnd = rand.Next(randtake);
                        if (!vi[rnd]) {
                            vi[rnd] = true;
                            randpoints[i][cnt++] = rnd;
                        }
                    }
                    for (int ppp = 0; ppp < randselect; ++ppp) {
                        var pt = Q_Points[point_distance[pi][randpoints[i][ppp]].Index];
                        //g.DrawString(i.ToString(), new Font("Arial", 7), new SolidBrush(Color.FromArgb(0, 128, 0)), new PointF((float)pt.X, (float)pt.Y));
                    }
                }
                #endregion
                #region 为这randcount组RSC分别选最好的模板
                var rscmatch = new Tuple<int, double, Vector2>[randcount]; // <Si, d, L>
                for (int rc = 0; rc < randcount; ++rc) {
                    var rsc_matches = new Tuple<double, Vector2>[templatecount]; // <d, L>
                    for (int i = 0; i < templatecount; ++i) {
                        #region 拷贝出一个rsc来
                        var rsc = new double[randselect][];
                        for (int j = 0; j < randselect; ++j) {
                            rsc[j] = new double[60];
                            Array.Copy(SCQ[randpoints[rc][j]], rsc[j], 60);
                        }
                        #endregion
                        var costmat = Jim.OCR.ShapeContext2D.ShapeContext.HistCost2(rsc, templatesc[i]);
                        var matches = costmat.Select(
                            row => row.Select((d, c) => new ValueIndexPair<double> { Value = d, Index = c })
                                      .OrderBy(d => d.Value).First()).ToArray();
                        Vector2 L = Vector2.Zero;
                        double M = 0;
                        for (int j = 0; j < randselect; ++j) {
                            int u = randpoints[rc][j], mu = matches[j].Index;
                            double d = matches[j].Value;
                            Vector2 pu = Q_Points[u], pmu = templatePoints[i][mu];
                            M += (1 - d);
                            L += (1 - d) * (pu - pmu);
                        }
                        L /= M;

                        rsc_matches[i] = new Tuple<double, Vector2> {
                            First = matches.Sum(r => r.Value),
                            Second = L
                        };
                    }
                    var best_template = rsc_matches.Select((mt, i) => new { Match = mt, i })
                                                   .OrderBy(t => t.Match.First)
                                                   .First();
                    rscmatch[rc] = new Tuple<int, double, Vector2> {
                        First = best_template.i,
                        Second = best_template.Match.First,
                        Third = best_template.Match.Second
                    };

                    string label = templateChars[best_template.i];
                    g.DrawString(label, new Font("Arial", 48), Brushes.Green,
                                 new PointF((float)best_template.Match.Second.X, (float)best_template.Match.Second.Y));
                }
                #endregion

                //Font f = new Font("Arial", 12);
                //var G = new Tuple<int, int, double>[templatecount][]; // <u, m(u), d>
                //#region 为每个Si挑选合适的Gi
                //{
                //    var costmats = new double[templatecount][,];
                //    for (int i = 0; i < templatecount; ++i) {
                //        double[,] SCi = templatesc[i];
                //        costmats[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(SCQ, SCi);
                //        G[i] = new Tuple<int, int, double>[sq * ss];
                //        for (int u = 0; u < sq; ++u) {
                //            for (int j = 0; j < ss; ++j) {
                //                G[i][u * ss + j] = new Tuple<int, int, double> {
                //                    First = u,
                //                    Second = j,
                //                    Third = costmats[i][u, j]
                //                };
                //            }
                //        }
                //        Array.Sort(G[i], (da, db) => da.Third.CompareTo(db.Third));
                //    }
                //}
                //#endregion
                //var d_Q_S = new double[templatecount];
                //#region 求出每个Si和Q的d(Q, Si)
                //{
                //    for (int i = 0; i < templatecount; ++i) {
                //        var Gi = G[i].Take(k);
                //        foreach (var g in Gi) {
                //            int u = g.First, mu = g.Second;
                //            double d = g.Third;
                //            double Nu = G.Average(gi => gi.First(t => t.First == u).Third);
                //            d_Q_S[i] += d / Nu;
                //        }
                //        d_Q_S[i] /= k;
                //    }
                //}
                //#endregion
                //var firstmG = new Tuple<Tuple<int, int, double>[], double, int>[m]; // <G, d, i> <=> <<u, m(u), d>[], d, i>
                //#region 根据d(Q, Si)截取前20个最好的Gi
                //{
                //    var firstmdQS = d_Q_S.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
                //        .OrderBy(p => p.Value)
                //        .Take(firstmG.Length).ToArray();
                //    for (int p = 0; p < firstmG.Length; ++p) {
                //        double d = firstmdQS[p].Value;
                //        int i = firstmdQS[p].Index;
                //        firstmG[p] = new Tuple<Tuple<int, int, double>[], double, int> {
                //            First = G[i].Take(k).ToArray(),
                //            Second = d,
                //            Third = i
                //        };
                //    }
                //}
                //#endregion
                //#region 计算每个G的位置
                //var L = new Vector2[m];
                //{
                //    for (int i = 0; i < m; ++i) {
                //        L[i] = Vector2.Zero;
                //        double Mi = 0;
                //        var Gi = firstmG[i];
                //        foreach (var u_mu_d in Gi.First) {
                //            int u = u_mu_d.First, mu = u_mu_d.Second;
                //            double d = u_mu_d.Third;
                //            Vector2 pu = Q_Points[u], pmu = templatePoints[Gi.Third][mu];
                //            L[i] += (1 - d) * (pu - pmu);
                //            Mi += (1 - d);
                //        }
                //        L[i] /= Mi;

                //        g.DrawString(templateChars[Gi.Third], new Font("Arial", 12), Brushes.Green,
                //                     new PointF((float)L[i].X, (float)L[i].Y));
                //    }
                //}
                //#endregion
                mmg.Save(Path.Combine(@"D:\Play Data\测试玩意", filename + ".bmp"));
                Debug("{0}\t用时{1}ms.", filename, timer.Stop());
            }
            #endregion
        }

        private static void testCAPTCHA() {
            var timer = new MyTimer();
            int s = 100, K = 100, m = 180;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\my_template_data\scq-" + K + "-" + m, "*.scq")
                .Take(10000).ToArray();
            #region 打开量化的模板
            int templatecount = templatefiles.Length;
            Debug("打开{0}个量化模板----------------------------------------", templatecount);
            timer.Restart();
            int[][] templatehistograms = new int[templatecount][];
            string[] templatechars = new string[templatecount];
            for (int f = 0; f < templatecount; ++f) {
                string file = templatefiles[f];
                string filename = Path.GetFileNameWithoutExtension(file);
                templatechars[f] = filename.Split('-')[1];
                templatehistograms[f] = new int[K];
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int i = 0; i < K; ++i) {
                            templatehistograms[f][i] = br.ReadInt32();
                        }
                    }
                }
            }
            Debug("打开完成，用时{0}ms.", timer.Stop());

            #endregion

            #region 打开Shapeme
            Debug("打开{0}个Shapeme.", K);
            timer.Restart();
            double[][] shapemes = new double[K][];
            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\my_template_data\sm-" + K, m + ".sm"), FileMode.Open)) {
                using (var br = new BinaryReader(fs)) {
                    for (int i = 0; i < K; ++i) {
                        shapemes[i] = new double[60];
                        for (int k = 0; k < 60; ++k) {
                            shapemes[i][k] = br.ReadDouble();
                        }
                    }
                }
            }

            Debug("Shapeme读取完成，用时{0}ms.", timer.Stop());
            #endregion

            #region 识别
            foreach (var challengeFile in Directory.GetFiles(@"D:\Play Data\字符\", "*.jpg")) {
                timer.Restart();
                string filename = Path.GetFileNameWithoutExtension(challengeFile);
                Image<Gray, Byte> img = new Image<Gray, byte>(challengeFile);
                var mmp = img.Convert<Bgr, Byte>();
                Graphics g = Graphics.FromImage(mmp.Bitmap);
                double width_height_ratio = (double)img.Width / img.Height;
                int sq = (int)(s * width_height_ratio * 1.2);
                int randcount = (int)(width_height_ratio * 2);
                //int windowcount = (int)(width_height_ratio * 2),
                //    windowstep = (int)(img.Width / windowcount),
                //    windowwidth = (int)(img.Width * 1.5 / windowcount);
                //int maxrandcount = randcount * 3;
                //Console.WriteLine("宽高比{0:F2},采样{1},随机数{2},随机上限{3}.",
                //    width_height_ratio, sq, randcount, maxrandcount);
                int slice = 16,//(int)(width_height_ratio * width_height_ratio),
                    overlap = 4,
                    windowcount = 7;//slice - (overlap - 1);
                double slice_width = (double)img.Width / slice;
                Console.WriteLine("宽高比{0:F2},切片数{1},切片宽度{2:F2},窗口数{3}.",
                    width_height_ratio, slice, slice_width, windowcount);
                var edge = getEdge(img).Sample(sq);
                foreach (var p in edge) {
                    mmp[p] = new Bgr(0, 0, 127);
                }
                bool[] edge_vi = Utils.InitArray(sq, false);
                #region 计算采样点之间的距离
                ValueIndexPair<double>[][] edgedists = new ValueIndexPair<double>[sq][];
                for (int i = 0; i < sq; ++i) {
                    edgedists[i] = new ValueIndexPair<double>[sq];
                    for (int j = 0; j < sq; ++j) {
                        double xi = edge[i].X, yi = edge[i].Y,
                               xj = edge[j].X, yj = edge[j].Y;
                        double d = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj));
                        edgedists[i][j] = new ValueIndexPair<double> { Value = d, Index = j };
                    }
                    Array.Sort(edgedists[i], (a, b) => a.Value.CompareTo(b.Value));
                }
                #endregion

                var charlist = new List<Tuple<string, int, PointF>>(); // <ch, count, center>
                //for (int rc = 0, rt = 0; rc < randcount && rt < maxrandcount; ++rt) {
                //    #region 随机取一个中心点，并且取离他近的点计算形状上下文
                //int center = rand.Next(sq);
                //if (edge_vi[center]) continue;
                //else rc++;
                //rc++;
                //var nearby = new List<Point>();
                //foreach (var pair in edgedists[center].Take((int)(s * 1.5))) {
                //    nearby.Add(edge[pair.Index]);
                //    edge_vi[pair.Index] = true;
                //}
                //nearby = nearby.Sample(s);
                for (int wd = 0; wd < windowcount; ++wd) {
                    #region 滑动窗口位置
                    //int window_center = wd * windowstep + (windowwidth / 2);
                    double window_center = (wd * 2 + overlap / 2.0) * slice_width;
                    g.DrawLine(Pens.Green, (float)window_center, 0, (float)window_center, img.Height);
                    var nearby = edge.Where(p => Math.Abs(p.X - window_center) < img.Height)
                                     .OrderBy(p => Math.Abs(p.X - window_center))
                                     .Take((int)(s * 1.2))
                                     .ToList().Sample(s);
                    if (nearby.Average(p => Math.Abs(p.X - window_center)) > img.Height) continue;
                    var sc = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC2(nearby);

                    #endregion

                    #region 对待测图的形状上下文进行量化

                    int[] histogram = new int[K];
                    for (int i = 0; i < s; ++i) {
                        double[] ds = new double[K];
                        for (int j = 0; j < K; ++j)
                            ds[j] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(sc[i], shapemes[j]);
                        int id = ds.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                            .OrderBy(p => p.Value)
                            .First().Index;
                        ++histogram[id];
                    }

                    #endregion

                    #region 计算量化后的比较距离

                    double[] dists = new double[templatecount];
                    for (int i = 0; i < templatecount; ++i) {
                        //dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.ChiSquareDistance(histogram, templatehistograms[i]);
                        dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(
                            histogram.Select(d => (double)d).ToArray(),
                            templatehistograms[i].Select(d => (double)d).ToArray());
                    }

                    #endregion

                    #region 对结果进行排序和统计

                    var arr = dists.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
                        .OrderBy(p => p.Value)
                        .Select(p => new { Distance = p.Value, Char = templatechars[p.Index] })
                        .Where(p => "0123456789".IndexOf(p.Char) != -1)
                        .ToArray();
                    Dictionary<string, int> matchcount = new Dictionary<string, int>();
                    int knn = 10;
                    foreach (var pair in arr.Take(knn)) {
                        string ch = pair.Char;
                        matchcount[ch] = matchcount.ContainsKey(ch) ? matchcount[ch] + 1 : 1;
                    }
                    var match = matchcount.Select(pair => new { Count = pair.Value, Ch = pair.Key })
                        .Where(v => v.Count > 0)
                        .OrderByDescending(v => v.Count).ToArray();
                    var firstmatch = match[0];
                    PointF newcenter = new PointF(nearby.Average(p => (float)p.X), nearby.Average(p => (float)p.Y));
                    charlist.Add(new Tuple<string, int, PointF> {
                        First = firstmatch.Ch,
                        Second = firstmatch.Count,
                        Third = newcenter
                    });

                    #endregion
                }
                foreach (var tp in charlist) {
                    string ch = tp.First;
                    int acc = tp.Second;
                    PointF center = tp.Third;
                    //g.DrawString(num.ToString(), new Font("Arial", 24), new SolidBrush(Color.FromArgb(40 + 20 * acc, 0, 0)),
                    //            new PointF(center.X - 12, center.Y - 12));
                    g.DrawString(ch, new Font("Arial", 24), (acc >= 5 ? Brushes.Red : Brushes.DarkGreen),
                                new PointF(center.X - 12, center.Y - 12));
                }
                mmp.Save(Path.Combine(@"D:\Play Data\测试玩意\", filename + ".jpg"));
                Debug("{0}-{1}ms", filename, timer.Stop());

            }
            #endregion
        }

        private static void clusterSC() {
            int s = 100, K = 100;
            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\my_template_data\sc\", "*.sc")
                .Take(180).ToArray();
            int templatecount = templatefiles.Length;
            #region 读取模板
            Debug("打开{0}个模板--------------------------------------------", templatecount);
            double[][] templates = new double[templatecount * s][];
            MyTimer timer = new MyTimer();
            timer.Restart();
            for (int i = 0; i < templatefiles.Length; ++i) {
                string file = templatefiles[i];
                string filename = Path.GetFileNameWithoutExtension(file);
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int j = 0; j < s; ++j) {
                            templates[i * s + j] = new double[60];
                            for (int k = 0; k < 60; ++k) {
                                templates[i * s + j][k] = br.ReadDouble();
                            }
                        }
                    }
                }
                if (i % 100 == 0)
                    Debug("已完成{0}个", i);
            }
            Debug("模板读取完成，用时{0}ms.", timer.Stop());
            #endregion

            #region 聚类
            timer.Restart();
            KMeans<double[]> kmeans = new KMeans<double[]>(templates, K,
                Jim.OCR.ShapeContext2D.ShapeContext.HistCost,
                scs => {
                    double[] scnew = new double[60];
                    for (int k = 0; k < 60; ++k) {
                        scnew[k] = scs.Average(sc => sc[k]);
                    }
                    return scnew;
                }
            );
            kmeans.MaxIterate = 100;
            var cluster = kmeans.Cluster();
            Debug("聚类完成，用时{0}ms.", timer.Stop());
            #endregion

            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\my_template_data\sm-" + K, templatecount + ".sm"), FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs)) {
                    for (int i = 0; i < K; ++i) {
                        for (int k = 0; k < 60; ++k) {
                            bw.Write(cluster[i].Center[k]);
                        }
                    }
                }
            }
        }

        private static void clusterGSC() {
            int s = 100, K = 100;
            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\gsc\", "*.gsc")
                .Take(100).ToArray();
            int templatecount = templatefiles.Length;
            #region 读取模板
            Debug("打开{0}个模板--------------------------------------------", templatecount);
            double[][] templates = new double[templatecount * s][];
            MyTimer timer = new MyTimer();
            timer.Restart();
            for (int i = 0; i < templatefiles.Length; ++i) {
                string file = templatefiles[i];
                string filename = Path.GetFileNameWithoutExtension(file);
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int j = 0; j < s; ++j) {
                            templates[i * s + j] = new double[60];
                            for (int k = 0; k < 60; ++k) {
                                templates[i * s + j][k] = br.ReadDouble();
                            }
                        }
                    }
                }
                if (i % 100 == 0)
                    Debug("已完成{0}个", i);
            }
            Debug("模板读取完成，用时{0}ms.", timer.Stop());
            #endregion

            #region 聚类
            timer.Restart();
            KMeans<double[]> kmeans = new KMeans<double[]>(templates, K,
                Jim.OCR.ShapeContext2D.ShapeContext.HistCost,
                scs => {
                    double[] scnew = new double[60];
                    for (int k = 0; k < 60; ++k) {
                        scnew[k] = scs.Average(sc => sc[k]);
                    }
                    return scnew;
                }
            );
            kmeans.MaxIterate = 100;
            var cluster = kmeans.Cluster();
            Debug("聚类完成，用时{0}ms.", timer.Stop());
            #endregion

            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\train_data\gsm-" + K, templatecount + ".gsm"), FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs)) {
                    for (int i = 0; i < K; ++i) {
                        for (int k = 0; k < 60; ++k) {
                            bw.Write(cluster[i].Center[k]);
                        }
                    }
                }
            }
        }

        private static void quantizeSC() {
            MyTimer timer = new MyTimer();
            int s = 100, K = 100, m = 180;
            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\my_template_data\sc\", "*.sc")
                .Skip(0).Take(1000).ToArray();
            int templatecount = templatefiles.Length;

            #region 打开Shapeme
            Debug("打开{0}个Shapeme.", K);
            timer.Restart();
            double[][] shapemes = new double[K][];
            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\my_template_data\sm-" + K, m + ".sm"), FileMode.Open)) {
                using (var br = new BinaryReader(fs)) {
                    for (int i = 0; i < K; ++i) {
                        shapemes[i] = new double[60];
                        for (int k = 0; k < 60; ++k) {
                            shapemes[i][k] = br.ReadDouble();
                        }
                    }
                }
            }
            Debug("Shapeme读取完成，用时{0}ms.", timer.Stop());
            #endregion

            #region 读取模板和量化
            Debug("对{0}个模板进行量化--------------------------------------------", templatecount);
            timer.Restart();
            for (int f = 0; f < templatecount; ++f) {
                string file = templatefiles[f];
                string filename = Path.GetFileNameWithoutExtension(file);

                int[] histogram = new int[K];
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int j = 0; j < s; ++j) {
                            double[] sc = new double[60];
                            for (int k = 0; k < 60; ++k) {
                                sc[k] = br.ReadDouble();
                            }

                            double[] dists = new double[K];
                            for (int k = 0; k < K; ++k)
                                dists[k] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(sc, shapemes[k]);
                            int id = dists.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                                          .OrderBy(p => p.Value)
                                          .First().Index;
                            ++histogram[id];
                        }
                    }
                }
                using (var fs = new FileStream(Path.Combine(@"D:\Play Data\my_template_data\scq-" + K + "-" + m, filename + ".scq"), FileMode.Create)) {
                    using (var bw = new BinaryWriter(fs)) {
                        for (int i = 0; i < K; ++i) {
                            bw.Write(histogram[i]);
                        }
                    }
                }

                if (f % 100 == 0)
                    Debug("已完成{0}个", f);
            }
            Debug("量化完成，用时{0}ms.", timer.Stop());
            #endregion
        }

        private static void quantizeGSC() {
            MyTimer timer = new MyTimer();
            int s = 100, K = 100, m = 100;
            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\gsc\", "*.gsc")
                .Skip(10000).Take(10000).ToArray();
            int templatecount = templatefiles.Length;

            #region 打开Shapeme
            Debug("打开{0}个Shapeme.", K);
            timer.Restart();
            double[][] shapemes = new double[K][];
            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\train_data\gsm-" + K, m + ".gsm"), FileMode.Open)) {
                using (var br = new BinaryReader(fs)) {
                    for (int i = 0; i < K; ++i) {
                        shapemes[i] = new double[60];
                        for (int k = 0; k < 60; ++k) {
                            shapemes[i][k] = br.ReadDouble();
                        }
                    }
                }
            }
            Debug("Shapeme读取完成，用时{0}ms.", timer.Stop());
            #endregion

            #region 读取模板和量化
            Debug("对{0}个模板进行量化--------------------------------------------", templatecount);
            timer.Restart();
            for (int f = 0; f < templatecount; ++f) {
                string file = templatefiles[f];
                string filename = Path.GetFileNameWithoutExtension(file);

                int[] histogram = new int[K];
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int j = 0; j < s; ++j) {
                            double[] sc = new double[60];
                            for (int k = 0; k < 60; ++k) {
                                sc[k] = br.ReadDouble();
                            }

                            double[] dists = new double[K];
                            for (int k = 0; k < K; ++k)
                                dists[k] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(sc, shapemes[k]);
                            int id = dists.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                                          .OrderBy(p => p.Value)
                                          .First().Index;
                            ++histogram[id];
                        }
                    }
                }
                using (var fs = new FileStream(Path.Combine(@"D:\Play Data\train_data\gscq-" + K + "-" + m, filename + ".gscq"), FileMode.Create)) {
                    using (var bw = new BinaryWriter(fs)) {
                        for (int i = 0; i < K; ++i) {
                            bw.Write(histogram[i]);
                        }
                    }
                }

                if (f % 100 == 0)
                    Debug("已完成{0}个", f);
            }
            Debug("量化完成，用时{0}ms.", timer.Stop());
            #endregion
        }

        private static void computeTemplateSC(int start, int count) {
            int s = 100;
            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\my_template\", "*.jpg")
                .Skip(start).Take(count).ToArray();
            int templatecount = templatefiles.Length;
            Debug("解析{0}个模板--------------------------------------------", templatecount);
            MyTimer timer = new MyTimer();
            timer.Restart();
            for (int i = 0; i < templatecount; ++i) {
                string file = templatefiles[i];
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                Image<Gray, Byte> img = new Image<Gray, byte>(file);
                var list = getEdge(img.Resize(2.5, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR)).Sample(s);
                if (i % 100 == 0)
                    Debug("已完成{0}个", i);
                var sc = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC(list);
                using (var fs = new FileStream(Path.Combine(@"D:\Play Data\my_template_data\sc\", filename + ".sc"), FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs)) {
                        for (int j = 0; j < s; ++j) {
                            for (int k = 0; k < 60; ++k) {
                                bw.Write(sc[j, k]);
                            }
                        }
                    }
                }
            }
            Debug("模板计算完成，用时{0}ms.", timer.Stop());
        }

        private static void computeTemplateGSC(int start, int count) {
            int s = 100;
            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train\", "*.bmp")
                .Skip(start).Take(count).ToArray();
            int templatecount = templatefiles.Length;
            Debug("解析{0}个模板--------------------------------------------", templatecount);
            MyTimer timer = new MyTimer();
            timer.Restart();
            for (int i = 0; i < templatecount; ++i) {
                string file = templatefiles[i];
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                Image<Gray, Byte> img = new Image<Gray, byte>(file).Resize(2.5, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                var list = getEdge(img).Sample(s);
                var sc = Jim.OCR.ShapeContext2D.ShapeContext.ComputeGSC(img, list);
                using (var fs = new FileStream(Path.Combine(@"D:\Play Data\train_data\gsc\", filename + ".gsc"), FileMode.Create)) {
                    using (var bw = new BinaryWriter(fs)) {
                        for (int j = 0; j < s; ++j) {
                            for (int k = 0; k < 60; ++k) {
                                bw.Write(sc[j, k]);
                            }
                        }
                    }
                }
                if (i % 100 == 0)
                    Debug("已完成{0}个", i);
            }
            Debug("模板计算完成，用时{0}ms.", timer.Stop());
        }

        private static void fastPruningWithRSC() {
            int s = 100, r = 5;
            MyTimer timer = new MyTimer();

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\sc\", "*.sc")
                .Take(1000).ToArray();
            #region 打开模板
            string templatepath = @"D:\Play Data\template\";
            int templatecount = templatefiles.Length;
            Debug("打开{0}个模板--------------------------------------------", templatecount);
            double[][,] templates = new double[templatecount][,];
            int[] templatenums = new int[templatecount];
            timer.Restart();
            for (int i = 0; i < templatefiles.Length; ++i) {
                string file = templatefiles[i];
                string filename = Path.GetFileNameWithoutExtension(file);
                templatenums[i] = int.Parse(filename.Split('-')[1]);
                templates[i] = new double[s, 60];
                unsafe {
                    #region 指针也快不了多少
                    //byte[] data = File.ReadAllBytes(file);
                    //fixed (byte* ptr = data) {
                    //    //using (var fs = new FileStream(file, FileMode.Open)) {
                    //    //    using (var br = new BinaryReader(fs)) {
                    //    int offset = 0;
                    //    for (int j = 0; j < s; ++j) {
                    //        for (int k = 0; k < 60; ++k) {
                    //            //templates[i][j, k] = br.ReadDouble();
                    //            fixed (double* t = &templates[i][j, k]) {
                    //                for (int p = 0; p < sizeof(double); ++p)
                    //                    *((byte*)t + p) = *(ptr + offset++);
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    using (var fs = new FileStream(file, FileMode.Open)) {
                        using (var br = new BinaryReader(fs)) {
                            for (int j = 0; j < s; ++j) {
                                for (int k = 0; k < 60; ++k) {
                                    templates[i][j, k] = br.ReadDouble();
                                }
                            }
                        }
                    }
                }
                if (i % 100 == 0)
                    Debug("已完成{0}个", i);
            }
            Debug("模板读取完成，用时{0}ms.", timer.Stop());
            #endregion

            string[] testfiles = Directory.GetFiles(@"D:\Play Data\test\", "*.bmp")
                .Take(100).ToArray();
            #region 测试
            int testcase = testfiles.Length, acc = 0;
            Debug("为{0}个对象寻找候选模板------------------------------------------", testcase);
            foreach (var file in testfiles) {
                timer.Restart();
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                int thisnum = int.Parse(filename.Split('-')[1]);
                Image<Gray, Byte> img = new Image<Gray, byte>(file);
                var list = getEdge(img.Resize(2.5, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR)).Sample(s);
                var sc100 = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC(list);
                double[,] scq = new double[r, 60];
                for (int i = 0; i < 5; ++i) {
                    int pos = rand.Next(s);
                    for (int k = 0; k < 60; ++k) {
                        scq[i, k] = sc100[pos, k];
                    }
                }
                var arr = new ValueIndexPair<double>[templatecount];
                for (int i = 0; i < templatecount; ++i) {
                    double[,] sci = templates[i];
                    double[,] costmat = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(scq, sci);
                    double cost = 0;
                    for (int j = 0; j < 5; ++j) {
                        double min = double.MaxValue;
                        for (int u = 0; u < s; ++u) {
                            double val = costmat[j, u];
                            if (val < min) min = val;
                        }
                        cost += min;
                    }
                    arr[i] = new ValueIndexPair<double> { Value = cost, Index = i };
                }
                Array.Sort(arr, (a, b) => a.Value.CompareTo(b.Value));
                int[] matchcount = new int[10];
                double[] matchcost = new double[10];
                int knn = 10;
                foreach (var pair in arr.Take(knn)) {
                    int num = templatenums[pair.Index];
                    matchcount[num]++;
                    matchcost[num] += pair.Value;
                }
                var match = matchcount.Select((val, i) => new { Count = val, Num = i })
                                      .Where(v => v.Count > 0)
                                      .OrderByDescending(v => v.Count).ToArray();
                //var match = matchcost.Select((val, i) => new { Cost = val / matchcount[i], Num = i })
                //                     .Where(v => !double.IsNaN(v.Cost))
                //                     .OrderBy(v => v.Cost).ToArray();

                #region 进行精细匹配，效果一般
                //double[] matchrate = new double[10];
                //foreach (var m in match) {
                //    if (m.Count == 0) break;
                //    string template = Path.Combine(templatepath, m.Num + ".bmp");
                //    Jim.OCR.ShapeContext2D.ShapeContext sc = new Jim.OCR.ShapeContext2D.ShapeContext(file, template);
                //    sc.debug_flag = false;
                //    sc.timer_flag = false;
                //    sc.display_flag = false;
                //    sc.n_iter = 3;
                //    sc.matchScale = 2.5;
                //    sc.maxsamplecount = 100;
                //    matchrate[m.Num] = sc.MatchFile();
                //}
                //var bestmatches = matchrate.Select((val, i) => new { Cost = val, Num = i })
                //                           .Where(m => m.Cost > 0)
                //                           .OrderBy(m => m.Cost).ToArray();
                //int firstmatch = bestmatches[0].Num;
                #endregion

                int firstmatch = match[0].Num;
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = firstmatch == thisnum ? ConsoleColor.Green : ConsoleColor.Red;
                string info = String.Format("{0} {1}ms - {2}\t", filename, timer.Stop(), (firstmatch == thisnum ? "Right" : "Wrong"));
                //foreach (var m in bestmatches.Take(4)) {
                foreach (var m in match) {
                    info += String.Format("{0}/{1}\t", m.Num, m.Count);
                    //info += String.Format("{0}/{1:F3}\t", m.Num, m.Cost);
                    //info += String.Format("{0}/{1:F3}\t", m.Num, m.Cost);
                }
                Debug(info);
                Console.ForegroundColor = fc;

                if (firstmatch == thisnum) {
                    ++acc;
                }
            }
            Debug("测试用例：{0}。正确率{1}。", testcase, acc);
            #endregion
        }

        private static void fastPruningWithShapeme() {
            var timer = new MyTimer();
            int s = 100, K = 100, m = 100;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\scq-" + K + "-" + m, "*.scq")
                .Take(20000).ToArray();
            #region 打开量化的模板
            int templatecount = templatefiles.Length;
            Debug("打开{0}个量化模板----------------------------------------", templatecount);
            timer.Restart();
            int[][] templatehistograms = new int[templatecount][];
            int[] templatenums = new int[templatecount];
            for (int f = 0; f < templatecount; ++f) {
                string file = templatefiles[f];
                string filename = Path.GetFileNameWithoutExtension(file);
                templatenums[f] = int.Parse(filename.Split('-')[1]);
                templatehistograms[f] = new int[K];
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int i = 0; i < K; ++i) {
                            templatehistograms[f][i] = br.ReadInt32();
                        }
                    }
                }
            }
            Debug("打开完成，用时{0}ms.", timer.Stop());

            #endregion

            #region 打开Shapeme
            Debug("打开{0}个Shapeme.", K);
            timer.Restart();
            double[][] shapemes = new double[K][];
            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\train_data\sm-" + K, m + ".sm"), FileMode.Open)) {
                using (var br = new BinaryReader(fs)) {
                    for (int i = 0; i < K; ++i) {
                        shapemes[i] = new double[60];
                        for (int k = 0; k < 60; ++k) {
                            shapemes[i][k] = br.ReadDouble();
                        }
                    }
                }
            }

            Debug("Shapeme读取完成，用时{0}ms.", timer.Stop());
            #endregion

            string[] testfiles = Directory.GetFiles(@"D:\Play Data\test\", "*.bmp")
                .Take(1000).ToArray();
            #region 测试
            int testcase = testfiles.Length, acc = 0;
            Debug("为{0}个对象寻找候选模板------------------------------------------", testcase);
            foreach (var file in testfiles) {
                timer.Restart();
                #region 计算待测图的形状上下文
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                int thisnum = int.Parse(filename.Split('-')[1]);
                Image<Gray, Byte> img = new Image<Gray, byte>(file);
                var list = getEdge(img.Resize(2.5, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR)).Sample(s);
                var sc = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC2(list);
                #endregion

                #region 对待测图的形状上下文进行量化
                int[] histogram = new int[K];
                for (int i = 0; i < s; ++i) {
                    double[] ds = new double[K];
                    for (int j = 0; j < K; ++j)
                        ds[j] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(sc[i], shapemes[j]);
                    int id = ds.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                                  .OrderBy(p => p.Value)
                                  .First().Index;
                    ++histogram[id];
                }
                #endregion

                #region 计算量化后的比较距离
                double[] dists = new double[templatecount];
                for (int i = 0; i < templatecount; ++i) {
                    dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.ChiSquareDistance(histogram, templatehistograms[i]);
                    //dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(histogram.Cast<double>().ToArray(), templatehistograms[i].Cast<double>().ToArray());
                }
                #endregion

                #region 对结果进行排序和统计
                var arr = dists.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
                                 .OrderBy(p => p.Value)
                                 .Select(p => new { Distance = p.Value, Num = templatenums[p.Index] })
                                 .ToArray();
                int[] matchcount = new int[10];
                int knn = 10;
                foreach (var pair in arr.Take(knn)) {
                    int num = pair.Num;
                    matchcount[num]++;
                }
                var match = matchcount.Select((val, i) => new { Count = val, Num = i })
                                      .Where(v => v.Count > 0)
                                      .OrderByDescending(v => v.Count).ToArray();
                #endregion

                int firstmatch = match[0].Num;
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = firstmatch == thisnum ? ConsoleColor.Green : ConsoleColor.Red;
                string info = String.Format("{0} {1}ms - {2}\t", filename, timer.Stop(), (firstmatch == thisnum ? "Right" : "Wrong"));
                foreach (var ma in match.Take(4)) {
                    info += String.Format("{0}/{1}\t", ma.Num, ma.Count);
                }
                Debug(info);
                Console.ForegroundColor = fc;

                if (firstmatch == thisnum) {
                    ++acc;
                }
            }
            Debug("测试用例：{0}。正确率{1}。", testcase, acc);
            #endregion
        }

        private static void fastPruningWithGShapeme() {
            var timer = new MyTimer();
            int s = 100, K = 100, m = 100;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\gscq-" + K + "-" + m, "*.gscq")
                .Take(20000).ToArray();
            #region 打开量化的模板
            int templatecount = templatefiles.Length;
            Debug("打开{0}个量化模板----------------------------------------", templatecount);
            timer.Restart();
            int[][] templatehistograms = new int[templatecount][];
            int[] templatenums = new int[templatecount];
            for (int f = 0; f < templatecount; ++f) {
                string file = templatefiles[f];
                string filename = Path.GetFileNameWithoutExtension(file);
                templatenums[f] = int.Parse(filename.Split('-')[1]);
                templatehistograms[f] = new int[K];
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int i = 0; i < K; ++i) {
                            templatehistograms[f][i] = br.ReadInt32();
                        }
                    }
                }
            }
            Debug("打开完成，用时{0}ms.", timer.Stop());

            #endregion

            #region 打开Shapeme
            Debug("打开{0}个Shapeme.", K);
            timer.Restart();
            double[][] shapemes = new double[K][];
            using (var fs = new FileStream(Path.Combine(@"D:\Play Data\train_data\gsm-" + K, m + ".gsm"), FileMode.Open)) {
                using (var br = new BinaryReader(fs)) {
                    for (int i = 0; i < K; ++i) {
                        shapemes[i] = new double[60];
                        for (int k = 0; k < 60; ++k) {
                            shapemes[i][k] = br.ReadDouble();
                        }
                    }
                }
            }

            Debug("Shapeme读取完成，用时{0}ms.", timer.Stop());
            #endregion

            string[] testfiles = Directory.GetFiles(@"D:\Play Data\test\", "*.bmp")
                .Take(1000).ToArray();
            #region 测试
            int testcase = testfiles.Length, acc = 0;
            Debug("为{0}个对象寻找候选模板------------------------------------------", testcase);
            foreach (var file in testfiles) {
                timer.Restart();
                #region 计算待测图的形状上下文
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                int thisnum = int.Parse(filename.Split('-')[1]);
                Image<Gray, Byte> img = new Image<Gray, byte>(file).Resize(2.5, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                var list = getEdge(img).Sample(s);
                var sc = Jim.OCR.ShapeContext2D.ShapeContext.ComputeGSC2(img, list);
                #endregion

                #region 对待测图的形状上下文进行量化
                int[] histogram = new int[K];
                for (int i = 0; i < s; ++i) {
                    double[] ds = new double[K];
                    for (int j = 0; j < K; ++j)
                        ds[j] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(sc[i], shapemes[j]);
                    int id = ds.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                                  .OrderBy(p => p.Value)
                                  .First().Index;
                    ++histogram[id];
                }
                #endregion

                #region 计算量化后的比较距离
                double[] dists = new double[templatecount];
                for (int i = 0; i < templatecount; ++i) {
                    dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.ChiSquareDistance(histogram, templatehistograms[i]);
                    //dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(histogram.Cast<double>().ToArray(), templatehistograms[i].Cast<double>().ToArray());
                }
                #endregion

                #region 对结果进行排序和统计
                var arr = dists.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
                                 .OrderBy(p => p.Value)
                                 .Select(p => new { Distance = p.Value, Num = templatenums[p.Index] })
                                 .ToArray();
                int[] matchcount = new int[10];
                int knn = 10;
                foreach (var pair in arr.Take(knn)) {
                    int num = pair.Num;
                    matchcount[num]++;
                }
                var match = matchcount.Select((val, i) => new { Count = val, Num = i })
                                      .Where(v => v.Count > 0)
                                      .OrderByDescending(v => v.Count).ToArray();
                #endregion

                int firstmatch = match[0].Num;
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = firstmatch == thisnum ? ConsoleColor.Green : ConsoleColor.Red;
                string info = String.Format("{0} {1}ms - {2}\t", filename, timer.Stop(), (firstmatch == thisnum ? "Right" : "Wrong"));
                foreach (var ma in match.Take(4)) {
                    info += String.Format("{0}/{1}\t", ma.Num, ma.Count);
                }
                Debug(info);
                Console.ForegroundColor = fc;

                if (firstmatch == thisnum) {
                    ++acc;
                }
            }
            Debug("测试用例：{0}。正确率{1}。", testcase, acc);
            #endregion
        }

        private static double[,] readPoint(string file) {
            string[] lines = File.ReadAllLines(file);
            var result = new double[lines.Length, 2];
            double xmin = double.MaxValue, xmax = double.MinValue,
                ymin = double.MaxValue, ymax = double.MinValue;
            for (int i = 0; i < lines.Length; ++i) {
                string[] p = lines[i].Split('\t');
                double x = double.Parse(p[0]), y = double.Parse(p[1]);
                result[i, 0] = x;
                result[i, 1] = y;
                if (x < xmin) xmin = x;
                if (x > xmax) xmax = x;
                if (y < ymin) ymin = y;
                if (y > ymax) ymax = y;
            }
            //for (int i = 0; i < lines.Length; ++i) {
            //    double x = result[i, 0], y = result[i, 1];
            //    result[i, 0] -= xmin;
            //    result[i, 1] -= ymin;
            //}
            return result;
        }

        private static double[,] readPoint2(string file) {
            string[] lines = File.ReadAllLines(file);
            double[,] result = new double[lines.Length, 2];
            double xmin = double.MaxValue, xmax = double.MinValue,
                ymin = double.MaxValue, ymax = double.MinValue;
            for (int i = 0; i < lines.Length; ++i) {
                string[] p = lines[i].Split(',');
                double x = double.Parse(p[0]), y = double.Parse(p[1]);
                result[i, 0] = x;
                result[i, 1] = y;
                if (x < xmin) xmin = x;
                if (x > xmax) xmax = x;
                if (y < ymin) ymin = y;
                if (y > ymax) ymax = y;
            }
            for (int i = 0; i < lines.Length; ++i) {
                double x = result[i, 0], y = result[i, 1];
                result[i, 0] = (x - xmin) / (xmax - xmin);
                result[i, 1] = 1 - (y - ymin) / (ymax - ymin);
            }
            return result;
        }

        private static void testMNIST2() {
            List<string> wrong = new List<string>();
            int testcount = 100, pos = 0;

            MyTimer timer = new MyTimer();

            foreach (var file in Directory.GetFiles(@"D:\Play Data\test", "*.bmp")) {
                if (pos == testcount) break;
                string filename = Path.GetFileNameWithoutExtension(file);
                int ans = int.Parse(filename.Split('-')[1]);
                timer.Restart();
                double[] results = new double[10];

                int scale = 8;
                for (int d = 0; d < 10; ++d) {
                    string template = Path.Combine(@"D:\Play Data\template", d + ".bmp");
                    var sc = new Jim.OCR.ShapeContext2D.ShapeContext(file, template);
                    sc.n_iter = 1;
                    //sc.showScale = 6;
                    sc.matchScale = 2.5;
                    sc.maxsamplecount = 100;
                    sc.display_flag = false;
                    sc.debug_flag = false;
                    sc.timer_flag = false;
                    double matchcost = sc.MatchFile();
                    results[d] = matchcost;// / sc.nsamp;
                }
                var sort = results.Select((c, i) => new { Cost = c, Digit = i }).OrderBy(v => v.Cost).ToArray();

                if (sort[0].Digit == ans) {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    string s = String.Format("File:{0}-{1}ms-Right!\t", filename, timer.Stop());
                    for (int i = 0; i < 4; ++i) {
                        s += String.Format("{0}/{1:F4}, ", sort[i].Digit, sort[i].Cost);
                    }
                    Debug(s);
                    Console.ForegroundColor = fc;
                } else {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    //Debug("File:{0}-{1}ms-Opps!", filename, Timer.Stop());
                    wrong.Add(filename);
                    string s = String.Format("File:{0}-{1}ms-Wrong!\t", filename, timer.Stop());
                    for (int i = 0; i < 4; ++i) {
                        s += String.Format("{0}/{1:F4}, ", sort[i].Digit, sort[i].Cost);
                    }
                    Debug(s);
                    Console.ForegroundColor = fc;

                    #region 画对比图
                    //string dir = Path.Combine(@"D:\Play Data\output", filename);
                    //if (Directory.Exists(dir)) Directory.Delete(dir, true);
                    //Directory.CreateDirectory(dir);
                    //for (int d = 0; d < 10; ++d) {
                    //    var sc = isc1[d];
                    //    var sci = isc2[d];
                    //    var km = kms[d];
                    //    int ax = (int)sc.Points.Average(p => p.X),
                    //    ay = (int)sc.Points.Average(p => p.Y),
                    //    bx = (int)sci.Points.Average(p => p.X),
                    //    by = (int)sci.Points.Average(p => p.Y);
                    //    int dx = bx - ax,
                    //        dy = by - ay;

                    //    Image<Bgr, Byte> img = new Image<Bgr, byte>(28 * scale, 28 * scale);

                    //    Font fs = new Font("Arial", 18);
                    //    using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                    //        g.Clear(Color.Black);

                    //        for (int j = 0; j < sci.Points.Count; ++j) {
                    //            int i = km.MatchPair[j + sc.Points.Count];
                    //            Point pb = sci.Points[j], pa = sc.Points[i];
                    //            pb.X -= dx;
                    //            pb.Y -= dy;
                    //            int r = 2;
                    //            g.DrawEllipse(Pens.Red, pa.X * scale - r, pa.Y * scale - r, r * 2 + 1, r * 2 + 1);
                    //            g.DrawEllipse(Pens.Green, pb.X * scale - r, pb.Y * scale - r, r * 2 + 1, r * 2 + 1);

                    //            g.DrawLine(Pens.Gray, new Point(pa.X * scale, pa.Y * scale), new Point(pb.X * scale, pb.Y * scale));
                    //        }
                    //        g.DrawString(results[d].ToString(), fs, Brushes.White, new PointF(0, 0));
                    //    }
                    //    img.Save(Path.Combine(dir, d + ".bmp"));
                    //}
                    #endregion
                }
                ++pos;
            }

            Debug("Wrong：{0}", wrong.Count);
            Debug("Wrong rate:{0:F2}%", 100.0 * wrong.Count / testcount);
        }

        /*    private static void testSC() {
                List<Point> la = prepareImage("Arial", 'Z').Sample(50),
                            lb = prepareImage("Comic Sans MS", 'Z').Sample(50);
                ShapeContext.ShapeContextMatching scm = new ShapeContext.ShapeContextMatching(
                    la.ToArray(), lb.ToArray(),
                    new Size(200, 200), (ps, c) => {
                        return ps;
                        //if (c == -1) return ps;
                        //return ps.ToList().Sample(c).ToArray();
                    });
                scm.DistanceTreshold = 50;

                Timer.Start();
                scm.Calculate();
                Console.WriteLine(Timer.Stop());
                int scale = 4;
                Point[] src = scm.LastSourceSamples.Select(p => new Point(p.X * scale, p.Y * scale)).ToArray(),
                        dst = scm.LastTargetSamples.Select(p => new Point(p.X * scale, p.Y * scale)).ToArray(),
                        trs = scm.ResultPoints.Select(p => new Point(p.X * scale, p.Y * scale)).ToArray();
                Bitmap bmp = new Bitmap(200 * scale, 200 * scale);
                int r = 3;
                Font ff1 = new Font("Arial", 120 * scale);
                Font ff2 = new Font("Comic Sans MS", 120 * scale);
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.Clear(Color.Black);
                    //g.DrawLines(Pens.Pink, src);
                    //g.DrawLines(Pens.LightGreen, dst);
                    //g.DrawLines(Pens.LightBlue, trs);

                    g.DrawString("Z", ff1, new SolidBrush(Color.FromArgb(20, 255, 0, 0)), new PointF(0, 0));
                    //g.DrawString("Z", ff2, new SolidBrush(Color.FromArgb(20, 0, 255, 0)), new PointF(0, 0));
                    //g.FillPolygon(new SolidBrush(Color.FromArgb(20, 0, 255, 0)), dst);
                    for (int i = 0; i < src.Length; ++i) {
                        Point pa = src[i],
                              pb = dst[i],
                              pc = trs[i];

                        g.DrawLine(Pens.Gray, pa, pb);
                        //g.DrawLine(Pens.Purple, pb, pc);
                        //g.DrawLine(Pens.Yellow, pa, pc);

                        g.DrawEllipse(Pens.Red, pa.X - r, pa.Y - r, 2 * r + 1, 2 * r + 1);
                        //g.DrawEllipse(Pens.Green, pb.X - r, pb.Y - r, 2 * r + 1, 2 * r + 1);
                        g.DrawEllipse(Pens.Blue, pb.X - r, pb.Y - r, 2 * r + 1, 2 * r + 1);
                    }
                }
                bmp.Save(@"D:\Play Data\match.bmp");
            }

            private static List<Point> prepareImage(string font, char c) {
                Font f = new Font(font, 120);
                Image<Bgr, Byte> img = new Image<Bgr, byte>(200, 200);
                using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                    g.Clear(Color.Black);
                    g.DrawString(c.ToString(), f, Brushes.White, 0, 0);
                }
                return getEdge(img.Convert<Gray, Byte>());
            }
            */

        private static void testLSW(string font) {
            string path = @"D:\Play Data\LSW\" + font;
            Font f = new Font(font, 240);
            for (int i = 0; i < 26; ++i) {
                char c = (char)(i + 'A');
                Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 400);
                using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                    g.Clear(Color.Black);
                    g.DrawString(c.ToString(), f, Brushes.White, 0, 0);
                }

                var sample = getEdge(img.Convert<Gray, Byte>()).Sample(100);
                using (var sw = new StreamWriter(Path.Combine(path, c + ".txt"))) {
                    foreach (var p in sample) {
                        sw.WriteLine("{0},{1}", p.X, p.Y);
                    }
                }
            }
        }

        private static StreamWriter dbg = new StreamWriter(@"D:\Play Data\debug.txt", false);
        private static void Debug(string fmt, params object[] param) {
            Console.WriteLine(fmt, param);
            dbg.WriteLine(fmt, param);
        }

        private static void testMNIST() {
            var timer = new MyTimer();
            List<Point>[] Edges = new List<Point>[10];
            #region 准备边缘集合
            //Font standardFont = new Font("Arial", 20);
            //for (int i = 0; i < 10; ++i) {
            //    Image<Bgr, Byte> img = new Image<Bgr, byte>(28, 28);
            //    using (Graphics g = Graphics.FromImage(img.Bitmap)) {
            //        g.Clear(Color.Black);
            //        g.DrawString(i.ToString(), standardFont, Brushes.White, 0, 0);
            //    }

            //    Edges[i] = getEdge(img.Convert<Gray, Byte>());
            //    //img.Save(Path.Combine(@"D:\Play Data\", i + ".bmp"));
            //}

            for (int i = 0; i < 10; ++i) {
                string template = Path.Combine(@"D:\Play Data\template", i + ".bmp");
                Image<Gray, Byte> img = new Image<Gray, byte>(template);
                Edges[i] = getEdge(img);
            }

            #endregion

            List<string> wrong = new List<string>();
            int testcount = 100, pos = 0;
            foreach (var file in Directory.GetFiles(@"D:\Play Data\test", "*.bmp")) {
                if (pos == testcount) break;
                string filename = Path.GetFileNameWithoutExtension(file);
                int ans = int.Parse(filename.Split('-')[1]);
                timer.Restart();
                List<Point> edge = getEdge(new Image<Gray, byte>(file));
                ImageShapeContext[] isc1 = new ImageShapeContext[10];
                ImageShapeContext[] isc2 = new ImageShapeContext[10];
                KM[] kms = new KM[10];
                double[] results = new double[10];

                int scale = 8;
                for (int d = 0; d < 10; ++d) {
                    int samplecount = Math.Min(edge.Count, Edges[d].Count);
                    var sc = createISC(edge, samplecount);
                    var sci = createISC(Edges[d], samplecount);
                    var scm = new ShapeContextMatching(sc.ShapeContests, sci.ShapeContests);
                    scm.BuildCostGraph();
                    var km = scm.Match();
                    isc1[d] = sc;
                    isc2[d] = sci;
                    kms[d] = km;
                    results[d] = (double)km.MatchResult / samplecount;
                }
                var sort = results.Select((c, i) => new { Cost = c, Digit = i }).OrderBy(v => v.Cost).ToArray();

                if (sort[0].Digit == ans) {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    string s = String.Format("File:{0}-{1}ms-Right!\t", filename, timer.Stop());
                    for (int i = 0; i < 4; ++i) {
                        s += String.Format("{0}/{1:F4}, ", sort[i].Digit, sort[i].Cost);
                    }
                    Debug(s);
                    Console.ForegroundColor = fc;
                } else {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    wrong.Add(filename);
                    string s = String.Format("File:{0}-{1}ms-Wrong!", filename, timer.Stop());
                    for (int i = 0; i < 5; ++i) {
                        s += String.Format("{0}/{1:F2}, ", sort[i].Digit, sort[i].Cost);
                    }
                    Debug(s);
                    Console.ForegroundColor = fc;

                    string dir = Path.Combine(@"D:\Play Data\output", filename);
                    if (Directory.Exists(dir)) Directory.Delete(dir, true);
                    Directory.CreateDirectory(dir);
                    for (int d = 0; d < 10; ++d) {
                        var sc = isc1[d];
                        var sci = isc2[d];
                        var km = kms[d];
                        int ax = (int)sc.Points.Average(p => p.X),
                        ay = (int)sc.Points.Average(p => p.Y),
                        bx = (int)sci.Points.Average(p => p.X),
                        by = (int)sci.Points.Average(p => p.Y);
                        int dx = bx - ax,
                            dy = by - ay;

                        Image<Bgr, Byte> img = new Image<Bgr, byte>(28 * scale, 28 * scale);

                        Font fs = new Font("Arial", 18);
                        using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                            g.Clear(Color.Black);

                            for (int j = 0; j < sci.Points.Count; ++j) {
                                int i = km.MatchPair[j + sc.Points.Count];
                                Point pb = sci.Points[j], pa = sc.Points[i];
                                pb.X -= dx;
                                pb.Y -= dy;
                                int r = 2;
                                g.DrawEllipse(Pens.Red, pa.X * scale - r, pa.Y * scale - r, r * 2 + 1, r * 2 + 1);
                                g.DrawEllipse(Pens.Green, pb.X * scale - r, pb.Y * scale - r, r * 2 + 1, r * 2 + 1);

                                g.DrawLine(Pens.Gray, new Point(pa.X * scale, pa.Y * scale), new Point(pb.X * scale, pb.Y * scale));
                            }
                            g.DrawString(results[d].ToString(), fs, Brushes.White, new PointF(0, 0));
                        }
                        img.Save(Path.Combine(dir, d + ".bmp"));
                    }
                }
                ++pos;
            }

            Debug("Wrong：{0}", wrong.Count);
            Debug("Wrong rate:{0:F2}%", 100.0 * wrong.Count / testcount);
        }

        private static ImageShapeContext createISC(List<Point> edge, int samplecount) {
            var sample = edge.Sample(samplecount);
            var norm = Jim.OCR.Algorithms.ShapeContext.Normalize(sample);

            var scs = new ImageShapeContext(sample);
            for (int i = 0; i < norm.Length; ++i) {
                scs.ShapeContests[i] = new Jim.OCR.Algorithms.ShapeContext(norm[i]);
            }
            return scs;
        }

        private static List<Point> getEdge(Image<Gray, Byte> img) {
            var canny = img.Canny(new Gray(50), new Gray(50));
            List<Point> edge = new List<Point>();
            for (int y = 0; y < canny.Height; ++y) {
                for (int x = 0; x < canny.Width; ++x) {
                    if (canny[y, x].Intensity > 0) {
                        edge.Add(new Point(x, y));
                    }
                }
            }
            return edge;
        }

        private static void analyzeMNIST() {
            string path = @"D:\Play Data\test";
            int sum = 0, min = int.MaxValue, max = 0, i = 0;
            foreach (string file in Directory.GetFiles(path, "*.bmp")) {
                int ss = 0;
                var img = new Image<Gray, Byte>(file);
                var canny = img.Convert<Gray, Byte>().Canny(new Gray(100), new Gray(60));
                for (int y = 0; y < canny.Height; ++y) {
                    for (int x = 0; x < canny.Width; ++x) {
                        if (canny[y, x].Intensity > 0) {
                            ++ss;
                        }
                    }
                }

                sum += ss;
                if (ss < min) min = ss;
                if (ss > max) max = ss;

                if (i % 100 == 0) Console.WriteLine(i);
                ++i;
            }
            Console.WriteLine("Avg:{0}, Min:{1}, Max:{2}", sum / 10000.0, min, max);
        }

        private static void openMNIST() {
            string imagePath = @"D:\Play Data\train-images.idx3-ubyte";
            string labelPath = @"D:\Play Data\train-labels.idx1-ubyte";
            int w = 28, h = 28, c = 60000, si = 16, sl = 8;
            byte[] imgs = File.ReadAllBytes(imagePath);
            byte[] lbls = File.ReadAllBytes(labelPath);
            for (int i = 0, off = si; i < c; ++i) {
                using (Image<Gray, Byte> img = new Image<Gray, byte>(w, h)) {
                    for (int y = 0; y < h; ++y) {
                        for (int x = 0; x < w; ++x, ++off) {
                            img[y, x] = new Gray(imgs[off]);
                        }
                    }
                    byte lbl = lbls[i + sl];
                    string file = String.Format("{0:D5}-{1}.bmp", i, lbl);
                    img.Save(Path.Combine(@"D:\Play Data\train", file));
                }
                if (i % 100 == 0) Console.WriteLine(i);
            }
        }

        private static void km() {
            /*    string[] ss = @"3 4 6 4 9
    6 4 5 3 8
    7 5 3 4 2
    6 3 2 2 5
    8 4 5 4 7".Split('\n');*/
            string[] ss = @"7 6 4 6 1
4 6 5 7 2
3 5 7 6 8
4 7 8 8 5
2 6 5 6 3".Split('\n');
            int[,] g = new int[5, 5];
            for (int i = 0; i < 5; ++i) {
                var l = ss[i].Split(' ');
                for (int j = 0; j < 5; ++j) {
                    g[i, j] = int.Parse(l[j]);
                }
            }
            var km = new KM(5, g);
            int val = km.Match(false);
            Console.WriteLine(val);
            for (int i = 0; i < 5; ++i) {
                Console.WriteLine("Y{0} -> X{1} @ {2}", i, km.MatchPair[i], g[km.MatchPair[i], i]);
            }
        }

        private static void CreateSample() {
            string path = @"D:\Play Data\毕业设计\测试数据\样本";
            List<char> list = new List<char>();
            for (int i = 0; i < 10; ++i) {
                list.Add((char)((int)'0' + i));
            }
            for (int i = 0; i < 26; ++i) {
                list.Add((char)((int)'a' + i));
                list.Add((char)((int)'A' + i));
            }

            //CreateShapeContext(path, "Arial", 40, 'A');
            //CreateShapeContext(path, "Verdana", 40, 'A');
            //CreateShapeContext(path, "Tahoma", 40, 'A');

            for (int i = 0; i < 26; ++i) {
                MatchShapeContext(path, "Arial", "Comic Sans MS", (char)((int)'A' + i));
            }
        }

        private static void MatchShapeContext(string path, string fa, string fb, char c) {
            MyTimer timer = new MyTimer();
            Console.WriteLine("Doing...{0}", c);
            var sca = CreateShapeContext(path, fa, 100, c);
            var scb = CreateShapeContext(path, fb, 100, c);

            var match = new ShapeContextMatching(sca.ShapeContests, scb.ShapeContests);

            timer.Restart();
            match.BuildCostGraph();
            timer.StopAndSay("Build Graph");

            timer.Restart();
            var km = match.Match();
            timer.StopAndSay("Match");
            Console.WriteLine("Match Result:{0}", km.MatchResult);

            int ax = (int)sca.Points.Average(p => p.X),
                ay = (int)sca.Points.Average(p => p.Y),
                bx = (int)scb.Points.Average(p => p.X),
                by = (int)scb.Points.Average(p => p.Y);
            int dx = bx - ax,
                dy = by - ay;
            int scale = 4;
            Image<Bgr, Byte> img = new Image<Bgr, byte>(400 * scale, 400 * scale);
            string scdir = Path.Combine(path, c.ToString());
            if (Directory.Exists(scdir)) Directory.Delete(scdir, true);
            Directory.CreateDirectory(scdir);
            Font fs = new Font("Arial", 12);
            using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                g.Clear(Color.Black);

                Font font1 = new Font(fa, 240 * scale);
                Font font2 = new Font(fb, 240 * scale);
                g.DrawString(c.ToString(), font1, new SolidBrush(Color.FromArgb(30, 255, 0, 0)), new Point(0, 0));
                g.DrawString(c.ToString(), font2, new SolidBrush(Color.FromArgb(30, 0, 255, 0)), new Point(-dx * scale, -dy * scale));

                for (int j = 0; j < scb.Points.Count; ++j) {
                    int i = km.MatchPair[j + sca.Points.Count];
                    Point pb = scb.Points[j], pa = sca.Points[i];
                    pb.X -= dx;
                    pb.Y -= dy;
                    int r = 3;
                    g.DrawEllipse(Pens.Red, pa.X * scale - r, pa.Y * scale - r, r * 2 + 1, r * 2 + 1);
                    g.DrawEllipse(Pens.Green, pb.X * scale - r, pb.Y * scale - r, r * 2 + 1, r * 2 + 1);

                    g.DrawLine(Pens.Gray, new Point(pa.X * scale, pa.Y * scale), new Point(pb.X * scale, pb.Y * scale));

                    Point ps = new Point(pa.X * scale, pa.Y * scale);
                    g.DrawString(i.ToString(), fs, Brushes.White, ps);

                    string hisa = Path.Combine(scdir, String.Format("{0}-{1}.bmp", i, fa));
                    string hisb = Path.Combine(scdir, String.Format("{0}-{1}.bmp", i, fb));
                    sca.ShapeContests[i].Histogram.ToImage().Save(hisa);
                    scb.ShapeContests[j].Histogram.ToImage().Save(hisb);
                }
            }
            img.Save(Path.Combine(path, String.Format("{0}-{1}-{2}.bmp", fa, fb, c)));
        }

        private static ImageShapeContext CreateShapeContext(string path, string fontFamily, int samplecount, char c) {
            MyTimer timer = new MyTimer();
            Font font = new Font(fontFamily, 240);
            string file = Path.Combine(path, String.Format("{0}-{1}.bmp", fontFamily, c));
            string fontpath = Path.Combine(path, fontFamily);
            Image<Bgr, Byte> img = new Image<Bgr, byte>(400, 400);
            using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                g.Clear(Color.Black);
                g.DrawString(c.ToString(), font, Brushes.White, 0, 0);
            }
            timer.Restart();
            var canny = img.Convert<Gray, Byte>().Canny(new Gray(100), new Gray(60));
            timer.StopAndSay("Canny");

            timer.Restart();
            List<Point> edge = new List<Point>();
            for (int y = 0; y < canny.Height; ++y) {
                for (int x = 0; x < canny.Width; ++x) {
                    if (canny[y, x].Intensity > 0) {
                        edge.Add(new Point(x, y));
                    }
                }
            }
            var sample = edge.Sample(samplecount);
            timer.StopAndSay("Sample");
            using (var sw = new StreamWriter(Path.Combine(path, String.Format("!Sample-{0}-{1}.txt", fontFamily, c)))) {
                foreach (var p in sample) {
                    sw.WriteLine("{0},{1}", p.X, p.Y);
                }
            }

            timer.Restart();
            double min, max;
            var norm = Jim.OCR.Algorithms.ShapeContext.Normalize(sample, out min, out max);
            timer.StopAndSay("Normalize");

            timer.Restart();
            var scs = new ImageShapeContext(sample);
            for (int i = 0; i < norm.Length; ++i) {
                scs.ShapeContests[i] = new Jim.OCR.Algorithms.ShapeContext(norm[i]);
                //var his = sc.Histogram.ToImage();
                var p = sample[i];
                //string hisfile = Path.Combine(fontpath, string.Format("[{0},{1}].bmp", p.X, p.Y));
                //his.Save(hisfile);
                img[p] = new Bgr(0, 0, 255);
            }
            timer.StopAndSay("ShapeContext");
            //foreach (var p in sample) {
            //    var sc = new ShapeContext(p, sample);
            //    var his = sc.Histogram.ToImage();
            //    string hisfile = Path.Combine(fontpath, string.Format("[{0},{1}].bmp", p.X, p.Y));
            //    his.Save(hisfile);
            //    img[p] = new Bgr(0, 0, 255);
            //}

            //var contours = img.Convert<Gray, Byte>().FindContours();
            //img.Draw(contours, new Bgr(0, 0, 255), new Bgr(0, 255, 0), 2, 1);

            //dfsContour(img, contours, 0, 2);
            //foreach (var ii in contours.) {
            //    //Console.WriteLine
            //    img[ii] = new Bgr(0, 0, 255);
            //}
            //img.Save(file);

            return scs;
        }

        static void dfsContour(Image<Bgr, Byte> img, Contour<Point> contour, int dep, int max) {
            //if (dep == max) return;
            if (contour == null) return;
            foreach (var p in contour)
                img[p] = new Bgr(0, 0, 255);
            dfsContour(img, contour.HNext, dep + 1, max);
            dfsContour(img, contour.VNext, dep + 1, max);
        }

        private static void DownloadImage() {
            int n = 200;
            string path = @"D:\Play Data\毕业设计\测试数据\bjmobile";
            //string url = "http://cnbeta.com/validate1.php"; // cnbeta
            string url = "https://passport.bj.chinamobile.com/passport/ValidateNum"; // 北京移动
            //string url = "http://passport.csdn.net/ShowExPwd.aspx"; // CSDN
            Random rand = new Random();

            int count = 100;
            while (count < n) {
                try {
                    HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(url);
                    loHttp.Timeout = 10000;

                    string filename = count.ToString("D3") + ".bmp";
                    string filePath = Path.Combine(path, filename);
                    HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
                    using (FileStream fs = new FileStream(filePath, FileMode.CreateNew)) {
                        Stream s = loWebResponse.GetResponseStream();
                        byte[] buf = new byte[4096];

                        while (true) {
                            int c = s.Read(buf, 0, 4096);
                            if (c == 0)
                                break;
                            fs.Write(buf, 0, c);
                        }
                    }
                    ++count;
                    Console.WriteLine(filename);
                } catch {
                }
            }
        }

        private static void ProcessImage() {
            string dirname = "cnbeta";
            string path = @"D:\Play Data\毕业设计\测试数据\" + dirname;
            string savepath = @"D:\Play Data\毕业设计\测试数据\" + dirname + "-out";

            Stopwatch timer = new Stopwatch();
            using (StreamWriter sw = new StreamWriter(@"D:\Play Data\毕业设计\测试数据\" + dirname + ".txt")) {
                foreach (var filename in Directory.GetFiles(path)) {
                    string file = Path.GetFileNameWithoutExtension(filename);
                    timer.Reset();
                    timer.Start();

                    Image<Bgr, Byte> bmp = new Image<Bgr, byte>(filename);
                    Image<Gray, Byte> gray = bmp.Convert<Gray, Byte>();
                    double threshold = new MaxVarianceBinarization().CalculateThreshold(gray);
                    Image<Gray, Byte> bi = gray.ThresholdBinaryInv(new Gray(threshold), new Gray(255));
                    int it;
                    //Stopwatch timer = Stopwatch.StartNew();
                    Image<Gray, Byte> dn = new AnnealKMeansClusterDenoise().Denoise(bi, 1, out it);
                    timer.Stop();
                    //appendLine("降噪用时{0}ms，迭代次数{1}。", timer.ElapsedMilliseconds, it);
                    Console.WriteLine("文件：{0}，用时：{1}ms， 降噪迭代次数：{2}", file, timer.ElapsedMilliseconds, it);
                    dn.Save(Path.Combine(savepath, file + ".out.bmp"));
                }
            }
        }
    }
}
