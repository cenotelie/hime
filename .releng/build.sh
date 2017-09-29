#!/bin/sh

SCRIPT="$(readlink -f "$0")"
RELENG="$(dirname "$SCRIPT")"
ROOT="$(dirname "$RELENG")"

# Gather version info
VERSION=$(grep "<Version>" "$ROOT/sdk/Hime.SDK.csproj" | grep -o -E "([[:digit:]]+\\.[[:digit:]]+\\.[[:digit:]])+")
HASH=$(hg -R "$ROOT" --debug id -i)
MONO=/usr/lib/mono/4.5/

echo "Building Hime version $VERSION ($HASH)"

# Build
dotnet restore "$ROOT/runtime-net"
dotnet pack "$ROOT/runtime-net" -c Release
dotnet restore "$ROOT/sdk"
dotnet pack "$ROOT/sdk" -c Release
dotnet restore "$ROOT/himecc"
(export FrameworkPathOverride="$MONO"; dotnet pack "$ROOT/himecc" -c Release)
dotnet restore "$ROOT/tests-executor-net"
(export FrameworkPathOverride="$MONO"; dotnet publish "$ROOT/tests-executor-net" -c Release -f net461)
dotnet restore "$ROOT/tests-driver"
(export FrameworkPathOverride="$MONO"; dotnet publish "$ROOT/tests-driver" -c Release -f net461)
mvn -f "$ROOT/runtime-java/pom.xml" clean install -Dgpg.skip=true
mvn -f "$ROOT/tests-executor-java/pom.xml" clean verify -Dgpg.skip=true

# Setup the test components
rm -rf "$ROOT/tests-results"
mkdir "$ROOT/tests-results"
cp "$ROOT/tests-driver/bin/Release/net461/netstandard.dll" "$ROOT/tests-results/netstandard.dll"
cp "$ROOT/tests-driver/bin/Release/net461/System.CodeDom.dll" "$ROOT/tests-results/System.CodeDom.dll"
cp "$ROOT/tests-driver/bin/Release/net461/Hime.Redist.dll" "$ROOT/tests-results/Hime.Redist.dll"
cp "$ROOT/tests-driver/bin/Release/net461/Hime.SDK.dll" "$ROOT/tests-results/Hime.SDK.dll"
cp "$ROOT/tests-driver/bin/Release/net461/driver.exe" "$ROOT/tests-results/driver.exe"
cp "$ROOT/tests-executor-net/bin/Release/net461/executor.exe" "$ROOT/tests-results/executor.exe"
cp $ROOT/tests-executor-java/target/hime-test-executor-*.jar "$ROOT/tests-results/executor.jar"
cp $ROOT/tests-executor-java/target/dependency/*.jar "$ROOT/tests-results/"
# Execute the tests
cd "$ROOT/tests-results"
mono driver.exe --targets Net Java
cd "$ROOT"
# Cleanup the tests
mv "$ROOT/tests-results/TestResults.xml" "$ROOT/TestResults.xml"
rm -r "$ROOT/tests-results"