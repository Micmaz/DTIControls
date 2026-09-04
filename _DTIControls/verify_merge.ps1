# Verify merged DTIControls.dll contains BaseClasses types
$dllPath = "C:\components\_Output\DTIControls.dll"

Write-Host "Loading assembly: $dllPath" -ForegroundColor Cyan
$assembly = [System.Reflection.Assembly]::LoadFrom($dllPath)

Write-Host "`n=== BaseClasses Namespace Types ===" -ForegroundColor Green
$baseClassesTypes = $assembly.GetTypes() | Where-Object { $_.Namespace -eq 'BaseClasses' } | Sort-Object Name
Write-Host "Total types in BaseClasses namespace: $($baseClassesTypes.Count)" -ForegroundColor Yellow

Write-Host "`nKey classes from SQLHelper.Standard.2.0 (C#):" -ForegroundColor Cyan
$sqlHelperClasses = @('BaseHelper', 'SQLHelper', 'EmptyHelper', 'Platform', 'AssemblyLoader', 'AssemblyCache', 'ConfigService')
foreach ($className in $sqlHelperClasses) {
    $found = $baseClassesTypes | Where-Object { $_.Name -eq $className }
    if ($found) {
        Write-Host "  ✓ $className (Language: $($found.Module.Name))" -ForegroundColor Green
    } else {
        Write-Host "  ✗ $className - MISSING!" -ForegroundColor Red
    }
}

Write-Host "`nKey classes from BaseClasses.vbproj (VB.NET):" -ForegroundColor Cyan
$vbClasses = @('DesignTimeSessionState', 'EncryptionHelper', 'Hashing', 'JsMinimizer', 'MimeDecoder', 'PageCache', 'QueryStringChanger', 'Spider', 'BaseSecurityPage', 'BaseWebService', 'GlobalBase', 'MasterBase', 'PageTracker', 'BaseVirtualPathProvider')
foreach ($className in $vbClasses) {
    $found = $baseClassesTypes | Where-Object { $_.Name -eq $className }
    if ($found) {
        Write-Host "  ✓ $className" -ForegroundColor Green
    } else {
        Write-Host "  ✗ $className - MISSING!" -ForegroundColor Red
    }
}

Write-Host "`n=== All BaseClasses Types ===" -ForegroundColor Green
$baseClassesTypes | ForEach-Object { Write-Host "  - $($_.Name)" }
