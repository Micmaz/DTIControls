Imports System.ComponentModel
Imports System.Configuration.Install
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Core
Imports System.IO

Public Class Installer1

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Private Sub Installer1_AfterInstall(ByVal sender As Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles Me.AfterInstall
        Dim installdir As String = Context.Parameters("targ")
        'Try
        '    ToolboxManager.Program.doProcess(New String() {"/silent", "/vs2005", "/installdesktop", installdir & "DTIControls.dll", "DTI Controls"})
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        'End Try

        'Try
        '    ToolboxManager.Program.doProcess(New String() {"/silent", "/vs2008", "/installdesktop", installdir & "DTIControls.dll", "DTI Controls"})
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        'End Try
        '/vs2005 /vs2008 /installdesktop '" & currentDir & "DTIControls.dll' 'DTI Controls'"""
        Dim pi As New ProcessStartInfo(installdir & "registerTools.bat")
        pi.WindowStyle = ProcessWindowStyle.Hidden
        pi.CreateNoWindow = True
        Process.Start(pi)
    End Sub

    Private Sub Installer1_BeforeUninstall(ByVal sender As Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles Me.BeforeUninstall
        Dim installdir As String = Context.Parameters("targ")
        'Try
        '    ToolboxManager.Program.doProcess(New String() {"/silent", "/vs2005", "/uninstall", "DTI Controls"})
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        'End Try

        'Try
        '    ToolboxManager.Program.doProcess(New String() {"/silent", "/vs2008", "/uninstall", "DTI Controls"})
        'Catch ex As Exception
        '    MsgBox(ex.Message & vbCrLf & ex.StackTrace)
        'End Try
        Dim pi As New ProcessStartInfo(installdir & "unregisterTools.bat")
        pi.WindowStyle = ProcessWindowStyle.Hidden
        pi.CreateNoWindow = True
        Process.Start(pi)
    End Sub

    Private Sub Installer1_Committed(ByVal sender As Object, ByVal e As System.Configuration.Install.InstallEventArgs) Handles Me.Committed
        ExtractZipFile("SampleSite.zip", "Samples")
    End Sub

    Public Sub ExtractZipFile(ByVal archiveFilenameIn As String, ByVal outFolder As String)
        Dim zf As ZipFile = Nothing
        Try
            Dim fs As FileStream = File.OpenRead(archiveFilenameIn)
            zf = New ZipFile(fs)
            'If Not [String].IsNullOrEmpty(password) Then
            '    ' AES encrypted entries are handled automatically
            '    zf.Password = password
            'End If
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
    End Sub
End Class
