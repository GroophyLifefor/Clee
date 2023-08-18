using System;
using Clee.Text;

namespace Clee.CleeWildcards;

public class OperatorSupport : BaseWildcard
{
    public override string WildcardString { get; } = "if*(*\"[first]\" [operator] \"[second]\")";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var @operator = modifyWildcard.GetWildcard("operator");
        var modified = EvalOperator(@operator!.Value);

        modifyWildcard.SetValue("operator", modified);
    }
    
    private string EvalOperator(string @operator)
    {
        return @operator switch
        {
            ">" => "GTR",
            ">=" => "GEQ",
            "<" => "LSS",
            "<=" => "LEQ",
            "==" => "EQU",
            "!=" => "NEQ",
            _ => @operator
        };
    }
}