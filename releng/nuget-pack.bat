nuget pack Lib.Redist\Lib.Redist.csproj -OutputDirectory releng -Build -Symbols -Properties Configuration=Release
nuget pack Lib.CentralDogma\Lib.CentralDogma.csproj -OutputDirectory releng -Build -Symbols -IncludeReferencedProjects -Properties Configuration=Release
nuget push releng\Hime.Redist.1.0.0.0.nupkg
nuget push releng\Hime.SDK.1.0.0.0.nupkg