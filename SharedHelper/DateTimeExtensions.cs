using System;

namespace SharedHelper
{
    public static class DateTimeExtensions
    {
        public static string Humanize(this TimeSpan ts)
        {
            string toReturn = "";
            if (ts.Days > 0) toReturn += ts.Days + " д. ";
            if (ts.Hours > 0) toReturn += ts.Hours + " ч. ";
            if (ts.Minutes > 0) toReturn += ts.Minutes + " мин. ";
            if (ts.Seconds > 0) toReturn += ts.Seconds + " сек.";
            return toReturn;
        }
    }
}
