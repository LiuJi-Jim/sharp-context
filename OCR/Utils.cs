using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;

namespace Jim.OCR {
    public static class Utils {
        public static byte[,] ByteArr1DTo2D(byte[] data, int width, int height) {
            byte[,] arr = (byte[,])Array.CreateInstance(typeof(byte), height, width);
            IntPtr arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
            Marshal.Copy(data, 0, arrPtr, data.Length);

            return arr;
        }

        public static byte[] ByteArr2DTo1D(byte[,] arr) {
            IntPtr arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0);
            byte[] result = new byte[arr.Length];
            Marshal.Copy(arrPtr, result, 0, result.Length);

            return result;
        }

        public static T[] InitArray<T>(int len, T val) {
            T[] arr = new T[len];
            FillArray<T>(arr, val);
            return arr;
        }

        public static T[,] InitArray2D<T>(int rows, int cols, T val) {
            T[,] arr = new T[rows, cols];
            FillArray2D<T>(arr, val);
            return arr;
        }

        public static void FillArray<T>(this T[] arr, T val) {
            for (int i = 0; i < arr.Length; ++i) arr[i] = val;
        }

        public static void FillArray2D<T>(this T[,] arr, T val) {
            int rows = arr.GetLength(0), cols = arr.GetLength(1);
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    arr[i, j] = val;
        }

        public static double[] LogSpace(double lower, double upper, int n) {
            double[] val = new double[n];
            double step = (upper - lower) / (n - 1);
            for (int i = 0; i < n; ++i) {
                val[i] = Math.Pow(10, lower + step * i);
            }
            return val;
        }

        public static int[] FindLocalMins(int[] arr) {
            List<int> ret = new List<int>();
            int last = int.MaxValue; //最左边加个最大值，保证一开始递减
            int index = -1; //上一个可行的下标
            for (int i = 0; i < arr.Length; i++) {
                if (arr[i] < last) {
                    index = i;
                } else if (arr[i] > last) {
                    if (index != -1) {
                        //index ~ i-1 这些都行
                        for (int j = index; j < i; j++)
                            ret.Add(j);
                        index = -1;
                    }
                }
                last = arr[i];
            }
            if (index != -1)
                for (int j = index; j < arr.Length; j++)
                    ret.Add(j);
            return ret.ToArray();
        }
    }

    public static class RandomEdgeSampler {
        private static readonly Random rand = new Random();

        public static List<Point> getEdge(this Image<Gray, Byte> img) {
            var canny = img.Canny(new Gray(50), new Gray(50));
            List<Point> edge = new List<Point>();
            for (int y = 0; y < canny.Height; ++y) {
                for (int x = 0; x < canny.Width; ++x) {
                    if (canny[y, x].Intensity > 0) {
                        edge.Add(new Point(x, y));
                    }
                }
            }
            return edge;
        }

        /// <summary>
        /// 对一组边缘点集进行随机采样，尽量保证均匀
        /// </summary>
        public static List<Point> Sample(this List<Point> edge, int count) {
            int randcount = count * 4;
            List<Point> randList = new List<Point>();
            if (randcount >= edge.Count) {
                randList.AddRange(edge);
            } else {
                bool[] visit = new bool[edge.Count];
                while (randList.Count < randcount) {
                    int p = rand.Next(edge.Count);
                    if (visit[p]) continue;
                    visit[p] = true;
                    randList.Add(edge[p]);
                }
            }
            bool[] del = new bool[randList.Count]; // 一个点是否被删除
            List<Distance> distance = new List<Distance>(); // 距离
            for (int i = 0; i < randList.Count; ++i) {
                for (int j = i + 1; j < randList.Count; ++j) {
                    Point a = randList[i], b = randList[j];
                    double disSqr = (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
                    distance.Add(new Distance { A = i, B = j, DisSqr = disSqr });
                }
            }
            distance.Sort(new Comparison<Distance>((a, b) => Math.Sign(a.DisSqr - b.DisSqr)));

            int delCount = randList.Count - count;
            for (int i = 0; i < distance.Count && delCount > 0; ++i) {
                var dis = distance[i];
                if (del[dis.A] || del[dis.B]) continue;
                if (rand.Next(2) == 0) del[dis.A] = true;
                else del[dis.B] = true;
                --delCount;
            }

            var result = new List<Point>();
            for (int i = 0; i < randList.Count; ++i) {
                if (!del[i]) result.Add(randList[i]);
            }
            return result;
        }

        struct Distance {
            public int A { get; set; }
            public int B { get; set; }
            public double DisSqr { get; set; }
        }
    }

    public class Tuple<T1, T2> {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public override string ToString() {
            return String.Format("<{0}, {1}>", First, Second);
        }
    }
    public class Tuple<T1, T2, T3> {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public T3 Third { get; set; }
        public override string ToString() {
            return String.Format("<{0}, {1}, {2}>", First, Second, Third);
        }
    }
    public class Tuple<T1, T2, T3, T4> {
        public T1 First { get; set; }
        public T2 Second { get; set; }
        public T3 Third { get; set; }
        public T4 Forth { get; set; }
        public override string ToString() {
            return String.Format("<{0}, {1}, {2}, {3}>", First, Second, Third, Forth);
        }
    }
    public class ValueIndexPair<T> {
        public T Value { get; set; }
        public int Index { get; set; }
        public override string ToString() {
            return String.Format("<{0} @ {1}>", Value, Index);
        }
    }

    public class MyTimer {
        private class TimeUseage {
            /// <summary>用时</summary>
            public double Time { get; set; }
            /// <summary>次数</summary>
            public int Times { get; set; }
        }

        private bool enabled = true;
        public bool Enabled {
            get { return enabled; }
            set { enabled = value; }
        }

        private readonly Stopwatch timer = new Stopwatch();

        private readonly Dictionary<string, TimeUseage> timeused = new Dictionary<string, TimeUseage>();

        public void Restart() {
            if (!enabled) return;
            timer.Reset();
            timer.Start();
        }

        public double Stop() {
            if (!enabled) return -1;
            timer.Stop();
            return timer.ElapsedMilliseconds;
        }

        public double StopAndSay(string str, params object[] objs) {
            if (!enabled) return -1;
            double time = Stop();

            Console.WriteLine(String.Format("{0}ms\t", time) + String.Format(str, objs));
            if (timeused.ContainsKey(str)) {
                timeused[str].Time += time;
                timeused[str].Times++;
            } else {
                timeused[str] = new TimeUseage { Time = time, Times = 1 };
            }

            return time;
        }

        public double StopButDontSay(string str) {
            if (!enabled) return -1;
            double time = Stop();

            if (timeused.ContainsKey(str)) {
                timeused[str].Time += time;
                timeused[str].Times++;
            } else {
                timeused[str] = new TimeUseage { Time = time, Times = 1 };
            }

            return time;
        }

        public void Stats() {
            if (!enabled) return;
            Console.WriteLine("---------------------用时情况统计--------------------");
            foreach (var i in timeused.OrderByDescending(p => p.Value.Time)) {
                Console.WriteLine(String.Format("{0}ms\t{1}次\t{2}", i.Value.Time, i.Value.Times, i.Key));
            }
            Console.WriteLine("总用时：{0}ms", timeused.Sum(p => p.Value.Time));
            Console.WriteLine("-------------------------------------------------------");
        }

        public void Clear() {
            if (!enabled) return;
            timeused.Clear();
        }

        private static readonly MyTimer _globalTimer = new MyTimer();
        public static MyTimer GlobalTimer {
            get { return _globalTimer; }
        }
    }
}
