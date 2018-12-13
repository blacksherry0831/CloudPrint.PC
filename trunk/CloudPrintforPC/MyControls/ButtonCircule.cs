using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
namespace CloudPrintforPC
{
    class ButtonCircule :Button
    {
        private Point  mOldLocation;
        private Size   mOldSize;
        private Point  mCenter=new Point();
        private int    mZindex;
        public ButtonCircule() : base()
        {
            this.SuspendLayout();
            // 
            // button1
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

       
            this.FlatAppearance.MouseOverBackColor = Color.Transparent;//鼠标经过  
            this.FlatAppearance.MouseDownBackColor = Color.Transparent;//鼠标按下
          
         
            this.UseVisualStyleBackColor = true;
           
            // 
            // testForm
            // 
            this.ResumeLayout(false);
        }
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            
           
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            ////this.BackColor = Color.Blue;
            //this.Image = this.Selected;
#if false
            if (this.BackgroundImage != this.Selected)
            {
                this.BackgroundImage = this.Selected;
            }
#endif
          //  this.mZindex = this.TabIndex;
            this.mOldLocation = this.Location;
            this.mOldSize = this.Size;
            this.BringToFront();
            this.WhenMoveInChangeView();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ////this.BackColor = Color.Red;
            //this.Image = this.UnSelected;
#if false
            if (this.BackgroundImage != this.UnSelected)
            {
                this.BackgroundImage =this.UnSelected;
            }
#endif
         //   this.TabIndex = this.mZindex;
            this.Location = this.mOldLocation;
            this.Size = this.mOldSize;
            this.SendToBack();
        }
        Image Selected = null;
        Image UnSelected = null;
        public void SetImage(Image slelcted, Image unSelected)
        {
            this.Selected = slelcted;
            this.UnSelected = unSelected;
#if false
            this.BackgroundImage = unSelected;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
#endif
        }
        private void WhenMoveInChangeView()
        {
            double scale =2;
            /*-----------------------------------------------*/
            int Width = mOldSize.Width;
            int Height = mOldSize.Height;
            /*-----------------------------------------------*/
            this.mCenter.X=this.Location.X+(Width)/2;
            this.mCenter.Y=this.Location.Y+(Height)/2;
            /*-----------------------------------------------*/
            this.Width = (int)(Width * scale);
            this.Height = (int)(Height * scale);
            /*-----------------------------------------------*/
            this.Location   =new Point(this.mCenter.X-this.Width/2, this.mCenter.Y-this.Height/2);

            Debug.WriteLine("TabIndex"+this.TabIndex+"\n");
         //   this.TabIndex = 0;
            
        
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.DrawRectangle(Pens.Black, this.ClientRectangle);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (this.Parent != null)
            {
                GraphicsContainer cstate = pevent.Graphics.BeginContainer();
                pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
                Rectangle clip = pevent.ClipRectangle;
                clip.Offset(this.Left, this.Top);
                PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, clip);

                //paint the container's bg
                InvokePaintBackground(this.Parent, pe);
                //paints the container fg
                InvokePaint(this.Parent, pe);
                //restores graphics to its original state
                pevent.Graphics.EndContainer(cstate);
            }
            else
                base.OnPaintBackground(pevent); // or base.OnPaint(pevent);...
        }


    }
}
