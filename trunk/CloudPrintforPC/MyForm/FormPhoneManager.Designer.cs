namespace CloudPrintforPC
{
    partial class FormPhoneManager
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
            this.timerUpdataList = new System.Windows.Forms.Timer(this.components);
            this.mListView = new CloudPrintforPC.UserControlPhoneList();
            this.SuspendLayout();
            // 
            // timerUpdataList
            // 
            this.timerUpdataList.Enabled = true;
            this.timerUpdataList.Tick += new System.EventHandler(this.timerUpdataList_Tick);
            // 
            // mListView
            // 
            this.mListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mListView.Location = new System.Drawing.Point(0, 0);
            this.mListView.Name = "mListView";
            this.mListView.Size = new System.Drawing.Size(284, 262);
            this.mListView.TabIndex = 0;
            this.mListView.Load += new System.EventHandler(this.mListView_Load);
            this.mListView.Resize += new System.EventHandler(this.mListView_Resize);
            // 
            // FormPhoneManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.mListView);
            this.Name = "FormPhoneManager";
            this.Text = "手机管理";
            this.Load += new System.EventHandler(this.FormPhoneManager_Load);
            this.ResumeLayout(false);

        }

        #endregion

      
        private UserControlPhoneList mListView;
        private System.Windows.Forms.Timer timerUpdataList;

    }
}