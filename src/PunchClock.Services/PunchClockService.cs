using PunchClock.Core;

namespace PunchClock.Services
{
    /// <summary>
    /// The main class used to control the PunchClock-Application
    /// holds current state (saved work items/ currently running TimeSheetEntry and WorkItem)
    /// </summary>
    public class PunchClockService
    {
        private TimeSheetEntry? currentTimeSheetEntry;

        public bool IsRunning => currentTimeSheetEntry != null && currentTimeSheetEntry.IsRunning;

        public DateTime? StartTime => currentTimeSheetEntry?.StartTime;
        public DateTime? EndTime => currentTimeSheetEntry?.EndTime;

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