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
dotnet pack "$ROOT/himecc" -c Release
dotnet restore "$ROOT/tests-executor-net"
dotnet publish "$ROOT/tests-executor-net" -c Release -f net461
dotnet restore "$ROOT/tests-driver"
dotnet publish "$ROOT/tests-driver" -c Release -f net461
mvn -f "$ROOT/runtime-java/pom.xml" clean install -Dgpg.skip=true
mvn -f "$ROOT/tests-executor-java/pom.xml" clean verify -Dgpg.skip=true

if [ $SKIP_TEST != "true" ]
  then
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
fi