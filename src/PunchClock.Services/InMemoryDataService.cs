using PunchClock.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PunchClock.Services
{
    public class InMemoryDataService
    {
        private HashSet<Job> jobs = new HashSet<Job>();

        /// <summary>
        /// Gets the number of jobs in internal collection
        /// </summary>
        public int JobCount => jobs.Count;

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
