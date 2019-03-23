using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazonspider.Core
{
    public static class Ex
    {
        public static long GetTimestamp(this DateTime dateTime)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long time = (long)(dateTime - startTime).TotalSeconds;
            return time;
        }
    }
}
