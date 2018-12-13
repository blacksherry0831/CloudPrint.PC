using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Client_simplify.lists;
using terminal.lib;
using System.Configuration;
using System.Web;
namespace CloudPrintforPC
{
    public class PrintDocThread
    {
        private static Mutex mMutex;
        FileSendOnNet file;
        ParseFileOnLocal2Print printerOnLocal;
        ParseFileOnNet2Print printerOnNet;
        public PrintDocThread(FileSendOnNet filefullpath)
        {
            this.Init();
            file = filefullpath;
            printerOnLocal = new ParseFileOnLocal2Print(file);
            printerOnNet = new ParseFileOnNet2Print(file);
        }
        public void StartPrint()
        {
            if (File.Exists(file.mFileFullPath))
            {
                new Thread(this.PrintDocment).Start();
            }
        }
        private void Init()
        {
            if (mMutex == null) {
                mMutex = new Mutex(false,"PrintTEST");
            }
        }
        private void Wait() 
        {
            if (mMutex != null)
            {
                mMutex.WaitOne();
            }
        }
        private void Release()
        {
            if (mMutex != null)
            {
                mMutex.ReleaseMutex();
            }
        }
        public void PrintDocment()
        {
            String error_str;
            this.Wait();
#if true
            bool Success = false;
          
                       
        #if true
                    Success=printerOnLocal.PrintDocment();
        #endif
            
           
                    if (Success==false)
                    {
        #if true
                        /*---本地打印失败-----*/
                        try {
                            Success = printerOnNet.PrintDocment();
                        }catch(Exception e){
                             LogHelper.WriteLog(this.GetType(),e);
                        }
               
        #endif       
                    }
                    LibCui.DeleteFile(file.mFileFullPath);
                    LibCui.DeleteFile(file.GetPCLprnfullPath());
#endif
            this.Release();
            if (Success==true)
            {
                PrintRecord pp = new PrintRecord();
                pp.WriteRecord2Disk(file);
            }
        }

    }

   
    public class ParseFileOnNet2Print
    {
        FileSendOnNet file;
        String PclFileID;
        public ParseFileOnNet2Print(FileSendOnNet filefullpath)
        {
            file = filefullpath;
        }
        public bool PrintDocment()
        {
            if (File.Exists(file.mFileFullPath))
            {
                //打印？
               return this.PrintDoc(file);

            }

            return false;


        }
        private bool PrintDoc(FileSendOnNet file)
        {
            PclFileID=this.PostFile2Server();
            if (!String.IsNullOrWhiteSpace(PclFileID))
            {
                /**Post提交成功**下载文件*/
                String PclFileName=null;
                Thread.Sleep(300);
                int TimeCount = 0;
                while (String.IsNullOrEmpty(PclFileName =this.DownloadPCLFile()))
                {
                  //未准备好
                    if (TimeCount++ < 10)
                        Thread.Sleep(500);
                
                }


                if (!String.IsNullOrEmpty(PclFileName))
                {
                    /*--下载成功-*/
                    SendPCL2Printer s2p = new SendPCL2Printer();
                    return s2p.SendFile2Printer(PclFileName, file.GetPrinterName());
                }
            }
            return false;
        }
        public String PostFile2Server() 
        {
            String filefullPath = file.mFileFullPath;
          
            StringBuilder FullUrl = new StringBuilder();
            FullUrl.Append(GetAppConfig("URL_POST_FILE"));
            FullUrl.Append("?LoginID=zhangxiaochen");
            FullUrl.Append("&Password=123123");
            FullUrl.Append("&FileName="+HttpUtility.UrlEncode(file.GetFileName(),Encoding.UTF8));
            FullUrl.Append("&PrinterCopies="+file.PParamcopies.ToString());
            FullUrl.Append("&PrinterColor=" + ((file.PParamcolor == true) ? 1 : 0).ToString());
            FullUrl.Append("&PrintPaperSize=A4");
            FullUrl.Append("&PrintRange="+file.PParamRange);
            FullUrl.Append("&PrinterIsDuplex="+((file.PParam2Paper==true)?1:0).ToString());
            FullUrl.Append("&WifiMac="+LibCui.GetWifiMacAddress());
            FullUrl.Append("&PrinterMachineMac="+LibCui.GetLocalMacAddress());
            FullUrl.Append("&MachineBrand="+HttpUtility.UrlEncode(file.GetPrinterName(),Encoding.UTF8));
            FullUrl.Append("&MachineType=" + HttpUtility.UrlEncode(file.GetPrinterName(), Encoding.UTF8));
            FullUrl.Append("&Latitude="+"0");
            FullUrl.Append("&Logitude="+"0");
            return this.HttpPostFile(FullUrl.ToString(),filefullPath);
        }
        public String DownloadPCLFile()
        {            
            StringBuilder FullUrl = new StringBuilder();
            FullUrl.Append(GetAppConfig("URL_DOWNLOAD_FILE"));
            FullUrl.Append("?LoginID=zhangxiaochen&");
            FullUrl.Append("Password=123123&");
            FullUrl.Append("FileID=" + PclFileID);

            return this. HttpDownloadFile(FullUrl.ToString(),file.GetPCLprnfullPath()); 
        }
        /// <summary>
        /// 返回＊.exe.config文件中appSettings配置节的value项
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static string GetAppConfig(string strKey)
        {
            

            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }
        /*----------------------------------------------------------------*/
        private string HttpPostFile(string Url, string filename)
        {
          

            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            long FILE_SZIE = fileStream.Length;
            /*-----------------------------------------------------------------------------*/
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType ="text/html";
            request.ContentLength = FILE_SZIE;
            Stream myRequestStream = request.GetRequestStream();
            {
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    myRequestStream.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();            
            }
            /*-------------返回值-----------------------------------------------------------------------*/
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string retString="";
            if (response.StatusCode == HttpStatusCode.OK) {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }            
            /*---------------------------------------------------------------------------------------------*/
            return retString;
        }
        private string HttpDownloadFile(string Url,string PclFileSavePath)
        {
           
                /*-----------------------------------------------------------------------------*/
                FileStream fileStream = new FileStream(PclFileSavePath, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                long FILE_SZIE = fileStream.Length;
                /*-----------------------------------------------------------------------------*/
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = "text/html";
                /*-------------返回值----------------------------------------------------------*/
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                
                 long  TotalLength=response.ContentLength;
                 long RcvLength = 0;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream myResponseStream = response.GetResponseStream();
                    while((bytesRead = myResponseStream.Read(buffer, 0, buffer.Length))!=0){
                       
                        fileStream.Write(buffer, 0, bytesRead);
                    }
                    fileStream.Flush();
                    RcvLength=fileStream.Length;
                    fileStream.Close();

                    myResponseStream.Close();
                }
                if (TotalLength ==RcvLength)
                {
                    return PclFileSavePath;
                }
                else {
                    return null;
                }
                /*---------------------------------------------------------------------------------------------*/
              

        }
    }
    public class ParseFileOnLocal2Print
    {
       FileSendOnNet file;
       public ParseFileOnLocal2Print(FileSendOnNet filefullpath)
        {
            file = filefullpath;
        }
       
        public bool PrintDocment()
        {
            if (File.Exists(file.mFileFullPath))
            {
                //打印？
                return this.PrintDoc(file);

            }
            else {
                return false;
            }

        }
        private List<Class_PrintArgs> readArgs(string filepath, string copies, bool color, bool duplex, string page)
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
        private List<Class_PrintArgs> readArgs(string filepath)
        {
            List<Class_PrintArgs> list = new List<Class_PrintArgs>();
            Class_PrintArgs c = new Class_PrintArgs();
            c.Filepath = filepath;
            c.Copies = file.PParamcopies;
            c.Color = file.PParamcolor;
            c.IsDuplex = file.PParam2Paper;
            c.Pages = file.PParamRange;
            list.Add(c);
            return list;
        }
        private bool PrintDoc(FileSendOnNet file)
        {
            String FilePath = file.mFileFullPath;
            int flag = 0;
            PCLConvertCS.PCLConvert pc = new PCLConvertCS.PCLConvert();
            PCLConvertCS.PDFConvert pcpdf = new PCLConvertCS.PDFConvert();
            string fileOut = file.GetPCLprnfullPath();
            string printer;

            if (file.GetPrinterName().Equals(""))
            {
                printer = PrintLocalPrint.LocalPrinter.DefaultPrinter();
            }
            else
            {
                printer = file.GetPrinterName();
            }

            if (FilePath.ToLower().EndsWith("doc") || FilePath.ToLower().EndsWith("docx"))
            {
                string result = pc.WordToPCL(readArgs(FilePath), fileOut, printer);
                if (result == null)
                {
                    flag = 1;
                }
            }
            if (FilePath.ToLower().EndsWith("pdf"))
            {
                string result = pcpdf.ConvertPdf2pcl(readArgs(FilePath), fileOut);
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
            if (FilePath.ToLower().EndsWith("xlsx") || FilePath.ToLower().EndsWith("xlsm") || FilePath.ToLower().EndsWith("xls"))
            {
#if false
                string sPath = System.IO.Path.GetTempPath() + System.IO.Path.GetFileName(FilePath).Replace(".","_") + ".pdf";
                Method m = new Method();

                if (m.Convert2PDF(FilePath, sPath, Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF))
                {

                    string result = pcpdf.ConvertPdf2pcl(readArgs(sPath), fileOut);
                    if (result == null)
                    {
                        flag = 1;
                    }
                }
#else
                string sPath = System.IO.Path.GetTempPath() + System.IO.Path.GetFileNameWithoutExtension(FilePath) +Guid.NewGuid().ToString()+ ".pdf";
                Method m = new Method();

                if (m.Convert2PDF(FilePath, sPath, Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF))
                {
#if true
                    string result = pcpdf.ConvertPdf2pcl(readArgs(sPath), fileOut);
#else
                      string result ="测试";
#endif
                    if (result == null)
                    {
                        flag = 1;
                    }
                }
#endif

            }
            if (flag == 1)
            {
                //文件转换成功-发送到打印机
                SendPCL2Printer s2p = new SendPCL2Printer();

                return s2p.SendFile2Printer(fileOut, printer);
            }
            else
            {
                Debug.Assert(false);
                //文件转换失败
                return false;
            }
        }
        
    }


    public class SendPCL2Printer
    { 
        public bool SendFile2Printer(String fileOut,String printer)
        {
            ;
                PCLConvertCS.PCLToPrinter pcl = new PCLConvertCS.PCLToPrinter();
                // 设置事件触发间隔
                pcl.SetEventTimeout(2000);
                // 添加打印监控事件处理handler
                pcl.OnPCLResult += new PCLConvertCS.PCLToPrinter.PCLResult(pcl_OnPCLResult);
                string result1 = pcl.SendFileToPrinter(printer, fileOut);
                if (result1 == null)
                {
                    return true;
                }
                else {
                    Debug.Assert(false);
                    return false;
                }
        }

        private void pcl_OnPCLResult(object sender, EventArgs e)
        {

        }

    }
}
