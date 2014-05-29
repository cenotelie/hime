#!/bin/sh

TARGET=hime-$1
mkdir $TARGET

# Copy README
cp releng/standalone/README.txt $TARGET/README.txt
# Copy Java runtime artifacts
cp runtimes/java/target/*.jar $TARGET/
# Copy .Net runtime artifacts
cp runtimes/net/bin/Release/Hime.Redist.dll $TARGET/Hime.Redist.dll
cp runtimes/net/bin/Release/Hime.Redist.xml $TARGET/Hime.Redist.xml
# Copy core SDK artifacts
cp core/bin/Release/Hime.CentralDogma.dll $TARGET/Hime.CentralDogma.dll
cp core/bin/Release/Hime.CentralDogma.xml $TARGET/Hime.CentralDogma.xml
# Copy .Net CLI
cp cli/net/bin/Release/himecc.exe $TARGET/himecc.exe

# Package
zip $TARGET.zip $TARGET/*

# Cleanup
rm -r $TARGET
