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
        /// Specify that callback for register on ignore event callback. It will be fired when user was notified, but popup has been closed by timeout.
        /// </summary>
        /// <param name="onIgnoreCallback"></param>
        /// <returns></returns>
        IBotCallback OnIgnore(Action onIgnoreCallback);

        /// <summary>
        /// Specify that callback for register Notified callback. It will be fired when user was notified and close popup manually.
        /// </summary>
        /// <param name="onNotifiedCallback"></param>
        /// <returns></returns>
        IBotCallback OnNotified(Action onNotifiedCallback);
    }
}
