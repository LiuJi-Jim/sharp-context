using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Jim.OCR;

namespace Test {
    class OnlyShapeme : TestBase {
        public void Run() {

            double totaltime = 0;

            int s = 100, K = 100, m = 100;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\scq-" + K + "-" + m, "*.scq")
                .Take(2000).ToArray();
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
                var timeused = timer.Stop();
                totaltime += timeused;
                string info = String.Format("{0} {1}ms - {2}\t", filename, timeused, (firstmatch == thisnum ? "Right" : "Wrong"));
                foreach (var ma in match.Take(4)) {
                    info += String.Format("{0}/{1}\t", ma.Num, ma.Count);
                }
                //Debug(info);
                if (firstmatch != thisnum) Console.Write(".");
                Console.ForegroundColor = fc;

                if (firstmatch == thisnum) {
                    ++acc;
                }
            }
            Debug("\nShapeme 测试用例：{0}。正确率{1}。", testcase, acc);

            Console.WriteLine("总用时：{0}ms，平均用时：{1}ms。", totaltime, totaltime / testcase);
            #endregion
        }
    }
}
