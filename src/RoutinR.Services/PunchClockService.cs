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
        /// starts a new TimeSheetEntry with a custom set start time
        /// </summary>
        /// <param name="startFrom">start time to be used</param>
        public void StartFrom(DateTime startFrom)
        {
            currentTimeSheetEntry = new TimeSheetEntry(startFrom);
        }

        /// <summary>
        /// starts a new TimeSheetEntry with current time
        /// </summary>
        /// <returns>start time</returns>
        public DateTime Start()
        {
            currentTimeSheetEntry = new TimeSheetEntry();
            return currentTimeSheetEntry.StartTime;
        }

        /// <summary>
        /// stops tracking time for the current TimeSheetEntry and logs the current time as end time
        /// <summary>
        /// stops tracking time for the current TimeSheetEntry and logs the current time as end time
        /// </summary>
        /// <param name="job">
        /// job to log this completed time sheet to,
        /// logs to default job if null
        /// </param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// if called before calling Start
        /// or if end time was not set for some reason
        /// </exception>
        public JobTimeSheetEntry Stop(Job? job = null)
        {
            if (currentTimeSheetEntry == null) throw new InvalidOperationException("cannot stop, because there was no previous start");
            var targetJob = job ?? Job.NewDefault();

            currentTimeSheetEntry.Stop();

            if (!currentTimeSheetEntry.EndTime.HasValue) throw new InvalidOperationException("end time was not set on current time sheet entry");
            return new JobTimeSheetEntry(job: targetJob, currentTimeSheetEntry.StartTime, currentTimeSheetEntry.EndTime.Value);
        }
    }
}