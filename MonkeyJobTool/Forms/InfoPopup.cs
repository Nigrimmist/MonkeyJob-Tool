using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Forms
{
    public partial class InfoPopup : Form
    {
        public string Title { get; set; }
        public string Text { get; set; }

        public InfoPopup(string title, string text)
        {
            InitializeComponent();
            this.Text = text;
            this.Title = title;
        }

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Text);
        }

        private void InfoPopup_Load(object sender, EventArgs e)
        {
            txtMessage.Text = Text;
            lblTitle.Text = Title;
            using (Graphics g = CreateGraphics())
            {
                txtMessage.Height = (int)g.MeasureString(txtMessage.Text, txtMessage.Font, txtMessage.Width).Height+10;
            }

            this.Height = txtMessage.Height + 30;
        }

        private void InfoPopup_Click(object sender, EventArgs e)
        {
            
        }

        private void InfoPopup_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Close();
            }
        }

        private void txtMessage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.Close();
            }
        }

        private void InfoPopup_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void InfoPopup_MouseLeave(object sender, EventArgs e)
        {
            
        }
    }
}
