using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Jim.OCR.Split {
    /// <summary>
    /// 连通区域划分，不能有噪声，不能粘连，不能断裂（苛刻啊……）
    /// </summary>
    [ProcessorName("连通区域划分")]
    [ProcessorDescription("不能有噪声，不能粘连，不能断裂（苛刻啊……）")]
    public class Connect : ISplit {
        public Image<Gray, byte>[] Split(Image<Gray, byte> src) {
            var result = new List<Image<Gray, Byte>>();
            var cl = src.connectLevel(1, 8, 0);
            var position_image = new List<Tuple<int, Image<Gray, Byte>>>();
            foreach (var domain in cl.Domains) {
                int most_left = int.MaxValue, most_right = 0;
                foreach (var point in domain.Points) {
                    if (point.X < most_left) most_left = point.X;
                    if (point.X > most_right) most_right = point.X;
                }
                int padding = 5;
                var split = new Image<Gray, Byte>(padding * 2 + (most_right - most_left), src.Height);
                foreach (var point in domain.Points) {
                    int x = point.X, y = point.Y;
                    split[y, padding + x - most_left] = src[point];
                }
                position_image.Add(new Tuple<int, Image<Gray, byte>> {
                    First = most_left,
                    Second = split
                });
            }
            foreach (var tp in position_image.OrderBy(t => t.First)) {
                result.Add(tp.Second);
            }

            return result.ToArray();
        }
    }
}
