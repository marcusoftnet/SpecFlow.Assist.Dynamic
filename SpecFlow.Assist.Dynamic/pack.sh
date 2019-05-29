#!/bin/bash

rm -rf ./SpecFlow.Assist.Dynamic/bin/Release

dotnet clean
dotnet test
dotnet build -c Release

dotnet pack ./SpecFlow.Assist.Dynamic.csproj -c Release /p:NuspecFile=./SpecFlow.Assist.Dynamic.nuspec