namespace PrintJobMonitor
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonShowJobs = new System.Windows.Forms.Button();
            this.buttonShowWordPages = new System.Windows.Forms.Button();
            this.buttonShowPrinter = new System.Windows.Forms.Button();
            this.buttonDelJob = new System.Windows.Forms.Button();
            this.buttonShowPdfPages = new System.Windows.Forms.Button();
            this.openFileDialogPDF = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogWord = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(449, 229);
            this.textBox1.TabIndex = 0;
            // 
            // buttonShowJobs
            // 
            this.buttonShowJobs.Location = new System.Drawing.Point(35, 254);
            this.buttonShowJobs.Name = "buttonShowJobs";
            this.buttonShowJobs.Size = new System.Drawing.Size(98, 43);
            this.buttonShowJobs.TabIndex = 1;
            this.buttonShowJobs.Text = "显示打印Job";
            this.buttonShowJobs.UseVisualStyleBackColor = true;
            this.buttonShowJobs.Click += new System.EventHandler(this.buttonShowJobs_Click);
            // 
            // buttonShowWordPages
            // 
            this.buttonShowWordPages.Location = new System.Drawing.Point(35, 325);
            this.buttonShowWordPages.Name = "buttonShowWordPages";
            this.buttonShowWordPages.Size = new System.Drawing.Size(98, 43);
            this.buttonShowWordPages.TabIndex = 2;
            this.buttonShowWordPages.Text = "显示Word页数";
            this.buttonShowWordPages.UseVisualStyleBackColor = true;
            this.buttonShowWordPages.Click += new System.EventHandler(this.buttonShowPages_Click);
            // 
            // buttonShowPrinter
            // 
            this.buttonShowPrinter.Location = new System.Drawing.Point(336, 254);
            this.buttonShowPrinter.Name = "buttonShowPrinter";
            this.buttonShowPrinter.Size = new System.Drawing.Size(98, 43);
            this.buttonShowPrinter.TabIndex = 3;
            this.buttonShowPrinter.Text = "显示打印机";
            this.buttonShowPrinter.UseVisualStyleBackColor = true;
            this.buttonShowPrinter.Click += new System.EventHandler(this.buttonShowPrinter_Click);
            // 
            // buttonDelJob
            // 
            this.buttonDelJob.Location = new System.Drawing.Point(181, 254);
            this.buttonDelJob.Name = "buttonDelJob";
            this.buttonDelJob.Size = new System.Drawing.Size(98, 43);
            this.buttonDelJob.TabIndex = 4;
            this.buttonDelJob.Text = "删除错误Job";
            this.buttonDelJob.UseVisualStyleBackColor = true;
            this.buttonDelJob.Click += new System.EventHandler(this.buttonDelJob_Click);
            // 
            // buttonShowPdfPages
            // 
            this.buttonShowPdfPages.Location = new System.Drawing.Point(336, 325);
            this.buttonShowPdfPages.Name = "buttonShowPdfPages";
            this.buttonShowPdfPages.Size = new System.Drawing.Size(98, 43);
            this.buttonShowPdfPages.TabIndex = 5;
            this.buttonShowPdfPages.Text = "显示PDF页数";
            this.buttonShowPdfPages.UseVisualStyleBackColor = true;
            this.buttonShowPdfPages.Click += new System.EventHandler(this.buttonShowPdfPages_Click);
            // 
            // openFileDialogPDF
            // 
            this.openFileDialogPDF.Filter = "PDF文档 (*.pdf)|*.pdf";
            // 
            // openFileDialogWord
            // 
            this.openFileDialogWord.Filter = "Word文档 (*.doc;*.docx)|*.doc;*.docx";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(473, 392);
            this.Controls.Add(this.buttonShowPdfPages);
            this.Controls.Add(this.buttonDelJob);
            this.Controls.Add(this.buttonShowPrinter);
            this.Controls.Add(this.buttonShowWordPages);
            this.Controls.Add(this.buttonShowJobs);
            this.Controls.Add(this.textBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印机Job监控";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonShowJobs;
        private System.Windows.Forms.Button buttonShowWordPages;
        private System.Windows.Forms.Button buttonShowPrinter;
        private System.Windows.Forms.Button buttonDelJob;
        private System.Windows.Forms.Button buttonShowPdfPages;
        private System.Windows.Forms.OpenFileDialog openFileDialogPDF;
        private System.Windows.Forms.OpenFileDialog openFileDialogWord;
    }
}

