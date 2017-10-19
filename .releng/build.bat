@ECHO OFF

SET "SCRIPT=%0"
SET "RELENG=%~dp0"
SET "RELENG=%RELENG:~0,-1%"
SET "ROOT=%RELENG:~0,-8%"

FINDSTR "<Version>" "%ROOT%\sdk\Hime.SDK.csproj" > match
FOR /F "tokens=3-5 delims=<>." %%A IN (match) DO @SET "VERSION=%%A.%%B.%%C"
DEL match
FOR /f "delims=" %%a IN ('hg -R "%ROOT%" --debug id -i') DO @SET HASH=%%a

ECHO Building Hime version %VERSION% (%HASH%)

ECHO 1. Checking Mono is installed ...
WHERE "mono" >nul 2>nul
IF %ERRORLEVEL% NEQ 0 (
	ECHO    =^> Mono is not installed!
	EXIT /B 1
)
mono --version > mono1
FINDSTR "version" mono1 > mono2
FOR /F "delims=" %%a IN (mono2) DO @SET MONO=%%a
DEL mono1
DEL mono2
ECHO    =^> Found %MONO%
FOR /f "delims=" %%a IN ('WHERE mono') DO @SET MONOPATH=%%a
SET "MONOPATH=%MONOPATH:~0,-12%"

echo 2. Checking .Net Framework 4.6.1 assemblies are installed ...
SET "MONO461=%MONOPATH%\lib\mono\4.6.1-api"
IF NOT EXIST "%MONO461%\mscorlib.dll" (
  ECHO    =^> Required Mono assemblies for .Net Framework 4.6.1 not found!
  EXIT /B 1
)
ECHO    =^> OK

echo 3. Checking .Net Framework 2.0 assemblies are installed ...
SET "MONO20=%MONOPATH%\lib\mono\2.0-api"
IF NOT EXIST "%MONO20%\mscorlib.dll" (
  ECHO    =^> Required Mono assemblies for .Net Framework 2.0 not found!
  EXIT /B 1
)
ECHO    =^> OK


ECHO -- Building Hime.Redist --
dotnet restore "%ROOT%\runtime-net"
SET "FrameworkPathOverride=%MONO20%"
dotnet build "%ROOT%\runtime-net" -c Release
ECHO -- Building Hime.CLI --
dotnet restore "%ROOT%\cli"
dotnet build "%ROOT%\cli" -c Release
ECHO -- Building Hime.SDK --
dotnet restore "%ROOT%\sdk"
dotnet build "%ROOT%\sdk" -c Release
ECHO -- Building HimeCC --
dotnet restore "%ROOT%\himecc"
SET "FrameworkPathOverride=%MONO461%"
dotnet build "%ROOT%\himecc" -c Release -f net461
ECHO -- Building Test Executor for .Net --
dotnet restore "%ROOT%\tests-executor-net"
dotnet build "%ROOT%\tests-executor-net" -c Release -f net461
ECHO -- Building Tests Driver --
dotnet restore "%ROOT%\tests-driver"
dotnet build "%ROOT%\tests-driver" -c Release -f net461
ECHO -- Building Hime Redist for Java --
CALL mvn -f "%ROOT%\runtime-java\pom.xml" clean install -Dgpg.skip=true
IF NOT "%ERRORLEVEL%" == "0" EXIT /b
ECHO -- Building Test Executor for Java --
CALL mvn -f "%ROOT%\tests-executor-java\pom.xml" clean verify -Dgpg.skip=true
IF NOT "%ERRORLEVEL%" == "0" EXIT /b


REM Setup the test components
RMDIR /S /Q "%ROOT%\tests-results"
MKDIR "%ROOT%\tests-results"
COPY /B "%ROOT%\tests-driver\bin\Release\net461\Hime.Redist.dll" "%ROOT%\tests-results\Hime.Redist.dll"
COPY /B "%ROOT%\tests-driver\bin\Release\net461\Hime.CLI.dll" "%ROOT%\tests-results\Hime.CLI.dll"
COPY /B "%ROOT%\tests-driver\bin\Release\net461\Hime.SDK.dll" "%ROOT%\tests-results\Hime.SDK.dll"
COPY /B "%ROOT%\tests-driver\bin\Release\net461\driver.exe" "%ROOT%\tests-results\driver.exe"
COPY /B "%ROOT%\tests-executor-net\bin\Release\net461\executor.exe" "%ROOT%\tests-results\executor.exe"
COPY /B %ROOT%\tests-executor-java\target\hime-test-executor-*.jar "%ROOT%\tests-results\executor.jar"
COPY /B %ROOT%\tests-executor-java\target\dependency\*.jar "%ROOT%\tests-results\"
REM Execute the tests
CD "%ROOT%/tests-results"
driver.exe --targets Net Java
CD "%ROOT%"
REM Cleanup the tests
MOVE "%ROOT%\tests-results\TestResults.xml" "%ROOT%\TestResults.xml"
RMDIR /S /Q "%ROOT%\tests-results"