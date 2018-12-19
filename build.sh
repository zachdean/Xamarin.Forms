#!/bin/bash

# Clean
git clean -dxf

# NuGet restore
nuget restore Xamarin.Forms.sln
msbuild /t:restore Xamarin.forms.sln

# Build XF build tasks
msbuild Xamarin.Forms.Build.Tasks/Xamarin.Forms.Build.Tasks.csproj


# open in vsmac and should be able to build/run from here
open Xamarin.Forms.sln