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
    class LibCui
    {
        /// <summary>
        /// 获得广播地址
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="subnetMask">子网掩码</param>
        /// <returns>广播地址</returns>
        public static string GetBroadcast(string ipAddress, string subnetMask)
        {

            byte[] ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            byte[] sub = IPAddress.Parse(subnetMask).GetAddressBytes();

            // 广播地址=子网按位求反 再 或IP地址
            for (int i = 0; i < ip.Length; i++)
            {
                ip[i] = (byte)((~sub[i]) | ip[i]);
            }
            return new IPAddress(ip).ToString();
        }       
        public static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }
        public static string IntToIp(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }
        public static IPAddress long2IPAddress(long ipInt) 
        {
             
            uint netInt = (uint)IPAddress.HostToNetworkOrder((Int32)ipInt);
            IPAddress ipaddr = new IPAddress((long)netInt);
            return ipaddr;
        }
        public static IPAddress[] GetUdpLocaladdrList()
        {
            ArrayList mAddr = new ArrayList();
            String ip;
            String mask;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection nics = mc.GetInstances();
            foreach (ManagementObject nic in nics)
            {
                if (Convert.ToBoolean(nic["ipEnabled"]) == true)
                {
                    ip = (nic["IPAddress"] as String[])[0];
                    mask = (nic["IPSubnet"] as String[])[0];
                    String gate_way = (nic["DefaultIPGateway"] as String[])[0];
                    String broad_cast = GetBroadcast(ip, mask);
                    GetLocaladdrList(ip,mask);

                }
            }
            IPAddress[] ip_return = (IPAddress[])mAddr.ToArray(typeof(IPAddress));
            return ip_return;
        }
        public static void GetLocaladdrList(String ip, string mask)
        {

            long ipInt = IpToInt(ip);
            long maskInt = IpToInt(mask);
            long ipStart=ipInt&maskInt;
            long ipNum=(long)(~maskInt);

            for (int i = 0; i < ipNum; i++) {
            
            }

        }
        public static bool IpIs_C_Type(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            long first = long.Parse(items[0]);
            if (first == 192)
            {
                return true;
            }
            else {
                return false;
            }
                    
        }
        public static String GetLocalMacAddress() 
        {
            string ip = "";
            string mac = "";
            try{
              
                            ManagementClass mc;
                            string hostInfo = Dns.GetHostName();
                            //IP地址
                            //System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;这个过时
                            System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                            for (int i = 0; i < addressList.Length; i++)
                            {
                                if (addressList[i].AddressFamily == AddressFamily.InterNetwork) {
                                    ip = addressList[i].ToString();
                                }
                             
                            }
                            //mac地址
                            mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                            ManagementObjectCollection moc = mc.GetInstances();
                            foreach (ManagementObject mo in moc)
                            {
                                if (mo["IPEnabled"].ToString() == "True")
                                {
                                    mac += mo["MacAddress"].ToString();
                                }
                            }         
              
            }catch (Exception e){
            
            }
            return mac;
        }
        public static String GetWifiMacAddress()
        {
            string ip = "";
            string mac = "suck";
           
            return mac;
        }

        public static void DeleteFile(String file)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(file)) {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }               

            }catch (Exception ex){

                ex = null;
            }


        }


        public static String ReadTxt(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            StringBuilder sb = new StringBuilder();
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line.ToString());
                sb.AppendLine(line);
            }
            return sb.ToString();
        }




    }






}
