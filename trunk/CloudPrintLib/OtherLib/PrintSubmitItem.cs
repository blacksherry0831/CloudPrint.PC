using CloudPrintforPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPrintLib.OtherLib
{
    /// <summary>
    /// 提交记录，下单记录
    /// </summary>
    class PrintSubmitItem : PrintRecordItem
    {
        public String _userName;///打印用户
        public String _orderID;///订单号
        public String _price;///价格
        public String _hostName;

        public PrintSubmitItem(FileSendOnNet sson) : base(sson)
        {

        }

        ///主机名
        public String UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }
        public String OrderID
        {
            get
            {
                return _orderID;
            }
            set
            {
                _orderID= value;
            }
        }
        public String Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price= value;
            }
        }


        private void generatePrice()
        {
            this.Price = "0.01";
        }
        private void generateOrderID()
        {
            if (String.IsNullOrEmpty(_userName)) {
                this._userName = "";
            }
            ///
            /// 订单号：用户名+手机型号+PC+GUID
            ///blacksherry,android,hostname+GUID
            this.OrderID =Guid.NewGuid().ToString();
        }

    }
}
