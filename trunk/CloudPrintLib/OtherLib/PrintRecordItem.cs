using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using PrintJobMonitor;
using BaseLib;

namespace CloudPrintforPC
{
   public class PrintRecordItem
    {
        public static readonly String PriceNone = "-0.01";
        private String _Price = PriceNone;/**<打印价格*/
        /*-----------------------------------*/
        public String _OrderId = "";
        private String _OrderId_Suffix;
        public String _UserName = "";
        private String _HavePay = "NO";
        public String _HavePrinted = "NO";/**<已经打印*/
        public String _FileFullPath;
        /*-----------------------------------*/
       public String _TimeCompete=DateTime.Now.ToString();//完成时间
       private DateTime _DateTimeSubmit =DateTime.Now;
       public String _phoneNumber;//手机号
       public String _phoneType;//手机型号
       public String _androidOs;//Android版本
       public int    _PParamcopies =0;//份数
       public bool   _PParamcolor = false;//彩打
       public String _PParamRange = "null";//打印范围
       public bool   _PParam2Paper = false;//双面
       public String _Paper_Type = "A4";//纸型
       public String _PrinterName = "无打印机";//打印机名
       private String _PrinterDriveName = "";
       private String _AsyncNofify="false"; 
        /*-----------------------------------*/
       private bool PrintOnce = false;

        public String AsyncNotify
        {
            get
            {
                return this._AsyncNofify;
            }

        }
        public String OrderId_Suffix
        {
            get
            {
                return this._OrderId_Suffix;
            }
            set
            {
               this._OrderId_Suffix = value;
            }
        }
        public String PrinterName
        {
            get {
                    return _PrinterName;
            }
            set {
                    _PrinterName = value;
            }
        }
        public String PaperType
        {
            get
            {
                return this._Paper_Type;
            }
            set
            {
                this._Paper_Type = value;
            }
        }

        public String Duplex
        {
            get {return (this._PParam2Paper == false)?"单面":"双面"; }
            set { }

        }

        public String Os
        {
            get { return this._androidOs; }
            set { }
        }

        public String PhoneId
        {
            get { return this._phoneNumber; }
            set { }
        }

        public String PhoneType
        {
            get { return this._phoneType; }
            set { }
        }

        public String Copies
        {
            get { return this._PParamcopies.ToString(); }
            set { }
        }

        public String Color {
            get { return (this._PParamcolor == true) ? "彩色" : "黑白"; }
            set { }
        }

        public String Range
        {
            get { return this._PParamRange; }
            set { }
        }
        public String PrintCount
        {
            set {  }
            get { return this._PParamRange;}
        }
        public String  PrintTime
        {
            set { }
            get { return this._TimeCompete; }

        }
        public  PrintRecordItem( FileSendOnNet sson)
       {
          /*-----------------------------------*/
           this._OrderId = sson.GetOrderId();
           this._OrderId_Suffix = sson.GetOrderId_Suffix();
           this._UserName = sson.GetUserName();
           this._HavePay = sson.GetPrice2Pay();
           this._Price = sson.GetPrice2Pay();
           this._FileFullPath = sson.FileFullPath;
          /*-----------------------------------*/
           this._phoneNumber = sson._PhoneNumber;
           this._phoneType = sson._PhoneType;
           this._androidOs = sson._OsNumber;
           this._PParamcopies = sson.PParamcopies;
           this._PParamcolor = sson.PParamcolor;
           this._PParamRange = sson.PParamRange;
           this._PParam2Paper=sson.PParam2Paper;
           this._Paper_Type = "自动适应纸张大小";
           this._PrinterName = sson.GetPrinterName();
            //this.PrinterDriveName = PrintLocal.GetPrinterDrivenName(this._PrinterName);
           this.PrinterDriveName = sson.GetPrinterDriveName();
           this._AsyncNofify=sson.GetProperty("ppneedasyncnotify", "false");
        }
       


     public PrintRecordItem()
     { 
     
     }
     public static XmlDocument GetXmlDoc(String fileName)
     {
         if (File.Exists(fileName))
         {
             //已存在
             XmlDocument xd=new XmlDocument();
             xd.Load(fileName);
             return xd;
             
         }
         else
         {
                XmlDocument xd = new XmlDocument();
                xd.AppendChild( xd.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
                return xd;
         }
     }

        public static void  Append(XmlDocument doc, XmlElement record, String child_key, String child_value)
        {
                XmlElement item = doc.CreateElement(child_key);
                item.InnerText = child_value;
                record.AppendChild(item);
        }
        public String WriteThisRecord2Disk(String fileName,bool save) 
     {
          XmlDocument doc = GetXmlDoc(fileName); // 创建dom对象
          XmlNode  root=doc.SelectSingleNode("/Datas");
         if(root==null){
             //  XmlElement doc=SelectSingleNode("/Datsa");
             root = doc.CreateElement("Datas"); // 创建根节点album
             doc.AppendChild(root);    //  加入到xml document
         }
         
         XmlElement record= this.GetThisRecord(doc, root);

        if (save) {
            doc.Save(fileName);
        }
         return   record.ToString();     
        
     }

    public String WriteSubmitRecord2Disk(bool save)
    {
      return  this.WriteThisRecord2Disk(this.GetFileFullPath_SubmitPath(),save);
    }
    public XmlElement GetThisRecord(XmlDocument doc, XmlNode root)
    {

        XmlElement record = doc.CreateElement("data");  // 创建preview元素
        root.AppendChild(record);   // 添加到xml document
        Append(doc, record, "OrderId", this._OrderId);
        Append(doc, record, "OrderId_Suffix", this._OrderId_Suffix);
        Append(doc, record, "UserName", this._UserName);
        Append(doc, record, "HavePay", this._HavePay);
        Append(doc, record, "HavePrinted", this._HavePrinted);
        Append(doc, record, "Price", this.GetPrice());
        Append(doc, record, "PrinterName", this._PrinterName);
        Append(doc, record, "PrinterDriveName", this._PrinterDriveName);
        Append(doc, record, "PhoneNumber", this._phoneNumber);
        Append(doc, record, "PhoneType", this._phoneType);
        Append(doc, record, "OS", this._androidOs);
        Append(doc, record, "PrintFile", this._FileFullPath);
        Append(doc, record, "PrintCopy", this._PParamcopies.ToString());
        Append(doc, record, "PrintColor", (this._PParamcolor) ? "彩色" : "黑白");// item.SetAttribute("Color",this._PParamcolor.ToString());
        Append(doc, record, "PrintRange", this._PParamRange);
        Append(doc, record, "PrintDuplx", this._PParam2Paper ? "双面" : "单面");//  item.SetAttribute("Duplx", this._PParam2Paper.ToString());
        Append(doc, record, "PrinterPaperType", this._Paper_Type);
        Append(doc, record, "TimeComplete", DateTime.Now.ToString());
        Append(doc, record, "AsyncNotify", this._AsyncNofify);
        return record;
    }
    public static String ReadNode(XmlNode node,String key)
    {

            XmlNode xn_t=node.SelectSingleNode(key);
            if (xn_t != null)
            {
                return xn_t.InnerText;
            }
            else {
                return "null";
            }
            
        }
     public static List<PrintRecordItem> ReadXml(String name)
     {
         List<PrintRecordItem> apri = new List<PrintRecordItem>();

      
         XmlDocument xmlDoc=PrintRecordItem.GetXmlDoc(name);
         XmlNode xn = xmlDoc.SelectSingleNode("Datas");
        // XmlNodeList xnl = xn.ChildNodes;
         foreach (XmlNode node in xn)
         {
             PrintRecordItem pri = new PrintRecordItem();
#if true

                 pri._OrderId=ReadNode(node,"OrderId");
                 pri._OrderId_Suffix = ReadNode(node, "OrderId_Suffix");
                 pri._UserName = ReadNode(node, "UserName");
                 pri._PrinterName = ReadNode(node, "PrinterName");
                 pri.PrinterDriveName=ReadNode(node, "PrinterDriveName");
                 pri._phoneNumber = ReadNode(node, "PhoneNumber");
                 pri._phoneType = ReadNode(node, "PhoneType");
                 pri._androidOs = ReadNode(node, "OS");
                 pri._FileFullPath = ReadNode(node, "PrintFile");
                 pri._PParamcopies = int.Parse(ReadNode(node, "PrintCopy"));
                 pri._PParamcolor = ReadNode(node, "PrintColor").Equals("彩色");
                 pri._PParamRange = ReadNode(node, "PrintRange");
                 pri._PParam2Paper = ReadNode(node, "PrintDuplx").Equals("双面");
                 pri._Paper_Type = ReadNode(node, "PrinterPaperType");
                 pri._TimeCompete = ReadNode(node, "TimeComplete");
                 pri._Price = ReadNode(node, "Price");//  XmlElement item = doc.CreateElement("Price");
                 pri._AsyncNofify = ReadNode(node, "AsyncNotify");
               apri.Add(pri);
       
#endif   
         }
         return apri;
     }
       /**/
     public ListViewItem GetListView() 
     {
         ListViewItem LVI = new ListViewItem(this._PrinterName);
        // LVI.SubItems.Clear();
        // LVI.SubItems.Add(this._phoneNumber);
         LVI.SubItems.Add(this._phoneNumber);
         LVI.SubItems.Add(this._phoneType);
         LVI.SubItems.Add(this._androidOs);
         LVI.SubItems.Add(this._PParamcopies.ToString());
         LVI.SubItems.Add((this._PParamcolor==true)?"彩色":"黑白");




         LVI.SubItems.Add(this._PParamRange);
         LVI.SubItems.Add((this._PParam2Paper==false)?"单面":"双面");
         LVI.SubItems.Add(this._Paper_Type);
         LVI.SubItems.Add(this._TimeCompete);
         return LVI;
     }
        /**
        */
        public  String GetFileFullPath_SubmitPath()
        {
            String forderData = _DateTimeSubmit.ToString("yyyy年MM月dd日");
            String hour = _DateTimeSubmit.ToString("HH时");
            string Fullpath = PrintRecord.GetRootPath() + forderData + "\\" + hour + "\\";
            if (!Directory.Exists(Fullpath))
            {
                Directory.CreateDirectory(Fullpath);
            }
            string FileFullpath = Fullpath + PrintRecord.FileName_Submit_Order;
            return FileFullpath;
        }
        /**
        *根据打印方式，计算价格
        */
        public String GetPrice()
        {

            lock (this) {

                if (string.IsNullOrEmpty(this._FileFullPath)){
                            //没有文件
                            return  PriceNone;
                        }
            
                        if (LibCui.IsStringNeg(this._Price))
                        {
                            //价格没有设置
               
                            // public int    _PParamcopies =0;//份数
                            //public bool _PParamcolor = false;//彩打
                            //public String _PParamRange = "null";//打印范围
                            //public bool _PParam2Paper = false;//双面

                            //  double price_t=_PParamcopies* (_PParamcolor==true?0.1:0.1)*

                            int Pages= FilePages.GetFilePages(this._FileFullPath);
                            //页数*价格*份数

                            double Duplex =(_PParam2Paper == true) ? 1.1 : 1.0;
                            double Color=(_PParamcolor == true) ? 1.1 : 1.0;

                            double Total_money = Pages * 0.1*_PParamcopies*Duplex*Color;
              



                            return this._Price=Total_money.ToString("0.00");


                        }
                        else {
                            return this._Price;

                        }
                        // return PriceNone;


            }

          
        }
        /**
        *
        */
        public String GetPriceJson()
        {
            JObject jo_t =new JObject();
            jo_t.Add("price2pay",this.GetPrice());
            jo_t.Add("result", "success");
            return jo_t.ToString();
        }

        /// <summary>
        /// 现阶段只打印一次
        /// </summary>
        /// <returns></returns>
        public String GetPrintCompletedJson()
        {
            lock (this) {
                     JObject jo_t = new JObject();
                                FileSendOnNet fileRcv = new FileSendOnNet();
                                fileRcv.Parse4PrintRecord(this);

                                if (this.PrintOnce == true) {
                                    jo_t.Add("result", "fali");
                                    jo_t.Add("ServerInfo", "has print once");
                                    return jo_t.ToString();
                                }

                                if (fileRcv.mFileFullPath == null)
                                {
                                    jo_t.Add("result", "fali");
                                    jo_t.Add("ServerInfo", "no file");
                                }
                                else
                                {
                                    PrintDocThread pdt = new PrintDocThread(fileRcv);
                                    String reslut_t=pdt.PrintDocment();
                                    if (String.IsNullOrEmpty(reslut_t))
                                    {
                                         jo_t.Add("result", "success");
                                         this.PrintOnce = true;
                                    }else {
                                        jo_t.Add("result", "fali");
                                        jo_t.Add("ServerInfo",reslut_t);
                                    }
                                }
                                //jo_t.Add("price2pay", this._Price);
                                  return jo_t.ToString();

            }

           
        }
        /**
      *
      */
       public String PrinterDriveName
        {
            get { return this._PrinterDriveName; }
            set { this._PrinterDriveName = value; }
        }
        /**
       *
       */
        public String GetHashKey()
        {
            return this._OrderId_Suffix;
        }
        /**
      *
      */

    }
}
