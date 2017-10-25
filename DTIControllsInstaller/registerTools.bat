@echo off
set TabName=DTIControls
set currdir=%~dp0
set DllName="..\Bin\DTIControls.dll"
set ControlsFolder="%userprofile%\My Documents\Visual Studio 2005\Controls\%TabName%"
"%currdir%\Toolbox.exe" /silent /vs2005 /installdesktop %DllName% "%TabName%"
"%currdir%\Toolbox.exe" /silent /vs2008 /installdesktop %DllName% "%TabName%"

set ControlsFolder="%userprofile%\My Documents\Visual Studio 2010\Controls\%TabName%"
mkdir %ControlsFolder%
copy %DllName% %ControlsFolder%
"%VS100COMNTOOLS%\..\IDE\devenv.exe" /command Tools.InstallCommunityControls /command File.Exit