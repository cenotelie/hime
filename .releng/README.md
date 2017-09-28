# README #

This directory contains various artifacts related to the release engineering of this project.


### Updating the version number ###

The version number of all the released artifacts must be the same and they must be released at the same time.
In this way, it is easy for users to identify the required runtime version for any implementation based on the version used to generate the parsers.

The version number of the artifacts must be updated as follow:

* In `.releng/VersionInfo.cs` modify the `AssemblyVersion` and `AssemblyFileVersion` values. This impacts all the .Net artifacts (.Net runtime, core SDK, CLI, tests, utilities).
* In `runtimes/java/pom.xml` modify the `project/version` element.
* In `core/Resources/Java/pom.xml` modify the `project/dependencies/dependency/version` element.
* In `tests/multi/java/pom.xml` modify:
	* the `project/version` element.
	* the `project/dependencies/dependency/version` element.

This is automatically taken care of by the script `.releng/toolkit/version-update.sh`.
To update to a new version number, run:

```
$ sh .releng/toolkit/version-update.sh X.Y.Z
```



### Releasing Procedure ###

1) Setup the environment (first time):

* Setup the NuGet API key for deployment.
* Setup a GPG key with developer's name and email.
* Setup the Maven confifguration for deployment (Nexus server).

```
$ nuget SetApiKey <key>
```

Configuration of the developer's Maven `settings.xml`:

```
<settings>
  <servers>
    <server>
      <id>ossrh</id>
      <username>your-jira-id</username>
      <password>your-jira-pwd</password>
    </server>
  </servers>
</settings>
```

2) Update the version numbers:
* Run the version update script.
* Commit the changed files with the new version number.
* Add a Mercurial tag vX.Y.Z.

```
$ sh .releng/toolkit/version-update.sh X.Y.Z
```

3) Build and deploy:

```
$ .releng\release.bat
```

or

```
$ sh .releng/release.sh
```

The <name>.symbols.nupkg are automatically pushed to SymbolSource.org when the <name>.nupkg is pushed to NuGet.org

Go to [Sonatype Nexus](https://oss.sonatype.org/) to finalize the repository and push to Maven Central.

Upload the produced standalone package to bitbucket.

Update the links to the download package in the wiki.

4) Build and publish the API documentation