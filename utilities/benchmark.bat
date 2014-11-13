@ECHO OFF

REM Parameters
SET SANDBOX=sandbox
SET TARGET=benchmark.txt
SET ITERATIONS=10

REM Rebuild the components
msbuild /p:Configuration=Release /t:Clean runtimes\net\Hime.Redist.csproj
msbuild /p:Configuration=Release /t:Clean core\Hime.SDK.csproj
msbuild /p:Configuration=Release /t:Clean cli\net\HimeCC.csproj
msbuild /p:Configuration=Release /t:Clean utilities\net\benchmark\Benchmark.csproj
msbuild /p:Configuration=Release runtimes\net\Hime.Redist.csproj
msbuild /p:Configuration=Release core\Hime.SDK.csproj
msbuild /p:Configuration=Release cli\net\HimeCC.csproj
msbuild /p:Configuration=Release utilities\net\benchmark\Benchmark.csproj


REM Build the sandbox environment
MKDIR %SANDBOX%
COPY /B cli\net\bin\Release\Hime.Redist.dll %SANDBOX%\Hime.Redist.dll > NUL
COPY /B cli\net\bin\Release\Hime.CentralDogma.dll %SANDBOX%\Hime.CentralDogma.dll > NUL
COPY /B cli\net\bin\Release\himecc.exe %SANDBOX%\himecc.exe > NUL
COPY /B utilities\net\benchmark\bin\Release\Hime.Benchmark.exe %SANDBOX%\Hime.Benchmark.exe > NUL
COPY /B core\Sources\Input\HimeGrammar.gram %SANDBOX%\HimeGrammar.gram > NUL
COPY /B extras\Grammars\ECMA_CSharp.gram %SANDBOX%\CSharp.gram > NUL
COPY /B runtimes\net\Sources\TextPosition.cs %SANDBOX%\SampleCS.cs > NUL

REM Build the inputs
CD %SANDBOX%
ECHO == Building inputs >_ && type _ && type _ >> result.txt
himecc.exe HimeGrammar.gram -a:public -o:assembly -n Hime.Benchmark.Generated
himecc.exe CSharp.gram -a:public -o:assembly -n Hime.Benchmark.Generated -m:rnglr
TYPE NUL > InputLALR_50.gram
TYPE NUL > InputRNGLR_50.cs
TYPE NUL > InputLALR.gram
TYPE NUL > InputRNGLR.cs
FOR /l %%x IN (1, 1, 50) DO (
  COPY /B InputLALR_50.gram + CSharp.gram InputLALR_50.gram > NUL
  COPY /B InputRNGLR_50.cs + SampleCS.cs InputRNGLR_50.cs > NUL
)
FOR /l %%x IN (1, 1, 10) DO (
  COPY /B InputLALR.gram + InputLALR_50.gram InputLALR.gram > NUL
)
FOR /l %%x IN (1, 1, 100) DO (
  COPY /B InputRNGLR.cs + InputRNGLR_50.cs InputRNGLR.cs > NUL
)
ECHO Input LALR >_ && type _ && type _ >> result.txt
Hime.Benchmark.exe HimeGrammar.dll InputLALR.gram --stats >_ && type _ && type _ >> result.txt
ECHO Input RNGLR >_ && type _ && type _ >> result.txt
Hime.Benchmark.exe CSharp.dll InputRNGLR.cs --stats >_ && type _ && type _ >> result.txt


REM Execute the benchmark
ECHO == Lexer >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe HimeGrammar.dll InputLALR.gram --lexer >_ && type _ && type _ >> result.txt
)
ECHO == LALR >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe HimeGrammar.dll InputLALR.gram >_ && type _ && type _ >> result.txt
)
ECHO == RNGLR >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe CSharp.dll InputRNGLR.cs >_ && type _ && type _ >> result.txt
)


REM Cleanup
CD ..
COPY %SANDBOX%\result.txt %TARGET% > NUL
RMDIR %SANDBOX% /S /Q