using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using  FileExplorer_TreeView_Cui;
using System.Threading;
using CloudPrintLib;
namespace CloudPrintforPC
{
    public partial class FileTransferForm : Form
    {
        public delegate void UiInvoke(Object o);
        private NetFindfTransfer mNetInfo;
        private ServerCloudPrint mServer;
        FileExplorer fe = new FileExplorer();
        private TreeNodeFile mSelectedTreeNode;
        public String mCurrentListID = null;
        public FileTransferForm()
        {
            InitializeComponent();
            this.Width += 1;
            this.Height += 1;
        }

        private void FileTransferForm_Load(object sender, EventArgs e)
        {
            fe.CreateTreeCui(this.treeView1);
            this.InitFileTransferBufferListView();
            this.InitListViewPC();
            this.InitListViewPhone();
        }
        private void InitFileTransferBufferListView()
        {
            //this.listViewSendFileBuffer;
            this.listViewSendFileBuffer.SmallImageList = fe.GetImageList();
            this.listViewSendFileBuffer.LargeImageList = fe.GetImageList();
        }
        private void InitListViewPC() 
        {
            if (!this.mNetInfo.mPcPhoneList.PhonePcListID.Equals(this.mCurrentListID) || this.listViewPC.Items.Count==0)
            {
                this.listViewPC.Clear();
                this.listViewPC.SmallImageList = this.mNetInfo.ImageIcon;
                this.listViewPC.LargeImageList = this.mNetInfo.ImageIcon;
                ServerInfo[] array = this.mNetInfo.mPcPhoneList.GetPClistArray();
                for (int i = 0; i < array.Length; i++)
                {

                    ListViewItemServer lvi = new ListViewItemServer();
                    lvi.Text = array[i].GetServerDes_v2();
                    lvi.ImageKey = array[i].GetImageKey();
                    lvi.mServerInfo = array[i];
                    this.listViewPC.Items.Add(lvi);

                }

              

            }
         
            
        }
        private void InitListViewPhone() 
        {
            //已存在就更新
            //不存在则添加
#if true
            if (!this.mNetInfo.mPcPhoneList.PhonePcListID.Equals(this.mCurrentListID) || this.listViewPhone.Items.Count == 0)
            {
                this.listViewPhone.Clear();
                this.listViewPhone.SmallImageList = this.mNetInfo.ImageIcon;
                this.listViewPhone.LargeImageList = this.mNetInfo.ImageIcon;
                ServerInfo[] array = this.mNetInfo.mPcPhoneList.GetPhonelistArray();
                for (int i = 0; i < array.Length; i++)
                {

                    ListViewItemServer lvi = new ListViewItemServer();
                    lvi.Text = array[i].GetServerDes_v2();
                    lvi.ImageKey = array[i].GetImageKey();
                    lvi.mServerInfo = array[i];
                    this.listViewPhone.Items.Add(lvi);

                }
               
            }
           
#else
           


#endif
            //this.listViewPhone.SmallImageList = this.mNetInfo.ImageIcon;
            //this.listViewPhone.LargeImageList = this.mNetInfo.ImageIcon;
            //ServerInfo[] arraylist = this.mNetInfo.mPcPhoneList.GetPhonelistArray();
            //this.listViewPhone.Items.ContainsKey
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void treeViewDirectory_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void explorerTreeView1_Load(object sender, EventArgs e)
        {

        }

        private void explorerTreeView1_Click(object sender, EventArgs e)
        {
            ;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           // ;
           // e.
            this.mSelectedTreeNode =(TreeNodeFile) e.Node;

        }
        private void trwFileExplorer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text == "")
            {
                TreeNode node = fe.EnumerateDirectoryCui((TreeNodeFile)(e.Node));
            }


        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer2_Resize(object sender, EventArgs e)
        {
            //容器改变大小时候
            SplitContainer split = (SplitContainer)sender;
            //split.Panel1.Width;
            //split.Panel1.Height;
          //  split.Panel1.BackColor = Color.Red;
            this.ResizePhonePcLayout();
            this.ResizeFileTransferLayout();

        }
        public void ResizePhonePcLayout()
        {
            int space = 4;
            int width = this.splitContainer2.Panel1.Width;
            int height = this.splitContainer2.Panel1.Height;

            this.listViewPhone.Width = width / 2 - space * 2;
            this.listViewPhone.Height = height - space * 2;
            this.listViewPhone.Location = new Point(space, space);

            this.listViewPC.Location = new Point(width / 2+space, space);
            this.listViewPC.Width = width / 2 - space * 2;
            this.listViewPC.Height = height - space * 2;


        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.ResizePhonePcLayout();
            this.ResizeFileTransferLayout();
        }
        public void ResizeFileTransferLayout()
        {
            int space=4;
            int width = this.splitContainer2.Panel2.Width;
            int height = this.splitContainer2.Panel2.Height;
            this.label1.Location = new Point((width-this.label1.Width)/2,4);

            this.listViewSendFileBuffer.Location = new Point(space, this.label1.Height + 8);
            this.listViewSendFileBuffer.Width = width - space * 2;
            this.listViewSendFileBuffer.Height = height - this.label1.Height - this.buttonSend.Height - 20;



        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            Button button_t = (Button)sender;
           
            SendFilePackage sfPackage = new SendFilePackage();
            foreach (ListViewItem lvi in this.listViewPhone.Items)  //选中项遍历
            {
                if(lvi.Checked)
                    sfPackage.ServerInfoSet.Add(((ListViewItemServer)lvi).mServerInfo);
            }
            foreach (ListViewItem lvi in this.listViewPC.Items)  //选中项遍历
            {
                if (lvi.Checked)
                    sfPackage.ServerInfoSet.Add(((ListViewItemServer)lvi).mServerInfo);
            }

            if (sfPackage.ServerInfoSet.Count == 0)
            {
                MessageBox.Show("没有选中的服务器");
                return;
            }
            foreach (ListViewItem lvi in this.listViewSendFileBuffer.Items)  //选中项遍历
            {
                sfPackage.PathSet.Add(((ListViewItemFile)lvi).mPath);
            }
            if (sfPackage.PathSet.Count == 0)
            {
                MessageBox.Show("没有选中文件");
                return;
            }
            button_t.Enabled = false;
            this.Invalidate();
            Thread t = new Thread(this.ThreadFileSendCheak);
            t.Start(sfPackage);
           
           
          
        }

        private void buttonEsc_Click(object sender, EventArgs e)
        {

        }
        public void ThreadFileSendCheak(Object o)
        {
           SendFilePackage  sfPackage=( SendFilePackage)o;
            if(sfPackage!=null){
                for(int i=0;i<sfPackage.ServerInfoSet.Count;i++)
                {
                    for(int j=0;j<sfPackage.PathSet.Count;j++){
                         sfPackage.ServerInfoSet[i].SendFileOrDirectory(sfPackage.PathSet[j]);

                         while (!sfPackage.ServerInfoSet[i].IsSendFileComplete());
                         String SendDes ="主机："+ sfPackage.ServerInfoSet[i].mHostname +"文件："+ sfPackage.PathSet[j];
                         if (sfPackage.ServerInfoSet[i].mFileSendSuccess) {
                             sfPackage.SendSuccess.Add(SendDes);
                         }
                         else if (sfPackage.ServerInfoSet[i].mFileSendAbort){
                             sfPackage.SendAbort.Add(SendDes);
                         }
                         else if (sfPackage.ServerInfoSet[i].mFileSendEsc){
                             sfPackage.SendEsc.Add(SendDes);
                         }
                    }
                     
                }                
            }

            this.BeginInvoke(new UiInvoke(this.SendFileSuccess), new object[] { sfPackage });
           
        }
        public void SendFileSuccess(Object o)
        {

            SendFilePackage sfPackage = (SendFilePackage)o;
            //this.Text = "文件传输------执行完成";
            MessageBox.Show(sfPackage.GetSendInfo());
            this.SendButtonEnable();
          
        }
        public void SendButtonEnable()
        {
            this.buttonSend.Enabled = true;
        }
        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            this.DoDragDrop(this.mSelectedTreeNode, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                   this.mSelectedTreeNode = (TreeNodeFile) this.treeView1.GetNodeAt(e.X, e.Y);
                   this.treeView1.SelectedNode = this.mSelectedTreeNode;
                }
            }
            catch { } 
        }

        private void listViewSendFileBuffer_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                TreeNodeFile node = (TreeNodeFile)e.Data.GetData(typeof(TreeNodeFile));
                if (node != null)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                    Cursor = Cursors.No;
            }
            catch { }
            finally { Cursor = Cursors.Default; } 
        }

        private void listViewSendFileBuffer_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.mSelectedTreeNode != null & (e.Button & MouseButtons.Left) == MouseButtons.Left)
                {
                    DragDropEffects dropEffect = this.listViewSendFileBuffer .DoDragDrop(this.mSelectedTreeNode, DragDropEffects.Move);
                }
            }
            catch { } 
        }

        private void listViewSendFileBuffer_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                TreeNodeFile node = (TreeNodeFile)e.Data.GetData(typeof(TreeNodeFile));
                if (node != null) {
                    ListViewItemFile lvi = new ListViewItemFile();
                    lvi.Text=node.mPath;
                    lvi.ImageKey=node.GetImageKey();
                    lvi.mPath = node.mPath;
                    this.listViewSendFileBuffer.Items.Add(lvi);
                }
                    
            }
            catch { }
            finally { Cursor = Cursors.Default; }
            this.mSelectedTreeNode = null;
        }

        private void listViewSendFileBuffer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewSendFileBuffer_ItemDrag(object sender, ItemDragEventArgs e)
        {

        }

        private void timerListViewUpdata_Tick(object sender, EventArgs e)
        {
            this.InitListViewPC();
            this.InitListViewPhone();
            this.mCurrentListID = this.mNetInfo.mPcPhoneList.PhonePcListID;

        }

        private void 删除选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem var in this.listViewSendFileBuffer.Items)
            {
                if (var.Selected)
                {
                    var.Remove();
                }
            }
        }

        private void 删除所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listViewSendFileBuffer.Items.Clear();
        }

        public void SetServer(ServerCloudPrint server)
        {
             this.mNetInfo=server.NetServer;
             this.mServer=server;
        }
    }
}
