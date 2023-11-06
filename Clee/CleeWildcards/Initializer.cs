using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class Initializer : BaseWildcard
{
    public override string WildcardString { get; } = "@init";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        if (modifyWildcard.StartIndex != 0)
        {
            __External.InvokeLogEvent("In \"Initializer\", start position have to be 0.");
        }

        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace($"@echo off & setlocal enabledelayedexpansion");
    }
}