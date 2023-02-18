namespace RoutinR.Services.Tests
{
    public class Test_PunchClockService
    {
        [Fact]
        [Trait("Category", "Restoring")]
        public void Cannot_restore_start_time_to_the_future()
        {
            var punchClock = new PunchClockService();
            var savedStartTime = DateTime.Now.AddMinutes(1);

            var gotExpectedException = false;
            try
            {
                punchClock.StartFrom(savedStartTime);
            }
            catch (ArgumentException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "restoring start time to the future did not raise the expected exception");
        }

        [Trait("Category", "Basic starting and stopping")]
        [Fact]
        public void Start_time_is_returned_from_starting()
        {
            var punchClock = new PunchClockService();
            var startTime = punchClock.Start();

            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime.Value == startTime, "start time returned from start call does not equal actual start time");
        }

        [Trait("Category", "Basic starting and stopping")]
        [Fact]
        public void After_stopping_a_complete_job_time_sheet_entry_is_returned()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            var jobTimeSheetEntry = punchClock.Stop();

            //Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime.Value == jobTimeSheetEntry.StartTime, "job in generated job time sheet entry does not equal punchclock's job");
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime.Value == jobTimeSheetEntry.StartTime, "start time in generated job time sheet entry does not equal punchclock's start time");
            Assert.True(punchClock.EndTime.HasValue && punchClock.EndTime.Value == jobTimeSheetEntry.EndTime, "end time in generated job time sheet entry does not equal punchclock's end time");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Cannot_stop_without_previously_starting()
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

            Assert.True(gotAnException, "stopping without previously starting did not raise an exception");
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
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_is_running_is_false()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            punchClock.Stop();
            Assert.True(!punchClock.IsRunning, "punch clock is running after stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Before_starting_is_running_is_false()
        {
            var punchClock = new PunchClockService();
            Assert.True(!punchClock.IsRunning, "punch clock is running before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_and_stopping_multiple_times_is_running_is_false()
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
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_is_running_is_true()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            Assert.True(punchClock.IsRunning, "punch clock is not running after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_restoring_start_time_is_running_is_true()
        {
            var punchClock = new PunchClockService();
            punchClock.StartFrom(DateTime.Now.AddMinutes(-1));
            Assert.True(punchClock.IsRunning, "punch clock is not running after restoring start time");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_and_stopping_and_starting_multiple_times_is_running_is_true()
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
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_without_previously_stopping_start_time_resets()
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
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_again_start_time_increases()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();
            var savedStartTime = punchClock.StartTime;
            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.StartTime > savedStartTime, "start time did not increase after starting again");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_first_time_start_time_has_a_value()
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
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_start_time_stays_the_same()
        {
            var punchClock = new PunchClockService();
            punchClock.Start();

            var savedStartTime = punchClock.StartTime;
            Assert.True(savedStartTime.HasValue, "punch clock's start time is not set after starting");

            punchClock.Stop();
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime.Equals(savedStartTime), "punch clock's start time is not the same as it was before stopping");
        }

        [Fact]
        [Trait("Category", "Restoring")]
        public void After_restoring_start_time_equals_desired_time()
        {
            var punchClock = new PunchClockService();
            var savedStartTime = DateTime.Now.AddMinutes(-1);

            punchClock.StartFrom(savedStartTime);
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime == savedStartTime, "punch clock's start time is not the same as the start time to be restored");
        }

        [Fact]
        [Trait("Category", "Restoring")]
        public void After_restoring_is_running_is_true()
        {
            var punchClock = new PunchClockService();
            var savedStartTime = DateTime.Now.AddMinutes(-1);

            punchClock.StartFrom(savedStartTime);
            Assert.True(punchClock.IsRunning, "punch clock is not running after being restored");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_end_time_is_set_not_before()
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
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_again_end_time_resets()
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

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void StartTimeOrDefault_yields_default_value_before_first_start()
        {
            var punchClock = new PunchClockService();
            Assert.True(punchClock.StartTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.StartTimeOrDefault)} did not yield default value before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_first_start_StartTimeOrDefault_yields_StartTime()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTimeOrDefault("customDefaultValue") == punchClock.StartTime.Value.ToString(), $"{nameof(punchClock.StartTimeOrDefault)} did not yield {nameof(punchClock.StartTime)} before starting");

            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTimeOrDefault("customDefaultValue") == punchClock.StartTime.Value.ToString(), $"{nameof(punchClock.StartTimeOrDefault)} did not yield {nameof(punchClock.StartTime)} before starting");

            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.StartTime.HasValue && punchClock.StartTimeOrDefault("customDefaultValue") == punchClock.StartTime.Value.ToString(), $"{nameof(punchClock.StartTimeOrDefault)} did not yield {nameof(punchClock.StartTime)} before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Before_stopping_EndTimeOrDefault_yields_default_value()
        {
            var punchClock = new PunchClockService();
            Assert.True(punchClock.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.EndTimeOrDefault)} did not yield default value before stopping");

            punchClock.Start();
            Assert.True(punchClock.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.EndTimeOrDefault)} did not yield default value before stopping");

            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.EndTimeOrDefault)} did not yield default value before stopping");

            punchClock.Stop();
            punchClock.Start();
            Assert.True(punchClock.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.EndTimeOrDefault)} did not yield default value before stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_EndTimeOrDefault_yields_EndTime_but_only_until_starting_again()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();
            punchClock.Stop();

            Assert.True(punchClock.EndTime.HasValue && punchClock.EndTimeOrDefault("customDefaultValue") == punchClock.EndTime.Value.ToString(), $"{nameof(punchClock.EndTimeOrDefault)} did not yield {nameof(punchClock.EndTime)} after stopping");
            punchClock.Start();

            Assert.True(punchClock.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.EndTimeOrDefault)} did not yield default value after starting again");

            punchClock.Stop();
            Assert.True(punchClock.EndTime.HasValue && punchClock.EndTimeOrDefault("customDefaultValue") == punchClock.EndTime.Value.ToString(), $"{nameof(punchClock.EndTimeOrDefault)} did not yield {nameof(punchClock.EndTime)} after stopping");

            punchClock.Start();
            Assert.True(punchClock.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClock.EndTimeOrDefault)} did not yield default value after starting again");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Before_starting_total_runtime_equals_zero()
        {
            var punchClock = new PunchClockService();
            Assert.True(punchClock.TotalRunTime.Equals(TimeSpan.Zero), $"{nameof(punchClock.TotalRunTime)} did not yield zero before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_total_runtime_is_greater_than_zero()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();
            Assert.True(punchClock.TotalRunTime > TimeSpan.Zero, $"{nameof(punchClock.TotalRunTime)} is not greater than zero after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_total_runtime_increases()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();

            var totalRunTime1 = punchClock.TotalRunTime;
            Thread.Sleep(1);
            var totalRunTime2 = punchClock.TotalRunTime;
            Assert.True(totalRunTime1 < totalRunTime2, $"{nameof(punchClock.TotalRunTime)} did not increase after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_Total_runtime_stays_constant()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();
            punchClock.Stop();

            var totalRunTime1 = punchClock.TotalRunTime;
            Thread.Sleep(1);
            var totalRunTime2 = punchClock.TotalRunTime;
            Assert.True(totalRunTime1 == totalRunTime2, $"{nameof(punchClock.TotalRunTime)} did not stay constant after stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_again_total_runtime_resets()
        {
            var punchClock = new PunchClockService();

            punchClock.Start();
            Thread.Sleep(2);
            punchClock.Stop();

            var totalRunTime1 = punchClock.TotalRunTime;

            punchClock.Start();
            var totalRunTime2 = punchClock.TotalRunTime;
            Assert.True(totalRunTime1 > totalRunTime2, $"first {nameof(punchClock.TotalRunTime)} was not longer than second one");
        }
    }
}