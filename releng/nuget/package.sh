#!/bin/sh

OUTPUT=releng/nuget

nuget pack runtimes/net/Hime.Redist.csproj -OutputDirectory $OUTPUT -Build -Symbols -Properties Configuration=Release
nuget pack core/Hime.SDK.csproj -OutputDirectory $OUTPUT -Build -Symbols -IncludeReferencedProjects -Properties Configuration=Release
