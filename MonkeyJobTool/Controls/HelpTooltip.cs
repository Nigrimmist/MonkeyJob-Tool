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
        public string HelpText
        {
            set
            {
                HelpPopupManager.BindHelpForm(picBox, value);
            }
        }

        public HelpTooltip()
        {
            InitializeComponent();
        }
    }
}
