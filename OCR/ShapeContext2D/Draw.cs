using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        public void Draw(Matrix a, Matrix b, int[] map, Matrix coords, string file, string title) {
            double xmin = double.MaxValue, xmax = double.MinValue,
                   ymin = double.MaxValue, ymax = double.MinValue;
            for (int i = 0; i < a.Rows; ++i) {
                double x = a[i, 0], y = a[i, 1];
                if (x < xmin) xmin = x;
                if (x > xmax) xmax = x;
                if (y < ymin) ymin = y;
                if (y > ymax) ymax = y;
            }
            for (int i = 0; i < b.Rows; ++i) {
                double x = b[i, 0], y = b[i, 1];
                if (x < xmin) xmin = x;
                if (x > xmax) xmax = x;
                if (y < ymin) ymin = y;
                if (y > ymax) ymax = y;
            }
            using (Bitmap bmp = new Bitmap(picWidth + picPadding * 2, picHeight + picPadding * 2)) {
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.Clear(Color.Black);
                    g.DrawString(title, titleFont, Brushes.White, new PointF(0, 0));

                    if (coords != null) {
                        for (int i = 0; i < coords.Rows; ++i) {
                            float x = (float)(((coords[i, 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                y = (float)(((coords[i, 1] - ymin) / (ymax - ymin)) * picHeight + picPadding);

                            g.FillEllipse(new SolidBrush(Color.FromArgb(0x99, 0x99, 0x99)),
                                          x - coordPointR, y - coordPointR, coordPointR * 2 + 1, coordPointR * 2 + 1);
                        }
                    }

                    bool[] va = new bool[a.Rows], vb = new bool[b.Rows];
                    for (int i = 0; i < a.Rows; ++i) {
                        if (map == null) {
                            va[i] = vb[i] = true;
                            float x1 = (float)(((a[i, 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                  y1 = (float)(((a[i, 1] - ymin) / (ymax - ymin)) * picHeight + picPadding),
                                  x2 = (float)(((b[i, 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                  y2 = (float)(((b[i, 1] - ymin) / (ymax - ymin)) * picHeight + picPadding);
                            PointF pa = new PointF(x1, y1), pb = new PointF(x2, y2);

                            //g.DrawLine(Pens.Gray, pa, pb);

                            g.DrawEllipse(Pens.Red, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                            g.DrawEllipse(Pens.Green, pb.X - circleR, pb.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        } else {
                            int j = map[i];
                            if (j >= a.Rows) continue;
                            va[j] = vb[i] = true;
                            float x1 = (float)(((a[map[i], 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                  y1 = (float)(((a[map[i], 1] - ymin) / (ymax - ymin)) * picHeight + picPadding),
                                  x2 = (float)(((b[i, 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                  y2 = (float)(((b[i, 1] - ymin) / (ymax - ymin)) * picHeight + picPadding);
                            PointF pa = new PointF(x1, y1), pb = new PointF(x2, y2);

                            g.DrawLine(new Pen(Color.FromArgb(0x33, 0x33, 0x33)), pa, pb);

                            g.DrawEllipse(Pens.Red, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                            g.DrawEllipse(Pens.Green, pb.X - circleR, pb.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        }
                    }
                    for (int i = 0; i < a.Rows; ++i) {
                        if (!va[i]) {
                            float x1 = (float)(((a[i, 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                  y1 = (float)(((a[i, 1] - ymin) / (ymax - ymin)) * picHeight + picPadding);
                            g.DrawEllipse(Pens.DarkRed, x1 - circleR, y1 - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        }
                    }
                    for (int i = 0; i < b.Rows; ++i) {
                        if (!vb[i]) {
                            float x2 = (float)(((b[i, 0] - xmin) / (xmax - xmin)) * picWidth + picPadding),
                                  y2 = (float)(((b[i, 1] - ymin) / (ymax - ymin)) * picHeight + picPadding);
                            g.DrawEllipse(Pens.DarkGreen, x2 - circleR, y2 - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        }
                    }
                }
                bmp.Save(file);
            }
        }

        public void Draw(Matrix a, Matrix b, int[] map, Matrix coords, int width, int height, string file, string title) {
            using (Bitmap bmp = new Bitmap(width * showScale + picPadding * 2, height * showScale + picPadding * 2)) {
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.Clear(Color.Black);
                    g.DrawRectangle(Pens.White, picPadding, picPadding, width * showScale, height * showScale);
                    g.DrawString(title, titleFont, Brushes.White, new PointF(0, 0));

                    if (coords != null) {
                        for (int i = 0; i < coords.Rows; ++i) {
                            float x = (float)(coords[i, 0] * showScale + picPadding),
                                  y = (float)(coords[i, 1] * showScale + picPadding);

                            //g.FillEllipse(new SolidBrush(Color.FromArgb(0x99, 0x99, 0x99)),
                            //              x - coordPointR, y - coordPointR, coordPointR * 2 + 1, coordPointR * 2 + 1);
                            g.FillRectangle(new SolidBrush(Color.FromArgb(0x99, 0x99, 0x99)),
                                            x - coordPointR, y - coordPointR, coordPointR * 2 + 1, coordPointR * 2 + 1);
                        }
                    }

                    bool[] va = new bool[a.Rows], vb = new bool[b.Rows];
                    for (int i = 0; i < a.Rows; ++i) {
                        if (map == null) {
                            va[i] = vb[i] = true;
                            float x1 = (float)(a[i, 0] * showScale + picPadding),
                                  y1 = (float)(a[i, 1] * showScale + picPadding),
                                  x2 = (float)(b[i, 0] * showScale + picPadding),
                                  y2 = (float)(b[i, 1] * showScale + picPadding);
                            PointF pa = new PointF(x1, y1), pb = new PointF(x2, y2);

                            //g.DrawLine(Pens.Gray, pa, pb);

                            g.DrawEllipse(Pens.Red, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                            g.DrawEllipse(Pens.Green, pb.X - circleR, pb.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        } else {
                            int j = map[i];
                            if (j >= a.Rows) continue;
                            va[j] = vb[i] = true;
                            float x1 = (float)(a[map[i], 0] * showScale + picPadding),
                                  y1 = (float)(a[map[i], 1] * showScale + picPadding),
                                  x2 = (float)(b[i, 0] * showScale + picPadding),
                                  y2 = (float)(b[i, 1] * showScale + picPadding);
                            PointF pa = new PointF(x1, y1), pb = new PointF(x2, y2);

                            g.DrawLine(new Pen(Color.FromArgb(0x33, 0x33, 0x33)), pa, pb);

                            g.DrawEllipse(Pens.Red, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                            g.DrawEllipse(Pens.Green, pb.X - circleR, pb.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        }
                    }
                    for (int i = 0; i < a.Rows; ++i) {
                        if (!va[i]) {
                            float x1 = (float)(a[i, 0] * showScale + picPadding),
                                  y1 = (float)(a[i, 1] * showScale + picPadding);
                            g.DrawEllipse(Pens.DarkRed, x1 - circleR, y1 - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        }
                    }
                    for (int i = 0; i < b.Rows; ++i) {
                        if (!vb[i]) {
                            float x2 = (float)(b[i, 0] * showScale + picPadding),
                                  y2 = (float)(b[i, 1] * showScale + picPadding);
                            g.DrawEllipse(Pens.DarkGreen, x2 - circleR, y2 - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        }
                    }
                }
                bmp.Save(file);
            }
        }

        public void Draw(Matrix V, string file, string title) {
            using (Bitmap bmp = new Bitmap(V.Columns * showScale, V.Rows * showScale)) {
                Font font = new Font("Arial", V.Rows * showScale / 25.0f);
                using (Graphics g = Graphics.FromImage(bmp)) {
                    for (int y = 0; y < V.Rows; ++y) {
                        for (int x = 0; x < V.Columns; ++x) {
                            int val = Math.Max(0, Math.Min((int)(V[y, x] * 255), 255));
                            g.FillRectangle(new SolidBrush(Color.FromArgb(val, val, val)),
                                            x * showScale, y * showScale, showScale, showScale);
                        }
                    }
                    g.DrawString(title, font, Brushes.Green, 0, 0);
                }
                bmp.Save(file);
            }
        }

        public void Draw(Matrix a, Matrix b, Matrix V1, Matrix V2, string file, string title) {
            int width = V1.Columns * showScale, height = V1.Rows * showScale;
            using (Bitmap bmp = new Bitmap(width, height)) {
                Font font = new Font("Arial", height / 25.0f);
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.Clear(Color.Black);
                    g.DrawString(title, font, Brushes.White, 0, 0);
                    for (int y = 0; y < V1.Rows; ++y) {
                        for (int x = 0; x < V1.Columns; ++x) {
                            int v1 = Math.Max(0, Math.Min((int)(V1[y, x] * 255), 255));
                            g.FillRectangle(new SolidBrush(Color.FromArgb(64, v1, 0, 0)),
                                            x * showScale, y * showScale, showScale, showScale);
                            int v2 = Math.Max(0, Math.Min((int)(V2[y, x] * 255), 255));
                            g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, v2, 0)),
                                            x * showScale, y * showScale, showScale, showScale);
                        }
                    }
                    for (int i = 0; i < a.Rows; ++i) {
                        float x1 = (float)(a[i, 0] * showScale),
                              y1 = (float)(a[i, 1] * showScale),
                              x2 = (float)(b[i, 0] * showScale),
                              y2 = (float)(b[i, 1] * showScale);
                        PointF pa = new PointF(x1, y1), pb = new PointF(x2, y2);

                        g.DrawEllipse(Pens.Red, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        g.DrawEllipse(Pens.Green, pb.X - circleR, pb.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);

                    }
                }
                bmp.Save(file);
            }
        }

        public void DrawGradient(Matrix a, Matrix b, Matrix ta, Matrix tb, int width, int height, string file, string title) {
            double arrowlen = 3;
            using (Bitmap bmp = new Bitmap(width * showScale + picPadding * 2, height * showScale + picPadding * 2)) {
                Font font = new Font("Arial", height / 25.0f);
                using (Graphics g = Graphics.FromImage(bmp)) {
                    g.Clear(Color.Black);
                    g.DrawRectangle(Pens.White, picPadding, picPadding, width * showScale, height * showScale);
                    g.DrawString(title, titleFont, Brushes.White, new PointF(0, 0));

                    for (int i = 0; i < a.Rows; ++i) {
                        float x = (float)(a[i, 0] * showScale + picPadding),
                              y = (float)(a[i, 1] * showScale + picPadding),
                              t = (float)ta[i, 0];
                        PointF pa = new PointF(x, y);
                        PointF pb = new PointF(x + (float)(Math.Cos(t) * arrowlen * showScale),
                                               y + (float)(Math.Sin(t) * arrowlen * showScale));

                        g.DrawEllipse(Pens.Red, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        g.DrawLine(Pens.Red, pa, pb);
                    } for (int i = 0; i < b.Rows; ++i) {
                        float x = (float)(b[i, 0] * showScale + picPadding),
                              y = (float)(b[i, 1] * showScale + picPadding),
                              t = (float)tb[i, 0];
                        PointF pa = new PointF(x, y);
                        PointF pb = new PointF(x + (float)(Math.Cos(t) * arrowlen * showScale),
                                               y + (float)(Math.Sin(t) * arrowlen * showScale));

                        g.DrawEllipse(Pens.Green, pa.X - circleR, pa.Y - circleR, circleR * 2 + 1, circleR * 2 + 1);
                        g.DrawLine(Pens.Green, pa, pb);
                    }
                }
                bmp.Save(file);
            }
        }
    }
}
