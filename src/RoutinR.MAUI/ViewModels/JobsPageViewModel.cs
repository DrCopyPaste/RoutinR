using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Networking;
using RoutinR.Core;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace RoutinR.MAUI.ViewModels
{
    public partial class JobsPageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;

        public JobsPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;
            Jobs = new ObservableCollection<Job>();

            foreach(var job in dataService.GetJobs()) Jobs.Add(job);
        }

        [RelayCommand]
        async Task Add()
        {
            if (string.IsNullOrWhiteSpace(NewJobName))
                return;

            dataService.AddJob(Job.NewFromName(NewJobName));
            Jobs.Add(Job.NewFromName(NewJobName));

            NewJobName = string.Empty;
        }

        [ObservableProperty]
        ObservableCollection<Job> jobs;

        [ObservableProperty]
        string newJobName;
    }
}