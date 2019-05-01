#! /usr/bin/env bash

SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
BUILD_DIR="$SCRIPT_DIR/build"
RUNTIMES=("win-x64" "osx-x64" "linux-x64")
VERSION="$1"
VERSION_STRING=""
BUILD_SC=true

if [ ! "$VERSION" == "" ]; then
    VERSION_STRING="-$VERSION"
    echo "Build version is set to $VERSION"
    echo ""
fi

for r in ${RUNTIMES[@]}; do
    echo "Building '$r'..."
    DIR="$BUILD_DIR/$r"
    DIR_SC="$BUILD_DIR/$r-sc"

    dotnet publish --configuration Release --output "$DIR" --verbosity normal --self-contained false --runtime $r

    if [ "$BUILD_SC" == true ]; then
        dotnet publish --configuration Release --output "$DIR_SC" --verbosity normal --self-contained true --runtime $r
    fi

    echo "Copying scripts..."
    cp "$SCRIPT_DIR/scripts/run-tool.sh" "$DIR/run.sh"
    cp "$SCRIPT_DIR/scripts/run-tool.ps1" "$DIR/run.ps1"

    if [ "$BUILD_SC" == true ]; then
        cp "$SCRIPT_DIR/scripts/run-tool.sh" "$DIR_SC/run.sh"
        cp "$SCRIPT_DIR/scripts/run-tool.ps1" "$DIR_SC/run.ps1"
    fi

    echo "Creating an archive..."
    zip -vrqj "$SCRIPT_DIR/build/nuget-helper$VERSION_STRING.$r.zip" "$DIR/"

    if [ "$BUILD_SC" == true ]; then
        zip -vrqj "$SCRIPT_DIR/build/nuget-helper$VERSION_STRING.${r}_full.zip" "$DIR_SC/"
    fi

    rm -R $DIR

    if [ "$BUILD_SC" == true ]; then
        rm -R $DIR_SC
    fi

    echo "Done for $r"
    echo ""
done
