using CommandLine;
using System;
using System.Reflection;

namespace Sqwatch
{
    class Program
    {
        static int Main(string[] args)
        {
            HelloWorld();

            return Parser.Default.ParseArguments<Parameters>(args)
                .MapResult(parameters => Run(parameters), _ => 1);
        }

        static void HelloWorld()
        {
            Console.WriteLine($"sqwatch {Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine($"Copyright (c) 2021 Matt Perkins");
            Console.WriteLine();
        }

        internal static int Run(Parameters parameters)
        {
            // actual app logic goes here
            return 0;
        }
    }
}
