using System;

namespace UserApp.Core
{
    public static class DateTimeUtility
    {
        public static DateTime ParseUnixTimestamp(double unixTimeStamp)
        {
            var epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return epochDate.AddSeconds(unixTimeStamp).ToLocalTime();
        }
    }
}
