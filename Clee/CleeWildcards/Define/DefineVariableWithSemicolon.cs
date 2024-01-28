using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class DefineVariableWithSemicolon : BaseWildcard
{
    public override string WildcardString { get; } = ";[name]=[value]\r\n";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var name = modifyWildcard.GetValue("name").Trim();
        
        if (name.Contains(' ')) return;
        
        var value = modifyWildcard.GetValue("value").Trim();
        var isArithmetic = IsArithmetic(modifyWildcard.Text);

        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        modifyWildcard.Replace($"SET {(isArithmetic ? "/A" : "")} {name}={value}\r\n");
    }

    static string[] ArithmeticKeys = new[] { "*", "/", "+", "-", "<<", ">>", "^", "*=", "/=", "%=", "+=", "-=", "&=", "^=", "|=", "<<=", ">>=" };
    private static bool IsArithmetic(string value)
        => ArithmeticKeys.Any(key => value.Contains(key));
}