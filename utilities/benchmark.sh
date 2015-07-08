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
mv HimeGrammar.dll HimeGrammarLALR.dll
mono himecc.exe HimeGrammar.gram -a:public -o:assembly -n Hime.Benchmark.Generated -m:rnglr
mv HimeGrammar.dll HimeGrammarRNGLR.dll
mono himecc.exe CSharp.gram -a:public -o:assembly -n Hime.Benchmark.Generated -m:rnglr
for i in $(seq 1 200); do
  cat CSharp.gram >> InputGram.txt
done
for i in $(seq 1 3000); do
  cat SampleCS.cs >> InputCS.txt
done
echo "Input Grammar" | tee -a result.txt
mono Hime.Benchmark.exe HimeGrammarLALR.dll InputGram.txt --stats | tee -a result.txt
echo "Input C#" | tee -a result.txt
mono Hime.Benchmark.exe CSharp.dll InputCS.txt --stats | tee -a result.txt


# Execute the benchmark
echo "== Lexer on Input Grammar" | tee -a result.txt
for i in $(seq 1 $ITERATIONS); do
  mono Hime.Benchmark.exe HimeGrammarLALR.dll InputGram.txt --lexer | tee -a result.txt
done
echo "== LALR on Input Grammar" | tee -a result.txt
for i in $(seq 1 $ITERATIONS); do
  mono Hime.Benchmark.exe HimeGrammarLALR.dll InputGram.txt | tee -a result.txt
done
echo "== RNGLR on Input Grammar" | tee -a result.txt
for i in $(seq 1 $ITERATIONS); do
  mono Hime.Benchmark.exe HimeGrammarRNGLR.dll InputGram.txt | tee -a result.txt
done
echo "== RNGLR on Input C#" | tee -a result.txt
for i in $(seq 1 $ITERATIONS); do
  mono Hime.Benchmark.exe CSharp.dll InputCS.txt | tee -a result.txt
done


# Cleanup
cd ..
cp $SANDBOX/result.txt $TARGET
rm -r $SANDBOX
