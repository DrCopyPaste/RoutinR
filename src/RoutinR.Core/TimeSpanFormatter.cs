namespace RoutinR.Core
{
    public class TimeSpanFormatter
    {
        public static string Format(TimeSpan timeSpan)
        {
            if (timeSpan < TimeSpan.Zero) throw new ArgumentException("timespan is smaller than zero");

            if (timeSpan.TotalMinutes < 60) return String.Format("{0:D2}m:{1:D2}s", timeSpan.Minutes, timeSpan.Seconds);


            return String.Format("{0}h:{1:D2}m:{2:D2}s", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds);            
        }
    }
}
