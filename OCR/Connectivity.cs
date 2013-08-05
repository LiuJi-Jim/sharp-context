using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using Iesi.Collections.Generic;
using Jim.OCR.Algorithms;

namespace Jim.OCR {
    public class ConnectLevel {
        public int Level { get; set; }
        public Iesi.Collections.Generic.ISet<ConnectDomain> Domains { get; private set; }
        public Iesi.Collections.Generic.ISet<Point> NotIn { get; private set; }
        public double MeanArea { get; set; }
        public double Variance { get; set; }
        public ConnectLevel() {
            Domains = new HashedSet<ConnectDomain>();
            NotIn = new HashedSet<Point>();
        }
        public void AddDomain(ConnectDomain domain) {
            Domains.Add(domain);
        }
    }
    public class ConnectDomain {
        public Iesi.Collections.Generic.ISet<Point> Points { get; private set; }
        public int Area { get { return Points.Count; } }
        public ConnectDomain() {
            Points = new HashedSet<Point>();
        }
        public void Add(Point p) {
            Points.Add(p);
        }
    }

    public static class Connectivity {
        private static readonly int[,] d8 = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 } };
        private static readonly int[,] d4 = { { -1, 0 }, { 1, 0 }, { 0, 1 }, { 0, -1 } };
        private static int connect = 8;

        public static bool Connect8 {
            get { return connect == 8; }
            set { connect = value ? 8 : 4; }
        }

        public static ConnectLevel[] Connect(this Image<Gray, Byte> src, int level) {
            var result = new ConnectLevel[level];
            for (int i = 0; i < level; ++i) {
                result[i] = connectLevel(src, i, level, 1);
            }
            return result;
        }

        public static Image<Bgr, Byte> ColorLevel(this Image<Gray, Byte> src, ConnectLevel conn) {
            var dst = src.Convert<Bgr, Byte>();
            var color = new Bgr(0, 0, 255);
            foreach (var domain in conn.Domains) {
                foreach (var p in domain.Points) {
                    dst[p.Y, p.X] = color;
                }
            }
            return dst;
        }

        private static int[,] DirVec {
            get { return Connect8 ? d8 : d4; }
        }

        public static ConnectLevel connectLevel(this Image<Gray, Byte> src, int curLv, int level, int padding) {
            ConnectLevel conn = new ConnectLevel { Level = curLv };
            int width = src.Width, height = src.Height;
            var visit = new bool[height, width];
            int grade = 256 / level, color = conn.Level * grade;

            for (int i = padding; i < height - padding; ++i) {
                for (int j = padding; j < width - padding; ++j) {
                    if (visit[i, j]) continue;
                    Point p = new Point(j, i);
                    if (src[i, j].Intensity < color) {
                        conn.NotIn.Add(p);
                    } else {
                        bfsSearch(src, visit, conn, p, color, padding);
                    }
                }
            }
            double[] stats = Stats.CalcMeansAndVariance(conn.Domains.Select(c => (double)c.Area).ToArray());
            conn.MeanArea = stats[0];
            conn.Variance = stats[1];
            return conn;
        }

        #region bfs

        private static int bfsSearch(Image<Gray, Byte> img, bool[,] vi, ConnectLevel conn, Point start, int color, int padding) {
            if (vi[start.Y, start.X]) return 0;

            var domain = new ConnectDomain();

            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(start);
            var dir = DirVec;
            while (queue.Count > 0) {
                Point p = queue.Dequeue();
                if (vi[p.Y, p.X]) continue;
                vi[p.Y, p.X] = true;
                domain.Add(p);
                for (int i = 0; i < connect; ++i) {
                    int ny = p.Y + dir[i, 1], nx = p.X + dir[i, 0];
                    if (ny < padding || ny >= img.Height - padding) continue;
                    if (nx < padding || nx >= img.Width - padding) continue;

                    if (vi[ny, nx]) continue; // 提前判断一下，好选择加入哪个集合

                    Point np = new Point(nx, ny);
                    if (img[ny, nx].Intensity >= color) {
                        queue.Enqueue(np);
                    } else {
                        conn.NotIn.Add(np);
                    }
                }
            }
            conn.AddDomain(domain);
            return domain.Area;
        }

        #endregion
    }
}
