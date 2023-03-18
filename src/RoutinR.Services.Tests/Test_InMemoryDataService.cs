using Microsoft.VisualStudio.TestPlatform.Utilities;
using RoutinR.Constants;
using RoutinR.Core;
using System.Data;
using System.Reflection.PortableExecutable;

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

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Cannot_add_job_time_sheet_entry_for_non_existing_jobs()
        {
            var gotExpectedException = false;
            try
            {
                dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewFromName("random non existing name"), startTime: DateTime.Now.AddMinutes(-1), endTime: DateTime.Now));
            }
            catch (MissingFieldException)
            {
                gotExpectedException = true;
            }
            Assert.True(gotExpectedException, "adding a job time sheet for a non existing job did not raise the expected exception");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Job_time_sheet_entries_count_is_zero_after_initialization()
        {
            Assert.True(dataService.JobTimeSheetEntryCount == 0, "Job time sheet entry count does not equal zero after initialization");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Job_time_sheet_entries_count_increases_by_adding_new_job_time_sheet_entries()
        {
            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2010, 1, 1, 10, 13, 37), endTime: new DateTime(2010, 1, 1, 10, 13, 38)));
            Assert.True(dataService.JobTimeSheetEntryCount == 1, "Job time sheet entry count does not equal zero after initialization");

            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2010, 1, 1, 10, 13, 40), endTime: new DateTime(2010, 1, 1, 10, 13, 50)));
            Assert.True(dataService.JobTimeSheetEntryCount == 2, "Job time sheet entry count does not equal zero after initialization");

            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2010, 1, 1, 11, 13, 37), endTime: new DateTime(2010, 1, 2, 10, 13, 37)));
            Assert.True(dataService.JobTimeSheetEntryCount == 3, "Job time sheet entry count does not equal zero after initialization");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Adjacent_job_time_sheet_entries_start_and_end_times_may_touch()
        {
            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2010, 1, 1, 10, 13, 37), endTime: new DateTime(2010, 1, 1, 10, 13, 38)));
            Assert.True(dataService.JobTimeSheetEntryCount == 1, "Job time sheet entry count does not equal zero after initialization");

            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2010, 1, 1, 10, 13, 38), endTime: new DateTime(2010, 1, 1, 10, 13, 50)));
            Assert.True(dataService.JobTimeSheetEntryCount == 2, "Job time sheet entry count does not equal zero after initialization");

            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2010, 1, 1, 10, 13, 50), endTime: new DateTime(2010, 1, 2, 10, 13, 37)));
            Assert.True(dataService.JobTimeSheetEntryCount == 3, "Job time sheet entry count does not equal zero after initialization");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Cannot_insert_overlapping_job_time_sheet_entries()
        {
            var originalStartTime = DateTime.Now.AddMinutes(-1);
            var originalEndTime = DateTime.Now;

            dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: originalStartTime, originalEndTime));
            var gotExpectedException = false;
            try
            {
                dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: originalStartTime, originalEndTime));
            }
            catch (ArgumentOutOfRangeException)
            {
                gotExpectedException = true;
            }
            Assert.True(gotExpectedException, "inserting the same job time sheet entry twice did not raise the expected exception");

            var gotExpectedException2 = false;
            try
            {
                dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: originalStartTime.AddSeconds(30), DateTime.Now));
            }
            catch (ArgumentOutOfRangeException)
            {
                gotExpectedException2 = true;
            }
            Assert.True(gotExpectedException2, "inserting a job time sheet entry with start time falling between start and end time of an existing entry did not raise the expected exception");

            var gotExpectedException3 = false;
            try
            {
                dataService.AddJobTimeSheetEntry(new TimeSheetEntry(job: Job.NewDefault(), startTime: originalStartTime.AddDays(-1), originalEndTime));
            }
            catch (ArgumentOutOfRangeException)
            {
                gotExpectedException3 = true;
            }
            Assert.True(gotExpectedException3, "inserting a job time sheet entry with end time falling between start and end time of an existing entry did not raise the expected exception");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Job_time_sheet_entries_can_be_updated_and_retrieved_again()
        {
            var job2 = Job.NewFromName("Job2");
            dataService.AddJob(job2);

            var entry = new TimeSheetEntry(job: Job.NewDefault(), startTime: DateTime.Now.AddMinutes(-100), DateTime.Now);
            dataService.AddJobTimeSheetEntry(entry);

            var changeName = new TimeSheetEntry(job: job2, startTime: DateTime.Now.AddMinutes(-100), DateTime.Now);
            dataService.UpdateJobTimeSheetEntry(entry, changeName);

            var changeStartTime = new TimeSheetEntry(job: job2, startTime: DateTime.Now.AddMinutes(-50), DateTime.Now);
            dataService.UpdateJobTimeSheetEntry(changeName, changeStartTime);

            var changeEndTime = new TimeSheetEntry(job: job2, startTime: DateTime.Now.AddMinutes(-50), DateTime.Now);
            dataService.UpdateJobTimeSheetEntry(changeStartTime, changeEndTime);

            var changeEveryThing = new TimeSheetEntry(job: Job.NewDefault(), startTime: new DateTime(2000, 2, 28, 5, 30, 0), new DateTime(2001, 1, 12, 17, 42, 0));
            dataService.UpdateJobTimeSheetEntry(changeEndTime, changeEveryThing);

            var updatedEntry = dataService.GetJobTimeSheetEntries().ToArray()[0];
            Assert.True(updatedEntry.Job.Name == updatedEntry.Job.Name && updatedEntry.StartTime == changeEveryThing.StartTime && updatedEntry.EndTime == changeEveryThing.EndTime,
                "updated entry from internal collection did not have expected properties");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Cannot_update_a_job_timesheet_entry_to_a_non_existing_job()
        {
            var entry = new TimeSheetEntry(job: Job.NewDefault(), startTime: DateTime.Now.AddMinutes(-20), DateTime.Now.AddMinutes(-10));
            dataService.AddJobTimeSheetEntry(entry);

            var wrongJobNameEntry = new TimeSheetEntry(job: Job.NewFromName("nonExistingEntryName"), startTime: entry.StartTime, entry.EndTime);
            var gotExpectedException = false;
            try
            {
                dataService.UpdateJobTimeSheetEntry(entry, wrongJobNameEntry);
            }
            catch (ArgumentException)
            {
                gotExpectedException = true;
            }
            Assert.True(gotExpectedException, "updating a job time sheet entry to a non existing job name did not raise the expected exception");
        }

        [Fact]
        [Trait("Category", "Job time sheet entries")]
        public void Cannot_create_overlapping_timesheets_by_updating()
        {
            var entry1 = new TimeSheetEntry(job: Job.NewDefault(), startTime: DateTime.Now.AddMinutes(-20), DateTime.Now.AddMinutes(-10));
            var entry2 = new TimeSheetEntry(job: Job.NewDefault(), startTime: entry1.EndTime, DateTime.Now);

            dataService.AddJobTimeSheetEntry(entry1);
            dataService.AddJobTimeSheetEntry(entry2);

            var existingTimeInterval = new TimeSheetEntry(job: Job.NewFromName(entry1.Job.Name), startTime: entry1.StartTime, entry1.EndTime);
            var gotExpectedException2 = false;
            try
            {
                dataService.UpdateJobTimeSheetEntry(entry2, existingTimeInterval);
            }
            catch (ArgumentException)
            {
                gotExpectedException2 = true;
            }
            Assert.True(gotExpectedException2, "updating a job time sheet entry to a non existing job name did not raise the expected exception");

            var startTimeFallsIntoExistingInterval = new TimeSheetEntry(job: Job.NewFromName(entry1.Job.Name), startTime: entry1.StartTime.AddMinutes(1), entry2.EndTime);
            var gotExpectedException3 = false;
            try
            {
                dataService.UpdateJobTimeSheetEntry(entry2, startTimeFallsIntoExistingInterval);
            }
            catch (ArgumentException)
            {
                gotExpectedException3 = true;
            }
            Assert.True(gotExpectedException3, "updating a job time sheet entry's start time to fall in between start and end time of another existing entry did not raise the expected exception");

            var endTimeFallsIntoExistingInterval = new TimeSheetEntry(job: Job.NewFromName(entry1.Job.Name), startTime: entry1.StartTime.AddMinutes(-1), entry1.EndTime.AddMinutes(-1));
            var gotExpectedException4 = false;
            try
            {
                dataService.UpdateJobTimeSheetEntry(entry2, endTimeFallsIntoExistingInterval);
            }
            catch (ArgumentException)
            {
                gotExpectedException4 = true;
            }
            Assert.True(gotExpectedException4, "updating a job time sheet entry's end time to fall in between start and end time of another existing entry did not raise the expected exception");
        }

        [Fact]
        [Trait("Category", "ApiExportProfiles")]
        public void Adding_a_profile_increases_count()
        {
            var countBefore = dataService.ApiExportProfileCount;
            var apiExportProfile = new ApiExportProfile(
                name: "TestName",
                postUrl: "https://postUrl",
                startTimeToken: "_START_",
                endTimeToken: "_END_",
                headers: new() { { "header1", "value1" } },
                jobNameJsonTemplates: new() { { "JobName", "_START__END_" } });
            dataService.AddApiExportProfile(apiExportProfile);
            var countAfter = dataService.ApiExportProfileCount;

            Assert.True(countAfter == (countBefore + 1), "Api export profile count did not increase by one after adding");
        }

        [Fact]
        [Trait("Category", "ApiExportProfiles")]
        public void Profiles_can_be_added_and_retrieved()
        {
            var addedProfile = new ApiExportProfile(
                name: "TestName",
                postUrl: "https://postUrl",
                startTimeToken: "_START_",
                endTimeToken: "_END_",
                headers: new() { { "header1", "value1" } },
                jobNameJsonTemplates: new() { { "JobName", "_START__END_" } });
            dataService.AddApiExportProfile(addedProfile);

            var profileFromService = dataService.GetApiExportProfileByName(name: "TestName");

            Assert.True(profileFromService != null, "profile from service is null");
            Assert.True(profileFromService.Name == addedProfile.Name, "profile from service did not have expected name");
            Assert.True(profileFromService.PostUrl == addedProfile.PostUrl, "profile from service did not have expected post url");
            Assert.True(profileFromService.Headers != null && profileFromService.Headers.Count == 1, "profile from service does not contain any headers");
            Assert.True(profileFromService.Headers.ContainsKey("header1"), "profile from service does not contain header1");
            Assert.True(profileFromService.Headers["header1"] == "value1", "profile's header was not correct");
            Assert.True(profileFromService.JobNameJsonTemplates != null && profileFromService.JobNameJsonTemplates.Count == 1, "profile from service does not contain any headers");
            Assert.True(profileFromService.JobNameJsonTemplates.ContainsKey("JobName"), "profile from service does not contain JobName template");
            Assert.True(profileFromService.JobNameJsonTemplates["JobName"] == "_START__END_", "profile's job template was not correct");
        }

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