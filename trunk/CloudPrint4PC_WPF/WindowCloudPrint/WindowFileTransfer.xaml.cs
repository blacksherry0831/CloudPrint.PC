using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CloudPrintforPC;
using CloudPrintLib;
using System.Collections.ObjectModel;
using QuickZip.IO.PIDL.UserControls.ViewModel;
using System.Windows.Threading;
using System.Threading;
using System.Collections;

namespace CloudPrint4PC_WPF.WindowCloudPrint
{
    /// <summary>
    /// WindowFileTransfer.xaml 的交互逻辑
    /// </summary>
    public partial class WindowFileTransfer : Window
    {

        public ServerCloudPrint mServer;
        private DispatcherTimer dTimer = new DispatcherTimer();
        private NetFindfTransfer mNetInfo;
        // public ObservableCollection<FileData> _Files=new ObservableCollection<FileData>();
        public ObservableCollection<FileData> Files
        {
            get;
            set;
        }
        public ObservableCollection<ServerInfo> PC
        {
            get;
            set;
        }
        public ObservableCollection<ServerInfo> PHONE
        {
            get;
            set;
        }

        public String VerPC { get; set; }
        public String VerPhone { get; set; }

        public WindowFileTransfer()
        {
            InitializeComponent();

        }
        public WindowFileTransfer(ServerCloudPrint Server)
        {
            InitializeComponent();
            this.Files = new ObservableCollection<FileData>();
            //  this.listViewFile.ItemsSource = Files;
            //   this.Files.Add(new FileData("suck"));
            this.listViewFile.DataContext = this;
            this.initTimer();
            this.mServer = Server;
            this.mNetInfo = Server.NetServer;
            PC = new ObservableCollection<ServerInfo>();
            PHONE = new ObservableCollection<ServerInfo>();
            this.listBoxPhone.DataContext = this;
            this.listBoxPc.DataContext = this;

         //   this.flist.AddHandler(ListView.MouseLeftButtonDownEvent,new MouseButtonEventHandler( FileDrag_LisetViewFile_PreviewMLBD), true);

        }
        static ListBoxItem getSelectedItem(Visual sender, Point position)
        {
            HitTestResult r = VisualTreeHelper.HitTest(sender, position);
            if (r == null) return null;

            DependencyObject obj = r.VisualHit;
            while (!(obj is ListBox) && (obj != null))
            {
                obj = VisualTreeHelper.GetParent(obj);

                if (obj is ListBoxItem)
                    return obj as ListBoxItem;
            }

            return null;
        }
        static bool listBoxMouseOverSelectedItem(ListBox lbSender)
        {
            ListBoxItem _selectedItem = getSelectedItem(lbSender, Mouse.GetPosition(lbSender));
            if (_selectedItem != null && lbSender.SelectedItem != null)
                return lbSender.SelectedItems.Contains(lbSender.ItemContainerGenerator.ItemFromContainer(_selectedItem));
            return false;
        }
        private void FileDrag_LisetViewFile_PreviewMLBD(object sender, MouseButtonEventArgs e)
        {




            bool IsDrag = false;
            var listview = sender as ListView;

           

            if (listview != null)
            {

                if (listview.SelectedItems != null && listview.SelectedItems.Count != 0) {

                    System.Collections.IList list = listview.SelectedItems as System.Collections.IList;

                    IsDrag = listBoxMouseOverSelectedItem(sender as ListBox);

                    if (IsDrag){
           
                        DataObject dataObject = new DataObject(typeof(System.Collections.IList), list);
                        DragDrop.DoDragDrop(listview, dataObject, DragDropEffects.Copy | DragDropEffects.Move);//启动拖拽

                    }

                }

            }

        }

        private void ListViewFile_DropIn(object sender, DragEventArgs e)
        {
            var listView = sender as ListView;




            if (e.Data.GetDataPresent(typeof(System.Collections.IList)))
            {
                System.Collections.IList FileList = e.Data.GetData(typeof(System.Collections.IList)) as System.Collections.IList;

                foreach (var v in FileList) {

                    FileListViewItemViewModel flvivm = v as FileListViewItemViewModel;
                    if (flvivm != null) {
                        //   this._Files.Add(new FileData(flvivm.FullName));
                        FileData fd = new FileData(flvivm.FullName);
                        if (!this.Files.Contains(fd)) {
                            this.Files.Add(fd);
                        }

                    }


                }
                //index为放置时鼠标下元素项的索引  

            }
        }
        delegate Point GetPositionDelegate(IInputElement element);
        private bool IsMouseOverTarget(Visual target, GetPositionDelegate getPosition)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = getPosition((IInputElement)target);
            return bounds.Contains(mousePos);
        }

        private void initTimer()
        {
            this.dTimer.Tick += new EventHandler(this.UpdataData);

            this.dTimer.Interval = new TimeSpan(0, 0, 1);
            this.dTimer.Start();
        }
        private void UpdataData(object sender, EventArgs e)
        {
            var Timer = sender as DispatcherTimer;
            this.UpdataPC();
            this.UpdataPhone();



        }
        private void UpdataPC()
        {
            if (VerPC == null) VerPC = Guid.NewGuid().ToString();

            if (!this.mNetInfo.mPcPhoneList.PhonePcListID.Equals(VerPC))
            {
           
                ServerInfo[] array = this.mNetInfo.mPcPhoneList.GetPClistArray();
                this.PC.Clear();
                for (int i = 0; i < array.Length; i++)
                {
                    this.PC.Add(array[i]);

                }

                this.VerPC= this.mNetInfo.mPcPhoneList.PhonePcListID;

            }
        }
        private void UpdataPhone()
        {
            if (VerPhone == null) VerPhone = Guid.NewGuid().ToString();
            if (!this.mNetInfo.mPcPhoneList.PhonePcListID.Equals(VerPhone))
            {
           
                ServerInfo[] array = this.mNetInfo.mPcPhoneList.GetPhonelistArray();
                this.PHONE.Clear();
                for (int i = 0; i < array.Length; i++)
                {

                    this.PHONE.Add(array[i]);

                }
                this.VerPhone = this.mNetInfo.mPcPhoneList.PhonePcListID;
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void SendFileClick(object sender, RoutedEventArgs e)
        {
            Button button_t = sender as Button;

            SendFilePackage sfPackage = new SendFilePackage();
            foreach (ServerInfo si in this.PHONE)  //选中项遍历
            {
                if (si.WPF_CHECK)
                    sfPackage.ServerInfoSet.Add(si);
            }
            foreach (ServerInfo si in this.PC)  //选中项遍历
            {
                if (si.WPF_CHECK)
                    sfPackage.ServerInfoSet.Add(si);
            }

            if (sfPackage.ServerInfoSet.Count == 0)
            {
                MessageBox.Show("没有选中的服务器");
                return;
            }
            foreach (FileData fd in this.Files)  //选中项遍历
            {
                sfPackage.PathSet.Add(fd.FileFullname);
            }
            if (sfPackage.PathSet.Count == 0)
            {
                MessageBox.Show("没有选中文件");
                return;
            }
            button_t.IsEnabled = false;
           
            Thread t = new Thread(this.ThreadFileSendCheak);
            t.Start(sfPackage);
        }
        /**
        *
        */
        public void ThreadFileSendCheak(Object o)
        {
            SendFilePackage sfPackage = (SendFilePackage)o;
            if (sfPackage != null)
            {
                for (int i = 0; i < sfPackage.ServerInfoSet.Count; i++)
                {
                    for (int j = 0; j < sfPackage.PathSet.Count; j++)
                    {
                        sfPackage.ServerInfoSet[i].SendFileOrDirectory(sfPackage.PathSet[j]);

                        while (!sfPackage.ServerInfoSet[i].IsSendFileComplete()) ;
                        String SendDes = "主机：" + sfPackage.ServerInfoSet[i].mHostname + "文件：" + sfPackage.PathSet[j];
                        if (sfPackage.ServerInfoSet[i].mFileSendSuccess)
                        {
                            sfPackage.SendSuccess.Add(SendDes);
                        }
                        else if (sfPackage.ServerInfoSet[i].mFileSendAbort)
                        {
                            sfPackage.SendAbort.Add(SendDes);
                        }
                        else if (sfPackage.ServerInfoSet[i].mFileSendEsc)
                        {
                            sfPackage.SendEsc.Add(SendDes);
                        }
                    }

                }
            }

            this.Dispatcher.BeginInvoke((Action)delegate () {

                this.SendFileSuccess(sfPackage);

            });

                

        }
        delegate void SendFileSuccessDelegate(Object o);
        /**
       *
       */
        public void SendFileSuccess(Object o)
        {
            SendFilePackage sfPackage = (SendFilePackage)o;
            //this.Text = "文件传输------执行完成";
            MessageBox.Show(sfPackage.GetSendInfo());
            this.ButtonSend.IsEnabled = true;

        }

        private void KeyWordsMenu_Files_Delete_click(object sender, RoutedEventArgs e)
        {
            this.Files.Clear();
        }

        private void KeyDeleteSelected_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<FileData> Temp = new ObservableCollection<FileData>();


            

            foreach (var data in this.listViewFile.SelectedItems)
            {
                var t = data as FileData;
                if (t != null) {
                    Temp.Add(t);       
                }
            }

            foreach (var data in Temp)
            {
                var t = data as FileData;
                if (t != null)
                {
                    this.Files.Remove(t);
                }
            }
        }

        private void KeyDeleteSelectedFileSource_Click(object sender, RoutedEventArgs e)
        {
            this.listViewFile.SelectedItems.Clear();
        }

        private void FileDragMove(object sender, MouseEventArgs e)
        {


            var listview = sender as ListView;

            if (listview != null&&e.LeftButton == MouseButtonState.Pressed)
            {

                if (listview.SelectedItems != null && listview.SelectedItems.Count != 0)
                {
                        System.Collections.IList list = listview.SelectedItems as System.Collections.IList;
                        DataObject dataObject = new DataObject(typeof(System.Collections.IList), list);
                        DragDrop.DoDragDrop(listview, dataObject, DragDropEffects.Copy | DragDropEffects.Move);//启动拖拽
                }

            }
        }

        private void flist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /**
*
*/
    }
}
