﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BaseLib;

namespace CloudPrintforPC
{
 public   class FileSendOnNet
    {
        private static int mPrefix=0;
        PhonePcCommunication.CommunicationType mCommunicationType;
        String mFileFullName;
        public String mFileFullPath;
        ArrayList List = new ArrayList();
        private  byte[] mHandshake=new byte[4]{0x22,0x22,0x33,0x33};
        private  byte[] mHandshakeBuffer = new byte[4];

        private byte[] mFileSizeBuffer=new byte[512];
         long          mFileSizeLong;
        private byte[] mFileNameBuffer =new byte[1024];
        String       mFileNameString;
        private byte[] mFileOpeaCmdBuffer = new byte[256];
        String mFileOpeaCmdBufferString;
        private byte[] mFileTransferCmdBuffer = new byte[256*2];
        private byte[] mPrinterNameBuffer = new byte[1024];
        private String mPrinterName;
        /*---------------------------------*/
        //打印参数
       public  int PParamcopies = 1;//份数
       public bool PParamcolor = false;//彩打
       public String PParamRange = "全部";//打印范围
       public bool PParam2Paper = false;//双面
        private byte[] mPParamcopiesBuffer = new byte[256];
        private byte[] mPParamcolorBuffer = new byte[256];
        private byte[] mPParamRangeBuffer = new byte[256];
        private byte[] mPParam2PaperBuffer = new byte[256];
        /*---------------------------------*/
        public String _PhoneNumber;
        public String _OsNumber;
        public String _PhoneType;
        private byte[] _Android_os_buffer = new byte[256];
        private byte[] _Android_phone_num = new byte[256];
        private byte[] _Android_phone_type = new byte[256];
        /*---------------------------------*/
        private byte[] _json_ext_buffer = new byte[256*20];
        private String _json_ext_string;
        public FileSendOnNet(String fileName) 
        {
            this.initBuffer();
            /*-----数组大小----*/
            this.mFileFullName = fileName;
            /*-----文件名----*/
            String filename = System.IO.Path.GetFileName(fileName);
            byte[] fileName_t = System.Text.Encoding.UTF8.GetBytes(filename);
            fileName_t.CopyTo(this.mFileNameBuffer, 0);

            if (File.Exists(fileName))
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                String fileLen = fs.Length.ToString();   // 文件长度
                byte[] fileSize_t = System.Text.Encoding.UTF8.GetBytes(fileLen);
                fileSize_t.CopyTo(mFileSizeBuffer, 0);
            }
            else {
                int length=-1;
                String fileLen = length.ToString();
                byte[] fileSize_t = System.Text.Encoding.UTF8.GetBytes(fileLen);
                fileSize_t.CopyTo(mFileSizeBuffer, 0);
            }         

        }
         public FileSendOnNet() 
        {

            this.initBuffer();
            this.mFileFullName = Environment.CurrentDirectory+"\\"+GetFolder()+"\\";

            if (false==Directory.Exists(this.mFileFullName)) {
                Directory.CreateDirectory(this.mFileFullName);
            }

        }
         public FileSendOnNet(String fileRcvPath,int type)
         {

             this.initBuffer();
             this.mFileFullName =fileRcvPath;

             if (false == Directory.Exists(this.mFileFullName))
             {
                 Directory.CreateDirectory(this.mFileFullName);
             }

         }
         public void initBuffer()
         {
             Array.Clear(mHandshakeBuffer, 0, mHandshakeBuffer.Length);
             Array.Clear(mFileSizeBuffer, 0, mFileSizeBuffer.Length);
             Array.Clear(mFileNameBuffer, 0, mFileNameBuffer.Length);
             Array.Clear(mFileOpeaCmdBuffer, 0, mFileOpeaCmdBuffer.Length);
         }
         public String GetFolder()
         {
             return "FileRecv";
         }
        public void ConvertBuffer2RealData()
        {
            /*-----文件大小----*/
            String fileSizeLongStr = GetStringFromBuffer(this.mFileSizeBuffer);
            this.mFileSizeLong = long.Parse(fileSizeLongStr);
            /*-----文件名----*/
            this.mFileNameString ="F-"+mPrefix+"-"+Guid.NewGuid().ToString()+"-"+GetStringFromBuffer(this.mFileNameBuffer);
            /*--------------*/
            this.mFileFullPath = this.mFileFullName + this.mFileNameString;
            /*客户端请求命令*/
            this.mFileOpeaCmdBufferString = GetStringFromBuffer(this.mFileOpeaCmdBuffer);
            /*--------------*/
            this.mPrinterName= GetStringFromBuffer(this.mPrinterNameBuffer);

            /*----------------------------------------------------------------------------*/
            String  temp = this.GetStringFromBuffer(this.mPParamcopiesBuffer);
            try {
                   this.PParamcopies = int.Parse(temp);
            }catch(Exception e){
                this.PParamcopies = 1;
            }
         
            temp = this.GetStringFromBuffer(this.mPParamcolorBuffer);
            if (temp.Equals("true")) {
                this.PParamcolor = true;
            }
            PParamRange = this.GetStringFromBuffer(this.mPParamRangeBuffer);
            if (PParamRange.Equals("")) {
                PParamRange = "全部";
            }
            temp = this.GetStringFromBuffer(this.mPParam2PaperBuffer);
            if (temp.Equals("true"))
            {
                this.PParam2Paper = true;
            }
            /*----------------------------------------------------------------------------*/
            _PhoneNumber=this.GetStringFromBuffer(this._Android_phone_num);
            _OsNumber=this.GetStringFromBuffer(this._Android_os_buffer);
            _PhoneType=this.GetStringFromBuffer(this._Android_phone_type);
            /*----------------------------------------------------------------------------*/
            this._json_ext_string = this.GetStringFromBuffer(this._json_ext_buffer);
        }
        public string GetStringFromBuffer(byte [] buffer)
        {
            String str=System.Text.Encoding.UTF8.GetString(buffer);
            str = str.Replace("\0", "");
            return str;
        }
        private bool IsHandShark()
        {
            bool bo=false;
            byte[]a=this.mHandshakeBuffer;
            byte[]b=this.mHandshake;
            for(int i=0;i<b.Length;i++)
            {
               if(a[i]==b[i])
                  bo=true;
               else
               {
                  bo=false;
                  break;
                }
            }
            return bo;
        }
        public byte[] GetFileHeader() 
        {
            byte []header=null;
          
            header= GetOperationHeader("PC 文件大小 打印机名");
            return header;
        }
/**
* 生成文件头
*   
*      
*/
        public byte[] GetOperationHeader(String cmd)
        {

            byte[] header = null;
            List.AddRange(this.mHandshake);
            List.AddRange(this.mFileSizeBuffer);
            List.AddRange(this.mFileNameBuffer);
         
            mFileOpeaCmdBufferString =cmd;
            byte[] buffer_t = System.Text.Encoding.UTF8.GetBytes(mFileOpeaCmdBufferString);
            buffer_t.CopyTo(mFileOpeaCmdBuffer, 0);
            List.AddRange(this.mFileOpeaCmdBuffer);

            List.AddRange(this.mPrinterNameBuffer);
          
            /*----------------------------------------*/

            List.AddRange(this.mPParamcopiesBuffer);
            List.AddRange(this.mPParamcolorBuffer);
            List.AddRange(this.mPParamRangeBuffer);
            List.AddRange(this.mPParam2PaperBuffer); /**/
            /*----------------------------------------*/
            List.AddRange(this._Android_os_buffer);
            List.AddRange(this._Android_phone_num);
            List.AddRange(this._Android_phone_type);
            /*----------------------------------------*/
            List.AddRange(this._json_ext_buffer);
            return header = (byte[])List.ToArray(typeof(byte));;
        }
/**
* 生成文件头
*  
*      public
*/
        public bool RcvFileHeader(Socket client) 
        {
            RcvBufferFully(client,mHandshakeBuffer);
          if (IsHandShark())
          {
               RcvBufferFully(client,mFileSizeBuffer);
               RcvBufferFully(client,mFileNameBuffer);
               RcvBufferFully(client,mFileOpeaCmdBuffer); 
               RcvBufferFully(client,mPrinterNameBuffer);
              /*--------------------------------------------------------------*/
            
                 RcvBufferFully(client,mPParamcopiesBuffer);
                 RcvBufferFully(client,mPParamcolorBuffer);
                 RcvBufferFully(client,mPParamRangeBuffer);
                 RcvBufferFully(client,mPParam2PaperBuffer);
                 /* * */
              /*--------------------------------------------------------------*/
                 RcvBufferFully(client,_Android_os_buffer);
                 RcvBufferFully(client,_Android_phone_num);
                 RcvBufferFully(client, _Android_phone_type);
                /*--------------------------------------------------------------*/
                RcvBufferFully(client, _json_ext_buffer);
                ConvertBuffer2RealData();
              return true;
          }else{
            return false;
          }
        }
        public static void RcvBufferFully(Socket client,byte[] bufffer) 
        {
            int Total =bufffer.Length;
            int offset = 0;
            int maxLoop = 0;
            int RcvTotal = 0;
            while (bufffer.Length != RcvTotal){
                int Rcv_Once = client.Receive(bufffer, offset, Total, SocketFlags.None);
                if (Rcv_Once == 0)
                {
                    Thread.Sleep(100);
                    //5分钟=5X60x1000=5x60x10x100=3000
                    if (maxLoop++ > 3000)
                        break;
                }
                else {
                    RcvTotal+= Rcv_Once;
                    offset += Rcv_Once;
                    Total -= Rcv_Once;
                }

                
              
            }
          
        }
        public byte[] GetSendPrintHeader()
        {

            return GetOperationHeader("PrintFile");
        }
        public byte[] GetSendFileHeader()
        {
            return GetOperationHeader("SendFile");
        }
        public byte[] GetXmlPrintsHeader()
        {
            return GetOperationHeader("Xml");
        }
        public static bool IsFile(String path)
        {
            if (File.Exists(path))
            {
               return true;
            }
            else if (Directory.Exists(path))
            {
               return false;
            }
           
            return false;
        }
        public static int SendDataCui(Socket socket, byte[] buffer, int outTime) 
        {
            if (socket == null || socket.Connected == false)
            {
                throw new ArgumentException("参数socket 为null，或者未连接到远程计算机");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("参数buffer 为null ,或者长度为 0");
            }

            int flag = 0;
            try
            {
                int left = buffer.Length;
                int sndLen = 0;

                while (true)
                {
                    if ((socket.Poll(outTime * 100, SelectMode.SelectWrite) == true))
                    {        // 收集了足够多的传出数据后开始发送
                        sndLen = socket.Send(buffer, sndLen, left, SocketFlags.None);
                        left -= sndLen;
                        if (left == 0)
                        {                                        // 数据已经全部发送
                            flag = 0;
                            break;
                        }
                        else
                        {
                            if (sndLen > 0)
                            {                                    // 数据部分已经被发送
                                continue;
                            }
                            else
                            {                                                // 发送数据发生错误
                                flag = -2;
                                break;
                            }
                        }
                    }
                    else
                    {                                                        // 超时退出
                        flag = -1;
                        break;
                    }
                }
            }
            catch (SocketException e)
            {

                flag = -3;
                throw e;
            }
            return flag;
        }
        /// <summary>
        /// 向远程主机发送数据
        /// </summary>
        /// <param name="socket">要发送数据且已经连接到远程主机的 Socket</param>
        /// <param name="buffer">待发送的数据</param>
        /// <param name="outTime">发送数据的超时时间，以秒为单位，可以精确到微秒</param>
        /// <returns>0:发送数据成功；-1:超时；-2:发送数据出现错误；-3:发送数据时出现异常</returns>
        /// <remarks >
        /// 当 outTime 指定为-1时，将一直等待直到有数据需要发送
        /// </remarks>
        /// 
#if false
        public static int SendData(Socket socket, byte[] buffer, int outTime)
        {
            if (socket == null || socket.Connected == false)
            {
                throw new ArgumentException("参数socket 为null，或者未连接到远程计算机");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("参数buffer 为null ,或者长度为 0");
            }

            int flag = 0;
            try
            {
                int left = buffer.Length;
                int sndLen = 0;

                while (true)
                {
                    if ((socket.Poll(outTime * 1000*1000*2, SelectMode.SelectWrite) == true))
                    {        // 收集了足够多的传出数据后开始发送
                        sndLen = socket.Send(buffer, sndLen, left, SocketFlags.None);
                        left -= sndLen;
                        if (left == 0)
                        {                                        // 数据已经全部发送
                            flag = 0;
                            break;
                        }
                        else
                        {
                            if (sndLen > 0)
                            {                                    // 数据部分已经被发送
                                continue;
                            }
                            else
                            {                                                // 发送数据发生错误
                                flag = -2;
                                break;
                            }
                        }
                    }
                    else
                    {                                                        // 超时退出
                        flag = -1;
                        break;
                    }
                }
            }
            catch (SocketException e)
            {

                flag = -3;
                throw e;
            }
            return flag;
        }
#endif
        public static int SendData_V2(Socket socket, byte[] buffer, int outTime)
        {
            if (socket == null || socket.Connected == false)
            {
                throw new ArgumentException("参数socket 为null，或者未连接到远程计算机");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("参数buffer 为null ,或者长度为 0");
            }

            int flag = 0;
            try
            {
                int left = buffer.Length;
                int sndLen = 0;

                while (true)
                {
                    {        // 收集了足够多的传出数据后开始发送
                        sndLen = socket.Send(buffer, sndLen, left, SocketFlags.None);
                        left -= sndLen;
                        if (left == 0)
                        {                                        // 数据已经全部发送
                            flag = 0;
                            break;
                        }
                        else
                        {
                            if (sndLen > 0)
                            {                                    // 数据部分已经被发送
                                continue;
                            }
                            else
                            {                                                // 发送数据发生错误
                                flag = -2;
                                break;
                            }
                        }
                    }
                    
                }
            }
            catch (SocketException e)
            {

                flag = -3;
                throw e;
            }
            return flag;
        }
         /// <summary>
        /// 向远程主机发送文件
        /// </summary>
        /// <param name="socket" >要发送数据且已经连接到远程主机的 socket</param>
        /// <param name="fileName">待发送的文件名称</param>
        /// <param name="maxBufferLength">文件发送时的缓冲区大小</param>
        /// <param name="outTime">发送缓冲区中的数据的超时时间</param>
        /// <returns>0:发送文件成功；-1:超时；-2:发送文件出现错误；-3:发送文件出现异常；-4:读取待发送文件发生错误</returns>
        /// <remarks >
        /// 当 outTime 指定为-1时，将一直等待直到有数据需要发送
        /// </remarks>
        ///    
        /// public static int SendFile(Socket socket, string fileName, int maxBufferLength, int outTime)
        public static int SendFile(Socket socket, string fileName, int maxBufferLength, int outTime)
        {
            if (fileName == null || maxBufferLength <= 0)
            {
                throw new ArgumentException("待发送的文件名称为空或发送缓冲区的大小设置不正确.");
            }
            int flag = 0;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                long fileLen = fs.Length;                        // 文件长度
                long leftLen = fileLen;                            // 未读取部分
                int readLen = 0;                                // 已读取部分
                byte[] buffer = null;

                if (fileLen <= maxBufferLength)
                {            /* 文件可以一次读取*/
                    buffer = new byte[fileLen];
                    readLen = fs.Read(buffer, 0, (int)fileLen);
                    flag = SendData_V2(socket, buffer, outTime);
                }
                else
                {
                    /* 循环读取文件,并发送 */

                    while (leftLen != 0)
                    {
                        if (leftLen < maxBufferLength)
                        {
                            buffer = new byte[leftLen];
                            readLen = fs.Read(buffer, 0, Convert.ToInt32(leftLen));
                        }
                        else
                        {
                            buffer = new byte[maxBufferLength];
                            readLen = fs.Read(buffer, 0, maxBufferLength);
                        }
                        if ((flag = SendData_V2(socket, buffer, outTime)) < 0)
                        {
                            break;
                        }
                        leftLen -= readLen;
                    }
                }
                fs.Flush();
                fs.Close();
            }
            catch (IOException e)
            {

                flag = -4;
                throw e;
            }
            return flag;
        }
        public static int SendFile(Socket socket, string fileName) 
        {
           return SendFile(socket,fileName, 10*1024*1024,10);
        }
        public long FileSize 
        {
            get { return this.mFileSizeLong; }       
        }
/**
* 
* 发送内置文件头
* 
* 
* */
        public void SendFileInfo(Socket socket) 
        {
            socket.Send(this.GetFileHeader());
        }
/**
* 
* 发送内置文件
* 
* 
* */
        public void SendFile(Socket socket)
        {
            SendFile(socket, this.mFileFullName);
        }
/**
* 
* 发送 请求发送命令
* 
* 
* */
        public void SendFileRequestedCmd_SendFile(Socket socket)
        {
            socket.Send(this.GetSendFileHeader());
        }
/**
* 
* 接受请求命令
* 
* 
* */
        public PhonePcCommunication.CommunicationType RcvFileRequestedCmd(Socket socket)
        {

         
            if (RcvFileHeader(socket))
            {

                if (this.mFileOpeaCmdBufferString.Equals("SendFile"))
                {
                    //没有付费
                    return mCommunicationType=PhonePcCommunication.CommunicationType.Send2Receive;
                }
                else if (this.mFileOpeaCmdBufferString.Equals("PrintFile"))
                {
                    return mCommunicationType=PhonePcCommunication.CommunicationType.Send2Print;

                }
                else if (this.mFileOpeaCmdBufferString.Equals("PrinterListXml"))
                {
                    return mCommunicationType=PhonePcCommunication.CommunicationType.GetPrinterListXml;
                } else if (this.mFileOpeaCmdBufferString.Equals("SubmitNoPay")) {
                    return mCommunicationType=PhonePcCommunication.CommunicationType.Send2PrintNoPay;
                } else if (this.mFileOpeaCmdBufferString.Equals("None")) {                   
                    Debug.Assert(false); //不应该出现的场景
                } else if((this.mFileOpeaCmdBufferString.Equals("GetPriceById"))){
                    return mCommunicationType=PhonePcCommunication.CommunicationType.GetPriceById;
                }else{
                    //Debug.Assert(false);
                    return mCommunicationType=PhonePcCommunication.CommunicationType.NULL;
                }

            }
           // Debug.Assert(false);
            return mCommunicationType=PhonePcCommunication.CommunicationType.NULL;
        }
/**
* 
* 发送  请求打印命令
* 
* 
* */
        public void SendFileRequestedCmd_GetPrinters(Socket socket)
        {
            socket.Send(this.GetXmlPrintsHeader());
        }
    
/**
* 
* 发送  请求打印命令
* 
* 
* */
        public void SendFileRequestedCmd_PrintFile(Socket socket)
        {
            socket.Send(this.GetSendPrintHeader());
        }

/**
* 
* 读取服务器权限
* 
* 
* */
        public bool ReadServerPermission(Socket socket)
        {
          
            socket.Receive(mHandshakeBuffer, 0, mHandshakeBuffer.Length, SocketFlags.None);
            if (IsHandShark())
            {
                socket.Receive(mFileTransferCmdBuffer, 0, mFileTransferCmdBuffer.Length, SocketFlags.None);
                socket.Receive(_json_ext_buffer,0,_json_ext_buffer.Length, SocketFlags.None);
            }
            /*0x22 允许权限控制*/
            if (this.mFileTransferCmdBuffer[0] == 0x22)
            {
                return true;
            }else{
                return false;
            }
          //  return true;
          
        }
/**
* 
* 发送服务器权限
* 
* 
* */
        public bool SendServerPermission(Socket socket)
        {
            socket.Send(this.mHandshake);
            this.mFileTransferCmdBuffer[0] = 0x22;
            socket.Send(this.mFileTransferCmdBuffer);
            socket.Send(this._json_ext_buffer);
            return true;

        }
        /**
        * 
        * 
        * string fileOu  tFJ = System.IO.Directory.GetCurrentDirectory() + @"\PCL\" + fileid + "FJ" + ".prn";
        * 
        * */
        public String GetPCLprnfullPath() 
        {
             String path = Path.GetTempPath() + this.mFileNameString+".prn";
             return path;
        }
    /**
        * 
        * 
        * string fileOu  tFJ = System.IO.Directory.GetCurrentDirectory() + @"\PCL\" + fileid + "FJ" + ".prn";
        * 
        * */
        public String GetPrinterName()
        {
            if (String.IsNullOrEmpty(this.mPrinterName))
            {
                return this.mPrinterName="";
            }
            String printerDriveName_t= this.GetProperty("PrinterDriveName", "");//打印机型号
            return PrintLocal.GetUsefulPrinter(this.mPrinterName,printerDriveName_t);
           
        }

        public String GetPrinterDriveName()
        {
            String printerDriveName_t = this.GetProperty("PrinterDriveName", "");//打印机型号
            if (!String.IsNullOrEmpty(printerDriveName_t))
            {
                return printerDriveName_t;
            }else {
                printerDriveName_t= PrintLocal.GetPrinterDrivenName(this.mPrinterName);
                this.SetProperty("PrinterDriveName",printerDriveName_t);
                    
            }
            return printerDriveName_t;

        }
/**
*
*
*/
        public void SetPrinterName(String pn)
        {
            if (pn != null)
            {
                this.mPrinterName = pn;
               
            }
          
        }
     /**
      * 
      */
        public String GetFileName()
        {
            return this.mFileNameString;
        }


        public String FileFullPath
        {
            get {
                return this.mFileFullPath;
            }
            set {

                if (value!=null)
                {
                    this.mFileNameString= this.mFileFullName =Path.GetFileName(value);
                    this.mFileFullPath = value;
                    this.mFileNameString.Replace(".", "_");
                }
            }
        }
        /**
        * {
            "phonetype": "HUAWEI C8815",
            "pprange": "全部",
            "phonenumber": "ffffffff-940e-7185-2691-c7390033c587",
            "printername": "No Printer",
            "price2pay": "-1",
            "username": "blacksherry",
            "pppapersize": "A4",
            "printid": "订单号",
            "systemtype": "4.1.2",
            "ppcolor": false,
            "ppduplex": false,
            "ppcopies": "1"
}
        */
        public String GetUserName()
        {
             return this.GetProperty("username","no user");

        }
        public void SetUserName(String value_t)
        {
            this.SetProperty("username",value_t);
        }

        public String GetOrderId()
        {
            return this.GetProperty("orderid", "no id");

        }
        public void  SetOrderId(String value_t)
        {
            this.SetProperty("orderid", value_t);

        }
        public String GetOrderId_Suffix()
        {
            return this.GetProperty("orderid_suffix", "no id");

        }
        public void SetOrderId_Suffix(String value_t)
        {
            this.SetProperty("orderid_suffix",value_t);
          

        }
        public String GetPrice2Pay()
        {
            String price_t = this.GetProperty("price2pay", PrintRecordItem.PriceNone);

            if (LibCui.IsStringNeg(price_t)) {
                    return PrintRecordItem.PriceNone;
            } else {
                    return price_t;
            }          

        }
        public void SetPrice2Pay(String value_t)
        {
           this.SetProperty("price2pay", value_t);
        }
        /**
          * 
          */
        public string GetProperty(String property,String default_t)
        {
            

            if (String.IsNullOrEmpty(this._json_ext_string))
            {
                return default_t;
            }
            else
            {

                try
                {
                    JObject jo = JObject.Parse(this._json_ext_string);

                    if (jo.Property(property) == null || jo.Property(property).ToString() == "")
                    {

                    }
                    else {
                             default_t =(jo[property]).ToString();
                    }     
                  
                }
                catch (Exception e)
                {
                    default_t = e.Message;
                }

                return default_t;
            }
        }
        /**
         * 
         */
        public void SetProperty(String property, String value_t)
        {
               /*---------------------------------------------------*/
                try
                {
                   JObject jo = JObject.Parse(this._json_ext_string);  
                }
                catch (Exception e)
                {
                    this._json_ext_string = new JObject().ToString();
                }
            /*---------------------------------------------------*/
                try
                {
                    JObject jo = JObject.Parse(this._json_ext_string);
                    jo[property] = value_t;
                    this._json_ext_string = jo.ToString();
                }
                catch (Exception e)
                {
                    this._json_ext_string = new JObject().ToString();
                }
            /*---------------------------------------------------*/

        }
        /**
          */
        public PhonePcCommunication.CommunicationType GetCommunicationType()
        {
            return this.mCommunicationType;
        }

        /**
          * 
          */
        public void Parse4PrintRecord(PrintRecordItem pri)
        {
            /*-----------------------------------*/
              this.SetOrderId(pri._OrderId);
              this.SetOrderId_Suffix( pri.OrderId_Suffix);
              this.SetUserName(pri._UserName);
              this.SetPrice2Pay(pri.GetPrice());
              this.FileFullPath=pri._FileFullPath;
            /*-----------------------------------*/
              this._PhoneNumber = pri._phoneNumber;
              this._PhoneType= pri._phoneType;
              this._OsNumber=pri._androidOs;
              this.PParamcopies= pri._PParamcopies;
              this.PParamcolor=  pri._PParamcolor;
              this.PParamRange= pri._PParamRange;
              this.PParam2Paper=pri._PParam2Paper;
              //pri._Paper_Type = "自动适应纸张大小";
              this.SetPrinterName(pri._PrinterName);
              this.SetProperty("PrinterDriveName", pri.PrinterDriveName);
              this.SetProperty("ppneedasyncnotify",pri.AsyncNotify);
            /*-----------------------------------*/
        }

    }

}
