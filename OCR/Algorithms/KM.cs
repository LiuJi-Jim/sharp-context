using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Algorithms {
    /// <summary>
    /// KM算法，带权二分图匹配
    /// <see cref="http://zjc5377.blog.hexun.com/28938013_d.html"/>
    /// </summary>
    public class KM {
        int n;                 // X 的大小
        int[,] weight;         // X 到 Y 的映射（权重）
        int[] lx, ly;         // 标号
        bool[] sx, sy;     // 是否被搜索过

        /// <summary>
        /// Y(i)与X(MatchPair[i])匹配
        /// </summary>
        public int[] MatchPair { get; private set; }

        public int MatchResult { get; private set; }

        /// <summary>
        /// 通过图的大小和邻接矩阵初始化
        /// </summary>
        /// <param name="n">节点个数</param>
        /// <param name="g">邻接矩阵</param>
        public KM(int n, int[,] g) {
            this.n = n;
            this.weight = g;

            this.lx = new int[n];
            this.ly = new int[n];
            this.sx = new bool[n];
            this.sy = new bool[n];
            this.MatchPair = new int[n];
        }

        /// <summary>
        /// 对有完美匹配的二分图进行带权匹配，返回匹配总权值，匹配结果保存在MatchPair中。
        /// </summary>
        /// <param name="maxsum">为true为最大权匹配，false为最小权匹配</param>
        public int Match(bool maxsum) {
            if (!maxsum) {
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        weight[i, j] = -weight[i, j];
                    }
                }
            }
            // 初始化标号

            for (int i = 0; i < n; i++) {
                lx[i] = -0x1FFFFFFF;
                ly[i] = 0;
                for (int j = 0; j < n; j++)

                    if (lx[i] < weight[i, j])

                        lx[i] = weight[i, j];

            }

            fillArray<int>(MatchPair, -1);

            for (int u = 0; u < n; u++) {
                while (true) {
                    fillArray<bool>(sx, false);
                    fillArray<bool>(sy, false);
                    if (dfsPath(u)) break;

                    // 修改标号
                    int dx = 0x7FFFFFFF;
                    for (int i = 0; i < n; i++) {
                        if (sx[i]) {
                            for (int j = 0; j < n; j++) {
                                if (!sy[j]) {
                                    dx = Math.Min(lx[i] + ly[j] - weight[i, j], dx);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < n; i++) {
                        if (sx[i]) lx[i] -= dx;
                        if (sy[i]) ly[i] += dx;
                    }

                }
            }
            int sum = 0;
            for (int i = 0; i < n; i++) {
                sum += weight[MatchPair[i], i];
            }
            if (!maxsum) {
                sum = -sum;
                for (int i = 0; i < n; i++) {
                    for (int j = 0; j < n; j++) {
                        weight[i, j] = -weight[i, j]; // 如果需要保持 weight [ ] [ ] 原来的值，这里需要将其还原}
                    }
                }

            }


            MatchResult = sum;
            return sum;
        }

        private bool dfsPath(int u) {
            sx[u] = true;
            for (int v = 0; v < n; v++) {
                if (!sy[v] && lx[u] + ly[v] == weight[u, v]) {
                    sy[v] = true;
                    if (MatchPair[v] == -1 || dfsPath(MatchPair[v])) {
                        MatchPair[v] = u;
                        return true;
                    }
                }
            }
            return false;
        }

        private static void fillArray<T>(T[] arr, T val) {
            for (int i = 0; i < arr.Length; ++i) {
                arr[i] = val;
            }
        }
    }
}
