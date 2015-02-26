using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonkeyJobTool.Extensions
{
    public static class EnumExtensions
    {
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
        {
            var retValue = value != null && Enum.IsDefined(typeof(TEnum), value);
            result = retValue ? (TEnum)Enum.Parse(typeof(TEnum), value) : default(TEnum);
            return retValue;
        }
    }
}
