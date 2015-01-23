#!/bin/sh

# Gather version info
VERSION=$(sh releng/toolkit/version.sh)
TAG=$(hg log -l 1 --template "{node|short}\n")

echo "Building Hime version $VERSION-$TAG"

# Rebuild the .Net artifacts
xbuild /p:Configuration=Release /t:Clean runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release /t:Clean core/Hime.SDK.csproj
xbuild /p:Configuration=Release /t:Clean cli/net/HimeCC.csproj
xbuild /p:Configuration=Release /p:Sign=True runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release /p:Sign=True core/Hime.SDK.csproj
xbuild /p:Configuration=Release /p:Sign=True cli/net/HimeCC.csproj

# Build the NuGet packages and push them
nuget pack runtimes/net/Hime.Redist.csproj -Build -Symbols -Properties Configuration=Release;Sign=True
nuget pack core/Hime.SDK.csproj -Build -Symbols -Properties Configuration=Release;Sign=True
nuget push  Hime.Redist.$VERSION.0.nupkg
nuget push  Hime.SDK.$VERSION.0.nupkg

# Rebuild the Java artifacts and push
mvn -f runtimes/java/pom.xml clean deploy

# Build the standalone package
mkdir hime-$VERSION-$TAG
cp LICENSE.txt hime-$VERSION-$TAG/README.txt
cp releng/standalone/README.txt hime-$VERSION-$TAG/README.txt
cp runtimes/java/target/*.jar hime-$VERSION-$TAG/
cp runtimes/net/bin/Release/Hime.Redist.dll hime-$VERSION-$TAG/Hime.Redist.dll
cp runtimes/net/bin/Release/Hime.Redist.XML hime-$VERSION-$TAG/Hime.Redist.xml
cp core/bin/Release/Hime.CentralDogma.dll hime-$VERSION-$TAG/Hime.CentralDogma.dll
cp core/bin/Release/Hime.CentralDogma.XML hime-$VERSION-$TAG/Hime.CentralDogma.xml
cp cli/net/bin/Release/himecc.exe hime-$VERSION-$TAG/himecc.exe
zip hime-$VERSION-$TAG.zip hime-$VERSION-$TAG/*
rm -r hime-$VERSION-$TAG