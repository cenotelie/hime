#!/bin/bash

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"

source "$ROOT/build-env.sh"
source "$ROOT/build-checks.sh"

echo "-- Rust components --"
cargo update
cargo build
cargo test
cargo clippy

echo "-- .Net components --"
dotnet restore "$ROOT/runtime-net"
(export FrameworkPathOverride="$MONO20"; dotnet build "$ROOT/runtime-net" -c Release)
(export FrameworkPathOverride="$MONO20"; dotnet pack "$ROOT/runtime-net" -c Release)
dotnet restore "$ROOT/tests-executor-net"
(export FrameworkPathOverride="$MONO461"; dotnet build "$ROOT/tests-executor-net" -c Release -f net461)

echo "-- Java components --"
mvn -f "$ROOT/runtime-java/pom.xml" clean install -Dgpg.skip=true -Duser.home="$HOME"
mvn -f "$ROOT/tests-executor-java/pom.xml" clean verify -Dgpg.skip=true -Duser.home="$HOME"

# Setup the test components
rm -rf "$ROOT/tests-results"
mkdir "$ROOT/tests-results"
cp "$ROOT/target/debug/hime_tests_driver" "$ROOT/tests-results/hime_tests_driver"
cp $ROOT/runtime-net/bin/Release/*.nupkg "$ROOT/tests-results/"
cp "$ROOT/tests-executor-net/bin/Release/net461/Hime.Redist.dll" "$ROOT/tests-results/Hime.Redist.dll"
cp "$ROOT/tests-executor-net/bin/Release/net461/executor.exe" "$ROOT/tests-results/executor-net.exe"
cp $ROOT/tests-executor-java/target/hime-test-executor-*.jar "$ROOT/tests-results/executor-java.jar"
cp $ROOT/tests-executor-java/target/dependency/*.jar "$ROOT/tests-results/"
cp "$ROOT/target/debug/hime_tests_executor_rust" "$ROOT/tests-results/executor-rust"

# Execute the tests
cd "$ROOT/tests-results"
./hime_tests_driver
cd "$ROOT"

# Cleanup the tests
mv "$ROOT/tests-results/TestResults.xml" "$ROOT/TestResults.xml"
rm -r "$ROOT/tests-results"
