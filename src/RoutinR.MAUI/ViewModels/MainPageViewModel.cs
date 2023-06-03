using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RoutinR.Constants;
using RoutinR.Core;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
using System.Collections.ObjectModel;

namespace RoutinR.MAUI.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly ExportService exportService;
        private readonly IDataService dataService;
        private readonly PunchClockService punchClockService;

        public MainPageViewModel(PunchClockService punchClockService, IDataService dataService, ExportService exportService)
        {
            this.dataService = dataService;
            this.exportService = exportService;
            this.punchClockService = punchClockService;

            Jobs = new ObservableCollection<Job>();
            RefreshEntriesAndSelectJob();
            RestoreCurrentJob();

            
            previousEndTime = Preferences.Default.Get(SettingNames.PreviousEndTime, DateTime.Now);
            if (!Preferences.Default.ContainsKey(SettingNames.PreviousEndTime)) Preferences.Default.Set(SettingNames.PreviousEndTime, previousEndTime);

            Timer = new Timer(HandleTimerCallback, this, 0, 20);

            if (Preferences.Default.ContainsKey(SettingNames.PreviousStartTime))
            {
                var restoredStartTime = Preferences.Default.Get(SettingNames.PreviousStartTime, DateTime.MinValue);

                punchClockService.StartFrom(restoredStartTime);
                CurrentlyRunning = true;
                PunchClockLabel = "Stop";

                return;
            }

            CurrentlyRunning = false;
            TotalRuntimeText = "0";
            PunchClockLabel = "Start";
            this.exportService = exportService;
        }

        private void RestoreCurrentJob()
        {
            if (!Preferences.Default.ContainsKey(SettingNames.CurrentJobName))
            {
                return;
            }

            var defaultJob = Jobs.First();
            
            var restoredJobName = Preferences.Default.Get(SettingNames.CurrentJobName, defaultJob.Name);
            var jobFromDb = this.dataService.GetJobByName(restoredJobName);
            if (jobFromDb == null)
            {
                // if restored job does not exist in db, restore preference setting to default
                Preferences.Default.Set(SettingNames.CurrentJobName, defaultJob.Name);
                CurrentJob = defaultJob;
            }
            else
            {
                CurrentJob = jobFromDb;
            }
        }

        [RelayCommand]
        async Task PunchClockClick()
        {
            if (CurrentlyRunning)
            {
                previousEndTime = DateTime.Now;
                Preferences.Default.Set(SettingNames.PreviousEndTime, previousEndTime);

                var jobTimeSheetEntry = punchClockService.Stop(CurrentJob);
                Preferences.Default.Remove(SettingNames.PreviousStartTime);

                CurrentlyRunning = false;
                Timer.Dispose();
                PunchClockLabel = "Start";

                dataService.AddJobTimeSheetEntry(jobTimeSheetEntry);

                if (Preferences.Default.Get(SettingNames.ExportOnTimeSheetCompletion, false))
                {
                    try
                    {
                        var apiProfiles = dataService.GetApiExportProfiles().Where(profile => profile.JobTemplates.Any(template => template.Key.Name.Equals(CurrentJob.Name)));
                        foreach (var profile in apiProfiles)
                        {
                            var result = await exportService.ExportToApi(jobTimeSheetEntry, profile, new HttpClient());
                            if (result != null && result.HasError)
                            {
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                var previousStartTime = punchClockService.Start();

                // save last start time to preferences to restore it in case of a restart
                Preferences.Default.Set(SettingNames.PreviousStartTime, previousStartTime);
                Preferences.Default.Remove(SettingNames.PreviousEndTime);

                CurrentlyRunning = true;
                Timer = new Timer(HandleTimerCallback, this, 0, 20);
                PunchClockLabel = "Stop";
            }
        }

        private void HandleTimerCallback(object state)
        {
            Application.Current.Dispatcher.DispatchAsync(
                () =>
                {
                    var prefix = string.Empty;

                    if (Paused)
                    {
                        TotalRuntimeText = "paused ";
                        return;
                    }

                    if (!CurrentlyRunning)
                    {
                        TotalRuntimeText = $"idle for {TimeSpanFormatter.Format(DateTime.Now.Subtract(previousEndTime))}";
                        return;
                    }

                    TotalRuntimeText = TimeSpanFormatter.Format(punchClockService.TotalRunTime);
                }
            );
        }

        public void RefreshEntriesAndSelectJob()
        {
            var lastJobName = CurrentJob == null ? Job.NewDefault().Name : CurrentJob.Name;

            Jobs.Clear();
            foreach (var job in this.dataService.GetJobs()) Jobs.Add(job);

            var jobFromDb = Jobs.FirstOrDefault(job => job.Name == lastJobName);
            if (jobFromDb != null)
            {
                CurrentJob = jobFromDb;
                return;
            }

            CurrentJob = Job.NewDefault();
        }

        public bool Paused { get; set; } = false;
        public Timer Timer { get; private set; }

        [ObservableProperty]
        private ObservableCollection<Job> jobs;

        [ObservableProperty]
        private Job currentJob;

        [ObservableProperty]
        private bool currentlyRunning;

        [ObservableProperty]
        private string totalRuntimeText;

        [ObservableProperty]
        string punchClockLabel;

        private DateTime previousEndTime;
    }
}
