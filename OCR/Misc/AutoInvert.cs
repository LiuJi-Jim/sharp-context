using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;

namespace Jim.OCR.Misc {
    /// <summary>
    /// 自动反色，根据四个角的平均颜色判断是否该反色，目的是让图像变成黑底白字。
    /// </summary>
    [ProcessorName("自动反色")]
    [ProcessorDescription("根据四个角的平均颜色判断是否该反色，目的是让图像变成黑底白字。")]
    public class AutoInvert : IImageProcessor {
        /// <summary>
        /// 边角采样区域（正方形）的大小。（默认为5）
        /// </summary>
        [DefaultValue(5)]
        [Description("边角采样区域（正方形）的大小。")]
        public int Width { get; set; }

        public AutoInvert() {
            Width = 5;
        }

        private bool shouldInvert(Image<Gray, Byte> src) {
            double sum = 0;
            int width = src.Width, height = src.Height;
            for (int i = 0; i < Width; ++i) {
                for (int j = 0; j < Width; ++j) {
                    sum += src[i, j].Intensity; // 左上
                    sum += src[height - 1 - i, j].Intensity; // 左下
                    sum += src[i, width - 1 - j].Intensity; // 右上
                    sum += src[height - 1 - i, width - 1 - j].Intensity; // 右下
                }
            }

            return (sum / (Width * Width)) > 127; // 平均值较大说明背景比较亮，于是要进行反色。
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            if (shouldInvert(src)) return src.Not();
            else return src.Clone();
        }
    }
}
