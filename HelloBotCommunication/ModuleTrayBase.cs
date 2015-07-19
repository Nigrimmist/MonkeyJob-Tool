using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class ModuleTrayBase : ModuleBase
    {

        /// <summary>
        /// Your OnFire method will be executed every "RunEvery" time + time, that will be elapsed for handle you OnFire method implementation.
        /// </summary>
        public abstract TimeSpan RunEvery { get; }

        /// <summary>
        /// You action handle implementation. Will be fired by timer using RunEvery timespan delay.
        /// </summary>
        /// <param name="trayModuleToken"></param>
        public abstract void OnFire(Guid trayModuleToken);

        /// <summary>
        /// Your tray icon in base64 string. Should be 16x16 px. Can be in png format (transparency supported). Please, remove from result string "data:image/x-icon;base64," or similiar if exist. You can use http://xaviesteve.com//pro/base64.php or similiar to convert your icon to base64.
        /// </summary>
        public abstract string TrayIconIn64Base { get; }

        /// <summary>
        /// Init method for retrieving client functionality if required. Will be called after constructor.
        /// </summary>
        /// <param name="client"></param>
        public virtual void Init(ITrayClient client)
        {

        }

        /// <summary>
        /// General tray module description
        /// </summary>
        public virtual string ModuleDescription { get { return null; } }
        
    }
}
