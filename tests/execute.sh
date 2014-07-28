#!/bin/sh

# Build the driver
xbuild /p:Configuration=Release tests/driver/Tests.Driver.csproj

# Build the executors:
xbuild /p:Configuration=Release tests/net/Tests.Executor.csproj
mvn -f runtimes/java/pom.xml clean install -Dgpg.skip=true
mvn -f tests/java/pom.xml clean package

# Setup the environment
mkdir tests/results
mv tests/driver/bin/Release/Hime.Redist.dll tests/results/Hime.Redist.dll
mv tests/driver/bin/Release/Hime.CentralDogma.dll tests/results/Hime.CentralDogma.dll
mv tests/driver/bin/Release/Tests.Driver.exe tests/results/driver.exe
mv tests/net/bin/Release/Tests.Executor.exe tests/results/executor.exe
mv tests/java/target/*.jar tests/results/executor.jar
mv tests/java/target/dependency/*.jar tests/results/

# Execute
cd tests/results
mono driver.exe --targets $*
cd ../../..

# Cleanup
mv tests/results/TestResults.xml tests/TestResults.xml
rm -r tests/results
