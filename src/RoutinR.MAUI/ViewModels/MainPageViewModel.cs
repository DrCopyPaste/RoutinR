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
        }

        public Command PunchClockClick => new(() =>
        {
            if (CurrentlyRunning)
            {
                punchClockService.Stop();
                CurrentlyRunning = false;
                timer.Dispose();

                LastEndTimeText = DateTime.Now.ToString();
                //LastEndTimeText = punchClockService.EndTime.ToString();

                // var jobTimeSheetEntry = new JobTimeSheetEntry(null, DateTime.Now, DateTime.Now);
                // dataService.AddJobTimeSheet(Job.NewDefault(), punchClockService.StartTime, punchClockService.EndTime);
            }
            else
            {
                CurrentlyRunning = true;
                punchClockService.Start();

                // PunchClockStartingTime.Text = DateTime.Now.ToString();
                timer = new Timer(HandleTimerCallback, this, 0, 200);

                LastEndTimeText = "currently running";
                LastStartTimeText = DateTime.Now.ToString();
            }
        });

        private void HandleTimerCallback(object state)
        {
            Application.Current.Dispatcher.DispatchAsync(
                () =>
                {
                    if (!CurrentlyRunning) return;
                    // PunchClockRunningTime.Text = ((int)DateTime.Now.Subtract(punchClockService.StartTime.Value).TotalSeconds).ToString();
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
    }
}
