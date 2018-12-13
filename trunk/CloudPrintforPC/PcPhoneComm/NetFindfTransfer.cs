//#define USE_OLD_BROADCAST
#define USE_P2P_BROADCAST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Management;
namespace CloudPrintforPC
{

/**
* 
* 
* 
* 
**/
   public class NetFindfTransfer
    {
     
        ImageList mImageList;
        public PCPhoneListItem mPcPhoneList = new PCPhoneListItem();
        private volatile bool mThreadRun = true;
        private UdpClient  mUdpServer;
        private Socket mTCPServer;
        private Thread     mUdpServerThread;
        private Thread     mUdpClientNotifyThread;
        private Thread     mTcpServerThread;
        //private HttpListener mFileListener;
        //private WebServer  mWebServer;
        //三个端口选一个作为服务器
       public static readonly  int[] mUdpSetverPort=new int[]{10001,20002,30003};
       public static readonly  int[] mTcpSetverPort=new int[]{10001,20002,30003};
      
/**
 * 
 * 
 * 
 * 
 **/
        public NetFindfTransfer()
        {
            ThreadPool.SetMaxThreads(300, 1000);
            ThreadPool.SetMinThreads(300, 300);
           
            this.StartTcpServer();
            this.StartUdpServer();
            this.StartUdpNotifySelf();
            this.InitImageList();
        }
        public void StopServer() {
            this.mThreadRun = false;
            //if (this.mWebServer != null) {
            //    this.mWebServer.Stop();
            //}
            if (this.mUdpServerThread!=null)
            this.mUdpServerThread.Abort();

            if (this.mUdpClientNotifyThread != null)
            this.mUdpClientNotifyThread.Abort();

            if (this.mUdpServer != null)
            this.mUdpServer.Close();
            try {
                this.mTCPServer.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }

            try
            {
                this.mTCPServer.Close();
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            mPcPhoneList.StopServer();
           
        }
/**
* 
* 
* 
* 
**/
        public void StartTcpServer() 
        {
#if false
           this.mFileListener = new HttpListener();
#else
       
            for (int i = 0; i < mTcpSetverPort.Length; i++)
            {
                try
                {
                    mTCPServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint ipe = new IPEndPoint(IPAddress.Any,mTcpSetverPort[i]);

                    mTCPServer.Bind(ipe);
                  
                    if (this.mTCPServer != null) break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                if (this.mTCPServer == null)
                {
                    Debug.WriteLine("没有多余的端口可用使用");
                    System.Environment.Exit(0);
                }
            }
            this.mTcpServerThread = new Thread(this.TcpListenerServer);
            this.mTcpServerThread.Start();
#endif

        }
 /**
 * 
 * 
 * 
 * 
 **/
        public void StartUdpServer()
        {
          for(int i=0;i<mUdpSetverPort.Length;i++){
              try{
                   this.mUdpServer=new UdpClient(new IPEndPoint(IPAddress.Any,mUdpSetverPort[i]));
                   if(this.mUdpServer!=null) break;
              }catch(Exception e){
                  Debug.WriteLine(e.Message);
              }
             
          }
              if(this.mUdpServer==null) {
                  Debug.WriteLine("没有多余的UDP端口可用使用");
                   System.Environment.Exit(0);
              }
              this.mUdpServerThread = new Thread(this.UdpServerRecv);
              this.mUdpServerThread.Start();
        }
/**
* 
* 
* 
* 
**/
        public void StartUdpNotifySelf() 
        {

            mUdpClientNotifyThread = new Thread(UdpClientNotifySelf);
            mUdpClientNotifyThread.Start();
           
        }
/**
* 
* 
* 接受UDP广播消息
* 
**/
        public void UdpServerRecv()
        {
            UdpClient client = this.mUdpServer;
            client.Client.ReceiveTimeout = 4000;
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            while (this.mThreadRun)
            {
                try
                {
                    ServerInfo data = new ServerInfo();
                    data.mBufferRcv = client.Receive(ref endpoint);
                    data.mAddress = endpoint.Address;
                   
                    ThreadPool.QueueUserWorkItem(ProcessUdpRequest,data);

                  //  string msg = Encoding.Default.GetString(buf);
                   // Debug.WriteLine(msg);

                    //this.mChildDeviceConnected.Add
                }
                catch(Exception e) {
                    Debug.WriteLine(e.Message);
                }
                
            }
            Debug.WriteLine("Udp Server Stop");
        }
        void ProcessUdpRequest(object Context)
        {
            try
            {
                ServerInfo context = (ServerInfo)Context;
                IPAddress IPa = context.mAddress;
              
              
                if (context.ParseNotifyMessageXml())
                {
                    //解析成功
                    this.mPcPhoneList.addItem(context);
                    if (context.IsMulitiply()){
                        //广播来
                        this.UdpNotifyMsg_Reponse(IPa,0);
                    }
                    else if ("isalive".Equals(context.GetMulitiply()))
                    {
                        this.UdpNotifyMsg_Reponse(IPa,0);
                    }
                    else if("false".Equals(context.GetMulitiply())){
                        ;
                    }
                   
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Request error: " + ex);
            }
        }
/**
* 
* 任意端口想局域网广播信息
* 
* 目标端口为UDP服务器备用端口 
* 
**/
        public void UdpClientNotifySelf()
        {

            String Status=new Guid().ToString();
            
            Thread.Sleep(1000);
           // this.UdpNotifyMsg_Request(GetIPaddress.GetNetFindBroadcastaddr());           

            while (this.mThreadRun)
            {
               String current=GetNetStatus();
               if (Status.Equals(current))
               {
                   //网络环境未改变
                   Thread.Sleep(500);
               }
               else {
                   //网络环境改变
                   this.UdpNotifyMsg_Request(GetIPaddress.GetNetFindBroadcastaddr());
                   Status = current;
               }
               Thread.Sleep(500);
               
            }
            Debug.WriteLine("Udp Notify Stop");
        }
/**
* 
* 
* 
* */
        public void TcpListenerServer() 
        {
            // Start listening for client requests.
            this.mTCPServer.Listen(512);
            while(this.mThreadRun)
            {
                try
                {
                    Socket client = this.mTCPServer.Accept();
                    if(client!=null){
                        ThreadPool.QueueUserWorkItem(ProcessTcpRequest4, client);
                    }

                }
                catch (HttpListenerException e)
                {
                    Debug.WriteLine(e.Message);
                }
                catch (InvalidOperationException e)
                {

                    Debug.WriteLine(e.Message);
                }catch(Exception e){
                
                }
              
            }
            try{
                this.mTCPServer.Shutdown(SocketShutdown.Both);
            }catch(Exception e){
                
            }          
            this.mTCPServer.Close();
        }
       public bool RemoteSocketIsAlive(TcpClient client)
       {
           Socket s = client.Client;

           return s.Poll(-1, SelectMode.SelectRead);
       }
       public bool RemoteSocketIsAlive2(Socket s)
       {
                   // .Connect throws an exception if unsuccessful
           // This is how you can determine whether a socket is still connected.          
           try
           {
               byte[] tmp = new byte[1];


               s.Send(tmp, 0, 0);
               Console.WriteLine("Connected!");
               //return true;
           }
           catch (SocketException e)
           {
               // 10035 == WSAEWOULDBLOCK
               if (e.NativeErrorCode.Equals(10035))
               {
                   Console.WriteLine("Still Connected, but the Send would block");
                   // return true;
               }
               else
               {
                   Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                   //  return false;
               }
           }
           finally {

           }

           return s.Connected;

       }
      public void ProcessTcpRequest2(object Context)
      {
          Socket client = (Socket)Context;       
        
          FileStream fs =null;
          try
          {

             // PhonePcCommunication.RcvFileBody(client,out fs);
             


          }
          catch (IOException e)
          {
              Debug.WriteLine("Request error: " + e);
          }
          catch (Exception ex)
          {
              Debug.WriteLine("Request error: " + ex);
          }
          finally
          {

              if (fs != null)
              {
                  fs.Flush();
                  fs.Close();
              }
              client.Shutdown(SocketShutdown.Both);
              client.Close();
             
             /*if (ReceivedLength == FileLength)
              {
                  //  System.Diagnostics.Process.Start(
                  Process[] wordProcesses = Process.GetProcessesByName("explorer");
                  bool processExit = false;
                  foreach (Process p in wordProcesses)
                  {
                      if (p.MainWindowTitle == fileRcv.GetFolder())
                      {
                          processExit = true;
                      }
                      else
                      {

                      }
                  }
                  if (processExit == false)
                  {
                      System.Diagnostics.Process.Start("Explorer.exe", @"/select," + fileRcv.mFileFullPath);
                  }

              }
              else
              {
                  if (File.Exists(fileRcv.mFileFullPath))
                  {
                      File.Delete(fileRcv.mFileFullPath);
                  }

              }*/
              
          }
      }


      public void ProcessTcpRequest3(object Context)
      {
          Socket client = (Socket)Context;

          FileStream fs = null;
          try
          {

              //PhonePcCommunication.RcvFileBody(client, out fs);



          }
          catch (IOException e)
          {
              Debug.WriteLine("Request error: " + e);
          }
          catch (Exception ex)
          {
              Debug.WriteLine("Request error: " + ex);
          }
          finally
          {

              if (fs != null)
              {
                  fs.Flush();
                  fs.Close();
              }
              client.Shutdown(SocketShutdown.Both);
              client.Close();

              /*if (ReceivedLength == FileLength)
               {
                   //  System.Diagnostics.Process.Start(
                   Process[] wordProcesses = Process.GetProcessesByName("explorer");
                   bool processExit = false;
                   foreach (Process p in wordProcesses)
                   {
                       if (p.MainWindowTitle == fileRcv.GetFolder())
                       {
                           processExit = true;
                       }
                       else
                       {

                       }
                   }
                   if (processExit == false)
                   {
                       System.Diagnostics.Process.Start("Explorer.exe", @"/select," + fileRcv.mFileFullPath);
                   }

               }
               else
               {
                   if (File.Exists(fileRcv.mFileFullPath))
                   {
                       File.Delete(fileRcv.mFileFullPath);
                   }

               }*/

          }
      }
      public void ProcessTcpRequest4(object Context)
      {
          Socket client=null;
          try
          {
              client = (Socket)Context;
              PhonePcCommunication.RcvFileFromClient(client);
              client.Shutdown(SocketShutdown.Both);
          }
          catch (SocketException e)
          {
              LogHelper.WriteLog(typeof(NetFindfTransfer), e);
          }
          finally {
              if (client != null) {
               
                  client.Close();
                  client = null;
              }
          }
         
      }
      public void UdpNotifyMsg_Reponse(IPAddress addr,int port)
      {
            byte[] buf = this.GetNotifyMessageXml(false);
          
            for (int i = 0; i < mUdpSetverPort.Length; i++)
            {
                IPEndPoint endpoint = new IPEndPoint(addr, mUdpSetverPort[i]);
                SendMsg2Notify sm2n = new SendMsg2Notify(addr.ToString(), buf, endpoint);
            }
  

      }
      public void UdpNotifyMsg_Request(IPAddress[] addrArray)
      {

#if false
           UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
          foreach (IPAddress addr in addrArray)
          {
              byte[] buf = this.GetNotifyMessageXml(addr.ToString().EndsWith(".255"));
              for (int i = 0; i < mUdpSetverPort.Length; i++)
              {
                  IPEndPoint endpoint = new IPEndPoint(addr, mUdpSetverPort[i]);
                  client.Send(buf, buf.Length, endpoint);
              }
          }
          client.Close();
#endif
#if false
          foreach(IPAddress addr in addrArray){
               byte[] buf = this.GetNotifyMessageXml(addr.ToString().EndsWith(".255"));
                   for (int i = 0; i < mUdpSetverPort.Length; i++){                 
                            IPEndPoint endpoint = new IPEndPoint(addr, mUdpSetverPort[i]);
                            UdpNotifyAllDev unad=new UdpNotifyAllDev(addr.ToString(), buf, endpoint);
                            unad.StartThread();
                      }          
          } 
#endif        
#if true
          byte[] buf = this.GetNotifyMessageXml(true);
          foreach (IPAddress addr in addrArray)
          {
             
              for (int i = 0; i < mUdpSetverPort.Length; i++)
              {
                  IPEndPoint endpoint = new IPEndPoint(addr, mUdpSetverPort[i]);
                  SendMsg2Notify sm2n = new SendMsg2Notify(addr.ToString(),buf,endpoint);
              }
          } 
#endif
      }
       private byte[] GetNotifyMessageXml(bool IsMulity)
      {

          byte[] buf = null;

          XmlDocument doc = new XmlDocument();
          XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
          doc.AppendChild(dec);

          XmlElement root = doc.CreateElement("CloudPrint");

          XmlNode node = doc.CreateElement("Notify");

          XmlElement elementPort = doc.CreateElement("Port");
          if (this.mTCPServer == null)
          {
              elementPort.InnerText = "10001";
          }
          else
          {
              elementPort.InnerText = ((IPEndPoint)(this.mTCPServer.LocalEndPoint)).Port.ToString();
          }

          node.AppendChild(elementPort);

          XmlElement elementHostType = doc.CreateElement("HostType");
          elementHostType.InnerText = "1";
          node.AppendChild(elementHostType);

          XmlElement elementHostName = doc.CreateElement("HostName");
          elementHostName.InnerText = "pc";
          node.AppendChild(elementHostName);

          XmlElement elementPhoneType = doc.CreateElement("PhoneType");
          elementPhoneType.InnerText = "n";
          node.AppendChild(elementPhoneType);

          XmlElement elementPcName = doc.CreateElement("PcName");
          elementPcName.InnerText = Dns.GetHostName();
          node.AppendChild(elementPcName);

          XmlElement elementPhoneNameAlias = doc.CreateElement("PhoneNameAlias");
          elementPhoneNameAlias.InnerText = "n";
          node.AppendChild(elementPhoneNameAlias);

          {
              XmlElement element = doc.CreateElement("MachineMac");
              element.InnerText =LibCui.GetLocalMacAddress();
              node.AppendChild(element);
          }

          XmlElement elementMultiply = doc.CreateElement("Multiply");
          if (IsMulity)
          {
              elementMultiply.InnerText = "true";
          }
          else
          {
              elementMultiply.InnerText = "false";
          }

          node.AppendChild(elementMultiply);









          root.AppendChild(node);

          doc.AppendChild(root);
          buf = Encoding.UTF8.GetBytes(doc.InnerXml);
          // doc.Save("Notify.xml");
          return buf;
      }
/**
* 
* 
* 
* 
**/
     public void InitImageList()
     {
         this.mImageList = new ImageList();
         this.mImageList.ImageSize = new System.Drawing.Size(36, 36);
         this.mImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
         this.mImageList.Images.Add("1", Properties.Resources.Computer);
         this.mImageList.Images.Add("2",Properties.Resources.phone);
     }

       public ImageList ImageIcon{
              get{      return this.mImageList;     }
      }


       /// <summary>  
       /// 是否能 Ping 通指定的主机  
       /// </summary>  
       /// <param name="ip">ip 地址或主机名或域名</param>  
       /// <returns>true 通，false 不通</returns>  
       public static bool Ping(string ip)
       {
           System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
           System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
           options.DontFragment = true;
           string data = "Test!";
           byte[] buffer = Encoding.ASCII.GetBytes(data);
           int timeout = 1000; // Timeout 时间，单位：毫秒  
           try {
                          System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
                       if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                           return true;
                       else
                           return false;
           }catch(Exception e){
                  return false;
           }
         
       } 
/**
* 
* 
* 
* 
**/
#if false
       public String GetNetStatus() 
       {
           String status="";
           NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

           foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
           {
              
               IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();
              

               UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;

               foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
               {
                   

                   if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                   {
                       if (Ping(UnicastIPAddressInformation.Address.ToString())){
                           status += UnicastIPAddressInformation.Address.ToString();
                       }


                   }

               }

           }
           return status;
       }
#else

        public String GetNetStatus() 
       {
           String status="";
         
             ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
           ManagementObjectCollection nics = mc.GetInstances();
           foreach (ManagementObject nic in nics)
           {
               if (Convert.ToBoolean(nic["ipEnabled"]) == true)
               {
                   try
                   {
                       String ip = (nic["IPAddress"] as String[])[0];
                       String mask = (nic["IPSubnet"] as String[])[0];
                       String gate_way = (nic["DefaultIPGateway"] as String[])[0];
                       status += ip;
                   }
                   catch (Exception e)
                   {

                   }
                   

               }
           }  
           
           return status;
       }

#endif


     
/**
* 
* 
* 
* 
**/
#if false
       public static IPAddress[] GetUdpBroadcastaddr()
       {
           ArrayList mAddr = new ArrayList();
         
           NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

           foreach (NetworkInterface NetworkIntf in NetworkInterfaces)
           {

               IPInterfaceProperties IPInterfaceProperties = NetworkIntf.GetIPProperties();


               UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;

               foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
               {


                   if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                   {
                       if (Ping(UnicastIPAddressInformation.Address.ToString()))
                       {
                           if (UnicastIPAddressInformation.Address != null && UnicastIPAddressInformation.IPv4Mask!=null)
                           {
                               String  ip= UnicastIPAddressInformation.Address.ToString();
                               String  mask= UnicastIPAddressInformation.IPv4Mask.ToString();
                               String  broad_cast=GetBroadcast(ip,mask);
                               mAddr.Add(IPAddress.Parse(broad_cast));
                          }
                          

                       }


                   }

               }

           }
           IPAddress[] ip_return = (IPAddress[]) mAddr.ToArray(typeof(IPAddress));
           return ip_return;
       }
#endif
#if false
       public static IPAddress[] GetUdpBroadcastaddr()
       {
           ArrayList mAddr = new ArrayList();

           ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
           ManagementObjectCollection nics = mc.GetInstances();
           foreach (ManagementObject nic in nics)
           {
               if (Convert.ToBoolean(nic["ipEnabled"]) == true)
               {
                   try {
                          String ip = (nic["IPAddress"] as String[])[0];
                          String mask=(nic["IPSubnet"] as String[])[0];
                          String gate_way=(nic["DefaultIPGateway"] as String[])[0];
                          String broad_cast = LibCui.GetBroadcast(ip, mask);
                           mAddr.Add(IPAddress.Parse(broad_cast));
                   }catch(Exception e){

                   }
                 

               }
           }  
           IPAddress[] ip_return = (IPAddress[])mAddr.ToArray(typeof(IPAddress));
           return ip_return;
       }
   
#endif
    }

}
