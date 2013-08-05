using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Jim.OCR;
using Jim.OCR.Algorithms;

namespace Test {
    class TestCaptcha : TestBase {
        private static readonly string basedir = "csdn";
        public void RunShapmeme() {
            double totaltime = 0;

            int s = 100, K = 100, m = 100;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\毕业设计\train\scq\", "*.scq")
                .Take(20000).ToArray();
            string[] templateSCfiles = Directory.GetFiles(@"D:\Play Data\毕业设计\train\sc\", "*.sc")
                .Take(20000).ToArray();
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

            string[] testfiles = Directory.GetFiles(@"D:\Play Data\" + basedir + "_rename", "*.bmp")
                .Take(1000).ToArray();
            #region 测试
            int testcase = testfiles.Length, acc = 0;
            Debug("为{0}个对象寻找候选模板------------------------------------------", testcase);
            foreach (var file in testfiles) {
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                string thischar = filename.Split('-')[1];
                timer.Restart();
                #region 计算待测图的形状上下文
                Image<Gray, Byte> img = new Image<Gray, byte>(file);
                var list = getEdge(img.Resize(2.5, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR)).Sample(s);
                var sc = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC2(list);
                var sc2 = new double[s, 60];
                for (int i = 0; i < s; ++i) {
                    for (int j = 0; j < 60; ++j) {
                        sc2[i, j] = sc[i][j];
                    }
                }
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
                    if ("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".IndexOf(templatechars[i]) < 0) {
                        dists[i] = double.MaxValue;
                        continue;
                    }
                    dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.ChiSquareDistance(histogram, templatehistograms[i]);
                    //dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(histogram.Cast<double>().ToArray(), templatehistograms[i].Cast<double>().ToArray());

                }
                #endregion

                #region 对结果进行排序和统计
                var arr = dists.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
                                 .OrderBy(p => p.Value);
                int knn = 10;
                #endregion

                #region 进行精细匹配
                var kmresults = new List<ValueIndexPair<double>>();
                foreach (var pair in arr.Take(knn)) {
                    int idx = pair.Index;
                    var scfile = templateSCfiles[idx];
                    // var  sci = new double[s][];
                    var sci = new double[s, 60];
                    using (var fs = new FileStream(scfile, FileMode.Open)) {
                        using (var br = new BinaryReader(fs)) {
                            for (int j = 0; j < s; ++j) {
                                //sci[j] = new double[60];
                                for (int k = 0; k < 60; ++k) {
                                    //sci[j][k] = br.ReadDouble();
                                    sci[j, k] = br.ReadDouble();
                                }
                            }
                        }
                    }
                    var costmat = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(sci, sc2);

                    var costmat_int = new int[s, s];
                    for (int ii = 0; ii < s; ++ii) {
                        for (int jj = 0; jj < s; ++jj) {
                            costmat_int[ii, jj] = (int)(costmat[ii, jj] * 10000);
                        }
                    }
                    var km = new KM(s, costmat_int);
                    km.Match(false);

                    kmresults.Add(new ValueIndexPair<double> { Index = idx, Value = km.MatchResult / 10000.0 });
                }
                kmresults.Sort((a, b) => a.Value.CompareTo(b.Value));
                #endregion

                //int firstmatch = match[0].Num;
                //int firstmatch = templatenums[kmresults[0].Index];
                string firstmatch = templatechars[kmresults[0].Index];
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = firstmatch == thischar ? ConsoleColor.Green : ConsoleColor.Red;
                if (firstmatch != thischar) {
                    if (kmresults.Any(km => templatechars[km.Index] == thischar)) {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                }

                var timeused = timer.Stop();
                totaltime += timeused;
                string info = String.Format("{0} {1}ms\t", filename, timeused);
                //foreach (var m in bestmatches.Take(4)) {
                foreach (var mmm in kmresults.Take(4)) {
                    info += String.Format("{0}/{1:F2}\t", templatechars[mmm.Index], mmm.Value);
                }
                Debug(info);
                //if (firstmatch != thischar) Console.Write(".");
                Console.ForegroundColor = fc;

                if (firstmatch == thischar) {
                    ++acc;
                }
            }
            Debug("\nShapeme and KM 测试用例：{0}。正确率{1}。", testcase, acc);

            Console.WriteLine("总用时：{0}ms，平均用时：{1}ms。", totaltime, totaltime / testcase);
            #endregion
        }

        public void RunRSC() {
            double totaltime = 0;

            int s = 100, r = 20;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\毕业设计\train\sc\", "*.sc")
                .Take(1000).ToArray();
            #region 打开模板
            int templatecount = templatefiles.Length;
            Debug("打开{0}个模板--------------------------------------------", templatecount);
            double[][,] templates = new double[templatecount][,];
            string[] templatechars = new string[templatecount];
            timer.Restart();
            for (int i = 0; i < templatefiles.Length; ++i) {
                string file = templatefiles[i];
                string filename = Path.GetFileNameWithoutExtension(file);
                templatechars[i] = filename.Split('-')[1];
                templates[i] = new double[s, 60];
                using (var fs = new FileStream(file, FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int j = 0; j < s; ++j) {
                            for (int k = 0; k < 60; ++k) {
                                templates[i][j, k] = br.ReadDouble();
                            }
                        }
                    }
                }
                if (i % 100 == 0)
                    Debug("已完成{0}个", i);
            }
            Debug("模板读取完成，用时{0}ms.", timer.Stop());
            #endregion

            string[] testfiles = Directory.GetFiles(@"D:\Play Data\" + basedir + "_rename", "*.bmp")
                .Take(1000).ToArray();
            #region 测试
            int testcase = testfiles.Length, acc = 0, fuzzy = 0;
            Debug("为{0}个对象寻找候选模板------------------------------------------", testcase);
            foreach (var file in testfiles) {
                timer.Restart();
                string filenameext = Path.GetFileName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                string thischar = filename.Split('-')[1];
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
                    if ("ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".IndexOf(templatechars[i]) < 0) {
                        arr[i] = new ValueIndexPair<double> { Index = i, Value = double.MaxValue };
                        continue;
                    }
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
                int knn = 10;

                #region 进行精细匹配
                var kmresults = new List<ValueIndexPair<double>>();
                foreach (var pair in arr.Take(knn)) {
                    int idx = pair.Index;
                    var costmat = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(templates[idx], sc100);

                    var costmat_int = new int[s, s];
                    for (int ii = 0; ii < s; ++ii) {
                        for (int jj = 0; jj < s; ++jj) {
                            costmat_int[ii, jj] = (int)(costmat[ii, jj] * 10000);
                        }
                    }
                    var km = new KM(s, costmat_int);
                    km.Match(false);

                    kmresults.Add(new ValueIndexPair<double> { Index = idx, Value = km.MatchResult / 10000.0 });
                }
                kmresults.Sort((a, b) => a.Value.CompareTo(b.Value));
                #endregion

                string firstmatch = templatechars[kmresults[0].Index];
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = firstmatch == thischar ? ConsoleColor.Green : ConsoleColor.Red;
                if (firstmatch != thischar) {
                    if (kmresults.Any(km => templatechars[km.Index] == thischar)) {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        ++fuzzy;
                    }
                }
                var timeused = timer.Stop();
                totaltime += timeused;
                string info = String.Format("{0} {1}ms\t", filename, timeused);
                foreach (var m in kmresults.Take(4)) {
                    info += String.Format("{0}/{1:F2}\t", templatechars[m.Index], m.Value);
                }
                Debug(info);
                //if (firstmatch != thisnum) Console.Write(".");
                Console.ForegroundColor = fc;

                if (firstmatch == thischar) {
                    ++acc;
                }
            }
            Debug("\nRSC and KM 测试用例：{0}。正确率{1}%。候选正确{2}%。",
                testcase,
                ((double)acc / testcase) * 100,
                ((double)fuzzy / testcase) * 100);

            Console.WriteLine("总用时：{0}ms，平均用时：{1}ms。", totaltime, totaltime / testcase);
            #endregion
        }
    }
}
