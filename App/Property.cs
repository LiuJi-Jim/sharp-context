using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace App {
    public partial class Property : Form {
        public Property() {
            InitializeComponent();

            propertyGrid1.SelectedObject = new TestPropertyClass();
        }
    }

    public class TestPropertyClass {
        [Category("Int型属性")]
        [DefaultValue(1)]
        [Description("默认值为1")]
        public int IntProp1 { get; set; }

        [Category("Int型属性")]
        [DefaultValue(2)]
        [Description("默认值为2")]
        public int IntProp2 { get; set; }

        [Category("Double型属性")]
        [DefaultValue(3.14)]
        [Description("默认值为3.14")]
        public double DoubleProp { get; set; }

        [Category("String型属性")]
        [DefaultValue("Hello")]
        [Description("默认值为\"Hello\"")]
        public string StringProp { get; set; }

        public TestPropertyClass() {
            IntProp1 = 1;
            IntProp2 = 2;
            DoubleProp = 3.14;
            StringProp = "Hello";
        }
    }
}
