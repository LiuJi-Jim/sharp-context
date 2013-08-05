using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        public Matrix Interpolation(Matrix X, Matrix Y, Matrix Z) {
            Matrix V = Zeros(Z.Rows, Z.Columns);
            for (int i = 0; i < Z.Rows; ++i) {
                for (int j = 0; j < Z.Rows; ++j) {
                    double x = X[i, j], y = Y[i, j], z = Z[i, j]; // 变换后的横坐标、纵坐标、映射的值
                    if (x < 0 || y < 0) continue;
                    double dx = x - (int)x, dy = y - (int)y;
                    int l = (int)Math.Floor(x), t = (int)Math.Floor(y),
                        r = l + 1, b = t + 1;
                    if (0 <= l && l < Z.Columns) {
                        if (0 <= t && t < Z.Rows) V[t, l] += z * dx * dy;
                        if (0 <= b && b < Z.Rows) V[b, l] += z * dx * (1 - dy);
                    }
                    if (0 <= r && r < Z.Columns) {
                        if (0 <= t && t < Z.Rows) V[t, r] += z * (1 - dx) * dy;
                        if (0 <= b && b < Z.Rows) V[b, r] += z * (1 - dx) * (1 - dy);
                    }
                }
            }
            return V;
        }
    }
}
