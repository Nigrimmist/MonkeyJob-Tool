using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Settings
{
    public class DataButton : Button
    {
        public object Data { get; set; }
        public int RowIndex { get; set; }
        public TableLayoutPanel ParentPanel { get; set; }
    }
}
