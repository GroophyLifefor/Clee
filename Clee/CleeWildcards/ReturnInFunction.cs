using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class ReturnInFunction : BaseWildcard
{
    public override string WildcardString { get; } = "./return([args])";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        string returnValue = modifyWildcard.GetValue("args").Trim();
        
        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace($"set !saveAs!={returnValue}");
    }
}