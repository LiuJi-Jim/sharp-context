using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using Iesi.Collections.Generic;
using System.Drawing;
using Jim.OCR.Algorithms;

namespace Jim.OCR.Grayscale {
    /*
    /// <summary>
    /// 灰度级别连通
    /// </summary>
    [ProcessorName("灰度级别连通")]
    [ProcessorDescription("将图像分成多个灰度级别，按级别进行连通性查找，找出其中最好的级别。")]
    public class GrayscaleConnectLevel : IImageProcessor {
        /// <summary>
        /// 连通级别数（默认8）
        /// </summary>
        [DefaultValue(8)]
        [Description("分多少个级别。")]
        public int Levels { get; set; }

        /// <summary>
        /// 是否八联通（默认false）
        /// </summary>
        [DefaultValue(false)]
        [Description("是否八联通。")]
        public bool Connect8 { get; set; }

        public GrayscaleConnectLevel() {
            Levels = 8;
            Connect8 = false;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            Connectivity.Connect8 = this.Connect8;
            var cls = from cl in src.Connect(this.Levels)
                      where cl.Domains.Count > 1 && cl.MeanArea >= 10 && cl.Variance < 20000
                      select cl;
            var counts = cls.Select(cl => cl.Domains.Count).ToArray();
            var mins = Utils.FindLocalMins(counts);
            var bestlevel = cls.Where((cl, i) => mins.Contains(i))
                               .OrderByDescending(cl => cl.MeanArea)
                               .ThenBy(cl => cl.Domains.Count)
                               .First();
            var dst = src.Clone();
            foreach (var p in bestlevel.NotIn) {
                dst[p] = new Gray(0);
            }

            return dst;
        }
    }*/
}
