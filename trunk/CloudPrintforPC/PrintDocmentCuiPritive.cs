using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;//在C#中使用ArrayList必须引用Collections类
using System.Drawing.Printing;
using System.Runtime.InteropServices;
namespace CloudPrintforPC
{

    class PrintDocmentCuiPritive
    {
        private static PrintDocument fPrintDocument = new PrintDocument();
        //获取本机默认打印机名称
        public static String DefaultPrinter()
        {
            return fPrintDocument.PrinterSettings.PrinterName;
        }
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name); //调用win api将指定名称的打印机设置为默认打印机
        
        /*
        public static List<PrintOneInfo> GetLocalPrintersInfo()
        {
            List<PrintOneInfo> fPrinters = new List<PrintOneInfo>();
            fPrinters.Add(new PrintOneInfo(DefaultPrinter(), PrintType.PcPrint, PrintFrom.Self)); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                PrintOneInfo poi = new PrintOneInfo(fPrinterName, PrintType.PcPrint, PrintFrom.Self);
                if (!fPrinters.Contains(poi))
                {
                    fPrinters.Add(poi);
                }
            }
            return fPrinters;
        }*/
    }
}
