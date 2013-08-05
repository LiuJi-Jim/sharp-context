namespace App {
    partial class Demo {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.打开文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开模板ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.撤销ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.二值化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.掩模平滑慢ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中值滤波ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.消除独立连通区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.分割ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.划分连通区域ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.识别ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.representativeShapeContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shapemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofd_Image = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.imageBox3 = new Emgu.CV.UI.ImageBox();
            this.imageBox2 = new Emgu.CV.UI.ImageBox();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.fbd_SC = new System.Windows.Forms.FolderBrowserDialog();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开文件ToolStripMenuItem,
            this.打开模板ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.分割ToolStripMenuItem,
            this.识别ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(677, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 打开文件ToolStripMenuItem
            // 
            this.打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem";
            this.打开文件ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.打开文件ToolStripMenuItem.Text = "打开文件";
            this.打开文件ToolStripMenuItem.Click += new System.EventHandler(this.打开文件ToolStripMenuItem_Click);
            // 
            // 打开模板ToolStripMenuItem
            // 
            this.打开模板ToolStripMenuItem.Name = "打开模板ToolStripMenuItem";
            this.打开模板ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.打开模板ToolStripMenuItem.Text = "打开模板";
            this.打开模板ToolStripMenuItem.Click += new System.EventHandler(this.打开模板ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.撤销ToolStripMenuItem,
            this.二值化ToolStripMenuItem,
            this.掩模平滑慢ToolStripMenuItem,
            this.中值滤波ToolStripMenuItem,
            this.消除独立连通区域ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(50, 21);
            this.toolStripMenuItem1.Text = "处理↓";
            // 
            // 撤销ToolStripMenuItem
            // 
            this.撤销ToolStripMenuItem.Name = "撤销ToolStripMenuItem";
            this.撤销ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.撤销ToolStripMenuItem.Text = "撤销";
            this.撤销ToolStripMenuItem.Click += new System.EventHandler(this.撤销ToolStripMenuItem_Click);
            // 
            // 二值化ToolStripMenuItem
            // 
            this.二值化ToolStripMenuItem.Name = "二值化ToolStripMenuItem";
            this.二值化ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.二值化ToolStripMenuItem.Text = "二值化";
            this.二值化ToolStripMenuItem.Click += new System.EventHandler(this.二值化ToolStripMenuItem_Click);
            // 
            // 掩模平滑慢ToolStripMenuItem
            // 
            this.掩模平滑慢ToolStripMenuItem.Name = "掩模平滑慢ToolStripMenuItem";
            this.掩模平滑慢ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.掩模平滑慢ToolStripMenuItem.Text = "掩模平滑（慢）";
            this.掩模平滑慢ToolStripMenuItem.Click += new System.EventHandler(this.掩模平滑慢ToolStripMenuItem_Click);
            // 
            // 中值滤波ToolStripMenuItem
            // 
            this.中值滤波ToolStripMenuItem.Name = "中值滤波ToolStripMenuItem";
            this.中值滤波ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.中值滤波ToolStripMenuItem.Text = "中值滤波";
            this.中值滤波ToolStripMenuItem.Click += new System.EventHandler(this.中值滤波ToolStripMenuItem_Click);
            // 
            // 消除独立连通区域ToolStripMenuItem
            // 
            this.消除独立连通区域ToolStripMenuItem.Name = "消除独立连通区域ToolStripMenuItem";
            this.消除独立连通区域ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.消除独立连通区域ToolStripMenuItem.Text = "消除独立连通区域";
            this.消除独立连通区域ToolStripMenuItem.Click += new System.EventHandler(this.消除独立连通区域ToolStripMenuItem_Click);
            // 
            // 分割ToolStripMenuItem
            // 
            this.分割ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.划分连通区域ToolStripMenuItem});
            this.分割ToolStripMenuItem.Name = "分割ToolStripMenuItem";
            this.分割ToolStripMenuItem.Size = new System.Drawing.Size(50, 21);
            this.分割ToolStripMenuItem.Text = "分割↓";
            // 
            // 划分连通区域ToolStripMenuItem
            // 
            this.划分连通区域ToolStripMenuItem.Name = "划分连通区域ToolStripMenuItem";
            this.划分连通区域ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.划分连通区域ToolStripMenuItem.Text = "划分连通区域";
            this.划分连通区域ToolStripMenuItem.Click += new System.EventHandler(this.划分连通区域ToolStripMenuItem_Click);
            // 
            // 识别ToolStripMenuItem
            // 
            this.识别ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.representativeShapeContextToolStripMenuItem,
            this.shapemeToolStripMenuItem});
            this.识别ToolStripMenuItem.Name = "识别ToolStripMenuItem";
            this.识别ToolStripMenuItem.Size = new System.Drawing.Size(50, 21);
            this.识别ToolStripMenuItem.Text = "识别↓";
            // 
            // representativeShapeContextToolStripMenuItem
            // 
            this.representativeShapeContextToolStripMenuItem.Name = "representativeShapeContextToolStripMenuItem";
            this.representativeShapeContextToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.representativeShapeContextToolStripMenuItem.Text = "Representative Shape Context";
            this.representativeShapeContextToolStripMenuItem.Click += new System.EventHandler(this.representativeShapeContextToolStripMenuItem_Click);
            // 
            // shapemeToolStripMenuItem
            // 
            this.shapemeToolStripMenuItem.Name = "shapemeToolStripMenuItem";
            this.shapemeToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.shapemeToolStripMenuItem.Text = "Shapeme";
            this.shapemeToolStripMenuItem.Click += new System.EventHandler(this.shapemeToolStripMenuItem_Click);
            // 
            // ofd_Image
            // 
            this.ofd_Image.FileName = "openFileDialog1";
            this.ofd_Image.FileOk += new System.ComponentModel.CancelEventHandler(this.ofd_Image_FileOk);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.imageBox3);
            this.splitContainer1.Panel1.Controls.Add(this.imageBox2);
            this.splitContainer1.Panel1.Controls.Add(this.imageBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(677, 456);
            this.splitContainer1.SplitterDistance = 80;
            this.splitContainer1.TabIndex = 1;
            // 
            // imageBox3
            // 
            this.imageBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageBox3.Location = new System.Drawing.Point(150, 0);
            this.imageBox3.Name = "imageBox3";
            this.imageBox3.Size = new System.Drawing.Size(75, 80);
            this.imageBox3.TabIndex = 4;
            this.imageBox3.TabStop = false;
            // 
            // imageBox2
            // 
            this.imageBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageBox2.Location = new System.Drawing.Point(75, 0);
            this.imageBox2.Name = "imageBox2";
            this.imageBox2.Size = new System.Drawing.Size(75, 80);
            this.imageBox2.TabIndex = 3;
            this.imageBox2.TabStop = false;
            // 
            // imageBox1
            // 
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageBox1.Location = new System.Drawing.Point(0, 0);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(75, 80);
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.Location = new System.Drawing.Point(75, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(602, 350);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(75, 350);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress,
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 350);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(677, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progress
            // 
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(100, 16);
            this.progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(52, 17);
            this.status.Text = "Waiting";
            // 
            // Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 481);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Demo";
            this.Text = "Demo";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开文件ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofd_Image;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Emgu.CV.UI.ImageBox imageBox3;
        private Emgu.CV.UI.ImageBox imageBox2;
        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.ToolStripMenuItem 打开模板ToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog fbd_SC;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progress;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 撤销ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 二值化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 掩模平滑慢ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中值滤波ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分割ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 划分连通区域ToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripMenuItem 识别ToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripMenuItem 消除独立连通区域ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem representativeShapeContextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shapemeToolStripMenuItem;
    }
}