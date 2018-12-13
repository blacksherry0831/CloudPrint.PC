using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace CloudPrintforPC
{
   public class ServerInfoBase 
    {
        public IPAddress mAddress;
        public int mPort;
        int mUDPPort;
        public int mHostType;
        public string mHostname;
        public string mPcName;
        public string mPhoneType;
        public string mPhoneNameAlias;

      

        public  int GetUdpPort(){
		 return this.mUDPPort;
	 }
	 
	 public  void SetUdpPort(int port){
		 this.mUDPPort=port;
	 }
    }
}
