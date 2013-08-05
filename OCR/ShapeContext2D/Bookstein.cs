using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        //function [cx,cy,E,L]=bookstein(X,Y,beta_k);
        public void Bookstein(Matrix X, Matrix Y, double? beta_k,
            ref Matrix cx, ref Matrix cy, ref double E) {
            //% [cx,cy,E,L]=bookstein(X,Y,beta_k);
            //%
            //% Bookstein PAMI89

            //N=size(X,1);
            int N = X.Rows;
            //Nb=size(Y,1);
            int Nb = Y.Rows;

            //if N~=Nb
            //   error('number of landmarks must be equal')
            //end
            if (N != Nb) {
                throw new Exception("标记数必须相等");
            }

            //% compute distances between left points
            //r2=dist2(X,X);								% dist2函数定义在dist.m里吧
            var r2 = Dist2(X, X);

            //K=r2.*log(r2+eye(N,N)); % add identity matrix to make K zero on the diagonal
            //% 据说eye函数产生n*n的单位矩阵
            var K = r2.PointMultiply((r2 + Eye(N, N)).Each(Math.Log));

            //P=[ones(N,1) X];							% Matlab还能这样表示矩阵……内牛满面
            var P = new DenseMatrix(N, 1 + X.Columns);
            P.SetSubMatrix(0, N, 0, 1, Ones(N, 1));
            P.SetSubMatrix(0, N, 1, X.Columns, X);

            //L=[K  P
            //   P' zeros(3,3)];							% 内牛满面again
            var Pt = P.Transpose();
            var L = new DenseMatrix(K.Rows + Pt.Rows, K.Columns + P.Columns);
            L.SetSubMatrix(0, K.Rows, 0, K.Columns, K); // 左上角K
            L.SetSubMatrix(K.Rows, Pt.Rows, 0, Pt.Columns, Pt); // 左下角Pt
            L.SetSubMatrix(0, P.Rows, K.Columns, P.Columns, P); // 右上角P
            L.SetSubMatrix(K.Rows, 3, K.Columns, 3, Zeros(3, 3)); // 右下角Zeros

            //V=[Y' zeros(2,3)];
            var Yt = Y.Transpose();
            var V = new DenseMatrix(Yt.Rows, Yt.Columns + 3);
            V.SetSubMatrix(0, Yt.Rows, 0, Yt.Columns, Yt);
            V.SetSubMatrix(0, Yt.Rows, Yt.Columns, 3, Zeros(2, 3));

            //if nargin>2									% nargin哪来的，晕
            if (beta_k != null) {
                //   % regularization
                //   L(1:N,1:N)=L(1:N,1:N)+beta_k*eye(N,N);	% 算算算
                L.SetSubMatrix(0, N, 0, N, L.GetSubMatrix(0, N, 0, N) + beta_k.Value * Eye(N, N));
                //end
            }
            //invL=inv(L);								% 求逆。。。
            var invL = L.Inverse();

            //c=invL*V';									% 矩阵乘法
            var c = invL * V.Transpose();

            cx = c.GetSubMatrix(0, c.Rows, 0, 1);//cx=c(:,1);
            cy = c.GetSubMatrix(0, c.Rows, 1, 1);//cy=c(:,2);

            //if nargout>2								% nargout哪来的？晕死
            //   % compute bending energy (w/o regularization) % 传说中的弯曲能量？
            //   Q=c(1:N,:)'*K*c(1:N,:);					% 看不懂看不懂看不懂
            var c1n = c.GetSubMatrix(0, N, 0, c.Columns);
            var Q = c1n.Transpose() * K * c1n;
            //   E=mean(diag(Q));							% 对角线？的均值
            E = Q.GetDiagonal().Average();
            //end
        }
    }
}
