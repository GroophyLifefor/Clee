using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CommandLine;
using Spectre.Console;

namespace Clee.CLI;

public static class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(options =>
            {
                if (options.Version)
                {
                    Console.WriteLine("0.22 Clee");
                    Console.WriteLine("0.01 Clee.CLI");
                    BeforeClose();
                    return;
                }
                
                switch (options.Command)
                {
                    case "new":
                        New.Base.Invoke(options, BeforeClose);
                        return;
                    case "build":
                        Build.Base.Invoke(options, BeforeClose);
                        return;
                }

            });
    }

    private static void BeforeClose()
    {
        Console.WriteLine();
    }
}