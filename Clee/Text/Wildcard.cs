using System;

namespace Clee.Text;

public class Wildcard
{
    public bool IsStatic { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int MaxLength { get; set; } = -1;
    public bool DoNotAcceptSpace { get; set; }
    public Func<string, bool> OnIteratorQuery { get; set; } = _ => true;
}