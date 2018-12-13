using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace CloudPrintforPC
{
    public partial class UserControlNetPrintItem : UserControl
    {
        public PrintOneInfo mPrintInfo;
        public UserControlNetPrintItem()
        {
            InitializeComponent();
        }
        public UserControlNetPrintItem(PrintOneInfo poi)
        {
            this.mPrintInfo = poi;
            InitializeComponent();
            this.labelName.Text = "打印机名：" + poi.mPrintName;
            this.SetPrinterStatus();
            this.SetPrinterStatus();
            this.label3.Text="正在刷新中";
        }

        private void PrintDocment_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Multiselect = true;
            o.Filter = "*.*|*.*";
            if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //o.FileNames选择的结果，多个文件路径及文件名
                //o.FileName选择的结果，单个文件路径及文件名
                foreach (String file in o.FileNames) 
                {
                    this.mPrintInfo.PrintFiles(file);
                }

            }
        }

        private void UserControlNetPrintItem_Load(object sender, EventArgs e)
        {
           
        }

        private void timerUpdata_Tick(object sender, EventArgs e)
        {
            this.SetPrinterStatus();
            this.SetPrinterProgress();
        }

        private void labelName_Click(object sender, EventArgs e)
        {

        }
        private void SetPrinterProgress()
        {
            if (this.mPrintInfo.IsOffline() == true)
            {
                //离线
                this.BackColor = Color.Gray;
            }
            else {
                try
                {
                    this.progressBar1.Value += 5;
                }
                catch (Exception e)
                {
                    this.progressBar1.Value = 0;
                }
            
            }
         
          
        }
        private void SetPrinterStatus() 
        {
            this.labelPrintNow.Text = "打印机状态：" + this.mPrintInfo.Status;
        }
        //public bool PrintFiles(String fileName)
        //{
        //     PrintDocument prnDoc=new PrintDocument();
        //    return true;
        //}
       
    }
}
