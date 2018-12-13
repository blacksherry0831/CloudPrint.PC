using Microsoft.Win32;
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

namespace CloudPrint4PC_WPF
{
    /// <summary>
    /// UserControlPrinterItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlPrinterItem : UserControl
    {
        public UserControlPrinterItem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Multiselect = true;
            o.Filter = "*.*|*.*";
            if (o.ShowDialog() == true)
            {
                //o.FileNames选择的结果，多个文件路径及文件名
                //o.FileName选择的结果，单个文件路径及文件名
                foreach (String file in o.FileNames)
                {
                    //this.mPrintInfo.PrintFiles(file);
                }

            }
        }
    }
}
