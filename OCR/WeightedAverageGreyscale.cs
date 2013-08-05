using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    public class WeightedAverageGreyscale : IGreyscale {
        public byte[] Greyscale(byte[] rgb) {
            byte[] result = new byte[rgb.Length / 3];
            for (int i = 0, j = 0; i < rgb.Length; i += 3, ++j) {
                result[j] = (byte)((rgb[i] * 299 + rgb[i + 1] * 587 + rgb[i + 2] * 114) / 1000);
            }
            return result;
        }
    }
}
