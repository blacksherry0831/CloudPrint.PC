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
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace CloudPrint4PC_WPF.WindowCloudPrint
{
    /// <summary>
    /// WindowPhoneManager.xaml 的交互逻辑
    /// </summary>
    public partial class WindowPhoneManager : Window
    {
        public ServerCloudPrint mServer;
        private DispatcherTimer dTimer = new DispatcherTimer();
        public String mCurrentListID = new Guid().ToString();
        public ObservableCollection <ServerInfo> Phone

        {
            get;
            set;
        }
        //public WindowPhoneManager()
        //{
        //    InitializeComponent();
        //}
        public WindowPhoneManager(ServerCloudPrint Server)
        {
            InitializeComponent();
            this.mServer = Server;
            this.Phone = new ObservableCollection<ServerInfo>();
            this.initTimer();
            this.DataContext = this;

        }

        private void initTimer()
        {
            this.dTimer.Tick += new EventHandler(this.UpdataData);

            this.dTimer.Interval = new TimeSpan(0, 0, 0, 1);
            this.dTimer.Start();
        }
        private void UpdataData(object sender, EventArgs e)
        {
            var Timer = sender as DispatcherTimer;
            this.SetPhoneData();
        }
        /**
        *
        */
        private void SetPhoneData()
        {
            try {
                     if (!mCurrentListID.Equals( this.mServer.NetServer.mPcPhoneList.PhonePcListID))
                                {
                                    this.Phone.Clear();

                                    ServerInfo[] array = this.mServer.NetServer.mPcPhoneList.GetPhonelistArray();
                                    foreach (ServerInfo si in array) {
                                        this.Phone.Add(si);
                                    }
             
                                    mCurrentListID = this.mServer.NetServer.mPcPhoneList.PhonePcListID;

                                }
            } catch (Exception e){

            }
            
           
        }
    }
}
