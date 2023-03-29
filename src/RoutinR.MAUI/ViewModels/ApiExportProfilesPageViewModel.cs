using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Core;
using RoutinR.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RoutinR.MAUI.ViewModels
{
    public partial class ApiExportProfilesPageViewModel : BaseViewModel
    {
        private readonly InMemoryDataService dataService;

        public ApiExportProfilesPageViewModel(InMemoryDataService dataService)
        {
            this.dataService = dataService;

            Jobs = new ObservableCollection<Job>();
            foreach (var job in dataService.GetJobs()) Jobs.Add(job);


            JobNames = new ObservableCollection<string>();
            foreach (var job in Jobs) JobNames.Add(job.Name);

            dataService.AddApiExportProfile(new ApiExportProfile(
                name: "TestName1",
                postUrl: "https://postUrl1",
                startTimeToken: "_START1_",
                endTimeToken: "_END1_",
                headers: new() { { "header2", "value2" } },
                jobNameJsonTemplates: new() { { Job.NewDefault().Name, "_START1__END1_" } }));

            ApiExportProfiles = new();
            foreach (var apiExportProfile in dataService.GetApiExportProfiles()) ApiExportProfiles.Add(apiExportProfile);

            NewHeaders = new();
            NewJobTemplates = new();
        }

        [RelayCommand]
        async Task Add()
        {
            if (string.IsNullOrWhiteSpace(NewProfileName))
                return;

            if (string.IsNullOrWhiteSpace(NewPostUrl))
                return;

            if (string.IsNullOrWhiteSpace(NewStartTimeToken))
                return;

            if (string.IsNullOrWhiteSpace(NewEndTimeToken))
                return;

            if (NewHeaders.Any())
            {
            }

            var newProfile = new ApiExportProfile(
                name: NewProfileName,
                postUrl: NewPostUrl,
                headers: NewHeaders.ToDictionary(x => x.Key, x => x.Value),
                jobNameJsonTemplates: NewJobTemplates.ToDictionary(x => x.Key, x => x.Value),
                startTimeToken: "123",
                endTimeToken: "456");

            dataService.AddApiExportProfile(newProfile);
            ApiExportProfiles.Add(newProfile);
        }

        [ObservableProperty]
        ObservableCollection<Job> jobs;

        [ObservableProperty]
        ObservableCollection<string> jobNames;

        [ObservableProperty]
        ObservableCollection<ApiExportProfile> apiExportProfiles;

        [ObservableProperty]
        string newProfileName;

        [ObservableProperty]
        string newPostUrl;

        [ObservableProperty]
        string newStartTimeToken;

        [ObservableProperty]
        string newEndTimeToken;

        [ObservableProperty]
        string newHeaderKey;

        [ObservableProperty]
        string newHeaderValue;

        [ObservableProperty]
        ObservableCollection<KeyValuePair<string, string>> newHeaders;

        [ObservableProperty]
        ObservableCollection<KeyValuePair<string, string>> newJobTemplates;
    }
}
