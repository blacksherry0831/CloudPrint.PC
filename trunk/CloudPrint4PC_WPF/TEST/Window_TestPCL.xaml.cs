using CloudPrintforPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CloudPrint4PC_WPF
{
    /// <summary>
    /// Window_TestPCL.xaml 的交互逻辑
    /// </summary>
    public partial class Window_TestPCL : Window
    {
        public Window_TestPCL()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            FileSendOnNet fileRcv = new FileSendOnNet();
            fileRcv.FileFullPath = @"E:/cc.pdf";
            fileRcv.SetPrinterName("Lenovo M7600D Printer PCL");
            //Lenovo M7600D Printer PCL
            PrintDocThread pdt = new PrintDocThread(fileRcv);
            pdt.StartPrint();
        }
    }
}
