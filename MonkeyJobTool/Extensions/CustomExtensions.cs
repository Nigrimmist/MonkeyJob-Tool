using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore.Entities;

namespace MonkeyJobTool.Extensions
{
    public static class CustomExtensions
    {
        public static ToolTipIcon ToTooltipType(this TooltipType type)
        {
            switch (type)
            {
                case TooltipType.Error:
                    return ToolTipIcon.Error;
                case TooltipType.Info:
                    return ToolTipIcon.Info;
                case TooltipType.Warning:
                    return ToolTipIcon.Warning;
                case TooltipType.None:
                    return ToolTipIcon.None;
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }
    }
}
