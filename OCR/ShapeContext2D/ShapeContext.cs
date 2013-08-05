using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnAnalytics.LinearAlgebra;
using System.IO;
using System.Drawing;

namespace Jim.OCR.ShapeContext2D {
    public partial class ShapeContext {
        #region Constants

        private static readonly double Epsilon = 1e-8;

        #endregion

        #region Variables

        private Matrix origX;
        private Matrix origY;

        public int nsamp;

        //displayflag=1;
        public bool display_flag = true;
        //mean_dist_global=[]; % use [] to estimate scale from the data
        public double? mean_dist_global = null;
        //nbins_theta=12;
        public static int nbins_theta = 12;
        //nbins_r=5;
        public static int nbins_r = 5;
        //nsamp1=size(X,1);
        public int nsamp1;
        //nsamp2=size(Y,1);
        public int nsamp2;
        //ndum1=0;
        public int ndum1;
        //ndum2=0;
        public int ndum2;
        //if nsamp2>nsamp1
        //   % (as is the case in the outlier test)
        //   ndum1=ndum1+(nsamp2-nsamp1);
        //end

        //eps_dum=0.15;
        public double eps_dum = 0.15;
        //r_inner=1/8;
        public static double r_inner = 1 / 8.0;
        //r_outer=2;
        public static double r_outer = 2.0;
        //n_iter=5;
        public int n_iter = 5;
        //r=1; % annealing rate
        public double r = 1;
        //beta_init=1;  % initial regularization parameter (normalized)
        public double beta_init = 1;

        public double ndum_frac = 0.25;

        public int maxsamplecount = 100;

        private static int picWidth = 600;
        private static int picHeight = 600;
        private static int picPadding = 100;
        private static float circleR = 3;
        private static float coordPointR = 0.5f;
        public int showScale = 6;
        public double matchScale = 2.5;
        private static int skipCoords = 5;
        private static double coordMarginRate = 0.0;
        private Font titleFont = new Font("Arial", (int)(picWidth * 0.02));

        #endregion

        #region Constructors

        public ShapeContext() {
            DebugWriter = Console.Out;
        }

        public ShapeContext(double[,] x, double[,] y)
            : this() {
            nbins_theta = 12;
            nbins_r = 5;
            ndum_frac = 0.25;
            nsamp1 = x.GetLength(0);
            nsamp2 = y.GetLength(0);
            //ndum1 = (int)(nsamp1 * ndum_frac);
            //ndum2 = (int)(nsamp2 * ndum_frac);
            if (nsamp2 > nsamp1) {
                ndum1 = ndum1 + (nsamp2 - nsamp1);
            }
            eps_dum = 0.15;
            n_iter = 5;
            r = 1;
            beta_init = 1;

            origX = new DenseMatrix(x);
            origY = new DenseMatrix(y);
        }

        private string file1, file2;
        public ShapeContext(string file1, string file2)
            : this() {
            this.file1 = file1;
            this.file2 = file2;

            nbins_theta = 12;
            nbins_r = 5;
            eps_dum = 0.25;
            ndum_frac = 0.25;
            n_iter = 5;
            r = 1;
            beta_init = 1;
        }
        #endregion

        #region Debug

        public TextWriter DebugWriter { get; set; }
        public bool debug_flag = true;

        private void Debug(string fmt, params object[] objs) {
            if (debug_flag) DebugWriter.WriteLine(fmt, objs);
        }

        private readonly MyTimer timer = new MyTimer();
        public bool timer_flag {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }

        #endregion
    }
}
