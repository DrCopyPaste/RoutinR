using CommunityToolkit.Mvvm.ComponentModel;
using RoutinR.Core;
using RoutinR.Services;

namespace RoutinR.MAUI.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly PunchClockService punchClockService;

        public MainPageViewModel(PunchClockService punchClockService)
        {
            this.punchClockService = punchClockService;

            CurrentlyRunning = false;
            LastStartTimeText = "never started before";
            LastEndTimeText = "never started or stopped before";
            TotalRuntimeText = "0";
        }

        public Command PunchClockClick => new(() =>
        {
            if (CurrentlyRunning)
            {
                punchClockService.Stop();
                CurrentlyRunning = false;
                timer.Dispose();

                LastEndTimeText = punchClockService.EndTimeOrDefault("endtime retrieval error");

                // var jobTimeSheetEntry = new JobTimeSheetEntry(null, DateTime.Now, DateTime.Now);
                // dataService.AddJobTimeSheet(Job.NewDefault(), punchClockService.StartTime, punchClockService.EndTime);
            }
            else
            {
                CurrentlyRunning = true;
                punchClockService.Start();

                timer = new Timer(HandleTimerCallback, this, 0, 20);

                LastEndTimeText = "currently running";
                LastStartTimeText = punchClockService.StartTimeOrDefault("starttime retrieval error");
            }
        });

        private void HandleTimerCallback(object state)
        {
            Application.Current.Dispatcher.DispatchAsync(
                () =>
                {
                    if (!CurrentlyRunning) return;

                    TotalRuntimeText = TimeSpanFormatter.Format(punchClockService.TotalRunTime);
                }
            );
        }

        private Timer timer = null;

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
