using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using PCLConvertCS;
using System.IO;
using System.Printing;
using System.ComponentModel;

namespace CloudPrintforPC
{
    public enum PrintFrom{
		Self,
		Phone,
		Pc,
		cloud
	}
	public enum PrintType{
		NetPrint,
		PcPrint,
		CloudPrint
	}
	public enum PrintFromType{
		pc_PcPrint,
		phone_NetPrint,
		self_NetPrint,
		cloud_CloudPrint
	}
    public class PrintOneInfo : PrinterInfoBase, INotifyPropertyChanged
    {
      
        public PrintFrom mPrintFrom;
        public PrintType mPrintType;
        /**
       * 经度
       */
        private double latitude;
        /**
         * 纬度
         */
        private double longitude;
        public ServerInfo mParent;



        public  String mSearchTime;
        public  IPAddress mAddress;
        public  String mIPAddrStr;
        private bool mCanDuplex = false;
        private bool mIsValid = false;
        private String mStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        public  String Status 
        {
            get {

                return mStatus=this.GetPrinterStatus();
            }
            set {

            }
        
        }
        public String PrintName
            {
            set { this.mPrintName = value; }
            get { return this.mPrintName; }
            
            }
        private int _wpf_progress = 0;
        public int WPF_PROGRESS {
            get {
                return this._wpf_progress;
            }
            set {
                this._wpf_progress = value;
                if (PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("WPF_PROGRESS"));
            } }
        public PrintOneInfo(
            String name,
            PrintType type,
            PrintFrom from,
            bool IsDuplex,
            bool IsValid):base(name)
        {
           // base(name);
            this.mPrintName = name;
            this.mPrintType = type;
            this.mPrintFrom = from;
            this.mCanDuplex=IsDuplex;
            this.mIsValid = false;
            this.WPF_PROGRESS = 0;
          
        }
        public PrintOneInfo(
             String name,
             PrintType type,
             PrintFrom from,
             String status)
            : base(name)
        {
            this.mPrintName = name;
            this.mPrintType = type;
            this.mPrintFrom = from;
            this.mStatus = status;
            if (String.IsNullOrEmpty(this.mStatus)) {
                this.mStatus = "未定义，崔兵兵";
            }
            this.WPF_PROGRESS = 0;
           
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(PrintOneInfo))
                return false;
            PrintOneInfo c = obj as PrintOneInfo;
            return (this.mPrintName == c.mPrintName && this.mPrintType == c.mPrintType);
        }
        public override int GetHashCode()
        {
            String hashString = this.mPrintName + this.mPrintType;
            return hashString.GetHashCode();
        }
        private void pcl_OnPCLResult(object sender, EventArgs e)
        {

        }
        public bool PrintFiles(String fileName) 
        {
            try { 
                String fileNameExt=Path.GetExtension(fileName);
                if (fileNameExt.ToLower().Equals(".jpg")||fileNameExt.ToLower().Equals(".jpeg"))
                {
                    String outFile = Path.GetFileNameWithoutExtension(fileName) + ".prn";
                    PCLConvert pclConvert = new PCLConvert(false);
                    pclConvert.JPGToPCL(fileName, outFile);

#if false    
                  
                   
                    /////////////////////////////////////

                    PCLConvertCS.PCLToPrinter pcl = new PCLConvertCS.PCLToPrinter();
                    // 设置事件触发间隔
                    pcl.SetEventTimeout(2000);
                    // 添加打印监控事件处理handler
                    pcl.OnPCLResult += new PCLConvertCS.PCLToPrinter.PCLResult(pcl_OnPCLResult);
                    string result1 = pcl.SendFileToPrinter(this.mPrintName, outFile);
#endif
                }
                else {
                    return false;
                }
            }catch(Exception e){
                return false;
            }
            return true;
              
        }
       /*
        public void GetPrintTaskInfo()
        {
           
            string copies = "";
            string fileid = "";
            string filetype = "";
            string filename = "";
            string printer = "";
            string page = "";
            string peisong = "";
            string time = "";
            bool isdouble = false;
            bool iscolor = false;
            int flag = 0;
         
           
          
            {
               
               
                {
                    for (int i = 0; i < orderlist.Count; i++)
                    {
                      


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
                        copies = order.Printcopies;
                        fileid = order.FileID;
                        filetype = order.Filetype;
                        page = order.Prtpage;
                        filename = order.Filename;
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
                        FilePath += fileid + "." + filetype;
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
      */

        public String GetStatus()
        {
            return GetPrinterStatus();
        }
        public bool IsOffline()
        {
           return  ("脱机（Off Line）").Equals(this.mStatus);
        }

      private String GetPrinterStatus()
       {

          return  this.mStatus = Printer.GetPrinterStatus(this.mPrintName);
       
       }
    
  }
}
