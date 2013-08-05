using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using Jim.OCR;

namespace Test {
    class Sampler : TestBase {
        public void Run() {
            var color = new Image<Bgr, byte>(400, 400);
            using (Graphics g = Graphics.FromImage(color.Bitmap)) {
                g.DrawString("A", new Font("Comic Sans MS", 240), Brushes.White, new PointF(0, 0));
            }
            var src = color.Convert<Gray, Byte>();
            var edge = getEdge(src);
            var sample1 = new List<Point>();
            for (int i = 0; i < 100; ++i) {
                sample1.Add(edge[rand.Next(edge.Count)]);
            }
            var sample2 = edge.Sample(100);

            draw(sample1).Save(@"D:\Play Data\sample-1.bmp");
            draw(sample2).Save(@"D:\Play Data\sample-2.bmp");
        }

        private Image<Bgr, Byte> draw(List<Point> list) {
            var img = new Image<Bgr, byte>(400, 400);
            using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                g.Clear(Color.White);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                foreach (var p in list) {
                    g.FillEllipse(Brushes.Black, new Rectangle(p.X, p.Y, 5, 5));
                }
            }
            return img;
        }
    }
}
