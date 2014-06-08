#!/bin/sh

# Build the driver
xbuild /p:Configuration=Release tests/multi/driver/Tests.Driver.csproj

# Build the executors
xbuild /p:Configuration=Release tests/multi/net/Tests.Executor.csproj


# Setup the environment
mkdir tests/multi/results
mv tests/multi/driver/bin/Release/Hime.Redist.dll tests/multi/results/Hime.Redist.dll
mv tests/multi/driver/bin/Release/Hime.CentralDogma.dll tests/multi/results/Hime.CentralDogma.dll
mv tests/multi/driver/bin/Release/Tests.Driver.exe tests/multi/results/driver.exe
mv tests/multi/net/bin/Release/Tests.Executor.exe tests/multi/results/executor.exe


# Execute
cd tests/multi/results
mono driver.exe
