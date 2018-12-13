using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Printing;
using Microsoft.Office.Interop.Word;
using System.Reflection;

namespace PrintJobMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        public string GetAllPrinter()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string pName in PrinterSettings.InstalledPrinters)
            {
                sb.Append(pName);
                sb.Append(";");
            }

            return sb.ToString();
        }

        private string getPrinterStatus(string printer)
        {
            StringBuilder status = new StringBuilder();

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsBusy)
            {
                status.Append("[Busy:Yes] ");
            }
            else
            {
                status.Append("[Busy:No ] ");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsDoorOpened)
            {
                status.Append("[DoorOpen:Yes]");
            }
            else
            {
                status.Append("[DoorOpen:No ]");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsInError)
            {
                status.Append("[Error:Yes] ");
            }
            else
            {
                status.Append("[Error:No ] ");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsNotAvailable)
            {
                status.Append("[NotAvailable:Yes]");
            }
            else
            {
                status.Append("[NotAvailable:No ]");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsOutOfPaper)
            {
                status.Append("[OutOfPaper:Yes]");
            }
            else
            {
                status.Append("[OutOfPaper:No ]");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsPaperJammed)
            {
                status.Append("[Jam:Yes]");
            }
            else
            {
                status.Append("[Jam:No ]");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsPrinting)
            {
                status.Append("[Printing:Yes]");
            }
            else
            {
                status.Append("[Printing:No ]");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsTonerLow)
            {
                status.Append("[TonerLow:Yes]");
            }
            else
            {
                status.Append("[TonerLow:No ]");
            }

            if (System.Printing.LocalPrintServer.GetDefaultPrintQueue().IsIOActive)
            {
                status.Append("[IOActive:Yes]");
            }
            else
            {
                status.Append("[IOActive:No ]");
            }

            return status.ToString();

        }

        #region BUTTON Click事件

        private void buttonShowJobs_Click(object sender, EventArgs e)
        {
            try
            {
                string psName = "";
                PrintServer _ps = new PrintServer();//(@"\\" + psName);
                PrintQueueCollection _psAllQueues = _ps.GetPrintQueues();

                StringBuilder prnStr = new StringBuilder();


                foreach (PrintQueue pq in _psAllQueues)
                {
                    try
                    {
                        pq.Refresh();
                        //_pq.IsProcessing();

                        PrintJobInfoCollection jobs = pq.GetPrintJobInfoCollection();
                        foreach (PrintSystemJobInfo job in jobs)
                        {
                            prnStr.Append("\r\n");
                            prnStr.Append(pq.Name);
                            prnStr.Append(" : ");
                            prnStr.Append(pq.Location);
                            prnStr.Append("\r\n");
                            prnStr.Append(job.JobName);
                            prnStr.Append(" ID: ");
                            prnStr.Append(job.JobIdentifier);
                            prnStr.Append(" ID: ");
                            prnStr.Append(job.NumberOfPagesPrinted);
                            prnStr.Append("/");
                            prnStr.Append(job.NumberOfPages);
                            prnStr.Append(" st:");
                            prnStr.Append(job.JobStatus.ToString());
                            prnStr.Append("\r\n-------------------------------------\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        string sEvent = pq.Name + " : " + ex.Message;
                    }
                }

                string jobList = prnStr.ToString();
                if (string.IsNullOrEmpty(jobList))
                {
                    this.textBox1.Text = "No job in queue.";
                }
                else
                {

                    this.textBox1.Text = jobList;
                }
            }
            catch (System.Exception ex)
            {
                this.textBox1.Text = ex.Message;
            }
        }

        private void buttonDelJob_Click(object sender, EventArgs e)
        {
            string psName = "";
            PrintServer _ps = new PrintServer();//(@"\\" + psName);
            PrintQueueCollection _psAllQueues = _ps.GetPrintQueues();

            StringBuilder prnStr = new StringBuilder();
            string jobList = "";

            foreach (PrintQueue pq in _psAllQueues)
            {
                try
                {
                    pq.Refresh();
                    //_pq.IsProcessing();

                    PrintJobInfoCollection jobs = pq.GetPrintJobInfoCollection();
                    foreach (PrintSystemJobInfo job in jobs)
                    {
                        // delete jobs when in error status
                        if (job.IsInError && !job.IsRestarted && !job.IsDeleting)
                        {
                            job.Cancel();
                            prnStr.Append("\r\nDelete Job:  ");
                            prnStr.Append(pq.Name);
                            prnStr.Append(" : ");
                            prnStr.Append(pq.Location);
                            prnStr.Append("\r\n");
                            prnStr.Append(job.JobName);
                            prnStr.Append(" ID: ");
                            prnStr.Append(job.JobIdentifier);
                            prnStr.Append(" ID: ");
                            prnStr.Append(job.NumberOfPagesPrinted);
                            prnStr.Append("/");
                            prnStr.Append(job.NumberOfPages);
                            prnStr.Append(" st:");
                            prnStr.Append(job.JobStatus.ToString());
                            prnStr.Append("\r\n-------------------------------------\r\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string sEvent = pq.Name + " : " + ex.Message;
                }
            }

            this.textBox1.Text = prnStr.ToString() + jobList;
        }

        private void buttonShowPages_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                DialogResult ret = this.openFileDialogWord.ShowDialog(this);

                if (ret != DialogResult.OK)
                {
                    return;
                }

                string docFile = this.openFileDialogWord.FileName;

                if (docFile == null || !(docFile.ToLower().EndsWith("doc") || docFile.ToLower().EndsWith("docx")))
                {
                    msg = "非word文件: " + docFile;
                    throw new Exception(msg);
                }

                this.textBox1.Text = msg;

                object wordFile = docFile;

                object oMissing = Missing.Value;

                //自定义object类型的布尔值
                object oTrue = true;
                object oFalse = false;

                object doNotSaveChanges = WdSaveOptions.wdDoNotSaveChanges;

                //定义WORD Application相关
                Microsoft.Office.Interop.Word.Application appWord = new Microsoft.Office.Interop.Word.Application();

                //WORD程序不可见
                appWord.Visible = false;
                //不弹出警告框
                appWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;

                //打开要打印的文件
                Microsoft.Office.Interop.Word.Document doc = appWord.Documents.Open(
                    ref wordFile,
                    ref oMissing,
                    ref oTrue,
                    ref oFalse,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing,
                    ref oMissing);


                // 计算Word文档页数             
                WdStatistic stat = WdStatistic.wdStatisticPages;
                int num = doc.ComputeStatistics(stat, ref oMissing);//.ComputeStatistics(stat, ref Nothing); 
                //打印完关闭WORD文件
                doc.Close(ref doNotSaveChanges, ref oMissing, ref oMissing);

                //退出WORD程序
                appWord.Quit(ref oMissing, ref oMissing, ref oMissing);

                doc = null;
                appWord = null;

                msg = docFile + " : All Pages:" + num;

            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }

            this.textBox1.Text = msg;
            
        }

        private void buttonShowPrinter_Click(object sender, EventArgs e)
        {
            try
            {
                string printers = GetAllPrinter();

                if (string.IsNullOrEmpty(printers))
                {
                    return;
                }

                string[] all = printers.Split(';');

                StringBuilder prnStr = new StringBuilder();

                for (int i = 0; i < all.Count(); i++)
                {
                    if (string.IsNullOrEmpty(all[i]))
                    {
                        continue;
                    }

                    prnStr.Append(all[i]);
                    prnStr.Append("\r\n");
                    prnStr.Append(getPrinterStatus(all[i]));
                    prnStr.Append("\r\n-------------------------------------\r\n");
                }
                this.textBox1.Text = prnStr.ToString();
            }
            catch (System.Exception ex)
            {
                this.textBox1.Text = ex.Message;
            }
            
        }

        private void buttonShowPdfPages_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                DialogResult ret = this.openFileDialogPDF.ShowDialog(this);

                if (ret != DialogResult.OK)
                {
                    return;
                }

                string docFile = this.openFileDialogPDF.FileName;


                if (docFile == null || !docFile.ToLower().EndsWith("pdf"))
                {
                    msg = "非pdf文件: " + docFile;
                    throw new Exception(msg);
                }

                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(docFile);
                int num = reader.NumberOfPages;

                msg = docFile + " : All Pages:" + num;                
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }

            this.textBox1.Text = msg;

        }
        #endregion
    }
}
