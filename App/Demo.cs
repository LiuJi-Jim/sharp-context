using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System.Threading;
using System.IO;
using Jim.OCR;
using Jim.OCR.ShapeContext2D;

namespace App {
    public partial class Demo : Form {
        public Demo() {
            InitializeComponent();
            imageBoxes.Add(imageBox1);
            imageBoxes.Add(imageBox2);
            imageBoxes.Add(imageBox3);
        }

        #region fields
        private readonly Random rand = new Random();
        private readonly MyTimer timer = new MyTimer();
        private readonly List<ImageBox> imageBoxes = new List<ImageBox>();
        private readonly Stack<Image<Gray, Byte>> imagestack = new Stack<Image<Gray, Byte>>();
        private double[][,] template_sc;
        private int[][] template_histograms;
        private string[] template_chars;
        private double[][] shapemes;
        private readonly List<Image<Gray, Byte>> splitted_chars = new List<Image<Gray, byte>>();
        #endregion

        #region helper
        private delegate void SimpleHandler();
        private void showImage() {
            this.Invoke(new SimpleHandler(() => {
                for (int i = 0; i < imageBoxes.Count; ++i) {
                    imageBoxes[i].Image = null;
                }
                var tmp = new Stack<Image<Gray, Byte>>();
                for (int i = 0; i < imageBoxes.Count && imagestack.Count > 0; ++i) {
                    var image = imagestack.Pop();
                    tmp.Push(image);
                }
                for (int i = 0; i < imageBoxes.Count && tmp.Count > 0; ++i) {
                    var image = tmp.Pop();
                    imagestack.Push(image);
                    imageBoxes[i].Image = image;
                    imageBoxes[i].Width = image.Width + 2;
                }
            }));
        }
        private void showSplit() {
            this.Invoke(new SimpleHandler(() => {
                splitter1.Controls.Clear();
                foreach (var split in splitted_chars.Reverse<Image<Gray, Byte>>()) {
                    if (split.Width > splitter1.SplitPosition) {
                        splitter1.SplitPosition = split.Width;
                    }
                    var box = new ImageBox();
                    box.Dock = DockStyle.Top;
                    box.Height = split.Height + 2;
                    box.Image = split;
                    splitter1.Controls.Add(box);
                }
            }));
        }
        private void showStatus(string text, params object[] objs) {
            this.Invoke(new SimpleHandler(() => { status.Text = String.Format(text, objs); }));
        }
        private void showProgress(double pro) {
            this.Invoke(new SimpleHandler(() => { progress.Value = (int)(pro * 100); }));
        }
        private void enable(Control ctrl) {
            ctrl.Invoke(new SimpleHandler(() => { ctrl.Enabled = true; }));
        }
        private void disable(Control ctrl) {
            ctrl.Invoke(new SimpleHandler(() => { ctrl.Enabled = false; }));
        }
        private void appendLine(string text, params object[] objs) {
            this.Invoke(new SimpleHandler(() => richTextBox1.AppendText(String.Format(text, objs) + "\n")));
        }
        private void clearLog() {
            this.Invoke(new SimpleHandler(richTextBox1.Clear));
        }
        #endregion

        #region threads
        private void openSC(string dir) {
            new Thread(new ThreadStart(() => {
                timer.Restart();
                const int shapeme_count = 100;
                shapemes = new double[shapeme_count][];
                showStatus("加载Shapeme");
                #region Shapeme
                using (var fs = new FileStream(Path.Combine(dir, "data.sm"), FileMode.Open)) {
                    using (var br = new BinaryReader(fs)) {
                        for (int i = 0; i < shapeme_count; ++i) {
                            shapemes[i] = new double[60];
                            for (int k = 0; k < 60; ++k) {
                                shapemes[i][k] = br.ReadDouble();
                            }
                            showProgress((i + 1.0) / shapeme_count);
                        }
                    }
                }
                #endregion

                #region SCQ
                showStatus("加载量化形状上下文");
                var template_files = Directory.GetFiles(Path.Combine(dir, "scq"), "*.scq").ToArray();
                int template_count = template_files.Length;
                template_histograms = new int[template_count][];
                template_chars = new string[template_count];
                for (int f = 0; f < template_count; ++f) {
                    string file = template_files[f];
                    string filename = Path.GetFileNameWithoutExtension(file);
                    template_chars[f] = filename.Split('-')[1];
                    template_histograms[f] = new int[shapeme_count];
                    using (var fs = new FileStream(file, FileMode.Open)) {
                        using (var br = new BinaryReader(fs)) {
                            for (int i = 0; i < shapeme_count; ++i) {
                                template_histograms[f][i] = br.ReadInt32();
                            }
                        }
                    }
                    showProgress((f + 1.0) / template_count);
                }
                #endregion

                #region SC
                showStatus("加载形状上下文");
                var sc_files = Directory.GetFiles(Path.Combine(dir, "sc"), "*.sc").ToArray();
                template_sc = new double[template_count][,];
                for (int f = 0; f < template_count; ++f) {
                    string file = sc_files[f];
                    string filename = Path.GetFileNameWithoutExtension(file);
                    template_sc[f] = new double[100, 60];
                    using (var fs = new FileStream(file, FileMode.Open)) {
                        using (var br = new BinaryReader(fs)) {
                            for (int j = 0; j < 100; ++j) {
                                for (int k = 0; k < 60; ++k) {
                                    template_sc[f][j, k] = br.ReadDouble();
                                }
                            }
                        }
                    }
                    showProgress((f + 1.0) / template_count);
                }
                #endregion
                showStatus("加载完成，用时{0}ms。", timer.Stop());
            })).Start();
        }
        private void threshold() {
            new Thread(new ThreadStart(() => {
                var image = imagestack.Peek();
                double threshold = new MaxVarianceBinarization().CalculateThreshold(image);
                imagestack.Push(image.ThresholdBinaryInv(new Gray(threshold), new Gray(255)));
                showImage();
                showStatus("二值化完成，阈值：{0:F2}。", threshold);
            })).Start();
        }
        private void smoothmask() {
            new Thread(new ThreadStart(() => {
                var image = imagestack.Peek();
                timer.Restart();
                imagestack.Push(image.SmoothMask());
                showImage();
                showStatus("掩模平滑完成，用时{0}ms。", timer.Stop());
            })).Start();
        }
        private void smoothmedian() {
            new Thread(new ThreadStart(() => {
                var image = imagestack.Peek();
                timer.Restart();
                imagestack.Push(image.SmoothMedian(5));
                showImage();
                showStatus("中值滤波完成，用时{0}ms。", timer.Stop());
            })).Start();
        }
        private void connect_denoise() {
            new Thread(new ThreadStart(() => {
                var image = imagestack.Peek();
                timer.Restart();
                int iter = 0;
                imagestack.Push(new AnnealKMeansClusterDenoise().Denoise(image, 1, out iter));
                showImage();
                showStatus("独立连通区域消除完成，迭代次数{0}，用时{1}ms。", iter, timer.Stop());
            })).Start();
        }
        private void connect_split() {
            new Thread(new ThreadStart(() => {
                timer.Restart();
                var image = imagestack.Peek();
                var cl = image.connectLevel(1, 8, 0);
                splitted_chars.Clear();
                var position_image = new List<Tuple<int, Image<Gray, Byte>>>();
                foreach (var domain in cl.Domains) {
                    int most_left = int.MaxValue, most_right = 0;
                    foreach (var point in domain.Points) {
                        if (point.X < most_left) most_left = point.X;
                        if (point.X > most_right) most_right = point.X;
                    }
                    int padding = 5;
                    var split = new Image<Gray, Byte>(padding * 2 + (most_right - most_left), image.Height);
                    foreach (var point in domain.Points) {
                        int x = point.X, y = point.Y;
                        split[y, padding + x - most_left] = image[point];
                    }
                    position_image.Add(new Tuple<int, Image<Gray, byte>> {
                        First = most_left,
                        Second = split
                    });
                }
                foreach (var tp in position_image.OrderBy(t => t.First)) {
                    splitted_chars.Add(tp.Second);
                }
                showSplit();
                showStatus("分割完成，得到{0}个字符，用时{1}ms。", cl.Domains.Count, timer.Stop());
            })).Start();
        }
        private void recognize_rsc() {
            if (template_histograms == null) {
                MessageBox.Show("尚未读取模板。");
                return;
            }
            new Thread(new ThreadStart(() => {
                clearLog();
                showStatus("搜索中");
                foreach (var challenge in splitted_chars) {
                    timer.Restart();
                    var list = challenge.getEdge().Sample(100);

                    #region 计算形状上下文
                    int r = 15;
                    var scall = Jim.OCR.ShapeContext2D.ShapeContext.ComputeSC(list);
                    var scq = new double[r, 60];
                    for (int i = 0; i < r; ++i) {
                        int pos = rand.Next(list.Count);
                        for (int k = 0; k < 60; ++k) {
                            scq[i, k] = scall[pos, k];
                        }
                    }
                    #endregion
                    #region 计算距离
                    var dists = new ValueIndexPair<double>[template_sc.Length];
                    for (int i = 0; i < template_sc.Length; ++i) {
                        double[,] sci = template_sc[i];
                        double[,] costmat = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(scq, sci);
                        double cost = 0;
                        for (int j = 0; j < r; ++j) {
                            double min = double.MaxValue;
                            for (int u = 0; u < 100; ++u) {
                                double val = costmat[j, u];
                                if (val < min) min = val;
                            }
                            cost += min;
                        }
                        dists[i] = new ValueIndexPair<double> { Value = cost, Index = i };
                        showProgress((i + 1.0) / template_histograms.Length);
                    }
                    #endregion

                    #region 对结果排序
                    var arr = dists
                        .OrderBy(p => p.Value)
                        .Select(p => new { Distance = p.Value, Char = template_chars[p.Index] })
                        //.Where(p => "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".IndexOf(p.Char) != -1)
                        .ToArray();
                    Dictionary<string, int> matchcount = new Dictionary<string, int>();
                    int knn = 10;
                    foreach (var pair in arr.Take(knn)) {
                        string ch = pair.Char;
                        matchcount[ch] = matchcount.ContainsKey(ch) ? matchcount[ch] + 1 : 1;
                    }
                    var match = matchcount.Select(pair => new { Count = pair.Value, Ch = pair.Key })
                                          .Where(v => v.Count > 0)
                                          .OrderByDescending(v => v.Count).ToArray();
                    string result = "";
                    foreach (var m in match.Take(3)) {
                        result += String.Format("Char:'{0}',Accuracy:{1}/{2}\t", m.Ch, m.Count, knn);
                    }
                    appendLine(result);
                    #endregion
                }
                appendLine("-----------------------------------------------------");
                showStatus("搜索完成，共用时{0}ms。", timer.Stop());
            })).Start();
        }
        private void recognize_shapeme() {
            if (template_histograms == null) {
                MessageBox.Show("尚未读取模板。");
                return;
            }
            new Thread(new ThreadStart(() => {
                clearLog();
                showStatus("搜索中");
                foreach (var challenge in splitted_chars) {
                    timer.Restart();
                    var list = challenge.getEdge().Sample(100);
                    var sc = ShapeContext.ComputeSC2(list);
                    if (sc.Length < 100) {
                        var tmp = new double[100][];
                        for (int i = 0; i < 100; ++i) tmp[i] = new double[60];
                        for (int i = 0; i < sc.Length; ++i) {
                            Array.Copy(sc[i], tmp[i], 60);
                        }
                        sc = tmp;
                    }

                    #region 量化到shapeme
                    int[] histogram = new int[100];
                    for (int i = 0; i < 100; ++i) {
                        double[] ds = new double[100];
                        for (int j = 0; j < 100; ++j)
                            ds[j] = ShapeContext.HistCost(sc[i], shapemes[j]);
                        int id = ds.Select((v, idx) => new ValueIndexPair<double> { Value = v, Index = idx })
                                   .OrderBy(p => p.Value)
                                   .First().Index;
                        ++histogram[id];
                    }
                    #endregion
                    #region 计算距离
                    double[] dists = new double[template_histograms.Length];
                    for (int i = 0; i < template_histograms.Length; ++i) {
                        dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.ChiSquareDistance(histogram, template_histograms[i]);
                        //dists[i] = Jim.OCR.ShapeContext2D.ShapeContext.HistCost(histogram.Cast<double>().ToArray(), templatehistograms[i].Cast<double>().ToArray());
                        showProgress((i + 1.0) / template_histograms.Length);
                    }
                    #endregion


                    #region 对结果排序
                    var arr = dists.Select((d, i) => new ValueIndexPair<double> { Value = d, Index = i })
                        .OrderBy(p => p.Value)
                        .Select(p => new { Distance = p.Value, Char = template_chars[p.Index] })
                        //.Where(p => "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789".IndexOf(p.Char) != -1)
                        .ToArray();
                    Dictionary<string, int> matchcount = new Dictionary<string, int>();
                    int knn = 10;
                    foreach (var pair in arr.Take(knn)) {
                        string ch = pair.Char;
                        matchcount[ch] = matchcount.ContainsKey(ch) ? matchcount[ch] + 1 : 1;
                    }
                    var match = matchcount.Select(pair => new { Count = pair.Value, Ch = pair.Key })
                                          .Where(v => v.Count > 0)
                                          .OrderByDescending(v => v.Count).ToArray();
                    string result = "";
                    foreach (var m in match.Take(3)) {
                        result += String.Format("Char:'{0}',Accuracy:{1}/{2}\t", m.Ch, m.Count, knn);
                    }
                    appendLine(result);
                    #endregion
                }
                appendLine("-----------------------------------------------------");
                showStatus("搜索完成，共用时{0}ms。", timer.Stop());
            })).Start();
        }
        #endregion

        #region event handlers
        private void ofd_Image_FileOk(object sender, CancelEventArgs e) {
            imagestack.Clear();
            var image = new Image<Bgr, Byte>(ofd_Image.FileName).Convert<Gray, Byte>();
            if (image.Height < 70) {
                image = image.Resize(70.0 / image.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            }
            imagestack.Push(image);
            showImage();
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e) {
            ofd_Image.ShowDialog();
        }

        private void 打开模板ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fbd_SC.ShowDialog() == DialogResult.OK) {
                openSC(fbd_SC.SelectedPath);
            }
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (imagestack.Count == 1) return;
            var img = imagestack.Pop();
            img.Dispose();
            showImage();
        }

        private void 二值化ToolStripMenuItem_Click(object sender, EventArgs e) {
            threshold();
        }

        private void 掩模平滑慢ToolStripMenuItem_Click(object sender, EventArgs e) {
            smoothmask();
        }

        private void 中值滤波ToolStripMenuItem_Click(object sender, EventArgs e) {
            smoothmedian();
        }

        private void 划分连通区域ToolStripMenuItem_Click(object sender, EventArgs e) {
            connect_split();
        }

        private void 识别ToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void 消除独立连通区域ToolStripMenuItem_Click(object sender, EventArgs e) {
            connect_denoise();
        }

        private void representativeShapeContextToolStripMenuItem_Click(object sender, EventArgs e) {
            recognize_rsc();
        }

        private void shapemeToolStripMenuItem_Click(object sender, EventArgs e) {
            recognize_shapeme();
        }
        #endregion
    }
}
