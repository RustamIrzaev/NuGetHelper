#! /usr/bin/env bash

SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )

SOLUTION_FOLDER="$1"
LICENSE_DEPENDENCY_GENERATION=true
LOAD_METADATA=true
IGNORE_CLI_TOOLS=false
IGNORE_PACKAGES_CONFIG=false
PRINT_RESULTS=true
SHORT_OUTPUT=true

if [ "$SOLUTION_FOLDER" == "" ]; then
    SOLUTION_FOLDER="$SCRIPT_DIR"
fi

if [ ! -d $SOLUTION_FOLDER ]; then
    echo "Specified folder was not found"
    exit 1
fi

ARGUMENTS=()

if [ "$LICENSE_DEPENDENCY_GENERATION" == true ]; then
    ARGUMENTS+=("--generate-license")
fi

if [ "$LOAD_METADATA" == true ]; then
    ARGUMENTS+=("--load-metadata")
fi

if [ "$IGNORE_CLI_TOOLS" == true ]; then
    ARGUMENTS+=("--ignore-cli-tools")
fi

if [ "$IGNORE_PACKAGES_CONFIG" == true ]; then
    ARGUMENTS+=("--ignore-packages-config")
fi

if [ "$PRINT_RESULTS" == true ]; then
    ARGUMENTS+=("--print-results")
fi

if [ "$SHORT_OUTPUT" == true ]; then
    ARGUMENTS+=("--short")
fi

dotnet NuGetHelper.Tool.dll --solution-folder $SOLUTION_FOLDER "${ARGUMENTS[@]}"
