using RoutinR.Constants;

namespace RoutinR.Core.Tests
{
    public class Test_TimeSpanFormatter
    {
        [Fact]
        public void Cannot_display_values_smaller_than_zero()
        {
            bool gotExpectedException1 = false;
            try
            {
                TimeSpanFormatter.Format(new TimeSpan(hours: 0, minutes: 0, seconds: -1));
            }
            catch (ArgumentException)
            {
                gotExpectedException1 = true;
            }
            Assert.True(gotExpectedException1, "rendering minus one sconds did not raise expected exception");

            bool gotExpectedException2 = false;
            try
            {
                TimeSpanFormatter.Format(new TimeSpan(hours: -1, minutes: 0, seconds: 0));
            }
            catch (ArgumentException)
            {
                gotExpectedException2 = true;
            }
            Assert.True(gotExpectedException2, "rendering minus one hour did not raise expected exception");

            bool gotExpectedException3 = false;
            try
            {
                TimeSpanFormatter.Format(TimeSpan.MinValue);
            }
            catch (ArgumentException)
            {
                gotExpectedException3 = true;
            }
            Assert.True(gotExpectedException3, "rendering minimum value did not raise expected exception");
        }

        [Fact]
        public void Between_0_and_59_seconds_render_minutes_and_seconds()
        {
            Assert.True(TimeSpanFormatter.Format(TimeSpan.Zero) == "00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1)) == "00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 15)) == "00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 430)) == "00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 1, milliseconds: 10)) == "00m:01s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 1, milliseconds: 15)) == "00m:01s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 1, milliseconds: 19)) == "00m:01s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 13, milliseconds: 37)) == "00m:13s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 49, milliseconds: 784)) == "00m:49s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 59, milliseconds: 999)) == "00m:59s", "timespan format did not equal expected string");
        }

        [Fact]
        public void Between_1_and_59_minutes_render_minutes_and_seconds()
        {
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 0, milliseconds: 1)) == "01m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 43, milliseconds: 888)) == "01m:43s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 42, seconds: 7, milliseconds: 999)) == "42m:07s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 59, seconds: 59, milliseconds: 999)) == "59m:59s", "timespan format did not equal expected string");
        }

        [Fact]
        public void Between_1_and_24_hours_render_hours_and_minutes_and_seconds()
        {
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 1, minutes: 0, seconds: 0, milliseconds: 0)) == "1h:00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 1, minutes: 0, seconds: 45, milliseconds: 999)) == "1h:00m:45s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 1, minutes: 17, seconds: 45, milliseconds: 999)) == "1h:17m:45s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 12, minutes: 48, seconds: 16, milliseconds: 999)) == "12h:48m:16s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 23, minutes: 59, seconds: 56, milliseconds: 999)) == "23h:59m:56s", "timespan format did not equal expected string");
        }

        [Fact]
        public void Between_1_day_and_max_value_render_hours_and_minutes_and_seconds()
        {
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0, milliseconds: 0)) == "24h:00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 1, hours: 7, minutes: 15, seconds: 33, milliseconds: 999)) == "31h:15m:33s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 1, hours: 23, minutes: 18, seconds: 14, milliseconds: 999)) == "47h:18m:14s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 20, hours: 0, minutes: 0, seconds: 0, milliseconds: 0)) == "480h:00m:00s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 718, hours: 13, minutes: 7, seconds: 20, milliseconds: 999)) == "17245h:07m:20s", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(TimeSpan.MaxValue) == "256204778h:48m:05s", "timespan format did not equal expected string");
        }
    }
}