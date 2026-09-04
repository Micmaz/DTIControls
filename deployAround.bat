@echo off
setlocal enabledelayedexpansion

echo Starting file copy operation...
echo.

REM Define source directory where your built files are located
set SOURCE_DIR=C:\components\_output

REM Define the files to copy
set FILES_TO_COPY=DTIControls.dll ^
DTIControls.pdb ^
DTIControls.xml ^
Reporting.dll ^
Reporting.pdb ^
Reporting.xml ^
DTIGrid.dll ^
DTIGrid.pdb ^
DTIGrid.xml ^
FusionCharts.dll ^
FusionCharts.pdb ^
FusionCharts.xml ^
Chart.js.dll ^
Chart.js.pdb ^
Chart.js.xml ^
Microsoft.Bcl.AsyncInterfaces.dll ^
System.Memory.dll ^
System.Buffers.dll ^
System.Numerics.Vectors.dll ^
System.Runtime.CompilerServices.Unsafe.dll ^
System.Threading.Tasks.Extensions.dll ^
System.Text.Json.dll ^
System.Text.Encodings.Web.dll ^
System.Configuration.ConfigurationManager.dll ^
System.Security.Permissions.dll ^
System.Security.AccessControl.dll ^
System.Security.Principal.Windows.dll ^
System.ComponentModel.Annotations.dll ^
README-DTIControls.md ^
DTIControls.bindingRedirects.config ^
DBHelpers\SQLiteHelper.dll ^
DBHelpers\SQLiteHelper.pdb ^
DBHelpers\SQLiteHelper.xml ^
DataImporter.exe ^
DataImporter.pdb

REM Define destination directories using array syntax
set "DEST_DIRS[0]=C:\Visual Studio Projects\ElectionFlow\Lib"
set "DEST_DIRS[1]=C:\Visual Studio Projects\MainRepo\Lib"
set DEST_COUNT=2

REM Copy files to each destination
for /L %%i in (0,1,1) do (
    call set dest_dir=%%DEST_DIRS[%%i]%%
    echo Copying to: !dest_dir!
    
    REM Create destination directory if it doesn't exist
    if not exist "!dest_dir!" (
        echo   Creating directory: !dest_dir!
        mkdir "!dest_dir!"
    )
    
    REM Copy each file
    for %%f in (%FILES_TO_COPY%) do (
        set filename=%%f
        set source_file=%SOURCE_DIR%\!filename!
        
        REM Extract just the filename for destination (remove any folder path)
        for %%j in ("!filename!") do set dest_filename=%%~nxj
        set dest_file=!dest_dir!\!dest_filename!
        
        if exist "!source_file!" (
            echo   Copying !filename!...
            copy /Y "!source_file!" "!dest_file!" >nul
            if !errorlevel! equ 0 (
                echo     SUCCESS: !dest_filename! copied to !dest_dir!
            ) else (
                echo     ERROR: Failed to copy !dest_filename! to !dest_dir!
            )
        ) else (
            echo     WARNING: Source file not found: !source_file!
        )
    )
    echo.

    REM Copy the MSBuild targets that flows the native SqlClient SNI libs into dependent project bins
    if exist "C:\components\_DTIControls\DTIControls.targets" (
        copy /Y "C:\components\_DTIControls\DTIControls.targets" "!dest_dir!\DTIControls.targets" >nul
        if !errorlevel! equ 0 (
            echo     SUCCESS: DTIControls.targets copied to !dest_dir!
        ) else (
            echo     ERROR: Failed to copy DTIControls.targets to !dest_dir!
        )
    ) else (
        echo     WARNING: DTIControls.targets not found at C:\components\_DTIControls\
    )
    echo.

    REM Copy x86 and x64 folders from DBHelpers
    echo   Copying x86 folder...
    if exist "%SOURCE_DIR%\DBHelpers\x86" (
        if not exist "!dest_dir!\x86" mkdir "!dest_dir!\x86"
        xcopy /Y /E /Q "%SOURCE_DIR%\DBHelpers\x86\*" "!dest_dir!\x86\" >nul
        if !errorlevel! equ 0 (
            echo     SUCCESS: x86 folder copied to !dest_dir!
        ) else (
            echo     ERROR: Failed to copy x86 folder to !dest_dir!
        )
    ) else (
        echo     WARNING: Source folder not found: %SOURCE_DIR%\DBHelpers\x86
    )

    echo   Copying x64 folder...
    if exist "%SOURCE_DIR%\DBHelpers\x64" (
        if not exist "!dest_dir!\x64" mkdir "!dest_dir!\x64"
        xcopy /Y /E /Q "%SOURCE_DIR%\DBHelpers\x64\*" "!dest_dir!\x64\" >nul
        if !errorlevel! equ 0 (
            echo     SUCCESS: x64 folder copied to !dest_dir!
        ) else (
            echo     ERROR: Failed to copy x64 folder to !dest_dir!
        )
    ) else (
        echo     WARNING: Source folder not found: %SOURCE_DIR%\DBHelpers\x64
    )
    echo.
)

echo Copy operation completed.
echo Press any key to exit...
pause >nul