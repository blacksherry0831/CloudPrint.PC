namespace CloudPrintforPC
{
    partial class UserControlNetPrintItem
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBoxNet = new System.Windows.Forms.PictureBox();
            this.PrintDocment = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.labelPrintNow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timerUpdata = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNet)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxNet
            // 
            this.pictureBoxNet.Image = global::CloudPrintforPC.Properties.Resources.print_out;
            this.pictureBoxNet.Location = new System.Drawing.Point(4, 4);
            this.pictureBoxNet.Name = "pictureBoxNet";
            this.pictureBoxNet.Size = new System.Drawing.Size(138, 149);
            this.pictureBoxNet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxNet.TabIndex = 0;
            this.pictureBoxNet.TabStop = false;
            // 
            // PrintDocment
            // 
            this.PrintDocment.BackColor = System.Drawing.Color.Transparent;
            this.PrintDocment.BackgroundImage = global::CloudPrintforPC.Properties.Resources.button2;
            this.PrintDocment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PrintDocment.FlatAppearance.BorderSize = 0;
            this.PrintDocment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PrintDocment.Location = new System.Drawing.Point(404, 114);
            this.PrintDocment.Name = "PrintDocment";
            this.PrintDocment.Size = new System.Drawing.Size(105, 39);
            this.PrintDocment.TabIndex = 1;
            this.PrintDocment.Text = "添加文件";
            this.PrintDocment.UseVisualStyleBackColor = false;
            this.PrintDocment.Click += new System.EventHandler(this.PrintDocment_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(180, 21);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(65, 12);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "打印机名：";
            this.labelName.Click += new System.EventHandler(this.labelName_Click);
            // 
            // labelPrintNow
            // 
            this.labelPrintNow.AutoSize = true;
            this.labelPrintNow.Location = new System.Drawing.Point(180, 58);
            this.labelPrintNow.Name = "labelPrintNow";
            this.labelPrintNow.Size = new System.Drawing.Size(77, 12);
            this.labelPrintNow.TabIndex = 3;
            this.labelPrintNow.Text = "打印机状态：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "正在刷新中：";
            // 
            // timerUpdata
            // 
            this.timerUpdata.Enabled = true;
            this.timerUpdata.Interval = 500;
            this.timerUpdata.Tick += new System.EventHandler(this.timerUpdata_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(182, 130);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(122, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // UserControlNetPrintItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelPrintNow);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.PrintDocment);
            this.Controls.Add(this.pictureBoxNet);
            this.Name = "UserControlNetPrintItem";
            this.Size = new System.Drawing.Size(512, 156);
            this.Load += new System.EventHandler(this.UserControlNetPrintItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxNet;
        private System.Windows.Forms.Button PrintDocment;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelPrintNow;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timerUpdata;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
