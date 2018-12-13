using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_simplify.lists
{
   public class Class_PrintArgs
    {
        private string filepath;

        public string Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }
        private int copies;

        public int Copies
        {
            get { return copies; }
            set { copies = value; }
        }
        private string m_pages;

        public string Pages
        {
            get { return m_pages; }
            set { m_pages = value; }
        }
        private bool color;

        public bool Color
        {
            get { return color; }
            set { color = value; }
        }
        private bool isDuplex;

        public bool IsDuplex
        {
            get { return isDuplex; }
            set { isDuplex = value; }
        }
        private string filename;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
    }
}
