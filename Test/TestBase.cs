using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Jim.OCR;

namespace Test {
    class TestBase {
        protected MyTimer timer = new MyTimer();
        protected Random rand = new Random();
        protected void Debug(string format, params object[] args) {
            Console.WriteLine(format, args);
        }
        
        protected static List<Point> getEdge(Image<Gray, Byte> img) {
            var canny = img.Canny(new Gray(50), new Gray(50));
            List<Point> edge = new List<Point>();
            for (int y = 0; y < canny.Height; ++y) {
                for (int x = 0; x < canny.Width; ++x) {
                    if (canny[y, x].Intensity > 0) {
                        edge.Add(new Point(x, y));
                    }
                }
            }
            return edge;
        }
    }
}
