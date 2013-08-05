using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jim.OCR;
using System.Reflection;

namespace App.Controls {
    public partial class SplitThenRecognize : UserControl {
        private class BindableRadioButton<T> : RadioButton {
            public T Data { get; set; }
        }

        public SplitThenRecognize() {
            InitializeComponent();
        }

        public ISplit SelectedSplit {
            get { return propertyGrid1.SelectedObject as ISplit; }
            set { propertyGrid1.SelectedObject = value; }
        }

        public IRecognizeSingle SelectedRecognizeSingle {
            get { return propertyGrid2.SelectedObject as IRecognizeSingle; }
            set { propertyGrid2.SelectedObject = value; }
        }

        private void SplitThenRecognize_Load(object sender, EventArgs e) {
            initSplits();
            initRecos();
        }

        private void initSplits() {
            Assembly asm = Assembly.GetAssembly(typeof(ISplit));
            var types = asm.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ISplit))).ToList();
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

            int offset_y = 0;
            bool first = true;
            foreach (var split in list) {
                var radio = new BindableRadioButton<Tuple<Type, string, string>>();
                radio.Data = split;

                radio.Text = split.Second;
                radio.Left = 5;
                radio.Top = offset_y;
                offset_y += radio.Height;
                radio.Click += new EventHandler(Radio_Split_Click);

                splitContainer1.Panel1.Controls.Add(radio);
                if (first) {
                    radio.Checked = true;
                    this.SelectedSplit = Activator.CreateInstance(split.First) as ISplit;
                    first = false;
                }
            }
        }

        private void initRecos() {
            Assembly asm = Assembly.GetAssembly(typeof(IRecognizeSingle));
            var types = asm.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRecognizeSingle))).ToList();
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

            int offset_y = 0;
            bool first = true;
            foreach (var reco in list) {
                var radio = new BindableRadioButton<Tuple<Type, string, string>>();
                radio.Data = reco;

                radio.Text = reco.Second;
                radio.Left = 5;
                radio.Top = offset_y;
                offset_y += radio.Height;
                radio.Click += new EventHandler(Radio_Reco_Click);

                splitContainer2.Panel1.Controls.Add(radio);
                if (first) {
                    radio.Checked = true;
                    this.SelectedRecognizeSingle = Activator.CreateInstance(reco.First) as IRecognizeSingle;
                    first = false;
                }
            }
        }

        private void Radio_Reco_Click(object sender, EventArgs e) {
            var radio = sender as BindableRadioButton<Tuple<Type, string, string>>;
            if (radio == null) return;
            else {
                label3.Text = radio.Data.Second;
                label4.Text = radio.Data.Third;

                SelectedRecognizeSingle = Activator.CreateInstance(radio.Data.First) as IRecognizeSingle;
            }
        }

        private void Radio_Split_Click(object sender, EventArgs e) {
            var radio = sender as BindableRadioButton<Tuple<Type, string, string>>;
            if (radio == null) return;
            else {
                label3.Text = radio.Data.Second;
                label4.Text = radio.Data.Third;

                SelectedSplit = Activator.CreateInstance(radio.Data.First) as ISplit;
            }
        }
    }
}
