using Microsoft.Extensions.DependencyInjection;
using RoutinR.Services;
using RoutinR.Services.Interfaces;
using RoutinR.SQLite.Services;

namespace RoutinR.Startup
{
    public class Startup
    {
        public static void RegisterAppServicesToServiceCollection(IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddSingleton<PunchClockService>();

            //serviceCollection.AddSingleton<IDataService, InMemoryDataService>();
            serviceCollection.AddTransient<IDataService, RoutinRSQLiteService>(x => { return new RoutinRSQLiteService(connectionString); });
        }
    }
}