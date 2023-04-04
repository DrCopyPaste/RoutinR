using RoutinR.Core;

namespace RoutinR.Services.Interfaces
{
    public interface IDataService
    {
        int ApiExportProfileCount { get; }
        int JobCount { get; }
        int JobTimeSheetEntryCount { get; }

        void AddApiExportProfile(ApiExportProfile apiExportProfile);
        void AddJob(Job jobToAdd);
        void AddJobTimeSheetEntry(TimeSheetEntry jobTimeSheetEntry);
        void DeleteJob();
        ApiExportProfile? GetApiExportProfileByName(string name);
        IEnumerable<ApiExportProfile> GetApiExportProfiles();
        Job? GetJobByName(string jobName);
        IEnumerable<Job> GetJobs();
        IEnumerable<TimeSheetEntry> GetJobTimeSheetEntries();
        void Initialize();
        void UpdateApiExportProfile(ApiExportProfile existingProfile, ApiExportProfile updatedProfile);
        void UpdateJob();
        void UpdateJobTimeSheetEntry(TimeSheetEntry existingEntry, TimeSheetEntry updatedEntry);
    }
}