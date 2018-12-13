namespace CloudPrintforPC
{
    partial class UserControlPhoneItem
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
            this.pictureBoxImg = new System.Windows.Forms.PictureBox();
            this.labelPhoneName = new System.Windows.Forms.Label();
            this.labelPhoneNumber = new System.Windows.Forms.Label();
            this.labelPrintDocNum = new System.Windows.Forms.Label();
            this.labelPaperNum = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImg)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxImg
            // 
            this.pictureBoxImg.Image = global::CloudPrintforPC.Properties.Resources.phone;
            this.pictureBoxImg.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxImg.Name = "pictureBoxImg";
            this.pictureBoxImg.Size = new System.Drawing.Size(109, 145);
            this.pictureBoxImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxImg.TabIndex = 0;
            this.pictureBoxImg.TabStop = false;
            this.pictureBoxImg.Click += new System.EventHandler(this.pictureBoxImg_Click);
            // 
            // labelPhoneName
            // 
            this.labelPhoneName.AutoSize = true;
            this.labelPhoneName.Location = new System.Drawing.Point(133, 12);
            this.labelPhoneName.Name = "labelPhoneName";
            this.labelPhoneName.Size = new System.Drawing.Size(53, 12);
            this.labelPhoneName.TabIndex = 1;
            this.labelPhoneName.Text = "手机名：";
            this.labelPhoneName.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelPhoneNumber
            // 
            this.labelPhoneNumber.AutoSize = true;
            this.labelPhoneNumber.Location = new System.Drawing.Point(133, 46);
            this.labelPhoneNumber.Name = "labelPhoneNumber";
            this.labelPhoneNumber.Size = new System.Drawing.Size(53, 12);
            this.labelPhoneNumber.TabIndex = 2;
            this.labelPhoneNumber.Text = "手机号：";
            // 
            // labelPrintDocNum
            // 
            this.labelPrintDocNum.AutoSize = true;
            this.labelPrintDocNum.Location = new System.Drawing.Point(133, 80);
            this.labelPrintDocNum.Name = "labelPrintDocNum";
            this.labelPrintDocNum.Size = new System.Drawing.Size(77, 12);
            this.labelPrintDocNum.TabIndex = 3;
            this.labelPrintDocNum.Text = "打印文件数：";
            // 
            // labelPaperNum
            // 
            this.labelPaperNum.AutoSize = true;
            this.labelPaperNum.Location = new System.Drawing.Point(133, 114);
            this.labelPaperNum.Name = "labelPaperNum";
            this.labelPaperNum.Size = new System.Drawing.Size(77, 12);
            this.labelPaperNum.TabIndex = 4;
            this.labelPaperNum.Text = "打印纸张数：";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.BackgroundImage = global::CloudPrintforPC.Properties.Resources.button2;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(374, 46);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 37);
            this.button1.TabIndex = 5;
            this.button1.Text = "允许打印";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // UserControlPhoneItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.pictureBoxImg);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelPaperNum);
            this.Controls.Add(this.labelPrintDocNum);
            this.Controls.Add(this.labelPhoneNumber);
            this.Controls.Add(this.labelPhoneName);
            this.Name = "UserControlPhoneItem";
            this.Size = new System.Drawing.Size(483, 151);
            this.Load += new System.EventHandler(this.UserControlPhoneItem_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.UserControlPhoneItem_Layout);
            this.Resize += new System.EventHandler(this.UserControlPhoneItem_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxImg;
        private System.Windows.Forms.Label labelPhoneName;
        private System.Windows.Forms.Label labelPhoneNumber;
        private System.Windows.Forms.Label labelPrintDocNum;
        private System.Windows.Forms.Label labelPaperNum;
        private System.Windows.Forms.Button button1;
    }
}
