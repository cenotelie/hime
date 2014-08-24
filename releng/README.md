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

This is automatically taken care of by the script `releng/toolkit/version-update.sh`.
To update to a new version number, run:

```
$ sh releng/toolkit/version-update.sh X.Y.Z
```



### Releasing ###

1) Update the version numbers with:

```
$ sh releng/toolkit/version-update.sh X.Y.Z
```

2) Execute the build process using PGP signing. This also produce the standalone package.

```
$ sh build.sh --sign
```

3) Building and releasing the NuGet packages:

```
$ nuget SetApi Key <key>
$ nuget pack runtimes/net/Hime.Redist.csproj -Build -Symbols -Properties Configuration=Release
$ nuget pack core/Hime.SDK.csproj -Build -Symbols -Properties Configuration=Release
$ nuget push runtimes/net/Hime.Redist.nupkg
$ nuget push core/Hime.SDK.nupkg
```

The <name>.symbols.nupkg are automatically pushed to SymbolSource.org when the <name>.nupkg is pushed to NuGet.org

4) Release Java runtime

Setup a GPG key with developer's name and email, then

```
$ mvn -f runtimes/java/pom.xml deploy
```

Go to [Sonatype Nexus](https://oss.sonatype.org/) to finalize the repository and push to Maven Central.
