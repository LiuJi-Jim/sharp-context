using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;

namespace Jim.OCR.Denoise {
    /// <summary>
    /// 中值滤波
    /// </summary>
    [ProcessorName("中值滤波")]
    [ProcessorDescription("用一个窗口的中值作为该窗口中心点的值。")]
    public class MidValueFilter : IImageProcessor {
        /// <summary>
        /// 窗口大小，必须是奇数。默认3。越大越容易消除噪声，但也越容易造成笔画破裂。
        /// </summary>
        [DefaultValue(3)]
        [Description("窗口大小，必须是奇数。越大越容易消除噪声，但也越容易造成笔画破裂。")]
        public int WindowSize { get; set; }

        public MidValueFilter() {
            WindowSize = 3;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            return src.SmoothMedian(WindowSize);
        }
    }
}
