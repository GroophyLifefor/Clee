using CommandLine;

namespace Clee.CLI;

public class CommandLineOptions
{
    [Value(0, MetaName = "command", Required = true, HelpText = "The command to execute.")]
    public string Command { get; set; }

    [Option('p', "path", HelpText = "The path to the project.")]
    public string Path { get; set; }
    
    [Option('n', "name", HelpText = "The name of the project.")]
    public string Name { get; set; }

    [Option('v', "version", HelpText = "Show the application version.")]
    public bool Version { get; set; }
}