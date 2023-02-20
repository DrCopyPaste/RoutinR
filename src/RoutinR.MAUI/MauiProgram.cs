﻿using Microsoft.Extensions.Logging;
using RoutinR.MAUI.ViewModels;
using RoutinR.Services;

namespace RoutinR.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterAppServices()
                .RegisterViewModels()
                .RegisterViews()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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

            Startup.Startup.RegisterAppServicesToServiceCollection(mauiAppBuilder.Services);
            return mauiAppBuilder;
        }

        public static void RegisterViewsToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AboutPage>();
            serviceCollection.AddTransient<JobsPage>();
            serviceCollection.AddTransient<JobTimeSheetEntriesPage>();
            serviceCollection.AddTransient<JobTimeSheetEntryPage>();
            serviceCollection.AddTransient<MainPage>();
        }

        public static void RegisterViewModelsToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AboutPageViewModel>();
            serviceCollection.AddTransient<JobsPageViewModel>();
            serviceCollection.AddTransient<JobTimeSheetEntriesPageViewModel>();
            serviceCollection.AddTransient<MainPageViewModel>();
        }
    }
}