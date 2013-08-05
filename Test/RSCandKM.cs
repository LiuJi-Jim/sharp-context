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
    class RSCandKM : TestBase {
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

                //int firstmatch = match[0].Num;
                int firstmatch = templatenums[kmresults[0].Index];
                var fc = Console.ForegroundColor;
                Console.ForegroundColor = firstmatch == thisnum ? ConsoleColor.Green : ConsoleColor.Red;
                var timeused = timer.Stop();
                totaltime += timeused;
                string info = String.Format("{0} {1}ms\t", filename, timeused);
                //foreach (var m in bestmatches.Take(4)) {
                foreach (var m in kmresults.Take(4)) {
                    info += String.Format("{0}/{1:F2}\t", templatenums[m.Index], m.Value);
                }
                //Debug(info);
                if (firstmatch != thisnum) Console.Write(".");
                Console.ForegroundColor = fc;

                if (firstmatch == thisnum) {
                    ++acc;
                }
            }
            Debug("\nRSC and KM 测试用例：{0}。正确率{1}。", testcase, acc);

            Console.WriteLine("总用时：{0}ms，平均用时：{1}ms。", totaltime, totaltime / testcase);
            #endregion
        }
    }
}
