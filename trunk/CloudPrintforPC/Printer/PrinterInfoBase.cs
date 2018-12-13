using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using PCLConvertCS;
using System.IO;
using System.Printing;
using System.Drawing.Printing;

namespace CloudPrintforPC
{
    public class PrinterInfoBase
    {
        private PrintQueue mPrinterQueue;
        public String mPrintName;
        protected bool _duplex=false;
        protected bool _support_color=false;
        protected List<PaperSize> _paper_sizes=new List<PaperSize>();
       public   PrinterInfoBase(String name)
         {

             this.mPrintName = name;
#if false
             this.mPrinterQueue = PrintLocalPrint.LocalPrinter.GetPrintQueue(name);
             this._duplex = CanDuplex(this.mPrintName);
             this._support_color = SupportColor(name);

             this._paper_sizes = GetListPaperSize(name);
             this.mPrinterQueue = PrintLocalPrint.LocalPrinter.GetPrintQueue(name);
#endif
         }
       public static bool CanDuplex(String name) {
           PrintDocument pd = new PrintDocument();
           pd.PrinterSettings.PrinterName = name;
           if (pd.PrinterSettings.IsValid)
           {
          
               return pd.PrinterSettings.CanDuplex;
           }
           else {
               return false;
           }
        

       
       }
       public static bool SupportColor(String name)
       {
           PrintDocument pd = new PrintDocument();
           pd.PrinterSettings.PrinterName = name;
           if (pd.PrinterSettings.IsValid)
           {
           
               return pd.PrinterSettings.SupportsColor;
           }
           else {
               return false;
           }
     


       }

       public List<PaperSize> GetListPaperSize(String name) 
       {
           List<PaperSize> PaperSizeSet = new List<PaperSize>();
           PaperSize pkSize;
           PrintDocument pd = new PrintDocument();
           pd.PrinterSettings.PrinterName = name;

           if (pd.PrinterSettings.IsValid)
           {
               for (int i = 0; i < pd.PrinterSettings.PaperSizes.Count; i++)
               {
                   pkSize = pd.PrinterSettings.PaperSizes[i];
                   PaperSizeSet.Add(pkSize);
               }
               return PaperSizeSet;
           }
           else {

                  return null;
           
           }
          
       }
         
    }
}
