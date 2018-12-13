using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;
namespace CloudPrintforPC
{
    public class PrintRecord
    {
        private static readonly String FileName = "PrintRecored.xml";
        private static Mutex mMutex;
      //  private static 
        private static void Init()
        {
            if (mMutex == null)
            {
                mMutex = new Mutex(false, "PrintTEST");
            }
        }
        private void Wait()
        {
            Init();
            if (mMutex != null)
            {
                mMutex.WaitOne();
            }
        }
        private void Release()
        {
            Init();
            if (mMutex != null)
            {
                mMutex.ReleaseMutex();
            }
        }
        public static String GetRootPath() 
        {
              String mParentPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
              String mData = "打印记录";
              return mParentPath + "\\"+mData+"\\";
        }
        public static String GetFileFullPath() 
        {
          
            String forderData = DateTime.Now.ToString("yyyy年MM月dd日");
            String hour = DateTime.Now.ToString("HH时");
            string Fullpath = GetRootPath() + forderData + "\\" + hour + "\\";
            if (!Directory.Exists(Fullpath)) {
                Directory.CreateDirectory(Fullpath);
            }
            string FileFullpath = Fullpath + FileName;
            return FileFullpath;
        }
        public void WriteRecord2Disk(FileSendOnNet fson)
        {
            try
            {
                PrintRecordItem PRI = new PrintRecordItem(fson);
                String FilePath = GetFileFullPath();
                PRI.WriteThisRecord2Disk(FilePath);
            }catch(Exception e){
                LogHelper.WriteLog(this.GetType(),e);
            }
        
        }
        public static List<PrintRecordItem>  ReadAllRecord() 
        {
            List<PrintRecordItem> LPRI = new List<PrintRecordItem>();
            String Root = GetRootPath();
            DirectoryInfo d = new DirectoryInfo(Root);
            if (!d.Exists) {
                return LPRI;
            }
            ArrayList Flst = GetAll(d);
            foreach(Object a in Flst){
                
                 if(a.GetType()==typeof(String)){
                     List<PrintRecordItem> Lpri=PrintRecordItem.ReadXml((String)a);
                     LPRI.AddRange(Lpri);
                 }
            }
            return LPRI;
        }
/*-------------------------------------------------------*/
       
/*-------------------------------------------------------*/
        private static ArrayList GetAll(DirectoryInfo dir)//搜索文件夹中的文件
        {
            ArrayList FileList = new ArrayList();

            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                if (fi.Extension.ToLower().Contains(".xml")) {
                
                    FileList.Add(fi.FullName);              
                }

            }

            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
               FileList.AddRange( GetAll(d));
            }
            return FileList;
        }
          
/*---------------------------------------------------------*/
    }
}
