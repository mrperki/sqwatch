using Microsoft.Extensions.DependencyInjection;

namespace Sqwatch
{
    partial class Program
    {
        static partial void Registration()
        {
            ServiceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration, Configuration>()
                .AddSingleton<IConfigurationValidator, ConfigurationValidator>()
                .AddSingleton<IConnectionFactory, SqlConnectionFactory>()
                .AddSingleton<IQueryEngine, QueryEngine>()
                .AddSingleton<ILooper, Looper>()
                .BuildServiceProvider();
        }
    }
}
