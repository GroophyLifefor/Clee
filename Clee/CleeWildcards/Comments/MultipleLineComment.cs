using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards.Comments;

public class MultipleLineComment : BaseWildcard
{
    public override string WildcardString { get; } = "/\\*\\*[comment]\\*\\*/";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var comments =  modifyWildcard
            .GetValue("comment")
            .Trim()
            .Split(new []{ "\r", "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(comment => comment.Trim().Trim('*'))
            .ToList();

        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace(string.Join(string.Empty, comments.Select(comment => $"REM {comment}\r\n")));
    }
}