#!/bin/sh

SCRIPT="$(readlink -f "$0")"
RELENG="$(dirname "$SCRIPT")"
ROOT="$(dirname "$RELENG")"

# Gather version info
VERSION=$(grep "<Version>" "$ROOT/sdk/Hime.SDK.csproj" | grep -o -E "([[:digit:]]+\\.[[:digit:]]+\\.[[:digit:]])+")
HASH=$(hg -R "$ROOT" --debug id -i)

echo "Building Hime version $VERSION ($HASH)"

echo "1. Checking Mono is installed ..."
MONO=$(which mono)
if [ -z "$MONO" ]
then
  echo "   => Mono is not installed!"
  exit 1
fi
MONO=$(mono --version | grep 'version')
echo "   => Found $MONO"

echo "2. Checking .Net Framework 4.6.1 assemblies are installed ..."
MONO461=/usr/lib/mono/4.6.1-api
if [ ! -f "$MONO461/mscorlib.dll" ]; then
  echo "   => Required Mono assemblies for .Net Framework 4.6.1 not found!"
  exit 1
fi
echo "   => OK"

echo "3. Checking .Net Framework 2.0 assemblies are installed ..."
MONO20=/usr/lib/mono/2.0-api
if [ ! -f "$MONO20/mscorlib.dll" ]; then
  echo "   => Required Mono assemblies for .Net Framework 2.0 not found!"
  exit 1
fi
echo "   => OK"

# Build
echo "-- Building Hime.Redist --"
dotnet restore "$ROOT/runtime-net"
(export FrameworkPathOverride="$MONO20"; dotnet pack "$ROOT/runtime-net" -c Release)
echo "-- Building Hime.SDK --"
dotnet restore "$ROOT/sdk"
(export FrameworkPathOverride="$MONO20"; dotnet pack "$ROOT/sdk" -c Release)
echo "-- Building HimeCC --"
dotnet restore "$ROOT/himecc"
(export FrameworkPathOverride="$MONO20"; dotnet publish "$ROOT/himecc" -c Release -f net20)
(export FrameworkPathOverride="$MONO461"; dotnet publish "$ROOT/himecc" -c Release -f net461)
dotnet publish "$ROOT/himecc" -c Release -f netcoreapp2.0
echo "-- Building Hime Redist for Java --"
mvn -f "$ROOT/runtime-java/pom.xml" clean install

echo "-- Building Package --"
# Build the standalone package
mkdir "$RELENG/hime-$VERSION"
mkdir "$RELENG/hime-$VERSION/nuget"
mkdir "$RELENG/hime-$VERSION/net20"
mkdir "$RELENG/hime-$VERSION/net461"
mkdir "$RELENG/hime-$VERSION/netcore20"
mkdir "$RELENG/hime-$VERSION/java"
cp "$ROOT/LICENSE.txt"                                                        "$RELENG/hime-$VERSION/LICENSE.txt"
cp "$RELENG/standalone/README.txt"                                            "$RELENG/hime-$VERSION/README.txt"
cp "$RELENG/standalone/himecc"                                                "$RELENG/hime-$VERSION/himecc"
cp "$ROOT/runtime-net/bin/Release/Hime.Redist.$VERSION.nupkg"                 "$RELENG/hime-$VERSION/nuget/Hime.Redist.$VERSION.nupkg"
cp "$ROOT/runtime-net/bin/Release/Hime.Redist.$VERSION.symbols.nupkg"         "$RELENG/hime-$VERSION/nuget/Hime.Redist.$VERSION.symbols.nupkg"
cp "$ROOT/sdk/bin/Release/Hime.SDK.$VERSION.nupkg"                            "$RELENG/hime-$VERSION/nuget/Hime.SDK.$VERSION.nupkg"
cp "$ROOT/sdk/bin/Release/Hime.SDK.$VERSION.symbols.nupkg"                    "$RELENG/hime-$VERSION/nuget/Hime.SDK.$VERSION.symbols.nupkg"
cp "$ROOT/himecc/bin/Release/net20/publish/Hime.Redist.dll"                   "$RELENG/hime-$VERSION/net20/Hime.Redist.dll"
cp "$ROOT/himecc/bin/Release/net20/publish/Hime.SDK.dll"                      "$RELENG/hime-$VERSION/net20/Hime.SDK.dll"
cp "$ROOT/himecc/bin/Release/net20/publish/himecc.exe"                        "$RELENG/hime-$VERSION/net20/himecc.exe"
cp "$ROOT/himecc/bin/Release/net461/publish/Hime.Redist.dll"                  "$RELENG/hime-$VERSION/net461/Hime.Redist.dll"
cp "$ROOT/himecc/bin/Release/net461/publish/Hime.SDK.dll"                     "$RELENG/hime-$VERSION/net461/Hime.SDK.dll"
cp "$ROOT/himecc/bin/Release/net461/publish/himecc.exe"                       "$RELENG/hime-$VERSION/net461/himecc.exe"
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