#!/bin/sh

SCRIPT="$(readlink -f "$0")"
RELENG="$(dirname "$SCRIPT")"
ROOT="$(dirname "$RELENG")"

# Gather version info
VERSION=$(grep "<Version>" "$ROOT/sdk/Hime.SDK.csproj" | grep -o -E "([[:digit:]]+\\.[[:digit:]]+\\.[[:digit:]])+")
HASH=$(hg -R "$ROOT" --debug id -i)
MONO=/usr/lib/mono/4.6.1-api/

echo "Building Hime version $VERSION ($HASH)"

# Build
dotnet restore "$ROOT/runtime-net"
dotnet pack "$ROOT/runtime-net" -c Release
dotnet restore "$ROOT/sdk"
dotnet pack "$ROOT/sdk" -c Release
dotnet restore "$ROOT/himecc"
(export FrameworkPathOverride="$MONO"; dotnet pack "$ROOT/himecc" -c Release)
(export FrameworkPathOverride="$MONO"; dotnet publish "$ROOT/himecc" -c Release -f net461)
(export FrameworkPathOverride="$MONO"; dotnet publish "$ROOT/himecc" -c Release -f netcoreapp2.0)
mvn -f "$ROOT/runtime-java/pom.xml" clean install

# Build the standalone package
mkdir "$RELENG/hime-$VERSION"
mkdir "$RELENG/hime-$VERSION/nuget"
mkdir "$RELENG/hime-$VERSION/net461"
mkdir "$RELENG/hime-$VERSION/netcore20"
mkdir "$RELENG/hime-$VERSION/java"
cp "$ROOT/LICENSE.txt"             "$RELENG/hime-$VERSION/LICENSE.txt"
cp "$RELENG/standalone/README.txt" "$RELENG/hime-$VERSION/README.txt"
cp "$RELENG/standalone/himecc"     "$RELENG/hime-$VERSION/himecc"
cp "$ROOT/runtime-net/bin/Release/Hime.Redist.$VERSION.nupkg"         "$RELENG/hime-$VERSION/nuget/Hime.Redist.$VERSION.nupkg"
cp "$ROOT/runtime-net/bin/Release/Hime.Redist.$VERSION.symbols.nupkg" "$RELENG/hime-$VERSION/nuget/Hime.Redist.$VERSION.symbols.nupkg"
cp "$ROOT/sdk/bin/Release/Hime.SDK.$VERSION.nupkg"                    "$RELENG/hime-$VERSION/nuget/Hime.SDK.$VERSION.nupkg"
cp "$ROOT/sdk/bin/Release/Hime.SDK.$VERSION.symbols.nupkg"            "$RELENG/hime-$VERSION/nuget/Hime.SDK.$VERSION.symbols.nupkg"
cp "$ROOT/himecc/bin/Release/net461/publish/netstandard.dll"    "$RELENG/hime-$VERSION/net461/netstandard.dll"
cp "$ROOT/himecc/bin/Release/net461/publish/System.CodeDom.dll" "$RELENG/hime-$VERSION/net461/System.CodeDom.dll"
cp "$ROOT/himecc/bin/Release/net461/publish/Hime.Redist.dll"    "$RELENG/hime-$VERSION/net461/Hime.Redist.dll"
cp "$ROOT/himecc/bin/Release/net461/publish/Hime.SDK.dll"       "$RELENG/hime-$VERSION/net461/Hime.SDK.dll"
cp "$ROOT/himecc/bin/Release/net461/publish/himecc.exe"         "$RELENG/hime-$VERSION/net461/himecc.exe"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/publish/System.CodeDom.dll"        "$RELENG/hime-$VERSION/netcore20/System.CodeDom.dll"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/publish/Hime.Redist.dll"           "$RELENG/hime-$VERSION/netcore20/Hime.Redist.dll"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/publish/Hime.SDK.dll"              "$RELENG/hime-$VERSION/netcore20/Hime.SDK.dll"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/publish/himecc.dll"                "$RELENG/hime-$VERSION/netcore20/himecc.dll"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/publish/himecc.deps.json"          "$RELENG/hime-$VERSION/netcore20/himecc.deps.json"
cp "$ROOT/himecc/bin/Release/netcoreapp2.0/publish/himecc.runtimeconfig.json" "$RELENG/hime-$VERSION/netcore20/himecc.runtimeconfig.json"
cp $ROOT/runtime-java/target/*.jar "$RELENG/hime-$VERSION/java/"
cd "$RELENG"
zip -r "hime-v$VERSION.zip" "hime-$VERSION"
cd "$ROOT"
rm -r "$RELENG/hime-$VERSION"