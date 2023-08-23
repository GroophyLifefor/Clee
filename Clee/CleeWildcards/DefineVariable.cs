using System;
using System.Linq;
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
        
        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace($"SET {name}={value}\r\n");
    }
}