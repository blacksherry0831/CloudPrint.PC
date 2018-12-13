using CloudPrintforPC;
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
using System.Configuration;
using CloudPrint4PC_WPF.WindowCloudPrint;
using BaseLib;

namespace CloudPrint4PC_WPF
{
    /// <summary>
    /// CircleWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CircleWindow : Window
    {
        public ServerCloudPrint _Server;
        Point _StartPoint;
        public CircleWindow(Point pt)
        {
         
            InitializeComponent(); 
            this._StartPoint = pt;
            this.InitStartPoint();
          //  this._CanvasMain.Opacity = 0.8;
        }
        public CircleWindow(Point pt,ServerCloudPrint Server)
        {

            InitializeComponent();
            this._StartPoint = pt;
            this.InitStartPoint();
            this._Server = Server;
            //  this._CanvasMain.Opacity = 0.8;
        }
        private void InitStartPoint() 
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = this._StartPoint.X-this.Width/2;
            this.Top = this._StartPoint.Y-this.Height/2;
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        //private void Image_MouseEnter_1(object sender, MouseEventArgs e)
        //{

        //}

        private void Image_MouseEnter_ChangeSize(object sender, MouseEventArgs e)
        {
            var ob = sender as Image;
            Point pt;
            if (ob != null) {
                
                pt=ob.TranslatePoint(new Point(0, 0), this._CanvasMain);
                Point CenterPoint = ob.TranslatePoint(new Point(ob.Width/2,ob.Height/2), this._CanvasMain);

                
                ob.Width = ob.Width * 2;
                ob.Height = ob.Height * 2;
            }
        }

        //private void Image_MouseLeave_1(object sender, MouseEventArgs e)
        //{

        //}

        //private void Image_MouseLeave_2(object sender, MouseEventArgs e)
        //{

        //}

        private void Image_MouseLeave_ChangeSize(object sender, MouseEventArgs e)
        {
            var ob = sender as Image;
            
            Point pt;
            if (ob != null)
            {
                ob.Width = ob.Width / 2;
                ob.Height = ob.Height / 2;
            }
        }

      

        private void OpenWebNet(object sender, MouseButtonEventArgs e)
        {
            //调用系统默认的浏览器 
           
          
            try
            {
                String url_str = ConfigurationManager.AppSettings["URL_CLOUD_PRINT_SHOP"];
                System.Diagnostics.Process.Start(url_str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.WriteLog(this.GetType(), ex);
            }
        }

        private void OpenPhoneManager(object sender, MouseButtonEventArgs e)
        {
            WindowPhoneManager WPM = new WindowPhoneManager(this._Server);
            WPM.Show();
        }

        private void OpenStatistical(object sender, MouseButtonEventArgs e)
        {
            WindowStatistics WS = new WindowStatistics(this._Server);
            WS.Show();
        }

        private void OpenFileTransfer(object sender, MouseButtonEventArgs e)
        {
            WindowFileTransfer WFT = new WindowFileTransfer(this._Server);
            WFT.Show();
        }

        private void OpenNetPrintList(object sender, MouseButtonEventArgs e)
        {
            WindowNetPrint WNP = new WindowNetPrint(this._Server);
            WNP.Show();
        }
    }
}
