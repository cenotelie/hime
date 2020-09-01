#!/bin/sh

SCRIPT="$(readlink -f "$0")"
RELENG="$(dirname "$SCRIPT")"
ROOT="$(dirname "$RELENG")"

YEAR=$(date +%Y)

# Update version for .csproj files
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/runtime-net/Hime.Redist.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/sdk-net/Hime.SDK.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/himecc/HimeCC.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/parseit-net/Parseit.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/tests-driver/Tests.Driver.csproj"
sed -i -b "s/<Version>.*<\/Version>/<Version>$1<\/Version>/" "$ROOT/tests-executor-net/Tests.Executor.csproj"
sed -i -b "s/Version=\".*\"/Version=\"$1\"/"                 "$ROOT/sdk-net/Resources/NetCore/parser.csproj"
sed -i -b "s/Version=\".*\"/Version=\"$1\"/"                 "$ROOT/sdk-rust/src/output/assembly_net.csproj"

# Update copyright for .csproj files
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/runtime-net/Hime.Redist.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/sdk-net/Hime.SDK.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/himecc/HimeCC.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/parseit-net/Parseit.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/tests-driver/Tests.Driver.csproj"
sed -i -b "s/<Copyright>.*<\/Copyright>/<Copyright>Copyright © Association Cénotélie $YEAR<\/Copyright>/" "$ROOT/tests-executor-net/Tests.Executor.csproj"

# Update version and copyright for pom.xml files
python "$ROOT/.releng/version.py" "$ROOT/runtime-java/pom.xml" $1 $2
python "$ROOT/.releng/version.py" "$ROOT/sdk-net/Resources/Java/pom.xml" $1 $2
python "$ROOT/.releng/version.py" "$ROOT/sdk-rust/src/output/assembly_java.xml" $1 $2
python "$ROOT/.releng/version.py" "$ROOT/tests-executor-java/pom.xml" $1 $2

# Update version for Cargo.toml files
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/runtime-rust/Cargo.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/runtime-rust/README.md"
sed -i -b "s/hime_redist\/.*\/hime_redist/hime_redist\/$1\/hime_redist/" "$ROOT/runtime-rust/README.md"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/runtime-rust/src/lib.rs"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/sdk-rust/Cargo.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/sdk-rust/Cargo.toml"
sed -i -b "s/hime_sdk = \".*\"/hime_sdk = \"$1\"/"        "$ROOT/sdk-rust/README.md"
sed -i -b "s/hime_sdk\/.*\/hime_sdk/hime_sdk\/$1\/hime_sdk/" "$ROOT/sdk-rust/README.md"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/tests-executor-rust/Cargo.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/tests-executor-rust/Cargo.toml"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/parseit-rust/Cargo.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/parseit-rust/Cargo.toml"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/himecc-rust/Cargo.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/himecc-rust/Cargo.toml"
sed -i -b "s/hime_sdk = \".*\"/hime_sdk = \"$1\"/"        "$ROOT/himecc-rust/Cargo.toml"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/unicode-gen/Cargo.toml"
sed -i -b "s/hime_sdk = \".*\"/hime_sdk = \"$1\"/"        "$ROOT/unicode-gen/Cargo.toml"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/langserv/Cargo.toml"
sed -i -b "s/hime_sdk = \".*\"/hime_sdk = \"$1\"/"        "$ROOT/langserv/Cargo.toml"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/sdk-net/Resources/Rust/Cargo.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/sdk-net/Resources/Rust/Cargo.toml"
sed -i -b "s/^version = \".*\"/version = \"$1\"/"         "$ROOT/sdk-rust/src/output/assembly_rust.toml"
sed -i -b "s/hime_redist = \".*\"/hime_redist = \"$1\"/"  "$ROOT/sdk-rust/src/output/assembly_rust.toml"

# Update version for doxygen configuration
sed -i -b "s/PROJECT_NUMBER         = .*/PROJECT_NUMBER         = $1/" "$ROOT/.releng/doxygen.conf"