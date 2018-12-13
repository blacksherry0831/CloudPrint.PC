using BaseLib;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PrintJobMonitor
{
  public static  class FilePages
    {
        private static int PAGE_MAX=int.MaxValue;
        public static int GetFilePages(String FilePath)
        {
            int pages_t= -1;
            if (FilePath.ToLower().EndsWith("pdf")) {
                return pages_t = GetPdfPages(FilePath);
            } else if (FilePath.ToLower().EndsWith("doc") ||
                FilePath.ToLower().EndsWith("docx")) {
                return pages_t = GetWordPages(FilePath);
            }
            else if (
                FilePath.ToLower().EndsWith("jpg") ||
                FilePath.ToLower().EndsWith("jpeg") ||
                FilePath.ToLower().EndsWith("png")  ||
                FilePath.ToLower().EndsWith("bmp")
                )
            {
                return 10000;
            }
            else if (FilePath.ToLower().EndsWith("xlsx") ||
                     FilePath.ToLower().EndsWith("xlsm") ||
                     FilePath.ToLower().EndsWith("xls"))
            {

                return 10000;
            }
            else {
                return pages_t;
            }
        }

        public static int GetPdfPages( string docFile)
        {
            int pages_t = -1;
            try
            {

                if (docFile == null || !docFile.ToLower().EndsWith("pdf"))
                {
                    return pages_t;
                }

                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(docFile);
                return   pages_t = reader.NumberOfPages;

             
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLogError(typeof(FilePages), ex);
            }
            return pages_t;
        }

        public static int GetWordPages(string docFile)
        {
            int pages_t = -1;
            string msg = "";
            Microsoft.Office.Interop.Word.Document doc=null;
            Microsoft.Office.Interop.Word.Application appWord = null;
            try
            {

                if (docFile == null || !(docFile.ToLower().EndsWith("doc") || docFile.ToLower().EndsWith("docx")))
                {
                    msg = "非word文件: " + docFile;
                    return -999;
                }


                object wordFile = docFile;

                object oMissing = Missing.Value;

                //自定义object类型的布尔值
                object oTrue = true;
                object oFalse = false;

                object doNotSaveChanges = WdSaveOptions.wdDoNotSaveChanges;

                //定义WORD Application相关
                appWord = new Microsoft.Office.Interop.Word.Application();

                //WORD程序不可见
                appWord.Visible = false;
                //不弹出警告框
                appWord.DisplayAlerts = WdAlertLevel.wdAlertsNone;



                //打开要打印的文件
                doc = appWord.Documents.Open(
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
                pages_t = doc.ComputeStatistics(stat, ref oMissing);//.ComputeStatistics(stat, ref Nothing); 
                //打印完关闭WORD文件
                doc.Close(ref doNotSaveChanges, ref oMissing, ref oMissing);

                //退出WORD程序
                appWord.Quit(ref oMissing, ref oMissing, ref oMissing);

                doc = null;
                appWord = null;

                return pages_t;

            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                LogHelper.WriteLogError(typeof(FilePages),ex.Message);
                pages_t = -997;
            }
            finally {

            }

            //this.textBox1.Text = msg;
            return pages_t;
        }
    }
}
