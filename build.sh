#!/bin/sh

# Gather parameters
SKIP_TEST=false

if [ "$1" = "--no-test" ]
  then
    SKIP_TEST=true
fi

# Gather version info
VERSION=$(sh releng/toolkit/version.sh)
TAG=$(hg log -l 1 --template "{node|short}\n")

echo "Building Hime version $VERSION-$TAG"


# Build the main components
xbuild /p:Configuration=Release /t:Clean runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release /t:Clean core/Hime.SDK.csproj
xbuild /p:Configuration=Release /t:Clean cli/net/HimeCC.csproj
xbuild /p:Configuration=Release runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release core/Hime.SDK.csproj
xbuild /p:Configuration=Release cli/net/HimeCC.csproj
mvn -f runtimes/java/pom.xml clean install -Dgpg.skip=true


if [ $SKIP_TEST != "true" ]
  then
	# Build the test components
	xbuild /p:Configuration=Release tests/driver/Tests.Driver.csproj
	xbuild /p:Configuration=Release tests/net/Tests.Executor.csproj
	mvn -f tests/java/pom.xml clean verify -Dgpg.skip=true
	# Setup the test components
	mkdir tests/results
	cp tests/driver/bin/Release/Hime.Redist.dll tests/results/Hime.Redist.dll
	cp tests/driver/bin/Release/Hime.CentralDogma.dll tests/results/Hime.CentralDogma.dll
	cp tests/driver/bin/Release/Tests.Driver.exe tests/results/driver.exe
	cp tests/net/bin/Release/Tests.Executor.exe tests/results/executor.exe
	cp tests/java/target/hime-test-executor-*.jar tests/results/executor.jar
	cp tests/java/target/dependency/*.jar tests/results/
	# Execute the tests
	cd tests/results
	mono driver.exe --targets Net Java
	cd ../..
	# Cleanup the tests
	mv tests/results/TestResults.xml tests/TestResults.xml
	rm -r tests/results
fi