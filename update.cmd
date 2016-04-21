@echo off
NuGet.exe update "Source\MongoDB.Abstracts.sln" -r "Source\packages"
msbuild master.proj /t:refresh