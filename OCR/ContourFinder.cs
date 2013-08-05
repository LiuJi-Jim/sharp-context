using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace Jim.OCR {
    public static class ContourFinder {
        public static Image<Bgr, Byte> ColorContours(Image<Gray, Byte> src, Gray bg, Gray fg) {
            int height = src.Height, width = src.Width;
            Image<Bgr, Byte> dst = new Image<Bgr, byte>(width, height);

            var ll = FindConturs(src, bg, fg);
            foreach (var upper in ll[0]) {
                dst[upper.Y, upper.X] = new Bgr(0, 0, 255);
            }
            foreach(var lower in ll[1]) {
                dst[lower.Y, lower.X] = new Bgr(0, 255, 0);
            }

            return dst;
        }

        public static List<Point>[] FindConturs(Image<Gray, Byte> src, Gray bg, Gray fg) {
            List<Point> upper = new List<Point>();
            List<Point> lower = new List<Point>();

            int height = src.Height, width = src.Width;

            Gray last = bg;
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y <= height; ++y) {
                    Gray current;
                    if (y == height) current = bg;
                    else current = src[y, x];

                    if (!last.Equals(current)) {
                        Point p = new Point(x, y);
                        if (last.Equals(bg)) {
                            // 背景 -> 前景
                            upper.Add(p);
                        } else {
                            // 前景 -> 背景
                            lower.Add(p);
                        }
                        last = current;
                    }
                }
            }

            return new List<Point>[2] { upper, lower };
        }
    }
}
