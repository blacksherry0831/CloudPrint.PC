using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;
using CloudPrintLib.OtherLib;
using BaseLib;

namespace CloudPrintforPC
{
    public class PrintRecord
    {
        public static readonly String FileName = "PrintRecored.xml";
        public static readonly String FileName_Submit_Order = "PrintRecored_Submit.xml";
       // private static PrintSubmitItems _SubmitItems=new PrintSubmitItems();
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
            //根路径
            //C:\\Users\\Administrator\\AppData\\Local
              String mParentPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
            
              String mData = "打印记录";
              return mParentPath + "\\"+mData+"\\";
        }
       /* public static String GetSubmitRootPath()
        {
            String mParentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            String mData = "亲打印提交记录";
            return mParentPath + "\\" + mData + "\\";
        }*/
       /* public static String GetSubmitFileFullPath()
        {

            String forderData = DateTime.Now.ToString("yyyy年MM月dd日");
            String hour = DateTime.Now.ToString("HH时");
            string Fullpath = GetSubmitRootPath() + forderData + "\\" + hour + "\\";
            if (!Directory.Exists(Fullpath))
            {
                Directory.CreateDirectory(Fullpath);
            }
            string FileFullpath = Fullpath + FileName;
            return FileFullpath;
        }*/
        /// <summary>
        /// 打印记录
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 提交记录
        /// </summary>
        /// <returns></returns>
        public static String GetFileFullPath_SubmitPath()
        {
            String forderData = DateTime.Now.ToString("yyyy年MM月dd日");
            String hour = DateTime.Now.ToString("HH时");
            string Fullpath = GetRootPath() + forderData + "\\" + hour + "\\";
            if (!Directory.Exists(Fullpath))
            {
                Directory.CreateDirectory(Fullpath);
            }
            string FileFullpath = Fullpath + FileName_Submit_Order;
            return FileFullpath;
        }
        public static String GetFileFullPath_SubmitDocPath()
        {
            String forderData = DateTime.Now.ToString("yyyy年MM月dd日");
            String hour = DateTime.Now.ToString("HH时");
            string Fullpath = GetRootPath() + forderData + "\\" + hour + "\\";
            if (!Directory.Exists(Fullpath))
            {
                Directory.CreateDirectory(Fullpath);
            }
            string FileFullpath = Fullpath;
            return FileFullpath;
        }
        /// <summary>
        /// 写入打印记录
        /// </summary>
        /// <param name="fson"></param>
        public void WriteRecord2Disk(FileSendOnNet fson)
        {
            this.Wait();
            try
            {
                PrintRecordItem PRI = new PrintRecordItem(fson);
                String FilePath = GetFileFullPath();
                PRI.WriteThisRecord2Disk(FilePath,true);
            }catch(Exception e){
                LogHelper.WriteLog(this.GetType(),e);
            }
            this.Release();
        
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


        /**
        *       保存提交信息到本地，
        *写入提交记录
        *
        */
        public void WriteSubmitRecord2Disk_NoPay(FileSendOnNet fson)
        {
            /*
            * 文件拷贝
            */
            {
                String from = fson.FileFullPath;
                String to = GetFileFullPath_SubmitDocPath() +fson.GetFileName();
                File.Move(from, to);
                fson.FileFullPath = to;
            }
           
            this.Wait();
            try
            {
                PrintRecordItem PRI = new PrintRecordItem(fson);

                PRI.WriteSubmitRecord2Disk(true);

                PrintSubmitItems.Add(PRI);
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(this.GetType(), e);
            }
            this.Release();

        }
        /*public void CopyFileFromTo(String from,String to)
        {
           
            File.Move(from, to);
        }*/
       //static public PrintSubmitItems SubmitItems
       // {
       //     get{ return _SubmitItems; }
            
       //  }

    }
}
