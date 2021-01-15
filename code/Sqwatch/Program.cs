using CommandLine;
using CommandLine.Text;
using Sqwatch.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Sqwatch
{
    class Program
    {
        static int Main(string[] args)
        {
            var parser = new Parser(config => config.HelpWriter = null);
            var parserResult = parser.ParseArguments<Parameters>(args);
            return parserResult.MapResult(
                parameters => Run(parameters),
                errors => DisplayHelp(parserResult, errors)
                );
        }

        static int DisplayHelp(ParserResult<Parameters> result, IEnumerable<Error> errors)
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

        internal static int Run(Parameters parameters)
        {
            Configuration.ApplyParameters(parameters);

            //todo
            return 0;
        }

        internal static bool ValidateInput(Parameters parameters)
        {
            //todo
            return true;
        }
    }
}
