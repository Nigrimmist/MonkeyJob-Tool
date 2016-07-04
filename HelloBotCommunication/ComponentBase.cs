using System.Drawing;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class ComponentBase
    {
        /// <summary>
        /// Please, override it if any new functionality provided by your code
        /// </summary>
        public virtual double Version
        {
            get { return 1.0; }
        }


        /// <summary>
        /// Please, setup it to current module version if your settings are incompatible with previous version.
        /// </summary>
        public virtual double ActualSettingsVersion
        {
            get { return 1.0; }
        }


        /// <summary>
        /// Will be displayed in UI title. If null, title will be retrieved from command, otherwise from  module assembly
        /// </summary>
        public virtual string Title { get { return null; } }

        /// <summary>
        /// Your icon in base64 string. Should be 26x26 px. Can be in png format (transparency supported). Please, remove from result string "data:image/x-icon;base64," or similiar if exist. You can use http://xaviesteve.com//pro/base64.php or similiar to convert your icon to base64.
        /// </summary>
        public virtual string IconInBase64{get { return null; }}
       
        
    }
}
