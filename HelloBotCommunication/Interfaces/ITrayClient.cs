using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace HelloBotCommunication.Interfaces
{
    public interface ITrayClient : IClient
    {
        void UpdateTrayText(Guid token, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName="Tahoma", Color? iconBorderColor=null);
        void ShowTrayBalloonTip(Guid token, string text, TimeSpan? timeout = null, TooltipType? tooltipType = null);
    }
}
