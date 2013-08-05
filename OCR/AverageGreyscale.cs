using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    public class AverageGreyscale : IGreyscale {
        public byte[] Greyscale(byte[] rgb) {
            byte[] result = new byte[rgb.Length / 3];
            for (int i = 0, j = 0; i < rgb.Length; i += 3, ++j) {
                result[j] = (byte)((rgb[i] + rgb[i + 1] + rgb[i + 2]) / 3);
            }
            return result;
        }
    }
}
