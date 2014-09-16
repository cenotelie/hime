#!/bin/sh

# Gather parameters
SKIP_TEST=false
SKIP_SIGN=true
SKIP_PACK=false
if [ "$1" = "--no-test" ] || [ "$2" = "--no-test" ]
  then
    SKIP_TEST=true
fi
if [ "$1" = "--sign" ] || [ "$2" = "--sign" ]
  then
    SKIP_SIGN=false
fi
if [ "$1" = "--no-pack" ] || [ "$2" = "--no-pack" ]
  then
    SKIP_PACK=true
fi

# Gather version info
VERSION=$(sh releng/toolkit/version.sh)
TAG=$(hg log -l 1 --template "{node|short}\n")

echo "Building Hime version $VERSION-$TAG"


# Build the main components
xbuild /p:Configuration=Release /t:Clean runtimes/net/Hime.Redist.csproj 
xbuild /p:Configuration=Release /t:Clean core/Hime.SDK.csproj
xbuild /p:Configuration=Release /t:Clean cli/net/HimeCC.csproj
if [ $SKIP_SIGN = "true" ]
  then
	xbuild /p:Configuration=Release runtimes/net/Hime.Redist.csproj
	xbuild /p:Configuration=Release core/Hime.SDK.csproj
	xbuild /p:Configuration=Release cli/net/HimeCC.csproj
  else
	xbuild /p:Configuration=Release /p:Sign=True runtimes/net/Hime.Redist.csproj
	xbuild /p:Configuration=Release /p:Sign=True core/Hime.SDK.csproj
	xbuild /p:Configuration=Release /p:Sign=True cli/net/HimeCC.csproj
fi

mvn -f runtimes/java/pom.xml clean install -Dgpg.skip=$SKIP_SIGN

if [ $SKIP_TEST != "true" ]
  then
	# Build the test components
	xbuild /p:Configuration=Release tests/driver/Tests.Driver.csproj
	xbuild /p:Configuration=Release tests/net/Tests.Executor.csproj
	mvn -f tests/java/pom.xml clean verify
	# Setup the test components
	mkdir tests/results
	cp tests/driver/bin/Release/Hime.Redist.dll tests/results/Hime.Redist.dll
	cp tests/driver/bin/Release/Hime.CentralDogma.dll tests/results/Hime.CentralDogma.dll
	cp tests/driver/bin/Release/Tests.Driver.exe tests/results/driver.exe
	cp tests/net/bin/Release/Tests.Executor.exe tests/results/executor.exe
	cp tests/java/target/*.jar tests/results/executor.jar
	cp tests/java/target/dependency/*.jar tests/results/
	# Execute the tests
	cd tests/results
	mono driver.exe --targets Net Java
	cd ../..
	# Cleanup the tests
	mv tests/results/TestResults.xml tests/TestResults.xml
	rm -r tests/results
fi

if [ $SKIP_PACK != "true" ]
  then
	# Package
	mkdir hime-$VERSION-$TAG
	cp LICENSE.txt hime-$VERSION-$TAG/README.txt
	cp releng/standalone/README.txt hime-$VERSION-$TAG/README.txt
	cp runtimes/java/target/*.jar hime-$VERSION-$TAG/
	cp runtimes/net/bin/Release/Hime.Redist.dll hime-$VERSION-$TAG/Hime.Redist.dll
	cp runtimes/net/bin/Release/Hime.Redist.XML hime-$VERSION-$TAG/Hime.Redist.xml
	cp core/bin/Release/Hime.CentralDogma.dll hime-$VERSION-$TAG/Hime.CentralDogma.dll
	cp core/bin/Release/Hime.CentralDogma.XML hime-$VERSION-$TAG/Hime.CentralDogma.xml
	cp cli/net/bin/Release/himecc.exe hime-$VERSION-$TAG/himecc.exe
	zip hime-$VERSION-$TAG.zip hime-$VERSION-$TAG/*
	rm -r hime-$VERSION-$TAG
fi
