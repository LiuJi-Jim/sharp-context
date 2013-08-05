using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;
using dnAnalytics.LinearAlgebra.Decomposition;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        /*
        public void IterMatchTPS() {
            //% script for doing shape-context based matching with alternating steps of estimating correspondences and estimating the regularized TPS transformation

            //% initialize transformed version of model pointset
            //% 初始化转换后版本的点集，这里只是复制一下，也就是第0版咯
            //Xk=X; 

            Matrix Xk = origX.Clone();
            Draw(origX, origY, null,null, @"D:\Play Data\Iteration\00000.bmp", "原始图像");
            //% initialize counter
            //% 初始化计数器
            //k=1;
            //s=1;
            int k = 0;
            //% out_vec_{1,2} are indicator vectors for keeping track of estimated outliers on each iteration
            //% out_vec_{1,2}是用来追踪每次迭代的野点的标记向量
            //out_vec_1=zeros(1,nsamp1); 
            var out_vec_1 = Utils.InitArray<bool>(nsamp1, false);
            //out_vec_2=zeros(1,nsamp2);
            var out_vec_2 = Utils.InitArray<bool>(nsamp2, false);

            double ori_weight = 0.1;

            //while s

            bool affine_start_flag = true;
            while (k < n_iter) {
                //   disp(['iter=' int2str(k)]) % 输出迭代次数
                Debug.WriteLine("Iter={0}", k);

                //   % compute shape contexts for (transformed) model
                //   [BH1,mean_dist_1]=sc_compute(Xk',zeros(1,nsamp1),mean_dist_global,nbins_theta,nbins_r,r_inner,r_outer,out_vec_1);
                double mean_dist_1;
                var BH1 = ComputeSC(Xk.Transpose(), Zeros(1, nsamp1), mean_dist_global, out mean_dist_1, out_vec_1);
                //mean_dist_global = mean_dist_1;
                //   % 计算Xk的形状上下文
                //   % 点集：Xk
                //   % tan(theta)：0数组？
                //   % 距离均值：mean_dist_global不知道定义在哪个文件里的
                //   % theta槽数：nbins_theta不知道定义在哪个文件里的
                //   % r槽数：nbins_r不知道定义在哪个文件里的
                //   % r_inner不知道定义在哪个文件里的
                //   % r_outer不知道定义在哪个文件里的
                //   % out_vec_1，上面定义了

                //   % compute shape contexts for target, using the scale estimate from the warped model
                //   % Note: this is necessary only because out_vec_2 can change on each iteration, which affects the shape contexts.  Otherwise, Y does
                //   % not change.
                //   [BH2,mean_dist_2]=sc_compute(Y',zeros(1,nsamp2),mean_dist_1,nbins_theta,nbins_r,r_inner,r_outer,out_vec_2);
                double? mean_dist_tmp = mean_dist_1;
                double mean_dist_2;
                var BH2 = ComputeSC(Y.Transpose(), Zeros(1, nsamp2), mean_dist_tmp, out mean_dist_2, out_vec_2);
                //   % 计算Y的形状上下文
                //   % 点集：Y
                //   % tan(theta)：0数组？
                //   % 距离均值：mean_dist_1，上一个形状上下文的计算结果
                //   % theta槽数：nbins_theta不知道定义在哪个文件里的
                //   % r槽数：nbins_r不知道定义在哪个文件里的
                //   % r_inner不知道定义在哪个文件里的
                //   % r_outer不知道定义在哪个文件里的
                //   % out_vec_2，上面定义了

                //   % compute regularization parameter
                //   beta_k=(mean_dist_1^2)*beta_init*r^(k-1);
                //   %计算正规化参数
                //double beta_k = mean_dist_1.Sum(dist => dist * dist) * beta_init * Math.Pow(r, k - 1);
                #region demo1
                //double beta_k = mean_dist_1 * mean_dist_1 * beta_init * Math.Pow(r, k);
                #endregion

                #region demo2
                //if affine_start_flag
                double lambda_o;
                if (affine_start_flag) {
                    //  if k==1
                    if (k == 0)
                        // % use huge regularization to get affine behavior
                        // lambda_o=1000;
                        lambda_o = 1000;
                    //  else
                    // lambda_o=beta_init*r^(k-2);	 
                    else
                        lambda_o = beta_init * Math.Pow(r, k - 1);
                    //  end
                    //else
                } else {
                    //  lambda_o=beta_init*r^(k-1);
                    lambda_o = beta_init * Math.Pow(r, k);
                    //end
                }
                //beta_k=(mean_dist_2^2)*lambda_o;
                double beta_k = mean_dist_2 * mean_dist_2 * lambda_o;
                #endregion


                //   % compute pairwise cost between all shape contexts
                //   costmat=hist_cost_2(BH1,BH2);					% 计算每一对形状上下文之间的代价，用hist_cost_2函数，不知道写在哪的
                //   % 结果costmat应该是个二维数组
                var costmat = HistCost(BH1, BH2);
                #region demo2

                #endregion

                //   % pad the cost matrix with costs for dummies
                //   % 用虚节点填充代价矩阵
                //   nptsd=nsamp1+ndum1; % ndum1不知道哪来的
                int nptsd = nsamp1 + ndum1;
                //   costmat2=eps_dum*ones(nptsd,nptsd);				% ones函数应该是生成全1的矩阵，eps_dum应该是虚节点匹配代价，不知道定义在哪的
                var costmat2 = eps_dum * Ones(nptsd, nptsd);
                //   costmat2(1:nsamp1,1:nsamp2)=costmat;				% 结合实节点与虚节点
                costmat2.SetSubMatrix(0, nsamp1, 0, nsamp2, costmat);
                //   disp('running hungarian alg.')					% 显示“运行匈牙利算法” - -||
                Debug.WriteLine("运行匈牙利算法");
                //   cvec=hungarian(costmat2);
                int[,] costmati = new int[nptsd, nptsd];
                for (int i = 0; i < nptsd; ++i) {
                    for (int j = 0; j < nptsd; ++j) {
                        costmati[i, j] = (int)(costmat2[i, j] * 1000);
                    }
                }
                var km = new Jim.OCR.Algorithms.KM(nptsd, costmati);
                //var km = new Jim.OCR.Algorithms.KMDouble(nptsd, costmat2.ToArray());
                km.Match(false);
                double matchcost = km.MatchResult / 1000.0;
                int[] cvec = km.MatchPair;
                //var cvec = km.MatchPair;

                //var cvec = Hungarian(costmat2, out matchcost);

                //   % cvec=hungarian_fast(costmat2);					% 为啥fast的反而注释掉？
                //   disp('done.');									% 显示“搞定了” - -||
                Debug.WriteLine("搞定了");

                //   % update outlier indicator vectors
                //   % 跟新野点标记向量
                //   [a,cvec2]=sort(cvec);							% 对cvec排序，a是排序结果，cvec2是a中每一项对于cvec中的索引（到这里难道cvec是个数组！？）
                int[] cvec2 = cvec.Select((v, i) => new { Val = v, Idx = i })
                                  .OrderBy(v => v.Val)
                                  .Select(v => v.Idx)
                                  .ToArray();

                //   out_vec_1=cvec2(1:nsamp1)>nsamp2;				% out_vec_1应该是cvec2中值大于nsamp2的那些
                //   out_vec_2=cvec(1:nsamp2)>nsamp1;					% out_vec_2应该是cvec从1~nsamp2的值中大于nsamp1那些
                out_vec_1 = cvec2.Take(nsamp1)
                                 .Select(v => v > nsamp2)
                                 .ToArray();
                out_vec_2 = cvec.Take(nsamp2)
                                 .Select(v => v > nsamp1)
                                 .ToArray();

                //   % format versions of Xk and Y that can be plotted with outliers'
                //   % correspondences missing
                //   X2=NaN*ones(nptsd,2);							% X2是一个nptsd*2的数组，全是NaN
                var X2 = NaNs(nptsd, 2);
                //   X2(1:nsamp1,:)=Xk;								% 猜是X2的1~nsamp1赋值为Xk
                X2.SetSubMatrix(0, nsamp1, 0, X2.Columns, Xk);

                //   X2=X2(cvec,:);									% X2中过滤出cvec？？？或者X2按cvec排序吗？
                // 看起来是将X2的所有行按cvec进行排序
                X2 = X2.SortRowsBy(cvec);
                //   X2b=NaN*ones(nptsd,2);							% X2b是nptsd*2的数组，全是NaN
                var X2b = NaNs(nptsd, 2);
                //   X2b(1:nsamp1,:)=X;								% X2b的1~nsamp1赋值为X
                X2b.SetSubMatrix(0, nsamp1, 0, X2b.Columns, X);
                //   X2b=X2b(cvec,:);									% X2b按照cvec排序吗？
                X2b = X2b.SortRowsBy(cvec);
                //   Y2=NaN*ones(nptsd,2);							% Y2是一个nptsd的数组，全是NaN
                var Y2 = NaNs(nptsd, 2);
                //   Y2(1:nsamp2,:)=Y;								% Y2的1~nsamp2赋值为Y
                Y2.SetSubMatrix(0, nsamp2, 0, Y2.Columns, Y);
                //   % 这段看不懂

                //   % extract coordinates of non-dummy correspondences and use them to estimate transformation
                //   ind_good=find(~isnan(X2b(1:nsamp1,1)));			% ind_good为找出X2b中的非(?)NaN点，最后那个参数1不知道是什么意思
                var ind_good = X2b.GetColumn(1).Take(nsamp1)
                    .Select((v, i) => new { Val = v, Idx = i })
                    .Where(v => !double.IsNaN(v.Val))
                    .Select(v => v.Idx).ToArray();
                //   % NOTE: Gianluca said he had to change nsamp1 to nsamp2 in the
                //   % preceding line to get it to work properly when nsamp1~=nsamp2 and
                //   % both sides have outliers...
                //   n_good=length(ind_good);							% n_good为非NaN点数量
                int n_good = ind_good.Length;
                Console.WriteLine("n_good:{0}", n_good);
                //   X3b=X2b(ind_good,:);								% X3b为X2b中过滤出来的非NaN点吧
                var X3b = X2b.FilterRowsBy(ind_good);
                //   Y3=Y2(ind_good,:);								% Y3为Y2中过滤出来的非NaN点吧
                var Y3 = Y2.FilterRowsBy(ind_good);

                //   if display_flag									% display_flag不知道哪定义的
                if (display_flag) {
                    //      figure(2)										% figure不知道哪定义的
                    //      plot(X2(:,1),X2(:,2),'b+',Y2(:,1),Y2(:,2),'ro') %plot函数画图的吧
                    //      hold on										% 虾米玩意>_<
                    //      h=plot([X2(:,1) Y2(:,1)]',[X2(:,2) Y2(:,2)]','k-'); % 又画图
                    //      hold off										% 虾米玩意>_<
                    //      title([int2str(n_good) ' correspondences (warped X)']) % 写标题
                    //      drawnow										% 画？！
                    //   end
                }

                //   if display_flag
                if (display_flag) {
                    //      % show the correspondences between the untransformed images
                    //      figure(3)
                    //      plot(X(:,1),X(:,2),'b+',Y(:,1),Y(:,2),'ro')
                    //      ind=cvec(ind_good);
                    //      hold on
                    //      plot([X2b(:,1) Y2(:,1)]',[X2b(:,2) Y2(:,2)]','k-')
                    //      hold off
                    //      title([int2str(n_good) ' correspondences (unwarped X)'])
                    //      drawnow	
                    //   end
                }

                //   % estimate regularized TPS transformation
                //   [cx,cy,E]=bookstein(X3b,Y3,beta_k);				% bookstein好像是个样条插值函数O_O，有个bookstein.m
                Matrix cx = null, cy = null;
                double E = 0;
                Matrix L = null;
                Bookstein(X3b, Y3, beta_k, ref cx, ref cy, ref E, ref L);

                //   % calculate affine cost							% 计算仿射代价
                var A = MatrixUtils.RankHorizon(
                    cx.GetSubMatrix(n_good + 1, 2, 0, 1), cy.GetSubMatrix(n_good + 1, 2, 0, 1)
                );//A=[cx(n_good+2:n_good+3,:) cy(n_good+2:n_good+3,:)];
                var s = new Svd(A, true).S(); // s=svd(A);
                var aff_cost = Math.Log(s[0] / s[1]); // aff_cost=log(s(1)/s(2));

                //   % calculate shape context cost					% 计算形状上下文代价
                var min1 = costmat.GetColumns().Select((col, i) => new { Val = col.Min(), Idx = i }).ToArray();// [a1,b1]=min(costmat,[],1);
                var min2 = costmat.GetRows().Select((row, i) => new { Val = row.Min(), Idx = i }).ToArray(); // [a2,b2]=min(costmat,[],2);}
                var sc_cost = Math.Max(min1.Average(a => a.Val), min2.Average(a => a.Val)); // sc_cost=max(mean(a1),mean(a2));

                //   % warp each coordinate

                // d2=max(dist2(X3b,X),0);
                var d2 = Dist2(X3b, X).Each(v => v > 0 ? v : 0);
                // U=d2.*log(d2+eps);
                var U = d2.PointMultiply(d2.Each(v => Math.Log(v + Epsilon)));
                var axt = cx.GetSubMatrix(n_good, 3, 0, 1).Transpose(); // axt是cx中的最后三个，即a的x分量
                var wxt = cx.GetSubMatrix(0, n_good, 0, 1).Transpose(); // wxt是cs中的前n个，即w的x分量
                var ayt = cy.GetSubMatrix(n_good, 3, 0, 1).Transpose();
                var wyt = cx.GetSubMatrix(0, n_good, 0, 1).Transpose();
                Matrix fx = null, fy = null;
                Transformation(X, U, axt, wxt, ayt, wyt, out fx, out fy);

                //   Z=[fx; fy]';
                Matrix Z = MatrixUtils.RankHorizon(fx, fy).Transpose();

                //   % compute the mean squared error between synthetic warped image
                //   % and estimated warped image (using ground-truth correspondences
                //   % on TPS transformed image) 
                //   mse2=mean((y2(:,1)-Z(:,1)).^2+(y2(:,2)-Z(:,2)).^2); % 搞毛，这么复杂
                var yz1 = Y2.GetColumn(0) - Z.GetColumn(0);
                for (int i = 0; i < yz1.Count; ++i) yz1[i] *= yz1[i];
                var yz2 = Y2.GetColumn(1) - Z.GetColumn(1);
                for (int i = 0; i < yz2.Count; ++i) yz2[i] *= yz2[i];
                var mse2 = (yz1 + yz2).Average();

                //   % Chui actually does mean of non-squared distance
                //   mse1=mean(sqrt((y2(:,1)-Z(:,1)).^2+(y2(:,2)-Z(:,2)).^2)); % 搞毛，again
                var mse1 = (yz1 + yz2).Average(v => Math.Sqrt(v));

                //   disp(['error = ' num2str(mse1)])					% 这个是输出，我知道
                Debug.WriteLine("Error={0}", mse1);

                //Ztan=[fx; fy]';
                //tk=atan2(Ztan(:,2)-Z(:,2),Ztan(:,1)-Z(:,1));

                //   if display_flag									% 又画画
                if (display_flag) {
                    Draw(X, Y, cvec,null, String.Format(@"D:\Play Data\Iteration\Iter-{0}-0.bmp", k),
                        String.Format("第{0}次迭代的匹配图像\n匹配代价{1}", k, matchcost));
                    //Draw(Z, Y, null, String.Format(@"D:\Play Data\Iteration\Iter-{0}-1.bmp", k),
                    //    String.Format("第{0}次迭代的变换图像\n错误指数{1}", k, mse1));
                    //      figure(4)
                    //      plot(Z(:,1),Z(:,2),'b+',Y(:,1),Y(:,2),'ro');
                    //      title(['recovered TPS transformation (k=' int2str(k) ', \lambda_o=' num2str(beta_init*r^(k-1)) ', I_f=' num2str(E) ', error=' num2str(mse1) ')']) 
                    //      % show warped coordinate grid
                    //      fx_aff=cx(n_good+1:n_good+3)'*[ones(1,M); x'; y'];
                    //      d2=dist2(X3b,[x y]);
                    //      fx_wrp=cx(1:n_good)'*(d2.*log(d2+eps));
                    //      fx=fx_aff+fx_wrp;
                    //      fy_aff=cy(n_good+1:n_good+3)'*[ones(1,M); x'; y'];
                    //      fy_wrp=cy(1:n_good)'*(d2.*log(d2+eps));
                    //      fy=fy_aff+fy_wrp;
                    //      hold on
                    //      plot(fx,fy,'k.','markersize',1)
                    //      hold off
                    //      drawnow
                    //   end
                }

                //   % update Xk for the next iteration
                //   Xk=Z;											% 更新Xk，进行下一次迭代
                Xk = Z;

                ++k;
                //   % stop early if shape context score is sufficiently low
                //   if k==n_iter
                //      s=0;
                //   else
                //      k=k+1;
                //   end
                //end
            }
        }
*/
    }
}
