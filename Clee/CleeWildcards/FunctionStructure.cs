﻿using System;
using System.Collections.Generic;
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
        
        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        var isSaveAsCustomized = args.Count(x => x.DisplayName == "saveAs") > 0;
        var labelParameters = GenerateLabelParameters(args, isSaveAsCustomized);
        var defineParameters = GenerateDefineParameters(args, isSaveAsCustomized: isSaveAsCustomized);
        var disposeParameters = GenerateDisposeParameters(args, isSaveAsCustomized);
        
        modifyWildcard.Replace(
            $":{name} {AddIfNotEndWith(labelParameters, Environment.NewLine)}" +
            $"{defineParameters}" +
            $"{AddIfNotStartWith(AddIfNotEndWith(functionCode, Environment.NewLine), Environment.NewLine)}" +
            $"{disposeParameters}" +
            $"GOTO :EOF" +
            $"{Environment.NewLine}" +
            $"{Environment.NewLine}");
    }

    private bool IsAllowedFunction(string fname)
    {
        if (__External.AllowedFunctionNames.Length == 0) return true;

        if (__External.AllowedFunctionNames.Length == 1
            && __External.AllowedFunctionNames.First().Equals("*"))
            return true;
            
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

    private List<Parameter> ParseArgs(string value)
    {
        return value.Split(',').Where(x => !string.IsNullOrEmpty(x.Trim())).Select(x =>
        {
            var doublePointIndex = x.IndexOf(':');
            return doublePointIndex != -1 ? new Parameter()
            {
                DisplayName = x.Substring(0, doublePointIndex).Trim(),
                DefineName = x.Substring(doublePointIndex + 1).Trim()
            } : new Parameter()
            {
                DisplayName = x.Trim(),
                DefineName = x.Trim()
            };
        }).ToList();
    }

    private static string GenerateLabelParameters(List<Parameter> args, bool isSaveAsCustomized )
        => args.Count == 0 
            ? "" 
            : string.Join(
                    string.Empty, 
                    args.Select(x => $"<{x.DisplayName}> ")
                    ).Trim() +
              (isSaveAsCustomized ? "" : $" <saveAs [If Needed]>");

    private static string GenerateDefineParameters(List<Parameter> args, int i = 1, bool isSaveAsCustomized = false)
        => string.Join(
            string.Empty, 
            args.Select(x => $"set {x.DefineName}=%~{i++}\r\n")
            ) + 
           (isSaveAsCustomized ? "" : $"set saveAs=%~{i++}\r\n");

    private string GenerateDisposeParameters(List<Parameter> args, bool isSaveAsCustomized)
        => string.Join(
               string.Empty,
               args.Select(x => $"set {x.DefineName}=\r\n")
           ) +
           (isSaveAsCustomized ? "" : $"set saveAs=\r\n");
}

internal class Parameter
{
    public string DisplayName { get; set; }
    public string DefineName { get; set; }
}