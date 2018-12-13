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
namespace CloudPrintforPC
{
    class PictureBoxCircule :PictureBox
    {
        Image Selected = null;
        Image UnSelected = null;
        private Point mOldLocation;
        private Size mOldSize;
        private Point mCenter = new Point();
        private int mZindex;
        private Boolean mBig=true;
        public PictureBoxCircule() 
        {
#if true
            this.BackColor = Color.Transparent;
#endif
        }

        public PictureBoxCircule(Boolean flag)
        {
            if (flag == false) {
#if true
                this.BackColor = Color.Transparent;
#endif
                this.mBig = false;

            }
         
        }
        public  void SetImage(Image slelcted,Image unSelected) 
        {
            this.SuspendLayout();
            this.Selected = slelcted;
            this.UnSelected = unSelected;
#if false
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Image = this.UnSelected;
#endif   
            
            this.ResumeLayout();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

                  //this.BackgroundImage = this.Selected;
            if(this.mBig)
            {
#if false
                     if (this.Selected != this.Image)
                    {
                        this.Image = this.Selected;
                    }
#endif
                this.mOldLocation = this.Location;
                    this.mOldSize = this.Size;
                    //this.BringToFront();
                    this.SendToBack();
                    this.WhenMoveInChangeView();
            
            }

           
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //this.BackgroundImage = this.UnSelected;
            if (this.mBig) {
#if false
                if (this.UnSelected != this.Image)
                {
                    this.Image = this.UnSelected;
                }
#endif
                this.Location = this.mOldLocation;
                this.Size = this.mOldSize;
                this.SendToBack();
            }
           
        }

        private void WhenMoveInChangeView()
        {
            double scale = 1.7;
            /*-----------------------------------------------*/
            int Width = mOldSize.Width;
            int Height = mOldSize.Height;
            /*-----------------------------------------------*/
            this.mCenter.X = this.Location.X + (Width) / 2;
            this.mCenter.Y = this.Location.Y + (Height) / 2;
            /*-----------------------------------------------*/
            this.Width = (int)(Width * scale);
            this.Height = (int)(Height * scale);
            /*-----------------------------------------------*/
            this.Location = new Point(this.mCenter.X - this.Width / 2, this.mCenter.Y - this.Height / 2);

            Debug.WriteLine("TabIndex" + this.TabIndex + "\n");

            

        }
#if true
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Graphics g = pevent.Graphics;
          //  g.Clear(Color.Transparent);
            g.DrawImage(this.Selected, new Rectangle(0, 0, this.Width, this.Height));
            //Brush brash = new SolidBrush(Color.DarkOrchid);

            //g.FillEllipse(brash, new Rectangle(0, 0, this.Width, this.Height));
        }
#endif

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
#if false
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
#else
                base.OnPaintBackground(pevent); // or base.OnPaint(pevent);...

             if (this.Parent != null)
            {
                
            }
            // pevent.Graphics.Clear(Color.Transparent);
            //Point Screen = new Point(Location.X, Location.Y);
            //pevent.Graphics.CopyFromScreen(this.PointToScreen(Screen).X, this.PointToScreen(Screen).Y, 0, 0, this.ClientSize);
#endif
        }


    }
}
