using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Threading;
using PrintService;
/**
 *项目使用SVN管理 
 * blacksherry2
 */
namespace CloudPrintforPC
{
    public partial class MainForm : Form
    {
        private Point ptMouseCurrentPos, ptMouseNewPos, ptFormPos, ptFormNewPos;
        private bool blnMouseDown = false;
     
        //Add ContextMenuStrip .
        private ContextMenuStrip docMenu;
       
        public  ServerCloudPrint  mServer = new ServerCloudPrint();
        public MainForm()
        {
           
            InitializeComponent();
        }
        //~MainForm() {
        //    this.mNetInfo.StopServer();
        //}
        private void Form1_Load(object sender, EventArgs e)
        {
            
            //this.Top = 20;
            //this.Left = Screen.PrimaryScreen.Bounds.Width - 200;
            //this.Width = 100;
            //this.Height = 60;
#if false
            BitmapRegion BitmapRegion = new BitmapRegion();//此为生成不规则窗体和控件的类
            BitmapRegion.CreateControlRegion(this,  global::CloudPrintforPC.Properties.Resources.main_frame_border_test);
#endif  
        }

        private void frmTopMost_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                //Get the current postion of the mouse in the screen.
                ptMouseNewPos = Control.MousePosition;

                //Set window postion.
                ptFormNewPos.X = ptMouseNewPos.X - ptMouseCurrentPos.X + ptFormPos.X;
                ptFormNewPos.Y = ptMouseNewPos.Y - ptMouseCurrentPos.Y + ptFormPos.Y;

                //Save window postion.
                Location = ptFormNewPos;
                ptFormPos = ptFormNewPos;

                //Save mouse pontion.
                ptMouseCurrentPos = ptMouseNewPos;
            }
           // this.Invalidate();
           
        }

        private void frmTopMost_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;

                //Save window postion and mouse postion.
                ptMouseCurrentPos = Control.MousePosition;
                ptFormPos = Location;
            }
            this.SetBgColor();
        }

        private void frmTopMost_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = false;
            }
        }
        //Restore parent from.
        private void frmTopMost_DoubleClick(object sender, System.EventArgs e)
        {
            SwitchToMain();
        }
        private void SwitchToMain()
        {
           
        }

        #region  //Context events.
        private void OpenLable_Click(object sender, System.EventArgs e)
        {
            SwitchToMain();
        }
        private void ExitLable_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void frmTopMost_MouseEnter(object sender, System.EventArgs e)
        {
         
        }
        private void frmTopMost_MouseLeave(object sender, System.EventArgs e)
        {
          
        }

        private void ovalShape1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void mButtonExtra_Click(object sender, EventArgs e)
        {
            this.StartCircle_Click(sender,e);
        }
        private void StartCircle_Click(object sender, EventArgs e)
        {
            this.Opacity = 0.5;
#if false
               Button button = (Button)(sender);
#else
            Control button = (Control)(sender);
#endif

            Point LocationForm = new Point(button.Location.X + button.Width / 2, button.Location.Y + button.Height / 2);
            Point locationScree = this.PointToScreen(LocationForm);
            FormCircleItemSelect formSelect = new FormCircleItemSelect(locationScree);
            formSelect.mServer = this.mServer;
          
            formSelect.ShowDialog();
            this.Opacity = 1;
        }
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.mServer.StopServer();
        }

        private void pictureBox_Circle_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox_Circle.Image = global::CloudPrintforPC.Properties.Resources.button_neg;
        }

        private void pictureBox_Circle_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBox_Circle.Image = global::CloudPrintforPC.Properties.Resources.button;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
#if false
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
#endif
            //Color C = this.GetBgTransparentColor();
            //this.BackColor = C;
            //this.TransparencyKey = C;


            base.OnPaint(e);
            //g.Clear(Color.Blue);
            //g.CopyFromScreen(Location.X, Location.Y, 0, 0, this.ClientSize);
            // g.DrawImage(global::CloudPrintforPC.Properties.Resources.main_frame_border_test, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));

            // g.DrawImageUnscaled(global::CloudPrintforPC.Properties.Resources.main_frame_border_test, new Point(0,0));
        } 
        //private void MainForm_Paint(object sender, PaintEventArgs e)
        //{
        //   // Graphics g = e.Graphics;
        //   // g.Clear(Color.Blue);
        //   // //Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
          
        //   // //设置截屏区域
        //   //g.CopyFromScreen(Location.X,Location.Y, 0, 0,this.Size);
        //   /*Color C=this.GetBgTransparentColor();
        //   this.BackColor = C;
        //   this.TransparencyKey = C;*/
        //}

        public Color GetBgTransparentColor()
        {          
            
            Bitmap image = new Bitmap(this.ClientSize.Width,this.ClientSize.Height);
            Graphics g = Graphics.FromImage(image);
            g.CopyFromScreen(Location.X, Location.Y, 0, 0, this.Size);
            //image.Save("suck.jpg");
            List<Color> ColorSet = new List<Color>();
            for (int x = 0; x < 10;x++){
                for (int y = image.Height - 8; y < image.Height; y++) {
                    ColorSet.Add(image.GetPixel(x, y));
                }
            }

            int Cr = 0;
            int Cg = 0;
            int Cb = 0;
            foreach(Color c in ColorSet){
                Cr += c.R;
                Cg += c.G;
                Cb += c.B;
            }
            Cr /= ColorSet.Count;
            Cg /= ColorSet.Count;
            Cb /= ColorSet.Count;
            g.Dispose();
            return Color.FromArgb(Cr, Cg, Cb);
           
        }

        public void SetBgColor() 
        {
#if true
            Color C = this.GetBgTransparentColor();
            if (!this.BackColor.Equals(C))
            {
                this.BackColor = C;
                this.TransparencyKey = C;
            }
#endif
        }

        private void timerUpdataBG_Tick(object sender, EventArgs e)
        {
            this.SetBgColor();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
        //    this.SetBgColor();
        }
        private void timerHideView_Tick(object sender, EventArgs e)
        {

            this.Visible = true;
            this.Opacity = 1;
            if (sender.GetType() == typeof(System.Windows.Forms.Timer))
            {
                System.Windows.Forms.Timer t = (System.Windows.Forms.Timer)sender;
                t.Stop();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                this.timerUpdataBG.Stop();
                this.TransparencyKey = Color.Blue;
            }
            if (e.KeyCode == Keys.F12)
            {
                this.timerUpdataBG.Start();
            }
            if (e.KeyData == (Keys.L|Keys.Control)) {
                String a=LibCui.ReadTxt("txt.txt");
                TextFilePrinter tfp = new TextFilePrinter(a);
                tfp.View();
                tfp.Print();
            }
        }

        private void timer_updata_view_content_Tick(object sender, EventArgs e)
        {
            this.label_phone_num.Text = this.mServer.NetServer.mPcPhoneList.PhoneCount;
            this.label_printer_num.Text = this.mServer.PrintSvr.PrintCount;
        }
        
    }
}
