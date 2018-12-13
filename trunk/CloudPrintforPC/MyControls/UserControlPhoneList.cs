using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CloudPrintforPC
{
    public partial class UserControlPhoneList : UserControl
    {
        public  List<ServerInfo> mListServerInfo;
        public ServerInfo[] mServerInfoArray;
        private List<UserControlPhoneItem> PhoneItemView;
         public UserControlPhoneList()
        {
            InitializeComponent();
        }

        public void SetData(ServerInfo[] array)
         {            
             this.flowLayoutPanel1.Controls.Clear();
             this.mServerInfoArray = array;
             for (int i = 0; i < array.Length; i++) {
                 UserControlPhoneItem pl =new UserControlPhoneItem(array[i]);
                 this.flowLayoutPanel1.Controls.Add(pl);
             }

         }

        private void UserControlPhoneList_Load(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            //foreach (Control control in ((Control)sender).Controls)
            //{
            //    control.Width = ((Control)sender).Width;
            //}
           
        }
    }
}
