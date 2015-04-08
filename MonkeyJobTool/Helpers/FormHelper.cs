using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Helpers
{
    public class FormHelper
    {
        public static IEnumerable<Control> IterateControls(Control.ControlCollection coll)
        {
            foreach (Control c in coll)
            {
                yield return c;
                foreach (var c2 in IterateControls(c.Controls))
                {
                    yield return c2;
                }
            }
        }
    }
}
