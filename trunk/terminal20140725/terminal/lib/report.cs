using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terminal.lib
{
    class report
    {
        private string orderid;

        public string Orderid
        {
            get { return orderid; }
            set { orderid = value; }
        }
        private int pagenum;

        public int Pagenum
        {
            get { return pagenum; }
            set { pagenum = value; }
        }
    }
}
