namespace Clee.CLI.Resource;

public class HelpMenu
{
    public static string Get()
        => """
Usage: Clee.CLI [[command]] [[path-to-application]] [[arguments]]

Execute a Clee application.

path-to-application:
  The path to an application .cleeproj file to apply.

sdk-options:
  -h|--help         Show command line help.
  --version         Display Clee.CLI version in use.

SDK commands:
  build             Build a Clee project.
  new               Create a new Clee project or file.

Run 'Clee.CLI [[command]] --help' for more information on a command.
Run '[Blue]Clee.CLI new --name "learnClee"[/]' for a quick start

""";
}