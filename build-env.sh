#!/bin/bash

VERSION=$(git rev-parse --short HEAD)
HASH=$(git rev-parse HEAD)
TAG=$(git tag -l --points-at HEAD)

LABEL=latest
if [[ ! -z "$TAG" ]]; then
  LABEL="$TAG"
fi
