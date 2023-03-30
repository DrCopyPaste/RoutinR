using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Core;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
using System.Collections.ObjectModel;

namespace RoutinR.MAUI.ViewModels
{
    [QueryProperty(nameof(JobTimeSheetEntry), "JobTimeSheetEntry")]
    public partial class JobTimeSheetEntryPageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;

        public JobTimeSheetEntryPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            Jobs = new ObservableCollection<Job>();
            foreach(var job in dataService.GetJobs()) Jobs.Add(job);
        }

        [ObservableProperty]
        Job updatedJob;

        [ObservableProperty]
        DateTime updatedStartTime;

        [ObservableProperty]
        DateTime updatedEndTime;


        [ObservableProperty]
        ObservableCollection<Job> jobs;

        [ObservableProperty]
        TimeSheetEntry jobTimeSheetEntry;

        [RelayCommand]
        async Task SaveChanges()
        {
            dataService.UpdateJobTimeSheetEntry(
                JobTimeSheetEntry,
                new TimeSheetEntry(UpdatedJob, UpdatedStartTime, UpdatedEndTime));
        }

        internal Task InitAsync()
        {
            UpdatedJob = JobTimeSheetEntry.Job;
            UpdatedStartTime = JobTimeSheetEntry.StartTime;
            UpdatedEndTime = JobTimeSheetEntry.EndTime;

            return Task.CompletedTask;
        }
    }
}