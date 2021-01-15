using System.Data;
using System.Text.Json.Serialization;

namespace Sqwatch.Models
{
    public class ConfigSqlSettings
    {
        public int QueryTimeoutSeconds { get; set; } = 60;
        [JsonConverter(typeof(JsonStringEnumConverter))] public IsolationLevel TransactionIsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
    }
}
