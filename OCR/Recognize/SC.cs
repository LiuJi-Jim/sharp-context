using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Jim.OCR.Recognize {
    internal static class SC {
        public static double[][,] template_sc;
        public static int[][] template_histograms;
        public static string[] template_chars;
        public static double[][] shapemes;

        public static string dir = @"D:\Play Data\毕业设计\train";

        static SC() {
            const int shapeme_count = 100;
            shapemes = new double[shapeme_count][];
            #region Shapeme
            using (var fs = new FileStream(Path.Combine(dir, "data.sm"), FileMode.Open)) {
                using (var br = new BinaryReader(fs)) {
                    for (int i = 0; i < shapeme_count; ++i) {
                        shapemes[i] = new double[60];
                        for (int k = 0; k < 60; ++k) {
                            shapemes[i][k] = br.ReadDouble();
                        }
                    }
                }
            }
            #endregion

            #region SCQ
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
            }
            #endregion

            #region SC
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
            }
            #endregion
        }
    }
}
