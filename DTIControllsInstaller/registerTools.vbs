Dim curdir, WshShell, currentDir, objEnv, userprofile, VS100COMNTOOLS
Set WshShell = CreateObject( "WScript.Shell" )
currentDir = Session.Property("CustomActionData")

'MsgBox "cmd /C """ & currentDir & "Toolbox.exe"" /vs2005 /vs2008 /installdesktop """ & currentDir & "DTIControls.dll"" ""DTI Controls"""
MsgBox "cmd /C ""'" & currentDir & "Toolbox.exe' /vs2005 /vs2008 /installdesktop '" & currentDir & "DTIControls.dll' 'DTI Controls'"""
WshShell.Run ("cmd /C ""'" & currentDir & "Toolbox.exe' /vs2005 /vs2008 /installdesktop '" & currentDir & "DTIControls.dll' 'DTI Controls'""")
'Toolbox.exe /vs2005 /vs2008 /installdesktop %DllName% %TabName%
userprofile = WshShell.ExpandEnvironmentStrings("userprofile")
vscontdll = WshShell.ExpandEnvironmentStrings("VS100COMNTOOLS")
'WshShell.Run ("cmd /C " & """" & command & """")


'set ControlsFolder= userprofile & "\My Documents\Visual Studio 2010\Controls\%TabName%"
'mkdir %ControlsFolder%
'copy %DllName% %ControlsFolder%
'"%VS100COMNTOOLS%\..\IDE\devenv.exe" /command Tools.InstallCommunityControls /command File.Exit



Set WshShell = Nothing
 