ECHO off

:: Build the driver
MSBuild /p:Configuration=Release tests/driver/Tests.Driver.csproj

:: Build the executors:
MSBuild /p:Configuration=Release tests/net/Tests.Executor.csproj
CALL mvn -f runtimes/java/pom.xml clean install -Dgpg.skip=true
IF not "%ERRORLEVEL%" == "0" EXIT /b
CALL mvn -f tests/java/pom.xml clean package
IF not "%ERRORLEVEL%" == "0" EXIT /b

:: Setup the environment
MKDIR tests\results
MOVE tests\driver\bin\Release\Hime.Redist.dll tests\results\Hime.Redist.dll
MOVE tests\driver\bin\Release\Hime.CentralDogma.dll tests\results\Hime.CentralDogma.dll
MOVE tests\driver\bin\Release\Tests.Driver.exe tests\results\driver.exe
MOVE tests\net\bin\Release\Tests.Executor.exe tests\results\executor.exe
MOVE tests\java\target\*.jar tests\results\executor.jar
MOVE tests\java\target\dependency\*.jar tests\results\

:: Execute
CD tests\results
driver.exe --targets Net Java
CD ..\..

:: Cleanup
MOVE tests\results\TestResults.xml tests\TestResults.xml
RD tests\results