using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Algorithms {
    /// <summary>
    /// 泛型KMeans聚类结果
    /// </summary>
    public class KMeansCluster<T> {
        public class Cluster : List<T> {
            public T Center { get; set; }
        }

        public Cluster[] Clusters { get; set; }
        public int DataCount { get; set; }
        public int[] ClusterIndex { get; set; }
        public KMeansCluster(int n, int k) {
            DataCount = n;
            Clusters = new Cluster[k];
            for (int i = 0; i < k; ++i) Clusters[i] = new Cluster();
            ClusterIndex = new int[n];
        }
        public Cluster this[int i] {
            get { return Clusters[i]; }
        }
    }

    /// <summary>
    /// 泛型KMeans聚类算法
    /// </summary>
    public class KMeans<T> {
        private static readonly Random rand = new Random();
        private readonly Func<T, T, double> distanceOf;
        private readonly Func<List<T>, T> centerOf;
        private readonly int n;
        private readonly T[] data;
        private readonly int k;
        private T[] centers;
        private int[] clusters;

        public int MaxIterate = 4096;
        public double Epsilon = 1e-6;
        public double alpha = 0.9;

        /// <summary>
        /// 泛型KMeans聚类
        /// </summary>
        /// <param name="data">数据集</param>
        /// <param name="k">类数</param>
        /// <param name="computeDistance">距离计算函数</param>
        /// <param name="computeCenter">中心计算函数</param>
        public KMeans(T[] data, int k, Func<T, T, double> computeDistance, Func<List<T>, T> computeCenter) {
            this.n = data.Length;
            this.data = data;
            this.k = k;
            this.distanceOf = computeDistance;
            this.centerOf = computeCenter;

            this.clusters = new int[n];
            this.centers = new T[k];
        }

        public KMeansCluster<T> Cluster() {
            initCenter(); // 随机初始化中心
            double RSS = double.MaxValue;
            int it = 0;
            while (it < MaxIterate) {
                bool changed = false;
                for (int i = 0; i < n; ++i) {
                    // 对剩余的每个向量测量其到每个质心的距离
                    double[] distance = new double[k];
                    for (int j = 0; j < k; ++j) {
                        distance[j] = distanceOf(data[i], centers[j]) + Epsilon;
                    }

                    int minCluster = 0;
                    for (int j = 1; j < k; ++j) {
                        if (distance[j] < distance[minCluster])
                            minCluster = j;
                    }
                    clusters[i] = minCluster;
                    #region 随机化
                    //double sumDis = 0;
                    //for (int j = 0; j < k; ++j) {
                    //    sumDis += distance[j];
                    //}
                    //for (int j = 0; j < k; ++j) {
                    //    distance[j] = distance[j] / sumDis;
                    //}
                    //double[] disSum = new double[k];
                    //disSum[0] = distance[0];
                    //for (int j = 1; j < k; ++j) {
                    //    disSum[j] = disSum[j - 1] + distance[j];
                    //}
                    //double r = rand.NextDouble();
                    //int pos = 0;
                    //while (pos < k) {
                    //    if (r < disSum[pos]) {
                    //        break;
                    //    }
                    //    pos++;
                    //}
                    //clusters[i] = pos;
                    #endregion
                }

                // if (!changed) break; // 未发现改变，跳出

                // 重新计算每个类的质心
                computeCenters();

                // 重新计算总方差
                double newRSS = computeRSS();

                double diff = Math.Abs(newRSS - RSS);
                //Console.WriteLine("迭代次数{0}，方差：{1}/{2}/{3}。", it, RSS, newRSS, diff);
                RSS = newRSS;
                Console.WriteLine("第{0}次迭代,方差{1:F4}.", it, RSS);
                ++it;

                if (diff < Epsilon) break; // RSS收敛，跳出
            }

            KMeansCluster<T> result = new KMeansCluster<T>(n, k);
            Array.Copy(clusters, result.ClusterIndex, n); // 拷贝聚类索引
            for (int i = 0; i < k; ++i) {
                result[i].Center = centers[i]; // 拷贝每类的中心
            }
            for (int i = 0; i < n; ++i) {
                int c = clusters[i];
                result[c].Add(data[i]);
            }

            return result;
        }

        /// <summary>
        /// 随机选其中的K个向量作为质心
        /// </summary>
        private void initCenter() {
            Dictionary<int, bool> used = new Dictionary<int, bool>();
            while (used.Count < k) {
                int r = rand.Next(data.Length);
                if (!used.ContainsKey(r)) used[r] = true;
            }

            int pos = 0;
            foreach (int i in used.Keys) {
                centers[pos++] = data[i];
            }
            //return centroids.OrderBy(v => v.ModSqr()).ToArray();

            // 平均分割
            //int n = data.Length;
            //Vector2[] tmp = new Vector2[n];
            //Array.Copy(data, tmp, n);
            //Array.Sort(tmp, (a, b) => a.ModSqr().CompareTo(b.ModSqr()));
            //int step = n / k;
            //Vector2[] result = new Vector2[k];
            //for (int i = 0; i < k; ++i) {
            //    int pos = i * step + step / 2;
            //    if (pos >= n) pos = n;
            //    result[i] = tmp[pos];
            //}
            //return result;

            //int n = data.Length;
            //Vector2 min = new Vector2(20000, 20000), max = Vector2.Zero;
            //foreach (var v in data) {
            //    if (v.ModSqr() < min.ModSqr()) min = v;
            //    if (v.ModSqr() > max.ModSqr()) max = v;
            //}
            //return new Vector2[] { min, max };
        }

        private void computeCenters() {
            List<T>[] dataC = new List<T>[k];
            for (int i = 0; i < dataC.Length; ++i) dataC[i] = new List<T>();
            for (int i = 0; i < data.Length; ++i) {
                int c = clusters[i];
                dataC[c].Add(data[i]);
            }
            for (int i = 0; i < k; ++i) {
                centers[i] = centerOf(dataC[i]);
            }
        }

        private double computeRSS() {
            double RSS = 0;
            for (int i = 0; i < data.Length; ++i) {
                int c = clusters[i];
                RSS += distanceOf(data[i], centers[c]);
            }
            return RSS;
        }

        #region 模拟退火改进

        private Func<T, T, T> add;
        private Func<T, T, T> sub;
        private Func<T, double, T> div;

        public KMeansCluster<T> AnnealCluster(Func<T, T, T> add, Func<T, T, T> sub, Func<T, double, T> div, out int it) {
            this.add = add;
            this.sub = sub;
            this.div = div;
            it = 0;
            double dmax = double.MinValue, dmin = double.MaxValue;
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n; ++j) {
                    if (i == j) continue;
                    double dis = distanceOf(data[i], data[j]);
                    if (dis > dmax) dmax = dis;
                    if (dis < dmin) dmin = dis;
                }
            } // 确定最大最小距离（最小不是通常都是0吗……）
            double t = 10 * dmax, tmin = 1e-1 * dmin + Epsilon;
            for (int i = 0; i < k; ++i) {
                centers[i] = data[i];
                clusters[i] = i;
            } // 以前c个样本作为初始聚类中心

            for (int i = k; i < n; ++i) {
                double[] ds = new double[k];
                for (int j = 0; j < k; ++j) {
                    ds[j] = distanceOf(data[i], centers[j]);
                }
                int pos = getPos(ds, t);
                clusters[i] = pos;
            } // 对其他m-c个样本计算类别

            computeCenters(); // 重新计算中心

            double RSS = computeRSS(); // 计算方差

            while (t > tmin && it < MaxIterate) {
                it++;

                double nr = adjust(t, RSS);
                double diff = Math.Abs(nr - RSS);
                RSS = nr;
                if (diff < Epsilon) break;

                t *= alpha; // 降温
            }

            KMeansCluster<T> result = new KMeansCluster<T>(n, k);
            Array.Copy(clusters, result.ClusterIndex, n); // 拷贝聚类索引
            for (int i = 0; i < k; ++i) {
                result[i].Center = centers[i]; // 拷贝每类的中心
            }
            for (int i = 0; i < n; ++i) {
                int c = clusters[i];
                result[c].Add(data[i]);
            }

            return result;
        }

        public KMeansCluster<T> AnnealCluster(Func<T, T, T> add, Func<T, T, T> sub, Func<T, double, T> div) {
            int it = 0;
            return AnnealCluster(add, sub, div, out it);
        }

        private int getPos(double[] ds, double t) {
            int l = ds.Select((d, i) => new { D = d, I = i }).OrderBy(o => o.D).First().I; // l = arg min (ds)

            double[] p = new double[k];
            for (int i = 0; i < k; ++i) {
                double dd = distanceOf(centers[l], centers[i]) / t;
                double exp = Math.Exp(dd);
                if (double.IsInfinity(exp)) p[i] = 1e-6;
                else p[i] = exp / (1.0 + exp);
            }
            double sum = p.Sum();
            double[] pb = new double[k];
            for (int i = 0; i < k; ++i) {
                pb[i] = p[i] / sum;
            }
            for (int i = 1; i < k; ++i) {
                pb[i] += pb[i - 1];
            }
            double r = rand.NextDouble();

            int pos = pb.Select((d, i) => new { P = d, I = i }).Where(v1 => v1.P > r).Min(v2 => v2.I);

            return pos;
        }

        private double adjust(double t, double RSS) {
            int[] counts = new int[k]; // 各类数量
            for (int i = 0; i < n; ++i) {
                counts[clusters[i]]++;
            }

            for (int j = 0; j < n; ++j) {
                double[] ds = new double[k];
                int cc = clusters[j];
                for (int i = 0; i < k; ++i) {
                    int cn = counts[i];
                    if (cn == 0) {
                        ds[i] = 0;
                    } else {
                        double mm = (i == cc) ? (cn - 1) : (cn + 1);
                        ds[i] = cn / mm * distanceOf(data[j], centers[i]);
                    }
                }
                int nc = getPos(ds, t);
                double dif = distanceOf(data[j], centers[cc]) - distanceOf(data[j], centers[nc]);
                if (nc != cc) {
                    // 类别改变
                    //centers[cc] = centers[cc] + (centers[cc] - data[j]) / (counts[cc] + 1.0);
                    centers[cc] = add(centers[cc], div(sub(centers[cc], data[j]), counts[cc] + 1 + Epsilon));
                    //centers[nc] = centers[nc] + (data[j] - centers[nc]) / (counts[nc] + 1.0);
                    centers[nc] = add(centers[nc], div(sub(data[j], centers[nc]), counts[nc] - 1 + Epsilon));
                    counts[cc]--;
                    counts[nc]++;
                    clusters[j] = nc;
                    RSS -= dif;
                }
            }

            return RSS;
        }

        #endregion
    }
}
