using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        /// <summary>
        /// 简化版的ShapeContext计算，没有矩阵，没有切向量，没有野点过滤
        /// </summary>
        public static double[,] ComputeSC(List<Point> samples) {
            int nsamp = samples.Count;
            double[] x_array = new double[nsamp],
                     y_array = new double[nsamp];
            for (int i = 0; i < nsamp; ++i) {
                x_array[i] = samples[i].X;
                y_array[i] = samples[i].Y;
            }

            double[,] r_array = new double[nsamp, nsamp];
            int[,] theta_array_q = new int[nsamp, nsamp];
            double mean_dist = 0;
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    double xi = x_array[i], yi = y_array[i],
                           xj = x_array[j], yj = y_array[j];
                    double r = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj));
                    r_array[i, j] = r;
                    mean_dist += r;

                    double t = (Math.Atan2(yi - yj, xi - xj) % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);
                    theta_array_q[i, j] = (int)(t / (2 * Math.PI / nbins_theta));
                }
            }
            mean_dist /= (nsamp * nsamp);
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    r_array[i, j] /= mean_dist;
                }
            }

            double[] r_bin_edges = Utils.LogSpace(Math.Log10(r_inner), Math.Log10(r_outer), nbins_r);
            int[,] r_array_q = new int[nsamp, nsamp];
            for (int m = 0; m < nbins_r; ++m) {
                for (int i = 0; i < nsamp; ++i) {
                    for (int j = 0; j < nsamp; ++j) {
                        if (r_array[i, j] < r_bin_edges[m]) {
                            ++r_array_q[i, j];
                        }
                    }
                }
            }

            double[,] BH = new double[nsamp, nbins_theta * nbins_r];
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    int tq = theta_array_q[i, j], tr = r_array_q[i, j];
                    if (tr > 0) tr--;
                    ++BH[i, tr * nbins_theta + tq];
                }
            }

            return BH;
        }

        public static double[][] ComputeSC2(List<Point> samples) {
            int nsamp = samples.Count;
            double[] x_array = new double[nsamp],
                     y_array = new double[nsamp];
            for (int i = 0; i < nsamp; ++i) {
                x_array[i] = samples[i].X;
                y_array[i] = samples[i].Y;
            }

            double[,] r_array = new double[nsamp, nsamp];
            int[,] theta_array_q = new int[nsamp, nsamp];
            double mean_dist = 0;
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    double xi = x_array[i], yi = y_array[i],
                           xj = x_array[j], yj = y_array[j];
                    double r = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj));
                    r_array[i, j] = r;
                    mean_dist += r;

                    double t = (Math.Atan2(yi - yj, xi - xj) % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);
                    theta_array_q[i, j] = (int)(t / (2 * Math.PI / nbins_theta));
                }
            }
            mean_dist /= (nsamp * nsamp);
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    r_array[i, j] /= mean_dist;
                }
            }

            double[] r_bin_edges = Utils.LogSpace(Math.Log10(r_inner), Math.Log10(r_outer), nbins_r);
            int[,] r_array_q = new int[nsamp, nsamp];
            for (int m = 0; m < nbins_r; ++m) {
                for (int i = 0; i < nsamp; ++i) {
                    for (int j = 0; j < nsamp; ++j) {
                        if (r_array[i, j] < r_bin_edges[m]) {
                            ++r_array_q[i, j];
                        }
                    }
                }
            }

            double[][] BH = new double[nsamp][];
            for (int i = 0; i < nsamp; ++i) {
                BH[i] = new double[nbins_theta * nbins_r];
                for (int j = 0; j < nsamp; ++j) {
                    int tq = theta_array_q[i, j], tr = r_array_q[i, j];
                    if (tr > 0) tr--;
                    ++BH[i][tr * nbins_theta + tq];
                }
            }

            return BH;
        }

        public static double[,] ComputeGSC(Image<Gray, Byte> image, List<Point> samples) {
            int nsamp = samples.Count, h = image.Height, w = image.Width;
            double[] x_array = new double[nsamp],
                     y_array = new double[nsamp],
                     tagent = new double[nsamp];
            for (int i = 0; i < nsamp; ++i) {
                int x = samples[i].X, y = samples[i].Y;
                x_array[i] = x;
                y_array[i] = y;
                double gx = 0, gy = 0;
                if (y == 0) gy = image[1, x].Intensity - image[0, x].Intensity;
                else if (y == h - 1) gy = image[h - 1, x].Intensity - image[h - 2, x].Intensity;
                else gy = (image[y + 1, x].Intensity - image[y - 1, x].Intensity) * 0.5;

                if (x == 0) gx = image[y, 1].Intensity - image[y, 0].Intensity;
                else if (x == w - 1) gx = image[y, w - 1].Intensity - image[y, w - 2].Intensity;
                else gx = (image[y, x + 1].Intensity - image[y, x - 1].Intensity) * 0.5;
                tagent[i] = ((Math.Atan2(gy, gx) + 0.5 * Math.PI) % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);
            }

            double[,] r_array = new double[nsamp, nsamp];
            int[,] theta_array_q = new int[nsamp, nsamp];
            double mean_dist = 0;
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    double xi = x_array[i], yi = y_array[i],
                           xj = x_array[j], yj = y_array[j];
                    double r = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj));
                    r_array[i, j] = r;
                    mean_dist += r;

                    double t = (Math.Atan2(yi - yj, xi - xj) % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);
                    theta_array_q[i, j] = (int)(t / (2 * Math.PI / nbins_theta));
                }
            }
            mean_dist /= (nsamp * nsamp);
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    r_array[i, j] /= mean_dist;
                }
            }

            double[] r_bin_edges = Utils.LogSpace(Math.Log10(r_inner), Math.Log10(r_outer), nbins_r);
            int[,] r_array_q = new int[nsamp, nsamp];
            for (int m = 0; m < nbins_r; ++m) {
                for (int i = 0; i < nsamp; ++i) {
                    for (int j = 0; j < nsamp; ++j) {
                        if (r_array[i, j] < r_bin_edges[m]) {
                            ++r_array_q[i, j];
                        }
                    }
                }
            }

            double[,] GSC = new double[nsamp, nbins_theta * nbins_r];
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    int tq = theta_array_q[i, j], tr = r_array_q[i, j];
                    if (tr > 0) tr--;
                    //++GSC[i, tr * nbins_theta + tq];
                    GSC[i, tr * nbins_theta + tq] += tagent[j];
                }
            }

            return GSC;
        }

        public static double[][] ComputeGSC2(Image<Gray, Byte> image, List<Point> samples) {
            int nsamp = samples.Count, h = image.Height, w = image.Width;
            double[] x_array = new double[nsamp],
                     y_array = new double[nsamp],
                     tagent = new double[nsamp];
            for (int i = 0; i < nsamp; ++i) {
                int x = samples[i].X, y = samples[i].Y;
                x_array[i] = x;
                y_array[i] = y;
                double gx = 0, gy = 0;
                if (y == 0) gy = image[1, x].Intensity - image[0, x].Intensity;
                else if (y == h - 1) gy = image[h - 1, x].Intensity - image[h - 2, x].Intensity;
                else gy = (image[y + 1, x].Intensity - image[y - 1, x].Intensity) * 0.5;

                if (x == 0) gx = image[y, 1].Intensity - image[y, 0].Intensity;
                else if (x == w - 1) gx = image[y, w - 1].Intensity - image[y, w - 2].Intensity;
                else gx = (image[y, x + 1].Intensity - image[y, x - 1].Intensity) * 0.5;
                tagent[i] = ((Math.Atan2(gy, gx) + 0.5 * Math.PI) % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);
            }

            double[,] r_array = new double[nsamp, nsamp];
            int[,] theta_array_q = new int[nsamp, nsamp];
            double mean_dist = 0;
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    double xi = x_array[i], yi = y_array[i],
                           xj = x_array[j], yj = y_array[j];
                    double r = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj));
                    r_array[i, j] = r;
                    mean_dist += r;

                    double t = (Math.Atan2(yi - yj, xi - xj) % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI);
                    theta_array_q[i, j] = (int)(t / (2 * Math.PI / nbins_theta));
                }
            }
            mean_dist /= (nsamp * nsamp);
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    r_array[i, j] /= mean_dist;
                }
            }

            double[] r_bin_edges = Utils.LogSpace(Math.Log10(r_inner), Math.Log10(r_outer), nbins_r);
            int[,] r_array_q = new int[nsamp, nsamp];
            for (int m = 0; m < nbins_r; ++m) {
                for (int i = 0; i < nsamp; ++i) {
                    for (int j = 0; j < nsamp; ++j) {
                        if (r_array[i, j] < r_bin_edges[m]) {
                            ++r_array_q[i, j];
                        }
                    }
                }
            }

            double[][] GSC = new double[nsamp][];
            for (int i = 0; i < nsamp; ++i) {
                GSC[i] = new double[nbins_theta * nbins_r];
                for (int j = 0; j < nsamp; ++j) {
                    int tq = theta_array_q[i, j], tr = r_array_q[i, j];
                    if (tr > 0) tr--;
                    //++GSC[i, tr * nbins_theta + tq];
                    GSC[i][tr * nbins_theta + tq] += tagent[j];
                }
            }

            return GSC;
        }

        /*
        function [BH,mean_dist]=sc_compute(Bsamp,Tsamp,mean_dist,nbins_theta,nbins_r,r_inner,r_outer,out_vec);
        % [BH,mean_dist]=sc_compute(Bsamp,Tsamp,mean_dist,nbins_theta,nbins_r,r_inner,r_outer,out_vec);
        %
        % compute (r,theta) histograms for points along boundary 
        %
        % Bsamp is 2 x nsamp (x and y coords.)
        % Bsamp是2n长的数组，提供x,y
        % Tsamp is 1 x nsamp (tangent theta)
        % Tsamp是n长的数组，提供tan(theta)
        % out_vec is 1 x nsamp (0 for inlier, 1 for outlier)
        % out_vec是n长的数组，表示是否野点
        %
        % mean_dist is the mean distance, used for length normalization
        % mean_dist是平均距离，用来归一化
        % if it is not supplied, then it is computed from the data
        %
        % outliers are not counted in the histograms, but they do get
        % assigned a histogram
        %
        end
         */
        public Matrix ComputeSC(Matrix Bsamp, Matrix Tsamp, double? mean_dist, out double mean_dist_out, bool[] out_vec) {
            //nsamp=size(Bsamp,2);
            //% 求n长
            //int nsamp = Bsamp.Columns;

            //in_vec=out_vec==0;
            //% 初始化野点标记数组吧？
            var in_vec = Utils.InitArray<bool>(nsamp, true);
            out_vec.FillArray(false);

            //% compute r,theta arrays 计算r和t的数组
            //r_array=real(sqrt(dist2(Bsamp',Bsamp'))); % real is needed to
            //                                          % prevent bug in Unix version
            //% r_array是Bsamp的转置和Bsamp的转置之间的dist2
            //% 应该是个n*n的数组了
            var Bsampt = Bsamp.Transpose();
            var r_array = Dist2(Bsampt, Bsampt).Each(Math.Sqrt);

            //theta_array_abs=atan2(Bsamp(2,:)'*ones(1,nsamp)-ones(nsamp,1)*Bsamp(2,:),Bsamp(1,:)'*ones(1,nsamp)-ones(nsamp,1)*Bsamp(1,:))';
            var theta_array_abs = new DenseMatrix(nsamp, nsamp);
            var x_array = Bsamp.GetRow(0);
            var y_array = Bsamp.GetRow(1);
            for (int i = 0; i < nsamp; ++i) {
                for (int j = 0; j < nsamp; ++j) {
                    double xi = x_array[i], yi = y_array[i],
                           xj = x_array[j], yj = y_array[j];
                    theta_array_abs[j, i] = Math.Atan2(yi - yj, xi - xj);
                }
            }

            var Tsampt = Tsamp.Transpose();

            //theta_array=theta_array_abs-Tsamp'*ones(1,nsamp);
            //% 不知道这里是干什么，但是theta_array应该是theta的二维数组
            var theta_array = theta_array_abs - Tsampt * Ones(1, nsamp);

            //% create joint (r,theta) histogram by binning r_array and
            //% theta_array
            //% 通过对r_array和theta_array插槽来求直方图

            //% normalize distance by mean, ignoring outliers
            //% 通过均值来归一化距离参数，忽略野点
            //if isempty(mean_dist)
            if (mean_dist == null) {
                //   tmp=r_array(in_vec,:);
                //   tmp=tmp(:,in_vec);
                //   mean_dist=mean(tmp(:));
                //end
                double mean_sum = 0;
                int mean_count = 0;
                for (int i = 0; i < r_array.Rows; ++i) {
                    for (int j = 0; j < r_array.Columns; ++j) {
                        if (in_vec[i] && in_vec[j]) {
                            mean_sum += r_array[i, j];
                            ++mean_count;
                        }
                    }
                }
                mean_dist_out = mean_sum / mean_count;
            } else {
                mean_dist_out = mean_dist.Value;
            }
            //% 看不懂，但是应该是求非野点的平均值吧，结果存在mean_dist中
            //r_array_n=r_array/mean_dist;
            //% r_array_n是规范化结果，用均值规范化，而不是用最大值，难道我错了？
            var r_array_n = r_array / mean_dist_out;

            //% use a log. scale for binning the distances
            //r_bin_edges=logspace(log10(r_inner),log10(r_outer),5);
            var r_bin_edges = Utils.LogSpace(Math.Log10(r_inner), Math.Log10(r_outer), nbins_r);

            //r_array_q=zeros(nsamp,nsamp);
            var r_array_q = Zeros(nsamp, nsamp);

            //for m=1:nbins_r %m从1循环到r槽数
            for (int i = 0; i < nbins_r; ++i) {
                //   r_array_q=r_array_q+(r_array_n<r_bin_edges(m)); % r_array_q = r_array_q + (r_array_n < r_bin_edges(m))
                r_array_q = r_array_q + r_array_n.Each(v => v < r_bin_edges[i] ? 1 : 0);
                //   % 这个代码的意思应该是说r_array_q中下标小于m的项统统加上r_array_n中小于r_bin_edges(m)的项目数
                //end
            }
            //fz=r_array_q>0; % flag all points inside outer boundary
            //% fz为r_array_q中大于0的项目标记（这应该是个数组）
            var fz = r_array_q.Each(v => v > 0 ? 1 : 0);

            //% put all angles in [0,2pi) range
            //theta_array_2 = rem(rem(theta_array,2*pi)+2*pi,2*pi);
            //% 将所有方向角都弄到[0,2pi)范围内，看不懂
            //% 猜测theta_array_2 = theta_array.Select(t => (t % (2*pi) + (2*pi)) % (2*pi))
            var theta_array_2 = theta_array.Each(t => (t % (2 * Math.PI) + 2 * Math.PI) % (2 * Math.PI));

            //% quantize to a fixed set of angles (bin edges lie on 0,(2*pi)/k,...2*pi
            //% 将方向角量化到固定的角度
            //theta_array_q = 1+floor(theta_array_2/(2*pi/nbins_theta));
            //% theta_array_q = theta_array_2.Select(t => floor(t / (2*pi/nbins_theta)) + 1)
            //% 这是以1为起点的吧
            var theta_array_q = theta_array_2.Each(t => (int)(t / (2 * Math.PI / nbins_theta)));

            //nbins=nbins_theta*nbins_r;
            int nbins = nbins_theta * nbins_r;
            //% 不用说
            //BH=zeros(nsamp,nbins);

            var BH = Zeros(nsamp, nbins);

            //% BH应该就是最后的直方图
            //for n=1:nsamp % n从1循环到nsamp
            for (int i = 0; i < nsamp; ++i) {
                //   fzn=fz(n,:)&in_vec;
                //   Sn=sparse(theta_array_q(n,fzn), r_array_q(n,fzn), 1, nbins_theta, nbins_r);
                //   % sparse这里貌似是生成稀疏矩阵的，Sn应该是用于计算匹配代价的直方图，而BH是包含野点的
                //   BH(n,:)=Sn(:)'; % 转置
                for (int j = 0; j < nsamp; ++j) {
                    int tq = (int)theta_array_q[i, j], tr = (int)r_array_q[i, j];
                    if (tr > 0) {
                        tr--;
                        ++BH[i, tr * nbins_theta + tq];
                    }
                }
            }

            return BH;
        }
    }
}
