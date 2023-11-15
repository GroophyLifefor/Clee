using System;
using System.Linq;
using Clee.Text;

namespace Clee.CleeWildcards;

public class Debugger : BaseWildcard
{
    public override string WildcardString { get; } = ";debugger";
    public override bool CaseSensitive { get; } = false;
    public override __External __External { get; set; }
    
    public override void OnWildcardStart() { }
    
    public override void OnWildcardFinish() { }

    public override void OnProcess(StringManager stringManager, ModifyWildcard modifyWildcard)
    {
        __External.InvokeLogEvent($"new {this.ToString().Split('.').Last()} as \r\n```clee\r\n{modifyWildcard.Text}\r\n```\r\n");

        string labelGuid = Guid.NewGuid().ToString();
        string commandGuid = "com"+Guid.NewGuid().ToString().Replace("-", "");
        
        modifyWildcard.Replace($"""
REM DEBUGGER START
:{labelGuid}
echo DEBUGGER (Write your commands)
set /p {commandGuid}=
if "%{commandGuid}%" EQU "exit" (goto :{labelGuid}-escape)
%{commandGuid}%
echo.
goto :{labelGuid}
:{labelGuid}-escape
set {commandGuid}=
REM DEBUGGER END
""");
    }
}