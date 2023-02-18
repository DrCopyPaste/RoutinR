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
        public void Between_0_and_59_seconds_render_seconds_and_milliseconds()
        {
            Assert.True(TimeSpanFormatter.Format(TimeSpan.Zero) == "00:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1)) == "00:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 15)) == "00:01", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 430)) == "00:43", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 1, milliseconds: 10)) == "01:01", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 1, milliseconds: 15)) == "01:01", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 1, milliseconds: 19)) == "01:01", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 13, milliseconds: 37)) == "13:03", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 49, milliseconds: 784)) == "49:78", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 59, milliseconds: 999)) == "59:99", "timespan format did not equal expected string");
        }

        [Fact]
        public void Between_1_and_59_minutes_render_minutes_and_seconds()
        {
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 0, milliseconds: 1)) == "01:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 43, milliseconds: 888)) == "01:43", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 42, seconds: 7, milliseconds: 999)) == "42:07", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 0, minutes: 59, seconds: 59, milliseconds: 999)) == "59:59", "timespan format did not equal expected string");
        }

        [Fact]
        public void Between_1_and_24_hours_render_hours_and_minutes()
        {
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 1, minutes: 0, seconds: 0, milliseconds: 0)) == "1:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 1, minutes: 0, seconds: 45, milliseconds: 999)) == "1:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 1, minutes: 17, seconds: 45, milliseconds: 999)) == "1:17", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 12, minutes: 48, seconds: 16, milliseconds: 999)) == "12:48", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 0, hours: 23, minutes: 59, seconds: 56, milliseconds: 999)) == "23:59", "timespan format did not equal expected string");
        }

        [Fact]
        public void Between_1_day_and_max_value_render_days_hours_and_minutes()
        {
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 1, hours: 0, minutes: 0, seconds: 0, milliseconds: 0)) == "1:0:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 1, hours: 7, minutes: 15, seconds: 33, milliseconds: 999)) == "1:7:15", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 1, hours: 23, minutes: 18, seconds: 14, milliseconds: 999)) == "1:23:18", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 20, hours: 0, minutes: 0, seconds: 0, milliseconds: 0)) == "20:0:00", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(new TimeSpan(days: 718, hours: 13, minutes: 7, seconds: 20, milliseconds: 999)) == "718:13:07", "timespan format did not equal expected string");
            Assert.True(TimeSpanFormatter.Format(TimeSpan.MaxValue) == "10675199:2:48", "timespan format did not equal expected string");
        }
    }
}