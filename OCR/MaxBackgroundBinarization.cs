using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Jim.OCR {
    public class MaxBackgroundBinarization {
        public double CalcThreshold(Image<Gray, Byte> img) {
            int[] counts = new int[257];
            counts[256] = int.MinValue;
            for (int i = 0; i < img.Height; ++i) {
                for (int j = 0; j < img.Width; ++j) {
                    ++counts[(int)Math.Ceiling(img[i, j].Intensity)];
                }
            }
            int mg = 256;
            for (int i = 0; i < 256; ++i) {
                if (counts[i] > counts[mg]) mg = i;
            }
            return mg;
        }
    }
}
