
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace ConsoleApplication2
{
    public class GetPrintTask
    {


        public List<Object> getTaskFromCloud(string json_text)
        {
            List<Object> orderlist = new List<Object>();
             JArray ja = (JArray)JsonConvert.DeserializeObject(json_text);
             for (int i = 0; i < ja.Count; i++)
             {
                 Orders obj = new Orders();
                 obj.FileID = ja[i]["FileID"].ToString();
                 //obj.Filename = ja[i]["Filename"].ToString();
                 obj.Filetype = ja[i]["Filetype"].ToString();
                 obj.Iscolor = ja[i]["Color"].ToString();
                 obj.IsDuplex = ja[i]["IsDouble"].ToString();
                 obj.Printer = ja[i]["PrintBand"].ToString();
                 obj.Printcopies = ja[i]["Printcopies"].ToString();
                 obj.Prtpage = ja[i]["Graypages"].ToString();
                 obj.OrderID = ja[i]["TaskID"].ToString();
                 orderlist.Add(obj);

             }
             return orderlist;
        }
    }
}
