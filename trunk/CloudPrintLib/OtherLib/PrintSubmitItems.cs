using CloudPrintforPC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CloudPrintLib.OtherLib
{
 public  static  class PrintSubmitItems
    {
       private static Hashtable _ht = new Hashtable(); //创建一个Hashtable实例

       static PrintSubmitItems()
        {
              ReadAllSubmitRecord();
        }

     

        public  static void Add(PrintRecordItem pst)
        {
            if (_ht.Contains(pst.GetHashKey()))
            {
                PrintRecordItem pri = (PrintRecordItem) _ht[pst.GetHashKey()];
                //是否在此更新数据
            }
            else {
                _ht.Add(pst.GetHashKey(), pst);
            }
              
        }
        public static PrintRecordItem Query(PrintRecordItem pst)
        {
            if (_ht.Contains(pst.GetHashKey()))
            {
                Object o= _ht[pst.GetHashKey()];
                if (o.GetType() == typeof(PrintRecordItem)) {

                    return (PrintRecordItem)o;
                }
                else
                {
                    return null;
                }

            } else {
                return null;
            }
                
        }

        public static PrintRecordItem QueryId(String OrderId)
        {
            if (_ht.Contains(OrderId))
            {
                Object o = _ht[OrderId];
                if (o.GetType() == typeof(PrintRecordItem))
                {

                    return (PrintRecordItem)o;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        public static void  Remove(PrintRecordItem pst)
        {
            _ht.Remove(pst.GetHashKey());
        }

        private static void ReadAllSubmitRecord()
        {
          
            String Root = PrintRecord.GetRootPath();
            DirectoryInfo d = new DirectoryInfo(Root);
            if (!d.Exists)
            {
                return;
            }
            ArrayList Flst = GetAllSubmitFile(d);
            foreach (Object a in Flst)
            {

                if (a.GetType() == typeof(String))
                {
                    List<PrintRecordItem> Lpri = PrintRecordItem.ReadXml((String)a);
                    foreach (PrintRecordItem pti in Lpri){
                        Add(pti);
                    }
                }
            }
          

        }
        /*---------------------------------------*/
        private static ArrayList GetAllSubmitFile(DirectoryInfo dir)//搜索文件夹中的文件
        {
            ArrayList FileList = new ArrayList();

            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                if (fi.FullName.Contains(PrintRecord.FileName_Submit_Order))
                {

                    FileList.Add(fi.FullName);
                }

            }

            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                FileList.AddRange( GetAllSubmitFile(d));
            }
            return FileList;
        }
        /*---------------------------------------*/

    }
}
