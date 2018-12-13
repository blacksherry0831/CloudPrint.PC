using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Classlib;
using ConsoleApplication2;
using ConsoledownloadFile;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Threading;
using Client_simplify.lists;
using printActiveX;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using System.Diagnostics;
using System.Web.Script.Serialization;
using terminal.lib;
using Newtonsoft.Json;


namespace Client.Classlib
{
    class HttpRsepData
    {
        public string RespData;
        public string RespStatus;
        public int RespStatusCode;
        public HttpRsepData(
              string RespData,
              string RespStatus,
              int RespStatusCode)
        {
            this.RespData = RespData;
            this.RespStatus = RespStatus;
            this.RespStatusCode = RespStatusCode;
        }
        public HttpRsepData()
        {

        }
    }
    class CloudCommunication
    {
        public static String HttpOK = "OK";
        public static String HttpNotFing = "NotFound";
        public static String url_part_MachineCapability = @"pCloud/MachineCapability";
        String c_userID, c_licenseCode, c_password;
        /******************************************************************************/
        String url_GetTask;
        /******************************************************************************/
        public CloudCommunication(
            String url_GetTask,
            String password,
            String id,
            String license)
        {

            this.url_GetTask = url_GetTask;
            /////////////////////////////
            this.c_licenseCode = license;
            this.c_password = password;
            this.c_userID = id;
        }
        public int mnum;
        public int num;//显示多少页
        private int m_lastPage = 0;
        private int m_firstPage = 0;
        private string m_pages = "";
        public string url="";
        private bool XLSConvertToPDF(string sourcePath, string targetPath)
        {
            //  MessageBox.Show(sourcePath+"\n"+targetPath);

            bool result = false;
            Microsoft.Office.Interop.Excel.XlFixedFormatType targetType = Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF;
            object missing = Type.Missing;
            Microsoft.Office.Interop.Excel.ApplicationClass application = null;
            Microsoft.Office.Interop.Excel.Workbook workBook = null;
            try
            {
                application = new Microsoft.Office.Interop.Excel.ApplicationClass();
                object target = targetPath;
                object type = targetType;
                workBook = application.Workbooks.Open(sourcePath, missing, missing, missing, missing, missing,
                        missing, missing, missing, missing, missing, missing, missing, missing, missing);

                workBook.ExportAsFixedFormat(targetType, target, Microsoft.Office.Interop.Excel.XlFixedFormatQuality.xlQualityStandard, true, false, missing, missing, missing, missing);
                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                result = false;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true, missing, missing);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;
        }
        public void countpage(string printPCLFile)
        {
            string docFile = printPCLFile;
            string msg = "";
            try
            {

                if (docFile.ToLower().EndsWith("pdf"))
                {
                    iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(docFile);
                    int num = reader.NumberOfPages;

                    msg = num + "页";
                    mnum = num;
                }
                if (docFile.ToLower().EndsWith("jpg")) mnum =1;
                if (docFile.ToLower().EndsWith("xlsx") || docFile.ToLower().EndsWith("xlsm"))
                {
                    string path = System.IO.Directory.GetCurrentDirectory();
                    string sPath = path + "\\temp\\xlsx";

                    // MessageBox.Show(docFile+"\n"+ sPath, "sPath");
                    if (XLSConvertToPDF(docFile, sPath))
                    {
                        string pdffile = sPath + ".pdf";
                        // MessageBox.Show(pdffile,"pdffile");
                        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(pdffile);
                        int num = reader.NumberOfPages;
                        msg = num + "页";
                        mnum = num;
                    }

                }
                if (docFile.ToLower().EndsWith("doc") || docFile.ToLower().EndsWith("docx"))
                {
                    //this.textFilename.Text = msg;
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
                    num = doc.ComputeStatistics(stat, ref oMissing);//.ComputeStatistics(stat, ref Nothing); 
                    //打印完关闭WORD文件
                    doc.Close(ref doNotSaveChanges, ref oMissing, ref oMissing);
                    //退出WORD程序
                    appWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                    doc = null;
                    appWord = null;
                    msg = num + "页";
                    mnum = num;
                }

            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
            }

        }
        private bool checkPrintPages(string page)
        {
            if (true)
            {
                int index = page.IndexOf('-');
                if (index == -1)
                {
                    try
                    {
                        this.m_firstPage = int.Parse(page);
                        this.m_lastPage = this.m_firstPage;
                        this.m_pages = page;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    string first = page.Substring(0, index).Trim();
                    string last = page.Substring(index + 1).Trim();
                    try
                    {
                        this.m_firstPage = int.Parse(first);
                        this.m_lastPage = int.Parse(last);
                        this.m_pages = this.m_firstPage + "-" + this.m_lastPage;
                    }
                    catch
                    {
                        return false;
                    }
                }

            }
            else
            {
                this.m_firstPage = 0;
                this.m_lastPage = 0;
                this.m_pages = "";
            }


            return true;
        }
        private static bool IsMatch(string text, string key)
        {
            text.Trim(' ');
            string[] contens = text.Split(' ');
            for (int nIndex = 0; nIndex < contens.Count(); nIndex++)
            {
                if (!string.IsNullOrEmpty(contens[nIndex]))
                {
                    if (contens[nIndex].Equals(key))
                    {
                        return true;
                    }
                    if (contens.Count() == 1 && contens[nIndex].Contains(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }  
        public void  GetPrintTaskInfo()
        {
            string orderid = "";
            string copies="";
            string fileid = "";
            string filetype = "";
            string filename = "";
            string printer="";
            string page="";
            string peisong = "";
            string time = "";
            bool isdouble=false;
            bool iscolor=false;
            int flag = 0;
            String post_send_data = "userID=" + c_userID + "&" + "licenseCode=" + c_licenseCode;
            HttpRsepData RespData = Post2ReceiveData(this.url_GetTask, post_send_data);
            if (Jsontext != ""&&Jsontext!=null)
            { 
            GetPrintTask obj = new GetPrintTask();
            List<Object> orderlist = obj.getTaskFromCloud(Jsontext);//获得云中任务
            List<report> r = new List<report>();
            if (orderlist.Count !=0)
            {
                for (int i = 0; i < orderlist.Count; i++)
                {
                    Orders order = (Orders)orderlist[i];
                    
        
                        if (order.IsDuplex == "1")
                        {
                            isdouble = false;

                        }
                        if (order.IsDuplex == "2")
                        {
                            isdouble = true;
                        }
                        if (order.Iscolor == "黑白")
                        {
                            iscolor = false;
                        }
                        else iscolor = true;
                        copies=order.Printcopies;
                        fileid = order.FileID;
                        filetype = order.Filetype;
                        page = order.Prtpage;
                        filename =order.Filename;
                        peisong = order.Peisong;
                        printer = order.Printer;

                    string printers = PCLConvertCS.PCLToPrinter.GetAllPrinter();
                    string[] all = printers.Split(';');
                    string x = "";
                    string murl = url;
                    murl += "/machine/privateDownloadFileHandler.ashx";
                    DownLoadFile obj2 = new DownLoadFile(@murl);
                    obj2.getFileFromCloud(orderlist);
                    string FilePath = System.IO.Directory.GetCurrentDirectory() + @"\下载文档\";
                    FilePath +=fileid + "." + filetype;
                    string fileOut = System.IO.Directory.GetCurrentDirectory() + @"\PCL\" + fileid + ".prn";//移动到哪里
                    PCLConvertCS.PCLConvert pc = new PCLConvertCS.PCLConvert();
                    PCLConvertCS.PDFConvert pcpdf = new PCLConvertCS.PDFConvert();
                    string fileOutFJ = System.IO.Directory.GetCurrentDirectory() + @"\PCL\" + fileid + "FJ" + ".prn";

                    if (FilePath.ToLower().EndsWith("doc") || FilePath.ToLower().EndsWith("docx"))
                    {
                        string result = pc.WordToPCL(readArgs(FilePath, copies, iscolor, isdouble, page), fileOut, printer);
                        if (result == null)
                        {
                            flag = 1;
                        }
                    }
                    if (FilePath.ToLower().EndsWith("pdf"))
                    {
                        string result = pcpdf.ConvertPdf2pcl(readArgs(FilePath, copies, iscolor, isdouble, page), fileOut);
                        if (result == null)
                        {
                            flag = 1;
                        }
                    }
                    if (FilePath.ToLower().EndsWith("jpg") || FilePath.ToLower().EndsWith("jpeg") || FilePath.ToLower().EndsWith("JPEG"))
                    {
                        string result = pc.JPGToPCL(FilePath, fileOut);
                        if (result == null)
                        {
                            flag = 1;
                        }
                    }
                    if (FilePath.ToLower().EndsWith("xlsx") || FilePath.ToLower().EndsWith("xlsm"))
                    {
                        string sPath = System.IO.Directory.GetCurrentDirectory() + @"\下载文档\" + "\\" + orderid + "\\" + fileid;
                        if (XLSConvertToPDF(FilePath, sPath))
                        {
                            FilePath = sPath + ".pdf";
                            string result = pcpdf.ConvertPdf2pcl(readArgs(FilePath, copies, iscolor, isdouble, page), fileOut);
                            if (result == null)
                            {
                                flag = 1;
                            }
                        }
                    }
                    if (flag == 1)
                    {
                        string fileOut1 = System.IO.Directory.GetCurrentDirectory() + @"\PCL\" + fileid + ".prn";
                        PCLConvertCS.PCLToPrinter pcl = new PCLConvertCS.PCLToPrinter();
                        // 设置事件触发间隔
                        pcl.SetEventTimeout(2000);
                        // 添加打印监控事件处理handler
                        pcl.OnPCLResult += new PCLConvertCS.PCLToPrinter.PCLResult(pcl_OnPCLResult);
                        string result1 = pcl.SendFileToPrinter(printer, fileOut1);
                        string userid = order.OrderID;
                        countpage(FilePath);
                        int xpage = 0;
                        if (page == "全部")
                        {
                            xpage = mnum;
                        }
                        else
                        {
                            xpage = m_lastPage - m_firstPage + 1;
                        }
                        int totalPage = Convert.ToInt32(copies) * xpage;
                        report a = new report();
                        a.Orderid = order.OrderID;
                        a.Pagenum = totalPage;
                        DirectoryInfo dirInfo = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + @"\PCL\");
                        FileInfo[] files = dirInfo.GetFiles();   // 获取该目录下的所有文件
                        foreach (FileInfo file in files)
                        {
                            file.Delete();
                        }

                        r.Add(a);
                    }
                }

                ReportData R = new ReportData();
                R.ReportCloud(CreateJsonParameters(r), @url);
                }
            }
        }

        private void pcl_OnPCLResult(object sender, EventArgs e)
        {
         
        }
        public List<Class_PrintArgs> readArgs(string filepath,string copies,bool color,bool duplex,string page)
        {
            List<Class_PrintArgs> list = new List<Class_PrintArgs>();
            Class_PrintArgs c = new Class_PrintArgs();
            c.Filepath = filepath;
            c.Copies = Convert.ToInt32(copies);
            c.Color = color;
            c.IsDuplex = duplex;
            c.Pages = page;
            list.Add(c);
            return list;
        }
        public string Jsontext;
        //定义委托
        public delegate void WeiTuo();
        //定义委托对象
        public event WeiTuo shijian = null;
        public HttpRsepData Post2ReceiveData(string strUrl, string HttpRequestData)
        {

                HttpRsepData RespData = new HttpRsepData();
                Encoding encode = System.Text.Encoding.UTF8;
                //注意提交的编码 这边是需要改变的 这边默认的是Default：系统当前编码
#if false
    byte[] arrB = encode.GetBytes(HttpRequestData);
#else
                byte[] arrB = Encoding.UTF8.GetBytes(HttpRequestData);
#endif

                CookieContainer cookieContainer = new CookieContainer();
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(strUrl);
                myReq.Method = "POST";
                myReq.ContentType = "application/x-www-form-urlencoded";
                myReq.ContentLength = arrB.Length;
                myReq.CookieContainer = cookieContainer;
                try
                {
                    Stream outStream = myReq.GetRequestStream();//其他信息: 无法连接到远程服务器
                    outStream.Write(arrB, 0, arrB.Length);
                    outStream.Close();
                }
                catch (Exception ex)
                {
                    shijian();
                    MessageBox.Show("网络错误，请稍后连接！");
                    
                }
                
                WebResponse myResp = null;
            
            try
            {
                //接收HTTP做出的响应
                myResp = myReq.GetResponse();
            }
            catch (Exception e)
            {
                int ii = 0;
            }
            if (myResp != null)
            {
                RespData.RespStatus = ((HttpWebResponse)myResp).StatusDescription;
                if (RespData.RespStatus == HttpOK)
                {
                    RespData.RespStatusCode = 200;
                    Encoding respencode = System.Text.Encoding.UTF8;
                    Stream ReceiveStream = myResp.GetResponseStream();
                    StreamReader readStream = new StreamReader(ReceiveStream, respencode);
                    RespData.RespData = readStream.ReadToEnd();                 
                    Jsontext = RespData.RespData;
                }
                else if (RespData.RespStatus == HttpNotFing)
                {
                    RespData.RespStatusCode = 400;
                }
                else
                {
                    RespData.RespStatusCode = -1;
                }


            }
            else
            {
                RespData.RespStatus = "服务器未连接-HttpWebRequest";
                RespData.RespStatusCode = -1;
            }
            return RespData;        
        }

        public string CreateJsonParameters(List<report> r)
        {
            StringBuilder JsonString = new StringBuilder();
            List<report> products = r;
                string[] ColumnName = new String[r.Count+1];
                    ColumnName[0] = "\"" + "orderid" + "\":\"";
                    ColumnName[1] = "\"" + "pagenum" + "\":\"";
                JsonString.Append("[");
                for (int i = 0; i < r.Count; i++)
                {
                    JsonString.Append("{");
                    report x = products[i];
                        JsonString.Append(ColumnName[0]);
                        JsonString.Append(x.Orderid);
                        JsonString.Append("\",");
                        JsonString.Append(ColumnName[1]);
                        JsonString.Append(x.Pagenum);
                        JsonString.Append("\"");
                    JsonString.Append("},");
                }
                JsonString.Remove(JsonString.Length - 1, 1);
                JsonString.Append("]");
                return JsonString.ToString();

        }
    }

}
