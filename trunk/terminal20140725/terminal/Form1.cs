using Classlib;
using Client.Classlib;
using Client.ClassList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrintSetUI;
using System.Xml;

namespace terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.notifyIcon1.Visible = true;
            this.timer1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string count_config = "";
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (count_config == "")
            {
                count_config = "0";
            }
            XmlDocument Myconfig = new XmlDocument();
            Myconfig.Load("login_config.xml");
            XmlNode Nodec = Myconfig.DocumentElement;
            string count = (Convert.ToInt32(count_config) + 1).ToString();
            XmlElement LoginNamec = Myconfig.CreateElement("loginer");
            XmlElement xmlcount = Myconfig.CreateElement("loginCount");
            xmlcount.InnerText = count;
            XmlElement xmlurl = Myconfig.CreateElement("url");
            xmlurl.InnerText = url;
            XmlElement xmlisLogin = Myconfig.CreateElement("Islogin");
            xmlisLogin.InnerText = "false";
            XmlElement xmlauto = Myconfig.CreateElement("auto");
            xmlauto.InnerText = "true";
            XmlNode nodec = Myconfig.SelectSingleNode("//loginer[loginCount = '" + count+ "']");
            if (nodec == null)           //若loginName不存在
            {
                LoginNamec.PrependChild(xmlcount);  //写入子节点loginName和loginPWD

                Nodec.PrependChild(LoginNamec);
            }
            else
            {
                //若loginName已存在，删除该子节点，并重新写入
                XmlNodeList nodesc;
                XmlElement root = Myconfig.DocumentElement;
                nodesc = root.SelectNodes("//loginer[loginCount = '" + count + "']");
                foreach (XmlNode node1 in nodesc)
                {
                    root.RemoveChild(node1);
                }
                LoginNamec.PrependChild(xmlauto);
                LoginNamec.PrependChild(xmlurl);
                LoginNamec.PrependChild(xmlisLogin);
                LoginNamec.PrependChild(xmlcount);
                Nodec.PrependChild(LoginNamec);
            }
            Myconfig.Save("login_config.xml");

            this.Close();
            
        }
        public string Password;
        public string Username;
        public string url;
        private void timer1_Tick(object sender, EventArgs e)
        {
            string murl = url;
            murl += "/machine/privateCloudHandler.ashx";
            string license = Password;
            CloudCommunication cloudcomm = new CloudCommunication(
                  @murl,
                   Password,
                   Username,
                   license);
            cloudcomm.url = url;
            cloudcomm.shijian+=new CloudCommunication.WeiTuo(CloseTimer);
            cloudcomm.GetPrintTaskInfo();    
        }
        public void CloseTimer()
        {
            this.timer1.Enabled = false;
        }

        private void 注册打印机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registorPrinter r = new registorPrinter();
            r.username = Username;
            r.pwd = Password;
            r.url = url;
            r.ShowDialog();
            r.Dispose();
            r.Close();
        }
    }
}
