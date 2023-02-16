using Microsoft.Maui.Controls.PlatformConfiguration;
using RoutinR.Core;
using RoutinR.Services;

namespace RoutinR.MAUI
{
    public partial class MainPage : ContentPage
    {
        private PunchClockService punchClockService;
        private InMemoryDataService inMemoryDataService;
        private Timer timer = null;

        public MainPage()
        {
            InitializeComponent();
            inMemoryDataService = new InMemoryDataService();
            punchClockService = new PunchClockService();
        }

        private void HandleTimerCallback(object state)
        {
            Application.Current.Dispatcher.DispatchAsync(
                () =>
                {
                    if (!punchClockService.IsRunning) return;
                    PunchClockRunningTime.Text = ((int)DateTime.Now.Subtract(punchClockService.StartTime.Value).TotalSeconds).ToString();
                }
            );
        }

        private void PunchClockButton_Clicked(object sender, EventArgs e)
        {
            if (punchClockService.IsRunning)
            {
                punchClockService.Stop();
                timer.Dispose();

                // var jobTimeSheetEntry = new JobTimeSheetEntry(null, DateTime.Now, DateTime.Now);
                inMemoryDataService.AddJobTimeSheet(Job.NewDefault(), punchClockService.StartTime, punchClockService.EndTime);
            }
            else
            {
                punchClockService.Start();
                PunchClockStartingTime.Text = DateTime.Now.ToString();
                timer = new Timer(HandleTimerCallback, this, 0, 200);
            }

            SemanticScreenReader.Announce("toggled punch clock");
        }
    }
}