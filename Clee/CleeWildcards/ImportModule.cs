using System;
using System.IO;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class ImportModule : BaseWildcard
{
    public override string WildcardString { get; } = "./import([args])";
    public override bool CaseSensitive { get; } = true;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        var args = modifyWildcard.GetValue("args").Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        
        string rawPath = args.Last().Trim('"');

        TryImportFunction(rawPath, args, __External, (isFound, code) =>
        {
            if (isFound)
            {
                __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

                modifyWildcard.Replace(string.Empty);
                stringManager.Replace(stringManager.MaxLength, 0, code + "\r\n");
            }
            else
            {
                __External.InvokeLogEvent($"file not found - {AddExtensionIfNeeded(Path.Combine(__External.LastestFilePath, Path.GetFileName(rawPath)), ".clee")}");
                
                modifyWildcard.Replace(string.Empty);
            }
        });
    }

    public static void TryImportFunction(string rawPath, string[] functionNames, __External external, Action<bool/* isFound */, string?/* code */> action)
    {
        if (TryGetFilePath(rawPath, out var path, external))
        {
            var codeGeneratorInstance = new CodeGeneratorInstance();
            var allowedFunctionNames = new string[functionNames.Length - 1];
            Array.Copy(functionNames, allowedFunctionNames, allowedFunctionNames.Length);
            var code = codeGeneratorInstance.TranspileWithGettingSpecificFunctions(
                Path.GetDirectoryName(path) ?? string.Empty, 
                File.ReadAllText(path).Replace("#Clee:Library", $"REM Clee:Library:{Path.GetFileName(path)}"),
                allowedFunctionNames);

            action.Invoke(true, code);
        }
        else
        {
            action.Invoke(false, null);
        }
    }

    private static bool TryGetFilePath(string path, out string match, __External external)
    {
        path = path.Replace('/', '\\');
        var baseFilePath = AddExtensionIfNeeded(Path.Combine(external.LastestFilePath, path), ".clee");
        var assemblyPath = Path.GetDirectoryName(baseFilePath);
        var assemblyCase = Path.Combine(assemblyPath, path);
        
        if (File.Exists(baseFilePath))
        {
            match = baseFilePath;
            return true;
        }
        
        if (File.Exists(assemblyCase))
        {
            match = assemblyCase;
            return true;
        }
        string[] environmentPaths = Environment.GetEnvironmentVariable("PATH")?.Split(';');

        foreach (string environmentPath in environmentPaths)
        {
            var environmentCase = Path.Combine(environmentPath, path);
            if (File.Exists(environmentCase))
            {
                match = environmentCase;
                return true;
            }
        }

        match = null;
        return false;
    }
    
    static string AddExtensionIfNeeded(string filePath, string newExtension)
    {
        if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
        {
            return Path.ChangeExtension(filePath, newExtension);
        }
        return filePath;
    }
}