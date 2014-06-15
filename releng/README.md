# README #

This directory contains various artifacts related to the release engineering of this project.


### Updating the version number ###

The version number of all the released artifacts must be the same and they must be released at the same time.
In this way, it is easy for users to identify the required runtime version for any implementation based on the version used to generate the parsers.

The version number of the artifacts must be updated as follow:

* In `releng/VersionInfo.cs` modify the `AssemblyVersion` and `AssemblyFileVersion` values. This impacts all the .Net artifacts (.Net runtime, core SDK, CLI, tests, utilities).
* In `runtimes/java/pom.xml` modify the `project/version` element.
* In `core/Resources/Java/pom.xml` modify the `project/dependencies/dependency/version` element.
* In `tests/multi/java/pom.xml` modify:
	* the `project/version` element.
	* the `project/dependencies/dependency/version` element.



### Releasing ###

1) Update all the version numbers as explained above.

2) Build and release NuGet packages:

```
$ nuget SetApi Key <key>
$ nuget pack runtimes/net/Hime.Redist.csproj -Build -Symbols -Properties Configuration=Release
$ nuget pack core/Hime.SDK.csproj -Build -Symbols -Properties Configuration=Release
$ nuget push runtimes/net/Hime.Redist.nupkg
$ nuget push core/Hime.SDK.nupkg
```

The <name>.symbols.nupkg are automatically pushed to SymbolSource.org when the <name>.nupkg is pushed to NuGet.org

3) Build and release Java runtime

Setup a GPG key with developer's name and email, then

```
$ mvn -f runtimes/java/pom.xml clean deploy
```

Go to [Sonatype Nexus](https://oss.sonatype.org/) to finalize the repository and push to Maven Central.

4) Build and release the standalone package

```
$ sh releng/standalone/package.sh vX.Y.Z
```

The result is the zip hime-vX.Y.Z.zip at the root. Upload the zip to Bitbucket.
