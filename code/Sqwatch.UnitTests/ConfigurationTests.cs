using Sqwatch.Models;
using Sqwatch.UnitTests.Mocks;
using Xunit;

namespace Sqwatch.UnitTests
{
    public class ConfigurationTests
    {
        private static readonly ConfigDefaults _defaults = new ConfigDefaults();
        private static readonly ConfigSqlSettings _sqlSettings = new ConfigSqlSettings();

        [Fact]
        public void ApplyParameters_Minimal()
        {
            const string connectionString = "some connection string";
            const string querySql = "select top 1 * from nothing";

            var config = new ConfigBuilder().Build().ToConfiguration();
            config.ApplyParameters(new Parameters
            {
                ConnectionString = connectionString,
                QuerySql = querySql
            });

            Assert.Equal(connectionString, config.ConnectionString);
            Assert.Equal(querySql, config.Query);
            Assert.Equal(_defaults.MaxExecutionSeconds, config.ExecutionTime.TotalSeconds);
            Assert.Equal(_defaults.QueryIntervalMs, config.QueryIntervalMs);
            Assert.Equal(_defaults.OutputToConsole, config.OutputToConsole);
            Assert.Equal(_defaults.OutputToFile, config.OutputToFile);
            Assert.Equal(_defaults.FileName, config.FileName);
            Assert.Equal(_defaults.IfFileExists, config.IfFileExists);
            Assert.Equal(_defaults.MaxFileSizeKb, config.MaxFileSizeKb);
            Assert.Equal(_sqlSettings.QueryTimeoutSeconds, config.QueryTimeoutSeconds);
            Assert.Equal(_sqlSettings.TransactionIsolationLevel, config.TransactionIsolationLevel);
        }
    }
}
