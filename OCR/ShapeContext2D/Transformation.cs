using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        private Matrix Transformation(Matrix A, Matrix U, Matrix axt, Matrix wxt, Matrix ayt, Matrix wyt) {
            var fx_aff = axt * MatrixUtils.RankVertical(Ones(1, A.Rows), A.Transpose());
            var fx_wrp = wxt * U;
            var fx = fx_aff + fx_wrp; //fx=fx_aff+fx_wrp;

            // fy_aff=cy(n_good+1:n_good+3)'*[ones(1,nsamp1); X'];
            var fy_aff = ayt * MatrixUtils.RankVertical(Ones(1, A.Rows), A.Transpose());
            var fy_wrp = wyt * U; // fy_wrp=cy(1:n_good)'*U;
            var fy = fy_aff + fy_wrp; // fy=fy_aff+fy_wrp;

            return MatrixUtils.RankVertical(fx, fy).Transpose();
        }
    }
}