using Microsoft.Extensions.DependencyInjection;
using Sqwatch.UnitTests.Mocks;

namespace Sqwatch.UnitTests
{
    public class UnitTestBase
    {
        protected readonly IServiceCollection Services = new ServiceCollection();
        protected IConfiguration Configuration { get; private set; }
        protected MockLooper MockLooper { get; } = new MockLooper();

        protected void InitConfiguration(Configuration config)
        {
            Configuration = config;
            Services.AddSingleton(Configuration);
        }

        protected void UseMockLooper() => Services.AddSingleton<ILooper>(MockLooper);
    }
}
