using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCommunication.Interfaces
{
    public interface IBotCallback
    {
        /// <summary>
        /// Specify that callback for register Click callback. It will be fired when user click to popup.
        /// </summary>
        /// <param name="onClickCallback"></param>
        /// <returns></returns>
        IBotCallback OnClick(Action onClickCallback);

        /// <summary>
        /// Specify that callback for register Notified callback. It will be fired when user was notified about event (close popup for example)
        /// </summary>
        /// <param name="onNotifiedCallback"></param>
        /// <returns></returns>
        IBotCallback OnClosed(Action onNotifiedCallback);
    }
}
