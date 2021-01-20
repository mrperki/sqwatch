using Sqwatch.Models;
using System;

namespace Sqwatch
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        private readonly IConfiguration _config;

        public ConfigurationValidator(IConfiguration config)
        {
            _config = config;
        }

        public string Validate(Parameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
