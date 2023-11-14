namespace Clee.CLI.Resource;

public class BuildHelp
{
    public static string Get()
        => """
Usage:
  Clee.CLI build [<PROJECT | SOLUTION>...] [options]

Arguments:
  <PROJECT | SOLUTION>  The project or solution file to operate on. If a file is not specified, the command will search 
                        the current directory for one.
""";
}