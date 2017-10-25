Set WshShell = CreateObject( "WScript.Shell" )
command = "unregisterTools.bat"
'msgbox command
WshShell.Run ("cmd /C " & """" & command & """")
Set WshShell = Nothing
