using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class DefineWithInvokingWithSemicolon : BaseWildcard
{
    public override string WildcardString { get; } = ";[name]=*./[functionName]([args])";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        if (modifyWildcard.WildcardIndexes[0].Value.Contains('\n') || modifyWildcard.WildcardIndexes[1].Value.Contains('\n'))
        {
            modifyWildcard.RevertWithIncreaseIndex(1);
            return;
        };

        string saveAs = '\"' + modifyWildcard.GetValue("name").Trim().Trim('"') + '\"'; 
        string functionName = modifyWildcard.GetValue("functionName");
        string[] args = modifyWildcard.GetValue("args").Split(',').Select(x => x.Trim()).ToArray();

        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace($"CALL :{functionName} {string.Join(" ", args)} {saveAs}");
    }
}