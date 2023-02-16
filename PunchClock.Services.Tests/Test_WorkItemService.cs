using PunchClock.Core;

namespace PunchClock.Services.Tests
{
    public class Test_WorkItemService
    {
        [Fact]
        public void Work_items_may_exist()
        {
            var workItem = Job.NewDefault();
            
            Assert.True(true, "work is being done");
        }
    }
}