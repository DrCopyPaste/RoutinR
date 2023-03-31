using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoutinR.Core;
using RoutinR.Services.Interfaces;
using RoutinR.SQLite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutinR.SQLite.Services
{
    public class RoutinRSQLiteService : IDataService, IDisposable
    {
        public int ApiExportProfileCount => throw new NotImplementedException();

        public int JobCount => context == null ? 0 : context.Jobs.Count();

        public int JobTimeSheetEntryCount => context == null ? 0 : context.TimeSheetEntries.Count();

        private readonly RoutinRContext context;
        private readonly SqliteConnection inMemorySqlite;

        public RoutinRSQLiteService(string connectionString)
        {
            inMemorySqlite = new SqliteConnection(connectionString);
            inMemorySqlite.Open();

            this.context = new RoutinRContext(inMemorySqlite);
            //this.context.Database.EnsureCreated();
            this.context.Database.Migrate();

            context.Jobs.Add(new Entities.Job()
            {
                Name = Core.Job.NewDefault().Name
            });

            context.SaveChanges();
        }

        public void Dispose()
        {
            if (inMemorySqlite != null)
            {
                inMemorySqlite.Close();
                inMemorySqlite.Dispose();
            }

            if (context != null) context.Dispose();
        }

        public void AddApiExportProfile(Core.ApiExportProfile apiExportProfile)
        {
            throw new NotImplementedException();
        }

        public void AddJob(Core.Job jobToAdd)
        {
            if (context.Jobs.Any(job => job.Name == jobToAdd.Name)) throw new ArgumentException("a job with that name already exists");

            context.Jobs.Add(new Entities.Job()
            {
                Name = jobToAdd.Name
            });

            context.SaveChanges();
        }

        public void AddJobTimeSheetEntry(Core.TimeSheetEntry jobTimeSheetEntry)
        {
            if (!context.Jobs.Any(job => job.Name == jobTimeSheetEntry.Job.Name)) throw new MissingFieldException("no job with that name exists");
            if (
                context.TimeSheetEntries.Any(entry =>
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


            var job = context.Jobs.FirstOrDefault(job => job.Name.Equals(jobTimeSheetEntry.Job.Name));
            if (job == null) return;

            context.TimeSheetEntries.Add(new Entities.TimeSheetEntry()
            {
                Job = job,
                StartTime = jobTimeSheetEntry.StartTime,
                EndTime = jobTimeSheetEntry.EndTime
            });

            context.SaveChanges();
        }

        public void DeleteJob()
        {
            throw new NotImplementedException();
        }

        public Core.ApiExportProfile? GetApiExportProfileByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Core.ApiExportProfile> GetApiExportProfiles()
        {
            throw new NotImplementedException();
        }

        public Core.Job? GetJobByName(string jobName)
        {
            if (!context.Jobs.Any(job => job.Name == jobName)) return null;

            var result = context.Jobs
                .Where(job => job.Name == jobName)
                .Select(job => Core.Job.NewFromName(job.Name)).First();
            return result;
        }

        public IEnumerable<Core.Job> GetJobs()
        {
            return context.Jobs.Select(job => Core.Job.NewFromName(job.Name)).AsEnumerable();
        }

        public IEnumerable<Core.TimeSheetEntry> GetJobTimeSheetEntries()
        {
            return context.TimeSheetEntries.Select(entry =>
                new Core.TimeSheetEntry(
                    Core.Job.NewFromName(entry.Job.Name),
                    entry.StartTime,
                    entry.EndTime)).AsEnumerable();
        }

        public void UpdateApiExportProfile(Core.ApiExportProfile existingProfile, Core.ApiExportProfile updatedProfile)
        {
            throw new NotImplementedException();
        }

        public void UpdateJob()
        {
            throw new NotImplementedException();
        }

        public void UpdateJobTimeSheetEntry(Core.TimeSheetEntry existingEntry, Core.TimeSheetEntry updatedEntry)
        {
            var originalEntry = context.TimeSheetEntries.FirstOrDefault(
                entry => entry.Job.Name.Equals(existingEntry.Job.Name)
                && entry.StartTime.Equals(existingEntry.StartTime)
                && entry.EndTime.Equals(existingEntry.EndTime));
            if (originalEntry == null) return;

            var updatedJob = context.Jobs.FirstOrDefault(job => job.Name.Equals(updatedEntry.Job.Name));
            if (updatedJob == null) throw new ArgumentException($"no job was found with name '{updatedEntry.Job.Name}'");

            context.TimeSheetEntries.Remove(originalEntry);
            context.SaveChanges();

            var errorMessage = string.Empty;
            try
            {
                AddJobTimeSheetEntry(updatedEntry);
            }
            catch (MissingFieldException)
            {
                errorMessage = "job time sheet entry could not be updated, because no job with that name exists";
                context.TimeSheetEntries.Add(originalEntry);
                context.SaveChanges();
            }
            catch (ArgumentOutOfRangeException)
            {
                errorMessage = "job time sheet entry could not be updated, because its timespan would overlap other existing time sheets";
                context.TimeSheetEntries.Add(originalEntry);
                context.SaveChanges();
            }

            if (!string.IsNullOrEmpty(errorMessage)) throw new ArgumentException(errorMessage);

        }
    }
}
