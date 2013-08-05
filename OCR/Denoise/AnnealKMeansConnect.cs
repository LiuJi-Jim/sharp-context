using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Jim.OCR.Algorithms;

namespace Jim.OCR.Denoise {
    /// <summary>
    /// 连通区域消除（聚类自适应阈值）
    /// </summary>
    [ProcessorName("连通区域消除（聚类自适应阈值）")]
    [ProcessorDescription("使用模拟退火改进的KMeans聚类算法自适应求阈值的连通区域消除。")]
    public class AnnealKMeansConnect : IImageProcessor {
        /// <summary>
        /// 是否8连通（默认false）
        /// </summary>
        [DefaultValue(false)]
        [Description("是否8连通。")]
        public bool Connect8 { get; set; }

        /// <summary>
        /// 聚类最大迭代次数，零散（连通区域多）的图应适当降低。（默认2048）
        /// </summary>
        [DefaultValue(2048)]
        [Description("聚类最大迭代次数，零散（连通区域多）的图应适当降低。")]
        public int MaxIteration { get; set; }

        public AnnealKMeansConnect() {
            Connect8 = false;
            MaxIteration = 2048;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            Connectivity.Connect8 = this.Connect8;
            var cl = src.connectLevel(1, 8, 0).Domains.ToList();

            var dst = src.Clone();

            if (cl.Count > 1) {
                Vector2[] vs = cl.Select(i => new Vector2(i.Area, 0)).ToArray();
                KMeansClustering.MaxIterate = this.MaxIteration;
                var result = KMeansClustering.AnnealCluster(vs, 2);

                int foreCluster = (result[0].Centroid.ModSqr() > result[1].Centroid.ModSqr()) ? 0 : 1;
                for (int i = 0; i < cl.Count; ++i) {
                    if (result.ClusterIndex[i] != foreCluster) {
                        foreach (var p in cl[i].Points) {
                            dst[p] = new Gray(0);
                        }
                    }
                }
            }

            return dst;
        }
    }
}
