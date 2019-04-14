#! /usr/bin/env bash

SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

dotnet publish --configuration Release --output "$SCRIPT_DIR/build" --verbosity normal --self-contained false
cp "$SCRIPT_DIR/scripts/run-tool.sh" "$SCRIPT_DIR/build/run.sh"
cp "$SCRIPT_DIR/scripts/run-tool.ps1" "$SCRIPT_DIR/build/run.ps1"
