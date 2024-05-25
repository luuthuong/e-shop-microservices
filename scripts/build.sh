#!/usr/bin/env bash

set -e

ScriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo $ScriptDir

cd "$ScriptDir/../"

dotnet build 
# cd dotnet

# Release config triggers also "dotnet format"
# dotnet build --configuration Release --interactive

# dotnet test --configuration Release --no-build --no-restore --interactive
