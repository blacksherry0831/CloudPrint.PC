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
    /// UserControlPhoneItem.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlPhoneItem : UserControl
    {
        public UserControlPhoneItem()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("权限设置成功");
        }
    }
}
