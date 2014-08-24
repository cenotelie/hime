#!/bin/sh

# Gather version info
VERSION=1.2.0
TAG=$(hg log -l 1 --template "{node|short}\n")
YEAR=$(date +%Y)
echo "Building Hime version $VERSION-$TAG"


# Prepare .Net components
sed -i "s/AssemblyCopyright(\".*\")/AssemblyCopyright(\"Copyright © $YEAR\")/" releng/VersionInfo.cs
sed -i "s/AssemblyVersion(\".*\")/AssemblyVersion(\"$VERSION.0\")/" releng/VersionInfo.cs
sed -i "s/AssemblyFileVersion(\".*\")/AssemblyFileVersion(\"$VERSION.0\")/" releng/VersionInfo.cs
# Prepare the Java components
python releng/toolkit/maven.py version runtimes/java/pom.xml $VERSION
python releng/toolkit/maven.py version core/Resources/Java/pom.xml $VERSION
python releng/toolkit/maven.py version tests/java/pom.xml $VERSION


# Build the main components
xbuild /p:Configuration=Release runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release core/Hime.SDK.csproj
xbuild /p:Configuration=Release cli/net/HimeCC.csproj
mvn -f runtimes/java/pom.xml clean verify -Dgpg.skip=true


# Build the test components
xbuild /p:Configuration=Release tests/driver/Tests.Driver.csproj
xbuild /p:Configuration=Release tests/net/Tests.Executor.csproj
mvn -f tests/java/pom.xml clean verify
# Setup the test components
mkdir tests/results
cp tests/driver/bin/Release/Hime.Redist.dll tests/results/Hime.Redist.dll
cp tests/driver/bin/Release/Hime.CentralDogma.dll tests/results/Hime.CentralDogma.dll
cp tests/driver/bin/Release/Tests.Driver.exe tests/results/driver.exe
cp tests/net/bin/Release/Tests.Executor.exe tests/results/executor.exe
cp tests/java/target/*.jar tests/results/executor.jar
cp tests/java/target/dependency/*.jar tests/results/
# Execute the tests
cd tests/results
mono driver.exe --targets Net Java
cd ../..
# Cleanup the tests
mv tests/results/TestResults.xml tests/TestResults.xml
rm -r tests/results


# Package
mkdir hime-$VERSION-$TAG
cp releng/standalone/README.txt hime-$VERSION-$TAG/README.txt
cp runtimes/java/target/*.jar hime-$VERSION-$TAG/
cp runtimes/net/bin/Release/Hime.Redist.dll hime-$VERSION-$TAG/Hime.Redist.dll
cp runtimes/net/bin/Release/Hime.Redist.XML hime-$VERSION-$TAG/Hime.Redist.xml
cp core/bin/Release/Hime.CentralDogma.dll hime-$VERSION-$TAG/Hime.CentralDogma.dll
cp core/bin/Release/Hime.CentralDogma.XML hime-$VERSION-$TAG/Hime.CentralDogma.xml
cp cli/net/bin/Release/himecc.exe hime-$VERSION-$TAG/himecc.exe
zip hime-$VERSION-$TAG.zip hime-$VERSION-$TAG/*
rm -r hime-$VERSION-$TAG


# Cleanup
hg revert --no-backup releng/VersionInfo.cs
hg revert --no-backup runtimes/java/pom.xml
hg revert --no-backup core/Resources/Java/pom.xml
hg revert --no-backup tests/java/pom.xml
