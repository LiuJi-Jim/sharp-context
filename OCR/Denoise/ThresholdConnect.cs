using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;

namespace Jim.OCR.Denoise {
    /// <summary>
    /// 连通区域消除（硬阈值）
    /// </summary>
    [ProcessorName("连通区域消除（硬阈值）")]
    [ProcessorDescription("用一个指定的阈值来消除小连通区域。")]
    public class ThresholdConnect : IImageProcessor {
        /// <summary>
        /// 是否8连通（默认false）
        /// </summary>
        [DefaultValue(false)]
        [Description("是否8连通。")]
        public bool Connect8 { get; set; }

        /// <summary>
        /// 区域面积阈值，小于该值的连通区域将会被消除。（默认值20）
        /// </summary>
        [DefaultValue(20)]
        [Description("区域面积阈值，小于该值的连通区域将会被消除。")]
        public int Threshold { get; set; }

        public ThresholdConnect() {
            Connect8 = false;
            Threshold = 10;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            var dst = src.Clone();

            Connectivity.Connect8 = this.Connect8;
            var cl = src.connectLevel(1, 8, 0);
            foreach (var dm in cl.Domains) {
                if (dm.Area < Threshold) {
                    foreach (var p in dm.Points) {
                        dst[p] = new Gray(0);
                    }
                }
            }

            return dst;
        }
    }
}
