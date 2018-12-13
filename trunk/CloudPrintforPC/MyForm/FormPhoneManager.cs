using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CloudPrintforPC
{
    public partial class FormPhoneManager : Form
    {
        public String mCurrentListID=null;
        public ServerCloudPrint mServer;
        public FormPhoneManager()
        {
            InitializeComponent();
        }

        private void FormPhoneManager_Load(object sender, EventArgs e)
        {
            this.InitListViewPhone();
        }

        private void InitListViewPhone()
        {

#if false
            if (this.mNetInfo.mPcPhoneList.mPhoneListUpdata || this.listViewPhone.Items.Count == 0)
            {
                this.listViewPhone.Clear();
                this.listViewPhone.SmallImageList = this.mNetInfo.ImageIcon;
                this.listViewPhone.LargeImageList = this.mNetInfo.ImageIcon;
                ServerInfo[] array = this.mNetInfo.mPcPhoneList.GetPhonelistArray();
                for (int i = 0; i < array.Length; i++)
                {

                    ListViewItemServer lvi = new ListViewItemServer();
                    lvi.Text = array[i].GetServerDes();
                    lvi.ImageKey = array[i].GetImageKey();
                    lvi.mServerInfo = array[i];
                    this.listViewPhone.Items.Add(lvi);

                }

            }
            this.mNetInfo.mPcPhoneList.mPhoneListUpdata = false;
#else

            if (mCurrentListID != this.mServer.NetServer.mPcPhoneList.PhonePcListID || this.mListView.mServerInfoArray == null || this.mListView.mServerInfoArray.Length == 0)
            {

                ServerInfo[] array = this.mServer.NetServer.mPcPhoneList.GetPhonelistArray();

                this.mListView.SetData(array);
                mCurrentListID = this.mServer.NetServer.mPcPhoneList.PhonePcListID;

            }
          
#endif
        }

        private void mListView_Resize(object sender, EventArgs e)
        {
            this.mListView.Width = this.Width;
            this.mListView.Height = this.Height;
        }

        private void timerUpdataList_Tick(object sender, EventArgs e)
        {
            this.InitListViewPhone();
        }

        private void mListView_Load(object sender, EventArgs e)
        {

        }
    }
}
