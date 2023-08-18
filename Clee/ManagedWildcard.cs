using Clee.CleeWildcards;
using Clee.Text;

namespace Clee;

public class ManagedWildcard
{
    public string WildcardName { get; set; } = string.Empty;
    public Wildcard[] Wildcard { get; set; }
    public int DeepLevel { get; set; }
    public BaseWildcard BaseWildcard { get; set; }
}