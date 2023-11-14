using System;
using Spectre.Console;

namespace Clee.CLI;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Help();
            BeforeClose();
            return;
        }

        string command;

        try
        {
            command = args[0];
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            throw;
        }

        switch (command)
        {
            case "new":
                New.Base.Invoke(args, BeforeClose);
                return;
            case "build":
                Build.Base.Invoke(args, BeforeClose);
                return;
            case "-v":
            case "--version":
                Console.WriteLine("0.22 Clee");
                Console.WriteLine("0.01 Clee.CLI");
                BeforeClose();
                break;
        }
    }

    private static void Help()
    {
        AnsiConsole.Markup(Resource.HelpMenu.Get());
    }

    private static void BeforeClose()
    {
        Console.WriteLine();
    }
}