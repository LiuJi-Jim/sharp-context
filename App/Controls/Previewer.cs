using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV;
using Jim.OCR;

namespace App.Controls {
    public partial class Previewer : UserControl {
        public Previewer() {
            InitializeComponent();
        }

        public IImage Source {
            get { return imageBox_Source.Image; }
            set { imageBox_Source.Image = value; }
        }

        public IImage Preview {
            get { return imageBox_Preview.Image; }
            set { imageBox_Preview.Image = value; }
        }

        private Jim.OCR.Tuple<IImage, string, int>[] results;
        public Jim.OCR.Tuple<IImage, string, int>[] Results {
            get { return results; }
            set {
                listView1.Items.Clear();
                results = value;
                imageList1.Images.Clear();
                if (results == null) return;
                for (int i = 0; i < results.Length; ++i) {
                    var tp = results[i];
                    imageList1.Images.Add(tp.First.Bitmap);
                    var it = new ListViewItem {
                        Text = String.Format("{0}/{1}", tp.Second, tp.Third),
                        ImageIndex = i
                    };
                    listView1.Items.Add(it);
                }
            }
        }
    }
}
