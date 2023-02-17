using CommunityToolkit.Mvvm.ComponentModel;
using RoutinR.Services;

namespace RoutinR.MAUI.ViewModels
{
    public partial class AboutPageViewModel: BaseViewModel
    {
        private readonly PunchClockService punchClockService;

        public AboutPageViewModel(PunchClockService punchClockService)
        {
            this.punchClockService = punchClockService;

            VersionInfoText = $"Version {ThisAssembly.AssemblyFileVersion} / {ThisAssembly.AssemblyInformationalVersion}";

            this.punchClockService.Start();
            this.punchClockService.Stop();
        }

        [ObservableProperty]
        private string versionInfoText;
    }
}
