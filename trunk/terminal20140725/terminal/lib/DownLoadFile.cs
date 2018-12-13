using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication2;
using System.Data;
using System.Data.SqlClient;

using Classlib;

namespace ConsoledownloadFile
{
   public class DownLoadFile
    {
        private string workDirectory = "";

        public string WorkDirectory
        {
            get { return workDirectory; }
            set { workDirectory = value; }
        }
        private string url="";

        public DownLoadFile(string urlCloud)
        {
            workDirectory = System.IO.Directory.GetCurrentDirectory()+@"\下载文档\";
            url = urlCloud;
         }
       public void  getFileFromCloud(List<Object> orderlist)
       {
          // ClassData c = new ClassData();
          // string constr = c.readconfig();
           //SqlConnection conn = new SqlConnection(constr);
         //  SqlCommand cmd = conn.CreateCommand();
          string  path = workDirectory;
           
           for (int i = 0; i < orderlist.Count; i++)           
           {
              // path += "\\"+((Orders)orderlist[i]);
               //if (!System.IO.Directory.Exists(path))
                 //  System.IO.Directory.CreateDirectory(path);
                    string lpath = path;
                    string posturl = url+"?fileID="+((Orders)orderlist[i]).FileID;
                    HttpClient obj = new HttpClient(posturl);
                    lpath+= "\\" + ((Orders)orderlist[i]).FileID+ "." + ((Orders)orderlist[i]).Filetype;
                    obj.SaveAsFile(lpath);   
                
           }
       }
    }
}
