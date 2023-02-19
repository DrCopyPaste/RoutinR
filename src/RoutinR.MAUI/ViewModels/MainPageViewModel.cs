﻿using CommunityToolkit.Mvvm.ComponentModel;
using RoutinR.Constants;
using RoutinR.Core;
using RoutinR.Services;

namespace RoutinR.MAUI.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly InMemoryDataService dataService;
        private readonly PunchClockService punchClockService;

        public MainPageViewModel(PunchClockService punchClockService, InMemoryDataService dataService)
        {
            this.dataService = dataService;
            this.punchClockService = punchClockService;

            if (Preferences.Default.ContainsKey(SettingNames.PreviousStartTime))
            {
                var restoredStartTime = Preferences.Default.Get(SettingNames.PreviousStartTime, DateTime.MinValue);

                punchClockService.StartFrom(restoredStartTime);
                LastStartTimeText = restoredStartTime.ToString();
                CurrentlyRunning = true;
                Timer = new Timer(HandleTimerCallback, this, 0, 20);
                LastEndTimeText = "currently running";

                return;
            }

            CurrentlyRunning = false;
            LastStartTimeText = "never started before";
            LastEndTimeText = "never started or stopped before";
            TotalRuntimeText = "0";
        }

        public Command PunchClockClick => new(() =>
        {
            if (CurrentlyRunning)
            {
                var jobTimeSheetEntry = punchClockService.Stop();
                Preferences.Default.Remove(SettingNames.PreviousStartTime);

                CurrentlyRunning = false;
                Timer.Dispose();
                LastEndTimeText = jobTimeSheetEntry.EndTime.ToString();

                dataService.AddJobTimeSheetEntry(jobTimeSheetEntry);
            }
            else
            {
                var previousStartTime = punchClockService.Start();

                // save last start time to preferences to restore it in case of a restart
                Preferences.Default.Set(SettingNames.PreviousStartTime, previousStartTime);

                CurrentlyRunning = true;
                Timer = new Timer(HandleTimerCallback, this, 0, 20);
                LastEndTimeText = "currently running";
                LastStartTimeText = previousStartTime.ToString();
            }
        });

        private void HandleTimerCallback(object state)
        {
            Application.Current.Dispatcher.DispatchAsync(
                () =>
                {
                    if (Paused) return;
                    if (!CurrentlyRunning) return;

                    TotalRuntimeText = TimeSpanFormatter.Format(punchClockService.TotalRunTime);
                }
            );
        }

        public bool Paused { get; set; } = false;
        public Timer Timer { get; private set; }

        [ObservableProperty]
        private Job currentJob;

        [ObservableProperty]
        private bool currentlyRunning;

        [ObservableProperty]
        private string lastStartTimeText;

        [ObservableProperty]
        private string lastEndTimeText;

        [ObservableProperty]
        private string totalRuntimeText;
    }
}
