#!/bin/bash

SCRIPT="$(readlink -f "$0")"
DIR="$(dirname "$SCRIPT")"
RELENG="$(dirname "$DIR")"
ROOT="$(dirname "$RELENG")"

HASH=$(git rev-parse HEAD)

# Check for nexus.agricol.io/build-env
IMAGES=$(docker images | grep -o -E '^cenotelie/hime-build-env(\s)+latest' | wc -l)
if [ "$IMAGES" -lt "1" ]; then
  echo "=> Building cenotelie/hime-build-env:latest"
  docker build --tag "cenotelie/hime-build-env:latest" --rm  --label changeset="$HASH" "$DIR"
fi
