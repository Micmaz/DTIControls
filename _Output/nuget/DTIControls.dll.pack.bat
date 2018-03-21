copy /Y ..\DTIControls.dll DTIControlls\lib
copy /Y ..\DBHelpers\SQLiteHelper.dll DTIControlls\lib
copy /Y ..\..\readme.md DTIControlls\readme.txt
SET ver=1.0.15

powershell -Command "(gc DTIControlls\DTIControls.dll.nuspec) -replace '1.0.11', '%ver%' | Out-File DTIControlls\DTIControls.dll.nuspec"
powershell -Command "(gc DTIGrid\DTIGrid.dll.nuspec)      -replace '1.0.11', '%ver%' | Out-File DTIGrid\DTIGrid.dll.nuspec"
powershell -Command "(gc Reporting\Reporting.dll.nuspec)    -replace '1.0.11', '%ver%' | Out-File Reporting\Reporting.dll.nuspec"
cd DTIControlls
nuget  pack DTIControls.dll.nuspec
mv *.nupkg ..\
cd ..
call Grid.dll.pack.bat
call Reporting.dll.pack.bat
powershell -Command "(gc DTIControlls\DTIControls.dll.nuspec) -replace '%ver%', '1.0.11' | Out-File DTIControlls\DTIControls.dll.nuspec"
powershell -Command "(gc DTIGrid\DTIGrid.dll.nuspec)      -replace '%ver%', '1.0.11' | Out-File DTIGrid\DTIGrid.dll.nuspec"
powershell -Command "(gc Reporting\Reporting.dll.nuspec)    -replace '%ver%', '1.0.11' | Out-File Reporting\Reporting.dll.nuspec"
pause