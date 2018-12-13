using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Net;
using BaseLib;
using System.Diagnostics;

namespace CloudPrint4PC_WPF
{
 public   class UpdatePackage
    {
        public static String URL_BASE = "http://blacksherry-software-distribution.oss-cn-shenzhen.aliyuncs.com/windows/qindayin/";
        public static String PATH_EXE= Assembly.GetExecutingAssembly().Location;
        public static String WriteVersion2Disk()
        {
            String ver = "ver.json";
            String format = "yyyy-MM-dd-HH-mm-ss";
            String CompileTime = System.IO.File.GetLastWriteTime(PATH_EXE).ToString(format);

            JObject jo = new JObject();

            jo["compileTime"] = CompileTime;
            jo["url_latest"] = URL_BASE + "setup" + CompileTime + ".exe";
            jo["url_server"] = "*.*.*.*";
            jo.ToString();


            using (StreamWriter sw = new StreamWriter(ver))
            {
                sw.Write(jo.ToString());
            }
            return jo.ToString();
            
        }

        public static String GetVersionFromNet()
        {



            return "";
        }

        public static void Check2Download()
         {
            try {
                         String CompileTime = WriteVersion2Disk();
                         JObject jo_local = JObject.Parse(CompileTime);
                         String NetVersion =GetHttp(URL_BASE+"ver.json");
                         JObject jo_net = JObject.Parse(NetVersion);

                if (jo_local["compileTime"].ToString().Equals(jo_net["compileTime"].ToString()))
                {
                   //版本一致
                }
                else {
                        System.Diagnostics.Process.Start(jo_net["url_latest"].ToString());
                }

                             
            }catch (Exception e) {
                LogHelper.WriteLog(e.GetType(),e);
                Debug.WriteLine(e);
            }
              

         }

        public static void Check2DownloadThread()
        {
            new Thread(Check2Download).Start();
        }
        public static string GetHttp(string url)
        {
           // string queryString = "?";

           

          //  queryString = queryString.Substring(0, queryString.Length - 1);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            //httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }



    }
      
    
}
