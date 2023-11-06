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
        string path;
        if (TryGetFilePath(rawPath, out path))
        {
            var codeGeneratorInstance = new CodeGeneratorInstance();
            var allowedFuncs = new string[args.Length - 1];
            Array.Copy(args, allowedFuncs, allowedFuncs.Length);
            var code = codeGeneratorInstance.TranspileWithGettingSpecificFunctions(
                Path.GetDirectoryName(path), 
                File.ReadAllText(path).Replace("#Clee:Library", $"REM Clee:Library:{Path.GetFileName(path)}"),
                allowedFuncs);
            
            __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

            modifyWildcard.Replace(string.Empty);
            stringManager.Replace(stringManager.MaxLength, 0, code + "\r\n");
        }
        else
        {
            modifyWildcard.Replace(string.Empty);
            __External.InvokeLogEvent($"file not found - {AddExtensionIfNeeded(Path.Combine(__External.LastestFilePath, Path.GetFileName(rawPath)), ".clee")}");
        }
    }

    private bool TryGetFilePath(string path, out string match)
    {
        path = path.Replace('/', '\\');
        var baseFilePath = AddExtensionIfNeeded(Path.Combine(__External.LastestFilePath, path), ".clee");
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