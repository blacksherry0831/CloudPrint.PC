using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Orders
    {

        private string fileID;

        public string FileID
        {
            get { return fileID; }
            set { fileID = value; }
        }

        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        private string isDuplex;

        public string IsDuplex
        {
            get { return isDuplex; }
            set { isDuplex = value; }
        }
        private string iscolor;

        public string Iscolor
        {
            get { return iscolor; }
            set { iscolor = value; }
        }
        private string prtpage;

        public string Prtpage
        {
            get { return prtpage; }
            set { prtpage = value; }
        }
        private string printcopies;

        public string Printcopies
        {
            get { return printcopies; }
            set { printcopies = value; }
        }


        private string filetype;

        public string Filetype
        {
            get { return filetype; }
            set { filetype = value; }
        }

        private string printer;

        public string Printer
        {
            get { return printer; }
            set { printer = value; }
        }
        private string peisong;

        public string Peisong
        {
            get { return peisong; }
            set { peisong = value; }
        }
        private string orderID;

        public string OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
      }
}
