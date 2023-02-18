using RoutinR.Core;

namespace RoutinR.Services
{
    /// <summary>
    /// The main class used to control the RoutinR-Application
    /// holds current state (saved work items/ currently running TimeSheetEntry and WorkItem)
    /// </summary>
    public class PunchClockService
    {
        private TimeSheetEntry? currentTimeSheetEntry;

        public bool IsRunning => currentTimeSheetEntry != null && currentTimeSheetEntry.IsRunning;

        public DateTime? StartTime => currentTimeSheetEntry?.StartTime;

        public TimeSpan TotalRunTime
        {
            get
            {
                if (!StartTime.HasValue) return TimeSpan.Zero;
                if (EndTime.HasValue) return EndTime.Value.Subtract(StartTime.Value);

                return DateTime.Now.Subtract(StartTime.Value);
            }
        }

        public string StartTimeOrDefault(string defaultValue)
        {
            if (string.IsNullOrEmpty(defaultValue)) throw new ArgumentException($"{nameof(defaultValue)} is null or empty");
            if (StartTime.HasValue) return StartTime.Value.ToString();

            return defaultValue;
        }

        public DateTime? EndTime => currentTimeSheetEntry?.EndTime;

        public string EndTimeOrDefault(string defaultValue)
        {
            if (string.IsNullOrEmpty(defaultValue)) throw new ArgumentException($"{nameof(defaultValue)} is null or empty");
            if (EndTime.HasValue) return EndTime.Value.ToString();

            return defaultValue;
        }

        /// <summary>
        /// starts a new TimeSheetEntry with current time
        /// </summary>
        public void Start()
        {
            currentTimeSheetEntry = new TimeSheetEntry();
        }

        /// <summary>
        /// stops tracking time for the current TimeSheetEntry and logs the current time as end time
        /// 
        /// raises Exception if called before calling Start
        /// </summary>
        public void Stop()
        {
            if (currentTimeSheetEntry == null) throw new InvalidOperationException("cannot stop, because there was no previous start");

            currentTimeSheetEntry.Stop();
        }
    }
}