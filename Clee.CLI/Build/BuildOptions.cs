#nullable enable
namespace Clee.CLI.Build;

public class BuildOptions
{
    public bool? Help { get; set; } = null;
    public bool? Logs { get; set; } = null;
    public string? Path { get; set; } = null;
}