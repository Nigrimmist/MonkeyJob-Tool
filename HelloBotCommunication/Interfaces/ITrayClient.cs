using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace HelloBotCommunication.Interfaces
{
    public interface ITrayClient : IClient
    {
        void UpdateTrayText(Guid token,string text);
        void UpdateTrayColor(Guid token,Color color);
    }
}
