using Classlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Readconfig
{
   public class readconfig
    {
       public List<datastring> connstr()
        {
            string sPath = Application.StartupPath;
            string connstr=null;
        try
            {
                using (StreamReader sr = new StreamReader(sPath + @"\配置文件.txt"))
                {
                    while (sr.Peek() >= 0)
                    {
                        connstr=sr.ReadLine();
                    }
                    List<datastring> list = new List<datastring>();
                    datastring model = new datastring();
                    model.Conn = connstr;
                    list.Add(model);
                    return list;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;

            }
        }
       
    }
}
