using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.Printing;
using System.Data.Sql;
using System.Data.SqlClient;
using BaseLib;
using PrintJobMonitor;
//using CloudPrintforPC;

namespace PCLConvertCS
{
    public class PCLConvert
    {
        #region 内部变量
        private int G_Witdh = 827;
        private int G_HEIGHT = 1169;
        private PrintDocument prnDoc;

        private const string DUPLEX_STR_PCL = "@PJL SET DUPLEX = ON";
        private const string DUP_BIND_STR_PCL = "@PJL SET BINDING = LONGEDGE";
        public const string DUP_BIND_STR_PCL_Short = "@PJL SET BINDING = SHORTEDGE";
        private byte[] PCL_SET_BYTES = {   (byte)'@', (byte)'P', (byte)'J', (byte)'L', (byte)' ', 
                                                  (byte)'S', (byte)'E', (byte)'T' , (byte)' '};

        private byte[] DUP_ON_BYTES = {        (byte)'D', (byte)'U', (byte)'P', (byte)'L', (byte)'E', (byte)'X',
                                                  (byte)' ', (byte)'=', (byte)' ' , 
                                                  (byte)'O', (byte)'N', 0x0D, 0x0A };

        private byte[] DUP_OFF_BYTES = {        (byte)'D', (byte)'U', (byte)'P', (byte)'L', (byte)'E', (byte)'X',
                                                  (byte)' ', (byte)'=', (byte)' ' , 
                                                  (byte)'O', (byte)'F', (byte)'F', 0x0D, 0x0A };

        private byte[] DUP_BIND_BYTES = {         (byte)'B', (byte)'I', (byte)'N', (byte)'D', (byte)'I', (byte)'N', (byte)'G',0x20, 
                                                  (byte)' ', (byte)'=', (byte)' ' , 
                                                  (byte)'L', (byte)'O', (byte)'N', (byte)'G', (byte)'E', (byte)'D', (byte)'G',(byte)'E',
                                                  0x0D, 0x0A };
        public byte[] DUP_BIND_BYTES_Short = {         (byte)'B', (byte)'I', (byte)'N', (byte)'D', (byte)'I', (byte)'N', (byte)'G',0x20, 
                                                  (byte)' ', (byte)'=', (byte)' ' , 
                                                  (byte)'S', (byte)'H', (byte)'O', (byte)'R', (byte)'T', (byte)'E', (byte)'D', (byte)'G',(byte)'E',
                                                  0x0D, 0x0A };

        //@PJL SET RENDERMODE = COLOR
        private byte[] COLOR_BYTES = {        (byte)'R', (byte)'E', (byte)'N', (byte)'D', (byte)'E', (byte)'R',(byte)'M', (byte)'O', (byte)'D', (byte)'E',
                                                  (byte)' ', (byte)'=', (byte)' ' , 
                                                  (byte)'C', (byte)'O', (byte)'L', (byte)'O', (byte)'R', 0x0D, 0x0A };
        //@PJL SET RENDERMODE = GRAYSCALE
        private byte[] GRAY_BYTES = {        (byte)'R', (byte)'E', (byte)'N', (byte)'D', (byte)'E', (byte)'R',(byte)'M', (byte)'O', (byte)'D', (byte)'E',
                                                  (byte)' ', (byte)'=', (byte)' ' , 
                                                  (byte)'G', (byte)'R', (byte)'A', (byte)'Y', (byte)'S',(byte)'C', (byte)'A', (byte)'L', (byte)'E', 0x0D, 0x0A };

        //PageSetupDialog pageSetupDialog1;
        private string m_sysTmpPath;

        private List<string> m_allJpgFile = new List<string>();
        #endregion

        public PCLConvert(bool isShowUI)
        {
            m_showUI = isShowUI;
            m_userCopies = 1;
            m_userPrintFirstPage = 0;
            m_userPrintLastPage = 0;
            m_userDuplex = false;

            try
            {
                m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
            }
            catch (System.Exception ex)
            {
                try
                {
                    m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
                }
                catch (System.Exception ex2)
                {
                    m_sysTmpPath = "c:\\windows\\PCL-CVT";
                }
            }

            try
            {
                System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(m_sysTmpPath);
                tmp.Delete(true);
            }
            catch (System.Exception ex)
            {

            }
        }

        public PCLConvert()
        {
            m_showUI = true;
            m_userCopies = 1;
            m_userPrintFirstPage = 0;
            m_userPrintLastPage = 0;
            m_userDuplex = false;

            try
            {
                m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
            }
            catch (System.Exception ex)
            {
                try
                {
                    m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
                }
                catch (System.Exception ex2)
                {
                    m_sysTmpPath = "c:\\windows\\PCL-CVT";
                }
            }

            try
            {
                System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(m_sysTmpPath);
                tmp.Delete(true);
            }
            catch (System.Exception ex)
            {

            }
        }

        #region 转后文件接口实现
        /// <summary>
        /// 转换多个JPG到PCL
        /// </summary>
        /// <param name="file">转换的文件</param>
        /// <param name="docPath">转换的临时存储目录</param>
        /// <returns></returns>
        public string MultiJPGToPCL(string file, string docPath, string outFile)
        {
            this.prnDoc = new System.Drawing.Printing.PrintDocument();
            this.prnDoc.PrintPage += new PrintPageEventHandler(this.pdPrintImagePage);

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(docPath);
            foreach (FileInfo fi in dir.GetFiles())
            {
                m_allJpgFile.Add(fi.FullName);
            }

            if (m_allJpgFile.Count == 0)
            {
                return "转换出错：文件名字未识别或者文件格式错误";
            }

            if (string.IsNullOrEmpty(outFile))
            {
                return JPGToPCL(file, file + ".prn"); 
            }

            return JPGToPCL(file, outFile); 
        }

        /// <summary>
        /// 转换一个JPG到PCL
        /// </summary>
        /// <param name="doc">JPG文件</param>
        /// <param name="outFile">输出的PCL文件，为null的时候，默认是源文件名+".prn"</param>
        /// <returns></returns>
        public string JPGToPCL(string doc, string outFile)
        {
            if (this.prnDoc == null)
            {
                this.prnDoc = new System.Drawing.Printing.PrintDocument();
                this.prnDoc.PrintPage += new PrintPageEventHandler(this.pdPrintImagePage);
            }

            string printer = PrintLocal.GetPCLPrinter();


            if (printer == null)
            {
                return "请安装PCL打印机驱动";
            }

            if (m_allJpgFile.Count == 0)                  
            {
                if (string.IsNullOrEmpty(doc)
                    || !(doc.EndsWith("JPG") 
                        || doc.EndsWith("jpg")
                        || doc.EndsWith("JPEG") 
                        || doc.EndsWith("jpeg")
                        )
                    )
                {
                    return "非JPG图片"+doc;
                }
                
                m_allJpgFile.Add(doc);
            }
            

            if (outFile == null)
            {
                outFile = doc + ".prn";
            }


            //this.pageSetupDialog1 = new PageSetupDialog();
            //this.pageSetupDialog1.Document = this.prnDoc;
            //this.pageSetupDialog1.ShowDialog();


            //设置打印到文件                
            this.prnDoc.PrinterSettings.PrinterName = printer;
            this.prnDoc.PrinterSettings.PrintToFile = true;
            this.prnDoc.PrinterSettings.PrintFileName = outFile;


            // 隐藏打印取消对话框
            PrintController pc = new StandardPrintController();
            PrintController oldPc = this.prnDoc.PrintController;
            this.prnDoc.PrintController = pc;


            try
            {   
                this.prnDoc.Print();
            }
            catch (Exception excep)
            {
                return excep.Message;
            }
            try
            {
                this.prnDoc.PrintController = oldPc;
            }
            catch (Exception excep) {
                return excep.Message;
            }
            finally {
                this.prnDoc.Dispose();
                this.prnDoc = null;
            }
           
         


            return null;
        }
        
        #region 打印参数设置，当设定非UI显示时，可以通过下面接口实现打印参数的设置

        private bool m_showUI;
        private int m_userCopies = 1;
        private int m_userPrintFirstPage = 0;
        private int m_userPrintLastPage = 0;
        private bool m_userDuplex = false;
        private bool m_colorInfo = false;

        /// <summary>
        /// 设置转换时打印参数
        /// </summary>
        /// <param name="page">转换时打印参数</param>
        /// <returns>bool</returns>
        public bool SetPrintParam(bool duplex, int copies, int firstPage, int lastPage, bool color)
        {
            SetDuplex(duplex);
            SetColorInfo(color);

            if (!SetCopies(copies))
            {
                return false;
            }
            if (!SetFirstPage(firstPage))
            {
                return false;
            }
            if (!SetLastPage(lastPage))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置彩色打印
        /// </summary>
        /// <param name="page">双面打印</param>
        /// <returns>void</returns>
        public void SetColorInfo(bool color)
        {
            this.m_colorInfo = color;
        }

        /// <summary>
        /// 设置双面打印
        /// </summary>
        /// <param name="page">双面打印</param>
        /// <returns>void</returns>
        public void SetDuplex(bool isEnable)
        {
            this.m_userDuplex = isEnable;
        }

        /// <summary>
        /// 设置打印份数
        /// </summary>
        /// <param name="page">打印份数</param>
        /// <returns>bool</returns>
        public bool SetCopies(int copies)
        {
            if (copies > 0)
            {
                this.m_userCopies = copies;

                return true;
            }

            return false;
        }


        /// <summary>
        /// 设置打印页码第一页
        /// </summary>
        /// <param name="page">打印页码</param>
        /// <returns>bool</returns>
        public bool SetFirstPage(int page)
        {
            if (page > 0)
            {
                if (this.m_userPrintLastPage < page || m_userPrintLastPage == 0)
                {
                    this.m_userPrintLastPage = page;
                }

                this.m_userPrintFirstPage = page;

                return true;
            }
            else if (page == 0)
            {
                this.m_userPrintFirstPage = 0;
                this.m_userPrintLastPage = 0;

                return true;
            }

            return false;
        }


        /// <summary>
        /// 设置打印页码最后一页
        /// </summary>
        /// <param name="page">打印页码</param>
        /// <returns>bool</returns>
        public bool SetLastPage(int page)
        {
            if (page > 0)
            {
                if (this.m_userPrintFirstPage > page)
                {
                    this.m_userPrintFirstPage = page;
                }
                else if (m_userPrintFirstPage == 0)
                {
                    this.m_userPrintFirstPage = 1;
                }

                this.m_userPrintLastPage = page;

                return true;
            }
            else if (page == 0)
            {
                this.m_userPrintFirstPage = 0;
                this.m_userPrintLastPage = 0;

                return true;
            }

            return false;
        }
        #endregion

        /// <summary>
        /// 转换word到PCL,需要安装office word
        /// </summary>
        /// <param name="wordDoc">word文件</param>
        /// <param name="outFile">输出的PCL文件，为null的时候，默认是源文件名+".prn"</param>
        /// <returns></returns>
        private int m_copies = 1;
        private string m_pages = "";
        private int m_lastPage = 0;
        private int m_firstPage = 0;
        private bool m_cancel = false;
        private bool isDouble = false;
        public string m_fileID;
        public bool color;
        public void Task(List<Client_simplify.lists.Class_PrintArgs> c)
        {
            Client_simplify.lists.Class_PrintArgs r = c[0];
            m_copies = r.Copies;
            isDouble = r.IsDuplex;
            m_pages = r.Pages;
            color = r.Color;
        }

        public bool getDuplexSet()
        {
            return isDouble;
        }

        public int getCopiesNum()
        {
            return m_copies;
        }

        public int getFirstPage()
        {
            return m_firstPage;
        }
        public int getLastPage()
        {
            return m_lastPage;
        }
        public string getPages()
        {
            return m_pages;
        }
        public bool getColorInfo()
        {
            return color;
        }
      //  public string constr = @"Data Source=192.168.0.100\SQLEXPRESS;Initial Catalog=Client;user id='sa';password='sa'";  
        private void checkPrintPages()
        {
            if (m_pages == "全部")
            {
                this.m_firstPage = 0;
                this.m_lastPage = 0;
                this.m_pages = "";

            }
            else
            {
                //int index = this.textBoxPages.Text.IndexOf('-');
                int index = this.m_pages.IndexOf('-');
                if (index == -1)
                {
                    try
                    {
                        this.m_firstPage = int.Parse(m_pages.Trim());
                        this.m_lastPage = this.m_firstPage;
                        this.m_pages = m_pages.Trim();



                    }
                    catch
                    {
                        MessageBox.Show("页码设置错误");
                    }
                }
                else
                {
                    //string first = this.textBoxPages.Text.Substring(0, index).Trim();
                    string first = this.m_pages.Substring(0, index).Trim();
                    //string last = this.textBoxPages.Text.Substring(index + 1).Trim();
                    string last = this.m_pages.Substring(index + 1).Trim();

                    try
                    {
                        this.m_firstPage = int.Parse(first);
                        this.m_lastPage = int.Parse(last);
                        this.m_pages = this.m_firstPage + "-" + this.m_lastPage;
                    }
                    catch
                    {
                        MessageBox.Show("页码设置错误");
                    }
                }
            }
        }  

        
        public string WordToPCL(List<Client_simplify.lists.Class_PrintArgs> c, string outFile,string printer)
        {
            String error_msg = null;
            Client_simplify.lists.Class_PrintArgs a=c[0];
            if ( a.Filepath== null
                    || !(a.Filepath.EndsWith("doc") || a.Filepath.EndsWith("DOC")
                         || a.Filepath.EndsWith("DOCX") || a.Filepath.EndsWith("docx")
                         ))
            {
                error_msg="非word文件: " + a.Filepath;
                LogHelper.WriteLog(this.GetType(), error_msg);
                return error_msg;  
            }
            if (outFile == null)
            {
                outFile = a.Filepath + ".prn";
            }
            string pclPrinter = printer;
            if (pclPrinter == null)
            {
                error_msg="找不到PCL打印机驱动";
                LogHelper.WriteLog(this.GetType(), error_msg);
                return error_msg;
            }
            if (this.m_showUI)
            {
                Task(c);
                checkPrintPages();
                this.m_userCopies = this.getCopiesNum();
                this.m_userDuplex = this.getDuplexSet();
                this.m_colorInfo = this.getColorInfo();
                this.m_userPrintFirstPage = this.getFirstPage();
                this.m_userPrintLastPage = this.getLastPage();
            }       
            bool duplexSet = this.m_userDuplex;
            object copies  = this.m_userCopies;
            object oFromPage = Missing.Value;
            object oToPage = Missing.Value;
            object ranges = Microsoft.Office.Interop.Word.WdPrintOutRange.wdPrintAllDocument; 
            if (this.m_userPrintFirstPage > 0 && this.m_userPrintLastPage > 0)
            {
                oFromPage = this.m_userPrintFirstPage.ToString();
                oToPage = this.m_userPrintLastPage.ToString();
                ranges = Microsoft.Office.Interop.Word.WdPrintOutRange.wdPrintFromTo;
            }
            object wordFile = a.Filepath;
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
            //先保存默认的打印机
            string defaultPrinter = appWord.ActivePrinter;
            //MessageBox.Show("默认打印机: " + defaultPrinter, "打印结束", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //打开要打印的文件
            Document doc = appWord.Documents.Open(
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
            //设置指定的打印机
            appWord.ActivePrinter = pclPrinter;
            //appWord.Options.PrintOddPagesInAscendingOrder = true;
            //appWord.Options.PrintEvenPagesInAscendingOrder = true;
            object oOutFile = outFile;
            // APIhttp://msdn.microsoft.com/en-us/library/microsoft.office.tools.word.document.printout(v=VS.80).aspx
            doc.PrintOut(
                ref oTrue, // Background 此处为true,表示后台打印
                ref oFalse, // Append true to append the document to the file specified by the OutputFileName argument; false to overwrite the contents of OutputFileName.
                ref ranges, // The page range. Can be any WdPrintOutRange value.
                ref oOutFile, //If PrintToFile is true, this argument specifies the path and file name of the output file.
                ref oFromPage, //The starting page number when Range is set to wdPrintFromTo.
                ref oToPage, //The ending page number when Range is set to wdPrintFromTo.
                ref oMissing, //The item to be printed. Can be any WdPrintOutItem value.
                ref copies, //The number of copies to be printed.
                ref oMissing, //The page numbers and page ranges to be printed, separated by commas. For example, "2, 6-10" prints page 2 and pages 6 through 10.
                ref oMissing, // The type of pages to be printed. Can be any WdPrintOutPages value.
                ref oTrue, //PrintToFile true to send printer instructions to a file. Make sure to specify a file name with OutputFileName.
                ref oMissing, //Collate When printing multiple copies of a document, true to print all pages of the document before printing the next copy.
                ref oMissing, //ActivePrinterMacGX This argument is available only in Microsoft Office Macintosh Edition. For additional information about this argument, consult the language 
                //reference Help included with Microsoft Office Macintosh Edition.
                ref oMissing, // ManualDuplexPrint 如果为 true，则在没有双面打印装置的打印机上打印双面文档。如果此参数为 true，则忽略 PrintBackground 和 PrintReverse 属性。
                ref oMissing, // PrintZoomColumn 希望 Word 在一页上水平布置的页数。可以为 1、2、3 或 4。与 PrintZoomRow 参数一起使用时可在单张纸上打印多页
                ref oMissing, // PrintZoomRow    希望 Word 在一页上垂直布置的页数。可以为 1、2 或 4。与 PrintZoomColumn 参数一起使用时可在单张纸上打印多页
                ref oMissing, // PrintZoomPaperWidth   希望 Word 将打印页缩放到的宽度
                ref oMissing  // PrintZoomPaperHeight  希望 Word 将打印页缩放到的高度
                );
            WaitForJobEnd(a.Filepath);
            //还原原来的默认打印机
            appWord.ActivePrinter = defaultPrinter;
            //MessageBox.Show("打印成功", "打印结束", MessageBoxButtons.OK, MessageBoxIcon.Information);   
            //打印完关闭WORD文件
            doc.Close(ref doNotSaveChanges, ref oMissing, ref oMissing);
            //退出WORD程序
            appWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            doc = null;
            appWord = null;
            if (ChangePCLArgs(outFile,printer) == false)
            {
                return "转换失败" + outFile;
            }
            return null;
        }


        public string Word2PDFToPCL(List<Client_simplify.lists.Class_PrintArgs> c, string outFile, string printer)
        {
            String error_msg = null;
           
            Client_simplify.lists.Class_PrintArgs a = c[0];
            if (a.Filepath == null
                    || !(a.Filepath.EndsWith("doc") || a.Filepath.EndsWith("DOC")
                         || a.Filepath.EndsWith("DOCX") || a.Filepath.EndsWith("docx")
                         ))
            {
                error_msg = "非word文件: " + a.Filepath;
                LogHelper.WriteLog(this.GetType(), error_msg);
                return error_msg;
            }
            if (outFile == null)
            {
                outFile = a.Filepath + ".prn";
            }
            string pclPrinter = printer;
            if (pclPrinter == null)
            {
                error_msg = "找不到PCL打印机驱动";
                LogHelper.WriteLog(this.GetType(), error_msg);
                return error_msg;
            }
            //////////////////////////////////////
            try {
                string path = System.Environment.GetEnvironmentVariable("TEMP");
                string temp_pdf = path +"\\"+ Guid.NewGuid().ToString() + ".pdf";
                    if (PrintJobMonitor.Convert2.ConvertWord2Pdf(a.Filepath, temp_pdf)){
                                a.Filepath = temp_pdf;//转换为PDF ,改变指向
                                PCLConvertCS.PDFConvert pcpdf = new PCLConvertCS.PDFConvert();
                                error_msg= pcpdf.ConvertPdf2pcl(c,outFile);                    
                    }               
                File.Delete(temp_pdf);
            }catch (Exception e) {
                LogHelper.WriteLog(this.GetType(), e.Message);
            }
            //////////////////////////////////////          
            return error_msg;

           
        }

        public string WordToPCLEndPage(string wordDoc, string outFile,string printer)
        {
            if (wordDoc == null
                    || !(wordDoc.EndsWith("doc") || wordDoc.EndsWith("DOC")
                         || wordDoc.EndsWith("DOCX") || wordDoc.EndsWith("docx")
                         ))
            {
                return "非word文件: " + wordDoc;
            }

            if (outFile == null)
            {
                outFile = wordDoc + ".prn";
            }

            //string pclPrinter = GetPCLPrinter();
            string pclPrinter = printer;
            if (pclPrinter == null)
            {
                return "找不到PCL打印机驱动";
                //MessageBox.Show("找不到PCL打印机驱动", "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (this.m_showUI)
            {
                // SetPrinter dlg = new SetPrinter();
                // this.Task();
                // dlg.showPrinterSet();
                /* if (dlg.getDialogResult() == DialogResult.Cancel)
                 {
                     return "取消转换";
                 }*/

                // this.m_userCopies = dlg.getCopiesNum();
                this.m_userCopies = 1;
                // this.m_userDuplex = dlg.getDuplexSet();
                // this.m_userPrintFirstPage = dlg.getFirstPage();
                // this.m_userPrintLastPage = dlg.getLastPage();
            }


            bool duplexSet = this.m_userDuplex;
            object copies = this.m_userCopies;
            object oFromPage = Missing.Value;
            object oToPage = Missing.Value;
            object ranges = Microsoft.Office.Interop.Word.WdPrintOutRange.wdPrintAllDocument;

            if (this.m_userPrintFirstPage > 0 && this.m_userPrintLastPage > 0)
            {
                oFromPage = this.m_userPrintFirstPage.ToString();
                oToPage = this.m_userPrintLastPage.ToString();
                ranges = Microsoft.Office.Interop.Word.WdPrintOutRange.wdPrintFromTo;
            }

            object wordFile = wordDoc;

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

            //先保存默认的打印机
            string defaultPrinter = appWord.ActivePrinter;
            //MessageBox.Show("默认打印机: " + defaultPrinter, "打印结束", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //打开要打印的文件
            Document doc = appWord.Documents.Open(
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


            //设置指定的打印机
            appWord.ActivePrinter = pclPrinter;
            //appWord.Options.PrintOddPagesInAscendingOrder = true;
            //appWord.Options.PrintEvenPagesInAscendingOrder = true;
            object oOutFile = outFile;
            // APIhttp://msdn.microsoft.com/en-us/library/microsoft.office.tools.word.document.printout(v=VS.80).aspx
            doc.PrintOut(
                ref oTrue, // Background 此处为true,表示后台打印
                ref oFalse, // Append true to append the document to the file specified by the OutputFileName argument; false to overwrite the contents of OutputFileName.
                ref ranges, // The page range. Can be any WdPrintOutRange value.
                ref oOutFile, //If PrintToFile is true, this argument specifies the path and file name of the output file.
                ref oFromPage, //The starting page number when Range is set to wdPrintFromTo.
                ref oToPage, //The ending page number when Range is set to wdPrintFromTo.
                ref oMissing, //The item to be printed. Can be any WdPrintOutItem value.
                ref copies, //The number of copies to be printed.
                ref oMissing, //The page numbers and page ranges to be printed, separated by commas. For example, "2, 6-10" prints page 2 and pages 6 through 10.
                ref oMissing, // The type of pages to be printed. Can be any WdPrintOutPages value.
                ref oTrue, //PrintToFile true to send printer instructions to a file. Make sure to specify a file name with OutputFileName.
                ref oMissing, //Collate When printing multiple copies of a document, true to print all pages of the document before printing the next copy.
                ref oMissing, //ActivePrinterMacGX This argument is available only in Microsoft Office Macintosh Edition. For additional information about this argument, consult the language 
                //reference Help included with Microsoft Office Macintosh Edition.
                ref oMissing, // ManualDuplexPrint 如果为 true，则在没有双面打印装置的打印机上打印双面文档。如果此参数为 true，则忽略 PrintBackground 和 PrintReverse 属性。
                ref oMissing, // PrintZoomColumn 希望 Word 在一页上水平布置的页数。可以为 1、2、3 或 4。与 PrintZoomRow 参数一起使用时可在单张纸上打印多页
                ref oMissing, // PrintZoomRow    希望 Word 在一页上垂直布置的页数。可以为 1、2 或 4。与 PrintZoomColumn 参数一起使用时可在单张纸上打印多页
                ref oMissing, // PrintZoomPaperWidth   希望 Word 将打印页缩放到的宽度
                ref oMissing  // PrintZoomPaperHeight  希望 Word 将打印页缩放到的高度
                );

            WaitForJobEnd(wordDoc);

            //还原原来的默认打印机
            appWord.ActivePrinter = defaultPrinter;
            //MessageBox.Show("打印成功", "打印结束", MessageBoxButtons.OK, MessageBoxIcon.Information);



            //打印完关闭WORD文件
            doc.Close(ref doNotSaveChanges, ref oMissing, ref oMissing);

            //退出WORD程序
            appWord.Quit(ref oMissing, ref oMissing, ref oMissing);

            doc = null;
            appWord = null;


            if (ChangePCLArgs(outFile,printer) == false)
            {
                return "转换失败" + outFile;
            }

            return "OK Pages:" + num;
        }
        #endregion

        #region 内部使用函数
        /// <summary>
        /// 监控Job状态
        /// </summary>
        /// <param name="szFileName">PCL打印文件名</param>
        /// <returns>Job状态</returns>
        private PrintJobStatus GetPrintJobStat(string szFileName)
        {
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
                        if (job.Name.IndexOf(szFileName) > 0)
                        {
                            Console.WriteLine(job.Name);
                            return job.JobStatus;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(this.GetType(), ex.Message);
                    Console.WriteLine(ex.Message);
                    return PrintJobStatus.None;
                }
            }

            return PrintJobStatus.Completed;
        }

/**
*
*if (((theJob.JobStatus & PrintJobStatus.Completed) == PrintJobStatus.Completed)
*||
*((theJob.JobStatus & PrintJobStatus.Printed) == PrintJobStatus.Printed))
*{
*Console.WriteLine("The job has finished. Have user recheck all output bins and be sure the correct printer is being checked.");
*}
*
*/
        private void WaitForJobEnd(string szFileName)
        {
            Thread.Sleep(2000);

            string file = szFileName;
            int index = -1;
            if ((index = szFileName.LastIndexOf(@"\")) > 0)
            {
                file = file.Substring(index + 1, szFileName.Length - index - 1);
            }


            PrintJobStatus st = GetPrintJobStat(file);
            LogHelper.WriteLog(this.GetType(), "Start>>WaitForJob&&PrintJobStatus.&&" + st.ToString());
            try
            {
                while (true)
                {
                    if ((st & PrintJobStatus.Completed) == PrintJobStatus.Completed)
                    {
                        break;
                    }
                    else if ((st & PrintJobStatus.Error) == PrintJobStatus.Error)
                    {
                        LogHelper.WriteLog(this.GetType(), "PrintJobStatus.&&" + st.ToString());
                        Console.WriteLine(st.ToString());
                        break;
                    }
                    else if ((st & PrintJobStatus.Deleted) == PrintJobStatus.Deleted)
                    {
                        Console.WriteLine(st.ToString());
                        break;
                    }
                    else if ((st & PrintJobStatus.Deleting) == PrintJobStatus.Deleting)
                    {
                        Console.WriteLine(st.ToString());
                        break;
                    } else if ((st & PrintJobStatus.Printed) == PrintJobStatus.Printed) {
                        Thread.Sleep(500);
                        break;
                    }else {
                        Thread.Sleep(500);
                        //未处理状态
                    }
                    Thread.Sleep(500);
                    st = GetPrintJobStat(file);
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(this.GetType(), ex.Message);
                Console.WriteLine(ex.Message);
            }
            finally
            {
                LogHelper.WriteLog(this.GetType(), "End>>WaitForJob&&PrintJobStatus.&&" + st.ToString());
                Console.WriteLine("WaitForJobEnd" + st.ToString());
            }

        }

        private bool ChangePCLArgs(string filePath,string printer)
        {
            try
            {
                return SetPCLArgs(filePath,printer);                
            }
            catch (System.Exception ex2)
            {
                LogHelper.WriteLog(this.GetType(),ex2.Message);
                Console.WriteLine(ex2.Message);
                return false;
            }
        }

        private bool BinaryCopyTo (Stream src, Stream dest)
        {
            if (src == null || dest == null)
            {
                return false;
            }

            if (!src.CanRead && !dest.CanWrite)
            {
                return false;
            }

            byte[] array = new byte[20480];
            int count;

            while ((count = src.Read(array, 0, array.Length)) != 0)
            {
                dest.Write(array, 0, count);
            }

            return true;
        }


        private bool SetPCLArgs(string filePath,string printer)
        {
            try
            {
                long posModify = GetInsertPCLPos(filePath);

                if (posModify == -1)
                {
                    return false;
                }

                string tmpName = filePath + ".tmp";
                FileInfo tmpFileInfo = new FileInfo(tmpName);
                if (tmpFileInfo.Exists)
                {
                    tmpFileInfo.Delete();
                }

                using (var newFile = new FileStream(tmpName, FileMode.CreateNew, FileAccess.Write))
                {
                    using (var oldFile = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte []buffer = new byte [posModify];
                        oldFile.Read(buffer, 0, buffer.Length);
                        newFile.Write(buffer, 0, buffer.Length);
                        newFile.Write(PCL_SET_BYTES, 0, PCL_SET_BYTES.Length);
                        if (this.m_userDuplex)
                        {
                            if(printer == "SHARP AR-M350U PCL6")
                            {
                                newFile.Write(DUP_ON_BYTES, 0, DUP_ON_BYTES.Length);
                                newFile.Write(PCL_SET_BYTES, 0, PCL_SET_BYTES.Length);
                                newFile.Write(DUP_BIND_BYTES_Short, 0, DUP_BIND_BYTES_Short.Length);
                            }
                            else
                            {
                            newFile.Write(DUP_ON_BYTES, 0, DUP_ON_BYTES.Length);
                            newFile.Write(PCL_SET_BYTES, 0, PCL_SET_BYTES.Length);
                            newFile.Write(DUP_BIND_BYTES, 0, DUP_BIND_BYTES.Length); 
                            }
                            
                        }
                        else
                        {
                            newFile.Write(DUP_OFF_BYTES, 0, DUP_OFF_BYTES.Length);
                        }

                        if (this.m_colorInfo)
                        {
                            newFile.Write(PCL_SET_BYTES, 0, PCL_SET_BYTES.Length);
                            newFile.Write(COLOR_BYTES, 0, COLOR_BYTES.Length);
                        }
                        else
                        {
                            newFile.Write(PCL_SET_BYTES, 0, PCL_SET_BYTES.Length);
                            newFile.Write(GRAY_BYTES, 0, GRAY_BYTES.Length);
                        } 

                        BinaryCopyTo(oldFile, newFile);
                        oldFile.Close();
                    }
                    newFile.Close();
                }

                // Copy temp file to dest file                
                FileInfo destFile = new FileInfo(filePath);
                FileInfo tmpFile = new FileInfo(tmpName);
                destFile.Delete();
                tmpFile.MoveTo(filePath);                
                

                //FileInfo objFileInfo = new FileInfo(filePath);
                //using (FileStream objFileStream = objFileInfo.OpenWrite())
                //{
                //    using (StreamWriter objStreamWriter = new StreamWriter(objFileStream))
                //    {
                //        objStreamWriter.BaseStream.Seek(posModify, SeekOrigin.Begin);                        
                //        objStreamWriter.Write("ON ");
                //        //objStreamWriter.WriteLine("@PJL SET BINDING=LONGEDGE");
                //        objStreamWriter.Close();
                //    }

                //    objFileStream.Close();
                //}
            }
            catch (System.Exception ex1)
            {
                Console.WriteLine(ex1.Message);
                LogHelper.WriteLog(this.GetType(), ex1.Message);
                return false;
            }

            return true;
        }

        //find pos -> @PJL ENTER LANGUAGE
        private long GetInsertPCLPos(string filePath)
        {
            long pos = -1;            
            try
            {
                FileInfo objFileInfo = new FileInfo(filePath);
                using (FileStream objFileStream = objFileInfo.OpenRead())
                {
                    using (BinaryReader objBR = new BinaryReader(objFileStream))
                    {
                        byte data;
                        int state = 0;
                        int count = 0;
                        long foundPos = -1;
                        while (true)
                        {
                            try
                            {
                                data = objBR.ReadByte();
                                count++;

                                // header zone 4k
                                if (count > 0x1000)
                                {
                                    break;
                                }

                                switch ((char)data)
                                {
                                    case '@':
                                        foundPos = objBR.BaseStream.Position;
                                        state = 1;
                                        break;

                                    case 'P':
                                    case 'J':
                                        if (state == 1)
                                        {
                                            state = 2;
                                        }
                                        break;

                                    case 'L':
                                        if (state == 1)
                                        {
                                            state = 2;
                                        }
                                        else if (state == 4)
                                        {
                                            state = 5;
                                        }
                                        break;

                                    case ' ':
                                        if (state == 2)
                                        {
                                            state = 3;
                                        }
                                        else if (state == 5)
                                        {
                                            state = 6;
                                        }
                                        break;

                                    case 'E':
                                    case 'N':
                                        if (state == 3)
                                        {
                                            state = 4;
                                        }
                                        else if (state == 4)
                                        {
                                            state = 5;
                                        }
                                        break;

                                    case 'T':
                                    case 'R':
                                        if (state == 3)
                                        {
                                            state = 4;
                                        }
                                        break;

                                    case 'A':
                                    case 'G':
                                    case 'U':
                                        if (state == 4)
                                        {
                                            state = 5;
                                        }
                                        break;

                                    default:
                                        state = 0;
                                        break;
                                }

                                if (state == 6)
                                {
                                    pos =  foundPos - 1;
                                    break;
                                }
                            }
                            catch (Exception ex11)
                            {
                                LogHelper.WriteLog(this.GetType(), ex11.Message);
                                Console.WriteLine(ex11.Message);
                                break;
                            }


                            //if (line.StartsWith("@PJL SET DUPLEX"))
                            //    return objBR.BaseStream.Position;
                            //if (line.StartsWith("@PJL ENTER LANGUAGE"))
                            //    return -1; 


                            //pos += line.ToCharArray().Length + 2;
                        }

                        objBR.Close();
                    }

                    objFileStream.Close();
                }
            }
            catch (System.Exception ex1)
            {
                LogHelper.WriteLog(this.GetType(), ex1.Message);
                Console.WriteLine(ex1.Message);
            }

            return pos;
        }

        private long GetDuplexPos(string filePath)
        {
            long pos = -1;
            try
            {
                FileInfo objFileInfo = new FileInfo(filePath);
                using (FileStream objFileStream = objFileInfo.OpenRead())
                {
                    using (BinaryReader objBR = new BinaryReader(objFileStream))
                    {
                        byte data;
                        int state = 0;
                        while (true)
                        {
                            try
                            {
                                data = objBR.ReadByte();
                                switch ((char)data)
                                {
                                    case '@':
                                        state = 1;
                                        break;

                                    case 'P':
                                        if (state == 1)
                                        {
                                            state = 2;
                                        }
                                        break;

                                    case 'J':
                                    case 'L':
                                    case ' ':
                                    case 'S':
                                    case 'E':
                                    case 'T':
                                    case 'D':
                                    case 'U':
                                    case 'X':
                                        if (state != 2)
                                        {
                                            state = 0;
                                        }
                                        break;
                                    case '=':
                                        if (state == 2)
                                        {
                                            state = 3;
                                        }
                                        else
                                        {
                                            state = 0;
                                        }
                                        break;

                                    default:
                                        state = 0;
                                        break;
                                }

                                if (state == 3)
                                {
                                    pos = objBR.BaseStream.Position;
                                    if (objBR.ReadChar() == ' ')
                                    {
                                        ++pos;
                                    }

                                    break;
                                }
                            }
                            catch (Exception ex11)
                            {
                                Console.WriteLine(ex11.Message);
                                break;
                            }


                            //if (line.StartsWith("@PJL SET DUPLEX"))
                            //    return objBR.BaseStream.Position;
                            //if (line.StartsWith("@PJL ENTER LANGUAGE"))
                            //    return -1; 


                            //pos += line.ToCharArray().Length + 2;
                        }

                        objBR.Close();
                    }

                    objFileStream.Close();
                }
            }
            catch (System.Exception ex1)
            {
                Console.WriteLine(ex1.Message);
            }

            return pos;
        }
        

        /// <summary>
        /// 获取PCL打印机，转换PCL文件需要PCL打印机驱动安装
        /// </summary>
        /// <returns></returns>
        //private string GetPCLPrinter()
        //{
        //    foreach (string pName in PrinterSettings.InstalledPrinters)
        //    {
        //        if (pName.IndexOf("PCL") != -1 || pName.IndexOf("pcl") != -1)
        //        {
        //            return pName;
        //        }
        //    }

        //    return null;
        //}

        /// <summary>
        /// 输出到PCL文件的执行函数(JPG用)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pdPrintImagePage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //获得绘制对象
            Graphics g = e.Graphics;

            if (m_allJpgFile.Count <= 0)
            {
                return;
            }

            string jpgFile = m_allJpgFile.ElementAt(0);
            if (string.IsNullOrEmpty(jpgFile))
            {
                return;
            }
            int w = e.PageBounds.Width /*- e.MarginBounds.Left*/;
            int h = e.PageBounds.Height /*- e.MarginBounds.Top*/;
            

            //convert not good , so set -1
          //  Image img = Image.FromFile(jpgFile); //zoomImage(m_jpgFile, w, h, -1);
                using (Image img = Image.FromFile(jpgFile)) {
                        System.Drawing.Rectangle rect = GetValidImageRect(img.Width, img.Height, G_Witdh, G_HEIGHT, 0);
                        //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, e.PageBounds.Width, e.PageBounds.Height);
                        g.DrawImage(img, rect);
                        m_allJpgFile.RemoveAt(0);
                }
                try
                {
                    //2016年5月16日10:53:45 改 不删除原图
                   // File.Delete(jpgFile);
                }
                catch (System.Exception ex)
                {

                }
            

            if (m_allJpgFile.Count > 0 )
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// 画像Rect
        /// </summary>
        /// <param name="imgW">画像宽度</param>
        /// <param name="imgH">画像高度</param>
        /// <param name="maxW">最大宽度</param>
        /// <param name="maxH">最大高度</param>
        /// <param name="type">转换类型TODO</param>
        /// <returns>转换后的Rect</returns>
        private System.Drawing.Rectangle GetValidImageRect(int imgW, int imgH, int maxW, int maxH, int type)
        {
            int newH = imgH, newW = imgW;
            if (newH <= maxH && newW <= maxW)
            {
                return new System.Drawing.Rectangle(0, 0, imgW, imgH);;
            }

            // 新的宽度大于最大宽度时，需要重新计算，保证比例不变
            if (newW > maxW)
            {
                newW = maxW;
                newH = imgH * newW / imgW;
            }
            
            // 新的高度大于最大高度时，需要重新计算，保证比例不变
            if (newH > maxH)
            {
                newH = maxH;
                newW = imgW * newH / imgH;
            }


            return new System.Drawing.Rectangle(0, 0, newW, newH);;
        }

        /// <summary>
        /// 画像缩放函数(TODO)
        /// </summary>
        /// <param name="imgFile">画像路径</param>
        /// <param name="maxW">最大宽度</param>
        /// <param name="maxH">最大高度</param>
        /// <param name="type">转换类型</param>
        /// <returns>转换后的画像</returns>
        private Image zoomImage(string imgFile, int maxW, int maxH, int type)
        {
            Image srcImg = Image.FromFile(imgFile);

            int newH = srcImg.Height, newW = srcImg.Width;
            if (newH <= maxH && newW <= maxW)
            {
                return srcImg;
            }

            if (newW > maxW)
            {
                newW = maxW;
            }

            if (newH > maxH)
            {
                newH = maxH;
            }

            if (newW > maxW)
            {
                newW = maxW;
                newH = srcImg.Height * newW / srcImg.Width;
            }

            if (newH > maxH)
            {
                newH = maxH;
                newW = srcImg.Width * newH / srcImg.Height;
            }


            switch (type)
            {
                case -1:
                default:
                    return srcImg;

                case 0:
                    return srcImg.GetThumbnailImage(newW, newH,
                        new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

            }
        }
        

        private bool ThumbnailCallback()
        {
            return false;
        }
        #endregion

    }

    /// <summary>
    /// PCL print event for result
    /// </summary>
    public class PCLEventArgs : EventArgs
    {
        private PrintJobStatus result;

        public PrintJobStatus Result
        {
            get
            {
                return this.result;
            }

            set
            {
                this.result = value;
            }
        }

        public string ToString()
        {
            return result.ToString();
        }

    }
    /// <summary>
    /// 发送PCL指令或者PCL文件给打印机
    /// </summary>
    public class PCLToPrinter
    {
        #region DLL引用设置
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        public class PRINTER_INFO_5
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pPrinterName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pPortName;

            public Int32 Attributes;
            public Int32 DeviceNotSelectedTimeout;
            public Int32 TransmissionRetryTimeout;

        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("winspool.Drv", EntryPoint = "EnumPrinters", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EnumPrinters(Int32 Flags, [MarshalAs(UnmanagedType.LPStr)] string Name, Int32 Level, [MarshalAs(UnmanagedType.LPStr)]PRINTER_INFO_5 pPrinterEnum, IntPtr cbBuf, out Int32 pcbNeeded, out Int32 pcReturned);
        #endregion

        #region 实现API
        /// <summary>
        /// 发送raw数据给打印机
        /// </summary>
        /// <param name="szPrinterName">打印机名</param>
        /// <param name="pBytes">数据</param>
        /// <param name="dwCount">数据长度</param>
        /// <returns>是否成功</returns>
        private string SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount, string szFileName)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = szFileName;
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
                return "打印错误code=" + dwError;
            }
            
            
            return null;            
        }

        /// <summary>
        /// 发送PCL文件给打印机
        /// </summary>
        /// <param name="szPrinterName">打印机名</param>
        /// <param name="szFileName">PCL文件</param>
        /// <returns>错误信息</returns>
        public PrintJobStatus GetPrintJobStat(string szFileName)
        {
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
                        if (job.Name == szFileName)
                        {
                            return job.JobStatus;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return PrintJobStatus.None;
                }
            }

            return PrintJobStatus.Completed;
        }

        private void ThreadJob(object obj)
        {
            int sleepTime = m_timeout;
            Thread.Sleep(1000);
            string file = obj.ToString();

            PrintJobStatus st = GetPrintJobStat(file);
            PCLEventArgs e = new PCLEventArgs();
            try
            {
                while (true)
                {
                    if (sleepTime != 100)
                    {
                        e.Result = st;
                        OnPCLResult(this, e);
                    }

                    if ((st & PrintJobStatus.Completed) == PrintJobStatus.Completed)
                    {                        
                        break;
                    }
                    else if ((st & PrintJobStatus.Error) == PrintJobStatus.Error)
                    {
                        Console.WriteLine(st.ToString());
                        break;
                    }
                    else if ((st & PrintJobStatus.Deleted) == PrintJobStatus.Deleted)
                    {
                        Console.WriteLine(st.ToString());
                        break;
                    }
                    else if ((st & PrintJobStatus.Deleting) == PrintJobStatus.Deleting && sleepTime != 100)
                    {
                        sleepTime = 100;
                        e.Result = st;
                        OnPCLResult(this, e);
                    }

                    Thread.Sleep(sleepTime);
                    st = GetPrintJobStat(file);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Mornitor Thread exit ---- st:" + st.ToString());
                e.Result = st;
                OnPCLResult(this, e);
            }
            
        }

        public void StartMornitor(string szFileName)
        {
            this.m_fileName = szFileName;

            object name = szFileName;

            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(ThreadJob);
            Thread t = new Thread(ParStart);
            t.Start(name);
        }

        /// <summary>
        /// 发送PCL文件给打印机
        /// </summary>
        /// <param name="szPrinterName">打印机名</param>
        /// <param name="szFileName">PCL文件</param>
        /// <returns>错误信息</returns>
        public string SendFileToPrinter(string szPrinterName, string szFileName)
        {
            string allPrinters = GetAllPrinter();
            String error_msg = null;
           /* if (string.IsNullOrEmpty(szPrinterName))            
            {
                SelectPrinter sp = new SelectPrinter();
                sp.SetPrinters(GetAllPrinter());
                sp.ShowDialog();
                if (string.IsNullOrEmpty(sp.printerSelect))
                {
                    return "打印取消";
                }

                szPrinterName = sp.printerSelect;
            }*/


            if(allPrinters.IndexOf(szPrinterName) == -1)
            {
                error_msg = "打印机设置错误";
                LogHelper.WriteLog(this.GetType(), error_msg);
                return "打印机设置错误";
            }

            if (!szFileName.ToLower().EndsWith("prn"))
            {
                error_msg = "非pcl(.prn)文件: " + szFileName;
                LogHelper.WriteLog(this.GetType(), error_msg);
                return "非pcl(.prn)文件: " + szFileName;
            }

            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = szFileName;
            di.pDataType = "RAW";

            string msg = "";
            int onceLen = 5*1024 * 1024;
            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                try                    
                {
                    // Start a document.
                    if (StartDocPrinter(hPrinter, 1, di))
                    {
                        // Start a page.
                        if (StartPagePrinter(hPrinter))
                        {
                            StartMornitor(szFileName);

                                // Open the file.
                                FileStream fs = new FileStream(szFileName, FileMode.Open);
                                // Create a BinaryReader on the file.
                               

                            long fsLen = fs.Length;

                            BinaryReader br = new BinaryReader(fs);
                            Byte[] bytes = new Byte[onceLen];
                            IntPtr pUnmanagedBytes = new IntPtr(0);

                            while (fsLen > onceLen)
                            {
                                try
                                {
                                    bytes = br.ReadBytes(onceLen);
                                    // Allocate some unmanaged memory for those bytes.
                                    pUnmanagedBytes = Marshal.AllocCoTaskMem(onceLen);
                                    // Copy the managed byte array into the unmanaged array.
                                    Marshal.Copy(bytes, 0, pUnmanagedBytes, onceLen);
                                    // Send the unmanaged bytes to the printer.

                                    // Write your bytes.
                                    bSuccess = WritePrinter(hPrinter, pUnmanagedBytes, onceLen, out dwWritten);

                                    Marshal.FreeCoTaskMem(pUnmanagedBytes);
                                }
                                catch (System.Exception ex2)
                                {
                                    LogHelper.WriteLog(this.GetType(),ex2.Message);
                                    msg = ex2.Message;
                                }

                                fsLen -= onceLen;
                            }

                            if (string.IsNullOrEmpty(msg))
                            {
                                try
                                {
                                    int leftLen = (int)fsLen;
                                    bytes = br.ReadBytes(leftLen);
                                    // Allocate some unmanaged memory for those bytes.
                                    pUnmanagedBytes = Marshal.AllocCoTaskMem(leftLen);
                                    // Copy the managed byte array into the unmanaged array.
                                    Marshal.Copy(bytes, 0, pUnmanagedBytes, leftLen);
                                    // Send the unmanaged bytes to the printer.

                                    // Write your bytes.
                                    bSuccess = WritePrinter(hPrinter, pUnmanagedBytes, leftLen, out dwWritten);

                                    Marshal.FreeCoTaskMem(pUnmanagedBytes);
                                }

                                catch (System.Exception ex2)
                                {
                                    msg = ex2.Message;
                                }
                            }


                            fs.Close();
                            EndPagePrinter(hPrinter);
                        }

                        EndDocPrinter(hPrinter);


                    }
                }
                catch (System.Exception ex)
                {
                    msg = ex.Message;
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.

            if (!string.IsNullOrEmpty(msg))
            {
                return msg;
            }

            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
                return "打印错误code=" + dwError;
            }

            return null;   
            
        }

        public static string GetAllPrinter()
        {
            StringBuilder sb = new StringBuilder();

            foreach (string pName in PrinterSettings.InstalledPrinters)
            {
                sb.Append(pName);
                sb.Append(";");
            }

            return sb.ToString();
        }

      /*  public string ShowSelectPrinter()
        {
            SelectPrinter sp = new SelectPrinter();
            sp.SetPrinters(GetAllPrinter());
            sp.ShowDialog();
            if (string.IsNullOrEmpty(sp.printerSelect))
            {
                return null;
            }

            return sp.printerSelect;
        }*/

        /// <summary>
        /// OnPCLResult事件发生时间间隔
        /// </summary>
        /// <param name="t">事件发生时间间隔，默认1秒</param>
        public void SetEventTimeout(int time)
        {
            m_timeout = time;
        }

        #endregion

        public delegate void PCLResult(object sender, PCLEventArgs e);
        public event PCLResult OnPCLResult;
        private string m_fileName;
        private int m_timeout = 1000;
    }

    public class PDFConvert  
    {  
        #region GhostScript Import  
        /// <summary>Create a new instance of Ghostscript. This instance is passed to most other gsapi functions. The caller_handle will be provided to callback functions.  
        ///  At this stage, Ghostscript supports only one instance. </summary>  
        /// <param name="pinstance"></param>  
        /// <param name="caller_handle"></param>  
        /// <returns></returns>  
        [DllImport("gsdll32.dll", EntryPoint="gsapi_new_instance")]  
        private static extern int gsapi_new_instance (out IntPtr pinstance, IntPtr caller_handle);  
        /// <summary>This is the important function that will perform the conversion</summary>  
        /// <param name="instance"></param>  
        /// <param name="argc"></param>  
        /// <param name="argv"></param>  
        /// <returns></returns>  
        [DllImport("gsdll32.dll", EntryPoint="gsapi_init_with_args")]  
        private static extern int gsapi_init_with_args (IntPtr instance, int argc, IntPtr argv);  
        /// <summary>  
        /// Exit the interpreter. This must be called on shutdown if gsapi_init_with_args() has been called, and just before gsapi_delete_instance().   
        /// </summary>  
        /// <param name="instance"></param>  
        /// <returns></returns>  
        [DllImport("gsdll32.dll", EntryPoint="gsapi_exit")]  
        private static extern int gsapi_exit (IntPtr instance);  
        /// <summary>  
        /// Destroy an instance of Ghostscript. Before you call this, Ghostscript must have finished. If Ghostscript has been initialised, you must call gsapi_exit before gsapi_delete_instance.   
        /// </summary>  
        /// <param name="instance"></param>  
        [DllImport("gsdll32.dll", EntryPoint="gsapi_delete_instance")]  
        private static extern void gsapi_delete_instance (IntPtr instance);  
        #endregion  

        #region Variables  
        private string _sDeviceFormat;  
        private int _iWidth;  
        private int _iHeight;  
        private int _iResolutionX;  
        private int _iResolutionY;  
        private int _iJPEGQuality;  
        private Boolean _bFitPage;  
        private IntPtr _objHandle;

        private string m_sysTmpPath;
        #endregion  

        #region Proprieties  
        public string OutputFormat  
        {  
            get { return _sDeviceFormat; }  
            set { _sDeviceFormat = value; }  
        }  
        public int Width  
        {  
            get { return _iWidth; }  
            set { _iWidth = value; }  
        }  
        public int Height  
        {  
            get { return _iHeight; }  
            set { _iHeight = value; }  
        }  
        public int ResolutionX  
        {  
            get { return _iResolutionX; }  
            set { _iResolutionX = value; }  
        }  
        public int ResolutionY  
        {  
            get { return _iResolutionY; }  
            set { _iResolutionY = value; }  
        }  
        public Boolean FitPage  
        {  
            get { return _bFitPage; }  
            set { _bFitPage = value; }  
        }  
        /// <summary>Quality of compression of JPG</summary>  
        public int JPEGQuality  
        {  
            get { return _iJPEGQuality; }  
            set { _iJPEGQuality = value; }  
        }  
        #endregion  

        #region Init  
        public PDFConvert(IntPtr objHandle)  
        {  
            _objHandle = objHandle;

            try
            {
                m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
            }
            catch (System.Exception ex)
            {
                try
                {
                    m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
                }
                catch (System.Exception ex2)
                {
                    m_sysTmpPath = "c:\\windows\\PCL-CVT";
                }
            }

            try
            {
                System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(m_sysTmpPath);
                tmp.Delete(false);
            }
            catch (System.Exception ex)
            {
            	
            }

        }  
        public PDFConvert(bool showUI)  
        {
            m_showUI = showUI;
            m_userCopies = 1;
            m_userPrintFirstPage = 0;
            m_userPrintLastPage = 0;
            m_userDuplex = false;

            _objHandle = IntPtr.Zero;
            try
            {
                m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
            }
            catch (System.Exception ex)
            {
                try
                {
                    m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
                }
                catch (System.Exception ex2)
                {
                    m_sysTmpPath = "c:\\windows\\PCL-CVT";
                }
            }

            try
            {
                System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(m_sysTmpPath);
                tmp.Delete(true);
            }
            catch (System.Exception ex)
            {

            }
        }

        public PDFConvert()
        {
            m_showUI = true;
            m_userCopies = 1;
            m_userPrintFirstPage = 0;
            m_userPrintLastPage = 0;
            m_userDuplex = false;

            _objHandle = IntPtr.Zero;
            try
            {
                m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
            }
            catch (System.Exception ex)
            {
                try
                {
                    m_sysTmpPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\PCL_CVT";
                }
                catch (System.Exception ex2)
                {
                    m_sysTmpPath = "c:\\windows\\PCL-CVT";
                }
            }

            try
            {
                System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(m_sysTmpPath);
                tmp.Delete(true);
            }
            catch (System.Exception ex)
            {

            }
        }  
        #endregion  

        #region 实现API
        /// <summary>
        /// 转后string到字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private byte[] StringToAnsiZ(string str)  
        {  
            //' Convert a Unicode string to a null terminated Ansi string for Ghostscript.  
            //' The result is stored in a byte array. Later you will need to convert  
            //' this byte array to a pointer with GCHandle.Alloc(XXXX, GCHandleType.Pinned)  
            //' and GSHandle.AddrOfPinnedObject()  
            int intElementCount;  
            int intCounter;  
            byte[] aAnsi;  
            byte bChar;  
            intElementCount = str.Length;  
            aAnsi = new byte[intElementCount+1];  
            for(intCounter = 0; intCounter < intElementCount;intCounter++)  
            {  
                bChar = (byte)str[intCounter];  
                aAnsi[intCounter] = bChar;  
            }  
            aAnsi[intElementCount] = 0;  
            return aAnsi;
        }
        
      

        /// <summary>
        ///   Convert(@"d:\\test.pdf", @"d:\\test",1,2,"jpeg",600,600);  
        /// </summary>
        /// <param name="inputFile">要转换的pdf文件</param>
        /// <param name="outPath">转换后存储路径</param>
        /// <param name="firstPage">转换的第一页码</param>
        /// <param name="lastPage">转换的最后页码</param>
        /// <param name="deviceFormat">转后文件格式</param>
        /// <param name="xRes">X解像度</param>
        /// <param name="yRes">Y解像度</param>
        private int m_copies = 1;
        private string m_pages = "";
        private int m_lastPage = 0;
        private int m_firstPage = 0;
        private bool m_cancel = false;
        private bool isDouble = false;
        public string m_fileID;
        public bool color=false;
         public void TaskPDF(List<Client_simplify.lists.Class_PrintArgs> c)
        {
            Client_simplify.lists.Class_PrintArgs r = c[0];
            m_copies = r.Copies;
            isDouble = r.IsDuplex;
            m_pages = r.Pages;
            color = r.Color;
        }

        public bool getDuplexSet()
        {
            // return this.checkBoxDuplex.Checked;
            return isDouble;
        }

        public int getCopiesNum()
        {
            return m_copies;
        }

        public int getFirstPage()
        {
            return m_firstPage;
        }
        public int getLastPage()
        {
            return m_lastPage;
        }
        public string getPages()
        {
            return m_pages;
        }
        public bool getColorInfo()
        {
            return color;
        }
        private void checkPrintPages()
        {
            if (m_pages == "全部")
            {
                this.m_firstPage = 0;
                this.m_lastPage = 0;
                this.m_pages = "";

            }
            else
            {
                //int index = this.textBoxPages.Text.IndexOf('-');
                int index = this.m_pages.IndexOf('-');
                if (index == -1)
                {
                    try
                    {
                        this.m_firstPage = int.Parse(m_pages.Trim());
                        this.m_lastPage = this.m_firstPage;
                        this.m_pages = m_pages.Trim();



                    }
                    catch
                    {
                        MessageBox.Show("页码设置错误");
                    }
                }
                else
                {                  
                    string first = this.m_pages.Substring(0, index).Trim();                   
                    string last = this.m_pages.Substring(index + 1).Trim();

                    try
                    {
                        this.m_firstPage = int.Parse(first);
                        this.m_lastPage = int.Parse(last);
                        this.m_pages = this.m_firstPage + "-" + this.m_lastPage;
                    }
                    catch
                    {
                        MessageBox.Show("页码设置错误");
                    }
                }
            }
        }

        private string Convert1(string inputFile, string outPath, string driver,
            int firstPage, int lastPage, string deviceFormat, int xRes, int yRes, bool isPath, List<Client_simplify.lists.Class_PrintArgs> c)  
        {  
            //Avoid to work when the file doesn't exist  
            if (!System.IO.File.Exists(inputFile))  
            {
                return "要转换的pdf文件不存在";  
            }  

            int intReturn;  
            IntPtr intGSInstanceHandle;  
            object[] aAnsiArgs;  
            IntPtr[] aPtrArgs;  
            GCHandle[] aGCHandle;  
            int intCounter;  
            int intElementCount;  
            IntPtr callerHandle;  
            GCHandle gchandleArgs;  
            IntPtr intptrArgs;

            string[] sArgs;
            
            if (driver.Equals("pxlmono"))
            {
                if (this.m_showUI)
                {
                   /* SetPrinter dlg = new SetPrinter();
                    dlg.showPrinterSet();
                    if (dlg.getDialogResult() == DialogResult.Cancel)
                    {
                        return "取消转换";
                    }*/
                    TaskPDF(c);
                    checkPrintPages();
                    this.m_userCopies = this.getCopiesNum();
                    this.m_userDuplex = this.getDuplexSet();
                    this.m_colorInfo = this.getColorInfo();
                    this.m_userPrintFirstPage = this.getFirstPage();
                    this.m_userPrintLastPage  = this.getLastPage();
                }
                


                sArgs = ArgsInitForPCL(inputFile, outPath,
                    this.m_userPrintFirstPage,
                    this.m_userPrintLastPage,
                    this.m_userCopies,
                    this.m_userDuplex,
                    this.m_colorInfo);
            }            
            else
            {

                sArgs = GetGeneratedArgs(inputFile, outPath, driver,
                    firstPage, lastPage, deviceFormat, xRes, yRes, isPath);
            }  
            // Convert the Unicode strings to null terminated ANSI byte arrays  
            // then get pointers to the byte arrays.  
            intElementCount = sArgs.Length;  
            aAnsiArgs = new object[intElementCount];  
            aPtrArgs = new IntPtr[intElementCount];  
            aGCHandle = new GCHandle[intElementCount];  
            // Create a handle for each of the arguments after   
            // they've been converted to an ANSI null terminated  
            // string. Then store the pointers for each of the handles  
            for(intCounter = 0; intCounter< intElementCount; intCounter++)  
            {  
                aAnsiArgs[intCounter] = StringToAnsiZ(sArgs[intCounter]);  
                aGCHandle[intCounter] = GCHandle.Alloc(aAnsiArgs[intCounter], GCHandleType.Pinned);  
                aPtrArgs[intCounter] = aGCHandle[intCounter].AddrOfPinnedObject();  
            }  
            // Get a new handle for the array of argument pointers  
            gchandleArgs = GCHandle.Alloc(aPtrArgs, GCHandleType.Pinned);  
            intptrArgs = gchandleArgs.AddrOfPinnedObject();

            intReturn = gsapi_new_instance(out intGSInstanceHandle, _objHandle); 
            callerHandle = IntPtr.Zero;  
            try
            { 
                intReturn = gsapi_init_with_args(intGSInstanceHandle, intElementCount, intptrArgs);  
            }  
            catch (Exception ex)  
            {  
                return ex.Message;  
                  
            }  
            finally  
            {  
                for (intCounter = 0; intCounter < intReturn; intCounter++)  
                {  
                    aGCHandle[intCounter].Free();  
                }  
                gchandleArgs.Free();  
                gsapi_exit(intGSInstanceHandle);  
                gsapi_delete_instance(intGSInstanceHandle);  
            }

            return null;
        }
        
        private void ConvertToImg(string inputFile, string outFile,
            int firstPage, int lastPage, int xRes, int yRes, List<Client_simplify.lists.Class_PrintArgs> c)
        {

            string driver = "pdf2img";
            Convert1(inputFile, outFile, driver, 1, 9999999, "jpeg", xRes, yRes, false,c);
        }

        public string ConvertToPCL(string inputFile, string outFile, List<Client_simplify.lists.Class_PrintArgs> c)
        {

            string driver = "pdf2img";

            if (inputFile == null || !inputFile.ToLower().EndsWith("pdf"))
            {
                return "非PDF文件" + inputFile;
            }


            string outPath =  m_sysTmpPath + "\\TMP" + System.DateTime.Now.ToString("MMddhhmmss");
                                    

            System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(outPath);
            tmp.Create();
            try
            {
                string ret = Convert1(inputFile, outPath, driver, 1, 9999999, "jpeg", 300, 300, true,c);

                if (ret != null)
                {
                    tmp.Delete(true);
                    return ret;
                }
            }
            catch (System.Exception ex)
            {

                tmp.Delete(true);
                return ex.Message;            
            }

            

            PCLConvertCS.PCLConvert pc = new PCLConvertCS.PCLConvert();
            string err = pc.MultiJPGToPCL(inputFile, outPath, outFile);

            try
            {
                tmp.Delete(true);
            }
            catch (System.Exception ex)
            {                
            
            }            
            return err;
        }
        public string ConvertPdf2pcl(List<Client_simplify.lists.Class_PrintArgs> c, string outFile)
        {
            Client_simplify.lists.Class_PrintArgs a = c[0];
            string inputFile = a.Filepath;
            if (inputFile == null || !inputFile.ToLower().EndsWith("pdf"))
            {
                return "非PDF文件" + inputFile;
            }
            string outPath = m_sysTmpPath + "\\TMP" + System.DateTime.Now.ToString("MMddhhmmss");
            System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(outPath);
            tmp.Create();
            try
            {
                string tmpIn = tmp + "\\"+Guid.NewGuid().ToString()+"ABBDD.pdf";

                string tmpOutFile = tmpIn + ".prn";

                File.Copy(inputFile, tmpIn, true);

                string ret = Convert1(tmpIn, tmpOutFile, "pxlmono", 1, 9999999, "pxlcolor", 300, 300, false, c);

                if (ret != null)
                {
                    return ret;
                }

                int count = 5;
                long lastLength = -1;
                while (true)
                {

                    FileInfo fi = new FileInfo(tmpOutFile);
                    if (fi.Exists)
                    {
                        if (fi.Length != lastLength)
                        {
                            lastLength = fi.Length;
                        }
                        File.Copy(tmpOutFile, outFile, true);
                        break;
                    }
                    else {
                        --count;
                        Thread.Sleep(600);
                    }
                    if (count < 0)
                    {
                        break;
                    }
                }

              
            }
            catch (System.Exception ex)
            {
                // return ex.Message;
                MessageBox.Show(ex.Message);
            }

            FileInfo FILEOUT = new FileInfo(outFile);
            if (FILEOUT.Exists)
            {
                return null;
            }
            else {
                return "文件不存在";
            }
          
        }
        public string ConvertPdf2pcl_v2(List<Client_simplify.lists.Class_PrintArgs> c, string outFile)
        {
            Client_simplify.lists.Class_PrintArgs a = c[0];
            string inputFile = a.Filepath;
            if (inputFile == null || !inputFile.ToLower().EndsWith("pdf"))
            {
                return "非PDF文件" + inputFile;
            }
            string outPath =  m_sysTmpPath + "\\TMP" + System.DateTime.Now.ToString("MMddhhmmss");
            System.IO.DirectoryInfo tmp = new System.IO.DirectoryInfo(outPath);
            tmp.Create();
#if true
            string tmpIn = tmp + "\\" + Path.GetFileName(inputFile).Replace(".","_") + ".pdf";

            string tmpOutFile = tmpIn.Replace(".", "_") + ".prn";

            File.Copy(inputFile, tmpIn, true);
#else


            string tmpIn = inputFile;

            string tmpOutFile = tmpIn + ".prn";

#endif
            try
            {
                

                string ret = Convert1(tmpIn, tmpOutFile, "pxlmono", 1, 9999999, "pxlcolor", 300, 300, false, c);

                if (ret != null)
                {
                    return ret;
                }

                int count = 5;
                long lastLength = -1;
                while (true)
                {

                    FileInfo fi = new FileInfo(tmpOutFile);
                    if (fi.Exists)
                    {
                        if (fi.Length != lastLength)
                        {
                            lastLength = fi.Length;
                            count = 5;
                        }
                        else
                        {
                            if (--count < 0)
                            {
                                break;
                            }
                        }
                    }

                    Thread.Sleep(600);
                }

                File.Copy(tmpOutFile, outFile, true);
                //File.Delete(tmpIn);
            }
            catch (System.Exception ex)
            {
                // return ex.Message;
                MessageBox.Show(ex.Message);
            }
            finally {
            
            
            }


            return null;
        }
  

        /// <summary>
        /// 转后string到字节
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string[] ArgsInitForPCL(string inFile, string outFile,
            int firstPage, int lastPage, int copies, bool duplex, bool isColor)
        {
            Console.WriteLine(firstPage + "-" + lastPage);

            int index = 0;
            
            string[] gsargv = new string[128];

            string duplexSet = "false";
            if (duplex)
            {
                duplexSet = "true";
            }

            string pxl = "pxlcolor";
            if (!isColor)
            {
                pxl = "pxlmono";
            }
                               
            gsargv[index++] = "pxlmono";
            gsargv[index++] = "-dNOPAUSE";
            gsargv[index++] = "-dBATCH";
            gsargv[index++] = "-dSAFER";
            //gsargv[index++] = "-sDEVICE=ps2write";
            gsargv[index++] = string.Format("-sDEVICE={0}", pxl);//"-sDEVICE=pxlcolor";
            gsargv[index++] = string.Format("-dDuplex={0}", duplexSet); //"-dDuplex=true";
            gsargv[index++] = "-dDoNumCopies=true";
            gsargv[index++] = string.Format("-dNumCopies={0}", copies); //"-dNumCopies=1";


            //gsargv[index++] = string.Format("-dGraphicsAlphaBits={0}", colorInfo);
            //gsargv[index++] = string.Format("-dGraphicsAlphaBits={0}", colorInfo);

            gsargv[index++] = string.Format("-sOutputFile={0}", outFile);

            if (firstPage > 0 && lastPage > 0)
            {
                gsargv[index++] = string.Format("-dFirstPage={0}", firstPage);
                gsargv[index++] = string.Format("-dLastPage={0}", lastPage); 
            } 

            gsargv[index++] = "-c";
            gsargv[index++] = ".setpdfwrite";
            gsargv[index++] = "-f";
            gsargv[index++] = string.Format("{0}", inFile);
            

            string[] initArgs = new string[index];

            while (--index >= 0)
            {
                initArgs[index] = gsargv[index];
            }

            return initArgs;

        }  

        /// <summary>
        /// 配置GS参数
        /// </summary>
        /// <param name="inputFile">输入文件</param>
        /// <param name="outPath">输出文件</param>
        /// <param name="firstPage">第一页码</param>
        /// <param name="lastPage">最后页码</param>
        /// <param name="xRes">X解像度</param>
        /// <param name="yRes">Y解像度</param>
        /// <returns>GS用参数</returns>
        private string[] GetGeneratedArgs(string inputFile, string outPath, string driver, 
            int firstPage, int lastPage, string deviceFormat, int xRes, int yRes, bool isPath)  
        {  
            this._sDeviceFormat = deviceFormat;
            // Count how many extra args are need - HRangel - 11/29/2006, 3:13:43 PM  
            ArrayList lstExtraArgs = new ArrayList();  
            if ( _sDeviceFormat=="jpg" && _iJPEGQuality > 0 && _iJPEGQuality < 101)  
                lstExtraArgs.Add("-dJPEGQ=" + _iJPEGQuality);  
            if (_iWidth > 0 && _iHeight > 0)  
                lstExtraArgs.Add("-g" + _iWidth + "x" + _iHeight);  
            if (_bFitPage)  
                lstExtraArgs.Add("-dPDFFitPage");

            //至少300dpi，否则转后不清晰
            if (xRes > 300 || yRes > 300)  
            {  
                lstExtraArgs.Add("-r600");  
            }
            else
            {
                lstExtraArgs.Add("-r300");  
            }

            // Load Fixed Args - HRangel - 11/29/2006, 3:34:02 PM  
            int iFixedCount = 17;  
            int iExtraArgsCount = lstExtraArgs.Count;  
            string[] args = new string[iFixedCount + lstExtraArgs.Count];  


            /* 
            // Keep gs from writing information to standard output 
        "-q",                      
        "-dQUIET", 
        
        "-dPARANOIDSAFER", // Run this command in safe mode 
        "-dBATCH", // Keep gs from going into interactive mode 
        "-dNOPAUSE", // Do not prompt and pause for each page 
        "-dNOPROMPT", // Disable prompts for user interaction            
        "-dMaxBitmap=500000000", // Set high for better performance 
         
        // Set the starting and ending pages 
        String.Format("-dFirstPage={0}", firstPage), 
        String.Format("-dLastPage={0}", lastPage),    
         
        // Configure the output anti-aliasing, resolution, etc 
        "-dAlignToPixels=0", 
        "-dGridFitTT=0", 
        "-sDEVICE=jpeg", 
        "-dTextAlphaBits=4", 
        "-dGraphicsAlphaBits=4", 
            */  
            args[0]= driver;//this parameter have little real use  
            args[1]="-dNOPAUSE";//I don't want interruptions  
            args[2]="-dBATCH";//stop after  
            args[3]="-dSAFER";
            //args[3] = "-dPARANOIDSAFER";  
            args[4]="-sDEVICE="+_sDeviceFormat;//what kind of export format i should provide  
            args[5] = "-q";  
            args[6] = "-dQUIET";  
            args[7] = "-dNOPROMPT";  
            args[8] = "-dMaxBitmap=500000000";
            args[9] =  String.Format("-dFirstPage={0}", firstPage);
            args[10] = String.Format("-dLastPage={0}", lastPage);  
            args[11] = "-dAlignToPixels=0";  
            args[12] = "-dGridFitTT=0";  
            args[13] = "-dTextAlphaBits=4";  
            args[14] = "-dGraphicsAlphaBits=4";  
            //For a complete list watch here:  
            //http://pages.cs.wisc.edu/~ghost/doc/cvs/Devices.htm  
            //Fill the remaining parameters  
            for (int i=0; i < iExtraArgsCount; i++)  
            {  
                args[15+i] = (string) lstExtraArgs[i];  
            }  
            //Fill outputfile and inputfile  
            //"-sOutputFile=d:\\temp\\form-%02d.jpg",
            if (isPath)
            {
                outPath += "\\pdftmp-%06d.jpg";
            }
            
            args[15 + iExtraArgsCount] = string.Format("-sOutputFile={0}", outPath);  
            args[16 + iExtraArgsCount] = string.Format("{0}",inputFile);  
            return args;
        }
        #endregion

        #region 打印参数设置，当设定非UI显示时，可以通过下面接口实现打印参数的设置

        private bool m_showUI;
        private int m_userCopies = 1;
        private int m_userPrintFirstPage = 0;
        private int m_userPrintLastPage = 0;
        private bool m_userDuplex = false;
        private bool m_colorInfo = true;

        /// <summary>
        /// 设置转换时打印参数
        /// </summary>
        /// <param name="page">转换时打印参数</param>
        /// <returns>bool</returns>
        public bool SetPrintParam(bool duplex, int copies, int firstPage, int lastPage, bool color)
        {
            SetDuplex(duplex);
            SetColorInfo(color);

            if (!SetCopies(copies))
            {
                return false;
            }
            if (!SetFirstPage(firstPage))
            {
                return false;
            }
            if (!SetLastPage(lastPage))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置彩色打印
        /// </summary>
        /// <param name="page">双面打印</param>
        /// <returns>void</returns>
        public void SetColorInfo(bool isColor)
        {
            this.m_colorInfo = isColor;
        }

        /// <summary>
        /// 设置双面打印
        /// </summary>
        /// <param name="page">双面打印</param>
        /// <returns>void</returns>
        public void SetDuplex(bool isEnable)
        {
            this.m_userDuplex = isEnable;
        }

        /// <summary>
        /// 设置打印份数
        /// </summary>
        /// <param name="page">打印份数</param>
        /// <returns>bool</returns>
        public bool SetCopies(int copies)
        {
            if (copies > 0)
            {
                this.m_userCopies = copies;

                return true;
            }

            return false;
        }


        /// <summary>
        /// 设置打印页码第一页
        /// </summary>
        /// <param name="page">打印页码</param>
        /// <returns>bool</returns>
        public bool SetFirstPage(int page)
        {
            if (page > 0)
            {
                if (this.m_userPrintLastPage < page || m_userPrintLastPage == 0)
                {
                    this.m_userPrintLastPage = page;
                }

                this.m_userPrintFirstPage = page;

                return true;
            } 
            else if (page == 0)
            {
                this.m_userPrintFirstPage = 0;
                this.m_userPrintLastPage = 0;    

                return true;
            }

            return false;
        }


        /// <summary>
        /// 设置打印页码最后一页
        /// </summary>
        /// <param name="page">打印页码</param>
        /// <returns>bool</returns>
        public bool SetLastPage(int page)
        {
            if (page > 0)
            {
                if (this.m_userPrintFirstPage > page)
                {
                    this.m_userPrintFirstPage = page;
                }
                else if (m_userPrintFirstPage == 0)
                {
                    this.m_userPrintFirstPage = 1;
                }

                this.m_userPrintLastPage = page;

                return true;
            }
            else if (page == 0)
            {
                this.m_userPrintFirstPage = 0;
                this.m_userPrintLastPage = 0;              

                return true;
            }

            return false;
        }
        #endregion
    }  
}
