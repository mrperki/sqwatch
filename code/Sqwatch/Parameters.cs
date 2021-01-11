using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace Sqwatch
{
    internal enum ExistingFileOperation
    {
        Fail,
        Append,
        Overwrite
    }

    internal class Parameters
    {
        [Option('n', "connectionname", Group = "connection", HelpText = "The named connection string from sqwatch.config to use for the database connection")]
        public string ConnectionName { get; set; }

        [Option('c', "connectionstring", Group = "connection", HelpText = "The connection string to use for the database connection")]
        public string ConnectionString { get; set; }

        [Option('m', "queryname", Group = "query", HelpText = "The named SQL query from sqwatch.config to execute")]
        public string QueryName { get; set; }

        [Option('q', "query", Group = "query", HelpText = "The SQL query string to execute")]
        public string QuerySql { get; set; }

        [Option('s', "seconds", Group = "time", HelpText = "Run the process for this many seconds before terminating")]
        public uint? RunForSeconds { get; set; }

        [Option('m', "minutes", Group = "time", HelpText = "Run the process for this many minutes before terminating")]
        public uint? RunForMinutes { get; set; }

        [Option('t', "diffcount", HelpText = "Run the process until this many changes have been recorded before terminating")]
        public uint? DiffCount { get; set; }

        [Option('i', "queryinterval", Default = 1000, HelpText = "Interval in milliseconds between query executions")]
        public uint? QueryIntervalMs { get; set; }

        [Option('c', "console", Default = false, HelpText = "Output the results of the query to the console")]
        public bool OutputToConsole { get; set; }

        [Option('f', "file", Default = null, HelpText = "Output the results of the query to the given file handle. If empty, uses defaults.outputFile from sqwatch.config")]
        public string OutputToFile { get; set; }

        [Option('k', "maxfilesize", HelpText = "End the process once the output file reaches this size, in kb")]
        public uint? MaxFileSizeKb { get; set; }

        [Option('e', "fileexists", Default = ExistingFileOperation.Fail, HelpText = "If the output file exists, perform this action")]
        public ExistingFileOperation IfFileExists { get; set; }

        [Usage(ApplicationAlias = "sqwatch")]
        public static IEnumerable<Example> Examples => new List<Example>
        {
            new Example("Simple query and connection string with console output", new Parameters
            {
                ConnectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;",
                QuerySql = "select top 1 * from tableA",
                OutputToConsole = true
            }),
            new Example("Named query and connection string with file output, runs for 60 seconds", new Parameters
            {
                ConnectionName = "myConnection",
                QueryName = "myFavouriteQuery",
                RunForSeconds = 60,
                OutputToFile = "myfile.json",
                IfFileExists = ExistingFileOperation.Append
            })
        };
    }
}
