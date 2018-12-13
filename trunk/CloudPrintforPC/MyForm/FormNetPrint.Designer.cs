namespace CloudPrintforPC
{
    partial class FormNetPrint
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.mNetPrintList = new CloudPrintforPC.UserControlNetPrintList();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mNetPrintList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 279);
            this.panel1.TabIndex = 1;
            // 
            // mNetPrintList
            // 
            this.mNetPrintList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.mNetPrintList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.mNetPrintList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mNetPrintList.Location = new System.Drawing.Point(0, 0);
            this.mNetPrintList.Name = "mNetPrintList";
            this.mNetPrintList.Size = new System.Drawing.Size(572, 279);
            this.mNetPrintList.TabIndex = 0;
            this.mNetPrintList.Load += new System.EventHandler(this.mNetPrintList_Load);
            // 
            // FormNetPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(572, 279);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "FormNetPrint";
            this.Text = "打印机管理";
            this.Load += new System.EventHandler(this.FormNetPrint_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlNetPrintList mNetPrintList;
        private System.Windows.Forms.Panel panel1;

    }
}