using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Algorithms {
    public class KMeansCluster {
        public class Cluster : List<Vector2> {
            public Vector2 Centroid { get; set; }
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

    public static class KMeansClustering {
        public static double Epsilon = 1e-8;
        public static int MaxIterate = 4096;
        public static double alpha = 0.99;

        private static readonly Random rand = new Random();

        #region 轮盘随机算法

        public static KMeansCluster Cluster(Vector2[] data, int k) {
            int n = data.Length;
            Vector2[] centroids = initCentroid(data, k);
            int[] clusters = new int[n];
            double RSS = double.MaxValue;
            int it = 0;
            while (it < MaxIterate) {
                bool changed = false;
                string cs = "";
                foreach (var v in centroids) cs += v + ", ";
                //Console.WriteLine("质心：" + cs);
                for (int i = 0; i < n; ++i) {
                    // 对剩余的每个向量测量其到每个质心的距离
                    double[] distance = new double[k];
                    for (int j = 0; j < k; ++j) {
                        distance[j] = Vector2.DistanceSqr(data[i], centroids[j]) + 0.0001;
                    }
                    double sumDis = 0;
                    for (int j = 0; j < k; ++j) {
                        sumDis += distance[j];
                    }
                    for (int j = 0; j < k; ++j) {
                        distance[j] = distance[j] / sumDis;
                    }
                    double[] disSum = new double[k];
                    disSum[0] = distance[0];
                    for (int j = 1; j < k; ++j) {
                        disSum[j] = disSum[j - 1] + distance[j];
                    }
                    double r = rand.NextDouble();
                    int pos = 0;
                    while (pos < k) {
                        if (r < disSum[pos]) {
                            break;
                        }
                        pos++;
                    }
                    clusters[i] = pos;
                }

                // if (!changed) break; // 未发现改变，跳出

                // 重新计算每个类的质心
                calcCentroids(data, centroids, clusters);

                // 重新计算总方差
                double newRSS = calcRSS(data, centroids, clusters);

                double diff = Math.Abs(newRSS - RSS);
                //Console.WriteLine("迭代次数{0}，方差：{1}/{2}/{3}。", it, RSS, newRSS, diff);
                RSS = newRSS;
                ++it;

                if (diff < Epsilon) break; // RSS收敛，跳出
            }

            //List<Vector2>[] result = new List<Vector2>[k];
            KMeansCluster result = new KMeansCluster(n, k);
            Array.Copy(clusters, result.ClusterIndex, n); // 拷贝聚类索引
            for (int i = 0; i < k; ++i) {
                result[i].Centroid = centroids[i]; // 拷贝每类的质心
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
        private static Vector2[] initCentroid(Vector2[] data, int k) {
            Dictionary<int, bool> used = new Dictionary<int, bool>();
            while (used.Count < k) {
                int r = rand.Next(data.Length);
                if (!used.ContainsKey(r)) used[r] = true;
            }

            List<Vector2> centroids = new List<Vector2>(k);
            foreach (int i in used.Keys) {
                centroids.Add(data[i]);
            }
            //return centroids.OrderBy(v => v.ModSqr()).ToArray();
            return centroids.ToArray();

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

        #endregion

        #region 计算类的质心和方差

        private static void calcCentroids(Vector2[] data, Vector2[] centroids, int[] clusters) {
            int k = centroids.Length;
            Vector2[] sum = new Vector2[k];
            int[] counts = new int[k];
            for (int i = 0; i < data.Length; ++i) {
                int c = clusters[i];
                sum[c] += data[i];
                ++counts[c];
            }
            for (int i = 0; i < k; ++i) {
                centroids[i] = sum[i] / counts[i];
            }
        }

        private static double calcRSS(Vector2[] data, Vector2[] centroids, int[] clusters) {
            double RSS = 0;
            for (int i = 0; i < data.Length; ++i) {
                int c = clusters[i];
                RSS += Vector2.DistanceSqr(data[i], centroids[c]);
            }
            return RSS;
        }

        #endregion

        #region 模拟退火算法

        /*  我自己的，先不写
        private static double createRandom(Vector2[] data, Vector2[] centroids, int[] clusters) {
            int n = data.Length, k = centroids.Length;
            Dictionary<int, bool> used = new Dictionary<int, bool>();
            while (used.Count < k) {
                int r = rand.Next(data.Length);
                if (!used.ContainsKey(r)) {
                    centroids[used.Count] = data[r];
                    used[r] = true;
                }
            }

            for (int i = 0; i < n; ++i) {
                // 对每个向量测量其到每个质心的距离
                double minDis = double.MaxValue;
                for (int j = 0; j < k; ++j) {
                    double dis = Vector2.DistanceSqr(data[i], centroids[j]);
                    if (dis < minDis) {
                        minDis = dis;
                        clusters[i] = j;
                    }
                }
            }

            // 计算每个类的质心
            calcCentroids(data, centroids, clusters);

            // 计算总方差
            double RSS = calcRSS(data, centroids, clusters);
            return RSS;
        }

        public static KMeansCluster AnnealCluster(Vector2[] data, int k) {
            throw new Exception();
        }

         * */

        /// <summary>
        /// 基于模拟退火的动态聚类算法，杨忠明，黄道，王行愚。
        /// </summary>
        public static KMeansCluster AnnealCluster(Vector2[] data, int c, out int it) {
            it = 0;
            int m = data.Length;
            double dmax = double.MinValue, dmin = double.MaxValue;
            for (int i = 0; i < m; ++i) {
                for (int j = 0; j < m; ++j) {
                    if (i == j) continue;
                    double dis = (data[i] - data[j]).Mod();
                    if (dis > dmax) dmax = dis;
                    if (dis < dmin) dmin = dis;
                }
            } // 确定最大最小距离（最小不是通常都是0吗……）
            double t = 10 * dmax, tmin = 1e-1 * dmin + 1e-6;

            Vector2[] center = new Vector2[c];
            int[] cls = new int[m];
            for (int i = 0; i < c; ++i) {
                center[i] = data[i];
                cls[i] = i;
            } // 以前c个样本作为初始聚类中心

            for (int i = c; i < m; ++i) {
                double[] ds = new double[c];
                for (int j = 0; j < c; ++j) {
                    ds[j] = (data[i] - center[j]).Mod();
                }
                int pos = getPos(ds, data[i], center, t);
                cls[i] = pos;
            } // 对其他m-c个样本计算类别

            Vector2[] cSum = new Vector2[c];
            int[] cCounts = new int[c];
            for (int i = 0; i < m; ++i) {
                int cs = cls[i];
                cSum[cs] += data[i];
                ++cCounts[cs];
            }
            for (int i = 0; i < c; ++i) {
                center[i] = cSum[i] / cCounts[i];
            }
            double RSS = 0;
            for (int i = 0; i < m; ++i) {
                int cs = cls[i];
                RSS += Vector2.DistanceSqr(data[i], center[cs]);
            } // 计算新的聚类中心和累计类内距离平方和

            while (t > tmin && it < MaxIterate) {
                it++;

                double nr = adjust(data, center, cls, cCounts, t, RSS);
                double diff = Math.Abs(nr - RSS);
                RSS = nr;
                if (diff < Epsilon) break;

                t *= alpha; // 降温
            }

            KMeansCluster result = new KMeansCluster(m, c);
            Array.Copy(cls, result.ClusterIndex, m); // 拷贝聚类索引
            for (int i = 0; i < c; ++i) {
                result[i].Centroid = center[i]; // 拷贝每类的质心
            }

            for (int i = 0; i < m; ++i) {
                int cc = cls[i];
                result[cc].Add(data[i]);
            }

            return result;
        }

        public static KMeansCluster AnnealCluster(Vector2[] data, int c) {
            int it = 0;
            return AnnealCluster(data, c, out it);
        }

        private static int randPos(double[] ds) {
            int c = ds.Length;
            double sum = ds.Sum();
            for (int j = 0; j < c; ++j) {
                ds[j] /= sum;
            }
            double[] disSum = new double[c];
            disSum[0] = ds[0];
            for (int j = 1; j < c; ++j) {
                disSum[j] = disSum[j - 1] + ds[j];
            }
            double r = rand.NextDouble();
            int pos = 0;
            while (pos < c) {
                if (r < disSum[pos]) {
                    break;
                }
                pos++;
            }
            if (pos == c) {
                Console.WriteLine();
            }
            return pos;
        }

        private static int getPos(double[] ds, Vector2 v, Vector2[] center, double t) {
            int c = center.Length;
            int l = ds.Select((d, i) => new { D = d, I = i }).OrderBy(o => o.D).First().I; // l = arg min (ds)

            double[] p = new double[c];
            for (int i = 0; i < c; ++i) {
                double dd = (center[l] - center[i]).Mod() / t;
                double exp = Math.Exp(dd);
                if (double.IsInfinity(exp)) p[i] = 1e-6;
                else p[i] = exp / (1.0 + exp);
            }
            double sum = p.Sum();
            double[] pb = new double[c];
            for (int i = 0; i < c; ++i) {
                pb[i] = p[i] / sum;
            }
            for (int i = 1; i < c; ++i) {
                pb[i] += pb[i - 1];
            }
            double r = rand.NextDouble();
 
            int pos = pb.Select((d, i) => new { P = d, I = i }).Where(v1 => v1.P > r).Min(v2 => v2.I);

            return pos;
        }

        private static double adjust(Vector2[] data, Vector2[] center, int[] cls, int[] counts, double t, double RSS) {
            int m = data.Length, c = center.Length;
            int[] ncounts = new int[c]; // 新一轮各组数量
            Array.Copy(counts, ncounts, c);

            for (int j = 0; j < m; ++j) {
                double[] ds = new double[c];
                int cc = cls[j];
                for (int i = 0; i < c; ++i) {
                    int cn = counts[i];
                    if (cn == 0) {
                        ds[i] = 0;
                    } else {
                        double mm = (i == cc) ? (cn - 1) : (cn + 1);
                        ds[i] = cn / mm * (data[j] - center[i]).ModSqr();
                    }
                }
                int nc = getPos(ds, data[j], center, t);
                double dif = (data[j] - center[cc]).Mod() - (data[j] - center[nc]).Mod();
                if (nc != cc) {
                    //if (counts[cc] != 1) {
                    // 类别改变
                    // 找不到好方法来防止-1变成0的问题，所以只好当前类只有一个元素的时候我就不动它了
                    center[cc] = center[cc] + (center[cc] - data[j]) / (counts[cc] + 1.0);
                    //center[cc] = center[cc] + (center[cc] - data[j]) / (counts[cc] + 1.0 + Epsilon);
                    center[nc] = center[nc] + (data[j] - center[nc]) / (counts[nc] + 1.0); // counts[nc] + 1??
                    //center[nc] = center[nc] + (data[j] - center[nc]) / (counts[nc] - 1.0 + Epsilon); // counts[nc] + 1??
                    counts[cc]--;
                    counts[nc]++; //是否可以在这里直接操作counts？
                    cls[j] = nc;
                    RSS -= dif;
                    //}
                }
            }

            return RSS;
        }

        #endregion
    }
}
