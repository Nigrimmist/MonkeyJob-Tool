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
        /// Your command description
        /// </summary>
        public virtual string ModuleDescription
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Will be displayed in UI title. If null, title will be retrieved from command, otherwise from  module assembly
        /// </summary>
        public virtual string ModuleTitle { get { return null; } }
    }
}
