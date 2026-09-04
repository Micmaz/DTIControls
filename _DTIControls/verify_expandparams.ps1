# Verify ExpandParameters method in merged DLL
$dllPath = "C:\components\_Output\DTIControls.dll"

Write-Host "Loading assembly: $dllPath" -ForegroundColor Cyan
$assembly = [System.Reflection.Assembly]::LoadFrom($dllPath)

Write-Host "Checking for ExpandParameters method in BaseClasses.BaseHelper..." -ForegroundColor Cyan
$type = $assembly.GetType('BaseClasses.BaseHelper')
if ($type) {
    $method = $type.GetMethod('ExpandParameters')
    if ($method) {
        Write-Host "[SUCCESS] ExpandParameters method found!" -ForegroundColor Green
        Write-Host "Method signature:" -ForegroundColor Yellow
        Write-Host "  $method"
    } else {
        Write-Host "[FAILED] ExpandParameters method NOT found!" -ForegroundColor Red
    }
} else {
    Write-Host "[FAILED] BaseClasses.BaseHelper type NOT found!" -ForegroundColor Red
}
