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
    public partial class FormNetPrint : Form
    {
        public ServerCloudPrint mServer;
        public FormNetPrint()
        {
            InitializeComponent();
        }

        private void FormNetPrint_Load(object sender, EventArgs e)
        {
            
            PrintServer ps=this.mServer.PrintSvr;
            for (int i = 0; i < ps.ListPrint.Count; i++) {

                PrintOneInfo poi = (PrintOneInfo)(ps.ListPrint[i]);
                this.mNetPrintList.Add(poi);
              
          }

        }

        private void mNetPrintList_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
