namespace RoutinR.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            // PreferencesService.Reset();
            // PreferencesService.EnsureDefaultConfig();

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}