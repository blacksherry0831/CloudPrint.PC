using Classlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using terminal;
using Readconfig;
using System.IO;
using System.Xml;

namespace PrintSetUI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.SizeChanged+=new EventHandler(Login_SizeChanged);
        }
        public string count_config = "";
        public string isLogin ="";
        public string auto = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text!= ""&&textBoxPWD.Text!="")
            {
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
                xmlisLogin.InnerText = "true";
                XmlElement xmlauto = Myconfig.CreateElement("auto");
                if (checkBox2.Checked == true)
                {
                    xmlauto.InnerText = "true";
                }
                else
                {
                    xmlauto.InnerText = "false";
                }
                XmlNode nodec = Myconfig.SelectSingleNode("//loginer[loginCount = '" + count_config + "']");
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
                    nodesc = root.SelectNodes("//loginer[loginCount = '" + count_config + "']");
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


                    if (checkBox1.Checked == true)
                    {
                        XmlDocument MyXML = new XmlDocument();
                        MyXML.Load("login.xml");
                        XmlNode Node = MyXML.DocumentElement;
                        string loginNames = comboBox1.Text;
                        string loginPWDs = textBoxPWD.Text;
                        XmlElement LoginName = MyXML.CreateElement("loginer");
                        XmlElement xmlName = MyXML.CreateElement("loginName");
                        xmlName.InnerText = loginNames;
                        XmlElement xmlPWD = MyXML.CreateElement("loginPWD");
                        xmlPWD.InnerText = loginPWDs;
                        XmlNode node = MyXML.SelectSingleNode("//loginer[loginName = '" + loginNames + "']");
                        if (node == null)           //若loginName不存在
                        {
                            LoginName.PrependChild(xmlPWD);  //写入子节点loginName和loginPWD
                            LoginName.PrependChild(xmlName);
                            Node.PrependChild(LoginName);
                        }
                        else
                        {
                            //若loginName已存在，删除该子节点，并重新写入
                            XmlNodeList nodes;
                            XmlElement root = MyXML.DocumentElement;
                            nodes = root.SelectNodes("//loginer[loginName = '" + loginNames + "']");
                            foreach (XmlNode node1 in nodes)
                            {
                                root.RemoveChild(node1);
                            }
                            LoginName.PrependChild(xmlPWD);
                            LoginName.PrependChild(xmlName);
                            Node.PrependChild(LoginName);
                        }
                        MyXML.Save("login.xml");
                    }
                    this.Hide();
                    Form1 f = new Form1();
                    f.Password = this.textBoxPWD.Text;
                    f.Username = this.comboBox1.Text;
                    f.url = url;
                    f.count_config = count_config;
                    f.ShowDialog();
                    this.Close();
            }
            else
            {
                MessageBox.Show("请输入用户名及密码!","登录提示");
            }
            
        }

        private void Login_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Minimized;
        }
        public string url;
        public void writeXmlConfig()
        {
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
            xmlisLogin.InnerText = "true";
            XmlElement xmlauto = Myconfig.CreateElement("auto");
                xmlauto.InnerText = "true";
            XmlNode nodec = Myconfig.SelectSingleNode("//loginer[loginCount = '" + count_config + "']");
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
                nodesc = root.SelectNodes("//loginer[loginCount = '" + count_config + "']");
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
        }
        private void Login_Load(object sender, EventArgs e)
        {

            url =readXML_loginconfig();

            readXML_loginuser();//读取用户名，密码
            if (auto == "true")//自动登录
            {
                if (comboBox1.Text != "" && textBoxPWD.Text != "")
                {
                    this.Hide();
                    writeXmlConfig();
                    Form1 f = new Form1();
                    f.Password = this.textBoxPWD.Text;
                    f.Username = this.comboBox1.Text;
                    f.url = url;
                    f.count_config = count_config;
                    f.ShowDialog();
                    this.Close();
                }
            }
            
            
            DirectoryInfo dirInfo2 = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + @"\下载文档\");
            FileInfo[] files2 = dirInfo2.GetFiles();   // 获取该目录下的所有文件
            foreach (FileInfo file in files2)
            {
                file.Delete();
            }
        }
        private string readXML_loginconfig()
        {
            string url = "";
            string count = "";
            XmlDocument MyXML = new XmlDocument();
            MyXML.Load("login_config.xml");
            XmlElement root = MyXML.DocumentElement;
            XmlNode xn = MyXML.LastChild.LastChild;
            //读取xml,加载combobox的items.
            foreach (XmlNode list in MyXML.GetElementsByTagName("login_config"))
            {
                foreach (XmlNode info2 in list.ChildNodes)
                foreach (XmlNode info in info2.ChildNodes)
                {
                    if (info.Name == "loginCount")
                    {
                        if (info.InnerText == "0")
                        {
                            count = "0";
                            
                        }
                        count_config = info.InnerText;
                    }
                    if (info.Name == "Islogin")
                    {   
                        isLogin = info.InnerText;
                        if (info.InnerText == "true")//有登陆
                        {
                            System.Diagnostics.Process.Start("http://210.28.186.83/main3.aspx");
                            this.Close();//关闭窗体
                        }
                    }
                    
                    if (info.Name == "url")
                    {
                        url = info.InnerText;
                        
                    }
                    if (info.Name == "auto")
                    {
                        if (info.InnerText == "true")
                        {
                            auto = "true";
                        }
                    }
                }
            }
            if (File.Exists("login_config.xml"))
            {
                StreamReader sr = new StreamReader("login_config.xml", true);
                string str = sr.ReadLine();
                while (str != null)
                {
                    if (!this.comboBox1.AutoCompleteCustomSource.Contains(str))//是否包含集合里
                    {
                        this.comboBox1.AutoCompleteCustomSource.Add(str);//不包含添加
                    }
                    str = sr.ReadLine();
                }
                sr.Close();
            }

            return url;
        }
        private void readXML_loginuser()
        {
            XmlDocument MyXML = new XmlDocument();
            MyXML.Load("login.xml");
            XmlElement root = MyXML.DocumentElement;
            XmlNode xn = MyXML.LastChild.LastChild;
            if (root.ChildNodes.Count > 20)
            {
                root.RemoveChild(xn);
            }
            MyXML.Save("login.xml");
            //读取xml,加载combobox的items.
            foreach (XmlNode list in MyXML.GetElementsByTagName("loginer"))
            {
                foreach (XmlNode info in list.ChildNodes)
                {
                    if (info.Name == "loginName")
                    {
                        comboBox1.Items.Add(info.InnerText);
                    }
                }
            }
            if (File.Exists("login.xml"))
            {
                StreamReader sr = new StreamReader("login.xml", true);
                string str = sr.ReadLine();
                while (str != null)
                {
                    if (!this.comboBox1.AutoCompleteCustomSource.Contains(str))//是否包含集合里
                    {
                        this.comboBox1.AutoCompleteCustomSource.Add(str);//不包含添加
                    }
                    str = sr.ReadLine();
                }
                sr.Close();
            }
            if (this.comboBox1.Items.Count != 0)
            {
                this.comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument MyXML = new XmlDocument();
            MyXML.Load("login.xml");
            XmlNode Node = MyXML.DocumentElement;
            string loginNames = comboBox1.Text;
            XmlNodeList nodes;
            XmlElement root = MyXML.DocumentElement;
            nodes = root.SelectNodes("//loginer[loginName='" + loginNames + "']");
            string strn = null;
            string strpwd = null;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                XmlNodeList nodech = xe.ChildNodes;
                foreach (XmlNode xnch in nodech)
                {
                    XmlElement xech = (XmlElement)xnch;
                    if (xech.LocalName == "loginName")
                    {
                        strn = xech.InnerText;
                        if (strn == loginNames)
                        {
                            XmlNode xnpwd = xech.NextSibling;
                            strpwd = xnpwd.InnerText;
                        }
                    }
                }
            }
            this.textBoxPWD.Text = strpwd;
        }
    }
}
