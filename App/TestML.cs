using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using System.Diagnostics;
using Jim.OCR.Algorithms;
using Jim.OCR;

namespace App {
    public partial class TestML : Form {
        public TestML() {
            InitializeComponent();
        }

        private void TestML_Load(object sender, EventArgs e) {
        }

        private Image<Bgr, Byte> kmeans() {
            int trainSampleCount = 1500;
            int sigma = 60;
            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 3, 1);
            trainData1.GetCols(0, 1).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));
            trainData1.GetCols(1, 2).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));

            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainData2.SetRandNormal(new MCvScalar(400), new MCvScalar(sigma));

            Matrix<float> trainData3 = trainData.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainData3.GetCols(0, 1).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));
            trainData3.GetCols(1, 2).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));

            PointF[] points = new PointF[trainSampleCount];
            for (int i = 0; i < points.Length; ++i) {
                points[i] = new PointF(trainData[i, 0], trainData[i, 1]);
            }
            var km = new KMeans<PointF>(points, 3,
                (a, b) => ((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)),
                list => new PointF(list.Average(p => p.X), list.Average(p => p.Y))
            );
            int it = 0;
            MyTimer timer = new MyTimer();
            timer.Restart();
            //var cluster = km.Cluster();
            var cluster = km.AnnealCluster(
                (a, b) => new PointF(a.X + b.X, a.Y + b.Y),
                (a, b) => new PointF(a.X - b.X, a.Y - b.Y),
                (p, v) => new PointF((float)(p.X / v), (float)(p.Y / v)),
                out it);
            var time = timer.Stop();
            this.Text = String.Format("n={0}, k={1}, time={2}ms, iter={3}.", trainSampleCount, 3, time, it);

            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);
            for (int y = 0; y < 500; ++y) {
                for (int x = 0; x < 500; ++x) {
                    double d0 = (x - cluster[0].Center.X) * (x - cluster[0].Center.X)
                              + (y - cluster[0].Center.Y) * (y - cluster[0].Center.Y);
                    double d1 = (x - cluster[1].Center.X) * (x - cluster[1].Center.X)
                              + (y - cluster[1].Center.Y) * (y - cluster[1].Center.Y);
                    double d2 = (x - cluster[2].Center.X) * (x - cluster[2].Center.X)
                              + (y - cluster[2].Center.Y) * (y - cluster[2].Center.Y);
                    Bgr color = new Bgr(0, 0, 0);
                    if (d0 < d1 && d0 < d2) {
                        color = new Bgr(20, 0, 0);
                    }
                    if (d1 < d0 && d1 < d2) {
                        color = new Bgr(0, 20, 0);
                    }
                    if (d2 < d0 && d2 < d1) {
                        color = new Bgr(0, 0, 20);
                    }
                    img[y, x] = color;
                }
            }
            Bgr[] colors = new[] { new Bgr(128, 0, 0), new Bgr(0, 128, 0), new Bgr(0, 0, 128) };
            Bgr[] centers = new[] { new Bgr(255, 0, 0), new Bgr(0, 255, 0), new Bgr(0, 0, 255) };
            for (int i = 0; i < 3; ++i) {
                foreach (var p in cluster[i]) {
                    img.Draw(new CircleF(p, 2), colors[i], 1);
                }
                img.Draw(new CircleF(cluster[i].Center, 5), centers[i], 3);
            }
            img.Draw(new CircleF(new PointF(100, 300), sigma), new Bgr(128, 128, 128), 2);
            img.Draw(new CircleF(new PointF(100, 300), 3), new Bgr(128, 128, 128), 2);
            img.Draw(new CircleF(new PointF(300, 100), sigma), new Bgr(128, 128, 128), 2);
            img.Draw(new CircleF(new PointF(300, 100), 3), new Bgr(128, 128, 128), 2);
            img.Draw(new CircleF(new PointF(400, 400), sigma), new Bgr(128, 128, 128), 2);
            img.Draw(new CircleF(new PointF(400, 400), 3), new Bgr(128, 128, 128), 2);

            return img;
        }

        private Image<Bgr, Byte> svm() {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            int trainSampleCount = 150;
            int sigma = 60;

            #region Generate the training data and classes

            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
            Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

            Matrix<float> sample = new Matrix<float>(1, 2);

            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 3, 1);
            trainData1.GetCols(0, 1).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));
            trainData1.GetCols(1, 2).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));

            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainData2.SetRandNormal(new MCvScalar(400), new MCvScalar(sigma));

            Matrix<float> trainData3 = trainData.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainData3.GetCols(0, 1).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));
            trainData3.GetCols(1, 2).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));

            Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount / 3, 1);
            trainClasses1.SetValue(1);
            Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainClasses2.SetValue(2);
            Matrix<float> trainClasses3 = trainClasses.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainClasses3.SetValue(3);

            #endregion

            timer.Stop();
            MessageBox.Show("生成" + timer.ElapsedMilliseconds + "ms");
            timer.Reset();
            timer.Start();

            using (SVM model = new SVM()) {
                SVMParams p = new SVMParams();
                p.KernelType = Emgu.CV.ML.MlEnum.SVM_KERNEL_TYPE.LINEAR;
                p.SVMType = Emgu.CV.ML.MlEnum.SVM_TYPE.C_SVC;
                p.C = 1;
                p.TermCrit = new MCvTermCriteria(100, 0.00001);

                //model.Load(@"D:\Play Data\训练数据");
                //bool trained = model.Train(trainData, trainClasses, null, null, p);
                bool trained = model.TrainAuto(trainData, trainClasses, null, null, p.MCvSVMParams, 5);
                timer.Stop();
                MessageBox.Show("训练" + timer.ElapsedMilliseconds + "ms");
                timer.Reset();
                timer.Start();

                for (int i = 0; i < img.Height; i++) {
                    for (int j = 0; j < img.Width; j++) {
                        sample.Data[0, 0] = j;
                        sample.Data[0, 1] = i;

                        //float response = model.Predict(sample);

                        //img[i, j] =
                        //   response == 1 ? new Bgr(90, 0, 0) :
                        //   response == 2 ? new Bgr(0, 90, 0) :
                        //   new Bgr(0, 0, 90);
                    }
                }
                //model.Save(@"D:\Play Data\训练数据");

                timer.Stop();
                MessageBox.Show("染色" + timer.ElapsedMilliseconds + "ms");
                timer.Reset();
                timer.Start();
                int c = model.GetSupportVectorCount();
                for (int i = 0; i < c; i++) {
                    float[] v = model.GetSupportVector(i);
                    PointF p1 = new PointF(v[0], v[1]);
                    img.Draw(new CircleF(p1, 4), new Bgr(128, 128, 128), 2);
                }
                timer.Stop();
                MessageBox.Show("画圈" + timer.ElapsedMilliseconds + "ms");
                timer.Reset();
                timer.Start();
            }

            // display the original training samples
            for (int i = 0; i < (trainSampleCount / 3); i++) {
                PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                img.Draw(new CircleF(p1, 2.0f), new Bgr(255, 100, 100), -1);
                PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2.0f), new Bgr(100, 255, 100), -1);
                PointF p3 = new PointF(trainData3[i, 0], trainData3[i, 1]);
                img.Draw(new CircleF(p3, 2.0f), new Bgr(100, 100, 255), -1);
            }
            timer.Stop();
            MessageBox.Show("标点" + timer.ElapsedMilliseconds + "ms");
            timer.Reset();
            timer.Start();

            return img;
        }

        private Image<Bgr, Byte> nn() {
            int trainSampleCount = 100;

            #region Generate the traning data and classes
            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
            Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

            Matrix<float> sample = new Matrix<float>(1, 2);
            Matrix<float> prediction = new Matrix<float>(1, 1);

            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount >> 1, 1);
            trainData1.SetRandNormal(new MCvScalar(200), new MCvScalar(50));
            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount >> 1, trainSampleCount, 1);
            trainData2.SetRandNormal(new MCvScalar(300), new MCvScalar(50));

            Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount >> 1, 1);
            trainClasses1.SetValue(1);
            Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount >> 1, trainSampleCount, 1);
            trainClasses2.SetValue(2);
            #endregion

            Matrix<int> layerSize = new Matrix<int>(new int[] { 2, 5, 1 });

            MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams();
            parameters.term_crit = new MCvTermCriteria(10, 1.0e-8);
            parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP;
            parameters.bp_dw_scale = 0.1;
            parameters.bp_moment_scale = 0.1;

            using (ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 1.0, 1.0)) {
                network.Train(trainData, trainClasses, null, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);

                for (int i = 0; i < img.Height; i++) {
                    for (int j = 0; j < img.Width; j++) {
                        sample.Data[0, 0] = j;
                        sample.Data[0, 1] = i;
                        network.Predict(sample, prediction);

                        // estimates the response and get the neighbors' labels
                        float response = prediction.Data[0, 0];

                        // highlight the pixel depending on the accuracy (or confidence)
                        img[i, j] = response < 1.5 ? new Bgr(90, 0, 0) : new Bgr(0, 90, 0);
                    }
                }
            }

            // display the original training samples
            for (int i = 0; i < (trainSampleCount >> 1); i++) {
                PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                img.Draw(new CircleF(p1, 2), new Bgr(255, 100, 100), -1);
                PointF p2 = new PointF((int)trainData2[i, 0], (int)trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2), new Bgr(100, 255, 100), -1);
            }

            return img;
        }

        private Image<Bgr, Byte> knn() {
            int K = 10;
            int trainSampleCount = 150;
            int sigma = 60;

            #region Generate the training data and classes

            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
            Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

            Matrix<float> sample = new Matrix<float>(1, 2);

            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 3, 1);
            trainData1.GetCols(0, 1).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));
            trainData1.GetCols(1, 2).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));

            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainData2.SetRandNormal(new MCvScalar(400), new MCvScalar(sigma));

            Matrix<float> trainData3 = trainData.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainData3.GetCols(0, 1).SetRandNormal(new MCvScalar(300), new MCvScalar(sigma));
            trainData3.GetCols(1, 2).SetRandNormal(new MCvScalar(100), new MCvScalar(sigma));

            Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount / 3, 1);
            trainClasses1.SetValue(1);
            Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainClasses2.SetValue(2);
            Matrix<float> trainClasses3 = trainClasses.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainClasses3.SetValue(3);

            #endregion

            Matrix<float> results, neighborResponses;
            results = new Matrix<float>(sample.Rows, 1);
            neighborResponses = new Matrix<float>(sample.Rows, K);
            //dist = new Matrix<float>(sample.Rows, K);

            //using (KNearest knn = new KNearest(trainData, trainClasses, null, false, K)) {
            using (KNearest knn = new KNearest()) {
                bool trained = knn.Train(trainData, trainClasses, null, false, K, false);

                for (int i = 0; i < img.Height; i++) {
                    for (int j = 0; j < img.Width; j++) {
                        sample.Data[0, 0] = j;
                        sample.Data[0, 1] = i;

                        //Matrix<float> nearestNeighbors = new Matrix<float>(K* sample.Rows, sample.Cols);
                        // estimates the response and get the neighbors' labels
                        float response = knn.FindNearest(sample, K, results, null, neighborResponses, null);

                        int accuracy = 0;
                        // compute the number of neighbors representing the majority
                        for (int k = 0; k < K; k++) {
                            if (neighborResponses.Data[0, k] == response)
                                accuracy++;
                        }
                        // highlight the pixel depending on the accuracy (or confidence)
                        //img[i, j] =
                        //response == 1 ?
                        //    (accuracy > 5 ? new Bgr(90, 0, 0) : new Bgr(90, 60, 0)) :
                        //    (accuracy > 5 ? new Bgr(0, 90, 0) : new Bgr(60, 90, 0));
                        img[i, j] =
                            response == 1 ? (accuracy > 5 ? new Bgr(90, 0, 0) : new Bgr(90, 30, 30)) :
                           response == 2 ? (accuracy > 5 ? new Bgr(0, 90, 0) : new Bgr(30, 90, 30)) :
                            (accuracy > 5 ? new Bgr(0, 0, 90) : new Bgr(30, 30, 90));
                    }
                }
                knn.Save(@"D:\Play Data\KNN训练数据");
            }

            // display the original training samples

            for (int i = 0; i < (trainSampleCount / 3); i++) {
                PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                img.Draw(new CircleF(p1, 2.0f), new Bgr(255, 100, 100), -1);
                PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2.0f), new Bgr(100, 255, 100), -1);
                PointF p3 = new PointF(trainData3[i, 0], trainData3[i, 1]);
                img.Draw(new CircleF(p3, 2.0f), new Bgr(100, 100, 255), -1);
            }
            return img;
        }

        private Image<Bgr, Byte> bayes() {
            Bgr[] colors = new Bgr[] { 
   new Bgr(0, 0, 255), 
   new Bgr(0, 255, 0),
   new Bgr(255, 0, 0)};
            int trainSampleCount = 150;

            #region Generate the traning data and classes
            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
            Matrix<int> trainClasses = new Matrix<int>(trainSampleCount, 1);

            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

            Matrix<float> sample = new Matrix<float>(1, 2);

            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 3, 1);
            trainData1.GetCols(0, 1).SetRandNormal(new MCvScalar(100), new MCvScalar(50));
            trainData1.GetCols(1, 2).SetRandNormal(new MCvScalar(300), new MCvScalar(50));

            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainData2.SetRandNormal(new MCvScalar(400), new MCvScalar(50));

            Matrix<float> trainData3 = trainData.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainData3.GetCols(0, 1).SetRandNormal(new MCvScalar(300), new MCvScalar(50));
            trainData3.GetCols(1, 2).SetRandNormal(new MCvScalar(100), new MCvScalar(50));

            Matrix<int> trainClasses1 = trainClasses.GetRows(0, trainSampleCount / 3, 1);
            trainClasses1.SetValue(1);
            Matrix<int> trainClasses2 = trainClasses.GetRows(trainSampleCount / 3, 2 * trainSampleCount / 3, 1);
            trainClasses2.SetValue(2);
            Matrix<int> trainClasses3 = trainClasses.GetRows(2 * trainSampleCount / 3, trainSampleCount, 1);
            trainClasses3.SetValue(3);
            #endregion

            using (NormalBayesClassifier classifier = new NormalBayesClassifier()) {
                classifier.Train(trainData, trainClasses, null, null, false);
                #region Classify every image pixel
                for (int i = 0; i < img.Height; i++)
                    for (int j = 0; j < img.Width; j++) {
                        sample.Data[0, 0] = i;
                        sample.Data[0, 1] = j;
                        int response = (int)classifier.Predict(sample, null);

                        Bgr color = colors[response - 1];

                        img[j, i] = new Bgr(color.Blue * 0.5, color.Green * 0.5, color.Red * 0.5);
                    }
                #endregion
            }

            // display the original training samples
            for (int i = 0; i < (trainSampleCount / 3); i++) {
                PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                img.Draw(new CircleF(p1, 2.0f), colors[0], -1);
                PointF p2 = new PointF(trainData2[i, 0], trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2.0f), colors[1], -1);
                PointF p3 = new PointF(trainData3[i, 0], trainData3[i, 1]);
                img.Draw(new CircleF(p3, 2.0f), colors[2], -1);
            }

            return img;
        }

        private void button4_Click(object sender, EventArgs e) {
            imageBox1.Image = bayes();
        }

        private void button3_Click(object sender, EventArgs e) {
            imageBox1.Image = svm();
        }

        private void button2_Click(object sender, EventArgs e) {
            imageBox1.Image = knn();
        }

        private void button1_Click(object sender, EventArgs e) {
            imageBox1.Image = nn();
        }

        private void button5_Click(object sender, EventArgs e) {
            imageBox1.Image = kmeans();
        }
    }
}
