#!/bin/bash

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"

MONO=$(which mono)
if [ -z "$MONO" ]
then
  echo "Mono is not installed!"
  exit 1
fi
MONO=$(mono --version | grep 'version')

MONO461=/usr/lib/mono/4.6.1-api
if [ ! -f "$MONO461/mscorlib.dll" ]; then
  echo "Required Mono assemblies for .Net Framework 4.6.1 not found!"
  exit 1
fi

MONO20=/usr/lib/mono/2.0-api
if [ ! -f "$MONO20/mscorlib.dll" ]; then
  echo "Required Mono assemblies for .Net Framework 2.0 not found!"
  exit 1
fi
