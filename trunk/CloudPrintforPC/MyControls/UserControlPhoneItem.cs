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
    public partial class UserControlPhoneItem : UserControl
    {
        private ServerInfo mServerInfo = null;
        public UserControlPhoneItem()
        {
            InitializeComponent();
        }
        public UserControlPhoneItem(ServerInfo si)
        {
            this.mServerInfo = si;
            InitializeComponent();
            this.labelPaperNum.Text = "打印纸张数："+"110";
            this.labelPhoneName.Text = "手机名：" + si.mHostname;
            this.labelPhoneNumber.Text = "手机号：" + "15365078745";
            this.labelPrintDocNum.Text = "打印文件数：" + "11";
            if(si.mHostType==1)
            {
                this.pictureBoxImg.Image = Properties.Resources.Computer;
            }else if(si.mHostType==2){
                this.pictureBoxImg.Image = Properties.Resources.phone;
            }else{
            
            }
 
         
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UserControlPhoneItem_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxImg_Click(object sender, EventArgs e)
        {

        }

        private void UserControlPhoneItem_Resize(object sender, EventArgs e)
        {
            //int width = this.Parent.Size.Width;
            //this.Width = width;
        }

        private void UserControlPhoneItem_Layout(object sender, LayoutEventArgs e)
        {
            //if (this.Parent != null)
            //{
            //    int width = this.Parent.Size.Width;
            //    this.Width = width;
            //}
      
        }
    }
}
