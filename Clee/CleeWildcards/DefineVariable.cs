using System;
using Clee.Text;

namespace Clee.CleeWildcards;

public class DefineVariable : BaseWildcard
{
    public override string WildcardString { get; } = ";[name]=[value]\r\n";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var name = modifyWildcard.GetValue("name").Trim();
        var value = modifyWildcard.GetValue("value").Trim();
        
        modifyWildcard.Replace($"SET {name}={value}\r\n");
    }
}