@echo off
Nuget.exe restore "Source\MongoDB.Repository.sln"

NuGet.exe install MSBuildTasks -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
NuGet.exe install xunit.runner.console -OutputDirectory .\Tools\ -ExcludeVersion -NonInteractive
