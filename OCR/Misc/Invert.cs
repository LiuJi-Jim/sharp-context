using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Jim.OCR.Misc {
    /// <summary>
    /// 反色
    /// </summary>
    [ProcessorName("反色")]
    [ProcessorDescription("就是反色。")]
    public class Invert : IImageProcessor {
        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            return src.Not();
        }
    }
}
