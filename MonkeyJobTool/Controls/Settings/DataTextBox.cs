using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Settings
{
    public class DataTextBox : TextBox, IUIDataItem
    {
        public string PropName { get; set; }
        public int DeepLevel { get; set; }
        public int? CollectionIndex { get; set; }
    }
}
