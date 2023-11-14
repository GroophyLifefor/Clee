namespace Clee.CLI.Resource;

public class NewHelp
{
    public static string Get()
        => """
Usage:
  Clee.CLI new [command] [options]

Options:
  -n, --name <name>        The name for the output being created. If no name is specified, the name of the output directory is used.
  -h, --help               Show command line help.
""";
}