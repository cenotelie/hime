#!/bin/sh

SCRIPT="$(readlink -f "$0")"
RELENG="$(dirname "$SCRIPT")"
ROOT="$(dirname "$RELENG")"

YEAR=$(date +%Y)

sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/runtime-net/Hime.Redist.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/cli/Hime.CLI.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/sdk/Hime.SDK.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/himecc/HimeCC.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/parseit/Parseit.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/tests-driver/Tests.Driver.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/tests-executor-net/Tests.Executor.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/utils-demo/Utils.Demo.csproj"
sed -i -b "s/Version=\".*\"/Version=\"$1\"/" "$ROOT/sdk/Resources/NetCore/parser.csproj"

sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/runtime-net/Hime.Redist.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/cli/Hime.CLI.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/sdk/Hime.SDK.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/himecc/HimeCC.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/parseit/Parseit.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/tests-driver/Tests.Driver.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/tests-executor-net/Tests.Executor.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/utils-demo/Utils.Demo.csproj"

python "$ROOT/.releng/version.py" "$ROOT/runtime-java/pom.xml" $1 $2
python "$ROOT/.releng/version.py" "$ROOT/sdk/Resources/Java/pom.xml" $1 $2
python "$ROOT/.releng/version.py" "$ROOT/tests-executor-java/pom.xml" $1 $2

sed -i -b "s/PROJECT_NUMBER         = .*/PROJECT_NUMBER         = $1/" "$ROOT/.releng/doxygen.conf"