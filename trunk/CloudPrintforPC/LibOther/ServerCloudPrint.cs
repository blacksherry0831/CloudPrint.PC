using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPrintforPC
{
   public class ServerCloudPrint
    {
        private NetFindfTransfer mNetServer;
        private PrintServer mPrintServer;
        public ServerCloudPrint() 
        {
            this.mNetServer = new NetFindfTransfer();
            this.mPrintServer = new PrintServer();
        }
        public NetFindfTransfer NetServer
        {
            get { return this.mNetServer;}
        }
        public PrintServer PrintSvr
        {
            get { return this.mPrintServer;}
        }
        public void StopServer() {

            this.mNetServer.StopServer();
            this.mPrintServer.StopServer();

        }
    }
}
