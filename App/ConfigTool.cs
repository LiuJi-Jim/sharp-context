using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Jim.OCR;
using Emgu.CV.Structure;
using Emgu.CV;
using App.Controls;

namespace App {
    public partial class ConfigTool : Form {
        private Image<Bgr, Byte> testImage;

        public ConfigTool() {
            InitializeComponent();
        }

        private bool split = true;

        private void ConfigTool_Load(object sender, EventArgs e) {
            Split = new SplitThenRecognize();
            Split.Dock = DockStyle.Fill;
            this.splitContainer1.Panel2.Controls.Add(Split);
        }

        public SplitThenRecognize Split { get; private set; }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl1.SelectedTab == tab_Preview && testImage != null) {
                previewer1.Source = testImage;
                var img = testImage.Convert<Gray, Byte>();
                //var img = new Image<Gray, Byte>(testImage.Width, testImage.Height);
                //for (int i = 0; i < testImage.Height; ++i) {
                //    for (int j = 0; j < testImage.Width; ++j) {
                //        var bgr = testImage[i, j];
                //        img[i, j] = new Gray(bgr.Red * 0.299 + bgr.Green * 0.587 + bgr.Blue * 0.114);
                //    }
                //}
                var ips = imageProcessorConfig1.IPs;
                foreach (var ip in ips) {
                    img = ip.Process(img);
                }
                previewer1.Preview = img;

                if (split) {
                    var spl = Split.SelectedSplit.Split(img);
                    var results = spl.Select(i => Split.SelectedRecognizeSingle.Recognize(i)).ToArray();
                    var tpls = new Tuple<IImage, string, int>[spl.Length];
                    for (int i = 0; i < tpls.Length; ++i) {
                        tpls[i] = new Tuple<IImage, string, int> {
                            First = spl[i],
                            Second = results[i].First,
                            Third = results[i].Second
                        };
                    }
                    previewer1.Results = tpls;
                } else {

                }
            }
        }

        private void button_OpenTestImage_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK) {
                textBox_TestImage.Text = ofd.FileName;
                testImage = new Image<Bgr, Byte>(textBox_TestImage.Text);
                //if (testImage.Height < 70) {
                //    testImage = testImage.Resize(70.0 / testImage.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                //}
            }
        }

        private void SplitTypeChanged(object sender, EventArgs e) {
            if (Object.ReferenceEquals(sender, radioButton_Split)) {
                // 分割后识别
                this.splitContainer1.Panel2.Controls.Add(this.Split);
                split = true;
            } else {
                // 不分割就识别
                this.splitContainer1.Panel2.Controls.Remove(this.Split);
                split = false;
            }
        }
    }
}
