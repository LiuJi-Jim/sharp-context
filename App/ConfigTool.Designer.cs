namespace App {
    partial class ConfigTool {
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
            this.tab_Preview = new System.Windows.Forms.TabPage();
            this.previewer1 = new App.Controls.Previewer();
            this.tab_Split = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.radioButton_NoSplit = new System.Windows.Forms.RadioButton();
            this.radioButton_Split = new System.Windows.Forms.RadioButton();
            this.tab_PreProcess = new System.Windows.Forms.TabPage();
            this.imageProcessorConfig1 = new App.Controls.ImageProcessorConfig();
            this.tab_File = new System.Windows.Forms.TabPage();
            this.button_OpenTestImage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_TestImage = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_Preview.SuspendLayout();
            this.tab_Split.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tab_PreProcess.SuspendLayout();
            this.tab_File.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab_Preview
            // 
            this.tab_Preview.Controls.Add(this.previewer1);
            this.tab_Preview.Location = new System.Drawing.Point(4, 26);
            this.tab_Preview.Name = "tab_Preview";
            this.tab_Preview.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Preview.Size = new System.Drawing.Size(711, 321);
            this.tab_Preview.TabIndex = 4;
            this.tab_Preview.Text = "预览";
            this.tab_Preview.UseVisualStyleBackColor = true;
            // 
            // previewer1
            // 
            this.previewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewer1.Location = new System.Drawing.Point(3, 3);
            this.previewer1.Name = "previewer1";
            this.previewer1.Preview = null;
            this.previewer1.Results = null;
            this.previewer1.Size = new System.Drawing.Size(705, 315);
            this.previewer1.Source = null;
            this.previewer1.TabIndex = 0;
            // 
            // tab_Split
            // 
            this.tab_Split.Controls.Add(this.splitContainer1);
            this.tab_Split.Location = new System.Drawing.Point(4, 26);
            this.tab_Split.Name = "tab_Split";
            this.tab_Split.Padding = new System.Windows.Forms.Padding(3);
            this.tab_Split.Size = new System.Drawing.Size(711, 321);
            this.tab_Split.TabIndex = 1;
            this.tab_Split.Text = "分割&识别";
            this.tab_Split.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.radioButton_NoSplit);
            this.splitContainer1.Panel1.Controls.Add(this.radioButton_Split);
            this.splitContainer1.Size = new System.Drawing.Size(705, 315);
            this.splitContainer1.SplitterDistance = 116;
            this.splitContainer1.TabIndex = 0;
            // 
            // radioButton_NoSplit
            // 
            this.radioButton_NoSplit.AutoSize = true;
            this.radioButton_NoSplit.Location = new System.Drawing.Point(5, 25);
            this.radioButton_NoSplit.Name = "radioButton_NoSplit";
            this.radioButton_NoSplit.Size = new System.Drawing.Size(107, 16);
            this.radioButton_NoSplit.TabIndex = 0;
            this.radioButton_NoSplit.Text = "不分割直接识别";
            this.radioButton_NoSplit.UseVisualStyleBackColor = true;
            this.radioButton_NoSplit.CheckedChanged += new System.EventHandler(this.SplitTypeChanged);
            // 
            // radioButton_Split
            // 
            this.radioButton_Split.AutoSize = true;
            this.radioButton_Split.Checked = true;
            this.radioButton_Split.Location = new System.Drawing.Point(5, 3);
            this.radioButton_Split.Name = "radioButton_Split";
            this.radioButton_Split.Size = new System.Drawing.Size(107, 16);
            this.radioButton_Split.TabIndex = 0;
            this.radioButton_Split.TabStop = true;
            this.radioButton_Split.Text = "分割后逐个识别";
            this.radioButton_Split.UseVisualStyleBackColor = true;
            this.radioButton_Split.CheckedChanged += new System.EventHandler(this.SplitTypeChanged);
            // 
            // tab_PreProcess
            // 
            this.tab_PreProcess.Controls.Add(this.imageProcessorConfig1);
            this.tab_PreProcess.Location = new System.Drawing.Point(4, 26);
            this.tab_PreProcess.Name = "tab_PreProcess";
            this.tab_PreProcess.Padding = new System.Windows.Forms.Padding(3);
            this.tab_PreProcess.Size = new System.Drawing.Size(711, 321);
            this.tab_PreProcess.TabIndex = 0;
            this.tab_PreProcess.Text = "预处理";
            this.tab_PreProcess.UseVisualStyleBackColor = true;
            // 
            // imageProcessorConfig1
            // 
            this.imageProcessorConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageProcessorConfig1.Location = new System.Drawing.Point(3, 3);
            this.imageProcessorConfig1.Name = "imageProcessorConfig1";
            this.imageProcessorConfig1.Size = new System.Drawing.Size(705, 315);
            this.imageProcessorConfig1.TabIndex = 0;
            // 
            // tab_File
            // 
            this.tab_File.Controls.Add(this.button_OpenTestImage);
            this.tab_File.Controls.Add(this.label1);
            this.tab_File.Controls.Add(this.textBox_TestImage);
            this.tab_File.Location = new System.Drawing.Point(4, 26);
            this.tab_File.Name = "tab_File";
            this.tab_File.Size = new System.Drawing.Size(711, 321);
            this.tab_File.TabIndex = 2;
            this.tab_File.Text = "文件";
            this.tab_File.UseVisualStyleBackColor = true;
            // 
            // button_OpenTestImage
            // 
            this.button_OpenTestImage.Location = new System.Drawing.Point(397, 1);
            this.button_OpenTestImage.Name = "button_OpenTestImage";
            this.button_OpenTestImage.Size = new System.Drawing.Size(75, 23);
            this.button_OpenTestImage.TabIndex = 2;
            this.button_OpenTestImage.Text = "打开";
            this.button_OpenTestImage.UseVisualStyleBackColor = true;
            this.button_OpenTestImage.Click += new System.EventHandler(this.button_OpenTestImage_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "测试图片";
            // 
            // textBox_TestImage
            // 
            this.textBox_TestImage.Location = new System.Drawing.Point(67, 3);
            this.textBox_TestImage.Name = "textBox_TestImage";
            this.textBox_TestImage.Size = new System.Drawing.Size(323, 21);
            this.textBox_TestImage.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_File);
            this.tabControl1.Controls.Add(this.tab_PreProcess);
            this.tabControl1.Controls.Add(this.tab_Split);
            this.tabControl1.Controls.Add(this.tab_Preview);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(48, 22);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(719, 351);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // ConfigTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 351);
            this.Controls.Add(this.tabControl1);
            this.Name = "ConfigTool";
            this.Text = "ConfigTool";
            this.Load += new System.EventHandler(this.ConfigTool_Load);
            this.tab_Preview.ResumeLayout(false);
            this.tab_Split.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tab_PreProcess.ResumeLayout(false);
            this.tab_File.ResumeLayout(false);
            this.tab_File.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tab_Preview;
        private App.Controls.Previewer previewer1;
        private System.Windows.Forms.TabPage tab_Split;
        private System.Windows.Forms.TabPage tab_PreProcess;
        private App.Controls.ImageProcessorConfig imageProcessorConfig1;
        private System.Windows.Forms.TabPage tab_File;
        private System.Windows.Forms.Button button_OpenTestImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_TestImage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton radioButton_NoSplit;
        private System.Windows.Forms.RadioButton radioButton_Split;



    }
}