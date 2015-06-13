using System.Drawing;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class ModuleBase
    {
        /// <summary>
        /// Using for settings version tracking.
        /// </summary>
        public virtual double ModuleVersion
        {
            get { return 1.0; }
        }

        /// <summary>
        /// Init method for retrieving client functionality if required. Will be called after constructor.
        /// </summary>
        /// <param name="client"></param>
        public virtual void Init(IClient client)
        {

        }
        
        /// <summary>
        /// Will be displayed in UI title. If null, title will be retrieved from command, otherwise from  module assembly
        /// </summary>
        public virtual string ModuleTitle { get { return null; } }

        /// <summary>
        /// Your icon in base64 string. Should be 26x26 px. Can be in png format (transparency supported). Please, remove from result string "data:image/x-icon;base64," if exist.
        /// </summary>
        public virtual string IconInBase64{get { return null; }}
       
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
