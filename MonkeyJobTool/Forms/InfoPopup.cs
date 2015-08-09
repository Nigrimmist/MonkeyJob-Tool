using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Entities;
using MonkeyJobTool.Helpers;

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
        public bool AlreadyNotified { get; set; }

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

        //n->0
        private Point FindOptimalWidthHeight(int maxHeight, int minWidth, string text, Font textFont, int heightTolerance, float currWidth, float? prevLeftBorder = null, float? prevRightBorder = null)
        {

            var textSize = this.CreateGraphics().MeasureString(text, textFont, new SizeF(currWidth, Int32.MaxValue));
            currWidth = textSize.Width;
            prevLeftBorder = prevLeftBorder ?? currWidth;
            prevRightBorder = prevRightBorder ?? 0;
            if (maxHeight - heightTolerance <= textSize.Height && maxHeight + heightTolerance >= textSize.Height || (currWidth < minWidth && textSize.Height <= maxHeight))
            {
                if (currWidth < minWidth)
                {
                    currWidth = minWidth;
                }
                textSize = this.CreateGraphics().MeasureString(text, textFont, new SizeF(currWidth, Int32.MaxValue));
                return new Point((int)currWidth, (int)textSize.Height);
            }

            if (textSize.Height < maxHeight)
            {
                prevLeftBorder = currWidth;
                currWidth = (currWidth / 2);
            }
            else
            {
                prevRightBorder = currWidth;
                currWidth += (prevLeftBorder.Value - currWidth) / 2;
            }

            return FindOptimalWidthHeight(maxHeight, minWidth, text, textFont, heightTolerance, currWidth, prevLeftBorder, prevRightBorder);
        }

        private void InfoPopup_Load(object sender, EventArgs e)
        {

            txtMessage.Visible = false;
            if (Icon!=null)
                IconPic.BackgroundImage = Icon;
            int textInitialWidth = txtMessage.Width;
            txtMessage.Text = Text;
            
            int screenHeight = Screen.FromPoint(this.Location).WorkingArea.Height;
            int screenWidth = Screen.FromPoint(this.Location).WorkingArea.Width;

            //find free size of text
            var textSize = this.CreateGraphics().MeasureString(Text, txtMessage.Font, new SizeF(Int32.MaxValue,Int32.MaxValue));
            int height = (int)textSize.Height;
            int width = (int)textSize.Width;
            
            //calculating text height/width with height as priority.
            if (textSize.Height <= screenHeight / 2) //wide text
            {
                if (textSize.Width > textInitialWidth)
                {
                    var optimalPoint = FindOptimalWidthHeight(screenHeight / 2, textInitialWidth, Text, txtMessage.Font, 50, textSize.Width);
                    height = optimalPoint.Y;
                    width = optimalPoint.X;
                }
                else
                {
                    height = (int)textSize.Height;
                    width = textInitialWidth;
                }
            }
            else //tall text
            {
                if (textSize.Width > textInitialWidth)
                {
                    textSize = this.CreateGraphics().MeasureString(Text, txtMessage.Font, new SizeF(Int32.MaxValue, screenHeight / 2));
                    height = (int)textSize.Height;
                    width = (int)textSize.Width;

                    //too big text, scrollbar will be shown
                    if (width > screenWidth / 2) width = screenWidth / 2;
                }
                else
                {
                    height = (int)textSize.Height;
                    width = textInitialWidth;
                }
            }

            int rightMargin = 20;
            this.Width = width + rightMargin;
            txtMessage.Width = width;
            txtMessage.Height = height;

            //remove scrollbar fix
            int realScrollBarHeightMargin = 16; //magic people woodoo people!
            var realHeight  = txtMessage.GetPositionFromCharIndex(txtMessage.Text.Length).Y;
            if (realHeight + realScrollBarHeightMargin >= height)
            {
                var pr = (realHeight + realScrollBarHeightMargin) / (double)height;
                if (pr < 1.3F) //if real is more than 30% - convert scroll to height
                {
                    txtMessage.Height = realHeight +16;
                }
            }
            if (txtMessage.Width > screenWidth/2)
            {
                txtMessage.Width = screenWidth/2;
                this.Width = txtMessage.Width + rightMargin;
            }
            
            int pnlMainWidth = this.Width; 
            pnlMain.Width = pnlMainWidth;
            
            rtTitle.Text = Title;
            txtMessage.Top = rtTitle.Height + 15; 
            this.Height = txtMessage.Height + 55 + rtTitle.Height;
            pnlMain.Height = this.Height;
            foreach (var c in FormHelper.IterateControls(Controls))
            {
                c.MouseUp += InfoPopup_MouseUp;
            }
            pnlHeader.Width = pnlMainWidth-2; 
            pnlHeader.Height = rtTitle.Height + 8;
            lblCloseHint.Text = _closeHint;
            pnlCloseHint.Top = pnlMain.Height - pnlCloseHint.Height-1;
            pnlCloseHint.Left = this.Width - pnlCloseHint.Width-10;
            rtTitle.BackColor = pnlHeader.BackColor = TitleColor;
            this.BackColor = txtMessage.BackColor=  BodyColor;

            if (_timeToClose.HasValue)
            {
                _timeToTimerEventFire = _timeToClose.Value;
                closeTimer.Start();
            }
            else
            {
                AlreadyNotified = true;
            }
            txtMessage.Visible = true;
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
                e.Graphics.DrawLine(p, 0, 0, 0, pnlMain.ClientSize.Height);
                e.Graphics.DrawLine(p, 0, 0, pnlMain.ClientSize.Width, 0);
                e.Graphics.DrawLine(p, pnlMain.ClientSize.Width - borderSize, 0, pnlMain.ClientSize.Width - borderSize, pnlMain.ClientSize.Height - borderSize);
                e.Graphics.DrawLine(p, pnlMain.ClientSize.Width - borderSize, pnlMain.ClientSize.Height - borderSize, 0, pnlMain.ClientSize.Height - borderSize);
            }
        }

        private void InfoPopup_Deactivate(object sender, EventArgs e)
        {
            if (!App.ApplicationIsActivated())
            {
                this.Hide();
            }
        }
        
    }


}
