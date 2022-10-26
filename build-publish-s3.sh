#!/bin/bash

SCRIPT="$(readlink -f "$0")"
ROOT="$(dirname "$SCRIPT")"

source "$ROOT/build-env.sh"

rm -f "$ROOT/version"
echo -n "$GIT_HASH" >> "$ROOT/version"

if [[ ! -z "$GIT_TAG" ]]; then
  pushd "$ROOT/target"
  "$ROOT/s3cli" -c $1 object uploadall --acl public-read cenotelie "hime/$GIT_TAG" --file windows macos linux-musl
  "$ROOT/s3cli" -c $1 object upload    --acl public-read cenotelie "hime/$GIT_TAG/version" --file "$ROOT/version"
  "$ROOT/s3cli" -c $1 object uploadall --acl public-read cenotelie "hime/stable" --file windows macos linux-musl
  "$ROOT/s3cli" -c $1 object upload    --acl public-read cenotelie "hime/stable/version" --file "$ROOT/version"
  pwd
  popd
else
  pushd "$ROOT/target"
  "$ROOT/s3cli" -c $1 object uploadall --acl public-read cenotelie "hime/latest" --file windows macos linux-musl
  "$ROOT/s3cli" -c $1 object upload    --acl public-read cenotelie "hime/latest/version" --file "$ROOT/version"
  pwd
  popd
fi

rm -f "$ROOT/version"
