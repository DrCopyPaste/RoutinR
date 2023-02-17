using Microsoft.Extensions.Logging;
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
            mauiAppBuilder.Services.AddTransient<AboutPage>();
            mauiAppBuilder.Services.AddTransient<MainPage>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<AboutPageViewModel>();
            mauiAppBuilder.Services.AddTransient<MainPageViewModel>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<PunchClockService>();
            mauiAppBuilder.Services.AddSingleton<InMemoryDataService>();

            return mauiAppBuilder;
        }
    }
}