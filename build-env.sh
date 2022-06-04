#!/bin/bash

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"

GIT_HASH_SHORT=$(git rev-parse --short HEAD)
GIT_HASH=$(git rev-parse HEAD)
GIT_TAG=$(git tag -l --points-at HEAD)

LABEL=latest
if [[ ! -z "$GIT_TAG" ]]; then
  LABEL="$GIT_TAG"
fi

VERSION=$(grep "version" "$ROOT/runtime-rust/Cargo.toml" | grep -o -E "([[:digit:]]+\\.[[:digit:]]+\\.[[:digit:]])+")
