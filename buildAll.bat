@echo off
setlocal enabledelayedexpansion

REM ============================================================
REM buildAll.bat - Builds every library that deployAround.bat
REM copies out of C:\components\_output:
REM
REM   DTIControls.dll / SQLiteHelper.dll  ->  DTIControls.sln
REM   DTIGrid.dll                         ->  DTIGrid\DTIGrid\DTIGrid.vbproj
REM   Reporting.dll                       ->  Reporting\Reporting\Reporting.vbproj
REM   Chart.js.dll                        ->  Reporting\Chart.js\Chart.js.vbproj
REM   FusionCharts.dll                    ->  Reporting\FusionCharts\FusionCharts.vbproj
REM   DataImporter.exe                    ->  DataImporter\DataImporter.sln (ILMerged into _Output)
REM
REM The System.*/Microsoft.* dependency DLLs in _Output come from
REM NuGet restore during the solution build; README-DTIControls.md
REM and DTIControls.bindingRedirects.config are static files.
REM
REM Usage: buildAll.bat [Configuration]   (default: Release)
REM ============================================================

set "ROOT=%~dp0"
set "CONFIG=%~1"
if "%CONFIG%"=="" set "CONFIG=Release"

echo Build configuration: %CONFIG%
echo.

REM --- Locate MSBuild (PATH first, then vswhere) ---
set "MSBUILD="
where msbuild >nul 2>nul && set "MSBUILD=msbuild"
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not defined MSBUILD if exist "%VSWHERE%" (
    for /f "usebackq tokens=*" %%i in (`"%VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"
)
if not defined MSBUILD (
    echo ERROR: MSBuild not found. Run from a Developer Command Prompt or install Visual Studio.
    exit /b 1
)
echo Using MSBuild: %MSBUILD%
echo.

set BUILD_FAILED=0

REM Build order matters: the satellite projects reference _Output\DTIControls.dll,
REM and Reporting also references _Output\DTIGrid.dll.
call :build "DTIControls solution - DTIControls.dll + SQLiteHelper.dll" "%ROOT%DTIControls.sln" %CONFIG% /restore
call :build "DTIGrid"      "%ROOT%DTIGrid\DTIGrid\DTIGrid.vbproj" %CONFIG%
call :build "Reporting"    "%ROOT%Reporting\Reporting\Reporting.vbproj" %CONFIG%
call :build "Chart.js"     "%ROOT%Reporting\Chart.js\Chart.js.vbproj" %CONFIG%
call :build "FusionCharts" "%ROOT%Reporting\FusionCharts\FusionCharts.vbproj" %CONFIG%

REM DataImporter always builds Release/AnyCPU via its solution: its post-build
REM event needs $(SolutionDir), and Release|AnyCPU ILMerges DataImporter.exe
REM straight into _Output where deployAround.bat picks it up
call :build "DataImporter" "%ROOT%DataImporter\DataImporter.sln" Release "/p:Platform=Any CPU"

echo ============================================================
if %BUILD_FAILED% gtr 0 echo BUILD COMPLETED WITH %BUILD_FAILED% FAILURES - do not run deployAround.bat
if %BUILD_FAILED% equ 0 echo All builds succeeded. _Output is ready for deployAround.bat
echo ============================================================
echo Press any key to exit...
pause >nul
exit /b %BUILD_FAILED%

REM --- subroutine: %1=label  %2=sln/proj path  %3=configuration  %4=extra msbuild arg ---
REM (no parenthesized blocks around %~1 - a label containing parens would break cmd block parsing)
:build
echo ============================================================
echo Building %~1 [%~3]
echo ============================================================
"%MSBUILD%" "%~2" /p:Configuration=%~3 /v:m /nologo %4
if errorlevel 1 goto :buildFailed
echo   SUCCESS: %~1
echo.
exit /b 0

:buildFailed
echo   ERROR: Build FAILED: %~1
set /a BUILD_FAILED+=1
echo.
exit /b 0
