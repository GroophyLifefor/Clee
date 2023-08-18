using System;
using Clee.Text;

namespace Clee.CleeWildcards;

public class BaseWildcard
{
    public virtual string WildcardString { get; } = string.Empty;
    public virtual bool CaseSensitive { get; } = false;
    public virtual __External __External { get; set; } = new __External();

    public virtual void OnWildcardStart() { }
    public virtual void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard) { }
    public virtual void OnWildcardFinish() { }
}