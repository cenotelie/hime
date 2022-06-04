#!/bin/bash

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"
RELENG="$ROOT/.releng"

source "$ROOT/build-env.sh"
source "$ROOT/build-checks.sh"

echo "-- Rust components --"
cargo publish --manifest-path "$ROOT/runtime-rust/Cargo.toml"
cargo publish --manifest-path "$ROOT/sdk-rust/Cargo.toml"
cargo publish --manifest-path "$ROOT/sdk-debugger/Cargo.toml"
cargo publish --manifest-path "$ROOT/himecc/Cargo.toml"

echo "-- .Net components --"
dotnet restore "$ROOT/runtime-net"
(export FrameworkPathOverride="$MONO20"; dotnet pack "$ROOT/runtime-net" -c Release)
(export FrameworkPathOverride="$MONO20"; dotnet publish "$ROOT/runtime-net" -c Release)

echo "-- Java components --"
mvn -f "$ROOT/runtime-java/pom.xml" deploy
