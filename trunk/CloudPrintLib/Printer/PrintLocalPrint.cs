using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Printing;
using System.Management;
using System.IO;
using System.Collections;
using BaseLib;

namespace CloudPrintforPC
{
   public static class PrintLocalPrint
    {
      public class Externs
        {
            [DllImport("winspool.drv")]
            public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
        }
      public  static class LocalPrinter
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
        public static bool HaveOnePclPrinter()
        {

            return PrintLocal.HaveOnePclPrinter();
           
        }

       //private static bool InitPrinterTable()
       // {
       //     ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer");
       //     using (StreamWriter sw = new StreamWriter("printerinfo.txt")) {
       //         foreach (ManagementObject queryObj in searcher.Get())
       //                 {
       //                         String deviceId_t = queryObj["DeviceID"].ToString();
       //                         String driverName = queryObj["DriverName"].ToString();
       //                         if (!_printerTable.Contains(deviceId_t)) {
       //                                 _printerTable.Add(deviceId_t, queryObj);
       //                         }
       //                         if (!_printerTable.Contains(driverName)) {
       //                                 _printerTable.Add(driverName, queryObj);
       //                          }
                                

       //                         sw.WriteLine("-----------------------------------");
       //                         sw.WriteLine("Win32_Printer instance");
       //                         sw.WriteLine("-----------------------------------");
       //                         sw.WriteLine("Default: {0}", queryObj["Default"]);
       //                         sw.WriteLine("DeviceID: {0}", queryObj["DeviceID"]);
       //                         sw.WriteLine("DriverName: {0}", queryObj["DriverName"]);
       //                         System.UInt16[] lan = (UInt16[]) queryObj["LanguagesSupported"];
       //                         if (lan != null)
       //                         {
       //                              sw.WriteLine("LanguagesSupported: {0}", queryObj["LanguagesSupported"].ToString()); //LanguagesSupported
       //                         }
       //                          lan = (UInt16[])queryObj["CurrentLanguage"];
       //                         if (lan != null)
       //                         {
       //                             sw.WriteLine("CurrentLanguage: {0}", queryObj["CurrentLanguage"].ToString()); //LanguagesSupported
       //                         }
       //                         lan = (UInt16[]) queryObj["DefaultLanguage"];
       //                         if (lan != null)
       //                         {
       //                             sw.WriteLine("DefaultLanguage: {0}", queryObj["DefaultLanguage"].ToString()); //LanguagesSupported
       //                         }

       //                          sw.WriteLine("SystemName: {0}", queryObj["SystemName"]);
       //                          sw.WriteLine("PrinterStatus: {0}", queryObj["PrinterStatus"]);//PrinterStatus
       //                          sw.WriteLine("CurrentNaturalLanguage: {0}", queryObj["CurrentNaturalLanguage"]);//PrinterStatus

       //             }
       //         sw.Close();

       //     }
       //     return true;
       // }



        /**
        *
        * 
        * 
*/

        //public static String GetPrinterDrivenName(String fPrinterName)
        //{
        //    String DriveName = "";
        //    if (_printerTable.Contains(fPrinterName))
        //    {
        //        ManagementObject mo = (ManagementObject)_printerTable[fPrinterName];
        //        DriveName = mo["DriverName"].ToString();
                
        //    }
        //    return DriveName;
        //}
        /**
        *
        * 
        * 
*/

        //private static Hashtable _printerTable = new Hashtable(); //  创建哈希表
        //static PrintLocalPrint()
        //{
        //    InitPrinterTable();
        //}
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
