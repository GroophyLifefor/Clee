using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class FunctionStructure : BaseWildcard
{
    public override string WildcardString { get; } = "fn[fName]([args])*{-[code]-}";
    public override bool CaseSensitive { get; } = true;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var name = modifyWildcard.GetValue("fName").Trim();
        var args = ParseArgs(modifyWildcard.GetValue("args"));
        var functionCode = modifyWildcard.GetValue("code");
        
        if (!IsAllowedFunction(name))
        {
            modifyWildcard.Replace(string.Empty);
            return;
        }

        var labelParameters = GenerateLabelParameters(args);
        var defineParameters = GenerateDefineParameters(args);
        
        modifyWildcard.Replace(
            $":{name} {AddIfNotEndWith(labelParameters, Environment.NewLine)}" +
            $"{defineParameters}" +
            $"{AddIfNotStartWith(AddIfNotEndWith(functionCode, Environment.NewLine), Environment.NewLine)}" +
            $"GOTO :EOF" +
            $"{Environment.NewLine}" +
            $"{Environment.NewLine}");
    }

    private bool IsAllowedFunction(string fname)
    {
        if (__External.AllowedFunctionNames.Length == 0) return true;
        
        foreach (var t in __External.AllowedFunctionNames)
        {
            if (t.Equals(fname, StringComparison.InvariantCultureIgnoreCase))
                return true;
        }

        return false;
    }

    private static string AddIfNotEndWith(string value, string c)
        => value.EndsWith(c) ? value : value + c;
    
    private static string AddIfNotStartWith(string value, string c)
        => value.StartsWith(c) ? value : c + value;

    private static string[] ParseArgs(string value)
        => value
            .Split(',')                             // split by ','
            .Select(x => x.Trim())                  // remove space characters
            .Where(x => !string.IsNullOrEmpty(x))   // don't get empty args
            .ToArray();

    private static string GenerateLabelParameters(string[] args)
        => args.Length == 0 
            ? "" 
            : string.Join(
                    string.Empty, 
                    args.Select(x => $"<{x}> ")
                    ).Trim();

    private static string GenerateDefineParameters(string[] args, int i = 1)
        => string.Join(
            string.Empty, 
            args.Select(x => $"set {x}=%~{i++}\r\n")
            );
}