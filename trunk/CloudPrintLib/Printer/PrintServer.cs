using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;//在C#中使用ArrayList必须引用Collections类
using System.Xml;
using System.Threading;
/*
 *
 *激活的打印队列
 * 
 */
namespace CloudPrintforPC
{
   public class PrintServer
    {
        private volatile Boolean mThreadRun=true;
        private ArrayList mListPrint = ArrayList.Synchronized(new ArrayList());
        public ArrayList ListPrint
        {
            get { return this.mListPrint; }
        }
        public PrintServer() {
            this.AddLocal();
            new Thread(this.SavePrinterThread).Start();
            
        }

        private void AddLocal()
        {
           List<PrintOneInfo>  so=PrintLocalPrint.LocalPrinter.GetLocalPrintersInfoDetail_ALL();
            foreach(PrintOneInfo poi in so){
                if (this.mListPrint.Contains(poi))
                {

                }
                else {
                    this.mListPrint.Add(poi);
                }
             
            
            }
        }

        public void AddNet() 
        {
         
        }
        public void SavePrinterThread()
        {
             int Last=0;
            while(mThreadRun)
            {
                int CurrentCount = this.ListPrint.Count;
                if (CurrentCount == Last)
                {
                    Thread.Sleep(1000);
                }else {
                   // this.SavePrinter2Disk();
                    Last = CurrentCount;
                    Thread.Sleep(1000);

                }
                  
            }

        }
      
        /**
         * 
         */
        public void StopServer()
        {

            this.mThreadRun = false;

        }
        /**
        * 
        */
        public String PrintCount
        {
            get {

                return this._PrinterCount().ToString();
            }
        }
       /**
        * 
        */
        private int _PrinterCount(){
            return this.ListPrint.Count;
       }
        /**
        * 
        */
    }
}
