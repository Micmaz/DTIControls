copy /Y ..\DTIGrid.dll DTIGrid\lib
cd DTIGrid
nuget  pack DTIGrid.dll.nuspec
mv *.nupkg ..\
pause