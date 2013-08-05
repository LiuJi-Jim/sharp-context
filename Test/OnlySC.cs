using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jim.OCR;

namespace Test {
    class OnlySC:TestBase {
        public void Run()
        {
            List<string> wrong = new List<string>();
            int testcount = 100, pos = 0;

            foreach (var file in Directory.GetFiles(@"D:\Play Data\test", "*.bmp")) {
                if (pos == testcount) break;
                string filename = Path.GetFileNameWithoutExtension(file);
                int ans = int.Parse(filename.Split('-')[1]);
                timer.Restart();
                double[] results = new double[10];

                int scale = 8;
                for (int d = 0; d < 10; ++d) {
                    string template = Path.Combine(@"D:\Play Data\template", d + ".bmp");
                    var sc = new Jim.OCR.ShapeContext2D.ShapeContext(file, template);
                    sc.n_iter = 3;
                    //sc.showScale = 6;
                    sc.matchScale = 2.5;
                    sc.maxsamplecount = 100;
                    sc.display_flag = false;
                    sc.debug_flag = false;
                    sc.timer_flag = false;
                    double matchcost = sc.MatchFile();
                    results[d] = matchcost;// / sc.nsamp;
                }
                var sort = results.Select((c, i) => new { Cost = c, Digit = i }).OrderBy(v => v.Cost).ToArray();

                if (sort[0].Digit == ans) {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    string s = String.Format("File:{0}-{1}ms-Right!\t", filename, timer.Stop());
                    for (int i = 0; i < 4; ++i) {
                        s += String.Format("{0}/{1:F4}, ", sort[i].Digit, sort[i].Cost);
                    }
                    Debug(s);
                    Console.ForegroundColor = fc;
                } else {
                    var fc = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    //Debug("File:{0}-{1}ms-Opps!", filename, Timer.Stop());
                    wrong.Add(filename);
                    string s = String.Format("File:{0}-{1}ms-Wrong!\t", filename, timer.Stop());
                    for (int i = 0; i < 4; ++i) {
                        s += String.Format("{0}/{1:F4}, ", sort[i].Digit, sort[i].Cost);
                    }
                    Debug(s);
                    Console.ForegroundColor = fc;

                    #region 画对比图
                    //string dir = Path.Combine(@"D:\Play Data\output", filename);
                    //if (Directory.Exists(dir)) Directory.Delete(dir, true);
                    //Directory.CreateDirectory(dir);
                    //for (int d = 0; d < 10; ++d) {
                    //    var sc = isc1[d];
                    //    var sci = isc2[d];
                    //    var km = kms[d];
                    //    int ax = (int)sc.Points.Average(p => p.X),
                    //    ay = (int)sc.Points.Average(p => p.Y),
                    //    bx = (int)sci.Points.Average(p => p.X),
                    //    by = (int)sci.Points.Average(p => p.Y);
                    //    int dx = bx - ax,
                    //        dy = by - ay;

                    //    Image<Bgr, Byte> img = new Image<Bgr, byte>(28 * scale, 28 * scale);

                    //    Font fs = new Font("Arial", 18);
                    //    using (Graphics g = Graphics.FromImage(img.Bitmap)) {
                    //        g.Clear(Color.Black);

                    //        for (int j = 0; j < sci.Points.Count; ++j) {
                    //            int i = km.MatchPair[j + sc.Points.Count];
                    //            Point pb = sci.Points[j], pa = sc.Points[i];
                    //            pb.X -= dx;
                    //            pb.Y -= dy;
                    //            int r = 2;
                    //            g.DrawEllipse(Pens.Red, pa.X * scale - r, pa.Y * scale - r, r * 2 + 1, r * 2 + 1);
                    //            g.DrawEllipse(Pens.Green, pb.X * scale - r, pb.Y * scale - r, r * 2 + 1, r * 2 + 1);

                    //            g.DrawLine(Pens.Gray, new Point(pa.X * scale, pa.Y * scale), new Point(pb.X * scale, pb.Y * scale));
                    //        }
                    //        g.DrawString(results[d].ToString(), fs, Brushes.White, new PointF(0, 0));
                    //    }
                    //    img.Save(Path.Combine(dir, d + ".bmp"));
                    //}
                    #endregion
                }
                ++pos;
            }

            Debug("Wrong：{0}", wrong.Count);
            Debug("Wrong rate:{0:F2}%", 100.0 * wrong.Count / testcount);
        }
    }
}
