using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
/*-------------------------*/
namespace CloudPrintforPC
{
   public class TreeNodeFile : TreeNode
    {
        public string mPath;
       public TreeNodeFile() {
          
        }
       public string GetImageKey() {
           return mPath;
       }
    }
   public class ListViewItemFile : ListViewItem
    {
        public string mPath;
        public ListViewItemFile() { }
        public string GetImageKey()
        {
            return mPath;
        }

    }
    //class ListViewItemPC: ListViewItem
    //{
    //    public ServerInfo mServerInfo;
    //    public ListViewItemPC() { }
    //}
   public class ListViewItemServer : ListViewItem
    {
        public ServerInfo mServerInfo;
        public ListViewItemServer() { }
    }
    //class ListViewItemPhone : ListViewItem
    //{
    //      public ListViewItemPhone() { }
    //      public ServerInfo mServerInfo;
    //}
}
