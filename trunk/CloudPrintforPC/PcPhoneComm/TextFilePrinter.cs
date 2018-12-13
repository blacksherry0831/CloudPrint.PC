using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;

namespace PrintService
{


    sealed class TextFilePrinter
    {
        string sTreamPriStr;
        Encoding theEncode;
        Font theFont;
        StreamReader srToPrint;
        int currPage;

        public TextFilePrinter(string sTreamPriStr)
            : this(sTreamPriStr, Encoding.GetEncoding("utf-8"), new Font("新宋体", 33,GraphicsUnit.Millimeter))
        {
        }

        public TextFilePrinter(string sTreamPriStr, Encoding theEncode, Font theFont)
        {
            this.sTreamPriStr = sTreamPriStr;
            this.theEncode = theEncode;
            this.theFont = theFont;
        }

        public void Print()
        {
            srToPrint = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sTreamPriStr)));
            PrintDialog dlg = new PrintDialog();
            dlg.Document = GetPrintDocument();
            dlg.AllowSomePages = true;
            dlg.AllowPrintToFile = false;
            if (dlg.ShowDialog() == DialogResult.OK) dlg.Document.Print();


        }

        /// <summary>
        /// 不需要打印预览直接打印
        /// </summary>
        public void Print2()
        {
            srToPrint = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sTreamPriStr)));
            PrintDialog dlg = new PrintDialog();
            dlg.Document = GetPrintDocument();
            dlg.AllowSomePages = true;
            dlg.AllowPrintToFile = false;
            dlg.Document.Print();
        }

        public void View()
        {
            srToPrint = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(sTreamPriStr)));
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = GetPrintDocument();
            dlg.ShowDialog();
        }

        PrintDocument GetPrintDocument()
        {
            currPage = 1;
            PrintDocument doc = new PrintDocument();
            doc.DocumentName = "打印";
            doc.PrintPage += new PrintPageEventHandler(PrintPageEvent);
            return doc;
        }


        void PrintPageEvent(object sender, PrintPageEventArgs ev)
        {
            string line = null;
            float linesPerPage = ev.MarginBounds.Height / theFont.GetHeight(ev.Graphics);
            bool isSomePages = ev.PageSettings.PrinterSettings.PrintRange == PrintRange.SomePages;
            if (isSomePages)
            {
                while (currPage < ev.PageSettings.PrinterSettings.FromPage)
                {
                    for (int count = 0; count < linesPerPage; count++)
                    {
                        line = srToPrint.ReadLine();
                        if (line == null) break;
                    }
                    if (line == null) return;
                    currPage++;
                }
                if (currPage > ev.PageSettings.PrinterSettings.ToPage) return;
            }
            for (int count = 0; count < linesPerPage; count++)
            {
                line = srToPrint.ReadLine();
                if (line == null) break;
                //ev.Graphics.DrawString(line, theFont, Brushes.Black, ev.MarginBounds.Left,
                //  ev.MarginBounds.Top + (count * theFont.GetHeight(ev.Graphics)), new StringFormat());

                ev.Graphics.DrawString(line, theFont, Brushes.Black, 2,
                  count * theFont.GetHeight(ev.Graphics) - 1, new StringFormat());
            }
            currPage++;
            if (isSomePages && currPage > ev.PageSettings.PrinterSettings.ToPage) return;
            if (line != null) ev.HasMorePages = true;
        }
    }

    public static class PrinterHel
    {
        //GetDefaultPrinter用到的API函数说明 
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int size);

        //SetDefaultPrinter用到的API函数声明 
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetDefaultPrinter(string Name);

        #region 获取本地打印机列表
        /// <summary> 
        /// 获取本地打印机列表 
        /// </summary> 
        /// <returns>打印机列表</returns> 
        public static List<string> GetPrinterList()
        {
            return CloudPrintforPC.PrintLocalPrint.LocalPrinter.GetLocalPrinters();
        }
        #endregion 获取本地打印机列表

        #region 获取本机的默认打印机名称
        /// <summary> 
        /// 获取本机的默认打印机名称 
        /// </summary> 
        /// <returns>默认打印机名称</returns> 
        public static string GetDeaultPrinterName()
        {
            StringBuilder dp = new StringBuilder(256);
            int size = dp.Capacity;
            if (GetDefaultPrinter(dp, ref size))
            {
                return dp.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion 获取本机的默认打印机名称

        #region 设置默认打印机
        /// <summary> 
        /// 设置默认打印机 
        /// </summary> 
        /// <param name="PrinterName">可用的打印机名称</param> 
        public static void SetPrinterToDefault(string PrinterName)
        {
            SetDefaultPrinter(PrinterName);
        }
        #endregion 设置默认打印机

        #region 判断打印机是否在系统可用的打印机列表中
        ///// <summary> 
        ///// 判断打印机是否在系统可用的打印机列表中 
        ///// </summary> 
        ///// <param name="PrinterName">打印机名称</param> 
        ///// <returns>是：在；否：不在</returns> 
        public static bool PrinterInList(string PrinterName)
        {
            bool bolRet = false;
            List<string> alPrinters = GetPrinterList();
            for (int i = 0; i < alPrinters.Count; i++)
            {
                if (PrinterName == alPrinters[i].ToString())
                {
                    bolRet = true;
                    break;
                }
            }
            alPrinters.Clear();
            alPrinters = null;
            return bolRet;
        }
        #endregion 判断打印机是否在系统可用的打印机列表中
    }


}
