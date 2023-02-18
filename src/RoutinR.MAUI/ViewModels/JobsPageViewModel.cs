using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
using RoutinR.Core;
using RoutinR.Services;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace RoutinR.MAUI.ViewModels
{
    public partial class JobsPageViewModel : BaseViewModel
    {
        private readonly InMemoryDataService dataService;

        public JobsPageViewModel(InMemoryDataService dataService)
        {
            this.dataService = dataService;
            jobs = new ObservableCollection<Job>();

            var persistedJobs = dataService.GetJobs();
            foreach(var job in persistedJobs) jobs.Add(job);
        }

        [RelayCommand]
        async Task Add()
        {
            if (string.IsNullOrWhiteSpace(NewJobName))
                return;

            dataService.AddJob(Job.NewFromName(NewJobName));
            Jobs.Add(Job.NewFromName(NewJobName));

            // add our item
            NewJobName = string.Empty;
        }

        [ObservableProperty]
        ObservableCollection<Job> jobs;

        [ObservableProperty]
        string newJobName;
    }
}