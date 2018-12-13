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
using BaseLib;

namespace CloudPrintforPC
{
   public class LibCui
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public static bool DeleteFile(String file)
        {
            bool status = false;
            try
            {
                if (!String.IsNullOrWhiteSpace(file))
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        status = true;
                        LogHelper.WriteLog(typeof(LibCui), "删除文件成功");
                    }
                    else
                    {
                        LogHelper.WriteLog(typeof(LibCui), "文件不存在");
                    }
                }
                else
                {
                    LogHelper.WriteLog(typeof(LibCui), "文件名为空");
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(LibCui), "删除文件异常");
                LogHelper.WriteLog(typeof(LibCui), ex);
            }

            return status;
            


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        public static bool  DeleteFileForce(String file)
        {
            bool status = false;
            int times = 0;
            try
            {
                if (!String.IsNullOrWhiteSpace(file))
                {
                    while (File.Exists(file)&&times++<50) {
                        
                        try {
                                Thread.Sleep(200);
                                File.Delete(file); 
                        } catch (Exception ex) {
                            LogHelper.WriteLog(typeof(LibCui), ex);
                        }
                         
                    }
                    if (File.Exists(file)==false)   status = true;
                }
              
            }
            catch (Exception ex){               
                LogHelper.WriteLog(typeof(LibCui), ex);
            }
            return status;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

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



        ///<summary>
        /// 检测是否安装office
        ///</summary>
        ///<param name="office_Version"> 获得并返回安装的office版本</param>
        ///<returns></returns>
        public static bool IsInstallOffice(out string office_Version, out string office_Path)
        {
            bool result = false;
            string str_OfficePath = string.Empty;
            string str_OfficeVersion = string.Empty;
            office_Version = string.Empty;
            office_Path = string.Empty;

            GetOfficePath(out str_OfficePath, out str_OfficeVersion);
            if (!string.IsNullOrEmpty(str_OfficePath) && !string.IsNullOrEmpty(str_OfficeVersion))
            {
                result = true;
                office_Version = str_OfficeVersion;
                office_Path = str_OfficePath;
            }
            return result;
        }
        ///<summary>
        /// 获取并返回当前安装的office版本和安装路径
        ///</summary>
        ///<param name="str_OfficePath">office的安装路径</param>
        ///<param name="str_OfficeVersion">office的安装版本</param>
        private static void GetOfficePath(out string str_OfficePath, out string str_OfficeVersion)
        {
            string str_PatheResult = string.Empty;
            string str_VersionResult = string.Empty;
            string str_KeyName = "Path";
            object objResult = null;
            Microsoft.Win32.RegistryValueKind regValueKind;//指定在注册表中存储值时所用的数据类型，或标识注册表中某个值的数据类型。
            Microsoft.Win32.RegistryKey regKey = null;//表示 Windows 注册表中的项级节点(注册表对象?)
            Microsoft.Win32.RegistryKey regSubKey = null;
            try
            {
                regKey = Microsoft.Win32.Registry.LocalMachine;//读取HKEY_LOCAL_MACHINE项
                if (regSubKey == null)
                {//office97
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\8.0\Common\InstallRoot", false);//如果bool值为true则对打开的项进行读写操作,否则为只读打开
                    str_VersionResult = "Office97";
                    str_KeyName = "OfficeBin";
                }
                if (regSubKey == null)
                {//Office2000
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\9.0\Common\InstallRoot", false);
                    str_VersionResult = "Pffice2000";
                    str_KeyName = "Path";
                }
                if (regSubKey == null)
                {//officeXp
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\10.0\Common\InstallRoot", false);
                    str_VersionResult = "OfficeXP";
                    str_KeyName = "Path";
                }

                if (regSubKey == null)
                {//Office2003
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\11.0\Common\InstallRoot", false);
                    str_VersionResult = "Office2003";
                    str_KeyName = "Path";
                    try
                    {
                        objResult = regSubKey.GetValue(str_KeyName);
                        regValueKind = regSubKey.GetValueKind(str_KeyName);
                    }
                    catch (Exception ex)
                    {
                        regSubKey = null;
                    }
                }

                if (regSubKey == null)
                {//office2007
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Common\InstallRoot", false);
                    str_VersionResult = "Office2007";
                    str_KeyName = "Path";
                }
                if (regSubKey == null)
                {//office2007
                    regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\14.0\Common\InstallRoot", false);
                    str_VersionResult = "Office2010";
                    str_KeyName = "Path";
                }
                objResult = regSubKey.GetValue(str_KeyName);
                regValueKind = regSubKey.GetValueKind(str_KeyName);
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    str_PatheResult = objResult.ToString();
                }
            }
            catch (Exception ex)
            {
               // LogHelper.WriteLogError(ex.ToString());
                //throw ex;
            }
            finally
            {
                if (regKey != null)
                {
                    regKey.Close();
                    regKey = null;
                }

                if (regSubKey != null)
                {
                    regSubKey.Close();
                    regSubKey = null;
                }
            }
            str_OfficePath = str_PatheResult;
            str_OfficeVersion = str_VersionResult;
        }


        /***
        *
        */

        public static bool IsWin8()
        {
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");
            if (currentVersion.CompareTo(compareToVersion) >= 0)
            {//win8及其以上版本的系统
               // Console.WriteLine("当前系统是WIN8及以上版本系统。");
                return true;
            }
            else
            {
              
               // Console.WriteLine("当前系统不是WIN8及以上版本系统。");
                return false;
            }
        }

        /***
      *
      */
        public static bool IsStringNeg(String num)
        {
            try {
               
                double price_f = double.Parse(num);
                if (price_f < 0)
                {
                    return true;
                }
               
            } catch (Exception e){

            }

            return false;

        }
/**
*
*/


        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        void GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            //txtLocalIP.Text = AddressIP;
        }
/**
*
*/

    }






}
