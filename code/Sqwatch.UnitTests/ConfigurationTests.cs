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

        [Fact]
        public void ApplyParameters_NamedConnection()
        {
            const string connectionString1 = "connection string1";
            const string connectionString2 = "connectionString 2";
            const string querySql = "select top 1 * from nothing";

            var config = new ConfigBuilder().Build()
                .AddNamedConnection("c1", connectionString1)
                .AddNamedConnection("c2", connectionString2)
                .ToConfiguration();
            config.ApplyParameters(new Parameters
            {
                ConnectionName = "c1",
                QuerySql = querySql
            });

            Assert.Equal(connectionString1, config.ConnectionString);
            Assert.Equal(querySql, config.Query);
        }

        [Fact]
        public void ApplyParameters_NamedQuery()
        {
            const string connectionString = "some connection string";
            const string query1 = "select Id from table1";
            const string query2 = "select col from table2";
            const string query3 = "select column from tableThree";

            var config = new ConfigBuilder().Build()
                .AddNamedQuery("q1", query1)
                .AddNamedQuery("q2", query2)
                .AddNamedQuery("q3", query3)
                .ToConfiguration();
            config.ApplyParameters(new Parameters
            {
                ConnectionString = connectionString,
                QueryName = "q2"
            });

            Assert.Equal(connectionString, config.ConnectionString);
            Assert.Equal(query2, config.Query);
        }
        
        [Theory]
        [InlineData(null, 30, 30)]
        [InlineData(0, 30, 30)]
        [InlineData(1, null, 60)]
        [InlineData(5, 20, 320)]
        public void ApplyParameters_ExecutionTimeParameters(int? minutes, int? seconds, int expectedSeconds)
        {
            const string connectionString = "some connection string";
            const string querySql = "select Id from table1";

            var config = new ConfigBuilder().Build().ToConfiguration();
            config.ApplyParameters(new Parameters
            {
                ConnectionString = connectionString,
                QuerySql = querySql,
                RunForMinutes = (uint?)minutes,
                RunForSeconds = (uint?)seconds
            });

            Assert.Equal(connectionString, config.ConnectionString);
            Assert.Equal(querySql, config.Query);
            Assert.Equal(expectedSeconds, config.ExecutionTime.TotalSeconds);
        }
    }
}

