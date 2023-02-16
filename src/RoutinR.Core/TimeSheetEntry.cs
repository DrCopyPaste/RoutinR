namespace RoutinR.Core
{
    /// <summary>
    /// represents a time slot
    /// </summary>
    public class TimeSheetEntry
    {
        private DateTime startTime = DateTime.Now;
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
        /// stops logging time
        /// </summary>
        public void Stop()
        {
            if (endTime.HasValue)
            {
                throw new ArgumentOutOfRangeException("Time sheet entry was already stopped.");
            }

            endTime = DateTime.Now;
        }
    }
}
