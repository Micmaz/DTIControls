@echo off
set TabName=DTIControls
set currdir=%~dp0
set DllName="..\Bin\DTIControls.dll"
set ControlsFolder="%userprofile%\My Documents\Visual Studio 2005\Controls\%TabName%"
"%currdir%\Toolbox.exe" /vs2005 /vs2008 /uninstall "%TabName%"

set ControlsFolder="%userprofile%\My Documents\Visual Studio 2010\Controls\%TabName%"
del /S /F /Q %ControlsFolder%
"%VS100COMNTOOLS%\..\IDE\devenv.exe" /command Tools.InstallCommunityControls