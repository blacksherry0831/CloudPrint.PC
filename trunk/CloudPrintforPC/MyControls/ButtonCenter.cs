using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CloudPrintforPC
{
    class ButtonCenter  :Button
    {
        Image Selected = null;
        public ButtonCenter() : base()
        {
            this.BackColor = System.Drawing.Color.Transparent;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SetImage(global::CloudPrintforPC.Properties.Resources.pointer);
        }
        public void SetImage(Image slelcted) 
        {
            this.Selected = slelcted;
            this.BackgroundImage =this.Selected;
        }
        public void Set(float Direction) 
        {
          
        }

    }
}
