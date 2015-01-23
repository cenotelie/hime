@ECHO OFF

REM Gather version info
FOR /f "delims=" %%a IN ('CALL releng\toolkit\version.bat') DO @SET VERSION=%%a
FOR /f "delims=" %%a IN ('hg log -l 1 --template "{node|short}\n"') DO @SET TAG=%%a

ECHO "Building Hime version %VERSION%-%TAG%"

REM Rebuild the .Net artifacts
msbuild /p:Configuration=Release /t:Clean runtimes\net\Hime.Redist.csproj
msbuild /p:Configuration=Release /t:Clean core\Hime.SDK.csproj
msbuild /p:Configuration=Release /t:Clean cli\net\HimeCC.csproj
msbuild /p:Configuration=Release /p:Sign=True runtimes\net\Hime.Redist.csproj
msbuild /p:Configuration=Release /p:Sign=True core\Hime.SDK.csproj
msbuild /p:Configuration=Release /p:Sign=True cli\net\HimeCC.csproj

REM Build the NuGet packages and push them
nuget pack runtimes\net\Hime.Redist.csproj -Build -Symbols -Properties Configuration=Release;Sign=True
nuget pack core\Hime.SDK.csproj -Build -Symbols -Properties Configuration=Release;Sign=True
nuget push  Hime.Redist.%VERSION%.0.nupkg
nuget push  Hime.SDK.%VERSION%.0.nupkg

REM Rebuild the Java artifacts and push
mvn -f runtimes\java\pom.xml clean deploy

REM Build the standalone package
MKDIR hime-%VERSION%-%TAG%
COPY /B LICENSE.txt hime-%VERSION%-%TAG%\README.txt
COPY /B releng\standalone\README.txt hime-%VERSION%-%TAG%\README.txt
COPY /B runtimes\java\target\*.jar hime-%VERSION%-%TAG%\
COPY /B runtimes\net\bin\Release\Hime.Redist.dll hime-%VERSION%-%TAG%\Hime.Redist.dll
COPY /B runtimes\net\b\Release\Hime.Redist.XML hime-%VERSION%-%TAG%\Hime.Redist.xml
COPY /B core\bin\Release\Hime.CentralDogma.dll hime-%VERSION%-%TAG%\Hime.CentralDogma.dll
COPY /B core\bin\Release\Hime.CentralDogma.XML hime-%VERSION%-%TAG%\Hime.CentralDogma.xml
COPY /B cli\net\bin\Release\himecc.exe hime-%VERSION%-%TAG%\himecc.exe
zip hime-%VERSION%-%TAG%.zip hime-%VERSION%-%TAG%\*
RMDIR hime-%VERSION%-%TAG% /S /Q