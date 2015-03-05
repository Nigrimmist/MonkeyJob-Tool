using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Settings
{
    public partial class CommandReplaceBlock : UserControl
    {
        public delegate void OnOneOfFieldFilledDelegate();
        public event OnOneOfFieldFilledDelegate OnOneOfFieldFilled;
        public event OnOneOfFieldFilledDelegate OnBothFieldsEmpty;

        public string From
        {
            get { return txtFrom.Text; }
            set { txtFrom.Text = value; }
        }

        public string To
        {
            get { return txtTo.Text; }
            set { txtTo.Text = value; }
        }

        public CommandReplaceBlock(string from, string to)
        {
            From = from;
            To = to;
        }

        public CommandReplaceBlock()
        {
            InitializeComponent();
        }

        private void txtFrom_TextChanged(object sender, EventArgs e)
        {
            CheckEvents();
        }

        private void txtTo_TextChanged(object sender, EventArgs e)
        {
            CheckEvents();
        }

        private void CheckEvents()
        {
            if (string.IsNullOrEmpty(txtTo.Text) && string.IsNullOrEmpty(txtFrom.Text))
            {
                if (OnBothFieldsEmpty != null)
                    OnBothFieldsEmpty();
            }
            else if (!string.IsNullOrEmpty(txtTo.Text) && !string.IsNullOrEmpty(txtFrom.Text))
            {
                if (OnOneOfFieldFilled != null)
                    OnOneOfFieldFilled();
            }
        }
    }
}
