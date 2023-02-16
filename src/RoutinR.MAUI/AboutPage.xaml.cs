namespace RoutinR.MAUI
{
    public partial class AboutPage : ContentPage
    {
        int count = 0;

        public AboutPage()
        {
            InitializeComponent();
            VersionLabel.Text = $"Version {ThisAssembly.AssemblyFileVersion} / {ThisAssembly.AssemblyInformationalVersion}";
        }
    }
}