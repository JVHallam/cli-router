#!/bin/bash

cd src/cli-router.core;
dotnet pack --configuration Release;
cp bin/Release/CliRouter.Core.* /git/local_nuget_feed;

# dotnet nuget add source /git/local_nuget_feed --name local
