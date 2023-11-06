using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Spectre.Console;

namespace Clee.CLI.Build;

public class Base
{
    public static void Invoke(CommandLineOptions options, Action beforeClose)
    {
        string path = options.Path ?? Directory.GetCurrentDirectory();

        string[] files = Directory.GetFiles(path, "*.cleeproj").Where(f => f.EndsWith(".cleeproj")).ToArray();

        if (files.Length < 1)
        {
            AnsiConsole.Markup("[red]ERR[/] cleeproj file not found.");
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

        CodeGeneratorInstance codeGenerator = new CodeGeneratorInstance();
        codeGenerator.OnLog += Console.WriteLine;
        var transpiledCode =
            codeGenerator.Transpile(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                File.ReadAllText(projectSettings.EntryFile));

        SafeCreateFile("bin/main.bat", transpiledCode);
        AnsiConsole.Markup("[green]DONE[/] The project \"{0}\" was successfully built.", projectSettings.ProjectName);
        beforeClose();
    }

    private static void SafeCreateFile(string filePath, string contents)
    {
        var combine = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        new FileInfo(combine).Directory!.Create();
        File.WriteAllText(combine, contents);
    }
}