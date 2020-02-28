@echo off
cp -Rf ../QTrkDotNet/QTrkDLLs release
cp -u bin/x64/Release/NanoBLoc.exe release
cp -u bin/x64/Release/QTrkDotNet.dll release
cp -u bin/x64/Release/TrackerUtils.dll release
cp -u bin/x64/Release/BitMiracle.LibTiff.NET.dll release
pause