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
using System.Windows.Threading;

namespace CloudPrint4PC_WPF.WindowCloudPrint
{
    /// <summary>
    /// WindowStatistics.xaml 的交互逻辑
    /// </summary>
    public partial class WindowStatistics : Window
    {
        public ServerCloudPrint mServer;
        private DispatcherTimer dTimer = new DispatcherTimer();
        public ObservableCollection<PrintRecordItem>  Records { get; set; }
        //public WindowStatistics()
        //{
        //    InitializeComponent();
        //}
        public WindowStatistics(ServerCloudPrint Server)
        {
            InitializeComponent();
            this.mServer = Server;
            this.Records = new ObservableCollection<PrintRecordItem>();
            this.initTimer();
            this.ListViewRecords.DataContext = this;
        }
        /**
        *
        *
        */
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /**
        *
        *
        */
        private void SetData()
        {

            List<PrintRecordItem> pRi_new = PrintRecord.ReadAllRecord();
            if (Records.Count == pRi_new.Count)
            {

            }
            else
            {
                    this.Records.Clear();

                    foreach (PrintRecordItem pri in pRi_new) {
                             this.Records.Add(pri);

                    }             
             
            }
        }
        /**
         *
         *
         */
        private void initTimer()
        {
            this.dTimer.Tick += new EventHandler(this.UpdataData);

            this.dTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            this.dTimer.Start();
        }
        /**
        *
        *
        */
        private void UpdataData(object sender, EventArgs e)
        {
            this.SetData();
        }
        /**
        *
        *
        */

    }
}
