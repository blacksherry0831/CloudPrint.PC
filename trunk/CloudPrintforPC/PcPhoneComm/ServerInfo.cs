using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.IO;


namespace CloudPrintforPC
{

    public class ServerInfo : ServerInfoBase
    {
       
        public int mTimeOut;
        private String mFile2Send;
        public volatile bool mFileSendSuccess=false;
        public volatile bool mFileSendAbort = false;
        public volatile bool mFileSendEsc=false;
        public byte[] mBufferRcv;
        Socket mSocketClient;
        Thread mSendThread = null;
        public DateTime mTimeAlive;
        private String mIsMultiply;
        //  public int        mSelf;
        public bool IsSendFileComplete()
        {
           // bool mFlag = this.mFileSendSuccess || this.mFileSendAbort || this.mFileSendEsc ;
            if (this.mSendThread != null)
            {
                return (this.mSendThread.ThreadState == System.Threading.ThreadState.Stopped);
            }
            else {
                return true;
            }
         
                
        }
        public bool ParseNotifyMessage(byte[] msg, IPAddress addr)
        {

            this.mAddress = addr;
            Debug.WriteLine(addr.ToString());
            if (msg[0] == 0x33 && msg[4] == 0x22)
            {

                this.mPort = msg[1] * 256 + msg[2];
                Debug.WriteLine(this.mPort.ToString());
                this.mHostType = msg[3];
                Debug.WriteLine(this.mHostType == 1 ? "Pc机器" : "Phone手机");

            }
            else
            {
                return false;
            }

            return true;

        }
        public bool ParseNotifyMessageXml(byte[] msg, IPAddress addr)
        {
            this.mAddress = addr;
           // Debug.WriteLine(addr.ToString());
            if(msg==null){
                return false;
            }
            string strXml = System.Text.Encoding.UTF8.GetString(msg);
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(strXml);
                XmlNode root = doc.DocumentElement.SelectSingleNode("Notify");
                 XmlNode node;
                if ((node = root.SelectSingleNode("Port")) != null)
                this.mPort = Convert.ToInt32(node.InnerText);

               if (( node = root.SelectSingleNode("HostType")) != null)
                this.mHostType = Convert.ToInt32(node.InnerText); ;

               if (this.mHostType == 1)
               {
                 
               }
               else if(this.mHostType==2){
                   if ((node = root.SelectSingleNode("PhoneNameAlias")) != null)
                       this.mHostname = node.InnerText;
               }
              

                if((node = root.SelectSingleNode("PhoneType"))!=null){
                    this.mPhoneType = node.InnerText;
                }
                if ((node = root.SelectSingleNode("PcName")) != null){
                    this.mPcName = node.InnerText;
                }
                if ((node = root.SelectSingleNode("PhoneNameAlias")) != null){
                    this.mPhoneNameAlias = node.InnerText;
                }
                if ((node = root.SelectSingleNode("Multiply")) != null)
                {
                   mIsMultiply = node.InnerText;
                }
                	
              //  Debug.WriteLine(this.mPort.ToString());
               // Debug.WriteLine(this.mHostType == 1 ? "Pc机器" : "Phone手机");
                this.mTimeOut = 0;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool ParseNotifyMessageXml() 
        { 
              byte[] msg=this.mBufferRcv;
              IPAddress addr = this.mAddress;
             return ParseNotifyMessageXml(msg,addr);
        }

        public string GetKey()
        {
            return mAddress.ToString();
        }
        public string GetImageKey()
        {
            return this.mHostType.ToString();
        }
        public string GetServerDes()
        {
            if (this.mHostType == 1)
            {
                string a = "计算机名：" + this.mHostname + "   " + "IP地址：" + this.mAddress.ToString();
                return a;
            }
            else
            {

                string a = "手机名：" + this.mHostname + "   " + "IP地址：" + this.mAddress.ToString();
                return a;
            }

        }
        public string GetServerDes_v2()
        {
            if (this.mHostType == 1)
            {
                string a = "计算机名：" + this.mPcName + "   " + "IP地址：" + this.mAddress.ToString();
                return a;
            }
            else
            {

                string a = "手机名：" + this.mPhoneType+"  "+this.mPhoneNameAlias+ "   " + "IP地址：" + this.mAddress.ToString();
                return a;
            }

        }
        public bool SendFileOrDirectory(String fileSet)
        {
           

            if (this.IsSendFileComplete())
            {
                this.mFile2Send = fileSet;
                this.mFileSendSuccess = false;
                this.mFileSendAbort = false;
                this.mFileSendEsc = false;
                if (File.Exists(fileSet))
                {
                    this.mSendThread = new Thread(this.SendThread);
                    this.mSendThread.Start(fileSet);
                    return true;
                }
                else if (Directory.Exists(fileSet))
                {
                    return false;
                }
                else
                {
                    Debug.Assert(false);
                    return false;

                }

            }
            else {
                return false;
            }

          
             

          
        }
        public void AboratSendFile()
        {
            this.mFileSendEsc = true;
            if (this.mSocketClient != null)
            {
                this.mSocketClient.Close();
            }
        }
        public void SendThread(Object o)
        {
           this.mFileSendSuccess = false;          
           this.mFileSendAbort=false;
            //取消发送置位  终止发送
           if (mFileSendEsc == true)
           {
               return;
           }
           else  {
                try
                {
                    String filepath = (String)o;
                  
                    this.mSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    PhonePcCommunication.SendFile2Save(this.mSocketClient, filepath, this.mAddress, this.mPort);
                    /*关闭缓冲*/
                    mSocketClient.Shutdown(SocketShutdown.Both);

                    this.mFileSendSuccess = true;//发送成功

                }
                catch (Exception e){
                    this.mFileSendAbort = true;//故障终止
                    Debug.Write(e.Message);
                    return;
                }
                finally {
                    if (this.mSocketClient != null) {                    
                       this.mSocketClient.Close();
                       this.mSocketClient = null;                   
                    }

                }
               

           }        

           
        }
        public void SendPrintsThread(Object o)
        {
            this.mFileSendSuccess = false;
            this.mFileSendAbort = false;
            //取消发送置位  终止发送
            if (mFileSendEsc == true)
            {
                return;
            }
            else
            {
                try
                {
                    String filepath = (String)o;

                    this.mSocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    PhonePcCommunication.SendFile2Save(this.mSocketClient, filepath, this.mAddress, this.mPort);

                    this.mFileSendSuccess = true;//发送成功

                }
                catch (Exception e)
                {
                    this.mFileSendAbort = true;//故障终止
                    Debug.Write(e.Message);
                    return;
                }
                finally
                {
                    if (this.mSocketClient != null)
                    {
                        this.mSocketClient.Close();
                        this.mSocketClient = null;
                    }

                }


            }


        }
        public bool IsMulitiply()
        {
            if ("true".Equals(this.mIsMultiply))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public String GetMulitiply()
        {

            return this.mIsMultiply;

        }
        public string Post(string url, string data)
        {
            string returnData = null;
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = buffer.Length;
                Stream postData = webReq.GetRequestStream();
                postData.Write(buffer, 0, buffer.Length);
                postData.Close();
                HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse();
                Stream answer = webResp.GetResponseStream();
                StreamReader answerData = new StreamReader(answer);
                returnData = answerData.ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return returnData.Trim() + "\n";
        }
    }
}
