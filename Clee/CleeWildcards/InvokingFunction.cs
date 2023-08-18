using System;
using System.Linq;
using Clee.Text;
using StringManager = Clee.Text.StringManager;

namespace Clee.CleeWildcards;

public class InvokingFunction : BaseWildcard
{
    public override string WildcardString { get; } = "./[functionName]([args])";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        string functionName = modifyWildcard.GetValue("functionName");
        string[] args = modifyWildcard.GetValue("args").Split(',').Select(x => x.Trim()).ToArray();
        
        modifyWildcard.Replace($"CALL :{functionName} {string.Join(" ", args)}");
    }
}