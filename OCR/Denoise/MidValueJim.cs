using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;

namespace Jim.OCR.Denoise {
    /// <summary>
    /// 中值滤波Jim版
    /// </summary>
    [ProcessorName("中值滤波Jim版")]
    [ProcessorDescription("用一个窗口的中值作为该窗口中心点的值。")]
    public class MidValueJim/* : IImageProcessor */{
        /// <summary>
        /// 窗口大小，必须是奇数。默认3。越大越容易消除噪声，但也越容易造成笔画破裂。
        /// </summary>
        [DefaultValue(3)]
        [Description("窗口大小，必须是奇数。越大越容易消除噪声，但也越容易造成笔画破裂。")]
        public int WindowSize { get; set; }

        public MidValueJim() {
            WindowSize = 3;
        }

        public Image<Gray, Byte> Process(Image<Gray, Byte> src) {
            int width = src.Width, height = src.Height, windowsize_half = (WindowSize-1)/2;
            var dst = new Image<Gray, Byte>(width, height);
            for (int i = windowsize_half; i < height - windowsize_half; ++i) {
                for (int j = windowsize_half; j < width - windowsize_half; ++j) {
                    if (src[i, j].Intensity == 255) {
                        int sum = 0;
                        for (int ii = -windowsize_half; ii <= windowsize_half; ++ii) {
                            for (int jj = -windowsize_half; jj <= windowsize_half; ++jj) {
                                if (src[i + ii, j + jj].Intensity == 255) ++sum;
                            }
                        }
                        if (sum > (windowsize_half + windowsize_half + 1) * (windowsize_half + windowsize_half + 1) / 2) {
                            dst[i, j] = new Gray(255);
                        } else {
                            dst[i, j] = new Gray(0);
                        }
                    }
                }
            }

            return dst;
        }
    }
}
