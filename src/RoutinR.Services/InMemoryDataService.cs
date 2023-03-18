using RoutinR.Core;
using System.Data;
using System.Linq;

namespace RoutinR.Services
{
    public class InMemoryDataService
    {
        private readonly HashSet<Job> jobs = new() { Job.NewDefault() };
        private readonly List<TimeSheetEntry> jobTimeSheetEntries = new();
        private readonly List<ApiExportProfile> apiExportProfiles = new();

        /// <summary>
        /// Gets the number of jobs in internal collection
        /// </summary>
        public int JobCount => jobs.Count;

        /// <summary>
        /// Gets the number of job time sheet entries in internal collection
        /// </summary>
        public int JobTimeSheetEntryCount => jobTimeSheetEntries.Count;

        public int ApiExportProfileCount => apiExportProfiles.Count;

        /// <summary>
        /// Gets a job by name
        /// </summary>
        /// <param name="jobName">job name to find</param>
        /// <returns>the found job, null if none was found</returns>
        public Job? GetJobByName(string jobName)
        {
            return jobs.FirstOrDefault(job => job.Name == jobName);
        }

        /// <summary>
        /// Gets all jobs in internal collection
        /// </summary>
        /// <returns><enumerable of jobs/returns>
        public IEnumerable<Job> GetJobs()
        {
            return jobs.AsEnumerable();
        }

        /// <summary>
        /// Gets all job time sheet entries
        /// </summary>
        /// <returns>enumerable of time sheet entries</returns>
        public IEnumerable<TimeSheetEntry> GetJobTimeSheetEntries()
        {
            return jobTimeSheetEntries.AsEnumerable();
        }

        /// <summary>
        /// Adds a new job time sheet entry to the internal collection
        /// </summary>
        /// <param name="jobTimeSheetEntry">the entry to be added</param>
        /// <exception cref="MissingFieldException">if no job with given name exists</exception>
        /// <exception cref="ArgumentOutOfRangeException">if new job time sheet interval would overlap existing time sheet entries</exception>
        public void AddJobTimeSheetEntry(TimeSheetEntry jobTimeSheetEntry)
        {
            if (!jobs.Any(job => job.Name == jobTimeSheetEntry.Job.Name)) throw new MissingFieldException("no job with that name exists");
            if (
                jobTimeSheetEntries.Any(entry =>
                    // start time and end time equal some other entry exactly
                    (entry.StartTime == jobTimeSheetEntry.StartTime && entry.EndTime == jobTimeSheetEntry.EndTime)
                    ||
                    // startTime is between start and end time of existing entries
                    (entry.StartTime <= jobTimeSheetEntry.StartTime && jobTimeSheetEntry.StartTime < entry.EndTime)
                    ||
                    // endTime is between start and end time of existing entries
                    (entry.StartTime < jobTimeSheetEntry.EndTime && jobTimeSheetEntry.EndTime <= entry.EndTime))
                )
            {
                throw new ArgumentOutOfRangeException("cannot insert job time sheet entry as it would overlap already existing entries");
            }
            jobTimeSheetEntries.Add(jobTimeSheetEntry);
        }

        /// <summary>
        /// updates an existing job time sheet entry with new job and/ or time interval data
        /// </summary>
        /// <param name="existingEntry">entry to lookup in internal collection</param>
        /// <param name="updatedEntry">entry to overwrite existingEntry with</param>
        /// <exception cref="ArgumentException">
        /// if the entry could not be updated
        /// (job name does not exist, time interval would overlap existing ones)
        /// </exception>
        public void UpdateJobTimeSheetEntry(TimeSheetEntry existingEntry, TimeSheetEntry updatedEntry)
        {
            var originalEntries = jobTimeSheetEntries.ToList();
            jobTimeSheetEntries.RemoveAll(entry =>
                entry.Job.Name == existingEntry.Job.Name
                && entry.StartTime == existingEntry.StartTime
                && entry.EndTime == existingEntry.EndTime);

            var errorMessage = string.Empty;
            try
            {
                AddJobTimeSheetEntry(updatedEntry);
            }
            catch (MissingFieldException)
            {
                errorMessage = "job time sheet entry could not be updated, because no job with that name exists";
                jobTimeSheetEntries.Clear();
                jobTimeSheetEntries.AddRange(originalEntries);
            }
            catch (ArgumentOutOfRangeException)
            {
                errorMessage = "job time sheet entry could not be updated, because its timespan would overlap other existing time sheets";
                jobTimeSheetEntries.Clear();
                jobTimeSheetEntries.AddRange(originalEntries);
            }

            if (!string.IsNullOrEmpty(errorMessage)) throw new ArgumentException(errorMessage);

        }

        /// <summary>
        /// Adds a job to the internal collection
        /// </summary>
        /// <param name="jobToAdd"></param>
        /// <exception cref="ArgumentException">thrown if job name to add already exists</exception>
        public void AddJob(Job jobToAdd)
        {
            if (jobs.Any(job => job.Name == jobToAdd.Name)) throw new ArgumentException("a job with that name already exists");

            jobs.Add(jobToAdd);
        }

        public void UpdateJob()
        {
            throw new NotImplementedException("updating is not supported");
        }

        public void DeleteJob()
        {
            throw new NotImplementedException("deleting is not supported");
        }

        public void Export()
        {
            throw new NotImplementedException("exporting is not supported");
        }

        public void AddApiExportProfile(ApiExportProfile apiExportProfile)
        {
            if (apiExportProfiles.Any(profile => profile.Name == apiExportProfile.Name)) throw new ArgumentException("an api export profile with that name already exists");

            apiExportProfiles.Add(apiExportProfile);
        }

        public ApiExportProfile? GetApiExportProfileByName(string name)
        {
            return apiExportProfiles.FirstOrDefault(profile => profile.Name == name);
        }

        public IEnumerable<ApiExportProfile> GetApiExportProfiles()
        {
            return apiExportProfiles.AsEnumerable();
        }

        public void UpdateApiExportProfile(ApiExportProfile existingProfile, ApiExportProfile updatedProfile)
        {
            if (existingProfile.Name != updatedProfile.Name && apiExportProfiles.Any(profile => profile.Name == updatedProfile.Name)) throw new ArgumentException("an api export profile with that name already exists");
            if (updatedProfile.JobNameJsonTemplates.Any(template => !jobs.Any(job => job.Name == template.Key))) throw new ArgumentException("not all job templates have valid corresponding jobs");

            apiExportProfiles.Remove(existingProfile);
            apiExportProfiles.Add(updatedProfile);
        }
    }
}
