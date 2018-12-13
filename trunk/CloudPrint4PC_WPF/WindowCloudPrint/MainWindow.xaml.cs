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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CloudPrintforPC;
using System.Windows.Threading;
/**
 * WPF 开启
 * 
 * 
 **/
namespace CloudPrint4PC_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Microsoft.Win32.RegistryKey regKey = null;
        private DispatcherTimer dTimer = new DispatcherTimer();
        public ServerCloudPrint mServer = new ServerCloudPrint();
        BitmapImage ImageEnter = new BitmapImage(new Uri(@"/CloudPrint4PC_WPF;component/Img/button_neg.png", UriKind.Relative));
        BitmapImage ImageLeave = new BitmapImage(new Uri(@"/CloudPrint4PC_WPF;component/Img/button.png", UriKind.Relative));
        public MainWindow()
        {
            InitializeComponent();
            this.initTimer();
            {
                regKey = Microsoft.Win32.Registry.LocalMachine;//读取HKEY_LOCAL_MACHINE项
                Microsoft.Win32.RegistryKey regSubKey = null;
                regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Office\14.0", false);
                if (regSubKey == null)
                {
                    MessageBox.Show("请安装好office 2010，再打开此软件 ");
                    this.Close();
                }
                else {

                }
            }
          


        }
        private void initTimer() 
        {
            this.dTimer.Tick += new EventHandler(this.Updata2TextBlock);
       
            this.dTimer.Interval = new TimeSpan(0, 0, 1);
            this.dTimer.Start();
        }
        private void Updata2TextBlock(object sender, EventArgs e) 
        {
            var Timer = sender as DispatcherTimer;
            

            if (!this._PhoneNum.Text.Equals(this.mServer.NetServer.mPcPhoneList.PhoneCount)) {
                  this._PhoneNum.Text=this.mServer.NetServer.mPcPhoneList.PhoneCount;

            }
            if(!this._PrintNum.Text.Equals(this.mServer.PrintSvr.PrintCount)){
                  this._PrintNum.Text = this.mServer.PrintSvr.PrintCount;
            }

        }
        private void StartCircleWindow(Point CenterPoint)
        {
            double OldValue = this.Opacity;
            this.Opacity = 0.8;
            this.ShowInTaskbar = false;
            
            {
                CircleWindow cw = new CircleWindow(CenterPoint,this.mServer);
                cw.ShowDialog();
            }
            this.ShowInTaskbar = true;
            this.Opacity = OldValue;
        }
        private void DragMove(object sender, MouseButtonEventArgs e) 
        {
            base.DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.mServer.StopServer();
        }

        private void MouseEnterImage(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            if (img != null && ImageEnter!=null)
            {
                img.BeginInit();
                img.Source = ImageEnter;
                img.EndInit();
            }
        }

        private void MouseLeaveImage(object sender, MouseEventArgs e)
        {
            var img= sender as Image;
            if (img != null&&ImageLeave!=null) {
                img.BeginInit();
                img.Source = ImageLeave;
                img.EndInit();
            }
        }

        private void StartCircleWindow(object sender, MouseButtonEventArgs e)
        {
            Point pt;
            var startbutton = sender as Image;
            if (startbutton != null) {

                startbutton.IsEnabled = false;
                pt = startbutton.TranslatePoint(new Point(0, 0),this);

                pt=this.PointToScreen(pt);

                double W = startbutton.Width;
                double H = startbutton.Height;
                this.StartCircleWindow(new Point(pt.X+W/2,pt.Y+H/2));
                startbutton.IsEnabled = true;
            
            }

        }
/**
*
*/
        private void MainImageButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("............");
        }
      
    }
}
