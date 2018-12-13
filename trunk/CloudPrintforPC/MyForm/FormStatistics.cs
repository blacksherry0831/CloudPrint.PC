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
    public partial class FormStatistics : Form
    {
        public ServerCloudPrint mServer;
        List<PrintRecordItem> pRi;
        public FormStatistics()
        {
            InitializeComponent();
        }

        private void FormStatistics_Load(object sender, EventArgs e)
        {
            this.AddData2List();
        }
        private void AddData2List() 
        {
            pRi = PrintRecord.ReadAllRecord();
            this.AddListView(this.listViewData,pRi);
        }

        public void AddListView(ListView listView, List<PrintRecordItem> pris) 
        {
            listView.BeginUpdate();
            listView.Items.Clear();
            foreach(PrintRecordItem pri in pris){

                listView.Items.Add(pri.GetListView());
            }

            listView.EndUpdate();
        }
        private void listViewData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timerUpdataData_Tick(object sender, EventArgs e)
        {
            List<PrintRecordItem> pRi_new = PrintRecord.ReadAllRecord();
            if (pRi_new.Count == this.pRi.Count)
            {

            }
            else {
                this.AddListView(this.listViewData, pRi_new);
                this.pRi = pRi_new;
            }
          
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.AddData2List();
        }
    }
}
