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
            JobTimeSheetEntries = new ObservableCollection<JobTimeSheetEntry>();
        }

        public void RefreshEntries()
        {
            JobTimeSheetEntries.Clear();
            foreach (var jobTimeSheetEntry in this.dataService.GetJobTimeSheetEntries()) JobTimeSheetEntries.Add(jobTimeSheetEntry);
        }

        [ObservableProperty]
        ObservableCollection<JobTimeSheetEntry> jobTimeSheetEntries;

        [RelayCommand]
        async Task Tap(string s)
        {
            await Shell.Current.GoToAsync($"{nameof(JobTimeSheetEntryPage)}");
        }
    }
}