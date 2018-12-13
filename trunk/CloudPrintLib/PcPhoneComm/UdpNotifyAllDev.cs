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
using System.Net.NetworkInformation;
namespace CloudPrintforPC
{
 public    class UdpNotifyAllDev
            {
                readonly String ip_addr;
                readonly byte[] msg;
                IPEndPoint endpoint;   
               public UdpNotifyAllDev(String ip_addr,byte[] msg,IPEndPoint endpoint) 
                {
                    this.ip_addr = ip_addr;
                    this.msg = msg;
                    this.endpoint = endpoint;

                }
                public void StartThread()
                {
                    IPAddress addr = IPAddress.Parse(ip_addr);

                    if (addr.ToString().EndsWith(".255"))
                    {
                        //广播
                        byte[] addrbyte = addr.GetAddressBytes();
                        for (int i = 1; i < 244; i++)
                        {
                            addrbyte[addrbyte.Length - 1] = (byte)i;
                            IPAddress IPOne = new IPAddress(addrbyte);
                            endpoint.Address = IPOne;
                            new SendMsg2Notify(IPOne.ToString(), msg,endpoint);
                        }
                    }
                    else {
                           endpoint.Address = addr;
                           new SendMsg2Notify(addr.ToString(), msg, endpoint);
                    }
                   
                }

            }
 public    class SendMsg2Notify
         {
                readonly String ip_addr;
                readonly byte[] msg;
                readonly IPEndPoint endpoint;                 
                 public SendMsg2Notify(String ip_addr,byte[] msg,IPEndPoint endpoint)
                  {

                       
                        this.ip_addr=ip_addr.ToString();
                        this.msg = msg;
                        this.endpoint =new IPEndPoint(IPAddress.Parse(ip_addr),endpoint.Port);
                       
                        ThreadPool.QueueUserWorkItem(SendMsg2NotifyThread, this);
                  }
                   void SendOneThread()
                  {
                      try {
                          UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
#if false
                           
                              if(NetFindfTransfer.Ping(ip_addr)){
                                  client.Send(msg, msg.Length, endpoint);

                              }
                           


#else
                          client.Send(msg, msg.Length, endpoint);
                          client.Close();
#endif

                              // Debug.Write("\nIP地址:{0}",ip_addr);
                      }catch(Exception e){
                       
                      }
                    
                  }
                   public static void SendMsg2NotifyThread(Object msg) 
                   {
                       if (msg!=null&&msg.GetType() == typeof(SendMsg2Notify))
                       {
                           SendMsg2Notify smn = (SendMsg2Notify)msg;
                           smn.SendOneThread();
                       }
                     
                   }
            
         }
    public class  GetIPaddress
    {
       
      public static IPAddress[] GetNetFindBroadcastaddr()
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
#if false
                          if(LibCui.IpIs_C_Type(ip)&&false){

                              // C 类 IP 地址--所有地址
                              IPAddress addr = IPAddress.Parse(ip);                              
                              byte[] addrbyte = addr.GetAddressBytes();
                                  for (int i = 1; i <= 254; i++)
                                  {
                                      addrbyte[addrbyte.Length - 1] = (byte)i;
                                      IPAddress IPOne = new IPAddress(addrbyte);
                                      mAddr.Add(IPOne);
                                  }
                              
                            
                          }else{
                              //A，B 类 IP地址 --广播地址
                              mAddr.Add(IPAddress.Parse(broad_cast));
                          }
#else
                          mAddr.Add(IPAddress.Parse(broad_cast));
#endif

                   }catch(Exception e){

                   }
                 

               }
           }  
           IPAddress[] ip_return = (IPAddress[])mAddr.ToArray(typeof(IPAddress));
           return ip_return;
       }
    }
    
}
