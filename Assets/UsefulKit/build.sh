#!/bin/sh
unityPath="/Applications/Unity/Hub/Editor/2019.3.11f1/Unity.app/Contents/MacOS/Unity"
projectPath="/Users/admin/Projects/vwn-xr/vwn-xr-unity"
output="/Users/admin/Projects/vwn-xr/builds/xcode-sh"
devBuild="true"
target="iOS" # Options are Standalone, Win, Win64, OSXUniversal, Linux64, iOS, Android, WebGL, XboxOne, PS4, WindowsStoreApps, Switch, tvOS

$unityPath -batchmode -quit -logFile - -projectPath "$projectPath" -buildTarget $target -executeMethod BuildScript.Build --BuildOutputPath "$output" --DevelopmentBuild "$devBuild"