#!/bin/sh

# Parameters
SANDBOX=sandbox
TARGET=benchmark.txt
ITERATIONS=10

# Rebuild the components
xbuild /p:Configuration=Release /t:Clean runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release /t:Clean core/Hime.SDK.csproj
xbuild /p:Configuration=Release /t:Clean cli/net/HimeCC.csproj
xbuild /p:Configuration=Release /t:Clean utilities/net/benchmark/Benchmark.csproj
xbuild /p:Configuration=Release runtimes/net/Hime.Redist.csproj
xbuild /p:Configuration=Release core/Hime.SDK.csproj
xbuild /p:Configuration=Release cli/net/HimeCC.csproj
xbuild /p:Configuration=Release utilities/net/benchmark/Benchmark.csproj


# Build the sandbox environment
mkdir $SANDBOX
cp cli/net/bin/Release/Hime.Redist.dll $SANDBOX/Hime.Redist.dll
cp cli/net/bin/Release/Hime.CentralDogma.dll $SANDBOX/Hime.CentralDogma.dll
cp cli/net/bin/Release/himecc.exe $SANDBOX/himecc.exe
cp utilities/net/benchmark/bin/Release/Hime.Benchmark.exe $SANDBOX/Hime.Benchmark.exe
cp core/Sources/Input/HimeGrammar.gram $SANDBOX/HimeGrammar.gram
cp extras/Grammars/ECMA_CSharp.gram $SANDBOX/CSharp.gram
cp runtimes/net/Sources/TextPosition.cs $SANDBOX/SampleCS.cs

# Build the inputs
cd $SANDBOX
echo "== Building inputs" | tee -a result.txt
mono himecc.exe HimeGrammar.gram -a:public -o:assembly -n Hime.Benchmark.Generated
mono himecc.exe CSharp.gram -a:public -o:assembly -n Hime.Benchmark.Generated -m:rnglr
for ((i=1;i<=50;i++));
do
  cat CSharp.gram >> InputLALR_50.gram
  cat SampleCS.cs >> InputRNGLR_50.cs
done
for ((i=1;i<=10;i++));
do
  cat InputLALR_50.gram >> InputLALR.gram
done
for ((i=1;i<=100;i++));
do
  cat InputRNGLR_50.cs >> InputRNGLR.cs
done
echo "Input LALR" | tee -a result.txt
mono Hime.Benchmark.exe HimeGrammar.dll InputLALR.gram --stats | tee -a result.txt
echo "Input RNGLR" | tee -a result.txt
mono Hime.Benchmark.exe CSharp.dll InputRNGLR.cs --stats | tee -a result.txt


# Execute the benchmark
echo "== Lexer" | tee -a result.txt
for ((i=1;i<=$ITERATIONS;i++));
do
  mono Hime.Benchmark.exe HimeGrammar.dll InputLALR.gram --lexer | tee -a result.txt
done
echo "== LALR" | tee -a result.txt
for ((i=1;i<=$ITERATIONS;i++));
do
  mono Hime.Benchmark.exe HimeGrammar.dll InputLALR.gram | tee -a result.txt
done
echo "== RNGLR" | tee -a result.txt
for ((i=1;i<=$ITERATIONS;i++));
do
  mono Hime.Benchmark.exe CSharp.dll InputRNGLR.cs | tee -a result.txt
done


# Cleanup
cd ..
cp $SANDBOX/result.txt $TARGET
rm -r $SANDBOX