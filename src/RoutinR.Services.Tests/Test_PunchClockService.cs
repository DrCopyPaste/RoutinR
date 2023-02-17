namespace RoutinR.Services.Tests
{
    public class Test_PunchClockService
    {
        [Fact]
        public void Is_not_running_after_stopping()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            punchClock.Stop();
            Assert.True(!punchClock.IsRunning, "punch clock is running after stopping");
        }

        [Fact]
        public void Is_not_running_before_starting()
        {
            var punchClock = new PunchClockService();
            Assert.True(!punchClock.IsRunning, "punch clock is running before starting");
        }

        [Fact]
        public void Is_not_running_after_starting_and_stopping_multiple_times()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            punchClock.Stop();
            punchClock.Start();
            punchClock.Stop();
            punchClock.Start();
            punchClock.Stop();
            Assert.True(!punchClock.IsRunning, "punch clock is running before starting");
        }

        [Fact]
        public void Is_running_after_starting()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            Assert.True(punchClock.IsRunning, "punch clock is not running after starting");
        }

        [Fact]
        public void Is_running_after_starting_and_stopping_and_starting_multiple_times()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            punchClock.Stop();
            punchClock.Start();
            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.IsRunning, "punch clock is not running after starting");
        }

        [Fact]
        public void Start_time_resets_when_starting_without_previously_stopping()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();

            var savedStartTime = punchClock.StartTime;
            punchClock.Start();
            Assert.True(!savedStartTime.Equals(punchClock.StartTime), "saved start time equals new start time after starting again");

            savedStartTime = punchClock.StartTime;
            punchClock.Start();
            Assert.True(!savedStartTime.Equals(punchClock.StartTime), "saved start time equals new start time after starting again");

        }

        [Fact]
        public void Stopping_without_previously_starting_fails()
        {
            var punchClock = new PunchClockService();
            var gotAnException = false;

            try
            {
                punchClock.Stop();
            }
            catch
            {
                gotAnException = true;
            }

            Assert.True(gotAnException, "stopping without previously starting did not rais an exception");
            punchClock.Start();
            punchClock.Stop();
            gotAnException = false;

            try
            {
                punchClock.Stop();
            }
            catch
            {
                gotAnException = true;
            }

            Assert.True(gotAnException, "stopping without previously starting did not rais an exception");
        }

        [Fact]
        public void Start_time_increases_when_starting_again()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            var savedStartTime = punchClock.StartTime;
            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.StartTime > savedStartTime, "start time did not increase after starting again");
        }

        [Fact]
        public void Start_time_has_no_value_only_before_starting_first_time()
        {
            var punchClock = new PunchClockService();

            Assert.True(!punchClock.StartTime.HasValue, "punch clock's start time has a value before starting");

            punchClock.Start();
            Assert.True(punchClock.StartTime.HasValue, "punch clock's start time has no value after starting");

            punchClock.Stop();
            Assert.True(punchClock.StartTime.HasValue, "punch clock's start time has no value after stopping");

            punchClock.Start();
            Assert.True(punchClock.StartTime.HasValue, "punch clock's start time has no value after starting again");
        }

        [Fact]
        public void Start_time_stays_the_same_after_stopping()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();

            var savedStartTime = punchClock.StartTime;
            Assert.True(savedStartTime.HasValue, "punch clock's start time is not set after starting");

            punchClock.Stop();
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime.Equals(savedStartTime), "punch clock's start time is not the same as it was before stopping");
        }

        [Fact]
        public void End_time_is_only_set_after_stopping()
        {
            var punchClock = new PunchClockService();

            Assert.True(!punchClock.EndTime.HasValue, "punch clock's end time is set before starting");

            punchClock.Start();
            Assert.True(!punchClock.EndTime.HasValue, "punch clock's end time is set before stopping");

            punchClock.Stop();
            Assert.True(punchClock.EndTime.HasValue, "punch clock's end time is not set after stopping");

            punchClock.Start();
            Assert.True(!punchClock.EndTime.HasValue, "punch clock's end time is set before stopping");

            punchClock.Stop();
            Assert.True(punchClock.EndTime.HasValue, "punch clock's end time is not set after stopping");
        }

        [Fact]
        public void End_time_resets_after_starting_again()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();
            punchClock.Stop();
            var savedStopTime = punchClock.EndTime;

            punchClock.Start();
            Assert.True(!punchClock.EndTime.HasValue, "end time must not have a value just after starting");

            punchClock.Stop();
            Assert.True(punchClock.EndTime.HasValue && punchClock.EndTime.Value > savedStopTime, "end time cannot be smaller than previous end time");
        }
    }
}