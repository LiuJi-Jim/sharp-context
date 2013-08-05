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
    class OnlyRSC :TestBase{
        public void Run() {
            double totaltime = 0;

            int s = 100, r = 5;

            string[] templatefiles = Directory.GetFiles(@"D:\Play Data\train_data\sc\", "*.sc")
                .Take(100).ToArray();
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

            string[] testfiles = Directory.GetFiles(@"D:\Play Data\test\", "*.bmp")
                .Take(1000).ToArray();
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
                var timeused = timer.Stop();
                totaltime += timeused;
                string info = String.Format("{0} {1}ms - {2}\t", filename, timeused, (firstmatch == thisnum ? "Right" : "Wrong"));
                //foreach (var m in bestmatches.Take(4)) {
                foreach (var m in match) {
                    info += String.Format("{0}/{1}\t", m.Num, m.Count);
                    //info += String.Format("{0}/{1:F3}\t", m.Num, m.Cost);
                    //info += String.Format("{0}/{1:F3}\t", m.Num, m.Cost);
                }
                //Debug(info);
                if (firstmatch != thisnum) Console.Write(".");
                Console.ForegroundColor = fc;

                if (firstmatch == thisnum) {
                    ++acc;
                }
            }
            Debug("\nRSC 测试用例：{0}。正确率{1}。", testcase, acc);

            Console.WriteLine("总用时：{0}ms，平均用时：{1}ms。", totaltime, totaltime/testcase);
            #endregion
        }
    }
}
