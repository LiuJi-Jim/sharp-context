using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace Jim.OCR {
    public class Projecting {
        public static int Scale = 4;

        public static Image<Gray, Byte> VerticalProject(Image<Gray, Byte> img) {
            int[] counter = new int[img.Width];
            for (int i = 0; i < img.Height; ++i) {
                for (int j = 0; j < img.Width; ++j) {
                    if (img[i, j].Equals(ColorDefination.ForeColor)) {
                        ++counter[j];
                    }
                }
            }
            int max = counter.Max(), height = max * Scale;
            Image<Gray, Byte> result = new Image<Gray, byte>(img.Width, height + 1);
            for (int i = 0; i < counter.Length; ++i) {
                int h = counter[i] * Scale;
                if (h == 0) continue;
                Point[] points = new Point[] {
                    new Point(i, height - h),
                    new Point(i + 1, height - h),
                    new Point(i, height),
                    new Point(i + 1, height)
                };
                result.FillConvexPoly(points, new Gray(255));
            }
            return result;
        }
    }
}
