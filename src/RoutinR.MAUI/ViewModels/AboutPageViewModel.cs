using CommunityToolkit.Mvvm.ComponentModel;
using RoutinR.Constants;

namespace RoutinR.MAUI.ViewModels
{
    public partial class AboutPageViewModel: BaseViewModel
    {
        public AboutPageViewModel()
        {
            VersionInfoText = $"Version {ThisAssembly.AssemblyFileVersion} / {ThisAssembly.AssemblyInformationalVersion}";
            ConnectionStringText = Preferences.Default.Get<string>(SettingNames.SettingsDbPath, "Data Source=:memory:");
        }

        [ObservableProperty]
        private string versionInfoText;

        [ObservableProperty]
        private string connectionStringText;
    }
}
