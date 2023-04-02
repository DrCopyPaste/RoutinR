using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoutinR.Core;
using RoutinR.Services.Interfaces;
using RoutinR.SQLite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoutinR.SQLite.Services
{
    public class RoutinRSQLiteService : IDataService, IDisposable
    {
        public int ApiExportProfileCount => context == null ? 0 : context.ApiExportProfiles.Count();

        public int JobCount => context == null ? 0 : context.Jobs.Count();

        public int JobTimeSheetEntryCount => context == null ? 0 : context.TimeSheetEntries.Count();

        private readonly RoutinRContext context;
        private readonly SqliteConnection inMemorySqlite;

        public RoutinRSQLiteService(string connectionString)
        {
            inMemorySqlite = new SqliteConnection(connectionString);
            inMemorySqlite.Open();

            this.context = new RoutinRContext(inMemorySqlite);
            this.context.Database.Migrate();


            if (context.Jobs.Any(job => job.Name.Equals(Core.Job.NewDefault().Name))) return;

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
            if (context.ApiExportProfiles.Any(profile => profile.Name == apiExportProfile.Name)) throw new ArgumentException("an api export profile with that name already exists");
            if (apiExportProfile.JobTemplates.Any(template => !context.Jobs.Any(job => job.Name.Equals(template.Key.Name)))) throw new ArgumentException("not all job templates have valid corresponding jobs");

            var addedProfile = context.ApiExportProfiles.Add(new Entities.ApiExportProfile()
            {
                Name = apiExportProfile.Name,
                PostUrl = apiExportProfile.PostUrl,
                Headers = JsonSerializer.Serialize(apiExportProfile.Headers),
                //JobTemplates = jobTemplates,
                StartTimeToken = apiExportProfile.StartTimeToken,
                EndTimeToken = apiExportProfile.EndTimeToken
            }).Entity;

            foreach (var template in apiExportProfile.JobTemplates)
            {
                var addedEntry = context.JobTemplates.Add(
                    new JobTemplate()
                    {
                        ApiExportProfile = addedProfile,
                        Key = context.Jobs.First(job => job.Name.Equals(template.Key.Name)),
                        Value = template.Value
                    }).Entity;
            };

            context.SaveChanges();
        }

        public void UpdateApiExportProfile(Core.ApiExportProfile existingProfile, Core.ApiExportProfile updatedProfile)
        {
            if (existingProfile.Name != updatedProfile.Name && context.ApiExportProfiles.Any(profile => profile.Name == updatedProfile.Name)) throw new ArgumentException("an api export profile with that name already exists");
            if (updatedProfile.JobTemplates.Any(template => !context.Jobs.Any(job => job.Name.Equals(template.Key.Name)))) throw new ArgumentException("not all job templates have valid corresponding jobs");

            var existingDbEntry = context.ApiExportProfiles.First(profile => profile.Name.Equals(existingProfile.Name));

            existingDbEntry.Name = updatedProfile.Name;
            existingDbEntry.PostUrl = updatedProfile.PostUrl;
            existingDbEntry.StartTimeToken = updatedProfile.StartTimeToken;
            existingDbEntry.EndTimeToken = updatedProfile.EndTimeToken;

            existingDbEntry.Headers = JsonSerializer.Serialize(updatedProfile.Headers);
            existingDbEntry.JobTemplates = new List<JobTemplate>();

            foreach (var template in updatedProfile.JobTemplates)
            {
                existingDbEntry.JobTemplates.Add(new JobTemplate()
                {
                    Key = context.Jobs.First(job => job.Name.Equals(template.Key.Name)),
                    Value = template.Value
                });
            };

            context.ApiExportProfiles.Update(existingDbEntry);
            context.SaveChanges();
        }

        public Core.ApiExportProfile? GetApiExportProfileByName(string name)
        {
            var dbResult = context.ApiExportProfiles
                .Where(profile => profile.Name == name)
                .Include(p => p.JobTemplates)
                .ThenInclude(template => template.Key)
                .FirstOrDefault();
            if (dbResult == null) return null;

            var dbResultTemplates = dbResult.JobTemplates;

            var result = new Core.ApiExportProfile(
                name: dbResult.Name,
                postUrl: dbResult.PostUrl,
                headers: dbResult.Headers == null ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(dbResult.Headers, (JsonSerializerOptions?) null),
                jobNameJsonTemplates: dbResult.JobTemplates != null && dbResult.JobTemplates.Any() ?
                    JobTemplatesToDictionary(dbResult.JobTemplates) :
                    new Dictionary<Core.Job, string>(),
                startTimeToken: dbResult.StartTimeToken,
                endTimeToken: dbResult.EndTimeToken);

            return result;
        }

        public IEnumerable<Core.ApiExportProfile> GetApiExportProfiles()
        {
            //var expr = context.ApiExportProfiles.Select(profile =>
            //    new Core.ApiExportProfile(
            //        profile.Name,
            //        profile.PostUrl,
            //        profile.Headers == null ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(profile.Headers, (JsonSerializerOptions?)null),
            //        profile.JobTemplates.Any() ?
            //            profile.JobTemplates.ToDictionary(template => Core.Job.NewFromName(template.Key.Name), template => template.Value) :
            //            new Dictionary<Core.Job, string>(),
            //        profile.StartTimeToken,
            //        profile.EndTimeToken));

            var pendingList = new List<Core.ApiExportProfile>();
            foreach(var profile in context.ApiExportProfiles
                .Include(profile => profile.JobTemplates)
                .ThenInclude(template => template.Key))
            {
                var pendingEntry = new Core.ApiExportProfile(
                    profile.Name,
                    profile.PostUrl,
                    profile.Headers == null ? new Dictionary<string, string>() : JsonSerializer.Deserialize<Dictionary<string, string>>(profile.Headers, (JsonSerializerOptions?)null),
                    profile.JobTemplates != null && profile.JobTemplates.Any() ?
                        JobTemplatesToDictionary(profile.JobTemplates) :
                        new Dictionary<Core.Job, string>(),
                    profile.StartTimeToken,
                    profile.EndTimeToken);

                pendingList.Add(pendingEntry);
            }


            return pendingList;
        }

        private ICollection<JobTemplate> DictionaryToJobTemplates(Entities.ApiExportProfile profile, Dictionary<Core.Job, string> dictionary)
        {
            if (profile == null) return new List<JobTemplate>();
            if (profile.JobTemplates == null) return new List<JobTemplate>();

            var jobsFromTemplates = context.Jobs.Where(job => profile.JobTemplates.Select(template => template.Key.Name).Contains(job.Name));
            if (jobsFromTemplates.Count() != profile.JobTemplates.Select(template => template.Key.Name).Distinct().Count()) return new List<JobTemplate>();

            var result = new List<JobTemplate>();
            foreach (var template in dictionary)
            {
                result.Add(new JobTemplate()
                {
                    ApiExportProfile = profile,
                    Key = jobsFromTemplates.First(job => job.Name.Equals(template.Key.Name)),
                    Value = template.Value
                });
            }

            return result;
        }

        private Dictionary<Core.Job, string> JobTemplatesToDictionary(ICollection<JobTemplate> jobTemplates)
        {
            var result = new Dictionary<Core.Job, string>();

            foreach (var jobTemplate in jobTemplates)
            {
                result.Add(Core.Job.NewFromName(jobTemplate.Key.Name), jobTemplate.Value);
            }

            return result;
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

        public Core.Job? GetJobByName(string jobName)
        {
            if (!context.Jobs.Any(job => job.Name == jobName)) return null;

            var result = context.Jobs
                .Where(job => job.Name == jobName)
                .Select(job => Core.Job.NewFromName(job.Name))
                .First();
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
