using CloudPrintforPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPrintLib
{
   public  class SendFilePackage
    {
        public List<ServerInfo> ServerInfoSet = new List<ServerInfo>();
        public List<String> PathSet = new List<String>();
        public List<String> SendSuccess = new List<String>();
        public List<String> SendEsc = new List<String>();
        public List<String> SendAbort = new List<String>();
        public String GetSendInfo()
        {
            String a = "发送成功：";
            foreach (String s in SendSuccess)
            {
                a += ("\n" + s);
            }
            a += ("\n" + "取消发送：");
            foreach (String s in SendEsc)
            {
                a += ("\n" + s);
            }
            a += ("\n" + "发送失败：");
            foreach (String s in SendAbort)
            {
                a += ("\n" + s);
            }
            return a;
        }
    }
}
