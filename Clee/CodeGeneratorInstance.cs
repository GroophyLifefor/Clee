#nullable enable
using System;
using Clee.CleeWildcards;
using Clee.CleeWildcards.Comments;
using Clee.CleeWildcards.Define;
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
        _wildcardManager = new WildcardManager(); // -1 :D
        
        _wildcardManager.AddWildcard("functionStructure", new FunctionStructure(), 0);
        
        _wildcardManager.AddWildcard("Initializer", new Initializer(), 1);
        _wildcardManager.AddWildcard("Debugger", new Debugger(), 1);

        _wildcardManager.AddWildcard("MultipleLineComment", new MultipleLineComment(), 2);
        _wildcardManager.AddWildcard("SingleLineComment", new SingleLineComment(), 2);
        _wildcardManager.AddWildcard("DefineWithInvoking", new DefineWithInvoking(), 2);

        _wildcardManager.AddWildcard("ImportModule", new ImportModule(), 3);
        _wildcardManager.AddWildcard("DefineWithInvokingWithSemicolon", new DefineWithInvokingWithSemicolon(), 3);
        _wildcardManager.AddWildcard("SubFunctionInvoking", new SubFunctionInvoking(), 3);

        _wildcardManager.AddWildcard("invokingFunction", new InvokingFunction(), 4);
        
        _wildcardManager.AddWildcard("defineVariable", new DefineVariable(), 5);
        
        _wildcardManager.AddWildcard("DefineVariableWithSemiColon", new DefineVariableWithSemicolon(), 6);
        _wildcardManager.AddWildcard("OperatorSupport", new OperatorSupport(), 6);
    }
    
    internal void InvokeLogEvent(string log)
        => OnLog?.Invoke(log);

    public string Transpile(string path, string cleeCode)
    {
        var logText = $"- Transpiling. {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fffffff")}";
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
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