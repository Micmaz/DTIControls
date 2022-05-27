copy /Y ..\DTIGrid.dll DTIGrid\lib
copy /Y ..\..\DTIGrid\readme.md DTIGrid\readme.txt
cd DTIGrid
nuget  pack DTIGrid.dll.nuspec
mv *.nupkg ..\
cd ..
pause