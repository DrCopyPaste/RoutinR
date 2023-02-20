using RoutinR.Core;

namespace RoutinR.Services
{
    public class InMemoryDataService
    {
        private readonly HashSet<Job> jobs = new() { Job.NewDefault() };
        private readonly List<JobTimeSheetEntry> jobTimeSheetEntries = new();

        /// <summary>
        /// Gets the number of jobs in internal collection
        /// </summary>
        public int JobCount => jobs.Count;

        /// <summary>
        /// Gets the number of job time sheet entries in internal collection
        /// </summary>
        public int JobTimeSheetEntryCount => jobTimeSheetEntries.Count;

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
        public IEnumerable<JobTimeSheetEntry> GetJobTimeSheetEntries()
        {
            return jobTimeSheetEntries.AsEnumerable();
        }

        /// <summary>
        /// Adds a new job time sheet entry to the internal collection
        /// </summary>
        /// <param name="jobTimeSheetEntry">the entry to be added</param>
        public void AddJobTimeSheetEntry(JobTimeSheetEntry jobTimeSheetEntry)
        {
            jobTimeSheetEntries.Add(jobTimeSheetEntry);
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
    }
}
