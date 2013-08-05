using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Jim.OCR.Recognize {
    /// <summary>
    /// 使用Representative Shape Context识别单个字符
    /// </summary>
    [ProcessorName("RSC")]
    [ProcessorDescription("使用Representative Shape Context识别单个字符")]
    public class RSCSingle : IRecognizeSingle {
        /// <summary>
        /// 取前几个做KNN。（默认10）
        /// </summary>
        [DefaultValue(10)]
        [Description("取前几个做KNN。")]
        public int Knn { get; set; }

        /// <summary>
        /// 包含的字符。（默认“abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789”）
        /// </summary>
        [DefaultValue("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")]
        [Description("包含的字符")]
        public string Filter { get; set; }

        private static readonly Random rand = new Random();

        public RSCSingle() {
            Knn = 10;
            Filter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        }

        public Tuple<string, int> Recognize(Image<Gray, byte> src) {
            var list = src.getEdge().Sample(100);

            #region 计算形状上下文
            int r = 15;
            var scall = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC(list);
            var scq = new double[r, 60];
            for (int i = 0; i < r; ++i) {
                int pos = rand.Next(list.Count);
                for (int k = 0; k < 60; ++k) {
                    scq[i, k] = scall[pos, k];
                }
            }
            #endregion
            #region 计算距离
            var dists = new ValueIndexPair<double>[SC.template_sc.Length];
            for (int i = 0; i < SC.template_sc.Length; ++i) {
                double[,] sci = SC.template_sc[i];
                double[,] costmat = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(scq, sci);
                double cost = 0;
                for (int j = 0; j < r; ++j) {
                    double min = double.MaxValue;
                    for (int u = 0; u < 100; ++u) {
                        double val = costmat[j, u];
                        if (val < min) min = val;
                    }
                    cost += min;
                }
                dists[i] = new ValueIndexPair<double> { Value = cost, Index = i };
            }
            #endregion

            #region 对结果排序
            var arr = dists
                .OrderBy(p => p.Value)
                .Select(p => new { Distance = p.Value, Char = SC.template_chars[p.Index] })
                //.Where(p => "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".IndexOf(p.Char) != -1)
                .Where(p => Filter.IndexOf(p.Char) != -1)
                .ToArray();
            Dictionary<string, int> matchcount = new Dictionary<string, int>();
            foreach (var pair in arr.Take(Knn)) {
                string ch = pair.Char;
                matchcount[ch] = matchcount.ContainsKey(ch) ? matchcount[ch] + 1 : 1;
            }
            var match = matchcount.Select(pair => new { Count = pair.Value, Ch = pair.Key })
                                  .Where(v => v.Count > 0)
                                  .OrderByDescending(v => v.Count).ToArray();
            //string result = "";
            //foreach (var m in match.Take(3)) {
            //    result += String.Format("Char:'{0}',Accuracy:{1}/{2}\t", m.Ch, m.Count, knn);
            //}
            #endregion

            return new Tuple<string, int> { First = match[0].Ch, Second = match[0].Count };
        }
    }
}
