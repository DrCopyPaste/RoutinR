using Microsoft.Extensions.Logging;
using RoutinR.Constants;
using RoutinR.MAUI.Controls;
using RoutinR.MAUI.ViewModels;
using RoutinR.Services;
using CommunityToolkit.Maui;

namespace RoutinR.MAUI
{
    public static class MauiProgram
    {
        public static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nameof(RoutinR) + ".db");
        public static MauiApp CreateMauiApp()
        {
            // Preferences.Default.Clear();
            // replace path with string.Empty for in memory db
            MauiProgram.dbPath = Preferences.Default.Get<string>(SettingNames.SettingsDbPath, dbPath);
            Preferences.Default.Set(SettingNames.SettingsDbPath, MauiProgram.dbPath);
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .RegisterAppServices()
                .RegisterViewModels()
                .RegisterViews()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            //mauiAppBuilder.Services.AddTransient<AboutPage>();
            //mauiAppBuilder.Services.AddTransient<JobsPage>();
            //mauiAppBuilder.Services.AddTransient<MainPage>();
            RegisterViewsToServiceCollection(mauiAppBuilder.Services);
            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            //mauiAppBuilder.Services.AddTransient<AboutPageViewModel>();
            //mauiAppBuilder.Services.AddTransient<JobsPageViewModel>();
            //mauiAppBuilder.Services.AddTransient<MainPageViewModel>();
            RegisterViewModelsToServiceCollection(mauiAppBuilder.Services);
            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            //mauiAppBuilder.Services.AddSingleton<PunchClockService>();
            //mauiAppBuilder.Services.AddSingleton<InMemoryDataService>();
            Startup.Startup.RegisterAppServicesToServiceCollection(mauiAppBuilder.Services, MauiProgram.dbPath);
            return mauiAppBuilder;
        }

        public static void RegisterViewsToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AboutPage>();
            serviceCollection.AddTransient<ApiExportProfilePage>();
            serviceCollection.AddTransient<ApiExportProfilesPage>();
            serviceCollection.AddTransient<JobsPage>();
            serviceCollection.AddTransient<JobTimeSheetEntriesPage>();
            serviceCollection.AddTransient<JobTimeSheetEntryPage>();
            serviceCollection.AddTransient<JobTimeSheetEntryPage>();
            serviceCollection.AddTransient<MainPage>();
            serviceCollection.AddTransient<SettingsPage>();
        }

        public static void RegisterViewModelsToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AboutPageViewModel>();
            serviceCollection.AddTransient<ApiExportProfilePageViewModel>();
            serviceCollection.AddTransient<ApiExportProfilesPageViewModel>();
            serviceCollection.AddTransient<JobsPageViewModel>();
            serviceCollection.AddTransient<JobTimeSheetEntriesPageViewModel>();
            serviceCollection.AddTransient<JobTimeSheetEntryPageViewModel>();
            serviceCollection.AddTransient<MainPageViewModel>();
            serviceCollection.AddTransient<SettingsPageViewModel>();
        }
    }
}