#!/bin/bash

# Turn off automatic packate restore on Visual STudio before running this script
# Visual Studio => Preferences => Nuget => General => uncheck the Package Restore box

# Clean
git clean -dxf

# NuGet restore
msbuild /t:restore Xamarin.forms.sln
nuget restore Xamarin.Forms.sln

# Build XF build tasks
msbuild Xamarin.Forms.Build.Tasks/Xamarin.Forms.Build.Tasks.csproj

# open in vsmac and should be able to build/run from here
open Xamarin.Forms.sln
