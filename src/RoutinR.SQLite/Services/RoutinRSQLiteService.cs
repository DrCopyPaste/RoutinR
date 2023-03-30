using RoutinR.Core;
using RoutinR.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoutinR.SQLite.Services
{
    public class RoutinRSQLiteService : IDataService
    {
        public int ApiExportProfileCount => throw new NotImplementedException();

        public int JobCount => throw new NotImplementedException();

        public int JobTimeSheetEntryCount => throw new NotImplementedException();

        public void AddApiExportProfile(ApiExportProfile apiExportProfile)
        {
            throw new NotImplementedException();
        }

        public void AddJob(Job jobToAdd)
        {
            throw new NotImplementedException();
        }

        public void AddJobTimeSheetEntry(TimeSheetEntry jobTimeSheetEntry)
        {
            throw new NotImplementedException();
        }

        public void DeleteJob()
        {
            throw new NotImplementedException();
        }

        public ApiExportProfile? GetApiExportProfileByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApiExportProfile> GetApiExportProfiles()
        {
            throw new NotImplementedException();
        }

        public Job? GetJobByName(string jobName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Job> GetJobs()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TimeSheetEntry> GetJobTimeSheetEntries()
        {
            throw new NotImplementedException();
        }

        public void UpdateApiExportProfile(ApiExportProfile existingProfile, ApiExportProfile updatedProfile)
        {
            throw new NotImplementedException();
        }

        public void UpdateJob()
        {
            throw new NotImplementedException();
        }

        public void UpdateJobTimeSheetEntry(TimeSheetEntry existingEntry, TimeSheetEntry updatedEntry)
        {
            throw new NotImplementedException();
        }
    }
}
