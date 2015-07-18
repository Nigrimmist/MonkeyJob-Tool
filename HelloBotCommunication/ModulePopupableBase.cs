using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public class ModulePopupableBase : ModuleBase
    {
        /// <summary>
        /// Init method for retrieving client functionality if required. Will be called after constructor.
        /// </summary>
        /// <param name="client"></param>
        public virtual void Init(IClient client)
        {

        }

        /// <summary>
        /// Popup header background color
        /// </summary>
        public virtual Color? HeaderBackGroundColor { get { return null; } }

        /// <summary>
        /// Popup body background color
        /// </summary>
        public virtual Color? BodyBackgroundColor { get { return null; } }
    }
}
