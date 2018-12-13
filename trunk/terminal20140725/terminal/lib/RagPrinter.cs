using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace terminal.lib
{
   public class RagPrinter
    {
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string pwd;

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        private string printerName;

        public string PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }


        private string beizhuName;

        public string BeizhuName
        {
            get { return beizhuName; }
            set { beizhuName = value; }
        }
    }
}
