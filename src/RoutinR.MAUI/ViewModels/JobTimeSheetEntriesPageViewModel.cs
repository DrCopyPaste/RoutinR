using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Core;
using RoutinR.Services;
using System.Collections.ObjectModel;

namespace RoutinR.MAUI.ViewModels
{
    public partial class JobTimeSheetEntriesPageViewModel : BaseViewModel
    {
        private readonly InMemoryDataService dataService;

        public JobTimeSheetEntriesPageViewModel(InMemoryDataService dataService)
        {
            this.dataService = dataService;
            JobTimeSheetEntries = new ObservableCollection<TimeSheetEntry>();
        }

        public void RefreshEntries()
        {
            JobTimeSheetEntries.Clear();
            foreach (var jobTimeSheetEntry in this.dataService.GetJobTimeSheetEntries()) JobTimeSheetEntries.Add(jobTimeSheetEntry);
        }

        [ObservableProperty]
        ObservableCollection<TimeSheetEntry> jobTimeSheetEntries;

        [RelayCommand]
        async Task Tap(TimeSheetEntry entry)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "JobTimeSheetEntry", entry }
            };

            await Shell.Current.GoToAsync($"{nameof(JobTimeSheetEntryPage)}", navigationParameter);
        }
    }
}