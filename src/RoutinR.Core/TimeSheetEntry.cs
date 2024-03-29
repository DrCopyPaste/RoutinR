﻿using RoutinR.Constants;

namespace RoutinR.Core
{
    public class TimeSheetEntry
    {
        private readonly Job job;
        private readonly DateTime startTime;
        private readonly DateTime endTime;

        public Job Job => job;
        public DateTime StartTime => startTime;
        public DateTime EndTime => endTime;

        /// <summary>
        /// not meant to be called
        /// use this instead:
        /// 
        /// TimeSheetEntry(string jobName, DateTime startTime, DateTime endTime)
        /// </summary>
        private TimeSheetEntry() { job = Job.NewDefault(); }

        public TimeSheetEntry(Job job, DateTime startTime, DateTime endTime)
        {
            if (job == null) throw new ArgumentException($"{nameof(job)} is null");
            if (startTime >= DateTime.Now) throw new ArgumentException($"{nameof(startTime)} is from the future");
            if (endTime >= DateTime.Now) throw new ArgumentException($"{nameof(endTime)} is from the future");
            if (startTime >= endTime) throw new ArgumentException($"{nameof(startTime)} is not smaller than {nameof(endTime)}");

            this.job = job;
            this.startTime = startTime;
            this.endTime = endTime;
        }
    }
}