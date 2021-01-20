using Microsoft.Extensions.Configuration;
using Sqwatch.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Sqwatch
{
    public class Configuration : IConfiguration
    {
        private readonly IDictionary<string, string> _namedConnections;
        private readonly IDictionary<string, string> _namedQueries;
        private readonly ConfigDefaults _defaults;
        private readonly ConfigSqlSettings _sqlSettings;
        
        private Parameters _parameters;

        public string ConnectionString { get; private set; }
        public string Query { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }
        public int QueryIntervalMs => (int?)_parameters?.QueryIntervalMs ?? _defaults.QueryIntervalMs;
        public bool OutputToConsole => _parameters?.OutputToConsole ?? _defaults.OutputToConsole;
        public bool OutputToFile => !string.IsNullOrEmpty(_parameters?.OutputToFile) || _defaults.OutputToFile;
        public string FileName => _parameters?.OutputToFile ?? _defaults.FileName;
        public ExistingFileOperation IfFileExists => _parameters?.IfFileExists ?? _defaults.IfFileExists;
        public int QueryTimeoutSeconds => _sqlSettings.QueryTimeoutSeconds;
        public IsolationLevel TransactionIsolationLevel => _sqlSettings.TransactionIsolationLevel;

        private static IConfigurationRoot DefaultConfigRoot()
            => new ConfigurationBuilder()
                .AddJsonFile("sqwatch.config.json", false, false)
                .Build();

        public Configuration() : this(DefaultConfigRoot())
        {
        }

        internal Configuration(IConfigurationRoot configRoot)
        {
            _namedConnections = configRoot.GetSection("namedConnections")
                .Get<List<NamedConnection>>()
                .ToDictionary(c => c.Name, c => c.ConnectionString);

            _namedQueries = configRoot.GetSection("namedQueries")
                .Get<List<NamedQuery>>()
                .ToDictionary(q => q.Name, q => q.Query);

            _defaults = configRoot.GetSection("defaults").Get<ConfigDefaults>();
            _sqlSettings = configRoot.GetSection("sqlSettings").Get<ConfigSqlSettings>();

            ConnectionString = string.Empty;
            Query = string.Empty;
            ExecutionTime = new TimeSpan(0, _defaults.MaxExecutionSeconds, 0);
        }

        public void ApplyParameters(Parameters parameters)
        {
            _parameters = parameters;

            ConnectionString = !string.IsNullOrEmpty(_parameters.ConnectionString)
                ? _parameters.ConnectionString
                : !string.IsNullOrEmpty(_parameters.ConnectionName) && _namedConnections.ContainsKey(_parameters.ConnectionName)
                    ? _namedConnections[_parameters.ConnectionName]
                    : _namedConnections.FirstOrDefault().Value;

            Query = !string.IsNullOrEmpty(_parameters.QuerySql)
                ? _parameters.QuerySql
                : !string.IsNullOrEmpty(_parameters.QueryName) && _namedQueries.ContainsKey(_parameters.QueryName)
                    ? _namedQueries[_parameters.QueryName]
                    : _namedQueries.FirstOrDefault().Value;

            ExecutionTime = _parameters.RunForSeconds.HasValue || _parameters.RunForMinutes.HasValue
                ? new TimeSpan(0, (int?)_parameters.RunForMinutes ?? 0, (int?)_parameters.RunForSeconds ?? 0)
                : new TimeSpan(0, _defaults.MaxExecutionSeconds, 0);
        }
    }
}
