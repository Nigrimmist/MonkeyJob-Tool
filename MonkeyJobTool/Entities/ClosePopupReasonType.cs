using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCore.Entities;

namespace MonkeyJobTool.Entities
{
    public enum ClosePopupReasonType
    {
        //don't change places
        Auto = 1,
        LeftClick,
        RightClick,
        Manually
    }

    public static class ClosePopupReasonTypeExtensions
    {
        public static UserReactionToCommandType ToUserReactonType(this ClosePopupReasonType t)
        {
            switch (t)
            {
                case ClosePopupReasonType.Auto:
                    return UserReactionToCommandType.HidedByTimeout;
                case ClosePopupReasonType.LeftClick:
                    return UserReactionToCommandType.Clicked;
                case ClosePopupReasonType.RightClick:
                case ClosePopupReasonType.Manually:
                    return UserReactionToCommandType.Closed;
                default:
                    throw new ArgumentOutOfRangeException("t", t, null);
            }
        }
    }
}
