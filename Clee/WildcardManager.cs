using System;
using System.Collections.Generic;
using Clee.CleeWildcards;
using Clee.Text;

namespace Clee;

public class WildcardManager
{
    private List<ManagedWildcard> _wildcards { get; } = new();

    public void AddWildcard(string wildcardName, BaseWildcard baseWildcard, int deepLevel)
    {
        var wildcardString = baseWildcard.WildcardString;
        var wildcard = StringManager.ExportWildcards(wildcardString);
        var managedWildcard = new ManagedWildcard()
        {
            WildcardName = wildcardName,
            Wildcard = wildcard,
            DeepLevel = deepLevel,
            BaseWildcard = baseWildcard
        };

        _wildcards.Add(managedWildcard);
        _wildcards.Sort((i, j) => i.DeepLevel.CompareTo(j.DeepLevel));
    }

    public StringManager ApplyWildcards(CodeGeneratorInstance instance, StringManager stringManager, string[] functionNames = null)
    {
        foreach (var managedWildcard in _wildcards)
        {
            managedWildcard.BaseWildcard.__External = new ()
            {
                InvokeLogEvent = instance.InvokeLogEvent,
                LastestFilePath = instance.LastestFilePath,
                AllowedFunctionNames = functionNames ?? Array.Empty<string>()
            };
            managedWildcard.BaseWildcard.OnWildcardStart();
            stringManager.ApplyWildcards(
                managedWildcard.Wildcard, 
                modifyWildcard => managedWildcard.BaseWildcard.OnProcess(stringManager, modifyWildcard),
                managedWildcard.BaseWildcard.CaseSensitive,
                managedWildcard.WildcardName);
            managedWildcard.BaseWildcard.OnWildcardFinish();
        }

        return stringManager;
    }
}