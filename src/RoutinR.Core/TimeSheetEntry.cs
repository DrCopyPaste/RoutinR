namespace RoutinR.Core
{
    /// <summary>
    /// represents a time slot
    /// </summary>
    public class TimeSheetEntry
    {
        private readonly DateTime startTime = DateTime.Now;
        private DateTime? endTime = null;

        public bool IsRunning => !endTime.HasValue;
        public DateTime StartTime => startTime;
        public DateTime? EndTime => endTime;

        /// <summary>
        /// starts logging time on construction
        /// </summary>
        public TimeSheetEntry()
        { }

        /// <summary>
        /// starts logging time from given time in the past
        /// </summary>
        /// <param name="startFrom">
        /// time to set start time to
        /// must be in the past
        /// </param>
        public TimeSheetEntry(DateTime startFrom)
        {
            if (DateTime.Now < startFrom) throw new ArgumentException("Time sheet entry starts in the future.");
            startTime = startFrom;
        }

        /// <summary>
        /// stops logging time
        /// </summary>
        public void Stop()
        {
            if (endTime.HasValue) throw new ArgumentException("Time sheet entry was already stopped.");
            endTime = DateTime.Now;
        }
    }
}
