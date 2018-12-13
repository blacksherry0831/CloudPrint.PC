//#define SHOW_TEST_CIRCLE 
#define USE_BUTTON_ITEM
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Configuration;
using BaseLib;

namespace CloudPrintforPC
{

    public partial class FormCircleItemSelect : Form
    {
        private  readonly int WIDTH = 130*2;
        private  readonly int HEIGHT = 130*2;
        private Size mOldClientSize = new Size(0, 0);
        public ServerCloudPrint mServer;
        public const float mButtonCircleSize=(float)(0.120);//按钮占宽高的比例
        public const float mButtonCenterSize = (float)(0.1);//中心按钮占全局的比例
        public const float mCircleLength= (float)0.125;//圆环的粗细
        public const float mCircleRadius_ARC=(float)0.36;//外圆半径位置
        public const float mCircleLengthIn=(float)0.14;//内圆半径
        public Point mCenter;
        public Point mMousePoint; 
        public Rectangle mCircleOut;
        public Rectangle mCircleCenter;
        public Rectangle mCircleIn;
        public int mChildNum;
        public int mChildSelectedIdx;
        public bool mSelectedMode;
        public Point mLocationCenter;
        Control[] mChildButton;
        PictureBoxCircule mCenterButton = new PictureBoxCircule(false);
        /*---------------------------------------------------------*/
        String[] ButtonTip = new String[] {
             Properties.Resources.StringCircleNetPrint,
             Properties.Resources.StringCirclePhoneManager,
             Properties.Resources.StringCircleStatistics,
             Properties.Resources.StringCircleExit,
             Properties.Resources.StringCircleFileTransfer,
             Properties.Resources.StringCircleCloudPrint
            };
        /*---------------------------------------------------------*/
        Image[] mImage = new Image[]{
              global::CloudPrintforPC.Properties.Resources.net_print,
              global::CloudPrintforPC.Properties.Resources.phone_magager,
              global::CloudPrintforPC.Properties.Resources.data_statistical,
              global::CloudPrintforPC.Properties.Resources.system_exit,
              global::CloudPrintforPC.Properties.Resources.file_transfer,
              global::CloudPrintforPC.Properties.Resources.shared_print
        };

        Image[] mImage_Selected  =  new Image []{
#if false
              global::CloudPrintforPC.Properties.Resources.net_print_selected,
              global::CloudPrintforPC.Properties.Resources.phone_magager_seleted,
              global::CloudPrintforPC.Properties.Resources.data_statistical_selected,
              global::CloudPrintforPC.Properties.Resources.system_exit_selected,
              global::CloudPrintforPC.Properties.Resources.file_transfer_selected,
              global::CloudPrintforPC.Properties.Resources.shared_print_selected
#endif
        };
        Image[] mImage_UnSelected = new Image[]{
#if false
              global::CloudPrintforPC.Properties.Resources.net_print_unselected,
              global::CloudPrintforPC.Properties.Resources.phone_magager_Unseleted,
              global::CloudPrintforPC.Properties.Resources.data_statistical_unselected,
              global::CloudPrintforPC.Properties.Resources.system_exit_unsenected,
              global::CloudPrintforPC.Properties.Resources.file_transfer_unselected,
              global::CloudPrintforPC.Properties.Resources.shared_print_unselected
#endif
        };
        /*---------------------------------------------------------*/
        public FormCircleItemSelect(Point locationScreen)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.mLocationCenter = locationScreen;
            this.initParam();
#if true
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
#endif
            this.Visible = false;
            this.Opacity = 0;           

        }
        public void initParam() {
            
            InitializeComponent();
            this.SuspendLayout();
                this.mChildNum = 6;
                this.mChildSelectedIdx = -1;
                this.UpdataCenterPoint();
                this.AddButton2Main();
                this.Width = WIDTH;
                this.Height = HEIGHT;
            this.ResumeLayout();
            this.LayoutByStaticSize();
        }
       
        public void ChangedClient2Square() {
            Rectangle rect=this.ClientRectangle;

            int rect_wh = Math.Min(rect.Width,rect.Height);
            this.SetClientSizeCore(rect_wh,rect_wh);
           
        }
        private void FormCircleItemSelect_Load(object sender, EventArgs e)
        {
            this.SetClientSizeCore(WIDTH,HEIGHT);
            this.ChangedClient2Square();
          
        }
        public void UpdataCenterPoint()
        {
            this.ChangedClient2Square();
            this.mCenter.X = ClientSize.Width / 2;
            this.mCenter.Y = ClientSize.Height / 2;
            this.mCircleOut = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            int margin_size = (int)(this.ClientSize.Height * 0.2/2);
            this.mCircleIn = new Rectangle(
                margin_size,
                margin_size,
                ClientSize.Width-2*margin_size,
                ClientSize.Height-2*margin_size);
            int margin_size_center = (int)(this.ClientSize.Height * (1 - mCircleRadius_ARC) / 2);
            this.mCircleCenter = new Rectangle(
                margin_size_center,
                margin_size_center,
                ClientSize.Width - 2 * margin_size_center,
                ClientSize.Height - 2 * margin_size_center);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.UpdataCenterPoint();
#if (SHOW_TEST_CIRCLE)
            e.Graphics.Clear(Color.White);
            this.DrawBgOnGraphics(e);
            this.DrawViewOnCanvas_test2(e);
#endif
#if false
            this.DrawBgOnGraphics(e);
           // this.DrawViewOnCanvas_test2(e);
#else
            this.SetBackGroundImage(e);
#endif
            
        }
        public void DrawBgOnGraphics(PaintEventArgs e)
        {

#if true
            Color Color_Smoke = Color.FromArgb(251, 251, 251);
            Graphics grfx = e.Graphics;
            grfx.SmoothingMode = SmoothingMode.AntiAlias;
            int penSize = (int)(this.ClientSize.Height*mCircleLength);
            Pen pen = new Pen(Color_Smoke, penSize);

            float delta = (float)3.0;
            float Step_angle =(float)( 360.0 / this.mChildNum);

            for (int i = 0; i < this.mChildNum; i++)
            {
                float Start = i * Step_angle-delta;

                if (i == this.mChildSelectedIdx)
                {
                    Pen pen_selected = new Pen(Color_Smoke, penSize);
                    grfx.DrawArc(pen_selected, this.mCircleCenter, Start, Step_angle+delta);
                    continue;
                } else {
                    grfx.DrawArc(pen, this.mCircleCenter, Start, Step_angle+delta);
                }


            }
#endif
#if false
            int margin_size_center = (int)(this.ClientSize.Height * (1 - mCircleLengthIn) / 2);
            Rectangle  CenterIn = new Rectangle(
                margin_size_center,
                margin_size_center,
                ClientSize.Width - 2 * margin_size_center,
                ClientSize.Height - 2 * margin_size_center);
            SolidBrush Brush_t = new SolidBrush(Color_Smoke);
            grfx.FillEllipse(Brush_t, CenterIn);
#endif
        }
        public void DrawViewOnCanvas_test(PaintEventArgs e) 
        {
            Graphics grfx = e.Graphics;

            grfx.Clear(Color.White);
            Pen pen = new Pen(Color.Red);
            grfx.DrawEllipse(pen, this.mCircleOut);
            Pen pen2 = new Pen(Color.Blue);
            grfx.DrawEllipse(pen2, this.mCircleIn);
            //grfx.DrawString("Hello World !!", this.Font, Brushes.Red, 0, 0);
            //
           // 绘制圆弧
            this.DrawArcViewOnClientView(e);

          //  grfx.DrawLine(new Pen(Color.Magenta),this.mCenter,this.mMousePoint);

        }
        public void DrawViewOnCanvas_test2(PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;

            Pen pen = new Pen(Color.Magenta,3);
            grfx.DrawEllipse(pen, this.mCircleCenter);
           

        }
        public void DrawArcViewOnClientView(PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;

            Pen pen=new Pen(Color.Cyan,10);
            float Step_angle = 360 / this.mChildNum;
            for (int i = 0; i < this.mChildNum; i++){
                float Start = i * Step_angle;
             

                if (i == this.mChildSelectedIdx)
                {
                    Pen pen_selected = new Pen(Color.Blue, 10);
                    grfx.DrawArc(pen_selected, this.mCircleIn, Start,Step_angle);
                    continue;
                }else {
                    grfx.DrawArc(pen, this.mCircleIn, Start,Step_angle);
                }
          

            }

            //grfx.DrawArc(pen, this.mCircleIn, 0, 90);
         
        }


        /*
        private void FormCircleItemSelect_Resize(object sender, EventArgs e)
        {
            

            if ((this.ClientSize.Width==this.mOldClientSize.Width)
                &&(this.ClientSize.Height==this.mOldClientSize.Height)) {
                    return;

              //尺寸没有变化
            }else{

                if(this.ClientSize.Width==WIDTH&&this.ClientSize.Height==HEIGHT){
                    //this.LayoutByStaticSize();
                }
        
            
            }

            this.mOldClientSize = this.ClientSize;

        }*/
        private void LayoutByStaticSize()
        {
                        this.UpdataCenterPoint();
                        this.InitButttonLayout();
                        this.SetFormPosition();
        }
        private void SetBackGroundImage(PaintEventArgs e)
        {
            if (this.ClientSize.Width == WIDTH && this.ClientSize.Height == HEIGHT) {
                    double Scale = mCircleLength + mCircleRadius_ARC+0.03;
                    int margin_size_center = (int)(WIDTH*(1-Scale) / 2);
                    Rectangle DrawRect=new Rectangle(
                        margin_size_center,
                        margin_size_center,
                        ClientSize.Width - 2 * margin_size_center,
                        ClientSize.Height - 2 * margin_size_center);

                    e.Graphics.DrawImage(global::CloudPrintforPC.Properties.Resources.circle_pannel, DrawRect);
            }
          
        
        }
        private void FormCircleItemSelect_MouseEnter(object sender, EventArgs e)
        {
            
            this.mSelectedMode = true;
        }

        private void FormCircleItemSelect_MouseLeave(object sender, EventArgs e)
        {
            Point point=new Point();
            point.Y = Cursor.Position.Y - this.Location.Y;
            point.X = Cursor.Position.X - this.Location.X;

            if (point.X < 0 || point.Y < 0 || point.X > this.Width || point.Y >= this.Height) { 
                   this.mSelectedMode = false;
                   this.mChildSelectedIdx = -1;
                   this.Invalidate();
            
            }

           
        }

        private void  FormCircleItemSelect_MouseMove(object sender, MouseEventArgs e)
        {
            int ChildSelectIdx_new=0;
            if(this.mSelectedMode){
                this.mMousePoint = new Point(e.X, e.Y);
                Point RelativeVector = new Point(mMousePoint.X - this.mCenter.X, mMousePoint.Y - this.mCenter.Y);
              //  double angle = Math.Atan2(RelativeVector.X, RelativeVector.Y)+Math.PI;
                double angle = Math.Atan2( RelativeVector.Y,RelativeVector.X);
              //  angle += Math.PI;
                double angleDegree = angle * 180 / Math.PI;
                
                double stepDegree = 360 / this.mChildNum;
                if (angleDegree >= 0){

                    ChildSelectIdx_new= (int)(angleDegree / stepDegree);
                    Debug.WriteLine("角度：{0}", angleDegree);

                }else {

                    angleDegree = angleDegree + 360;
                    ChildSelectIdx_new = (int)(angleDegree / stepDegree);
                    Debug.WriteLine("角度：{0}", angleDegree);

                }
            }
            if(this.mChildSelectedIdx!=ChildSelectIdx_new)
            {
                this.mChildSelectedIdx=ChildSelectIdx_new;
                this.Invalidate();
            }

        }
        public  void  InitButttonLayout() 
        {
            if (this.mChildButton != null)
            {
                this.SuspendLayout();
                int ClientWidth = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
                int ButtonWidth = (int)(ClientWidth * mButtonCircleSize);
                int ButtonCenterWidth = (int)(ClientWidth * mButtonCenterSize);
                float Step_angle = (float)(360.0 / this.mChildNum);
                float Radius = mCircleRadius_ARC * ClientWidth / 2;

                this.mCenterButton.Location=new Point(this.mCenter.X-ButtonCenterWidth/2,this.mCenter.Y-ButtonCenterWidth / 2);
                this.mCenterButton.Width=ButtonCenterWidth;
                this.mCenterButton.Height = ButtonCenterWidth;
                this.mCenterButton.Click+=new EventHandler(mCenterButton_Click);
                this.mCenterButton.SetImage(global::CloudPrintforPC.Properties.Resources.button_neg, global::CloudPrintforPC.Properties.Resources.button_neg);

                for (int i = 0; i <this.mChildNum; i++)
                {

                    this.mChildButton[i].Width = ButtonWidth;
                    this.mChildButton[i].Height = ButtonWidth;
                    Point location = new Point();

                    float ArcStart = i * Step_angle;
                    float ArcEnd = (i + 1) * Step_angle;
                    float ButtonAngle = (ArcStart + ArcEnd) / 2;
                    ButtonAngle=(float)(ButtonAngle*Math.PI/180);

                    //location.X = this.mCenter.X + (int)(Radius*Math.Sin((double)ButtonAngle)) - ButtonWidth;
                    //location.Y = this.mCenter.Y + (int)(Radius*Math.Cos((double)ButtonAngle)) - ButtonWidth;
#if true
                    location.X = this.mCenter.X + (int)(Radius * Math.Cos((double)ButtonAngle))-ButtonWidth/2;
                    location.Y = this.mCenter.Y + (int)(Radius * Math.Sin((double)ButtonAngle))-ButtonWidth/2;
#else
                    location.X = this.mCenter.X + (int)(Radius * Math.Cos((double)ButtonAngle));
                    location.Y = this.mCenter.Y + (int)(Radius * Math.Sin((double)ButtonAngle));
#endif
                    this.mChildButton[i].Location = location;

                   

                }
                
                this.ResumeLayout();
            }
        }
        public  void  AddButton2Main() 
        {

            this.SuspendLayout();
            List<Control> child = new List<Control>();
            for (int i = 0; i < this.mChildNum; i++)
            {
#if false
                ButtonCircule pbc = new ButtonCircule();
#else
                PictureBoxCircule pbc = new PictureBoxCircule();         
#endif
#if false
                pbc.SetImage(mImage_Selected[i],mImage_UnSelected[i]);
#else
                pbc.SetImage(mImage[i], mImage[i]);
#endif
                child.Add(pbc);


            }

            this.mChildButton = child.ToArray();

              
         

            float Step_angle = (float)(360.0 / this.mChildNum);

            for (int i = 0; i < this.mChildNum; i++)
            {

                float Start = i * Step_angle ;
             //   this.toolTip1.SetToolTip(this.mChildButton[i], ButtonTip[i]);
#if true
              
#else
                this.mChildButton[i].Text = i.ToString();
                this.mChildButton[i].BackColor = Color.Red;
#endif       
                this.Controls.Add(this.mChildButton[i]);
                
            }
            this.mChildButton[0].Click += new EventHandler(this.mButton_CloudPrint_Click);
            this.mChildButton[1].Click +=new EventHandler(this.mButton_PhoneManager_Click);
            this.mChildButton[2].Click +=new EventHandler(this.mButton_Statistics_Click);
            this.mChildButton[3].Click += new EventHandler(this.mButton_Exit_Click);
            this.mChildButton[4].Click += new EventHandler(this.mButton_FileTransfer_Click);
            this.mChildButton[5].Click += new EventHandler(this.mButton_NetPrint_Click);

            this.Controls.Add(this.mCenterButton);
            this.ResumeLayout();
           
        }
        private void  mButton_NetPrint_Click(object sender, EventArgs e)
        {
            FormNetPrint form = new FormNetPrint();
            form.mServer = this.mServer;
            form.Show();
        }
        private void  mButton_PhoneManager_Click(object sender, EventArgs e)
        {
            FormPhoneManager form = new FormPhoneManager();
            form.mServer = this.mServer;
            form.Show();
        }
        private void  mButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void mCenterButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void  mButton_FileTransfer_Click(object sender, EventArgs e)
        {
            FileTransferForm form = new FileTransferForm();
            form.SetServer(this.mServer);
            form.Show();
        }
      
        
        private void  mButton_Statistics_Click(object sender, EventArgs e)
        {
            FormStatistics form = new FormStatistics();
            form.mServer = this.mServer;
            form.Show();
        }
        private void  mButton_CloudPrint_Click(object sender, EventArgs e)
        {
#if false
            FormCloudPrint form = new FormCloudPrint();
            form.mServer = this.mServer;
            form.Show();
#else
            //调用系统默认的浏览器 
            String url_str = ConfigurationManager.AppSettings["URL_CLOUD_PRINT_SHOP"];
            try {
                System.Diagnostics.Process.Start(url_str);
            }catch(Exception ex){
                LogHelper.WriteLog(this.GetType(), ex);
            }
         
#endif
        }
        private void  FormCircleItemSelect_Layout(object sender, LayoutEventArgs e)
        {
           
        }
        private void  SetFormPosition() 
        {
            if (this.StartPosition == FormStartPosition.Manual) {
                this.Location = new Point(this.mLocationCenter.X - this.Width / 2, this.mLocationCenter.Y - this.Height / 2);
            }
        }

        private  void timerHideView_Tick(object sender, EventArgs e)
        {
          
            this.Visible = true;
            this.Opacity = 0.9;
            if (sender.GetType() == typeof(Timer)) {
                Timer t = (Timer)sender;
                t.Stop();
            }
        }

        private void FormCircleItemSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11) {
                this.TransparencyKey = this.BackColor;
            }
            if (e.KeyCode == Keys.F12)
            {
                this.TransparencyKey = Color.Blue;
            }
        }
      
    }
}
