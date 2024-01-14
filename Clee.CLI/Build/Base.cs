using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using Spectre.Console;

namespace Clee.CLI.Build;

public class Base
{
    public static void Invoke(string[] args, Action beforeClose)
    {
        BuildOptions options = ParseArguments(args);

        if (options.Help ?? false)
        {
            Help(beforeClose);
            return;
        }
        
        string path = options.Path ?? Directory.GetCurrentDirectory();

        string[] files = Directory.GetFiles(path, "*.cleeproj").Where(f => f.EndsWith(".cleeproj")).ToArray();

        if (files.Length < 1)
        {
            AnsiConsole.Markup("[red]ERR[/] cleeproj file not found.\r\n[yellow]INFO[/] Path: " + path);
            beforeClose();
            return;
        }

        if (files.Length > 1)
        {
            AnsiConsole.Markup("[red]ERR[/] many cleeproj file found.");
            beforeClose();
            return;
        }

        var xmlString = File.ReadAllText(files.First());
        ProjectSettings projectSettings;

        using (StringReader stringReader = new StringReader(xmlString))
        {
            projectSettings = (ProjectSettings)new XmlSerializer(typeof(ProjectSettings)).Deserialize(stringReader);
        }

        double ms = -1;
        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Building...", _ => 
            {
                CodeGeneratorInstance codeGenerator = new CodeGeneratorInstance();
                
                // --logs option
                if (options.Logs ?? false)
                {
                    codeGenerator.OnLog += Console.WriteLine;
                }
                
                var transpiledCode =
                    codeGenerator.Transpile(
                        path,
                        File.ReadAllText(Path.Combine(path, projectSettings.EntryFile)),
                        out ms);

                SafeCreateFile("bin/main.bat", transpiledCode);
            });

        AnsiConsole.Markup("[green]DONE[/] The project \"{0}\" was successfully built in {1}ms.", projectSettings.ProjectName, ms);
        beforeClose();
    }

    private static void Help(Action beforeClose)
    {
        Console.WriteLine(Resource.BuildHelp.Get());
        beforeClose();
    }

    private static BuildOptions ParseArguments(string[] args)
    {
        BuildOptions options = new BuildOptions();

        for (int i = 1; i < args.Length; i++)
        {
            var arg = args[i];

            switch (arg)
            {
                case "-h":
                case "--help":
                    options.Help = true;
                    break;
                case "-l":
                case "--logs":
                    options.Logs = true;
                    break;
                case "-p":
                case "--path":
                    if (args.Length > i+1)
                    {
                        i++;
                        options.Path = args[i];
                    }
                    else
                    {
                        AnsiConsole.WriteException(new ArgumentNullException(nameof(options.Path)));
                    }
                    break;
                default:
                    if (options.Path is null)
                    {
                        AnsiConsole.WriteException(new ArgumentNullException(nameof(options.Path)));
                    }
                    else
                    {
                        options.Path = arg;
                    }
                    break;
            }
        }

        return options;
    }

    private static void SafeCreateFile(string filePath, string contents)
    {
        var combine = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        new FileInfo(combine).Directory!.Create();
        File.WriteAllText(combine, contents);
    }
}