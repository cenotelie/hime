#!/bin/sh

msbuild /p:Configuration=Release /t:Clean runtimes/net/Hime.Redist.csproj
msbuild /p:Configuration=Release /t:Clean core/Hime.SDK.csproj
msbuild /p:Configuration=Release /p:Sign=True runtimes/net/Hime.Redist.csproj
msbuild /p:Configuration=Release /p:Sign=True core/Hime.SDK.csproj
nuget pack runtimes/net/Hime.Redist.csproj -Build -Symbols -Properties Configuration=Release;Sign=True
nuget pack core/Hime.SDK.csproj -Build -Symbols -Properties Configuration=Release;Sign=True