using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace Jim.OCR {
    public static class GrayLevel {
        public static Image<Gray, Byte> ToGrayN(this Image<Gray, Byte> src, int level) {
            int height = src.Height, width = src.Width, grade = 256 / level;
            Image<Gray, Byte> dst = new Image<Gray, byte>(width, height);

            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    int gray = 256 - grade - (int)src[y, x].Intensity / grade * grade;
                    dst[y, x] = new Gray(gray);
                }
            }

            return dst;
        }

        public static Image<Gray, Byte> ToGray16(this Image<Gray, Byte> src) {
            return src.ToGrayN(16);
        }
    }
}
