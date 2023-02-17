using Microsoft.Maui.Controls.PlatformConfiguration;
using RoutinR.Core;
using RoutinR.Services;

namespace RoutinR.MAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //private void PunchClockButton_Clicked(object sender, EventArgs e)
        //{
        //    if (punchClockService.IsRunning)
        //    {
        //        punchClockService.Stop();

        //        // var jobTimeSheetEntry = new JobTimeSheetEntry(null, DateTime.Now, DateTime.Now);
        //        dataService.AddJobTimeSheet(Job.NewDefault(), punchClockService.StartTime, punchClockService.EndTime);
        //    }
        //    else
        //    {
        //        punchClockService.Start();
        //        // PunchClockStartingTime.Text = DateTime.Now.ToString();
        //    }

        //    SemanticScreenReader.Announce("toggled punch clock");
        //}
    }
}