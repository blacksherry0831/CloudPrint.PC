namespace CloudPrintforPC
{
    partial class UserControlNetPrintList
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
            this.flowLayoutPanelList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelList
            // 
            this.flowLayoutPanelList.AutoScroll = true;
            this.flowLayoutPanelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelList.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelList.Name = "flowLayoutPanelList";
            this.flowLayoutPanelList.Size = new System.Drawing.Size(465, 231);
            this.flowLayoutPanelList.TabIndex = 0;
            this.flowLayoutPanelList.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // UserControlNetPrintList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanelList);
            this.Name = "UserControlNetPrintList";
            this.Size = new System.Drawing.Size(465, 231);
            this.Load += new System.EventHandler(this.UserControlNetPrintList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelList;
    }
}
