using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Jim.OCR;
using System.Web.Script.Serialization;

namespace App.Controls {
    public partial class ImageProcessorConfig : UserControl {
        public ImageProcessorConfig() {
            InitializeComponent();
        }

        public List<IImageProcessor> IPs {
            get {
                return listBox2.Items.Cast<Jim.OCR.Tuple<IImageProcessor, string>>().Select(t => t.First).ToList();
            }
        }

        private void ImageProcessorConfig_Load(object sender, EventArgs e) {
            Assembly asm = Assembly.GetAssembly(typeof(IImageProcessor));
            var types = asm.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IImageProcessor))).ToList();
            var list = types.Select(t => {
                var name = t.GetCustomAttributes(typeof(ProcessorNameAttribute), false)
                             .Cast<ProcessorNameAttribute>().First();
                var description = t.GetCustomAttributes(typeof(ProcessorDescriptionAttribute), false)
                                   .Cast<ProcessorDescriptionAttribute>().First();
                return new Tuple<Type, string, string> {
                    First = t,
                    Second = name.Name,
                    Third = description.Description
                };
            }).ToList();
            listBox1.DataSource = list;
            listBox1.DisplayMember = "Second";

            listBox2.DisplayMember = "Second";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var type = listBox1.SelectedItem as Tuple<Type, string, string>;
            label1.Text = type.Second;
            label2.Text = type.Third;
        }

        private void button_Add_Click(object sender, EventArgs e) {
            var type = listBox1.SelectedItem as Tuple<Type, string, string>;
            if (type == null) return;

            var ip = Activator.CreateInstance(type.First) as IImageProcessor;
            var seltype = new Tuple<IImageProcessor, string> {
                First = ip,
                Second = type.Second
            };
            listBox2.Items.Add(seltype);
            listBox2.SelectedItem = seltype;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e) {
            var type = listBox2.SelectedItem as Tuple<IImageProcessor, string>;
            if (type == null) return;
            propertyGrid1.SelectedObject = type.First;
        }

        private void button_Remove_Click(object sender, EventArgs e) {
            var type = listBox2.SelectedItem as Tuple<IImageProcessor, string>;
            if (type == null) return;

            if (type == listBox2.SelectedItem) propertyGrid1.SelectedObject = null;
            listBox2.Items.Remove(type);
        }

        private void button_Up_Click(object sender, EventArgs e) {
            int cur = listBox2.SelectedIndex;
            if (cur > 0 && cur < listBox2.Items.Count) {
                var tmp = listBox2.Items[cur - 1];
                listBox2.Items[cur - 1] = listBox2.Items[cur];
                listBox2.Items[cur] = tmp;
                listBox2.SelectedIndex = cur - 1;
            }
        }

        private void button_Down_Click(object sender, EventArgs e) {
            int cur = listBox2.SelectedIndex;
            if (cur >= 0 && cur < listBox2.Items.Count - 1) {
                var tmp = listBox2.Items[cur + 1];
                listBox2.Items[cur + 1] = listBox2.Items[cur];
                listBox2.Items[cur] = tmp;
                listBox2.SelectedIndex = cur + 1;
            }
        }
    }
}
