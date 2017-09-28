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

xbuild /p:Configuration=Release /t:Clean "$ROOT/runtime-net/Hime.Redist.csproj"
xbuild /p:Configuration=Release "$ROOT/runtime-net/Hime.Redist.csproj"
xbuild /p:Configuration=Release /t:Clean "$ROOT/sdk/Hime.SDK.csproj"
xbuild /p:Configuration=Release "$ROOT/sdk/Hime.SDK.csproj"
xbuild /p:Configuration=Release /t:Clean "$ROOT/himecc/HimeCC.csproj"
xbuild /p:Configuration=Release "$ROOT/himecc/HimeCC.csproj"
mvn -f "$ROOT/runtime-java/pom.xml" clean install -Dgpg.skip=true
xbuild /p:Configuration=Release /t:Clean "$ROOT/utils-demo/Utils.Demo.csproj"
xbuild /p:Configuration=Release "$ROOT/utils-demo/Utils.Demo.csproj"
xbuild /p:Configuration=Release /t:Clean "$ROOT/tests-driver/Tests.Driver.csproj"
xbuild /p:Configuration=Release "$ROOT/tests-driver/Tests.Driver.csproj"
xbuild /p:Configuration=Release /t:Clean "$ROOT/tests-executor-net/Tests.Executor.csproj"
xbuild /p:Configuration=Release "$ROOT/tests-executor-net/Tests.Executor.csproj"
mvn -f "$ROOT/tests-executor-java/pom.xml" clean verify -Dgpg.skip=true

if [ $SKIP_TEST != "true" ]
  then
	# Setup the test components
	mkdir "$ROOT/tests-results"
	cp "$ROOT/tests-driver/bin/Release/Hime.Redist.dll" "$ROOT/tests-results/Hime.Redist.dll"
	cp "$ROOT/tests-driver/bin/Release/Hime.SDK.dll" "$ROOT/tests-results/Hime.SDK.dll"
	cp "$ROOT/tests-driver/bin/Release/Tests.Driver.exe" "$ROOT/tests-results/driver.exe"
	cp "$ROOT/tests-executor-net/bin/Release/Tests.Executor.exe" "$ROOT/tests-results/executor.exe"
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