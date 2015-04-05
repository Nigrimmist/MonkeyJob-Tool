using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
        public override sealed string Text { get; set; }
        public PopupType PopupType;
        private TimeSpan? _timeToClose;
        private readonly object _sessionData;
        private TimeSpan _timeToTimerEventFire;
        public bool AlreadyNotified { get; private set; }

        public delegate void OnPopupCloseDelegate(ClosePopupReasonType reason, object sessionData);
        public event OnPopupCloseDelegate OnPopupClosedBy;

        public delegate void OnPopupHidedDelegate();
        public event OnPopupHidedDelegate OnPopupHided;
        private readonly string _closeHint = "*Правый клик - для закрытия";
        private readonly string _closeHintTimeFormat = "*Правый клик - для закрытия ({0}с)";

        public new Image Icon { get; set; }
        public Color TitleColor = Color.SkyBlue;
        public Color BodyColor = Color.White;


        public InfoPopup(PopupType popupType, string title, string text, TimeSpan? displayTime, object sessionData = null, Image icon = null, Color? titleBackgroundColor = null, Color? bodyBackgroundColor = null)
        {
            InitializeComponent();
            this.Text = text;
            this.Title = title;
            PopupType = popupType;
            _timeToClose = displayTime;
            _sessionData = sessionData;
            Icon = icon;

            if (titleBackgroundColor.HasValue)
                TitleColor = titleBackgroundColor.Value;

            if (bodyBackgroundColor.HasValue)
                BodyColor = bodyBackgroundColor.Value;
        }
        
        
        private void InfoPopup_Load(object sender, EventArgs e)
        {
            if (Icon!=null)
                IconPic.BackgroundImage = Icon;

            int pnlMainWidth = this.Width; 
            pnlMain.Width = pnlMainWidth;
            txtMessage.Text = Text;
            rtTitle.Text = Title;
            txtMessage.Top = rtTitle.Height + 15; 
            this.Height = txtMessage.Height + 55 + rtTitle.Height;
            pnlMain.Height = this.Height;
            _initControlsRecursive(this.Controls);
            pnlHeader.Width = pnlMainWidth-2; 
            pnlHeader.Height = rtTitle.Height + 8;
            lblCloseHint.Text = _closeHint;
            pnlCloseHint.Top = pnlMain.Height - pnlCloseHint.Height-1;
            rtTitle.BackColor = pnlHeader.BackColor = TitleColor;
            this.BackColor = txtMessage.BackColor=  BodyColor;
            
            if (_timeToClose.HasValue)
            {
                _timeToTimerEventFire = _timeToClose.Value;
                closeTimer.Start();
            }
        }

        private void _initControlsRecursive(Control.ControlCollection coll)
        {
            foreach (Control c in coll)
            {
                c.MouseUp += InfoPopup_MouseUp;
                _initControlsRecursive(c.Controls);
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
            else if (e.Button == MouseButtons.Left)
            {
                this.Close();
                if (OnPopupClosedBy != null)
                {
                    OnPopupClosedBy(ClosePopupReasonType.LeftClick, _sessionData);
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


        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            int borderSize = 1;
            
            using (Pen p = new Pen(Color.Black, borderSize))
            {
                if (PopupType != PopupType.Fixed)
                {
                    e.Graphics.DrawLine(p, 0, 0, 0, pnlMain.ClientSize.Height);
                    e.Graphics.DrawLine(p, 0, 0, pnlMain.ClientSize.Width, 0);
                    e.Graphics.DrawLine(p, pnlMain.ClientSize.Width - borderSize, 0, pnlMain.ClientSize.Width - borderSize, pnlMain.ClientSize.Height - borderSize);

                    e.Graphics.DrawLine(p, pnlMain.ClientSize.Width - borderSize, pnlMain.ClientSize.Height - borderSize, 0, pnlMain.ClientSize.Height - borderSize);
                }
                //e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                //    halfThickness,
                //    pnlMain.ClientSize.Width - borderSize,
                //    pnlMain.ClientSize.Height - borderSize));
            }
        }
        
    }


}
