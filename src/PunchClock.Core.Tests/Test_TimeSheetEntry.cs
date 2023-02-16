namespace RoutinR.Core.Tests
{
    public class Test_TimeSheetEntry
    {
        [Fact]
        public void Start_time_must_be_in_the_past()
        {
            var timeSheetEntry = new TimeSheetEntry();
            Assert.True(timeSheetEntry.StartTime < DateTime.Now, "start time is the future");
        }

        [Fact]
        public void Start_time_must_stay_the_same_after_stopping()
        {
            var timeSheetEntry = new TimeSheetEntry();
            var savedStartTime = timeSheetEntry.StartTime;
            timeSheetEntry.Stop();
            Assert.True(timeSheetEntry.StartTime.Equals(savedStartTime), "start time after stopping should does not start time after construction");
        }

        [Fact]
        public void Start_time_must_be_smaller_than_end_time_after_stopping()
        {
            var timeSheetEntry = new TimeSheetEntry();
            timeSheetEntry.Stop();
            Assert.True(timeSheetEntry.EndTime.HasValue && timeSheetEntry.EndTime.Value > timeSheetEntry.StartTime, "end time is not smaller than start time");
        }

        [Fact]
        public void End_time_must_be_set_after_stopping()
        {
            var timeSheetEntry = new TimeSheetEntry();
            timeSheetEntry.Stop();
            Assert.True(timeSheetEntry.EndTime.HasValue, "end time has no value after stopping");
        }

        [Fact]
        public void End_time_must_not_be_set_until_stopping()
        {
            var timeSheetEntry = new TimeSheetEntry();
            Assert.True(!timeSheetEntry.EndTime.HasValue, "end time has a value before stopping");
            timeSheetEntry.Stop();
            Assert.True(timeSheetEntry.EndTime.HasValue, "end time has no after stopping");
        }

        [Fact]
        public void End_time_must_be_in_the_past_after_stopping()
        {
            var timeSheetEntry = new TimeSheetEntry();
            timeSheetEntry.Stop();
            Assert.True(timeSheetEntry.EndTime.HasValue && timeSheetEntry.EndTime.Value < DateTime.Now, "end time is the future");
        }

        [Fact]
        public void Is_running_until_stopped()
        {
            var timeSheetEntry = new TimeSheetEntry();
            Assert.True(timeSheetEntry.IsRunning, "timesheetentry should be running, there was no stop command, yet");
            Assert.True(!timeSheetEntry.EndTime.HasValue, "timesheetentry should have no end time set, yet");

            timeSheetEntry.Stop();

            Assert.True(!timeSheetEntry.IsRunning, "timesheetentry should not be running anymore");
            Assert.True(timeSheetEntry.EndTime.HasValue, "timesheetentry should have an end time set by now");
        }

        [Fact]
        public void Cannot_be_stopped_multiple_times()
        {
            var timeSheetEntry = new TimeSheetEntry();
            timeSheetEntry.Stop();

            var gotExpectedException = false;
            try
            {
                timeSheetEntry.Stop();
            }
            catch (ArgumentOutOfRangeException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "Stopping twice should raise an exception");
        }
    }
}