
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
namespace CloudPrintforPC
{
    public class PCPhoneListItem : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged=null;
#if true
        private volatile bool mThreadRun = true;
#endif
        private Hashtable mPCList = Hashtable.Synchronized(new Hashtable());
        private Hashtable mPhoneList = Hashtable.Synchronized(new Hashtable());
       
        private String mPhonelistID;
        private String mPClistID;

        public String PhonePcListID 
        {
            get { return this.mPhonelistID+this.mPClistID; }
        }
        public String PhoneListID
        {
            get { return this.mPhonelistID; }
        }
        public String PCListID
        {
            get { return this.mPClistID; }
        }
        public PCPhoneListItem() 
        {
#if START_POLL
            this.StartThreadCheakPcPhone();
#endif
        }
     
        public void addItem( ServerInfo s)
        {
            if (s.mPort <= 1024) return;
            if(s.mHostType==1){
                if (this.mPCList.ContainsKey(s.GetKey()))
                {
                    ((ServerInfo)(this.mPCList[s.GetKey()])).mTimeAlive = DateTime.Now;
                    return;
                }
                else {
                    //IPHostEntry entry=Dns.GetHostEntry(s.mAddress);
                    //s.mHostname = entry.HostName;
                    lock(this.mPCList)
                    {
                        s.mTimeAlive = DateTime.Now;
                        this.mPCList.Add(s.GetKey(), s);
                        this.mPClistID = Guid.NewGuid().ToString();
                    }
                 
                }
                
            }else if(s.mHostType==2){
                if (this.mPhoneList.ContainsKey(s.GetKey()))
                {
                    ServerInfo si_inner= ((ServerInfo)(this.mPhoneList[s.GetKey()]));
                    si_inner.mTimeAlive= DateTime.Now;

                    if (si_inner.mPhoneNameAlias.Equals(s.mPhoneNameAlias))
                    {

                    }
                    else {
                        si_inner.mPhoneNameAlias = s.mPhoneNameAlias;
                         this.mPhonelistID = Guid.NewGuid().ToString();
                    }
                   
                    return;
                }
                else
                {
                    //IPHostEntry entry = Dns.GetHostEntry(s.mAddress);
                    //s.mHostname = entry.HostName;
                    lock(this.mPhoneList){
                          s.mTimeAlive = DateTime.Now;
                          this.mPhoneList.Add(s.GetKey(), s);
                          this.mPhonelistID = Guid.NewGuid().ToString();
                    }
                  
                    
                }
            }else{
            
            }
        
        }
        public void RemoveItem( ServerInfo s)
        {

            if (s.mHostType == 1){
                lock(this.mPCList){
                      this.mPCList.Remove(s.GetKey());
                      this.mPClistID = Guid.NewGuid().ToString();
                }
              
            }
            else if (s.mHostType == 2){
                lock(this.mPhoneList){
                       this.mPhoneList.Remove(s.GetKey());
                       this.mPhonelistID = Guid.NewGuid().ToString();
                }
             
            }else{

           }
        }
        public ServerInfo[] GetPClistArray() 
        {
            //var length = ht.Count;
            //Guest[] array = new Guest[length];
            //ht.Values.CopyTo(array, 0);

            var length = this.mPCList.Count;
            ServerInfo []array=new ServerInfo[length];
            this.mPCList.Values.CopyTo(array,0);
            return array;
        }
        public ServerInfo[] GetPhonelistArray()
        {
            //var length = ht.Count;
            //Guest[] array = new Guest[length];
            //ht.Values.CopyTo(array, 0);
            try {
                var length = this.mPhoneList.Count;
                ServerInfo[] array = new ServerInfo[length];
                this.mPhoneList.Values.CopyTo(array, 0);
                return array;
            
            }catch(Exception e){
                Debug.WriteLine(e.Message);
            }
            return null;
        }
        /**
    * 
    * 
    * 
    * 
    **/
        public void StopServer() 
        {
            this.mThreadRun = false;
        }
#if START_POLL
        private void StartThreadCheakPcPhone()
        {
            Thread t = new Thread(this.ThreadCheakPcPhone);
            t.Start();
        }
             /**
        * 
        * 
        * 
        * 
        **/
        private void ThreadCheakPcPhone()
        {
            int count = 0;
            while (this.mThreadRun)
            {

               poll();
            
                   Thread.Sleep(1000);
             
               if (count++ % 5 == 0)
               {
                   //每5秒检查一次心跳
                   this.CheckIsAlive(mPCList);
                   this.CheckIsAlive(mPhoneList);
               }  

            }
        }
        private void CheckIsAlive(Hashtable List)
        {
            foreach (ServerInfo si in List.Values)
            {
                this.UdpNotifyMsg_heart(si.mAddress, si.GetUdpPort());
            }
        }
        public void UdpNotifyMsg_heart(IPAddress addr, int port)
        {
            byte[] buf = this.GetNotifyMessageXml("isalive");

            for (int i = 0; i < NetFindfTransfer.mUdpSetverPort.Length; i++)
            {
                IPEndPoint endpoint = new IPEndPoint(addr, NetFindfTransfer.mUdpSetverPort[i]);
                SendMsg2Notify smn = new SendMsg2Notify(addr.ToString(), buf, endpoint);
            }
        }
        private byte[] GetNotifyMessageXml(String msg)
        {

            byte[] buf = null;

            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("CloudPrint");

            XmlNode node = doc.CreateElement("Notify");
            XmlElement elementMultiply = doc.CreateElement("Multiply");            
                       elementMultiply.InnerText = msg;
            node.AppendChild(elementMultiply);
            root.AppendChild(node);
            doc.AppendChild(root);
            buf = Encoding.UTF8.GetBytes(doc.InnerXml);
            doc.Save("Notify2.xml");
            return buf;
        }
        private void poll() 
        {
#if true
            ArrayList removeItemsPc = new ArrayList();
            ArrayList removeItemsPhone = new ArrayList();
            Thread.Sleep(100);
            lock (this.mPCList)
            {
                        foreach(ServerInfo si in this.mPCList.Values){
                        TimeSpan tsp = DateTime.Now - si.mTimeAlive;
                        if (tsp.TotalSeconds > 5000/1000.0*2)
                        {
                    
                            removeItemsPc.Add(si);
                        }
                    }
            }
          lock(this.mPhoneList)
          {
              foreach (ServerInfo si in this.mPhoneList.Values)
              {
                  TimeSpan tsp = DateTime.Now - si.mTimeAlive;
                  if (tsp.TotalSeconds > 5000 / 1000.0 * 2)
                  {
                      removeItemsPhone.Add(si);
                  }
              }
          }
           
   
            foreach(ServerInfo si in removeItemsPc){
                this.RemoveItem(si);             
            }
            foreach (ServerInfo si in removeItemsPhone)
            {
                this.RemoveItem(si);   
            }
#endif
        }
#endif

        public String PhoneCount
        {
            get {
                return this.GetPhoneCount().ToString();
            }
        }
        public String PcCount {
            get {
                return this.GetPcCount().ToString();
            }
        
        }
        private int GetPhoneCount() 
        {
            if (mPhoneList != null)
            {
                return mPhoneList.Count;
            }
            else {
                return 0;
            }
        
        }
        private int GetPcCount()
        {
            if (mPCList != null)
            {
                   return mPCList.Count;
            }else{
                   return 0;
            }
         


        }
       

    }
}
