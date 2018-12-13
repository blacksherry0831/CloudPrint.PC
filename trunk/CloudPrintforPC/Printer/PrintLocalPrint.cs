using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Printing;
namespace CloudPrintforPC
{
   public class PrintLocalPrint
    {
      public class Externs
        {
            [DllImport("winspool.drv")]
            public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
        }
      public  class LocalPrinter
        {
            private static PrintDocument fPrintDocument = new PrintDocument();
            //获取本机默认打印机名称
            public static String DefaultPrinter()
            {
                return fPrintDocument.PrinterSettings.PrinterName;
            }
            public static List<String> GetLocalPrinters()
            {
                List<String> fPrinters = new List<String>();
                fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
                foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                {
                    if (!fPrinters.Contains(fPrinterName))
                    {
                        fPrinters.Add(fPrinterName);
                    }
                }
                return fPrinters;
            }

            public static List<PrintOneInfo> GetLocalPrintersInfo()
            {
                PrintOneInfo poi = null;
                List<PrintOneInfo> fPrinters = new List<PrintOneInfo>();
                poi=new PrintOneInfo(   DefaultPrinter(),
                                        PrintType.PcPrint,
                                        PrintFrom.Self,
                                        false,
                                        false);
                fPrinters.Add(poi); //默认打印机始终出现在列表的第一项
                foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                {
                    poi = new PrintOneInfo(
                                            fPrinterName,
                                            PrintType.PcPrint,
                                            PrintFrom.Self,
                                            false,
                                            false);
                    if (!fPrinters.Contains(poi))
                    {
                        fPrinters.Add(poi);
                    }
                }
                return fPrinters;
            }
/**
*
* 非离线的打印机
* 
*/
            public static List<PrintOneInfo> GetLocalPrintersInfoDetail()
            {
                PrintOneInfo poi = null;
                List<PrintOneInfo> fPrinters = new List<PrintOneInfo>();
#if true
               
                    poi = new PrintOneInfo(DefaultPrinter(),
                                            PrintType.PcPrint,
                                            PrintFrom.Self,
                                            Printer.GetPrinterStatus(DefaultPrinter()));
                /*----------------------------------------------------------------------------------*/
                if(poi.IsOffline()==false){
                    fPrinters.Add(poi); //默认打印机始终出现在列表的第一项
                }
                /*----------------------------------------------------------------------------------*/
#endif

                foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                {
                    poi = new PrintOneInfo( fPrinterName,
                                            PrintType.PcPrint,
                                            PrintFrom.Self,
                                            Printer.GetPrinterStatus(fPrinterName));
                    if (!fPrinters.Contains(poi)&&(poi.IsOffline()==false))
                    {
                        fPrinters.Add(poi);
                    }
                }
                /*----------------------------------------------------------------------------------*/
         

                return fPrinters;
            }
/**
*
* 
* 
*/
            public static List<PrintOneInfo> GetLocalPrintersInfoDetail_ALL()
            {
                PrintOneInfo poi = null;
                List<PrintOneInfo> fPrinters = new List<PrintOneInfo>();
#if true

                poi = new PrintOneInfo(DefaultPrinter(),
                                        PrintType.PcPrint,
                                        PrintFrom.Self,
                                        Printer.GetPrinterStatus(DefaultPrinter()));
                /*----------------------------------------------------------------------------------*/
               
                {
                    fPrinters.Add(poi); //默认打印机始终出现在列表的第一项
                }
                /*----------------------------------------------------------------------------------*/
#endif

                foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                {
                    poi = new PrintOneInfo(fPrinterName,
                                            PrintType.PcPrint,
                                            PrintFrom.Self,
                                            Printer.GetPrinterStatus(fPrinterName));
                    if (!fPrinters.Contains(poi))
                    {
                        fPrinters.Add(poi);
                    }
                }
                /*----------------------------------------------------------------------------------*/


                return fPrinters;
            }
/*
 *
 *
 * 
 * 
 */
            public static PrintQueue GetPrintQueue(string PrinterName)
            {
                LocalPrintServer pr = new LocalPrintServer();
                pr.Refresh();
                EnumeratedPrintQueueTypes[] enumerationFlags = {EnumeratedPrintQueueTypes.Local,
                                                            EnumeratedPrintQueueTypes.Connections,
                                                           };
                foreach (PrintQueue pq in pr.GetPrintQueues(enumerationFlags))
                {
                    if (pq.Name == PrinterName)
                    {
                        return pq;
                    }
                }
                return null;
            }
        }
/**
*
* 
* 
*/
      

      
/**
*
* 
* 
*/


  
  /**
  *
  * 状态枚举
  * 
  */
 
 /**
 *
 * 
 * 
 */
    }
}
