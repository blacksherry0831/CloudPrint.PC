namespace CloudPrintforPC
{
    partial class FileTransferForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listViewPC = new System.Windows.Forms.ListView();
            this.listViewPhone = new System.Windows.Forms.ListView();
            this.buttonEsc = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.listViewSendFileBuffer = new System.Windows.Forms.ListView();
            this.contextMenuStripFileTransferBuffer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除选中ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除所有ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.timerListViewUpdata = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStripFileTransferBuffer.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(573, 446);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.Snow;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HotTracking = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(185, 442);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trwFileExplorer_BeforeExpand);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView1_ItemDrag);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel1.Controls.Add(this.listViewPC);
            this.splitContainer2.Panel1.Controls.Add(this.listViewPhone);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer2.Panel2.Controls.Add(this.buttonEsc);
            this.splitContainer2.Panel2.Controls.Add(this.buttonSend);
            this.splitContainer2.Panel2.Controls.Add(this.listViewSendFileBuffer);
            this.splitContainer2.Panel2.Controls.Add(this.label1);
            this.splitContainer2.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel2_Paint);
            this.splitContainer2.Size = new System.Drawing.Size(380, 446);
            this.splitContainer2.SplitterDistance = 137;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            this.splitContainer2.Resize += new System.EventHandler(this.splitContainer2_Resize);
            // 
            // listViewPC
            // 
            this.listViewPC.BackColor = System.Drawing.Color.Snow;
            this.listViewPC.CheckBoxes = true;
            this.listViewPC.Location = new System.Drawing.Point(214, 12);
            this.listViewPC.Name = "listViewPC";
            this.listViewPC.Size = new System.Drawing.Size(133, 107);
            this.listViewPC.TabIndex = 1;
            this.listViewPC.UseCompatibleStateImageBehavior = false;
            this.listViewPC.View = System.Windows.Forms.View.List;
            // 
            // listViewPhone
            // 
            this.listViewPhone.BackColor = System.Drawing.Color.Snow;
            this.listViewPhone.CheckBoxes = true;
            this.listViewPhone.Location = new System.Drawing.Point(20, 12);
            this.listViewPhone.Name = "listViewPhone";
            this.listViewPhone.Size = new System.Drawing.Size(134, 107);
            this.listViewPhone.TabIndex = 0;
            this.listViewPhone.UseCompatibleStateImageBehavior = false;
            this.listViewPhone.View = System.Windows.Forms.View.List;
            // 
            // buttonEsc
            // 
            this.buttonEsc.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonEsc.BackgroundImage = global::CloudPrintforPC.Properties.Resources.button2;
            this.buttonEsc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonEsc.FlatAppearance.BorderSize = 0;
            this.buttonEsc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEsc.Location = new System.Drawing.Point(224, 264);
            this.buttonEsc.Name = "buttonEsc";
            this.buttonEsc.Size = new System.Drawing.Size(101, 37);
            this.buttonEsc.TabIndex = 3;
            this.buttonEsc.Text = "取消";
            this.buttonEsc.UseVisualStyleBackColor = true;
            this.buttonEsc.Click += new System.EventHandler(this.buttonEsc_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonSend.BackgroundImage = global::CloudPrintforPC.Properties.Resources.button2;
            this.buttonSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSend.FlatAppearance.BorderSize = 0;
            this.buttonSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSend.Location = new System.Drawing.Point(51, 264);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(101, 37);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // listViewSendFileBuffer
            // 
            this.listViewSendFileBuffer.AllowDrop = true;
            this.listViewSendFileBuffer.BackColor = System.Drawing.Color.Snow;
            this.listViewSendFileBuffer.ContextMenuStrip = this.contextMenuStripFileTransferBuffer;
            this.listViewSendFileBuffer.Location = new System.Drawing.Point(20, 41);
            this.listViewSendFileBuffer.Name = "listViewSendFileBuffer";
            this.listViewSendFileBuffer.Size = new System.Drawing.Size(327, 207);
            this.listViewSendFileBuffer.TabIndex = 1;
            this.listViewSendFileBuffer.UseCompatibleStateImageBehavior = false;
            this.listViewSendFileBuffer.View = System.Windows.Forms.View.List;
            this.listViewSendFileBuffer.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewSendFileBuffer_ItemDrag);
            this.listViewSendFileBuffer.SelectedIndexChanged += new System.EventHandler(this.listViewSendFileBuffer_SelectedIndexChanged);
            this.listViewSendFileBuffer.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewSendFileBuffer_DragDrop);
            this.listViewSendFileBuffer.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewSendFileBuffer_DragEnter);
            this.listViewSendFileBuffer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewSendFileBuffer_MouseMove);
            // 
            // contextMenuStripFileTransferBuffer
            // 
            this.contextMenuStripFileTransferBuffer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除选中ToolStripMenuItem,
            this.删除所有ToolStripMenuItem});
            this.contextMenuStripFileTransferBuffer.Name = "contextMenuStripFileTransferBuffer";
            this.contextMenuStripFileTransferBuffer.Size = new System.Drawing.Size(125, 48);
            // 
            // 删除选中ToolStripMenuItem
            // 
            this.删除选中ToolStripMenuItem.Name = "删除选中ToolStripMenuItem";
            this.删除选中ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除选中ToolStripMenuItem.Text = "删除选中";
            this.删除选中ToolStripMenuItem.Click += new System.EventHandler(this.删除选中ToolStripMenuItem_Click);
            // 
            // 删除所有ToolStripMenuItem
            // 
            this.删除所有ToolStripMenuItem.Name = "删除所有ToolStripMenuItem";
            this.删除所有ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除所有ToolStripMenuItem.Text = "删除所有";
            this.删除所有ToolStripMenuItem.Click += new System.EventHandler(this.删除所有ToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件传输池";
            // 
            // timerListViewUpdata
            // 
            this.timerListViewUpdata.Enabled = true;
            this.timerListViewUpdata.Tick += new System.EventHandler(this.timerListViewUpdata_Tick);
            // 
            // FileTransferForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Snow;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(573, 446);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Name = "FileTransferForm";
            this.Text = "文件传输";
            this.Load += new System.EventHandler(this.FileTransferForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStripFileTransferBuffer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listViewPC;
        private System.Windows.Forms.ListView listViewPhone;
        private System.Windows.Forms.Button buttonEsc;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ListView listViewSendFileBuffer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timerListViewUpdata;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFileTransferBuffer;
        private System.Windows.Forms.ToolStripMenuItem 删除选中ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除所有ToolStripMenuItem;
    }
}