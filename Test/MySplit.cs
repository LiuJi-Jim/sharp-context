using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Jim.OCR.Smooth;
using Jim.OCR;

namespace Test {
    class MySplit {
        private static readonly string basedir = "csdn";
        private static readonly List<IImageProcessor> ips = new List<IImageProcessor>();
        private static readonly Random rand = new Random();
        static MySplit() {
            ips.Add(new Mask { Type = Mask.MaskType.Mask9 });
            ips.Add(new Jim.OCR.Binarization.MaxVariance());
            ips.Add(new Jim.OCR.Misc.Pad { Padding = 2 });
            ips.Add(new Jim.OCR.Denoise.AnnealKMeansConnect { });
            ips.Add(new Jim.OCR.Denoise.ThresholdConnect { Threshold = 10 });
        }

        public void Split() {
            string basepath = @"D:\Play Data\" + basedir;
            string outpath = @"D:\Play Data\" + basedir + "_split";

            foreach (var filepath in Directory.GetFiles(basepath)) {
                Image<Gray, Byte> img = new Image<Gray, byte>(filepath);
                foreach (var ip in ips) {
                    img = ip.Process(img);
                }

                var split = (new Jim.OCR.Split.Connect()).Split(img);
                foreach (var sp in split) {
                    string r = rand.Next(10000000).ToString("D8");
                    sp.Save(Path.Combine(outpath, r + ".bmp"));
                }

                Console.WriteLine(Path.GetFileName(filepath));
            }
        }

        public void Rename() {
            var basepath = @"D:\Play Data\" + basedir + "_split";
            var outpath = @"D:\Play Data\" + basedir + "_rename";
            var list = Directory.GetFiles(basepath)
                .Select(filepath => Path.GetFileNameWithoutExtension(filepath).Split('-').Concat(new[] { filepath }).ToArray())
                .OrderBy(p => rand.Next())
                .ToArray();
            for (int i = 0; i < list.Length; ++i) {
                File.Copy(list[i][2], Path.Combine(outpath, String.Format("{0:D5}-{1}.bmp", i, list[i][0])));
            }
        }
    }
}
