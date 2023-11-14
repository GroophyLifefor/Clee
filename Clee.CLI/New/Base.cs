using System;
using System.IO;
using Spectre.Console;

namespace Clee.CLI.New;

public class Base
{
    public static void Invoke(string[] args, Action beforeClose)
    {
        NewOptions options = ParseArguments(args);

        if (options.Help ?? false)
        {
            Help(beforeClose);
            return;
        }
        
        bool isNamed = !string.IsNullOrEmpty(options.Name);
        var name = isNamed ? options.Name : "CleeProject";

        SafeCreateFile(
            isNamed,
            name,
            $"{name}.cleeproj",
            Resource.ProjectSettingsXML.Get(name)
        );

        SafeCreateFile(
            isNamed,
            name,
            "main.clee",
            Resource.CleeHelloWorld.Get()
        );

        AnsiConsole.Markup($"""
[green]DONE[/] The project \"{0}\" was created successfully.

  [invert] CD {options.Name} [/]
  [invert] Clee.CLI build [/]
  [invert] CALL bin/main.bat [/]

""", name);
        beforeClose();
    }

    private static void Help(Action beforeClose)
    {
        Console.WriteLine(Resource.NewHelp.Get());
        beforeClose();
    }

    private static NewOptions ParseArguments(string[] args)
    {
        NewOptions options = new NewOptions();

        for (int i = 1; i < args.Length; i++)
        {
            var arg = args[i];

            switch (arg)
            {
                case "-h":
                case "--help":
                    options.Help = true;
                    break;
                case "-n":
                case "--name":
                    i++;
                    options.Name = args[i];
                    break;
            }
        }

        return options;
    }

    private static void SafeCreateFile(bool isNamed, string name, string filePath, string contents)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),
            isNamed ? Path.Combine(name, filePath) : filePath);

        new FileInfo(path).Directory!.Create();
        File.WriteAllText(path, contents);
    }
}