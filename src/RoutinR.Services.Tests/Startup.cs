using Microsoft.Extensions.DependencyInjection;

namespace RoutinR.Services.Tests
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            RoutinR.Startup.Startup.RegisterAppServicesToServiceCollection(services);
        }
    }
}
