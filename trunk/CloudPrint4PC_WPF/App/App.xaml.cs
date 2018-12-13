using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Reflection;
using BaseLib;
using CloudPrintforPC;
using CloudPrintLib;


namespace CloudPrint4PC_WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static bool is64BitProcess = (IntPtr.Size == 8);
        bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
        [In] IntPtr hProcess,
        [Out] out bool wow64Process
        );

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
            Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="isAutoRun"></param>
       
        public void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                    throw new Exception("该文件不存在!");
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
                //lbl_autorunerr.Visible = false;
            }
            catch
            {
                //lbl_autorunerr.Visible = true;
                //throw new Exception(ex.ToString());  
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }
        /// <summary>
        /// 单例模式
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Get Reference to the current Process
            Process thisProc = Process.GetCurrentProcess();
            if (is64BitProcess)
            {
                MessageBox.Show("不支持64位");
                Application.Current.Shutdown();
                return;
            }
            // Check how many total processes have the same name as the current one
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                // If ther is more than one, than it is already running.
                MessageBox.Show("Application is already running.");
                Application.Current.Shutdown();
                return;
            }
            //设置开机自动启动
            try{
                this.SetAutoRun(Assembly.GetExecutingAssembly().Location,true);
            }catch (Exception ex) {
                LogHelper.WriteLog(this.GetType(),ex);
                MessageBox.Show("【失败】开机自启动设置");
            }
            //检查PCL 打印机---PDF 文件必须支持
            if (!PrintLocalPrint.HaveOnePclPrinter())
            {
                MessageBox.Show("【PCL】没有PCL打印机");
            }
            //推荐使用的服务器Ip地址
            String ip_result = PhonePcCommunication.IsRecommendIpAddress();
            if (!"true".Equals(ip_result)) {
                MessageBox.Show("【IP推荐】"+ip_result);
            }
            //通知本地服务器
            PcCloudComm.Notify_Thread();
            //保存编译时间到本地

            UpdatePackage.Check2DownloadThread();

            base.OnStartup(e);
        }
    }
}
