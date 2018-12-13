using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ClassList
{
    class listcloudlicense
    {
        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string license;

        public string License
        {
            get { return license; }
            set { license = value; }
        }
    }
}
