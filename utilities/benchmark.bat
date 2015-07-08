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
MOVE HimeGrammar.dll HimeGrammarLALR.dll
himecc.exe HimeGrammar.gram -a:public -o:assembly -n Hime.Benchmark.Generated -m:rnglr
MOVE HimeGrammar.dll HimeGrammarRNGLR.dll
himecc.exe CSharp.gram -a:public -o:assembly -n Hime.Benchmark.Generated -m:rnglr
TYPE NUL > InputGram.txt
TYPE NUL > InputCS.txt
FOR /l %%x IN (1, 1, 200) DO (
  COPY /B InputGram.txt + HimeGrammar.gram InputGram.txt > NUL
)
FOR /l %%x IN (1, 1, 3000) DO (
  COPY /B InputCS.txt + SampleCS.cs InputCS.txt > NUL
)
ECHO Input Grammar >_ && type _ && type _ >> result.txt
Hime.Benchmark.exe HimeGrammarLALR.dll InputGram.txt --stats >_ && type _ && type _ >> result.txt
ECHO Input C# >_ && type _ && type _ >> result.txt
Hime.Benchmark.exe CSharp.dll InputCS.txt --stats >_ && type _ && type _ >> result.txt


REM Execute the benchmark
ECHO == Lexer on Input Grammar >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe HimeGrammarLALR.dll InputGram.txt --lexer >_ && type _ && type _ >> result.txt
)
ECHO == LALR on Input Grammar >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe HimeGrammarLALR.dll InputGram.txt >_ && type _ && type _ >> result.txt
)
ECHO == RNGLR on Input Grammar >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe HimeGrammarRNGLR.dll InputGram.txt >_ && type _ && type _ >> result.txt
)
ECHO == RNGLR on Input C# >_ && type _ && type _ >> result.txt
FOR /l %%x IN (1, 1, %ITERATIONS%) DO (
  Hime.Benchmark.exe CSharp.dll InputCS.txt >_ && type _ && type _ >> result.txt
)


REM Cleanup
CD ..
COPY %SANDBOX%\result.txt %TARGET% > NUL
RMDIR %SANDBOX% /S /Q
