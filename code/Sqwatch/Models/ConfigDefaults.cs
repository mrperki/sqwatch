using System.Text.Json.Serialization;

namespace Sqwatch.Models
{
    public class ConfigDefaults
    {
        public int MaxExecutionSeconds { get; set; } = 120;
        public int QueryIntervalMs { get; set; } = 1000;
        public bool OutputToConsole { get; set; } = false;
        public bool OutputToFile { get; set; } = true;
        public string FileName { get; set; } = ".\\sqwatch.result.json";
        [JsonConverter(typeof(JsonStringEnumConverter))] public ExistingFileOperation IfFileExists { get; set; }
        public int MaxFileSizeKb { get; set; } = 1024;
    }
}
