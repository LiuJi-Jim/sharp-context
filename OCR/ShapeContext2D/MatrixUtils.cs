using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;
using System.IO;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        private static DenseMatrix Zeros(int rows, int cols) {
            return new DenseMatrix(rows, cols, 0);
        }

        private static DenseMatrix Ones(int rows, int cols) {
            return new DenseMatrix(rows, cols, 1);
        }

        private static DenseMatrix Epsilons(int rows, int cols) {
            return new DenseMatrix(rows, cols, Epsilon);
        }

        private static DenseMatrix NaNs(int rows, int cols) {
            return new DenseMatrix(rows, cols, double.NaN);
        }

        private static DenseMatrix Eye(int rows, int cols) {
            var m = Zeros(rows, cols);
            int n = Math.Min(rows, cols);
            for (int i = 0; i < n; ++i) {
                m[i, i] = 1;
            }
            return m;
        }
    }

    public static class MatrixUtils {
        /// <summary>
        /// 返回所有列的迭代器
        /// </summary>
        public static IEnumerable<Vector> GetColumns(this Matrix m) {
            for (int j = 0; j < m.Columns; ++j) {
                yield return m.GetColumn(j);
            }
        }

        /// <summary>
        /// 返回所有行的迭代器
        /// </summary>
        public static IEnumerable<Vector> GetRows(this Matrix m) {
            for (int i = 0; i < m.Rows; ++i) {
                yield return m.GetRow(i);
            }
        }

        public static void SetRows(this Matrix m, Vector[] rows) {
            for (int i = 0; i < m.Rows; ++i) {
                m.SetRow(i, rows[i]);
            }
        }

        public static void SetColumns(this Matrix m, Vector[] cols) {
            for (int i = 0; i < m.Columns; ++i) {
                m.SetColumn(i, cols[i]);
            }
        }

        /// <summary>
        /// 按列累加成行向量
        /// </summary>
        public static DenseMatrix Sum1(this Matrix m) {
            //var row = m.GetColumns().Select(c => c.Sum()).ToArray();
            //var sum = new DenseMatrix(1, m.Columns);
            //sum.SetRow(0, row);
            var sum = new DenseMatrix(1, m.Columns);
            for (int i = 0; i < m.Rows; ++i) {
                for (int j = 0; j < m.Columns; ++j) {
                    sum[0, j] += m[i, j];
                }
            }

            return sum;
        }

        /// <summary>
        /// 按行累加成列向量
        /// </summary>
        public static DenseMatrix Sum2(this Matrix m) {
            //var col = m.GetRows().Select(c => c.Sum()).ToArray();
            //var sum = new DenseMatrix(m.Rows, 1);
            //sum.SetColumn(0, col);
            var sum = new DenseMatrix(m.Rows, 1, 0);
            for (int i = 0; i < m.Rows; ++i) {
                for (int j = 0; j < m.Columns; ++j) {
                    sum[i, 0] += m[i, j];
                }
            }

            return sum;
        }

        public static double SumAll(this Matrix m) {
            double sum = 0;
            for (int i = 0; i < m.Rows; ++i) {
                for (int j = 0; j < m.Columns; ++j) {
                    sum += m[i, j];
                }
            }
            return sum;
        }

        public static DenseMatrix PointDivide(this Matrix a, Matrix b) {
            var c = new DenseMatrix(a.Rows, a.Columns);
            var rowvec = new double[c.Columns];
            for (int i = 0; i < a.Rows; ++i) {
                var arowi = a.GetRow(i);
                var browi = b.GetRow(i);
                for (int j = 0; j < a.Columns; ++j) {
                    rowvec[j] = arowi[j] / browi[j];
                }
                c.SetRow(i, rowvec);
            }
            return c;
        }

        public static DenseMatrix PointMultiply(this Matrix a, Matrix b) {
            var c = new DenseMatrix(a.Rows, a.Columns);
            var rowvec = new double[c.Columns];
            for (int i = 0; i < a.Rows; ++i) {
                var arowi = a.GetRow(i);
                var browi = b.GetRow(i);
                for (int j = 0; j < a.Columns; ++j) {
                    rowvec[j] = arowi[j] * browi[j];
                }
                c.SetRow(i, rowvec);
            }
            return c;
        }

        /// <summary>
        /// 重复
        /// </summary>
        public static DenseMatrix RepMat(this Matrix m, int rows, int cols) {
            DenseMatrix result = new DenseMatrix(rows * m.Rows, cols * m.Columns);
            for (int i = 0; i < rows; ++i) {
                for (int j = 0; j < cols; ++j) {
                    result.SetSubMatrix(i * m.Rows, m.Rows, j * m.Columns, m.Columns, m);
                }
            }
            return result;
        }

        public static DenseMatrix SortRowsBy(this Matrix m, int[] order) {
            var rows = m.GetRows().ToArray();
            DenseMatrix result = new DenseMatrix(m.Rows, m.Columns);
            for (int i = 0; i < rows.Length; ++i) {
                result.SetRow(i, rows[order[i]]);
            }
            return result;
        }

        public static DenseMatrix SortColumnsBy(this Matrix m, int[] order) {
            var cols = m.GetColumns().ToArray();
            DenseMatrix result = new DenseMatrix(m.Rows, m.Columns);
            for (int j = 0; j < cols.Length; ++j) {
                result.SetColumn(j, cols[order[j]]);
            }
            return result;
        }

        public static DenseMatrix FilterRowsBy(this Matrix m, int[] filter) {
            var result = new DenseMatrix(filter.Length, m.Columns);
            for (int i = 0; i < filter.Length; ++i) {
                result.SetRow(i, m.GetRow(filter[i]));
            }
            return result;
        }

        public static DenseMatrix FilterColumnsBy(this Matrix m, int[] filter) {
            var result = new DenseMatrix(m.Rows, filter.Length);
            for (int j = 0; j < filter.Length; ++j) {
                result.SetColumn(j, m.GetColumn(j));
            }
            return result;
        }

        public static DenseMatrix Each(this Matrix m, Func<double, double> func) {
            var result = new DenseMatrix(m.Rows, m.Columns);
            var rowvec = new double[m.Columns];
            for (int i = 0; i < m.Rows; ++i) {
                var mrowi = m.GetRow(i);
                for (int j = 0; j < m.Columns; ++j) {
                    rowvec[j] = func(mrowi[j]);
                }
                result.SetRow(i, rowvec);
            }
            return result;
        }

        public static void Each(this Matrix m, Action<double, int, int> action) {
            for (int i = 0; i < m.Rows; ++i) {
                for (int j = 0; j < m.Columns; ++j) {
                    action(m[i, j], i, j);
                }
            }
        }

        public static void EachT(this Matrix m, Action<double, int, int> action) {
            for (int j = 0; j < m.Columns; ++j) {
                for (int i = 0; i < m.Rows; ++i) {
                    action(m[i, j], i, j);
                }
            }
        }

        public static void Each(this Vector v, Action<double, int> action) {
            for (int i = 0; i < v.Count; ++i) {
                action(v[i], i);
            }
        }

        public static int[] FindIdxBy(this IEnumerable<double> v, Predicate<double> predicate) {
            return v.Select((d, i) => new { Val = d, Idx = i })
                    .Where(d => predicate(d.Val))
                    .Select(d => d.Idx)
                    .ToArray();
        }

        public static Matrix GradientX(this Matrix m) {
            Matrix result = new DenseMatrix(m.Rows, m.Columns);

            for (int i = 0; i < m.Rows; ++i) {
                result[i, 0] = m[i, 1] - m[i, 0];
                for (int j = 1; j < m.Columns - 1; ++j) {
                    result[i, j] = (m[i, j + 1] - m[i, j - 1]) / 2;
                }
                result[i, m.Columns - 1] = m[i, m.Columns - 1] - m[i, m.Columns - 2];
            }

            return result;
        }

        public static Matrix GradientY(this Matrix m) {
            Matrix result = new DenseMatrix(m.Rows, m.Columns);

            for (int j = 0; j < m.Columns; ++j) {
                result[0, j] = m[1, j] - m[0, j];
                for (int i = 1; i < m.Rows - 1; ++i) {
                    result[i, j] = (m[i + 1, j] - m[i - 1, j]) / 2;
                }
                result[m.Rows - 1, j] = m[m.Rows - 1, j] - m[m.Rows - 2, j];
            }

            return result;
        }

        public static Matrix RankVertical(params Matrix[] ms) {
            Matrix result = new DenseMatrix(ms.Sum(m => m.Rows), ms[0].Columns);
            int row_offset = 0;
            foreach (var m in ms) {
                result.SetSubMatrix(row_offset, m.Rows, 0, result.Columns, m);
                row_offset += m.Rows;
            }
            return result;
        }

        public static Matrix RankHorizon(params Matrix[] ms) {
            Matrix result = new DenseMatrix(ms[0].Rows, ms.Sum(m => m.Columns));
            int col_offset = 0;
            foreach (var m in ms) {
                result.SetSubMatrix(0, result.Rows, col_offset, m.Columns, m);
                col_offset += m.Columns;
            }
            return result;
        }

        public static Matrix GaussianKernal(int N) {
            var m = new DenseMatrix(N, N, 1);
            for (int i = 1; i < N; ++i) {
                for (int j = N - 2; j >= 0; --j) {
                    m[i, j] = m[i, j + 1] + m[i - 1, j];
                }
            }
            var diag = m.GetDiagonal();
            var w = new DenseMatrix(N, 1);
            for (int i = 0; i < N; ++i) {
                w[i, 0] = diag[i] * Math.Pow(2, 1 - N);
            }
            return w * w.Transpose();
        }

        public static Matrix Reshape(this Matrix a, int rows, int cols) {
            Matrix b = new DenseMatrix(rows, cols);
            for (int j = 0; j < a.Columns; ++j) {
                for (int i = 0; i < a.Rows; ++i) {
                    int pos = j * a.Rows + i;
                    int col = pos / b.Rows, row = pos % b.Rows;
                    b[row, col] = a[i, j];
                }
            }
            return b;
        }

        public static void CreateGrid(int N1, int N2, out Matrix x, out Matrix y) {
            int M = N1 * N2;
            x = new DenseMatrix(M, 1);
            y = new DenseMatrix(M, 1);
            for (int i = 0; i < N2; ++i) {
                for (int j = 0; j < N1; ++j) {
                    x[i * N1 + j, 0] = i;
                    y[i * N1 + j, 0] = j;
                }
            }
        }

        public static void WriteOut(this Matrix m, TextWriter writer) {
            for (int i = 0; i < m.Rows; ++i) {
                for (int j = 0; j < m.Columns; ++j) {
                    writer.Write(m[i, j].ToString("F4"));
                    if (j < m.Columns - 1) writer.Write("\t");
                }
                writer.WriteLine();
            }
        }
    }
}
