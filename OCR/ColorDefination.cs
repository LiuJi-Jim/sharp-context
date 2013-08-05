using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;

namespace Jim.OCR {
    public static class ColorDefination {
        public static readonly Gray BackColor = new Gray(0);
        public static readonly Gray ForeColor = new Gray(255);
        public static readonly Gray HighColor = new Gray(127);
    }
}
