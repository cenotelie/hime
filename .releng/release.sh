#!/bin/sh

SCRIPT="$(readlink -f "$0")"
RELENG="$(dirname "$SCRIPT")"
ROOT="$(dirname "$RELENG")"

# Gather version info
VERSION=$(grep "<Version>" "$ROOT/sdk/Hime.SDK.csproj" | grep -o -E "([[:digit:]]+\\.[[:digit:]]+\\.[[:digit:]])+")
HASH=$(hg -R "$ROOT" --debug id -i)

echo "Building Hime version $VERSION ($HASH)"

FrameworkPathOverride=/usr/lib/mono/4.5/

# Build
dotnet restore "$ROOT/runtime-net"
dotnet pack "$ROOT/runtime-net" -c Release
dotnet restore "$ROOT/sdk"
dotnet pack "$ROOT/sdk" -c Release
dotnet restore "$ROOT/himecc"
dotnet pack "$ROOT/himecc" -c Release
mvn -f "$ROOT/runtime-java/pom.xml" clean install

# Build the standalone package
mkdir "$RELENG/hime-$VERSION"
cp "$ROOT/LICENSE.txt" "$RELENG/hime-$VERSION/LICENSE.txt"
cp "$RELENG/standalone/README.txt" "$RELENG/hime-$VERSION/README.txt"
cp "$ROOT/himecc/bin/Release/net461/netstandard.dll" "$RELENG/hime-$VERSION/netstandard.dll"
cp "$ROOT/himecc/bin/Release/net461/System.CodeDom.dll" "$RELENG/hime-$VERSION/System.CodeDom.dll"
cp "$ROOT/himecc/bin/Release/net461/Hime.Redist.dll" "$RELENG/hime-$VERSION/Hime.Redist.dll"
cp "$ROOT/himecc/bin/Release/net461/Hime.SDK.dll" "$RELENG/hime-$VERSION/Hime.SDK.dll"
cp "$ROOT/himecc/bin/Release/net461/himecc.exe" "$RELENG/hime-$VERSION/himecc.exe"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/himecc.dll" "$RELENG/hime-$VERSION/himecc.dll"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/himecc.deps.json" "$RELENG/hime-$VERSION/himecc.deps.json"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/himecc.runtimeconfig.json" "$RELENG/hime-$VERSION/himecc.runtimeconfig.json"
cp $ROOT/runtime-java/target/*.jar "$RELENG/hime-$VERSION/"
zip "$RELENG/hime-$VERSION.zip" $RELENG/hime-$VERSION/*
rm -r "$RELENG/hime-$VERSION"