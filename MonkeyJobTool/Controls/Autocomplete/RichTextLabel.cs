﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
            //this.Height = e.NewRectangle.Height + 1;

            //int screenHeight = Screen.FromPoint(this.Location).WorkingArea.Height;
            //if (this.Height > screenHeight / 2)
            //{
            //    this.Height = screenHeight / 2;
            //}
        }

        
    }



    public class Win32
    {
        // offset of window style value
        public const int GWL_STYLE = -16;

        // window style constants for scrollbars
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_HSCROLL = 0x00100000;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    }
}
