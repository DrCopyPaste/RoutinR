using RoutinR.Constants;

namespace RoutinR.Core.Tests
{
    public class Test_Job
    {
        [Fact]
        public void Newly_created_default_job_has_default_job_name()
        {
            var job = Job.NewDefault();
            Assert.True(job.Name == JobNames.Idle, $"job name is not {JobNames.Idle}");
        }

        [Fact]
        public void Newly_created_named_job_has_expected_name()
        {
            var jobName = "TestJob123";

            var job = Job.NewFromName(jobName);
            Assert.True(job.Name == jobName, $"job name is not {jobName}");
        }
    }
}