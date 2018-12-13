using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CloudPrintforPC;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Windows.Threading;
/**
*
*/
namespace CloudPrint4PC_WPF.WindowCloudPrint
{
    /// <summary>
    /// WindowNetPrint.xaml 的交互逻辑
    /// </summary>
    public partial class WindowNetPrint : Window
    {
        public ServerCloudPrint mServer;
        private DispatcherTimer dTimer = new DispatcherTimer();
        public ObservableCollection<PrintOneInfo> Printers

        {
            get;
            set;
        }
        //public WindowNetPrint()
        //{
        //    InitializeComponent();
        //}
        public WindowNetPrint(ServerCloudPrint Server)
        {
            InitializeComponent();
            this.mServer = Server;
            this.Printers = new ObservableCollection<PrintOneInfo>();
            this.DataContext = this;
            this.Init();
            this.initTimer();
        }

        public void Init()
        {

            PrintServer ps = this.mServer.PrintSvr;
            for (int i = 0; i < ps.ListPrint.Count; i++)
            {

                PrintOneInfo poi = (PrintOneInfo)(ps.ListPrint[i]);
                //this.mNetPrintList.Add(poi);
                this.Printers.Add(poi);

            }
        }

        private void initTimer()
        {
            this.dTimer.Tick += new EventHandler(this.UpdataData);

            this.dTimer.Interval = new TimeSpan(0, 0, 0,0,200);
            this.dTimer.Start();
        }
        private void UpdataData(object sender, EventArgs e)
        {
            var Timer = sender as DispatcherTimer;

                    lock (Printers) {
                           foreach (PrintOneInfo poi in Printers)
                                    {
                                        if (poi.WPF_PROGRESS >= 100) {
                                                poi.WPF_PROGRESS = 0;
                                        }else{
                                                poi.WPF_PROGRESS += 2;
                                        }
                                   
                                    }
                            }
            }
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog o = new OpenFileDialog();
        //    o.Multiselect = true;
        //    o.Filter = "*.*|*.*";
        //    if (o.ShowDialog() == true)
        //    {
        //        //o.FileNames选择的结果，多个文件路径及文件名
        //        //o.FileName选择的结果，单个文件路径及文件名
        //        foreach (String file in o.FileNames)
        //        {
        //            //this.mPrintInfo.PrintFiles(file);
        //        }

        //    }
        //}
    }
}
