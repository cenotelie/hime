@ECHO OFF

REM Gather parameters
SET SKIP_TEST=false

IF "%1"=="--no-test" (
	SET SKIP_TEST=true
)

REM Gather version info
FOR /f "delims=" %%a IN ('CALL releng\toolkit\version.bat') DO @SET VERSION=%%a
FOR /f "delims=" %%a IN ('hg log -l 1 --template "{node|short}\n"') DO @SET TAG=%%a

ECHO "Building Hime version %VERSION%-%TAG%"


REM Build the main components
msbuild /p:Configuration=Release /t:Clean runtimes\net\Hime.Redist.csproj
msbuild /p:Configuration=Release /t:Clean core\Hime.SDK.csproj
msbuild /p:Configuration=Release /t:Clean cli\net\HimeCC.csproj
msbuild /p:Configuration=Release runtimes\net\Hime.Redist.csproj
msbuild /p:Configuration=Release core\Hime.SDK.csproj
msbuild /p:Configuration=Release cli\net\HimeCC.csproj
CALL mvn -f runtimes\java\pom.xml clean install -Dgpg.skip=true
IF NOT "%ERRORLEVEL%" == "0" EXIT /b

IF "%SKIP_TEST%"=="false" (
	REM Build the test components
	msbuild /p:Configuration=Release tests\driver\Tests.Driver.csproj
	msbuild /p:Configuration=Release tests\net\Tests.Executor.csproj
	CALL mvn -f tests\java\pom.xml clean verify -Dgpg.skip=true
	IF NOT "%ERRORLEVEL%" == "0" EXIT /b
	REM Setup the test components
	MKDIR tests\results
	COPY /B tests\driver\bin\Release\Hime.Redist.dll tests\results\Hime.Redist.dll > NUL
	COPY /B tests\driver\bin\Release\Hime.CentralDogma.dll tests\results\Hime.CentralDogma.dll > NUL
	COPY /B tests\driver\bin\Release\Tests.Driver.exe tests\results\driver.exe > NUL
	COPY /B tests\net\bin\Release\Tests.Executor.exe tests\results\executor.exe > NUL
	COPY /B tests\java\target\hime-test-executor-%VERSION%.jar tests\results\executor.jar > NUL
	COPY /B tests\java\target\dependency\*.jar tests\results\ > NUL
	REM Execute the tests
	CD tests\results
	driver.exe --targets Net Java
	CD ..\..
	REM Cleanup the tests
	MOVE tests\results\TestResults.xml tests\TestResults.xml > NUL
	RMDIR tests\results /S /Q
)