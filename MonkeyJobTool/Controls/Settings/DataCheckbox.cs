using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Controls.Settings
{
    public class DataCheckBox : CheckBox, IUIDataItem
    {
        public string PropName { get; set; }
        public int DeepLevel { get; set; }
        public int? CollectionIndex { get; set; }
    }
}
