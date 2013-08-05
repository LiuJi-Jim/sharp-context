using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;
using System.Drawing;

namespace Jim.OCR.Split {
    /// <summary>
    /// 平均分切割，适合分布系数均匀的图片，切割前可以考虑去下黑边。
    /// </summary>
    [ProcessorName("平均分切割")]
    [ProcessorDescription("适合分布系数均匀的图片，切割前可以考虑去下黑边。")]
    public class Average : ISplit {
        /// <summary>
        /// 分的个数（默认4）
        /// </summary>
        [DefaultValue(4)]
        [Description("分的个数")]
        public int SplitCount { get; set; }

        public Average() {
            SplitCount = 4;
        }

        public Image<Gray, byte>[] Split(Image<Gray, byte> src) {
            var dst = new Image<Gray, byte>[SplitCount];
            int width = src.Width / SplitCount;
            for (int i = 0; i < SplitCount; ++i) {
                var rect = new Rectangle(i * width, 0, width, src.Height);
                dst[i] = src.GetSubRect(rect);
            }

            return dst;
        }
    }
}
