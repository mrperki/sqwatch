using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sqwatch.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sqwatch.UnitTests.Mocks
{
    public class ConfigBuilder
    {
        private ConfigDefaults _defaults;
        private ConfigSqlSettings _sqlSettings;
        private readonly List<NamedConnection> _namedConnections = new List<NamedConnection>();
        private readonly List<NamedQuery> _namedQueries = new List<NamedQuery>();

        public ConfigBuilder Build() => Build(new ConfigDefaults(), new ConfigSqlSettings());
        public ConfigBuilder Build(ConfigDefaults defaults) => Build(defaults, new ConfigSqlSettings());
        public ConfigBuilder Build(ConfigSqlSettings sqlSettings) => Build(new ConfigDefaults(), sqlSettings);
        public ConfigBuilder Build(ConfigDefaults defaults, ConfigSqlSettings sqlSettings)
        {
            _defaults = defaults;
            _sqlSettings = sqlSettings;
            return this;
        }

        public ConfigBuilder AddNamedConnection(NamedConnection namedConnection)
        {
            _namedConnections.Add(namedConnection);
            return this;
        }

        public ConfigBuilder AddNamedQuery(NamedQuery namedQuery)
        {
            _namedQueries.Add(namedQuery);
            return this;
        }

        public IConfigurationRoot ToConfigurationRoot()
        {
            var json = JsonConvert.SerializeObject(new
            {
                namedConnections = _namedConnections,
                namedQueries = _namedQueries,
                defaults = _defaults,
                sqlSettings = _sqlSettings
            });
            return new ConfigurationBuilder()
                .AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(json)))
                .Build();
        }

        public IConfiguration ToConfiguration()
        {
            return new Configuration(ToConfigurationRoot());
        }
    }
}
