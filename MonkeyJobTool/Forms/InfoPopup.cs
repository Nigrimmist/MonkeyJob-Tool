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
        private TimeSpan? _timeToClose;
        private int _timerStep;
        private int _initialFormWidth ;

        public InfoPopup(string title, string text, TimeSpan? displayTime)
        {
            InitializeComponent();
            this.Text = text;
            this.Title = title;
            _timeToClose = displayTime;
        }

        
        private void InfoPopup_Load(object sender, EventArgs e)
        {
            txtMessage.Text = Text;
            this.Height = txtMessage.Height+ 50;
            lblTitle.Text = Title;

            pnlTimeRemain.Top = this.Height - pnlTimeRemain.Height;
            foreach (Control control in Controls)
            {
                control.Click += InfoPopup_Click;
                control.MouseUp += InfoPopup_MouseUp;
                control.Cursor = Cursors.Hand;
            }
            pnlTimeRemain.Width = this.Width;
            if (_timeToClose.HasValue)
            {
                pnlTimeRemain.Width = 0;
                _timerStep = (int) (this.Width/(_timeToClose.Value.TotalMilliseconds/closeTimer.Interval));
                _initialFormWidth = this.Width;
                closeTimer.Start();
            }

            
        }

        private void InfoPopup_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Text);
            App.Instance.ShowPopup("Скопировано в буфер обмена", TimeSpan.FromSeconds(2));
        }

        private void InfoPopup_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Close();
            }
        }

        
        private void closeTimer_Tick(object sender, EventArgs e)
        {
            pnlTimeRemain.Width += _timerStep;
            if (pnlTimeRemain.Width >= _initialFormWidth)
            {
                closeTimer.Stop();
                this.Close();
            }
        }

        
                
    }


}
