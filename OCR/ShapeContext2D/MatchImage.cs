using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using dnAnalytics.LinearAlgebra;
using System.Drawing;
using Jim.OCR.Algorithms;
using dnAnalytics.LinearAlgebra.Decomposition;
using System.IO;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        public double MatchFile() {
            Image<Gray, Byte> pic1 = new Image<Gray, Byte>(file1).Resize(matchScale, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR),
                              pic2 = new Image<Gray, Byte>(file2).Resize(matchScale, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            List<Point> edge1 = findEdge(pic1),
                        edge2 = findEdge(pic2);
            nsamp = Math.Min(maxsamplecount, Math.Min(edge1.Count, edge2.Count));
            edge1 = edge1.Sample(nsamp);
            edge2 = edge2.Sample(nsamp);

            Matrix t1, t2, V1, V2;
            ExtractBoundary(pic1, edge1, out origX, out V1, out t1);
            ExtractBoundary(pic2, edge2, out origY, out V2, out t2);

            return MatchIteration(origX, origY, V1, V2, t1, t2);
        }

        public double MatchChar(char a, char b) {
            Image<Bgr, Byte> pic1 = new Image<Bgr, Byte>(100, 100).Resize(matchScale, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR),
                             pic2 = new Image<Bgr, Byte>(100, 100).Resize(matchScale, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            using (var g1 = Graphics.FromImage(pic1.Bitmap)) {
                g1.Clear(Color.Black);
                g1.DrawString(a.ToString(), new Font("Arial", 64), Brushes.White, Point.Empty);
            }
            using (var g2 = Graphics.FromImage(pic2.Bitmap)) {
                g2.Clear(Color.Black);
                g2.DrawString(b.ToString(), new Font("Comic Sans MS", 64), Brushes.White, Point.Empty);
            }

            List<Point> edge1 = findEdge(pic1.Convert<Gray, Byte>()),
                        edge2 = findEdge(pic2.Convert<Gray, Byte>());
            nsamp = Math.Min(maxsamplecount, Math.Min(edge1.Count, edge2.Count));
            edge1 = edge1.Sample(nsamp);
            edge2 = edge2.Sample(nsamp);

            Matrix t1, t2, V1, V2;
            ExtractBoundary(pic1.Convert<Gray, Byte>(), edge1, out origX, out V1, out t1);
            ExtractBoundary(pic2.Convert<Gray, Byte>(), edge2, out origY, out V2, out t2);

            return MatchIteration(origX, origY, V1, V2, t1, t2);
        }

        public double MatchIteration(Matrix X, Matrix Y, Matrix V1, Matrix V2, Matrix t1, Matrix t2) {
            timer.Clear();
            double timeused = 0;

            timer.Restart();
            var tk = t1.Clone(); // tk=t1;
            int w = 4;
            int ndum = (int)(nsamp * ndum_frac);
            nsamp1 = nsamp2 = nsamp;

            #region demo2迭代
            Matrix Xk = X.Clone();
            int N1 = V1.Rows, N2 = V2.Columns;

            if (display_flag) {
                Draw(X, Y, V1, V2, @"D:\Play Data\Iteration\原图.bmp", "原始图像和采样");
                //DrawGradient(X, Y, t1, t2, N2, N1, @"D:\Play Data\Iteration\原图梯度.bmp", "原始图像切向量");
            }

            int k = 0;
            var out_vec_1 = Utils.InitArray<bool>(nsamp1, false);//out_vec_1=zeros(1,nsamp1); 
            var out_vec_2 = Utils.InitArray<bool>(nsamp2, false);//out_vec_2=zeros(1,nsamp2);

            double ori_weight = 0.1;
            double tan_eps = 1.0;
            bool affine_start_flag = true;
            bool polarity_flag = true;
            double matchcost = double.MaxValue;
            double sc_cost = 0, aff_cost = 0, E = 0;

            Matrix cx = null, cy = null; // cx, cy是插值线性方程组的解的两列
            Matrix axt = null, wxt = null, ayt = null, wyt = null, d2 = null, U = null,
                X2 = null, Y2 = null, X2b = null, X3b = null, Y3 = null;
            double mean_dist_1 = 0, mean_dist_2 = 0;
            var min1 = new[] { new { Val = 0.0, Idx = 0 } };
            var min2 = min1;

            #region 用于打网格的坐标
            Matrix coordX = null, coordY = null;
            int coordMargin = (int)(N1 * coordMarginRate);
            MatrixUtils.CreateGrid(N1 + coordMargin * 2, N2 + coordMargin * 2, out coordX, out coordY);
            coordX = coordX.Each(v => v - coordMargin * 2);
            coordY = coordY.Each(v => v - coordMargin * 2);
            //int MM = N1 * N2 / 25; // M=length(x);
            #endregion

            timeused += timer.StopAndSay("初始化");

            while (k < n_iter) {
                Debug("Iter={0}", k);

                #region 计算两个形状上下文
                timer.Restart();
                // [BH1,mean_dist_1]=sc_compute(Xk',zeros(1,nsamp),mean_dist_global,nbins_theta,nbins_r,r_inner,r_outer,out_vec_1);
                var BH1 = ComputeSC(Xk.Transpose(), Zeros(1, nsamp), mean_dist_global, out mean_dist_1, out_vec_1);
                //var BH1 = ComputeSC(Xk.Transpose(), t1.Transpose(), mean_dist_global, out mean_dist_1, out_vec_1);

                // [BH2,mean_dist_2]=sc_compute(Y',zeros(1,nsamp),mean_dist_global,nbins_theta,nbins_r,r_inner,r_outer,out_vec_2);
                var BH2 = ComputeSC(Y.Transpose(), Zeros(1, nsamp), mean_dist_global, out mean_dist_2, out_vec_2);
                //var BH2 = ComputeSC(Y.Transpose(), t2.Transpose(), mean_dist_global, out mean_dist_2, out_vec_2);

                timeused += timer.StopAndSay("计算两个形状上下文");

                Debug("Mean_dist_1:{0:F4}", mean_dist_1);
                Debug("Mean_dist_2:{0:F4}", mean_dist_2);
                #endregion

                #region 计算lambda_o和beta_k
                double lambda_o;
                if (affine_start_flag) {
                    if (k == 0)
                        lambda_o = 1000;
                    else
                        lambda_o = beta_init * Math.Pow(r, k - 1); // lambda_o=beta_init*r^(k-2);	
                } else {
                    lambda_o = beta_init * Math.Pow(r, k); // lambda_o=beta_init*r^(k-1);
                }
                double beta_k = mean_dist_2 * mean_dist_2 * lambda_o;
                #endregion

                #region 计算代价矩阵
                timer.Restart();
                var costmat_shape = HistCost(BH1, BH2); // costmat_shape = hist_cost_2(BH1, BH2);

                // theta_diff=repmat(tk,1,nsamp)-repmat(t2',nsamp,1);
                var theta_diff = tk.RepMat(1, nsamp) - t2.Transpose().RepMat(nsamp, 1);

                Matrix costmat_theta;
                if (polarity_flag) {
                    // costmat_theta=0.5*(1-cos(theta_diff));
                    //costmat_theta = 0.5 * (Ones(costmat_shape.Rows, costmat_shape.Columns) - theta_diff.Each(v => Math.Cos(v)));
                    costmat_theta = theta_diff.Each(v => 0.5 * (1 - Math.Cos(v)));
                } else {
                    // costmat_theta=0.5*(1-cos(2*theta_diff));
                    //costmat_theta = 0.5 * (Ones(costmat_shape.Rows, costmat_shape.Columns) - theta_diff.Each(v => Math.Cos(2 * v)));
                    costmat_theta = theta_diff.Each(v => 0.5 * (1 - Math.Cos(2 * v)));
                }
                // costmat=(1-ori_weight)*costmat_shape+ori_weight*costmat_theta;
                var costmat = (1 - ori_weight) * costmat_shape + ori_weight * costmat_theta;

                int nptsd = nsamp + ndum; // nptsd=nsamp+ndum;
                var costmat2 = new DenseMatrix(nptsd, nptsd, eps_dum); // costmat2=eps_dum*ones(nptsd,nptsd);
                costmat2.SetSubMatrix(0, nsamp, 0, nsamp, costmat); // costmat2(1:nsamp,1:nsamp)=costmat;
                timeused += timer.StopAndSay("计算代价矩阵");
                #endregion

                #region 匈牙利算法
                timer.Restart();
                var costmat_int = new int[nptsd, nptsd];
                for (int i = 0; i < nptsd; ++i) {
                    for (int j = 0; j < nptsd; ++j) {
                        costmat_int[i, j] = (int)(costmat2[i, j] * 10000);
                    }
                }
                var km = new KM(nptsd, costmat_int);
                km.Match(false);
                matchcost = km.MatchResult / 10000.0;
                int[] cvec = km.MatchPair; // cvec=hungarian(costmat2);
                timeused += timer.StopAndSay("匈牙利算法");
                #endregion

                #region 计算野点标记向量，重排匹配点
                timer.Restart();
                int[] cvec2 = cvec.Select((v, i) => new { Val = v, Idx = i })
                                  .OrderBy(v => v.Val)
                                  .Select(v => v.Idx)
                                  .ToArray();// [a,cvec2]=sort(cvec);
                out_vec_1 = cvec2.Take(nsamp1).Select(v => v > nsamp2).ToArray(); // out_vec_1=cvec2(1:nsamp1)>nsamp2;
                out_vec_2 = cvec.Take(nsamp2).Select(v => v > nsamp1).ToArray(); // out_vec_2=cvec(1:nsamp2)>nsamp1;

                //X2 = NaNs(nptsd, 2); // X2=NaN*ones(nptsd,2);
                //X2.SetSubMatrix(0, nsamp1, 0, X2.Columns, Xk); // X2(1:nsamp1,:)=Xk;	
                //X2 = X2.SortRowsBy(cvec); // X2=X2(cvec,:);

                X2b = NaNs(nptsd, 2); // X2b=NaN*ones(nptsd,2);
                X2b.SetSubMatrix(0, nsamp1, 0, X2b.Columns, X); // X2b(1:nsamp1,:)=X;
                X2b = X2b.SortRowsBy(cvec); // X2b=X2b(cvec,:);

                Y2 = NaNs(nptsd, 2); // Y2=NaN*ones(nptsd,2);
                Y2.SetSubMatrix(0, nsamp2, 0, Y2.Columns, Y); // Y2(1:nsamp2,:)=Y;	


                var ind_good = X2b.GetColumn(1).Take(nsamp).FindIdxBy(v => !double.IsNaN(v)); // ind_good=find(~isnan(X2b(1:nsamp,1)));
                int n_good = ind_good.Length; // n_good=length(ind_good);

                X3b = X2b.FilterRowsBy(ind_good);//  X3b=X2b(ind_good,:);
                Y3 = Y2.FilterRowsBy(ind_good); // Y3=Y2(ind_good,:);
                timeused += timer.StopAndSay("计算野点标记向量，重排匹配点");
                #endregion

                #region figure 2
                if (display_flag) {
                    //figure(2)
                    //plot(X2(:,1),X2(:,2),'b+',Y2(:,1),Y2(:,2),'ro')
                    //hold on
                    //h=plot([X2(:,1) Y2(:,1)]',[X2(:,2) Y2(:,2)]','k-');

                    //if display_flag
                    //%	 set(h,'linewidth',1)
                    //quiver(Xk(:,1),Xk(:,2),cos(tk),sin(tk),0.5,'b') // 画箭头
                    //quiver(Y(:,1),Y(:,2),cos(t2),sin(t2),0.5,'r')
                    DrawGradient(Xk, Y, tk, t2, N2, N1,
                                 String.Format(@"D:\Play Data\Iteration\梯度\{0}.bmp", k),
                                 String.Format("Iter={0}梯度方向\n匹配点数{1}", k, n_good));
                    //end
                    //hold off
                    //axis('ij')
                    //title([int2str(n_good) ' correspondences (warped X)'])
                    //axis([1 N2 1 N1])
                    //drawnow	
                }
                #endregion

                #region figure 3 显示未变形图的匹配关系
                if (display_flag) {
                    //% show the correspondences between the untransformed images
                    //figure(3)
                    //plot(X(:,1),X(:,2),'b+',Y(:,1),Y(:,2),'ro')
                    //ind=cvec(ind_good);
                    //hold on
                    //plot([X2b(:,1) Y2(:,1)]',[X2b(:,2) Y2(:,2)]','k-')
                    //hold off
                    //axis('ij')
                    //title([int2str(n_good) ' correspondences (unwarped X)'])
                    //axis([1 N2 1 N1])
                    //drawnow	
                    Draw(X, Y, cvec, null, N2, N1, String.Format(@"D:\Play Data\Iteration\匹配\{0}.bmp", k),
                         String.Format("Iter={0}\n匹配代价{1},匹配数{2}", k, matchcost, n_good));
                }
                #endregion

                #region 求解变换矩阵
                timer.Restart();
                Bookstein(X3b, Y3, beta_k, ref cx, ref cy, ref E); // [cx,cy,E]=bookstein(X3b,Y3,beta_k);
                timeused += timer.StopAndSay("求解变换矩阵");
                #endregion

                #region 通过解出来的变换对点和梯度进行变换
                timer.Restart();
                // % calculate affine cost
                var A = MatrixUtils.RankHorizon(
                    cx.GetSubMatrix(n_good + 1, 2, 0, 1), cy.GetSubMatrix(n_good + 1, 2, 0, 1)
                );//A=[cx(n_good+2:n_good+3,:) cy(n_good+2:n_good+3,:)];
                var s = new Svd(A, true).S(); // s=svd(A);
                aff_cost = Math.Log(s[0] / s[1]); // aff_cost=log(s(1)/s(2));

                // % calculate shape context cost
                min1 = costmat.GetColumns().Select(col => {
                    int minwhere = 0;
                    for (int i = 1; i < col.Count; ++i) {
                        if (col[i] < col[minwhere]) minwhere = i;
                    }
                    return new { Val = col[minwhere], Idx = minwhere };
                }).ToArray();// [a1,b1]=min(costmat,[],1);
                min2 = costmat.GetRows().Select(row => {
                    int minwhere = 0;
                    for (int i = 1; i < row.Count; ++i) {
                        if (row[i] < row[minwhere]) minwhere = i;
                    }
                    return new { Val = row[minwhere], Idx = minwhere };
                }).ToArray(); // [a2,b2]=min(costmat,[],2);}
                sc_cost = Math.Max(min1.Average(a => a.Val), min2.Average(a => a.Val)); // sc_cost=max(mean(a1),mean(a2));

                // % warp each coordinate
                axt = cx.GetSubMatrix(n_good, 3, 0, 1).Transpose(); // axt是cx中的最后三个，即a的x分量
                wxt = cx.GetSubMatrix(0, n_good, 0, 1).Transpose(); // wxt是cs中的前n个，即w的x分量
                ayt = cy.GetSubMatrix(n_good, 3, 0, 1).Transpose();
                wyt = cy.GetSubMatrix(0, n_good, 0, 1).Transpose();

                d2 = Dist2(X3b, X).Each(v => v > 0 ? v : 0); // d2=max(dist2(X3b,X),0);
                U = d2.PointMultiply(d2.Each(v => Math.Log(v + Epsilon))); // U=d2.*log(d2+eps);

                Debug("MatchCost:{0:F4}\taff_cost:{1:F4}\tsc_cost:{2:F4}\tE:{3:F8}", matchcost, aff_cost, sc_cost, E);

                var Z = Transformation(X, U, axt, wxt, ayt, wyt);

                //% apply the warp to the tangent vectors to get the new angles
                var Xtan = X + tan_eps * MatrixUtils.RankHorizon(t1.Each(Math.Cos), t1.Each(Math.Sin)); // Xtan=X+tan_eps*[cos(t1) sin(t1)];
                d2 = Dist2(X3b, Xtan).Each(v => v > 0 ? v : 0); // d2=max(dist2(X3b,Xtan),0);
                U = d2.PointMultiply(d2.Each(v => Math.Log(v + Epsilon))); // U=d2.*log(d2+eps);
                //Transformation(Xtan, U, axt, wxt, ayt, wyt, out fx, out fy);
                //var Ztan = MatrixUtils.RankVertical(fx, fy).Transpose(); // Ztan=[fx; fy]';
                var Ztan = Transformation(Xtan, U, axt, wxt, ayt, wyt);
                for (int i = 0; i < nsamp; ++i) {
                    tk[i, 0] = Math.Atan2(Ztan[i, 1] - Z[i, 1], Ztan[i, 0] - Z[i, 0]);
                }//tk=atan2(Ztan(:,2)-Z(:,2),Ztan(:,1)-Z(:,1));
                timeused += timer.StopAndSay("通过解出来的变换对点和梯度进行变换");
                #endregion

                #region figure 4 显示变形后的点集
                if (display_flag) {
                    //figure(4)
                    //plot(Z(:,1),Z(:,2),'b+',Y(:,1),Y(:,2),'ro');
                    //axis('ij')
                    //title(['k=' int2str(k) ', \lambda_o=' num2str(lambda_o) ', I_f=' num2str(E) ', aff.cost=' num2str(aff_cost) ', SC cost=' num2str(sc_cost)])
                    //axis([1 N2 1 N1])
                    //% show warped coordinate grid
                    //fx_aff=cx(n_good+1:n_good+3)'*[ones(1,M); x'; y'];
                    //d2=dist2(X3b,[x y]);
                    //fx_wrp=cx(1:n_good)'*(d2.*log(d2+eps));
                    //fx=fx_aff+fx_wrp;
                    //fy_aff=cy(n_good+1:n_good+3)'*[ones(1,M); x'; y'];
                    //fy_wrp=cy(1:n_good)'*(d2.*log(d2+eps));
                    //fy=fy_aff+fy_wrp;
                    //hold on
                    //plot(fx,fy,'k.','markersize',1)
                    //hold off
                    //drawnow
                    d2 = Dist2(X3b, MatrixUtils.RankHorizon(coordX, coordY));//d2=dist2(X3b,[x y]);
                    U = d2.PointMultiply(d2.Each(v => Math.Log(v + Epsilon)));
                    //Transformation(MatrixUtils.RankHorizon(coordX, coordY), U, axt, wxt, ayt, wyt, out fx, out fy);
                    //var coordsT = MatrixUtils.RankVertical(fx, fy).Transpose();
                    var coordsT = Transformation(MatrixUtils.RankHorizon(coordX, coordY), U, axt, wxt, ayt, wyt);

                    Draw(Z, Y, null, coordsT, N2, N1, String.Format(@"D:\Play Data\Iteration\变换\{0}.bmp", k),
                        String.Format("Iter={0}\nλo={1:F4},If={2:F4},aff_cost{3:F4},sc_cost{4:F4}", k, lambda_o, E, aff_cost, sc_cost));
                }
                #endregion

                Xk = Z.Clone();
                ++k;
            }

            #endregion

            #region 迭代完成后的代价计算

            #region 我来尝试计算Dsc
            // Xk是Q变换后的结果，而Y是模板图形P
            /*
             * Dsc = Avg_each_p_in_P(argmin(q in Q, C(p, T(q))) + Avg_each_q_in_Q(argmin(p in P, C(p, T(q)))
             * */
            double mean_dist_final_1;
            var scQ = ComputeSC(Xk.Transpose(), Zeros(1, nsamp), mean_dist_global, out mean_dist_final_1, out_vec_1);
            //var scQ = ComputeSC(Xk.Transpose(), Zeros(1, nsamp), mean_dist_1, out mean_dist_final_1, out_vec_1);
            //var scQ = ComputeSC(Xk.Transpose(), t1.Transpose(), mean_dist_1, out mean_dist_final_1, out_vec_1);

            double mean_dist_final_2;
            var scP = ComputeSC(Y.Transpose(), Zeros(1, nsamp), mean_dist_global, out mean_dist_final_2, out_vec_2);
            //var scP = ComputeSC(Y.Transpose(), Zeros(1, nsamp), mean_dist_2, out mean_dist_final_2, out_vec_2);
            //var scP = ComputeSC(Y.Transpose(), t2.Transpose(), mean_dist_2, out mean_dist_final_2, out_vec_2);

            var costmat_final = HistCost(scQ, scP);
            double distance_sc = costmat_final.GetRows().Select(row => row.Min()).Average()
                               + costmat_final.GetColumns().Select(col => col.Min()).Average();
            Debug("distance_sc:{0}", distance_sc);

            #endregion

            #region 图像变换和插值
            timer.Restart();
            //[x,y]=meshgrid(1:N2,1:N1);
            //x=x(:);y=y(:);
            Matrix x = null, y = null;
            MatrixUtils.CreateGrid(N1, N2, out x, out y);
            //int M = N1 * N2; // M=length(x);
            d2 = Dist2(X3b, MatrixUtils.RankHorizon(x, y));//d2=dist2(X3b,[x y]);
            U = d2.PointMultiply(d2.Each(v => Math.Log(v + Epsilon)));
            //Transformation(MatrixUtils.RankHorizon(x, y), U, axt, wxt, ayt, wyt, out fx, out fy);
            var fxy = Transformation(MatrixUtils.RankHorizon(x, y), U, axt, wxt, ayt, wyt);

            //disp('computing warped image...')
            //V1w=griddata(reshape(fx,N1,N2),reshape(fy,N1,N2),V1,reshape(x,N1,N2),reshape(y,N1,N2));   
            Matrix V1w = Interpolation(
                fxy.GetSubMatrix(0, fxy.Rows, 0, 1).Reshape(N1, N2),
                fxy.GetSubMatrix(0, fxy.Rows, 1, 1).Reshape(N1, N2),
                V1
            );

            #region 这个山寨插值方法会造成图像裂缝，用闭运算来尝试修补
            Image<Gray, Byte> img = new Image<Gray, byte>(N2, N1);
            for (int i = 0; i < N2; ++i) {
                for (int j = 0; j < N1; ++j) {
                    img[i, j] = new Gray(V1w[i, j] * 255);
                }
            }
            var see = new StructuringElementEx(new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }, 1, 1);
            //img = img.MorphologyEx(see, Emgu.CV.CvEnum.CV_MORPH_OP.CV_MOP_CLOSE, 1);
            img = img.Dilate(1).Erode(1);
            for (int i = 0; i < N2; ++i) {
                for (int j = 0; j < N1; ++j) {
                    V1w[i, j] = img[i, j].Intensity / 255;
                }
            }
            img.Dispose();
            #endregion
            timeused += timer.StopAndSay("图像变换和插值");
            #endregion

            //fz=find(isnan(V1w));
            //V1w(fz)=0;
            var ssd = (V2 - V1w).Each(v => v * v);//ssd=(V2-V1w).^2;			%%%%%%SSD在这里
            var ssd_global = ssd.SumAll();//ssd_global=sum(ssd(:));
            Debug("ssd_global:{0}", ssd_global);

            #region figure 5
            if (display_flag) {
                //   figure(5)
                //   subplot(2,2,1)
                //   im(V1)
                //   subplot(2,2,2)
                //   im(V2)
                //   subplot(2,2,4)
                //   im(V1w)
                //   title('V1 after warping')
                //   subplot(2,2,3)
                //   im(V2-V1w)
                //   h=title(['SSD=' num2str(ssd_global)]);
                //   colormap(cmap)
            }
            #endregion

            #region 窗口SSD比较

            timer.Restart();
            //%%%
            //%%% windowed SSD comparison
            //%%%
            var wd = 2 * w + 1;//wd=2*w+1;
            var win_fun = MatrixUtils.GaussianKernal(wd); //win_fun=gaussker(wd);
            //% extract sets of blocks around each coordinate
            //% first do 1st shape; need to use transformed coords.
            var win_list_1 = Zeros(nsamp, wd * wd);//win_list_1=zeros(nsamp,wd^2);
            for (int qq = 0; qq < nsamp; ++qq) {
                int row_qq = (int)Xk[qq, 1],//   row_qq=round(Xk(qq,2));
                    col_qq = (int)Xk[qq, 0];//   col_qq=round(Xk(qq,1));
                row_qq = Math.Max(w + 1, Math.Min(N1 - w - 1, row_qq));//   row_qq=max(w+1,min(N1-w,row_qq));
                col_qq = Math.Max(w + 1, Math.Min(N2 - w - 1, col_qq));//   col_qq=max(w+1,min(N2-w,col_qq));
                //   tmp=V1w(row_qq-w:row_qq+w,col_qq-w:col_qq+w);
                var tmp = V1w.GetSubMatrix(row_qq - w, w * 2 + 1, col_qq - w, w * 2 + 1);
                tmp = win_fun.PointMultiply(tmp);//   tmp=win_fun.*tmp;
                //   win_list_1(qq,:)=tmp(:)';
                win_list_1.SetSubMatrix(qq, 1, 0, win_list_1.Columns, tmp.Reshape(1, tmp.Rows * tmp.Columns));
            }

            //% now do 2nd shape
            var win_list_2 = Zeros(nsamp, wd * wd);//win_list_2=zeros(nsamp,wd^2);
            for (int qq = 0; qq < nsamp; ++qq) {
                int row_qq = (int)Y[qq, 1],//   row_qq = round(Y(qq, 2));
                    col_qq = (int)Y[qq, 0];//   col_qq = round(Y(qq, 1));
                row_qq = Math.Max(w + 1, Math.Min(N1 - w - 1, row_qq));//   row_qq=max(w+1,min(N1-w,row_qq));
                col_qq = Math.Max(w + 1, Math.Min(N2 - w - 1, col_qq));//   col_qq=max(w+1,min(N2-w,col_qq));
                //   tmp=V2(row_qq-w:row_qq+w,col_qq-w:col_qq+w)
                var tmp = V2.GetSubMatrix(row_qq - w, w * 2 + 1, col_qq - w, w * 2 + 1);
                tmp = win_fun.PointMultiply(tmp);//   tmp=win_fun.*tmp;
                win_list_2.SetSubMatrix(qq, 1, 0, win_list_2.Columns, tmp.Reshape(1, tmp.Rows * tmp.Columns));// win_list_2(qq,:)=tmp(:)';
            }

            var ssd_all = Dist2(win_list_1, win_list_2).Each(Math.Sqrt);//ssd_all=sqrt(dist2(win_list_1,win_list_2));
            timeused += timer.StopAndSay("窗口SSD比较");
            #endregion

            #region 最后的KNN，不知道干嘛用

            timer.Restart();
            //%%%%%%%	KNN在此
            //% loop over nearest neighbors in both directions, project in
            //% both directions, take maximum
            double cost_1 = 0, cost_2 = 0;
            //List<double> cost_1 = new List<double>(), cost_2 = new List<double>();
            for (int qq = 0; qq < nsamp; ++qq) {
                cost_1 += ssd_all[qq, min2[qq].Idx];//   cost_1=cost_1+ssd_all(qq,b2(qq));
                cost_2 += ssd_all[min1[qq].Idx, qq];//   cost_2=cost_2+ssd_all(b1(qq),qq);
                //cost_1.Add(ssd_all[qq, min2[qq].Idx]);
                //cost_2.Add(ssd_all[min1[qq].Idx, qq]);
            }
            var ssd_local = (1.0 / nsamp) * Math.Max(cost_1, cost_2);
            var ssd_local_avg = (1.0 / nsamp) * 0.5 * (cost_1 + cost_2);
            //var ssd_local = (1.0 / nsamp) * Math.Max(cost_1.Average(), cost_2.Average());//ssd_local=(1/nsamp)*max(mean(cost_1),mean(cost_2));
            //var ssd_local_avg = (1.0 / nsamp) * 0.5 * (cost_1.Average() + cost_2.Average());//ssd_local_avg=(1/nsamp)*0.5*(mean(cost_1)+mean(cost_2));
            Debug("ssd_local:{0}", ssd_local);
            Debug("ssd_local_avg:{0}", ssd_local_avg);
            timeused += timer.StopAndSay("计算ssd_local");
            #region 最后的组合图
            //if display_flag
            //%   set(h,'string',['local SSD=' num2str(ssd_local) ', avg. local SSD=' num2str(ssd_local_avg)])
            //   set(h,'string',['local SSD=' num2str(ssd_local)])
            //end
            if (display_flag) {
                Draw(V1, @"D:\Play Data\Iteration\结果\0-V1.bmp", "V1");
                Draw(V2, @"D:\Play Data\Iteration\结果\1-V2.bmp", "V2");
                Draw(V1w, @"D:\Play Data\Iteration\结果\2-V1w.bmp", "V1 after warping");
                Draw(V2 - V1w, @"D:\Play Data\Iteration\结果\3-V2-V1w.bmp",
                     String.Format("local SSD={0:F8}", ssd_local_avg));
            }
            #endregion
            #endregion

            #endregion

            var distance = 1.6 * ssd_local_avg + distance_sc + 0.3 * E; // 不知道最后的距离公式到底是什么
            Debug("distance={0:F8}", distance);
            return distance;
        }

        private List<Point> findEdge(Image<Gray, Byte> img) {
            using (var canny = img.Canny(new Gray(100), new Gray(60))) {
                List<Point> edge = new List<Point>();
                for (int i = 0; i < img.Height; ++i) {
                    for (int j = 0; j < img.Width; ++j) {
                        if (canny[i, j].Intensity > 0) {
                            edge.Add(new Point(j, i));
                        }
                    }
                }
                return edge;
            }
        }

        private void ExtractBoundary(Image<Gray, Byte> img, List<Point> edge, out Matrix xy, out Matrix V, out Matrix t) {
            int h = img.Height, w = img.Width;
            V = Zeros(h, w);
            for (int i = 0; i < h; ++i) {
                for (int j = 0; j < w; ++j) {
                    V[i, j] = img[i, j].Intensity / 255.0;
                }
            }
            Matrix G1 = V.GradientX(), G2 = V.GradientY();

            int n = edge.Count;
            xy = Zeros(n, 2);
            t = Zeros(n, 1);

            for (int i = 0; i < n; ++i) {
                int x = edge[i].X, y = edge[i].Y;
                xy[i, 0] = x;
                xy[i, 1] = y;
                t[i, 0] = Math.Atan2(G2[y, x], G1[y, x]) + 0.5 * Math.PI;
            }
        }
    }
}
