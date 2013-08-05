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
using TTimer = System.Timers.Timer;

namespace App {
    public partial class LiquidParticles : Form {
        class Mover {
            public Bgr color;
            public Color colour;
            public double y, x, vX, vY, size;
            public Mover() {
                this.color = new Bgr(rand.NextDouble() * 255, rand.NextDouble() * 255, rand.NextDouble() * 255);
                colour = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            }
        }
        public LiquidParticles() {
            InitializeComponent();
        }

        private void init() {
            canvasW = canvas.Width;
            canvasH = canvas.Height;
            img = new Image<Bgr, byte>(canvasW, canvasH);
            ctx = Graphics.FromImage(img.Bitmap);
            ctx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            ctx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }

        int canvasW = 1000;
        int canvasH = 560;
        int numMovers = 550;
        List<Mover> movers = new List<Mover>();
        double friction = .96;
        double radCirc = Math.PI * 2;

        int mouseX, mouseY, mouseVX, mouseVY, prevMouseX = 0, prevMouseY = 0;
        bool isMouseDown = false;
        Image<Bgr, Byte> img = null;
        Graphics ctx = null;

        //TTimer timer = new System.Timers.Timer(25);
        private Timer timer = null;
        private static readonly Random rand = new Random();
        private int frames = 0;
        private DateTime lasttime;

        void setup() {
            for (int i = 0; i < numMovers; ++i) {
                var m = new Mover();
                m.x = canvasW * .5;
                m.y = canvasH * .5;
                m.vX = Math.Cos(i) * rand.NextDouble() * 25;
                m.vY = Math.Sin(i) * rand.NextDouble() * 25;
                m.size = 2;
                movers.Add(m);
            }
        }

        private Bgr back = new Bgr(8, 8, 12);
        void run(object sender, EventArgs e) {
            //img.FillConvexPoly(new[]{
            //    new Point(0,0),
            //    new Point(0,canvasH),
            //    new Point(canvasW, canvasH),
            //    new Point(canvasW,0)
            //}, back);
            ctx.FillRectangle(new SolidBrush(Color.FromArgb(255/190, 8, 8, 12)), new Rectangle(new Point(0, 0), img.Size));
            mouseVX = mouseX - prevMouseX;
            mouseVY = mouseY - prevMouseY;
            prevMouseX = mouseX;
            prevMouseY = mouseY;

            var toDist = canvasW / 1.15;
            var stirDist = canvasW / 8;
            var blowDist = canvasW / 2;

            for (int i = 0; i < numMovers; ++i) {
                var m = movers[i];
                var x = m.x;
                var y = m.y;
                var vX = m.vX;
                var vY = m.vY;

                var dX = x - mouseX;
                var dY = y - mouseY;
                var d = Math.Sqrt(dX * dX + dY * dY);
                var a = Math.Atan2(dY, dX);
                var cosA = Math.Cos(a);
                var sinA = Math.Sin(a);

                if (isMouseDown) {
                    if (d < blowDist) {
                        var blowAcc = (1 - (d / blowDist)) * 14;
                        vX += cosA * blowAcc + .5 - rand.NextDouble();
                        vY += sinA * blowAcc + .5 - rand.NextDouble();
                    }
                }

                if (d < toDist) {
                    var toAcc = (1 - (d / toDist)) * canvasW * .0014;
                    vX -= cosA * toAcc;
                    vY -= sinA * toAcc;
                }

                if (d < stirDist) {
                    var mAcc = (1 - (d / stirDist)) * canvasW * .00022;
                    vX += mouseVX * mAcc;
                    vY += mouseVY * mAcc;
                }


                vX *= friction;
                vY *= friction;

                var avgVX = Math.Abs(vX);
                var avgVY = Math.Abs(vY);
                var avgV = (avgVX + avgVY) * .5;

                if (avgVX < .1) vX *= rand.NextDouble() * 3;
                if (avgVY < .1) vY *= rand.NextDouble() * 3;

                var sc = avgV * .45;
                sc = Math.Max(Math.Min(sc, 3.5), .4);


                var nextX = x + vX;
                var nextY = y + vY;

                if (nextX > canvasW) {
                    nextX = canvasW;
                    vX *= -1;
                } else if (nextX < 0) {
                    nextX = 0;
                    vX *= -1;
                }

                if (nextY > canvasH) {
                    nextY = canvasH;
                    vY *= -1;
                } else if (nextY < 0) {
                    nextY = 0;
                    vY *= -1;
                }


                m.vX = vX;
                m.vY = vY;
                m.x = nextX;
                m.y = nextY;

                //img.Draw(new CircleF(new PointF((float)nextX, (float)nextY), (float)(sc + 2)), m.color, -1);

                float fx = (float) (nextX), fy = (float) (nextY);
                ctx.FillEllipse(new SolidBrush(m.colour), fx, fy, (float)(sc+10)*2, (float)(sc+10)*2);

            }
            //img = img.SmoothGaussian(3);
            canvas.Image = img;

            ++frames;
            if (frames >= 20) {
                DateTime time = DateTime.Now;
                TimeSpan elps = time - lasttime;
                double fps = frames / elps.TotalSeconds;
                this.Text = String.Format("FPS:{0}", fps);
                lasttime = time;
                frames = 0;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e) {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void LiquidParticles_FormClosing(object sender, FormClosingEventArgs e) {
            timer.Stop();
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e) {
            isMouseDown = true;
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e) {
            isMouseDown = false;
        }

        private void LiquidParticles_Load(object sender, EventArgs e) {
            init();

            setup();

            lasttime = DateTime.Now;
            //timer = new TTimer(25);
            //timer.Elapsed += run;
            //timer.Enabled = true;
            //timer.AutoReset = true;
            //timer.Start();
            timer = new Timer();
            timer.Tick += run;
            timer.Interval = 5;
            timer.Start();
        }
    }
}
// ==========================================================================================





//// ==========================================================================================


//function rect( context , x , y , w , h ) 
//{
//    context.beginPath();
//    context.rect( x , y , w , h );
//    context.closePath();
//    context.fill();
//}


//// ==========================================================================================


//function trace( str )
//{
//    document.getElementById("output").innerHTML = str;
//}
//    }
//}
