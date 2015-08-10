using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public class RichTextLabelResizeable : RichTextLabel
    {
        public RichTextLabelResizeable()
        {
            base.ContentsResized += cntrl_ContentsResized;
        }
        
        //richtextlabel resize hack
        private void cntrl_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            this.Height = e.NewRectangle.Height + 1;

            int screenHeight = Screen.FromPoint(this.Location).WorkingArea.Height;
            if (this.Height > screenHeight / 2)
            {
                this.Height = screenHeight / 2;
            }
        }
    }
}
