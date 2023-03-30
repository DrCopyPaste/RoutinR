using Microsoft.Extensions.DependencyInjection;
using RoutinR.Services;
using RoutinR.Services.Interfaces;

namespace RoutinR.Startup
{
    public class Startup
    {
        public static void RegisterAppServicesToServiceCollection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<PunchClockService>();
            serviceCollection.AddSingleton<IDataService, InMemoryDataService>();
        }
    }
}