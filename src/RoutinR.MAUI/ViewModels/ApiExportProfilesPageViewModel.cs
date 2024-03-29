﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Constants;
using RoutinR.Core;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
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
        private readonly IDataService dataService;

        public ApiExportProfilesPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;

            Jobs = new ObservableCollection<Job>();
            JobNames = new ObservableCollection<string>();
            ApiExportProfiles = new();
            NewHeaders = new();
            NewJobTemplates = new();

            Refresh();
        }

        [RelayCommand]
        async Task Tap(ApiExportProfile entry)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "ApiExportProfile", entry }
            };

            await Shell.Current.GoToAsync($"{nameof(ApiExportProfilePage)}", navigationParameter);
        }

        [RelayCommand]
        async Task Add()
        {
            await Shell.Current.GoToAsync($"{nameof(ApiExportProfilePage)}");
        }

        public void Refresh()
        {
            Jobs.Clear();
            foreach (var job in dataService.GetJobs()) Jobs.Add(job);

            JobNames.Clear();
            foreach (var job in Jobs) JobNames.Add(job.Name);

            ApiExportProfiles.Clear();
            foreach (var apiExportProfile in dataService.GetApiExportProfiles()) ApiExportProfiles.Add(apiExportProfile);
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
