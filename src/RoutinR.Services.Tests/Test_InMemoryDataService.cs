using Microsoft.VisualStudio.TestPlatform.Utilities;
using RoutinR.Core;

namespace RoutinR.Services.Tests
{
    public class Test_InMemoryDataService
    {
        [Fact]
        public void Cannot_add_default_job_twice()
        {
            var dataService = new InMemoryDataService();
            var gotExpectedException = false;

            dataService.AddJob(Job.NewDefault());

            try
            {
                dataService.AddJob(Job.NewDefault());
            }
            catch (ArgumentException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "adding default job twice did not raise the expected exception");
        }

        [Fact]
        public void Cannot_add_a_job_name_that_already_exists()
        {
            var dataService = new InMemoryDataService();
            var gotExpectedException = false;

            dataService.AddJob(Job.NewFromName("abc"));

            try
            {
                dataService.AddJob(Job.NewFromName("abc"));
            }
            catch (ArgumentException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "adding same name twice did not raise the expected exception");
        }

        [Fact]
        public void Job_count_is_zero_after_initialization()
        {
            var dataService = new InMemoryDataService();

            Assert.True(dataService.JobCount == 0, "job count does not equal 0 after initialization");
        }

        [Fact]
        public void Adding_a_job_increases_job_count()
        {
            var dataService = new InMemoryDataService();

            dataService.AddJob(Job.NewFromName("abc"));
            Assert.True(dataService.JobCount == 1, "job count does not equal 1 after first add");

            dataService.AddJob(Job.NewFromName("def"));
            Assert.True(dataService.JobCount == 2, "job count does not equal 2 after second add");

            dataService.AddJob(Job.NewFromName("ghi"));
            Assert.True(dataService.JobCount == 3, "job count does not equal 3 after third add");
        }

        [Fact]
        public void Deleting_a_job_is_not_supported()
        {
            var dataService = new InMemoryDataService();
            var gotExpectedException = false;

            dataService.AddJob(Job.NewFromName("abc"));

            try
            {
                dataService.DeleteJob();
            }
            catch (NotImplementedException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "deleting a job did not raise the expected exception");
        }

        [Fact]
        public void Updating_a_job_is_not_supported()
        {
            var dataService = new InMemoryDataService();
            var gotExpectedException = false;

            dataService.AddJob(Job.NewFromName("abc"));

            try
            {
                dataService.UpdateJob();
            }
            catch (NotImplementedException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "updating a job did not raise the expected exception");
        }

        [Fact]
        public void Exporting_is_not_supported()
        {
            var dataService = new InMemoryDataService();
            var gotExpectedException = false;

            dataService.AddJob(Job.NewFromName("abc"));

            try
            {
                dataService.Export();
            }
            catch (NotImplementedException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "exporting not raise the expected exception");
        }

        [Fact]
        public void Jobs_added_can_be_retrieved()
        {
            var dataService = new InMemoryDataService();

            dataService.AddJob(Job.NewDefault());
            dataService.AddJob(Job.NewFromName("Job1"));
            dataService.AddJob(Job.NewFromName("Job2"));
            dataService.AddJob(Job.NewFromName("Job3"));

            var jobDefault = dataService.GetJobByName(Constants.JobNames.Idle);
            Assert.True(jobDefault != null && jobDefault.Name == Constants.JobNames.Idle, "default job was null or did not have the right name");

            var job1 = dataService.GetJobByName("Job1");
            Assert.True(job1 != null && job1.Name == "Job1", "job 1 was null or did not have the right name");

            var job2 = dataService.GetJobByName("Job2");
            Assert.True(job2 != null && job2.Name == "Job2", "job 2 was null or did not have the right name");

            var job3 = dataService.GetJobByName("Job3");
            Assert.True(job3 != null && job3.Name == "Job3", "job 3 was null or did not have the right name");
        }
    }
}