namespace RoutinR.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ApiExportProfilePage), typeof(ApiExportProfilePage));
            Routing.RegisterRoute(nameof(JobTimeSheetEntryPage), typeof(JobTimeSheetEntryPage));
        }
    }
}