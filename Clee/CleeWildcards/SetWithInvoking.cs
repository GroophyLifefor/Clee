using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class SetWithInvoking : BaseWildcard
{
    public override string WildcardString { get; } = ";[name]=*./[functionName]([args])";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        string saveAs = '\"' + modifyWildcard.GetValue("name").Trim().Trim('"') + '\"'; 
        string functionName = modifyWildcard.GetValue("functionName");
        string[] args = modifyWildcard.GetValue("args").Split(',').Select(x => x.Trim()).ToArray();
        
        modifyWildcard.Replace($"CALL :{functionName} {string.Join(" ", args)} {saveAs}");
    }
}