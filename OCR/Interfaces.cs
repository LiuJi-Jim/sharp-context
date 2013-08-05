using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Jim.OCR {
    [AttributeUsage(AttributeTargets.Class)]
    public class ProcessorNameAttribute : Attribute {
        public string Name { get; set; }
        public ProcessorNameAttribute(string name) {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ProcessorDescriptionAttribute : Attribute {
        public string Description { get; set; }
        public ProcessorDescriptionAttribute(string description) {
            this.Description = description;
        }
    }

    public interface IImageProcessor {
        Image<Gray, Byte> Process(Image<Gray, Byte> src);
    }

    public interface ISplit {
        Image<Gray, Byte>[] Split(Image<Gray, Byte> src);
    }

    public interface IRecognizeSingle {
        Tuple<string, int> Recognize(Image<Gray, Byte> src);
    }

    public interface IRecognizeAll {
        Tuple<string, int>[] Recognize(Image<Gray, Byte> src);
    }
}
