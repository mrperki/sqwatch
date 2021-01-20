using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.DependencyInjection;
using Sqwatch.Models;
using System;
using System.Reflection;

namespace Sqwatch
{
    partial class Program
    {
        internal static IServiceProvider ServiceProvider;

        static int Main(string[] args)
        {
            Registration();            
            
            var parser = new Parser(config => config.HelpWriter = null);
            var parserResult = parser.ParseArguments<Parameters>(args);

            return parserResult.MapResult(
                parameters => Run(parameters),
                errors => DisplayHelp(parserResult)
                );
        }

        internal static int Run(Parameters parameters)
        {
            var result = ServiceProvider.GetRequiredService<IConfigurationValidator>()
                .Validate(parameters);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
                return 1;
            }

            ServiceProvider.GetRequiredService<IConfiguration>().ApplyParameters(parameters);

            //todo
            return 0;
        }

        static int DisplayHelp(ParserResult<Parameters> result)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.Heading = $"sqwatch {Assembly.GetExecutingAssembly().GetName().Version}";
                h.Copyright = "Copyright (c) 2021 Matt Perkins";
                h.AddNewLineBetweenHelpSections = true;
                h.AddEnumValuesToHelpText = true;
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);

            return 1;
        }

        static partial void Registration();
    }
}
