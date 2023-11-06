using System;
using System.IO;
using System.Xml.Serialization;
using Spectre.Console;

namespace Clee.CLI.New;

public class Base
{
    public static void Invoke(CommandLineOptions options, Action beforeClose)
    {
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

        AnsiConsole.Markup("[green]DONE[/] The project \"{0}\" was created successfully.", name);
        beforeClose();
    }

    private static void SafeCreateFile(bool isNamed, string name, string filePath, string contents)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),
            isNamed ? Path.Combine(name, filePath) : filePath);

        new FileInfo(path).Directory!.Create();
        File.WriteAllText(path, contents);
    }
}