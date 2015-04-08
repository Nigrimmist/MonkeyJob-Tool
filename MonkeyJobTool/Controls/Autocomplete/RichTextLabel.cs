using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Autocomplete
{
    public class RichTextLabel : RichTextBox
    {
        public RichTextLabel()
        {
            base.ReadOnly = true;
            base.BorderStyle = BorderStyle.None;
            base.TabStop = false;
            base.SetStyle(ControlStyles.Selectable, false);
            base.SetStyle(ControlStyles.UserMouse, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Cursor = Cursors.Hand;
            
            base.ContentsResized += cntrl_ContentsResized;
            
        }

        protected override void WndProc(ref Message m)
        {
            //if (m.Msg == 0x204) 
            //    return; // WM_RBUTTONDOWN
            //if (m.Msg == 0x205) 
            //    return; // WM_RBUTTONUP
            base.WndProc(ref m);
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
