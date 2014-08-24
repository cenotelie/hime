#!/bin/sh

YEAR=$(date +%Y)

sed -i "s/AssemblyCopyright(\".*\")/AssemblyCopyright(\"Copyright © $YEAR\")/" releng/VersionInfo.cs
sed -i "s/AssemblyVersion(\".*\")/AssemblyVersion(\"$1.0\")/" releng/VersionInfo.cs
sed -i "s/AssemblyFileVersion(\".*\")/AssemblyFileVersion(\"$1.0\")/" releng/VersionInfo.cs
python releng/toolkit/maven.py version runtimes/java/pom.xml $1
python releng/toolkit/maven.py version core/Resources/Java/pom.xml $1
python releng/toolkit/maven.py version tests/java/pom.xml $1
