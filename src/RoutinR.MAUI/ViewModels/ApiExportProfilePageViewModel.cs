using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Constants;
using RoutinR.Core;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
using RoutinR.SQLite.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RoutinR.MAUI.ViewModels
{
    [QueryProperty(nameof(ApiExportProfile), "ApiExportProfile")]
    public partial class ApiExportProfilePageViewModel : BaseViewModel
    {
        private readonly IDataService dataService;
        private bool updateExistingProfile = false;

        public ApiExportProfilePageViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            JobNames = new ObservableCollection<string>();
            Headers = new ObservableCollection<KeyValuePair<string, string>>();
            JobTemplates = new ObservableCollection<KeyValuePair<string, string>>();

            Refresh();
        }

        public void Refresh()
        {
            JobNames.Clear();
            foreach (var job in dataService.GetJobs()) JobNames.Add(job.Name);
        }

        /// <summary>
        /// this is a task to be called from appearing in the target page
        /// because Query Parameter is not available at construction time
        /// </summary>
        internal Task InitAsync()
        {
            if (ApiExportProfile == null) return Task.CompletedTask;

            updateExistingProfile = true;
            ProfileName = ApiExportProfile.Name;
            PostUrl = ApiExportProfile.PostUrl;
            StartTimeToken = ApiExportProfile.StartTimeToken;
            EndTimeToken = ApiExportProfile.EndTimeToken;

            if (ApiExportProfile.Headers != null)
            {
                foreach (var header in ApiExportProfile.Headers) Headers.Add(new KeyValuePair<string, string>(header.Key, header.Value));
            }

            foreach (var jobTemplate in ApiExportProfile.JobTemplates) JobTemplates.Add(new KeyValuePair<string, string>(jobTemplate.Key.Name, jobTemplate.Value));

            return Task.CompletedTask;
        }

        [RelayCommand]
        async Task Save()
        {
            if (ApiExportProfile == null && updateExistingProfile) return;

            if (string.IsNullOrWhiteSpace(ProfileName))
                return;

            if (string.IsNullOrWhiteSpace(PostUrl))
                return;

            if (string.IsNullOrWhiteSpace(StartTimeToken))
                return;

            if (string.IsNullOrWhiteSpace(EndTimeToken))
                return;

            if (Headers.Any())
            {
                // need to check/verify anything here?
            }

            var newProfile = new Core.ApiExportProfile(
                name: ProfileName,
                postUrl: PostUrl,
                headers: Headers.ToDictionary(x => x.Key, x => x.Value),
                jobNameJsonTemplates: JobTemplates.ToDictionary(x => Core.Job.NewFromName(x.Key), x => x.Value),
                startTimeToken: StartTimeToken,
                endTimeToken: EndTimeToken);

            if (updateExistingProfile)
            {
                dataService.UpdateApiExportProfile(ApiExportProfile, newProfile);
            }
            else
            {
                dataService.AddApiExportProfile(newProfile);
            }
        }

        [ObservableProperty]
        Core.ApiExportProfile apiExportProfile;

        [ObservableProperty]
        ObservableCollection<string> jobNames;

        [ObservableProperty]
        string profileName;

        [ObservableProperty]
        string postUrl;

        [ObservableProperty]
        string startTimeToken;

        [ObservableProperty]
        string endTimeToken;

        [ObservableProperty]
        ObservableCollection<KeyValuePair<string, string>> headers;

        [ObservableProperty]
        ObservableCollection<KeyValuePair<string, string>> jobTemplates;
    }
}
