namespace RoutinR.Services.Tests
{
    public class Test_PunchClockService
    {
        private readonly PunchClockService punchClockService;
        public Test_PunchClockService()
        {
            this.punchClockService = new PunchClockService();
        }

        [Fact]
        [Trait("Category", "Restoring")]
        public void Cannot_restore_start_time_to_the_future()
        {
            var savedStartTime = DateTime.Now.AddMinutes(1);

            var gotExpectedException = false;
            try
            {
                punchClockService.StartFrom(savedStartTime);
            }
            catch (ArgumentException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "restoring start time to the future did not raise the expected exception");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Start_time_is_returned_from_starting()
        {
            var startTime = punchClockService.Start();

            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTime.Value == startTime, "start time returned from start call does not equal actual start time");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_a_complete_job_time_sheet_entry_is_returned()
        {
            punchClockService.Start();
            var jobTimeSheetEntry = punchClockService.Stop();

            //Assert.True(punchClock.StartTime.HasValue && punchClock.StartTime.Value == jobTimeSheetEntry.StartTime, "job in generated job time sheet entry does not equal punchclock's job");
            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTime.Value == jobTimeSheetEntry.StartTime, "start time in generated job time sheet entry does not equal punchclock's start time");
            Assert.True(punchClockService.EndTime.HasValue && punchClockService.EndTime.Value == jobTimeSheetEntry.EndTime, "end time in generated job time sheet entry does not equal punchclock's end time");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Cannot_stop_without_previously_starting()
        {
            var gotAnException = false;

            try
            {
                punchClockService.Stop();
            }
            catch
            {
                gotAnException = true;
            }

            Assert.True(gotAnException, "stopping without previously starting did not raise an exception");
            punchClockService.Start();
            punchClockService.Stop();
            gotAnException = false;

            try
            {
                punchClockService.Stop();
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
            punchClockService.Start();
            punchClockService.Stop();
            Assert.True(!punchClockService.IsRunning, "punch clock is running after stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Before_starting_is_running_is_false()
        {
            Assert.True(!punchClockService.IsRunning, "punch clock is running before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_and_stopping_multiple_times_is_running_is_false()
        {
            punchClockService.Start();
            punchClockService.Stop();
            punchClockService.Start();
            punchClockService.Stop();
            punchClockService.Start();
            punchClockService.Stop();
            Assert.True(!punchClockService.IsRunning, "punch clock is running before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_is_running_is_true()
        {
            punchClockService.Start();
            Assert.True(punchClockService.IsRunning, "punch clock is not running after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_restoring_start_time_is_running_is_true()
        {
            punchClockService.StartFrom(DateTime.Now.AddMinutes(-1));
            Assert.True(punchClockService.IsRunning, "punch clock is not running after restoring start time");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_and_stopping_and_starting_multiple_times_is_running_is_true()
        {
            punchClockService.Start();
            punchClockService.Stop();
            punchClockService.Start();
            punchClockService.Stop();
            punchClockService.Start();
            Assert.True(punchClockService.IsRunning, "punch clock is not running after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_without_previously_stopping_start_time_resets()
        {
            punchClockService.Start();

            var savedStartTime = punchClockService.StartTime;
            punchClockService.Start();
            Assert.True(!savedStartTime.Equals(punchClockService.StartTime), "saved start time equals new start time after starting again");

            savedStartTime = punchClockService.StartTime;
            punchClockService.Start();
            Assert.True(!savedStartTime.Equals(punchClockService.StartTime), "saved start time equals new start time after starting again");

        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_again_start_time_increases()
        {
            punchClockService.Start();
            var savedStartTime = punchClockService.StartTime;
            punchClockService.Stop();
            punchClockService.Start();
            Assert.True(punchClockService.StartTime > savedStartTime, "start time did not increase after starting again");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_first_time_start_time_has_a_value()
        {
            Assert.True(!punchClockService.StartTime.HasValue, "punch clock's start time has a value before starting");

            punchClockService.Start();
            Assert.True(punchClockService.StartTime.HasValue, "punch clock's start time has no value after starting");

            punchClockService.Stop();
            Assert.True(punchClockService.StartTime.HasValue, "punch clock's start time has no value after stopping");

            punchClockService.Start();
            Assert.True(punchClockService.StartTime.HasValue, "punch clock's start time has no value after starting again");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_start_time_stays_the_same()
        {
            punchClockService.Start();

            var savedStartTime = punchClockService.StartTime;
            Assert.True(savedStartTime.HasValue, "punch clock's start time is not set after starting");

            punchClockService.Stop();
            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTime.Equals(savedStartTime), "punch clock's start time is not the same as it was before stopping");
        }

        [Fact]
        [Trait("Category", "Restoring")]
        public void After_restoring_start_time_equals_desired_time()
        {
            var savedStartTime = DateTime.Now.AddMinutes(-1);

            punchClockService.StartFrom(savedStartTime);
            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTime == savedStartTime, "punch clock's start time is not the same as the start time to be restored");
        }

        [Fact]
        [Trait("Category", "Restoring")]
        public void After_restoring_is_running_is_true()
        {
            var savedStartTime = DateTime.Now.AddMinutes(-1);

            punchClockService.StartFrom(savedStartTime);
            Assert.True(punchClockService.IsRunning, "punch clock is not running after being restored");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_end_time_is_set_not_before()
        {
            Assert.True(!punchClockService.EndTime.HasValue, "punch clock's end time is set before starting");

            punchClockService.Start();
            Assert.True(!punchClockService.EndTime.HasValue, "punch clock's end time is set before stopping");

            punchClockService.Stop();
            Assert.True(punchClockService.EndTime.HasValue, "punch clock's end time is not set after stopping");

            punchClockService.Start();
            Assert.True(!punchClockService.EndTime.HasValue, "punch clock's end time is set before stopping");

            punchClockService.Stop();
            Assert.True(punchClockService.EndTime.HasValue, "punch clock's end time is not set after stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_again_end_time_resets()
        {
            punchClockService.Start();
            punchClockService.Stop();
            var savedStopTime = punchClockService.EndTime;

            punchClockService.Start();
            Assert.True(!punchClockService.EndTime.HasValue, "end time must not have a value just after starting");

            punchClockService.Stop();
            Assert.True(punchClockService.EndTime.HasValue && punchClockService.EndTime.Value > savedStopTime, "end time cannot be smaller than previous end time");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void StartTimeOrDefault_yields_default_value_before_first_start()
        {
            Assert.True(punchClockService.StartTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.StartTimeOrDefault)} did not yield default value before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_first_start_StartTimeOrDefault_yields_StartTime()
        {
            punchClockService.Start();
            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTimeOrDefault("customDefaultValue") == punchClockService.StartTime.Value.ToString(), $"{nameof(punchClockService.StartTimeOrDefault)} did not yield {nameof(punchClockService.StartTime)} before starting");

            punchClockService.Stop();
            punchClockService.Start();
            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTimeOrDefault("customDefaultValue") == punchClockService.StartTime.Value.ToString(), $"{nameof(punchClockService.StartTimeOrDefault)} did not yield {nameof(punchClockService.StartTime)} before starting");

            punchClockService.Stop();
            punchClockService.Start();
            Assert.True(punchClockService.StartTime.HasValue && punchClockService.StartTimeOrDefault("customDefaultValue") == punchClockService.StartTime.Value.ToString(), $"{nameof(punchClockService.StartTimeOrDefault)} did not yield {nameof(punchClockService.StartTime)} before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Before_stopping_EndTimeOrDefault_yields_default_value()
        {
            Assert.True(punchClockService.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.EndTimeOrDefault)} did not yield default value before stopping");

            punchClockService.Start();
            Assert.True(punchClockService.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.EndTimeOrDefault)} did not yield default value before stopping");

            punchClockService.Stop();
            punchClockService.Start();
            Assert.True(punchClockService.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.EndTimeOrDefault)} did not yield default value before stopping");

            punchClockService.Stop();
            punchClockService.Start();
            Assert.True(punchClockService.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.EndTimeOrDefault)} did not yield default value before stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_EndTimeOrDefault_yields_EndTime_but_only_until_starting_again()
        {
            punchClockService.Start();
            punchClockService.Stop();

            Assert.True(punchClockService.EndTime.HasValue && punchClockService.EndTimeOrDefault("customDefaultValue") == punchClockService.EndTime.Value.ToString(), $"{nameof(punchClockService.EndTimeOrDefault)} did not yield {nameof(punchClockService.EndTime)} after stopping");
            punchClockService.Start();

            Assert.True(punchClockService.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.EndTimeOrDefault)} did not yield default value after starting again");

            punchClockService.Stop();
            Assert.True(punchClockService.EndTime.HasValue && punchClockService.EndTimeOrDefault("customDefaultValue") == punchClockService.EndTime.Value.ToString(), $"{nameof(punchClockService.EndTimeOrDefault)} did not yield {nameof(punchClockService.EndTime)} after stopping");

            punchClockService.Start();
            Assert.True(punchClockService.EndTimeOrDefault("customDefaultValue") == "customDefaultValue", $"{nameof(punchClockService.EndTimeOrDefault)} did not yield default value after starting again");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void Before_starting_total_runtime_equals_zero()
        {
            Assert.True(punchClockService.TotalRunTime.Equals(TimeSpan.Zero), $"{nameof(punchClockService.TotalRunTime)} did not yield zero before starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_total_runtime_is_greater_than_zero()
        {
            punchClockService.Start();
            Assert.True(punchClockService.TotalRunTime > TimeSpan.Zero, $"{nameof(punchClockService.TotalRunTime)} is not greater than zero after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_total_runtime_increases()
        {
            punchClockService.Start();

            var totalRunTime1 = punchClockService.TotalRunTime;
            Thread.Sleep(1);
            var totalRunTime2 = punchClockService.TotalRunTime;
            Assert.True(totalRunTime1 < totalRunTime2, $"{nameof(punchClockService.TotalRunTime)} did not increase after starting");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_stopping_Total_runtime_stays_constant()
        {
            punchClockService.Start();
            punchClockService.Stop();

            var totalRunTime1 = punchClockService.TotalRunTime;
            Thread.Sleep(1);
            var totalRunTime2 = punchClockService.TotalRunTime;
            Assert.True(totalRunTime1 == totalRunTime2, $"{nameof(punchClockService.TotalRunTime)} did not stay constant after stopping");
        }

        [Fact]
        [Trait("Category", "Basic starting and stopping")]
        public void After_starting_again_total_runtime_resets()
        {
            punchClockService.Start();
            Thread.Sleep(2);
            punchClockService.Stop();

            var totalRunTime1 = punchClockService.TotalRunTime;

            punchClockService.Start();
            var totalRunTime2 = punchClockService.TotalRunTime;
            Assert.True(totalRunTime1 > totalRunTime2, $"first {nameof(punchClockService.TotalRunTime)} was not longer than second one");
        }
    }
}