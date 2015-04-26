using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonkeyJobTool.Managers;

namespace MonkeyJobTool.Controls
{
    public partial class HelpTooltip : UserControl
    {
        public string HelpText { get; set; }

        public HelpTooltip()
        {
            InitializeComponent();
        }

        private void picBox_Click(object sender, EventArgs e)
        {

        }

        private void HelpTooltip_Load(object sender, EventArgs e)
        {
            HelpPopupManager.BindHelpForm(picBox, HelpText);
        }
    }
}
