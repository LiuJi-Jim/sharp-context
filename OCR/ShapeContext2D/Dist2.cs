using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        /*
         * DIST2	Calculates squared distance between two sets of points.
         * 
         * Description
         * D = DIST2(X, C) takes two matrices of vectors and calculates the
         * squared Euclidean distance between them.  Both matrices must be of
         * the same column dimension.  If X has M rows and N columns, and C has
         * L rows and N columns, then the result has M rows and L columns.  The
         * I, Jth entry is the  squared distance from the Ith row of X to the
         * Jth row of C.
         * 
         * See also
         * GMMACTIV, KMEANS, RBFFWD
         * 
         * 
         * Copyright (c) Christopher M Bishop, Ian T Nabney (1996, 1997)
         *
         */
        /// <summary>
        /// TODO: 还没验证对不对
        /// </summary>
        public Matrix Dist2(Matrix A, Matrix B) {
            int arows = A.Rows, acols = A.Columns,
                brows = B.Rows, bcols = B.Columns;
            if (acols != bcols)
                throw new Exception("Data dimension does not match dimension of centres");

            //var n2 = (ones(ncentres, 1) * sum((x.^2)', 1))' + ...
            //          ones(ndata, 1) * sum((c.^2)',1) - ...
            //          2.*(x*(c'))

            DenseMatrix result = new DenseMatrix(arows, brows);//Zeros(arows, brows);

            for (int i = 0; i < arows; ++i) {
                var arowi = A.GetRow(i);
                for (int j = 0; j < brows; ++j) {
                    //result[i, j] = x.GetRow(i).Sum(xi => xi * xi)
                    //             + c.GetRow(j).Sum(ci => ci * ci)
                    //             - 2 * x.GetRow(i).Select((xi, jj) => xi * c.GetRow(j)[jj]).Sum();
                    var browj = B.GetRow(j);
                    double rij = 0;
                    for (int k = 0; k < acols; ++k) {
                        //double a = A[i, k], b = B[j, k];
                        //double a = arowi[k], b = browj[k];
                        //result[i, j] += (a * a + b * b - 2 * a * b);
                        //rij += (a * a + b * b - 2 * a * b);

                        double d = arowi[k] - browj[k];
                        rij += (d*d);
                    }
                    result[i, j] = rij;

                    // result[i,j] = (Xi^2+Xi^2) + (Xj^2+Xj^2) - 2(Xi*Xj + Xi*Xj)
                    // = (Xi-Xj)^2 + (Yi-Yj)^2
                    //double Xi = x[i, 0], Yi = x[i, 1], Xj = c[j, 0], Yj = c[j, 1];
                    //result[i, j] = (Xi - Xj) * (Xi - Xj) + (Yi - Yj) * (Yi - Yj);
                }
            }
            return result;
        }
    }
}
