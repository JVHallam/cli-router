#!/bin/bash

pushd .;
cd src/cli-router.core;
dotnet pack --configuration Release;
dotnet nuget push "$HOME/.nuget/local-sources/CliRouter.Core.1.0.1.nupkg" --api-key "$nuget_key" --source https://api.nuget.org/v3/index.json
rm -rf "$HOME/.nuget/packages/clirouter.core/"
popd;

# dotnet nuget add source /git/local_nuget_feed --name local
