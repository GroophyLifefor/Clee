#nullable enable
using System;
using System.Diagnostics;
using Clee.CleeWildcards;
using Clee.Text;

namespace Clee;

public class CodeGeneratorInstance
{
    private WildcardManager _wildcardManager;
    public delegate void OnLogDelegate(string log);
    public event OnLogDelegate? OnLog;
    public string LastestFilePath { get; set; } = string.Empty;

    public CodeGeneratorInstance()
    {
        _wildcardManager = new WildcardManager();
        
        _wildcardManager.addWildcard("functionStructure", new FunctionStructure(), 0);
        
        _wildcardManager.addWildcard("ImportModule", new ImportModule(), 1);
        _wildcardManager.addWildcard("SetWithInvoking", new SetWithInvoking(), 1);
        _wildcardManager.addWildcard("SubFunctionInvoking", new SubFunctionInvoking(), 1);

        _wildcardManager.addWildcard("invokingFunction", new InvokingFunction(), 2);
        
        _wildcardManager.addWildcard("defineVariable", new DefineVariable(), 3);
        _wildcardManager.addWildcard("OperatorSupport", new OperatorSupport(), 3);
    }
    
    internal void InvokeLogEvent(string log)
        => OnLog?.Invoke(log);

    public string Transpile(string path, string cleeCode)
    {
        var logText = $"- Transpiling. {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fffffff")}";
        var stopwatch = Stopwatch.StartNew();
        InvokeLogEvent($"{new string('-', logText.Length)}\r\n{logText}");

        LastestFilePath = path;
        cleeCode = cleeCode.Replace("\r\n", "ͳ").Replace("\n", "\r\n").Replace("ͳ", "\r\n");
        var stringManager = _wildcardManager.ApplyWildcards(this, new StringManager(cleeCode));
        InvokeLogEvent($"- Transpile end within {stopwatch.Elapsed.TotalMilliseconds}ms.\r\n{new string('-', logText.Length)}");
        return stringManager.Text.Trim();
    }
    
    public string TranspileWithGettingSpecificFunctions(string path, string cleeCode, string[] functionNames)
    {
        var logText = $"- Transpiling. {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fffffff")}";
        InvokeLogEvent($"{new string('-', logText.Length)}\r\n{logText}\r\n{new string('-', logText.Length)}");

        LastestFilePath = path;
        cleeCode = cleeCode.Replace("\r\n", "ͳ").Replace("\n", "\r\n").Replace("ͳ", "\r\n");
        var stringManager = _wildcardManager.ApplyWildcards(this, new StringManager(cleeCode), functionNames);
        return string.Join("\r\n", stringManager.Text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)).Trim();
    }
    
    
}