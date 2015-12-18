using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Helpers
{
    public class MultithreadHelper
    {
        public static void ThreadSafeCall(Control controlToInvoke, Action func)
        {
            if (controlToInvoke.InvokeRequired)
            {
                controlToInvoke.Invoke(new MethodInvoker(func));
            }
            else
                func();
        }
    }
}
