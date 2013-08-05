using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    /// <summary>
    /// 最简单的二值化，用全图灰度平均值做阈值，无实际用途。
    /// </summary>
    public class SimplestBinarization : IBinarization {
        public byte[] Binarization(byte[] grey, int t, int width, int height) {
            byte[] result = new byte[grey.Length];
            for (int i = 0; i < grey.Length; ++i) {
                result[i] = (byte)(grey[i] < t ? 0 : 255);
            }
            return result;
        }

        public byte[] Binarization(byte[] grey, int width, int height) {
            double avg = grey.Average(b => b);
            byte[] result = new byte[grey.Length];
            for (int i = 0; i < grey.Length; ++i) {
                result[i] = (byte)(grey[i] < avg ? 0 : 255);
            }
            return result;
        }
    }
}
