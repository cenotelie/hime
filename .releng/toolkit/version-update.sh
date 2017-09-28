#!/bin/sh

YEAR=$(date +%Y)

sed -i "s/<version>.*<\/version>/<version>$1.0<\/version>/" runtimes/net/Hime.Redist.nuspec
sed -i "s/<version>.*<\/version>/<version>$1.0<\/version>/" core/Hime.SDK.nuspec
sed -i "s/AssemblyCopyright(\".*\")/AssemblyCopyright(\"Copyright © Association Cénotélie $YEAR\")/" .releng/VersionInfo.cs
sed -i "s/AssemblyVersion(\".*\")/AssemblyVersion(\"$1.0\")/" .releng/VersionInfo.cs
sed -i "s/AssemblyFileVersion(\".*\")/AssemblyFileVersion(\"$1.0\")/" .releng/VersionInfo.cs
sed -i "s/PROJECT_NUMBER         = .*/PROJECT_NUMBER         = $1/" .releng/doxygen.conf
python .releng/toolkit/maven.py runtimes/java/pom.xml $1 $2
python .releng/toolkit/maven.py core/Resources/Java/pom.xml $1 $2
python .releng/toolkit/maven.py tests/java/pom.xml $1 $2
