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
    public partial class UserControlNetPrintList : UserControl
    {
        public UserControlNetPrintList()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserControlNetPrintList_Load(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < 10; i++) {
                this.flowLayoutPanelList.Controls.Add(new UserControlNetPrintItem());
            }*/
             
        }
        public void Add(PrintOneInfo poi)
        {
            this.flowLayoutPanelList.Controls.Add(new UserControlNetPrintItem(poi));
        }
        public void Add(UserControlNetPrintItem item)
        {
            this.flowLayoutPanelList.Controls.Add(item);
        }
        public void Clear()
        {
            this.flowLayoutPanelList.Controls.Clear();
        }
    }
}
