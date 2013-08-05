using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Jim.OCR.ShapeContext2D;
using System.ComponentModel;

namespace Jim.OCR.Recognize {
    /// <summary>
    /// 使用Shapeme识别单个字符
    /// </summary>
    [ProcessorName("Shapeme")]
    [ProcessorDescription("使用Shapeme识别单个字符")]
    public class ShapemeSingle : IRecognizeSingle {
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

        public ShapemeSingle() {
            Knn = 10;
            Filter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        }

        public Tuple<string, int> Recognize(Image<Gray, byte> challenge) {
            var list = challenge.getEdge().Sample(100);
            var sc = ShapeContext.ComputeSC2(list);
            if (sc.Length < 100) {
                var tmp = new double[100][];
                for (int i = 0; i < 100; ++i) tmp[i] = new double[60];
                for (int i = 0; i < sc.Length; ++i) {
                    Array.Copy(sc[i], tmp[i], 60);
                }
                sc = tmp;
            }

            #region 量化到shapeme
            int[] histogram = new int[100];
            for (int i = 0; i < 100; ++i) {
                double[] ds = new double[100];
                for (int j = 0; j < 100; ++j)
                    ds[j] = ShapeContext.HistCost(sc[i], SC.shapemes[j]);
                int id = ds.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                           .OrderBy(p => p.Value)
                           .First().Index;
                ++histogram[id];
            }
            #endregion
            #region 计算距离
            double[] dists = new double[SC.template_histograms.Length];
            for (int i = 0; i < SC.template_histograms.Length; ++i) {
                dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.ChiSquareDistance(histogram, SC.template_histograms[i]);
                //dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(histogram.Cast<double>().ToArray(), templatehistograms[i].Cast<double>().ToArray());
            }
            #endregion

            #region 对结果排序
            var arr = dists.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
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
            //    result += String.Format("Char:'{0}',Accuracy:{1}/{2}\t", m.Ch, m.Count, Knn);
            //}
            #endregion

            return new Tuple<string, int> { First = match[0].Ch, Second = match[0].Count };
        }
    }
}
