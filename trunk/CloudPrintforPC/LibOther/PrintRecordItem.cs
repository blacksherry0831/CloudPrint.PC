using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
namespace CloudPrintforPC
{
   public class PrintRecordItem
    {
       public String TimeCompete=DateTime.Now.ToString();
       public String _phoneNumber;//手机号
       public String _phoneType;//手机型号
       public String _androidOs;//Android版本
       public int PParamcopies =0;//份数
       public bool PParamcolor = false;//彩打
       public String PParamRange = "null";//打印范围
       public bool PParam2Paper = false;//双面
       public String Paper_Type = "A4";
       public String PrinterName = "无打印机";
     public  PrintRecordItem( FileSendOnNet sson)
       {
           this._phoneNumber = sson._PhoneNumber;
           this._phoneType = sson._PhoneType;
           this._androidOs = sson._OsNumber;
           this.PParamcopies = sson.PParamcopies;
           this.PParamcolor = sson.PParamcolor;
           this.PParamRange = sson.PParamRange;
           this.PParam2Paper=sson.PParam2Paper;
           this.Paper_Type = "自动适应纸张大小";
           this.PrinterName = sson.GetPrinterName();
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
             return new XmlDocument();
         }
     }
     public void WriteThisRecord2Disk(String fileName) 
     {
          XmlDocument doc = GetXmlDoc(fileName); // 创建dom对象
          XmlNode  root=doc.SelectSingleNode("/Datas");
         if(root==null){
             //  XmlElement doc=SelectSingleNode("/Datsa");
             root = doc.CreateElement("Datas"); // 创建根节点album
             doc.AppendChild(root);    //  加入到xml document
         }       




         {
            XmlElement record = doc.CreateElement("data");  // 创建preview元素

             root.AppendChild(record);   // 添加到xml document

             //下面一样,不一行行写解释了
             {
                 XmlElement item = doc.CreateElement("PrinterName");
                 item.InnerText = this.PrinterName;
                 record.AppendChild(item);
             }
             {
                XmlElement item = doc.CreateElement("PhoneNumber");
                item.InnerText= this._phoneNumber;
                record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("PhoneType");
                 item.InnerText = this._phoneType;
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("OS");
                 item.InnerText = this._androidOs;
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("PrintCopy");
                  item.InnerText = this.PParamcopies.ToString();
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("PrintColor");
                 item.SetAttribute("Color",this.PParamcolor.ToString());
                 item.InnerText = (this.PParamcolor)?"彩色":"黑白";
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("PrintRange");
                 item.InnerText = this.PParamRange;
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("PrintDuplx");
                 item.SetAttribute("Duplx", this.PParam2Paper.ToString());
                 item.InnerText = this.PParam2Paper?"双面":"单面";
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("PrinterPaperType");
                 item.InnerText = this.Paper_Type;
                 record.AppendChild(item);
             }
             {
                 XmlElement item = doc.CreateElement("TimeComplete");
                 item.InnerText = DateTime.Now.ToString();
                 record.AppendChild(item);
             }
         }
        
       //  doc.AppendChild(root);
         doc.Save(fileName);
       
        
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
             {
                 XmlNode xn_t = node.SelectSingleNode("PrinterName");
                 if(xn_t!=null){
                     pri.PrinterName = xn_t.InnerText;
                 }
                // item.InnerText = this.PrinterName;
                // record.AppendChild(item);
             }
          
             {
                 XmlNode xn_t = node.SelectSingleNode("PhoneNumber");
                 if(xn_t!=null)
                 pri._phoneNumber = xn_t.InnerText;
             }
             {
                 XmlNode item = node.SelectSingleNode("PhoneType");
                  if (item != null)
                   pri._phoneType= item.InnerText;
              
             }
             {
                 XmlNode item = node.SelectSingleNode("OS");
                 if (item != null)
                  pri._androidOs= item.InnerText;
               
             }
             {
                 XmlNode item = node.SelectSingleNode("PrintCopy");
                 if (item != null)
                 pri.PParamcopies=int.Parse(item.InnerText);
                 
             }
             {
                 XmlNode item = node.SelectSingleNode("PrintColor");
                 if (item != null)
                 pri.PParamcolor=item.InnerText.Equals("彩色");
               
             }
             {
                 XmlNode item = node.SelectSingleNode("PrintRange");
                 if (item != null)
                 pri.PParamRange= item.InnerText;
               
             }
             {
                 XmlNode item = node.SelectSingleNode("PrintDuplx");
                 if (item != null)
                  pri.PParam2Paper =item.InnerText.Equals("双面");
                 
             }
             {
                 XmlNode item = node.SelectSingleNode("PrinterPaperType");
                 if (item != null)
                  pri.Paper_Type=item.InnerText;
                
             }
             {
                 XmlNode item = node.SelectSingleNode("TimeComplete");
                 if (item != null)
                 pri.TimeCompete= item.InnerText;
               
             }
             apri.Add(pri);
       
#endif   
         }
         return apri;
     }
       /**/
     public ListViewItem GetListView() 
     {
         ListViewItem LVI = new ListViewItem(this.PrinterName);
        // LVI.SubItems.Clear();
        // LVI.SubItems.Add(this._phoneNumber);
         LVI.SubItems.Add(this._phoneNumber);
         LVI.SubItems.Add(this._phoneType);
         LVI.SubItems.Add(this._androidOs);
         LVI.SubItems.Add(this.PParamcopies.ToString());
         LVI.SubItems.Add((this.PParamcolor==true)?"彩色":"黑白");

         LVI.SubItems.Add(this.PParamRange);
         LVI.SubItems.Add((this.PParam2Paper==false)?"单面":"双面");
         LVI.SubItems.Add(this.Paper_Type);
         LVI.SubItems.Add(this.TimeCompete);
         return LVI;
     }
       /**/
    }
}
