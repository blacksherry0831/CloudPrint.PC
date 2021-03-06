﻿#define ReceiveTimeout 
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
using Client_simplify.lists;
using terminal.lib;
using CloudPrintLib.OtherLib;
using BaseLib;

namespace CloudPrintforPC
{
   
    /**
* 
* 
* 
* */
    public  class PhonePcCommunication
    {
        public  const int RecvTimeOut =0;
        public enum  CommunicationType
        {
            GetPrinterListXml,
            Send2Receive,
            Send2Print,
            Send2PrintNoPay,
            Send2PrintPayed,
            pay4Order,
            GetPriceById,
            QueryPrintList,
            NULL
        }
        public class ClientCmdInfo
        { 
        
        }
        public static IPAddress[] GetUDPBroadcastAdddr()
        {
            IPAddress[] addr=null;

            return addr;
        }
        private static bool SendLocalPrints(Socket socketClient, String filepath, IPAddress address, int port)
        {
            FileSendOnNet file = new FileSendOnNet(filepath);
            if (socketClient.Connected == false)
            {
                socketClient.Connect(address, port);
            }

            file.SendFileInfo(socketClient);
            file.SendFile(socketClient);

            return true;
        }
        private static bool SendFileRawData(Socket socketClient, String filepath, IPAddress address, int port)
        {
            FileSendOnNet file = new FileSendOnNet(filepath);
            if(socketClient.Connected==false){
                socketClient.Connect(address, port);
            }
      
            file.SendFileInfo(socketClient);
            file.SendFile(socketClient);
           
            return true;
        }
        /// <summary>
        /// 保存到本地
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static FileSendOnNet RcvFileRawData2Save(Socket client)
        {
            FileSendOnNet fileRcv = RcvFileRawData(client);
                if(fileRcv.mFileFullPath==null){
                
                }else{
                     OpenFileInExploer(fileRcv);
                }
            return fileRcv;
        }
        /// <summary>
        /// 保存到，未付款打印队列
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static FileSendOnNet RcvFileRawData2Save_NoPay(Socket client)
        {
            FileSendOnNet fileRcv = RcvFileRawData(client);
            if (fileRcv.mFileFullPath == null)
            {

            }
            else
            {
                //文件拷贝带走。
                
                //OpenFileInExploer(fileRcv);
                //保存到提交记录
                
                PrintRecord pp = new PrintRecord();
                pp.WriteSubmitRecord2Disk_NoPay(fileRcv);
            }
            return fileRcv;
        }
        /// <summary>
        /// 直接发送打印机
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static FileSendOnNet RcvFileRawData2Print(Socket client)
        {
            FileSendOnNet fileRcv = RcvFileRawData(client);
         
             if(fileRcv.mFileFullPath==null){
                    Debug.WriteLine("打印命令：未接受到完整文件");
                }else{
                     PrintDocThread pdt = new PrintDocThread(fileRcv);
                     pdt.StartPrint();
                }
              
               
            
            return fileRcv;
        }
        public static FileSendOnNet RcvFileRawData(Socket client)
        {
            FileSendOnNet fileRcv = new FileSendOnNet();
            long FileLength = 0;
            long ReceivedLength = 0;
            FileStream fs = null;

            try
            {
              ;
                if (fileRcv.RcvFileHeader(client))
                {
#if true
                    LogHelper.WriteLog(typeof(PhonePcCommunication),"+++++++开始接受文件："+fileRcv.GetFileName());
#endif
                            fs = new FileStream(fileRcv.mFileFullPath, FileMode.Create);
                            FileLength = fileRcv.FileSize;
                            int RcvOneReadFile = 0;
                            byte[] recvBytes = new byte[1024];
                            //1.2接收文件内容
                            while (ReceivedLength < FileLength)
                            {
                                long UnReadLength = FileLength - ReceivedLength;
                                if (UnReadLength == 0) break;
                                //有数据
                                RcvOneReadFile = client.Receive(
                                     recvBytes,
                                     0,
                                    recvBytes.Length,
                                    SocketFlags.None);

                                if (RcvOneReadFile == 0)
                                {
                                    Thread.Sleep(200);
                                    if (client.Available > 0)
                                    {

                                    }
                                    else
                                    {
                                        Thread.Sleep(200);
                                    }
                                }
                                else
                                {
                                    ReceivedLength += RcvOneReadFile;
                                    fs.Write(recvBytes, 0, RcvOneReadFile);
                        
                                }
                            }




                }
            }
            catch (IOException e)
            {
                Debug.WriteLine("Request error: " + e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Request error: " + ex);
            }
            finally
            {

                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }
              //  int test_t=-1;
              //  byte[] recvBytes = new byte[1024];
                if (ReceivedLength == FileLength)
                {
#if true
                    LogHelper.WriteLog(typeof(PhonePcCommunication), "-------文件【已接受】：" + fileRcv.GetFileName());
#endif
                    //test_t = client.Receive(
                    //           recvBytes,
                    //           0,
                    //           recvBytes.Length ,
                    //           SocketFlags.None);
                    
                }
                else
                {
#if true
                    LogHelper.WriteLog(typeof(PhonePcCommunication), "********文件【接收失败】：" + fileRcv.GetFileName());
#endif
                    fileRcv.mFileFullPath = null;
                    DeleteFile(fileRcv);

                }

              
            }
          
            return fileRcv;
        }
/**
 *发送本机下挂的所有打印机
 * 
 * 
*/
        public static void SendPrinterList(Socket client)
        {
           
            XmlDocument doc=null;
            bool load_success = false;
            try {
                doc = SavePrinter2Disk();
                load_success = true;

            } catch (Exception e) {
                load_success = false;
                LogHelper.WriteLog(e.GetType(), e.Message);
            }
            /*----------------------------*/
            try {
                if (load_success)
                {
                    client.Send(System.Text.Encoding.UTF8.GetBytes(doc.InnerXml)); 
                }else{
                    client.Send(System.Text.Encoding.UTF8.GetBytes("msg")); 
                }
            } catch (Exception e2) {
                LogHelper.WriteLog(e2.GetType(), e2.Message);
            }

           
            

            
        }
        /**
 *发送本机下挂的所有打印机
 * 
 * 
*/
        public static void SendPrinterPrice(Socket client, FileSendOnNet fson)
        {
            String orderid = fson.GetProperty("orderid_suffix", "");
            String cmd = fson.GetProperty("cmd", "");
            String asyncNotify_t = fson.GetProperty("ppneedasyncnotify", "false");

            if ("getprice".Equals(cmd))
            {
                if (!String.IsNullOrEmpty(orderid))
                {
                    PrintRecordItem pri = PrintSubmitItems.QueryId(orderid);
                    if (pri != null)
                    {
                        String price_t = pri.GetPriceJson();
                        client.Send(System.Text.Encoding.UTF8.GetBytes(price_t));
                        return;
                    }
                }
            }
            else if ("PrintDoc".Equals(cmd))
            {             
                    //已经付费---打印之
                    if (!String.IsNullOrEmpty(orderid))
                    {
                            PrintRecordItem pri = PrintSubmitItems.QueryId(orderid);
                            if (pri != null)
                            {
                                String result_t = pri.GetPrintCompletedJson();
                                client.Send(System.Text.Encoding.UTF8.GetBytes(result_t));
                                return;
                            }

                            if ("true".Equals(asyncNotify_t)) {
             
                                     LogHelper.WriteLogError(typeof(PhonePcCommunication),"需要异步通知"+pri.WriteSubmitRecord2Disk(false));
                            }

                    }
            }
            else{


            }

        }
        private static bool IsSavePrinter = true;
        public static XmlDocument SavePrinter2Disk()
        {
            List<PrintOneInfo> so = PrintLocalPrint.LocalPrinter.GetLocalPrintersInfoDetail_ALL();

            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("Printers");

            for (int i = 0; i < so.Count; i++)
            {
                PrintOneInfo poi = (PrintOneInfo)so[i];
                if (poi.IsOffline() == false) {
                    XmlNode node = doc.CreateElement("Printer");
                    XmlElement elementHostName = doc.CreateElement("PrinterName");
                    elementHostName.InnerText = poi.mPrintName;
                    XmlElement elementHostType = doc.CreateElement("PrinterType");
                    elementHostType.InnerText = poi.mPrintType.ToString();
                    node.AppendChild(elementHostName);
                    node.AppendChild(elementHostType);
                    root.AppendChild(node);                
                }
              
            }

            doc.AppendChild(root);
            if (IsSavePrinter == true)
            {
                  doc.Save("Printers.xml");
                IsSavePrinter = false;
            }
            return doc;
        }
/**
* 
* 
*/
      public static void DeleteFile(FileSendOnNet fileRcv) 
       {
 
           LibCui.DeleteFile(fileRcv.mFileFullPath);

       }
/**
* 
* 
*/
public static void OpenFileInExploer( FileSendOnNet fileRcv)
 {
     Process[] wordProcesses = Process.GetProcessesByName("explorer");
        bool processExit = false;
        foreach (Process p in wordProcesses)
        {
            if (p.MainWindowTitle == fileRcv.GetFolder())
            {
                processExit = true;
            }
            else
            {

            }
        }
        if (processExit == false)
        {
            System.Diagnostics.Process.Start("Explorer.exe", @"/select," + fileRcv.mFileFullPath);
        }
 
 }
        

        private static  bool ReadServerPermission(Socket client)
        {
#if ReceiveTimeout
            client.ReceiveTimeout =PhonePcCommunication.RecvTimeOut;
#endif
            FileSendOnNet fileRcv = new FileSendOnNet();
            return fileRcv.ReadServerPermission(client);
           

        }
        private static void SendServerPermission(Socket client)
        {
            FileSendOnNet fileRcv = new FileSendOnNet();
            fileRcv.SendServerPermission(client);


        }
/**
* 
* 
* 
* */
//public bool ReadFileHeader(Socket socketClient, CommunicationType type, String filename)
//{
//        FileSendOnNet myFile = new FileSendOnNet(filename);
//        if (type == CommunicationType.Send2Receive)
//        {
//            myFile.SendFileRequestedCmd_SendFile(socketClient);
//        }
//        else if (type == CommunicationType.Send2Print)
//        {
//            myFile.SendFileRequestedCmd_PrintFile(socketClient);
//        }
//        else
//        {
//            Debug.Assert(false);
//        }

//        return true;
//}
/**
* 
* 
* 
* */
       /* private static CommunicationType RcvOperationHeader(Socket client)
        {
            FileSendOnNet myFile = new FileSendOnNet();
            return  myFile.RcvFileRequestedCmd(client);           
        }*/
        /**
* 
* 
* 
* */
        private static FileSendOnNet RcvOperationHeader_EXT(Socket client)
        {
             FileSendOnNet myFile = new FileSendOnNet();
             myFile.RcvFileRequestedCmd(client);
             return myFile;
        }
        /**
        * 
        * 
        * 
        * */
        private static bool SendOperationHeader(Socket socketClient,CommunicationType type,String filename)
        {
            FileSendOnNet myFile=new FileSendOnNet(filename);
            if (type == CommunicationType.Send2Receive)
            {
                myFile.SendFileRequestedCmd_SendFile(socketClient);
            }
            else if (type == CommunicationType.Send2Print)
            {
                myFile.SendFileRequestedCmd_PrintFile(socketClient);
            }
            else if (type == CommunicationType.GetPrinterListXml)
            {
                myFile.SendFileRequestedCmd_GetPrinters(socketClient);
            }else{
                Debug.Assert(false);
            }
           
            return true;
        }
/**
 * 
 * 
 * 
 * */

/**
* 
* 发送文件 请求打印
* 
* */
        public static bool SendFile2Print(Socket socketClient, String filename, IPAddress mAddress, int mPort)
        {

            return SendFile2_with_Cmd(socketClient, filename, CommunicationType.Send2Print, mAddress, mPort);
        }
/**
* 
* 发送文件 请求保存
* 
* */
        public static bool SendFile2Save(Socket socketClient, String filename, IPAddress mAddress, int mPort)
        {

            return SendFile2_with_Cmd(socketClient,filename,CommunicationType.Send2Receive,mAddress,mPort);
        }

        public static bool SendFile2_with_Cmd(Socket socketClient, String filename, CommunicationType type, IPAddress mAddress, int mPort)
        {
            socketClient.Connect(mAddress, mPort);
            if (socketClient.Connected == false)
            {
                return false;
            }
            /*发送发送文件头*/
            SendOperationHeader(socketClient, type, filename);
            /*读取服务器反馈*/
            bool sendAllow = ReadServerPermission(socketClient);
            if (sendAllow == true)
            {
                /*发送文件*/
                SendFileRawData(socketClient, filename, mAddress, mPort);
                return true;
            }
            else {
                return false;
                 
            }
        }
/**
* 
* 发送文件 请求保存
* 
* */
public static bool GetPrinterList(Socket socketClient, IPAddress mAddress, int mPort)
{
   
    socketClient.Connect(mAddress, mPort);
    if (socketClient.Connected == false)
    {
        return true;
    }
    /*向服务器请求打印机列表*/
    SendOperationHeader(socketClient, CommunicationType.GetPrinterListXml,null);
    /*读取服务器反馈*/
    bool sendAllow = ReadServerPermission(socketClient);
    if (sendAllow == true)
    {
        /*读取服务器发送的XML文件*/
       // SendFileRawData(socketClient, filename, mAddress, mPort);
    }
    /*关闭缓冲*/
    socketClient.Shutdown(SocketShutdown.Both);

    return true;
}
/**
* 
* TCP 服务器接收文件时发生
* 
* */
        public static bool RcvFileFromClient(Socket client)
        {
            /*接受文件头*/

            //请求头
            FileSendOnNet  fson=RcvOperationHeader_EXT(client);
            CommunicationType cType = fson.GetCommunicationType();

            if (cType == CommunicationType.Send2Print)
            {
                //打印文件
                SendServerPermission(client);
                RcvFileRawData2Print(client);
            }
            else if (cType == CommunicationType.Send2Receive)
            {
                //发送文件
                SendServerPermission(client);
                RcvFileRawData2Save(client);

            }
            else if (cType == CommunicationType.GetPrinterListXml)
            {
                //客户获取打印机列表
                //发送本地打印机列表XML文件
                SendServerPermission(client);
                SendPrinterList(client);
            }
            else if (cType == CommunicationType.Send2PrintNoPay)
            {
                //打印文件--未付款
                //返回客户端，订单号。
                SendServerPermission(client);
                //接受文件到本地磁盘
                RcvFileRawData2Save_NoPay(client);
                //PrintRecord pp = new PrintRecord();
                //pp.WriteRecord2Disk(file);
            }
            else if (cType == CommunicationType.GetPriceById) {
                //客户获取价格
                //
                SendServerPermission(client);
                SendPrinterPrice(client, fson);
                //String id fson.GetProperty("orderid_suffix","");

                //SendPrinterList(client);
            }
            else{
                //Debug.Assert(false);
            }
          
            /*返回客户端权限*/
            return true;
        
        }
        /**
        * 
        * 
        * 
        * */
        public static String IsRecommendIpAddress()
        {
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    if (_IPAddress.ToString().EndsWith(".88")) { return "true"; }
                }
            }

            return "*.*.*.88";
        }
/**
* 
* 
* 
* */
    }
}
