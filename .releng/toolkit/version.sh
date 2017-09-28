#!/bin/sh

VERSION=$(grep "AssemblyVersion" .releng/VersionInfo.cs | grep -o -E "([[:digit:]]+\\.[[:digit:]]+\\.[[:digit:]])+")
echo $VERSION
