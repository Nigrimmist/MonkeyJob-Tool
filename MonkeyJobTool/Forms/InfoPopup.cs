using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Entities;

namespace MonkeyJobTool.Forms
{
    public partial class InfoPopup : Form
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public PopupType PopupType;
        private TimeSpan? _timeToClose;
        private readonly object _sessionData;
        private TimeSpan _timeToTimerEventFire;
        public bool AlreadyNotified { get; private set; }

        public delegate void OnPopupCloseDelegate(ClosePopupReasonType reason, object sessionData);
        public event OnPopupCloseDelegate OnPopupClosedBy;

        public delegate void OnPopupHidedDelegate();
        public event OnPopupHidedDelegate OnPopupHided;
        private string _closeHint = "*Правый клик - для закрытия";
        private string _closeHintTimeFormat = "*Правый клик - для закрытия ({0}с)";

        public InfoPopup(PopupType popupType,string title, string text, TimeSpan? displayTime, object sessionData = null)
        {
            InitializeComponent();
            this.Text = text;
            this.Title = title;
            PopupType = popupType;
            _timeToClose = displayTime;
            _sessionData = sessionData;
        }

        
        private void InfoPopup_Load(object sender, EventArgs e)
        {
            int pnlMainWidth = this.Width; //minus borders
            pnlMain.Width = pnlMainWidth;
            txtMessage.Text = Text;
            rtTitle.Text = Title;
            txtMessage.Top = rtTitle.Height + 15; //padding
            this.Height = txtMessage.Height + 55 + rtTitle.Height;
            pnlMain.Height = this.Height;
            //pnlTimeRemain.Top = pnlMain.Height - pnlTimeRemain.Height;

            foreach (Control control in Controls)
            {
                control.Click += InfoPopup_Click;
                control.MouseUp += InfoPopup_MouseUp;
                control.Cursor = Cursors.Hand;
            }

            //pnlTimeRemain.Width = pnlMainWidth;
            pnlHeader.Width = pnlMainWidth-2; //minus border
            pnlHeader.Height = rtTitle.Height + 8; //+margin
            lblCloseHint.Text = _closeHint;
            pnlCloseHint.Top = pnlMain.Height - /*pnlTimeRemain.Height -*/ pnlCloseHint.Height-1;
            rtTitle.BackColor = pnlHeader.BackColor = Color.SkyBlue;

            
            if (_timeToClose.HasValue)
            {
                _timeToTimerEventFire = _timeToClose.Value;
                closeTimer.Start();
            }
        }

        private void InfoPopup_Click(object sender, EventArgs e)
        {
            this.Close();
            if (OnPopupClosedBy != null)
            {
                OnPopupClosedBy(ClosePopupReasonType.LeftClick, _sessionData);
            }
        }

        private void InfoPopup_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Close();
                if (OnPopupClosedBy != null)
                {
                    OnPopupClosedBy(ClosePopupReasonType.RightClick, _sessionData);
                }
            }
        }
        
        private void closeTimer_Tick(object sender, EventArgs e)
        {
            if (_timeToTimerEventFire.TotalSeconds <= 0)
            {
                lblCloseHint.Text = _closeHint;
                closeTimer.Stop();
                AlreadyNotified = true;
                if (PopupType != PopupType.Notification)
                {
                    this.Close();
                    if (OnPopupClosedBy != null)
                    {
                        OnPopupClosedBy(ClosePopupReasonType.Auto, _sessionData);
                    }
                }
                else
                {
                    this.Hide();
                    if (OnPopupHided != null)
                        OnPopupHided();
                }

            }
            else
            {
                lblCloseHint.Text = string.Format(_closeHintTimeFormat, _timeToTimerEventFire.TotalSeconds);
            }
            _timeToTimerEventFire = _timeToTimerEventFire.Subtract(TimeSpan.FromSeconds(1));
            
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private void InfoPopup_Paint(object sender, PaintEventArgs e)
        {
            //ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }


        private void IconPic_Click(object sender, EventArgs e)
        {

        }

        private void lblCloseHint_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rtTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            int borderSize = 1; 
            int halfThickness = borderSize / 2;
            using (Pen p = new Pen(Color.Black, borderSize))
            {
                e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                    halfThickness,
                    pnlMain.ClientSize.Width - borderSize,
                    pnlMain.ClientSize.Height - borderSize));
            }
        }
    }


}
