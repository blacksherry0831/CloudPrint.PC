using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BaseLib;
namespace CloudPrintLib
{
 public static   class PcCloudComm
    {

        public static void Notify_Thread()
        {

            new Thread(Notify).Start();
        }
        public static void Notify()
        {
            String url_str = ConfigurationManager.AppSettings["URL_LOCAL"];
            send("2",url_str);
        }
        public static void send(string x,string ip_str)
        {
            byte[] result = new byte[1024];
        //设定服务器IP地址  
           IPAddress ip = IPAddress.Parse(ip_str);

            using ( Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {

                clientSocket.ReceiveTimeout = 5000;
                clientSocket.SendTimeout = 5000;
                            try
                            {
                                clientSocket.Connect(new IPEndPoint(ip, 8888)); //配置服务器IP与端口 
                     
                            }
                            catch(Exception e)
                            {
                                 LogHelper.WriteLog(typeof(PcCloudComm),e.Message);
                                 Console.WriteLine("连接服务器失败，请按回车键退出！");
                                return;
                            }
                            //通过clientSocket接收数据  
                            int receiveLength = clientSocket.Receive(result);
                            // Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, receiveLength));
                            //通过 clientSocket 发送数据  

                            try
                            {

                                string sendMessage = x;
                                clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                                clientSocket.Shutdown(SocketShutdown.Both);
                                clientSocket.Close();
                                //  Console.WriteLine("向服务器发送消息：{0}" + sendMessage);
                            }
                            catch (Exception e2)
                            {
                                LogHelper.WriteLog(typeof(PcCloudComm), e2.Message);
                            }

                }


            
               
           

        }
    }
}
