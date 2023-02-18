using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutinR.Services
{
    public class TimeSpanFormatter
    {
        public static string Format(TimeSpan timeSpan)
        {
            if (timeSpan.TotalSeconds < 60) return timeSpan.ToString("ss\\:ff");
            if (timeSpan.TotalMinutes < 60) return timeSpan.ToString("mm\\:ss");
            if (timeSpan.TotalHours < 24) return timeSpan.ToString("h\\:mm");

            return timeSpan.ToString("d\\:h\\:mm");
        }
    }
}
