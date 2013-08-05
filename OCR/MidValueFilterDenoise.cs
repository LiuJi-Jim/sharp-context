using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    /// <summary>
    /// 中值滤波
    /// </summary>
    public class MidValueFilterDenoise : IDenoise {
        /// <summary>
        /// 滑动窗口的大小，这个大小是正负两向的，即WindowSize为1时窗口大小为3x3，为2时窗口大小为5*5。
        /// </summary>
        public int WindowSize = 1;

        public byte[] Denoise(byte[] data, int width, int height) {
            byte[,] arr = Utils.ByteArr1DTo2D(data, width, height);
            byte[,] dst = (byte[,])Array.CreateInstance(typeof(byte), height, width);
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    dst[i, j] = 255;
                }
            }
            for (int i = WindowSize; i < height - WindowSize; ++i) {
                for (int j = WindowSize; j < width - WindowSize; ++j) {
                    if (arr[i, j] == 0) {
                        int sum = 0;
                        for (int ii = -WindowSize; ii <= WindowSize; ++ii) {
                            for (int jj = -WindowSize; jj <= WindowSize; ++jj) {
                                if (arr[i + ii, j + jj] == 0) ++sum;
                            }
                        }
                        if (sum > (WindowSize + WindowSize + 1) * (WindowSize + WindowSize + 1) / 2) {
                            dst[i, j] = 0;
                        } else {
                            dst[i, j] = 255;
                        }
                    }
                }
            }

            byte[] result = Utils.ByteArr2DTo1D(dst);
            return result;
        }
    }
}
