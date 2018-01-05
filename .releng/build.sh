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

echo "-- Building Hime.Redist --"
dotnet restore "$ROOT/runtime-net"
(export FrameworkPathOverride="$MONO20"; dotnet build "$ROOT/runtime-net" -c Release)
echo "-- Building Hime.CLI --"
dotnet restore "$ROOT/cli"
(export FrameworkPathOverride="$MONO20"; dotnet build "$ROOT/cli" -c Release)
echo "-- Building Hime.SDK --"
dotnet restore "$ROOT/sdk"
(export FrameworkPathOverride="$MONO20"; dotnet build "$ROOT/sdk" -c Release)
echo "-- Building HimeCC --"
dotnet restore "$ROOT/himecc"
(export FrameworkPathOverride="$MONO461"; dotnet build "$ROOT/himecc" -c Release -f net461)
echo "-- Building Test Executor for .Net --"
dotnet restore "$ROOT/tests-executor-net"
(export FrameworkPathOverride="$MONO461"; dotnet build "$ROOT/tests-executor-net" -c Release -f net461)
echo "-- Building Tests Driver --"
dotnet restore "$ROOT/tests-driver"
(export FrameworkPathOverride="$MONO461"; dotnet build "$ROOT/tests-driver" -c Release -f net461)
echo "-- Building Hime Redist for Java --"
mvn -f "$ROOT/runtime-java/pom.xml" clean install -Dgpg.skip=true
echo "-- Building Test Executor for Java --"
mvn -f "$ROOT/tests-executor-java/pom.xml" clean verify -Dgpg.skip=true
echo "-- Building Hime Redist for Rust --"
cargo test --manifest-path "$ROOT/runtime-rust/Cargo.toml"
echo "-- Building Test Executor for Rust --"
cargo build --release --manifest-path "$ROOT/tests-executor-rust/Cargo.toml"

# Setup the test components
rm -rf "$ROOT/tests-results"
mkdir "$ROOT/tests-results"
cp "$ROOT/tests-driver/bin/Release/net461/Hime.Redist.dll" "$ROOT/tests-results/Hime.Redist.dll"
cp "$ROOT/tests-driver/bin/Release/net461/Hime.CLI.dll" "$ROOT/tests-results/Hime.CLI.dll"
cp "$ROOT/tests-driver/bin/Release/net461/Hime.SDK.dll" "$ROOT/tests-results/Hime.SDK.dll"
cp "$ROOT/tests-driver/bin/Release/net461/driver.exe" "$ROOT/tests-results/driver.exe"
cp "$ROOT/tests-executor-net/bin/Release/net461/executor.exe" "$ROOT/tests-results/executor-net.exe"
cp $ROOT/tests-executor-java/target/hime-test-executor-*.jar "$ROOT/tests-results/executor-java.jar"
cp $ROOT/tests-executor-java/target/dependency/*.jar "$ROOT/tests-results/"
cp "$ROOT/tests-executor-rust/target/release/tests_executor_rust" "$ROOT/tests-results/executor-rust"
# Execute the tests
cd "$ROOT/tests-results"
mono driver.exe --targets Net Java Rust
cd "$ROOT"
# Cleanup the tests
mv "$ROOT/tests-results/TestResults.xml" "$ROOT/TestResults.xml"
rm -r "$ROOT/tests-results"
