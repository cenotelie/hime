#!/bin/sh

# Gather version info
VERSION=$(sh releng/toolkit/version.sh)
TAG=$(hg log -l 1 --template "{node|short}\n")

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