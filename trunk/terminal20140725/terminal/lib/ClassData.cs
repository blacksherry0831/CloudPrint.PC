using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readconfig;
using System.Data.SqlClient;
using Client.ClassList;


namespace Classlib
{
    public class datastring
    {
        public string conn;

        public string Conn
        {
            get { return conn; }
            set { conn = value; }
        }



    }
    class ClassData
    {
        //读取配置文件中的连接字符串
        public string readconfig()
        {
            readconfig rd = new readconfig(); 
            List<datastring> datastring=rd.connstr();
            datastring a=datastring[0];
            return a.Conn;
        }
        //验证用户名，密码
        public List<Classdenglu1> yanzheng(string textbox1, string textbox2)
        {
            string yanzheng=null;
            string constr = this.readconfig();      
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string strCommand = "SELECT [username],[pwd] FROM [RoleInfo] where [username] ='" +textbox1  + "'and [pwd] ='" + textbox2 + "'";
                SqlCommand cmd = new SqlCommand(strCommand, conn);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yanzheng = reader["username"].ToString();
                }
                List<Classdenglu1> list = new List<Classdenglu1>();
                Classdenglu1 a = new Classdenglu1();
                a.Yanzheng=yanzheng;
                list.Add(a);
                return list;
            }
        }
        //验证用户名
        public List<Classdenglu1> yanzhenguser(string textbox1)
        {
            string yanzheng = null;
            string constr = this.readconfig();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string strCommand = "SELECT [username],[pwd],[role] FROM [RoleInfo] where [username] ='" + textbox1 + "'";
                SqlCommand cmd = new SqlCommand(strCommand, conn);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yanzheng = reader["username"].ToString();
                }
                List<Classdenglu1> list = new List<Classdenglu1>();
                Classdenglu1 a = new Classdenglu1();
                a.Yanzheng = yanzheng;
                list.Add(a);
                return list;
            }

        }
        public List<listcloudlicense> Readforcommunicationcloud()
        {
            string constr = this.readconfig();
            List<listcloudlicense> list = new List<listcloudlicense>();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                string strCommand = "SELECT * FROM jiekou";
                SqlCommand cmd = new SqlCommand(strCommand, conn);
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    listcloudlicense a = new listcloudlicense();
                    a.Url = reader["url"].ToString();
                    a.Username = reader["username"].ToString();
                    a.Password = reader["password"].ToString();
                    a.License = reader["license"].ToString();
                    list.Add(a);
                }
                return list;
            }
        }
        public void insertOrderlist(string orderID, string location, string locationID, string totalFee, string status, string preFinishTime, string allocateTime, string deliverInfo, string finishTime, int tasknum)
        {
            string constr = this.readconfig();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into [printOrder] ([orderID],[location],[locationID],[totalfee],[status],[preFinishTime],[allocateTime],[deliverInfo],[finishTime],[resivetime],[tasknum],[tasknumfact],[report]) values('" + orderID + "','" + location + "','" + locationID + "','" + totalFee + "','" + status + "','" + preFinishTime + "','" + allocateTime + "','" + deliverInfo + "','" + finishTime + "','" + DateTime.Now.ToString() + "','" + tasknum + "','0','0')";
                cmd.ExecuteNonQuery();
            }
        }
        public void insertfilelist(string fileID, string orderID, string fileName, string fileType, string prtCopies, string isOdd, string Prsize, string coloPages, string grayPages, bool Isdouble, bool color, string PageStyle, string zhizuo, string zhuangding)
        {
            string constr = this.readconfig();
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into [printTaskInfo]([fileID],[orderID],[fileName],[fileType],[prtCopies],[isOdd],[PrSize],[coloPages],[grayPages],[isDouble],[color],[pageStyle],[zhizuo],[zhuangding],[isPrinted],[yewuleixing],[isattach],[isPay],[prtPage],[Workload],[duizhang]) values('" + fileID + "','" + orderID + "','" + fileName + "','" + fileType + "','" + prtCopies + "','" + isOdd + "','" + Prsize + "','" + coloPages + "','" + grayPages + "','" + Isdouble + "','" + color + "','" + PageStyle + "','" + zhizuo + "','" + zhuangding + "','N','云业务','N','N','全部','4','N')";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
