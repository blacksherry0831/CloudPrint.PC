namespace CloudPrintforPC
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label_printer_num = new System.Windows.Forms.Label();
            this.label_phone_num = new System.Windows.Forms.Label();
            this.pictureBox_Linked = new System.Windows.Forms.PictureBox();
            this.pictureBox_Circle = new System.Windows.Forms.PictureBox();
            this.timerUpdataBG = new System.Windows.Forms.Timer(this.components);
            this.timerHideView = new System.Windows.Forms.Timer(this.components);
            this.timer_updata_view_content = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon_N = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Linked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Circle)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.CloseToolStripMenuItem.Text = "关闭";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::CloudPrintforPC.Properties.Resources.print_out;
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(59, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::CloudPrintforPC.Properties.Resources.phone;
            this.pictureBox2.Location = new System.Drawing.Point(59, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // label_printer_num
            // 
            this.label_printer_num.AutoSize = true;
            this.label_printer_num.BackColor = System.Drawing.Color.Transparent;
            this.label_printer_num.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Bold);
            this.label_printer_num.Location = new System.Drawing.Point(86, 15);
            this.label_printer_num.Name = "label_printer_num";
            this.label_printer_num.Size = new System.Drawing.Size(11, 10);
            this.label_printer_num.TabIndex = 4;
            this.label_printer_num.Text = "1";
            this.label_printer_num.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.label_printer_num.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.label_printer_num.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // label_phone_num
            // 
            this.label_phone_num.AutoSize = true;
            this.label_phone_num.BackColor = System.Drawing.Color.Transparent;
            this.label_phone_num.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Bold);
            this.label_phone_num.Location = new System.Drawing.Point(86, 33);
            this.label_phone_num.Name = "label_phone_num";
            this.label_phone_num.Size = new System.Drawing.Size(11, 10);
            this.label_phone_num.TabIndex = 5;
            this.label_phone_num.Text = "1";
            this.label_phone_num.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.label_phone_num.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.label_phone_num.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // pictureBox_Linked
            // 
            this.pictureBox_Linked.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Linked.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_Linked.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Linked.Image")));
            this.pictureBox_Linked.Location = new System.Drawing.Point(16, 16);
            this.pictureBox_Linked.Name = "pictureBox_Linked";
            this.pictureBox_Linked.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_Linked.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Linked.TabIndex = 6;
            this.pictureBox_Linked.TabStop = false;
            this.pictureBox_Linked.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.pictureBox_Linked.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.pictureBox_Linked.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // pictureBox_Circle
            // 
            this.pictureBox_Circle.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Circle.Image = global::CloudPrintforPC.Properties.Resources.button_neg;
            this.pictureBox_Circle.ImageLocation = "";
            this.pictureBox_Circle.Location = new System.Drawing.Point(106, 20);
            this.pictureBox_Circle.Name = "pictureBox_Circle";
            this.pictureBox_Circle.Size = new System.Drawing.Size(18, 18);
            this.pictureBox_Circle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Circle.TabIndex = 7;
            this.pictureBox_Circle.TabStop = false;
            this.pictureBox_Circle.Click += new System.EventHandler(this.mButtonExtra_Click);
            this.pictureBox_Circle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.pictureBox_Circle.MouseEnter += new System.EventHandler(this.pictureBox_Circle_MouseEnter);
            this.pictureBox_Circle.MouseLeave += new System.EventHandler(this.pictureBox_Circle_MouseLeave);
            this.pictureBox_Circle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.pictureBox_Circle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            // 
            // timerUpdataBG
            // 
            this.timerUpdataBG.Enabled = true;
            this.timerUpdataBG.Interval = 200;
            this.timerUpdataBG.Tick += new System.EventHandler(this.timerUpdataBG_Tick);
            // 
            // timerHideView
            // 
            this.timerHideView.Enabled = true;
            this.timerHideView.Interval = 20;
            this.timerHideView.Tick += new System.EventHandler(this.timerHideView_Tick);
            // 
            // timer_updata_view_content
            // 
            this.timer_updata_view_content.Enabled = true;
            this.timer_updata_view_content.Interval = 500;
            this.timer_updata_view_content.Tick += new System.EventHandler(this.timer_updata_view_content_Tick);
            // 
            // notifyIcon_N
            // 
            this.notifyIcon_N.BalloonTipText = "XXX";
            this.notifyIcon_N.BalloonTipTitle = "XXX";
            this.notifyIcon_N.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon_N.Icon")));
            this.notifyIcon_N.Text = "云打印";
            this.notifyIcon_N.Visible = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(218)))), ((int)(((byte)(221)))));
            this.BackgroundImage = global::CloudPrintforPC.Properties.Resources.main_frame_another;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(136, 56);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label_phone_num);
            this.Controls.Add(this.label_printer_num);
            this.Controls.Add(this.pictureBox_Circle);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox_Linked);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(218)))), ((int)(((byte)(221)))));
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseDown);
            this.MouseEnter += new System.EventHandler(this.frmTopMost_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.frmTopMost_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmTopMost_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Linked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Circle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label_printer_num;
        private System.Windows.Forms.Label label_phone_num;
        private System.Windows.Forms.PictureBox pictureBox_Linked;
        private System.Windows.Forms.PictureBox pictureBox_Circle;
        private System.Windows.Forms.Timer timerUpdataBG;
        private System.Windows.Forms.Timer timerHideView;
        private System.Windows.Forms.Timer timer_updata_view_content;
        private System.Windows.Forms.NotifyIcon notifyIcon_N;

    }
}

