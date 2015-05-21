using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Managers
{
    public class LogManager
    {
        public static void Error(Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
}
