using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPrintforPC
{
    class ButtonNoBorder : System.Windows.Forms.Button  
    {
        protected override bool ShowFocusCues
        {
            get
            {
                // 获得焦点的时候什么都不做  
                return false;
            }
        }  
    }
}
