using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        //function HC=hist_cost_2(BH1,BH2);
        //% HC=hist_cost_2(BH1,BH2);
        //%
        //% same as hist_cost.m but BH1 and BH2 can be of different lengths

        public Matrix HistCost(Matrix BH1, Matrix BH2) {
            if (BH1.Columns != BH2.Columns)
                throw new Exception("槽数不等没法比！");

            //[nsamp1,nbins]=size(BH1);
            //[nsamp2,nbins]=size(BH2);
            int nbins = BH1.Columns;


            //BH1n=BH1./repmat(sum(BH1,2)+eps,[1 nbins]);
            //%应该是repmat的结果 [sum(BH1,2),sum(BH1,2),sum(BH1,2),sum(BH1,2),sum(BH1,2).....]
            var BH1n = BH1.PointDivide((BH1.Sum2() + Epsilons(nsamp1, 1)).RepMat(1, nbins));

            //BH2n=BH2./repmat(sum(BH2,2)+eps,[1 nbins]);
            var BH2n = BH2.PointDivide((BH2.Sum2() + Epsilons(nsamp2, 1)).RepMat(1, nbins));


            //tmp1=repmat(permute(BH1n,[1 3 2]),[1 nsamp2 1]);		% permute似乎用来改变维的顺序
            //tmp2=repmat(permute(BH2n',[3 2 1]),[nsamp1 1 1]);
            //HC=0.5*sum(((tmp1-tmp2).^2)./(tmp1+tmp2+eps),3);

            // 不知道怎么弄，猜吧
            DenseMatrix cost = Zeros(BH1.Rows, BH2.Rows);
            for (int i = 0; i < BH1.Rows; ++i) { // BH1的每一行
                var BH1nrow = BH1n.GetRow(i);
                for (int j = 0; j < BH2.Rows; ++j) { // BH2的每一行
                    var BH2nrow = BH2n.GetRow(j);
                    double costij = 0;
                    for (int k = 0; k < nbins; ++k) {
                        //double tmp1 = BH1n[i, k], tmp2 = BH2n[j, k];
                        double tmp1 = BH1nrow[k], tmp2 = BH2nrow[k];
                        costij += ((tmp1 - tmp2) * (tmp1 - tmp2)) / (tmp1 + tmp2 + Epsilon);
                    }
                    cost[i, j] = costij * 0.5;
                }
            }

            return cost;
        }

        public static double[,] HistCost(double[,] BH1, double[,] BH2) {
            int n1 = BH1.GetLength(0), n2 = BH2.GetLength(0);
            int nbins = nbins_theta * nbins_r;
            double[] sumbh1 = new double[n1],
                     sumbh2 = new double[n2];
            for (int i = 0; i < n1; ++i) {
                for (int j = 0; j < nbins; ++j) {
                    sumbh1[i] += BH1[i, j];
                }
            }
            for (int i = 0; i < n2; ++i) {
                for (int j = 0; j < nbins; ++j) {
                    sumbh2[i] += BH2[i, j];
                }
            }

            double[,] cost = new double[n1, n2];
            for (int i = 0; i < n1; ++i) {
                for (int j = 0; j < n2; ++j) {
                    double cost_ij = 0;
                    for (int k = 0; k < nbins; ++k) {
                        double tmp1 = BH1[i, k] / (sumbh1[i] + Epsilon),
                               tmp2 = BH2[j, k] / (sumbh2[j] + Epsilon);
                        cost_ij += ((tmp1 - tmp2) * (tmp1 - tmp2)) / (tmp1 + tmp2 + Epsilon);
                    }
                    cost[i, j] = cost_ij * 0.5;
                }
            }

            return cost;
        }

        public static double[][] HistCost2(double[][] BH1, double[][] BH2) {
            int n1 = BH1.GetLength(0), n2 = BH2.GetLength(0);
            int nbins = nbins_theta * nbins_r;
            double[] sumbh1 = new double[n1],
                     sumbh2 = new double[n2];
            for (int i = 0; i < n1; ++i) {
                for (int j = 0; j < nbins; ++j) {
                    sumbh1[i] += BH1[i][j];
                }
            }
            for (int i = 0; i < n2; ++i) {
                for (int j = 0; j < nbins; ++j) {
                    sumbh2[i] += BH2[i][j];
                }
            }

            double[][] cost = new double[n1][];
            for (int i = 0; i < n1; ++i) {
                cost[i] = new double[n2];
                for (int j = 0; j < n2; ++j) {
                    double cost_ij = 0;
                    for (int k = 0; k < nbins; ++k) {
                        double tmp1 = BH1[i][k] / (sumbh1[i] + Epsilon),
                               tmp2 = BH2[j][k] / (sumbh2[j] + Epsilon);
                        cost_ij += ((tmp1 - tmp2) * (tmp1 - tmp2)) / (tmp1 + tmp2 + Epsilon);
                    }
                    cost[i][j] = cost_ij * 0.5;
                }
            }

            return cost;
        }

        public static double HistCost(double[] H1, double[] H2) {
            double sum1 = 0, sum2 = 0;
            int nbins = H1.Length;
            for (int i = 0; i < nbins; ++i) {
                sum1 += H1[i];
                sum2 += H2[i];
            }

            double cost = 0;
            for (int k = 0; k < nbins; ++k) {
                double tmp1 = H1[k] / (sum1 + Epsilon),
                       tmp2 = H2[k] / (sum2 + Epsilon);
                cost += ((tmp1 - tmp2) * (tmp1 - tmp2)) / (tmp1 + tmp2 + Epsilon) * 0.5;
            }

            return cost;
        }

        public static double[,] GHistCost(double[,] GSC1, double[,] GSC2) {
            int n1 = GSC1.GetLength(0), n2 = GSC2.GetLength(0);
            int nbins = nbins_theta * nbins_r;

            double[,] v1 = new double[n1, nbins * 2],
                      v2 = new double[n2, nbins * 2];
            for (int i = 0; i < n1; ++i) {
                for (int j = 0, off = 0; j < nbins; ++j, off += 2) {
                    double t = GSC1[i, j];
                    v1[i, off] = Math.Cos(t);
                    v1[i, off + 1] = Math.Sin(t);
                }
            }
            for (int i = 0; i < n2; ++i) {
                for (int j = 0, off = 0; j < nbins; ++j, off += 2) {
                    double t = GSC2[i, j];
                    v2[i, off] = Math.Cos(t);
                    v2[i, off + 1] = Math.Sin(t);
                }
            }

            double[,] cost = new double[n1, n2];
            for (int i = 0; i < n1; ++i) {
                for (int j = 0; j < n2; ++j) {
                    double sum_square = 0;
                    for (int k = 0; k < nbins * 2; ++k) {
                        double tmp1 = v1[i, k], tmp2 = v2[j, k];
                        sum_square += (tmp1 - tmp2) * (tmp1 - tmp2);
                    }
                    cost[i, j] = Math.Sqrt(sum_square);
                }
            }

            return cost;
        }

        public static double GHistCost(double[] GSC1, double[] GSC2) {
            int nbins = GSC1.Length;

            double[] v1 = new double[nbins * 2],
                     v2 = new double[nbins * 2];
            for (int i = 0, off = 0; i < nbins; ++i, off += 2) {
                double t1 = GSC1[i], t2 = GSC2[i];
                v1[off] = Math.Cos(t1);
                v1[off + 1] = Math.Sin(t1);
                v2[off] = Math.Cos(t2);
                v2[off + 1] = Math.Sin(t2);
            }

            double sum_square = 0;
            for (int k = 0; k < nbins * 2; ++k) {
                double tmp1 = v1[k], tmp2 = v2[k];
                sum_square += (tmp1 - tmp2) * (tmp1 - tmp2);
            }

            return Math.Sqrt(sum_square);
        }

        public static double ChiSquareDistance(int[] H1, int[] H2) {
            double dist = 0;
            for (int k = 0; k < H1.Length; ++k) {
                double v1 = H1[k], v2 = H2[k];
                dist += ((v1 - v2) * (v1 - v2)) / (v1 + v2 + Epsilon) * 0.5;
            }
            return dist;
        }
    }
}
