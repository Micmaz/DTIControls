Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core
Imports System.IO

Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim installdir As String = "C:\Program Files (x86)\DTI\DTIControlls\"
        'Try
        '    ToolboxManager.Program.doProcess(New String() {"/vs2005", "/installdesktop", installdir & "DTIControls.dll", "DTI Controls"})
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        'End Try

        'Try
        '    ToolboxManager.Program.doProcess(New String() {"/vs2008", "/installdesktop", installdir & "DTIControls.dll", "DTI Controls"})
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        'End Try
        ''/vs2005 /v

        'Process.Start(installdir & "registerTools.bat")
        ExtractZipFile("SampleSite.zip", installdir & "Samples")
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim installdir As String = "C:\Program Files (x86)\DTI\DTIControlls\"
        '    Dim installdir As String = "D:\BetaDLLs\Merged"
        '    Try
        '        ToolboxManager.Program.doProcess(New String() {"/vs2005", "/uninstall", "DTI Controls"})
        '    Catch ex As Exception
        '        MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        '    End Try

        '    Try
        '        ToolboxManager.Program.doProcess(New String() {"/vs2008", "/uninstall", "DTI Controls"})
        '    Catch ex As Exception
        '        MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        '    End Try
        If Directory.Exists(installdir & "Samples") Then
            Directory.Delete(installdir & "Samples", True)
        End If
    End Sub

    Public Sub ExtractZipFile(ByVal archiveFilenameIn As String, ByVal outFolder As String)
        Dim zf As ZipFile = Nothing

        If File.Exists(archiveFilenameIn) Then
            Try

                Dim fs As FileStream = File.OpenRead(archiveFilenameIn)
                zf = New ZipFile(fs)

                For Each zipEntry As ZipEntry In zf
                    If Not zipEntry.IsFile Then
                        ' Ignore directories
                        Continue For
                    End If
                    Dim entryFileName As [String] = zipEntry.Name
                    ' to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    ' Optionally match entrynames against a selection list here to skip as desired.
                    ' The unpacked length is available in the zipEntry.Size property.

                    Dim buffer As Byte() = New Byte(4095) {}
                    ' 4K is optimum
                    Dim zipStream As Stream = zf.GetInputStream(zipEntry)

                    ' Manipulate the output filename here as desired.
                    Dim fullZipToPath As [String] = Path.Combine(outFolder, entryFileName)
                    Dim directoryName As String = Path.GetDirectoryName(fullZipToPath)
                    If directoryName.Length > 0 Then
                        Directory.CreateDirectory(directoryName)
                    End If

                    ' Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    ' of the file, but does not waste memory.
                    ' The "using" will close the stream even if an exception occurs.
                    Using streamWriter As FileStream = File.Create(fullZipToPath)
                        StreamUtils.Copy(zipStream, streamWriter, buffer)
                    End Using
                Next
            Finally
                If zf IsNot Nothing Then
                    zf.IsStreamOwner = True
                    ' Makes close also shut the underlying stream
                    ' Ensure we release resources
                    zf.Close()
                End If
            End Try
            File.Delete(archiveFilenameIn)
        End If
    End Sub
End Class