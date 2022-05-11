using System;

namespace UniCEC.Data.Common
{
    public class LocalTime
    {
        public LocalTime()
        {
        }

        public DateTimeOffset GetLocalTime()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset localTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            return localTime;
        }
    }
}
