using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Forms
{
    public partial class DonateForm : Form
    {
        public DonateForm()
        {
            InitializeComponent();
        }

        private void DonateRequisitesForm_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.heartIco;
        }

        private void DonateRequisitesForm_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void paypalBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=JM6UXF2UXWFYN");
            MessageBox.Show("Спасибо, дружище");
        }

        
    }
}
