using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jim.OCR.Algorithms;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Jim.OCR {
    public class AnnealKMeansClusterDenoise : IDenoise {
        /// <summary>
        /// 连通性，4或8
        /// </summary>
        public int Connective = 4;

        public int MaxSearchDepth = 2048;

        private int backgroundColor = 0;
        private int foregroundColor = 255;

        public byte[] Denoise(byte[] data, int width, int height, int padding, out int it) {
            it = 0;
            byte[,] arr = Utils.ByteArr1DTo2D(data, width, height);
            bool[,] vi = (bool[,])Array.CreateInstance(typeof(bool), height, width);

            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    vi[i, j] = false;
                }
            }

            List<Point> points = new List<Point>();
            List<int> areas = new List<int>();

            for (int i = padding; i < height - padding; ++i) {
                for (int j = padding; j < width - padding; ++j) {
                    if (vi[i, j]) continue;
                    if (arr[i, j] == backgroundColor) continue;
                    int sum = 0;
                    dfsSearch(arr, vi, i, j, width, height, padding, ref sum);
                    points.Add(new Point(j, i));
                    areas.Add(sum);
                }
            }

            if (points.Count > 1) {
                Vector2[] vs = areas.Select(i => new Vector2(i, 0)).ToArray();
                it = 0;
                KMeansCluster result = KMeansClustering.AnnealCluster(vs, 2, out it);

                int bigcluster = (result[0].Centroid.ModSqr() > result[1].Centroid.ModSqr()) ? 0 : 1;
                for (int i = 0; i < points.Count; ++i) {
                    if (result.ClusterIndex[i] != bigcluster) {
                        //dfsColor(arr, points[i].Y, points[i].X, width, height, 1);
                        dfsClear(arr, points[i].Y, points[i].X, width, height, 1);
                    }
                }
            } else {
                dfsColor(arr, points[0].Y, points[0].X, width, height, 1);
            }

            byte[] bytes = Utils.ByteArr2DTo1D(arr);
            return bytes;
        }

        private static readonly int[] dy8 = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private static readonly int[] dx8 = { -1, 0, 1, 1, 1, 0, -1, -1 };
        private static readonly int[] dy4 = { -1, 0, 1, 0 };
        private static readonly int[] dx4 = { 0, 1, 0, -1 };

        private void dfsColor(byte[,] arr, int y, int x, int width, int height, int padding) {
            dfsSetColor(arr, y, x, width, height, padding, 127);
        }

        private void dfsClear(byte[,] arr, int y, int x, int width, int height, int padding) {
            dfsSetColor(arr, y, x, width, height, padding, 0);
        }

        private void dfsSetColor(byte[,] arr, int y, int x, int width, int height, int padding, byte color) {
            arr[y, x] = color;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < padding || ny >= height - padding) continue;
                if (nx < padding || nx >= width - padding) continue;

                if (arr[ny, nx] == foregroundColor) dfsSetColor(arr, ny, nx, width, height, padding, color);
            }
        }

        private void dfsSearch(byte[,] arr, bool[,] vi, int y, int x, int width, int height, int padding, ref int sum) {
            if (sum > MaxSearchDepth) return;
            if (vi[y, x]) return;
            vi[y, x] = true;
            ++sum;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < padding || ny >= height - padding) continue;
                if (nx < padding || nx >= width - padding) continue;

                if (arr[ny, nx] == foregroundColor) dfsSearch(arr, vi, ny, nx, width, height, padding, ref sum);
            }
        }

        public byte[] Denoise(byte[] data, int width, int height) {
            int it = 0;
            return this.Denoise(data, width, height, 0, out it);
        }


        #region 用Emgu.CV.Image的版本

        public Image<Gray, Byte> Denoise(Image<Gray, Byte> img, int padding, out int it) {
            it = 0;
            var output = img.Clone();
            int width = img.Width, height = img.Height;

            if (padding > 0) {
                for (int p = 0; p < padding; ++p) {
                    for (int i = 0; i < height; ++i) {
                        // 左右两边
                        output[i, 0 + p] = ColorDefination.BackColor;
                        output[i, width - 1 - p] = ColorDefination.BackColor;
                    }
                    for (int j = 0; j < width; ++j) {
                        // 上下两边
                        output[0 + p, j] = ColorDefination.BackColor;
                        output[height - 1 - p, j] = ColorDefination.BackColor;
                    }
                }
            } // 清除宽为padding的边框

            bool[,] vi = (bool[,])Array.CreateInstance(typeof(bool), height, width);
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    vi[i, j] = false;
                }
            } // 不知道会不会自动初始化成false，如果会，这段可以去掉

            List<Point> points = new List<Point>();
            List<int> areas = new List<int>();

            for (int i = padding; i < height - padding; ++i) {
                for (int j = padding; j < width - padding; ++j) {
                    if (vi[i, j]) continue;
                    if (img[i, j].Equals(ColorDefination.BackColor)) continue;
                    //int sum = 0;
                    //dfsSearch(img, vi, i, j, padding, ref sum);
                    Point p = new Point(j, i);
                    int sum = bfsSearch(img, vi, p, padding);
                    points.Add(p);
                    areas.Add(sum);
                }
            }

            if (points.Count > 1) {
                //var vs = areas.Select(d => (double)d).ToArray();
                //KMeans<double> kmeans = new KMeans<double>(vs, 2,
                //    (a, b) => Math.Abs(a - b),
                //    arr => arr.Average());
                //var result = kmeans.AnnealCluster(
                //    (a, b) => a + b,
                //    (a, b) => a - b,
                //    (a, b) => a / b);
                Vector2[] vs = areas.Select(i => new Vector2(i, 0)).ToArray();
                var result = KMeansClustering.AnnealCluster(vs, 2, out it);

                int foreCluster = (result[0].Centroid.ModSqr() > result[1].Centroid.ModSqr()) ? 0 : 1;
                //int foreCluster = (result[0].Center > result[1].Center) ? 0 : 1;
                for (int i = 0; i < points.Count; ++i) {
                    if (result.ClusterIndex[i] != foreCluster) {
                        bfsClear(output, points[i], padding);
                    }
                }
            } else {
                //dfsColor(output, points[0].Y, points[0].X, padding);
            }

            return output;
        }

        #region dfs，不好，容易StackOverflow，已淘汰

        private void dfsHighlight(Image<Gray, Byte> img, int y, int x, int padding) {
            dfsSetColor(img, y, x, padding, ColorDefination.BackColor, ColorDefination.HighColor);
        }

        private void dfsClear(Image<Gray, Byte> img, int y, int x, int padding) {
            dfsSetColor(img, y, x, padding, ColorDefination.ForeColor, ColorDefination.BackColor);
        }

        private void dfsSetColor(Image<Gray, Byte> img, int y, int x, int padding, Gray backColor, Gray foreColor) {
            img[y, x] = foreColor;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < padding || ny >= img.Height - padding) continue;
                if (nx < padding || nx >= img.Width - padding) continue;

                if (img[ny, nx].Equals(backColor)) dfsSetColor(img, ny, nx, padding, backColor, foreColor);
            }
        }

        private void dfsSearch(Image<Gray, Byte> img, bool[,] vi, int y, int x, int padding, ref int sum) {
            if (sum > MaxSearchDepth) return;
            if (vi[y, x]) return;
            vi[y, x] = true;
            ++sum;
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            for (int i = 0; i < Connective; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < padding || ny >= img.Height - padding) continue;
                if (nx < padding || nx >= img.Width - padding) continue;
                if (img[ny, nx].Equals(ColorDefination.ForeColor)) dfsSearch(img, vi, ny, nx, padding, ref sum);
            }
        }

        #endregion

        #region bfs

        private void bfsClear(Image<Gray, Byte> img, Point start, int padding) {
            bfsSetColor(img, start, padding, ColorDefination.ForeColor, ColorDefination.BackColor);
        }

        private void bfsHighlight(Image<Gray, Byte> img, Point start, int padding) {
            bfsSetColor(img, start, padding, ColorDefination.ForeColor, ColorDefination.HighColor);
        }

        private void bfsSetColor(Image<Gray, Byte> img, Point start, int padding, Gray backColor, Gray foreColor) {
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(start);
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            while (queue.Count > 0) {
                Point p = queue.Dequeue();
                img[p] = foreColor;
                for (int i = 0; i < Connective; ++i) {
                    int ny = p.Y + dy[i], nx = p.X + dx[i];
                    if (ny < padding || ny >= img.Height - padding) continue;
                    if (nx < padding || nx >= img.Width - padding) continue;
                    if (img[ny, nx].Equals(backColor)) {
                        queue.Enqueue(new Point(nx, ny));
                    }
                }
            }
        }

        private int bfsSearch(Image<Gray, Byte> img, bool[,] vi, Point start, int padding) {
            int sum = 0;
            if (vi[start.Y, start.X]) return 0;
            Queue<Point> queue = new Queue<Point>();
            queue.Enqueue(start);
            int[] dy, dx;
            if (Connective == 8) {
                dy = dy8;
                dx = dx8;
            } else {
                dy = dy4;
                dx = dx4;
            }
            while (queue.Count > 0) {
                Point p = queue.Dequeue();
                if (vi[p.Y, p.X]) continue;
                vi[p.Y, p.X] = true;
                ++sum;
                for (int i = 0; i < Connective; ++i) {
                    int ny = p.Y + dy[i], nx = p.X + dx[i];
                    if (ny < padding || ny >= img.Height - padding) continue;
                    if (nx < padding || nx >= img.Width - padding) continue;
                    if (img[ny, nx].Equals(ColorDefination.ForeColor)) {
                        queue.Enqueue(new Point(nx, ny));
                    }
                }
            }
            return sum;
        }

        #endregion

        #endregion
    }
}
