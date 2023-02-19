using Microsoft.VisualStudio.TestPlatform.Utilities;
using RoutinR.Constants;
using RoutinR.Core;

namespace RoutinR.Services.Tests
{
    public class Test_InMemoryDataService
    {
        private readonly InMemoryDataService dataService;

        public Test_InMemoryDataService()
        {
            this.dataService = new InMemoryDataService();
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void Default_job_exists_by_default()
        {
            Assert.True(dataService.GetJobByName(JobNames.Idle) != null, "no default job was found");
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void Cannot_add_default_job()
        {
            var gotExpectedException = false;

            try
            {
                dataService.AddJob(Job.NewDefault());
            }
            catch (ArgumentException)
            {
                gotExpectedException = true;
            }

            Assert.True(gotExpectedException, "adding default job did not raise the expected exception");
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void Cannot_add_a_job_name_that_already_exists()
        {
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
        [Trait("Category", "Jobs")]
        public void Job_count_is_one_after_initialization()
        {
            // default job already exists making job count = 1 before adding anything custom
            Assert.True(dataService.JobCount == 1, "job count does not equal 1 after initialization");
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void Adding_a_job_increases_job_count()
        {

            // default job already exists making job count = 1 before adding anything custom
            dataService.AddJob(Job.NewFromName("abc"));
            Assert.True(dataService.JobCount == 2, "job count does not equal 1 after first add");

            dataService.AddJob(Job.NewFromName("def"));
            Assert.True(dataService.JobCount == 3, "job count does not equal 2 after second add");

            dataService.AddJob(Job.NewFromName("ghi"));
            Assert.True(dataService.JobCount == 4, "job count does not equal 3 after third add");
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void Deleting_a_job_is_not_supported()
        {
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
        [Trait("Category", "Jobs")]
        public void Updating_a_job_is_not_supported()
        {
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
        [Trait("Category", "Jobs")]
        public void Default_job_can_be_retrieved()
        {
            var jobDefault = dataService.GetJobByName(JobNames.Idle);
            Assert.True(jobDefault != null && jobDefault.Name == JobNames.Idle, "default job was null or did not have the right name");
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void Jobs_added_can_be_retrieved()
        {
            dataService.AddJob(Job.NewFromName("Job1"));
            dataService.AddJob(Job.NewFromName("Job2"));
            dataService.AddJob(Job.NewFromName("Job3"));

            var job1 = dataService.GetJobByName("Job1");
            Assert.True(job1 != null && job1.Name == "Job1", "job 1 was null or did not have the right name");

            var job2 = dataService.GetJobByName("Job2");
            Assert.True(job2 != null && job2.Name == "Job2", "job 2 was null or did not have the right name");

            var job3 = dataService.GetJobByName("Job3");
            Assert.True(job3 != null && job3.Name == "Job3", "job 3 was null or did not have the right name");
        }

        [Fact]
        [Trait("Category", "Jobs")]
        public void GetJobs_gets_all_jobs_in_collection()
        {
            dataService.AddJob(Job.NewFromName("Job1"));
            dataService.AddJob(Job.NewFromName("Job2"));
            dataService.AddJob(Job.NewFromName("Job3"));

            var alljobs = dataService.GetJobs();

            Assert.True(alljobs.Any(job => job.Name == JobNames.Idle), "idle job does not exist in job collection");
            Assert.True(alljobs.Any(job => job.Name == "Job1"), "job 1 does not exist in job collection");
            Assert.True(alljobs.Any(job => job.Name == "Job2"), "job 2 does not exist in job collection");
            Assert.True(alljobs.Any(job => job.Name == "Job3"), "job 3 does not exist in job collection");
        }

        //[Fact]
        //[Trait("Category", "Job sheet entries")]
        //public void Job_time_sheet_entries_added_can_be_retrieved()
        //{

        //    dataService.AddJobTimeSheetEntry(new JobTimeSheetEntry(job: Job.NewDefault(), startTime: DateTime.Now.AddMinutes(-1), endTime: DateTime.Now));

        //    //var job1 = dataService.GetJobByName("Job1");
        //    //Assert.True(job1 != null && job1.Name == "Job1", "job 1 was null or did not have the right name");

        //    //var job2 = dataService.GetJobByName("Job2");
        //    //Assert.True(job2 != null && job2.Name == "Job2", "job 2 was null or did not have the right name");

        //    //var job3 = dataService.GetJobByName("Job3");
        //    //Assert.True(job3 != null && job3.Name == "Job3", "job 3 was null or did not have the right name");
        //}

        [Fact]
        [Trait("Category", "Exporting")]
        public void Exporting_is_not_supported()
        {
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
    }
}