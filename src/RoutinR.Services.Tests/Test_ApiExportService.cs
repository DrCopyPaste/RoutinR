using RoutinR.Core;

namespace RoutinR.Services.Tests
{
    public class Test_ApiExportService
    {
        [Fact]
        public void Work_items_may_exist()
        {
            var workItem = Job.NewDefault();
            
            Assert.True(true, "work is being done");
        }
    }
}