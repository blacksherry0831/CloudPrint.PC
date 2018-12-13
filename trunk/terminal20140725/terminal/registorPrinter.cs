using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using terminal.lib;

namespace terminal
{
    public partial class registorPrinter : Form
    {
        public registorPrinter()
        {
            InitializeComponent();
        }

        private void registorPrinter_Load(object sender, EventArgs e)
        {
            string printers = PCLConvertCS.PCLToPrinter.GetAllPrinter();
            if (string.IsNullOrEmpty(printers))
            {
                return;
            }
            string[] all = printers.Split(';');
            for (int i = 0; i < all.Length; i++)
            {
                this.comboBox1.Items.Add(all[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string username;
        public string pwd;
        public string url;
        private void button2_Click(object sender, EventArgs e)
        {
            string panDuan="";
            if (this.textBox1.Text.Trim() != ""&&this.comboBox1.Text.Trim()!="")
            {
               panDuan=ReportCloud(username, pwd, this.comboBox1.Text, this.textBox1.Text, url);
               if (panDuan == "ok!") MessageBox.Show("注册成功！");
               else MessageBox.Show("注册失败！");
            }
            else
            {
                MessageBox.Show("error!");
            }
        }
        public string ReportCloud(string userid,string pwd,string Printer,string Place, string url1)
        {
            string zxc = GetJsonString(userid,pwd,Printer,Place);
            string Text = "";
            url1 += "/machine/privateRegPrinterHandler.ashx";
            string url = @url1;
            List<string> list = new List<string>();
            list.Add("reports");
            list.Add(zxc);
            printActiveX.ReportData.ReportOrder report = new printActiveX.ReportData.ReportOrder(@url);
           Text=report.sendMsgToServer(list, "post");   // report
           return Text;
        }
        public string GetJsonString(string userid,string pwd,string Printer,string Place)
        { 
            List<RagPrinter> products = new List<RagPrinter>(){  
            new RagPrinter(){Username=userid,Pwd=pwd,PrinterName=Printer,BeizhuName=Place},  
            };  
            ProductList productlist = new ProductList();  
            productlist.RegPrinter = products;  
           // return new JavaScriptSerializer().Serialize(productlist);  
            return new JavaScriptSerializer().Serialize(products); 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    public class ProductList
    {
        public List<RagPrinter> RegPrinter { get; set; }
    } 
}
