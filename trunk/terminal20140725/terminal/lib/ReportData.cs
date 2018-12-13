using ConsoledownloadFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace printActiveX
{
    class ReportData
    {
        public void ReportCloud(string userid,string url1)
        {

            string colortext;
            string Text = userid;
            url1 += "/machine/privateCloudReportHandler.ashx";
            string url = @url1;
            List<string> list = new List<string>();
            list.Add("reports");
            list.Add(Text);
            ReportOrder report = new ReportOrder(url);
            report.sendMsgToServer(list, "post");   // report

        }

        public string getData(string fileID, int copies, bool color, string prtpages)
        {
            List<Product> products = new List<Product>(){  
            new Product(){Fileid=fileID,copies=copies,color=color,pages=prtpages},  
            };
            ProductList productlist = new ProductList();
            productlist.GetProducts = products;
            return new JavaScriptSerializer().Serialize(productlist);


        }

        public class Product
        {
            public string Fileid { get; set; }
            public int copies { get; set; }
            public string pages { get; set; }
            public bool color { get; set; }
        }

        public class ProductList
        {
            public List<Product> GetProducts { get; set; }
        }



        public class ReportOrder
        {
            string url = "";

            public string Url
            {
                get { return url; }
                set { url = value; }
            }

            public ReportOrder(string url1) { url = url1; }

            public string sendMsgToServer(List<string> list, string method)
            {
                HttpClient obj = new HttpClient(url);
                if (method.ToLower() == "post") obj.Verb = HttpVerb.POST;
                else if (method.ToLower() == "get") obj.Verb = HttpVerb.GET;
                for (int i = 0; i < list.Count; i = i + 2)
                {
                    obj.PostingData.Add(list[i], list[i + 1]);
                }
                return obj.GetString();

            }

            public string report(string username, string password, string method)
            {
                HttpClient obj = new HttpClient(url);
                if (method.ToLower() == "post") obj.Verb = HttpVerb.POST;
                else if (method.ToLower() == "get") obj.Verb = HttpVerb.GET;
                obj.PostingData.Add(username, password);
                return obj.GetString();
            }
        }
    }
}
