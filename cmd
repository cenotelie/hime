#!/bin/bash

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"

HASH=$(git rev-parse HEAD)
TAG=$(git tag -l --points-at HEAD)

CD=$(pwd)
WD="/src/"
case "$CD" in
  "$ROOT"* )
    if [ "$CD" != "$ROOT" ]; then
      S=$((${#ROOT} + 1))
      SUB=$(echo "$CD" | cut -c "$S-${#CD}")
      WD="/src$SUB"
    fi
  ;;
esac

TERMINAL=""
if [ -t 1 ] ; then
  docker run -it --rm --user=$(id -u) -v "$ROOT:/src" -v "cargo-registry:/home/builder/.cargo/registry" -v "$HOME/.m2:/home/builder/.m2" -e "HOME=/home/builder" -e "GIT_HASH=$HASH" -e "GIT_TAG=$TAG" -w "$WD" "cenotelie/hime-build-env:latest" $@
else
  docker run --rm --user=$(id -u) -v "$ROOT:/src" -v "cargo-registry:/home/builder/.cargo/registry" -v "$HOME/.m2:/home/builder/.m2" -e "HOME=/home/builder" -e "GIT_HASH=$HASH" -e "GIT_TAG=$TAG" -w "$WD" "cenotelie/hime-build-env:latest" $@
fi
