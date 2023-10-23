using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards.Comments;

public class SingleLineComment : BaseWildcard
{
    public override string WildcardString { get; } = "//[comment]\r\n";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var beforeChar = stringManager.Text.Substring(modifyWildcard.StartIndex - 1, 1);

        if (!Char.IsWhiteSpace(beforeChar, 0))
            return;

        string comment =  modifyWildcard.GetValue("comment").Trim();

        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace($"REM {comment}\r\n");
    }
}