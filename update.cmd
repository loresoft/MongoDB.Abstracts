@echo off
NuGet.exe update "Source\MongoDB.Repository.sln" -r "Source\packages"
msbuild master.proj /t:refresh