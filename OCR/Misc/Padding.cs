using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using System.ComponentModel;
using System.Drawing;

namespace Jim.OCR.Misc {
    /// <summary>
    /// 去外边，可指定宽度，也可以去掉所有黑边。
    /// </summary>
    [ProcessorName("去外边")]
    [ProcessorDescription("可指定宽度，也可以去掉所有黑边。")]
    public class Pad : IImageProcessor {
        /// <summary>
        /// 为正数时去掉指定宽度外边，为0或负数时去掉所有黑边。（默认值1）
        /// </summary>
        [DefaultValue(1)]
        [Description("为正数时去掉指定宽度外边，为0或负数时去掉所有黑边。")]
        public int Padding { get; set; }

        public Pad() {
            Padding = 1;
        }

        public Image<Gray, byte> Process(Image<Gray, byte> src) {
            int width = src.Width, height = src.Height;
            if (Padding > 0) {
                var rect = new Rectangle(Padding, Padding, width - Padding - Padding, height - Padding - Padding);
                return src.Copy(rect);
            } else {
                int l = 0, r = width - 1, t = 0, b = height - 1;
                bool findL = false, findR = false, findT = false, findB = false;
                #region 找最左
                while (l < width && !findL) {
                    for (int i = 0; i < height; ++i) {
                        if (src[i, l].Intensity > 0) {
                            findL = true;
                            break;
                        }
                    }
                    l++;
                }
                #endregion
                #region 找最右
                while (r >= 0 && !findR) {
                    for (int i = 0; i < height; ++i) {
                        if (src[i, r].Intensity > 0) {
                            findR = true;
                            break;
                        }
                    }
                    r--;
                }
                #endregion
                #region 找最上
                while (t < height && !findT) {
                    for (int j = 0; j < width; ++j) {
                        if (src[t, j].Intensity > 0) {
                            findT = true;
                            break;
                        }
                    }
                    t++;
                }
                #endregion
                #region 找最下
                while (b >= 0 && !findB) {
                    for (int j = 0; j < width; ++j) {
                        if (src[b, j].Intensity > 0) {
                            findB = true;
                            break;
                        }
                    }
                    b--;
                }
                #endregion

                var rect = new Rectangle(l, t, r - l + 1, b - t + 1);
                return src.Copy(rect);
            }
        }
    }
}
