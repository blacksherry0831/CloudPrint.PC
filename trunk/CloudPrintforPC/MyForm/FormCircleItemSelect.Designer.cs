namespace CloudPrintforPC
{
    partial class FormCircleItemSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timerHideView = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerHideView
            // 
            this.timerHideView.Enabled = true;
            this.timerHideView.Interval = 20;
            this.timerHideView.Tick += new System.EventHandler(this.timerHideView_Tick);
            // 
            // FormCircleItemSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(183)))), ((int)(((byte)(27)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(260, 260);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCircleItemSelect";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormCircleItemSelect";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(183)))), ((int)(((byte)(27)))));
            this.Load += new System.EventHandler(this.FormCircleItemSelect_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCircleItemSelect_KeyDown);
            this.ResumeLayout(false);

        }　

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timerHideView;
    }
}