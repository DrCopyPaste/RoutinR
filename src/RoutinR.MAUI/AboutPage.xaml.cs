namespace RoutinR.MAUI
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            VersionLabel.Text = $"Version {ThisAssembly.AssemblyFileVersion} / {ThisAssembly.AssemblyInformationalVersion}";
        }
    }
}