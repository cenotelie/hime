#!/bin/bash

SCRIPT="$(readlink -f "$0")"
DIR="$(dirname "$SCRIPT")"
RELENG="$(dirname "$DIR")"
ROOT="$(dirname "$RELENG")"

HASH=$(git rev-parse HEAD)

IMAGES=$(docker images | grep -o -E '^cenotelie/hime-build-env-light(\s)+latest' | wc -l)
if [ "$IMAGES" -lt "1" ]; then
  echo "=> Building cenotelie/hime-build-env-light:latest"
  docker build --tag "cenotelie/hime-build-env-light:latest" --rm  --label changeset="$HASH" "$DIR"
fi
