copy /Y ..\DTIControls.dll DTIControlls\lib
copy /Y ..\DBHelpers\SQLiteHelper.dll DTIControlls\lib
cd DTIControlls
nuget  pack DTIControls.dll.nuspec
mv *.nupkg ..\
pause