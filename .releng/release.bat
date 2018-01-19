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
dotnet pack "%ROOT%\runtime-net" -c Release
ECHO -- Building Hime.CLI --
dotnet restore "%ROOT%\cli"
dotnet pack "%ROOT%\cli" -c Release
ECHO -- Building Hime.SDK --
dotnet restore "%ROOT%\sdk"
dotnet pack "%ROOT%\sdk" -c Release
ECHO -- Building HimeCC --
dotnet restore "%ROOT%\himecc"
SET "FrameworkPathOverride=%MONO20%"
dotnet publish "%ROOT%\himecc" -c Release -f net20
SET "FrameworkPathOverride=%MONO461%"
dotnet publish "%ROOT%\himecc" -c Release -f net461
dotnet publish "%ROOT%\himecc" -c Release -f netcoreapp2.0
ECHO -- Building Parsit --
dotnet restore "%ROOT%\parseit"
dotnet publish "%ROOT%\himecc" -c Release -f net20
dotnet publish "%ROOT%\parseit" -c Release -f net20
dotnet publish "%ROOT%\himecc" -c Release -f net461
dotnet publish "%ROOT%\parseit-net" -c Release -f net461
dotnet publish "%ROOT%\parseit-net" -c Release -f netcoreapp2.0
ECHO -- Building Hime Redist for Java --
CALL mvn -f "%ROOT%\runtime-java\pom.xml" clean verify
IF NOT "%ERRORLEVEL%" == "0" EXIT /b
ECHO -- Building Hime Redist for Rust --
cargo build --release --manifest-path "%ROOT%\runtime-rust\Cargo.toml"
cargo package --no-verify --manifest-path "%ROOT%\runtime-rust\Cargo.toml"

ECHO -- Building Package --
RMDIR /S /Q "%RELENG%\hime-%VERSION%"
DEL /F /Q "hime-v%VERSION%.zip"
MKDIR "%RELENG%\hime-%VERSION%"
MKDIR "%RELENG%\hime-%VERSION%\nuget"
MKDIR "%RELENG%\hime-%VERSION%\net20"
MKDIR "%RELENG%\hime-%VERSION%\net461"
MKDIR "%RELENG%\hime-%VERSION%\netcore20"
MKDIR "%RELENG%\hime-%VERSION%\java"
MKDIR "%RELENG%\hime-%VERSION%\rust"
COPY "%ROOT%\LICENSE.txt"                                                          "%RELENG%\hime-%VERSION%\LICENSE.txt"
COPY "%RELENG%\standalone\README.txt"                                              "%RELENG%\hime-%VERSION%\README.txt"
COPY "%RELENG%\standalone\himecc"                                                  "%RELENG%\hime-%VERSION%\himecc"
COPY "%RELENG%\standalone\himecc.bat"                                              "%RELENG%\hime-%VERSION%\himecc.bat"
COPY "%RELENG%\standalone\parseit"                                                 "%RELENG%\hime-%VERSION%\parseit"
COPY "%RELENG%\standalone\parseit.bat"                                             "%RELENG%\hime-%VERSION%\parseit.bat"
COPY "%ROOT%\runtime-net\bin\Release\Hime.Redist.%VERSION%.nupkg"                  "%RELENG%\hime-%VERSION%\nuget\Hime.Redist.%VERSION%.nupkg"
COPY "%ROOT%\runtime-net\bin\Release\Hime.Redist.%VERSION%.symbols.nupkg"          "%RELENG%\hime-%VERSION%\nuget\Hime.Redist.%VERSION%.symbols.nupkg"
COPY "%ROOT%\sdk\bin\Release\Hime.SDK.%VERSION%.nupkg"                             "%RELENG%\hime-%VERSION%\nuget\Hime.SDK.%VERSION%.nupkg"
COPY "%ROOT%\sdk\bin\Release\Hime.SDK.%VERSION%.symbols.nupkg"                     "%RELENG%\hime-%VERSION%\nuget\Hime.SDK.%VERSION%.symbols.nupkg"
COPY "%ROOT%\himecc\bin\Release\net20\publish\Hime.Redist.dll"                     "%RELENG%\hime-%VERSION%\net20\Hime.Redist.dll"
COPY "%ROOT%\himecc\bin\Release\net20\publish\Hime.Redist.pdb"                     "%RELENG%\hime-%VERSION%\net20\Hime.Redist.pdb"
COPY "%ROOT%\himecc\bin\Release\net20\publish\Hime.SDK.dll"                        "%RELENG%\hime-%VERSION%\net20\Hime.SDK.dll"
COPY "%ROOT%\himecc\bin\Release\net20\publish\Hime.SDK.pdb"                        "%RELENG%\hime-%VERSION%\net20\Hime.SDK.pdb"
COPY "%ROOT%\himecc\bin\Release\net20\publish\himecc.exe"                          "%RELENG%\hime-%VERSION%\net20\himecc.exe"
COPY "%ROOT%\himecc\bin\Release\net20\publish\himecc.pdb"                          "%RELENG%\hime-%VERSION%\net20\himecc.pdb"
COPY "%ROOT%\parseit\bin\Release\net20\publish\parseit.exe"                        "%RELENG%\hime-%VERSION%\net20\parseit.exe"
COPY "%ROOT%\parseit\bin\Release\net20\publish\parseit.pdb"                        "%RELENG%\hime-%VERSION%\net20\parseit.pdb"
COPY "%ROOT%\runtime-net\bin\Release\net20\Hime.Redist.xml"                        "%RELENG%\hime-%VERSION%\net20\Hime.Redist.xml"
COPY "%ROOT%\sdk\bin\Release\net20\Hime.SDK.xml"                                   "%RELENG%\hime-%VERSION%\net20\Hime.SDK.xml"
COPY "%ROOT%\himecc\bin\Release\net461\publish\Hime.Redist.dll"                    "%RELENG%\hime-%VERSION%\net461\Hime.Redist.dll"
COPY "%ROOT%\himecc\bin\Release\net461\publish\Hime.Redist.pdb"                    "%RELENG%\hime-%VERSION%\net461\Hime.Redist.pdb"
COPY "%ROOT%\himecc\bin\Release\net461\publish\Hime.SDK.dll"                       "%RELENG%\hime-%VERSION%\net461\Hime.SDK.dll"
COPY "%ROOT%\himecc\bin\Release\net461\publish\Hime.SDK.pdb"                       "%RELENG%\hime-%VERSION%\net461\Hime.SDK.pdb"
COPY "%ROOT%\himecc\bin\Release\net461\publish\himecc.exe"                         "%RELENG%\hime-%VERSION%\net461\himecc.exe"
COPY "%ROOT%\himecc\bin\Release\net461\publish\himecc.pdb"                         "%RELENG%\hime-%VERSION%\net461\himecc.pdb"
COPY "%ROOT%\parseit\bin\Release\net461\publish\parseit.exe"                       "%RELENG%\hime-%VERSION%\net461\parseit.exe"
COPY "%ROOT%\parseit\bin\Release\net461\publish\parseit.pdb"                       "%RELENG%\hime-%VERSION%\net461\parseit.pdb"
COPY "%ROOT%\runtime-net\bin\Release\net20\Hime.Redist.xml"                        "%RELENG%\hime-%VERSION%\net461\Hime.Redist.xml"
COPY "%ROOT%\sdk\bin\Release\net20\Hime.SDK.xml"                                   "%RELENG%\hime-%VERSION%\net461\Hime.SDK.xml"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\System.CodeDom.dll"          "%RELENG%\hime-%VERSION%\netcore20\System.CodeDom.dll"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\Hime.Redist.dll"             "%RELENG%\hime-%VERSION%\netcore20\Hime.Redist.dll"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\Hime.Redist.pdb"             "%RELENG%\hime-%VERSION%\netcore20\Hime.Redist.pdb"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\Hime.SDK.dll"                "%RELENG%\hime-%VERSION%\netcore20\Hime.SDK.dll"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\Hime.SDK.pdb"                "%RELENG%\hime-%VERSION%\netcore20\Hime.SDK.pdb"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\himecc.dll"                  "%RELENG%\hime-%VERSION%\netcore20\himecc.dll"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\himecc.pdb"                  "%RELENG%\hime-%VERSION%\netcore20\himecc.pdb"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\himecc.deps.json"            "%RELENG%\hime-%VERSION%\netcore20\himecc.deps.json"
COPY "%ROOT%\himecc\bin\Release\netcoreapp2.0\publish\himecc.runtimeconfig.json"   "%RELENG%\hime-%VERSION%\netcore20\himecc.runtimeconfig.json"
COPY "%ROOT%\parseit\bin\Release\netcoreapp2.0\publish\parseit.dll"                "%RELENG%\hime-%VERSION%\netcore20\parseit.dll"
COPY "%ROOT%\parseit\bin\Release\netcoreapp2.0\publish\parseit.pdb"                "%RELENG%\hime-%VERSION%\netcore20\parseit.pdb"
COPY "%ROOT%\parseit\bin\Release\netcoreapp2.0\publish\parseit.deps.json"          "%RELENG%\hime-%VERSION%\netcore20\parseit.deps.json"
COPY "%ROOT%\parseit\bin\Release\netcoreapp2.0\publish\parseit.runtimeconfig.json" "%RELENG%\hime-%VERSION%\netcore20\parseit.runtimeconfig.json"
COPY "%ROOT%\runtime-net\bin\Release\netstandard2.0\Hime.Redist.xml"               "%RELENG%\hime-%VERSION%\netcore20\Hime.Redist.xml"
COPY "%ROOT%\sdk\bin\Release\netstandard2.0\Hime.SDK.xml"                          "%RELENG%\hime-%VERSION%\netcore20\Hime.SDK.xml"
COPY %ROOT%\runtime-java\target\*.jar                                              "%RELENG%\hime-%VERSION%\java\"
COPY %ROOT%\runtime-rust\target\package\*.crate                                    "%RELENG%\hime-%VERSION%\rust\"
CD "%RELENG%"
zip -r "hime-v%VERSION%.zip" "hime-%VERSION%"
CD "%ROOT%"
RMDIR /S /Q "%RELENG%\hime-%VERSION%"