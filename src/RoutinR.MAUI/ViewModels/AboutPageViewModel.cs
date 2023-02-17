using CommunityToolkit.Mvvm.ComponentModel;
using RoutinR.Services;

namespace RoutinR.MAUI.ViewModels
{
    public partial class AboutPageViewModel: BaseViewModel
    {
        public AboutPageViewModel()
        {
            VersionInfoText = $"Version {ThisAssembly.AssemblyFileVersion} / {ThisAssembly.AssemblyInformationalVersion}";
        }

        [ObservableProperty]
        private string versionInfoText;
    }
}
