#!/bin/sh

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"

# Gather parameters
SKIP_TEST=false

if [ "$1" = "--no-test" ]
  then
    SKIP_TEST=true
fi

# Gather version info
VERSION=$(sh .releng/toolkit/version.sh)
TAG=$(hg log -l 1 --template "{node|short}\n")

echo "Building Hime version $VERSION-$TAG"

FrameworkPathOverride=/usr/lib/mono/4.5/

dotnet restore "$ROOT/runtime-net"
dotnet pack "$ROOT/runtime-net" -c Release
dotnet restore "$ROOT/sdk"
dotnet pack "$ROOT/sdk" -c Release
dotnet restore "$ROOT/himecc"
dotnet publish "$ROOT/himecc" -c Release -f net461
dotnet publish "$ROOT/himecc" -c Release -f netcoreapp2.0
dotnet restore "$ROOT/tests-executor-net"
dotnet build "$ROOT/tests-executor-net" -c Release
dotnet restore "$ROOT/tests-driver"
dotnet build "$ROOT/tests-driver" -c Release
mvn -f "$ROOT/runtime-java/pom.xml" clean install -Dgpg.skip=true
mvn -f "$ROOT/tests-executor-java/pom.xml" clean verify -Dgpg.skip=true

if [ $SKIP_TEST != "true" ]
  then
	# Setup the test components
	mkdir "$ROOT/tests-results"
	cp "$ROOT/tests-driver/bin/Release/netcoreapp2.0/Hime.Redist.dll" "$ROOT/tests-results/Hime.Redist.dll"
	cp "$ROOT/tests-driver/bin/Release/netcoreapp2.0/Hime.SDK.dll" "$ROOT/tests-results/Hime.SDK.dll"
	cp "$ROOT/tests-driver/bin/Release/netcoreapp2.0/Tests.Driver.dll" "$ROOT/tests-results/driver.dll"
	cp "$ROOT/tests-executor-net/bin/Release/netcoreapp2.0/Tests.Executor.dll" "$ROOT/tests-results/executor.dll"
	cp $ROOT/tests-executor-java/target/hime-test-executor-*.jar "$ROOT/tests-results/executor.jar"
	cp $ROOT/tests-executor-java/target/dependency/*.jar "$ROOT/tests-results/"
	# Execute the tests
	cd "$ROOT/tests-results"
	dotnet driver.dll --targets Net Java
	cd "$ROOT"
	# Cleanup the tests
	mv "$ROOT/tests-results/TestResults.xml" "$ROOT/TestResults.xml"
	rm -r "$ROOT/tests-results"
fi