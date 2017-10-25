Imports System.IO

#If DEBUG Then
Partial Public Class JavaFileUploader
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class JavaFileUploader
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Private Shared _ht As Hashtable
        Public Shared ReadOnly Property securityHT() As Hashtable
            Get
                If _ht Is Nothing Then _ht = New Hashtable
                Return _ht
            End Get
        End Property

        Private ReadOnly Property currentguid() As String
            Get
                Return Request.QueryString("sguid")
            End Get
        End Property

        Public aguid As Guid = Guid.NewGuid

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request.QueryString("f") = "1" Then
                If Not currentguid Is Nothing AndAlso securityHT.ContainsKey(currentguid) Then
                    Dim f As HttpPostedFile = Request.Files("file0")
                    Dim final As Boolean = True
                    Dim part As Integer = -1
                    Dim exists As Boolean

                    If Request.QueryString("jupart") IsNot Nothing Then
                        final = Request.QueryString("jufinal") = "1"
                        part = Integer.Parse(Request.QueryString("jupart"))
                    End If

                    If f IsNot Nothing Then
                        Dim fs As FileStream
                        Dim fpath As String = Server.MapPath(DTIServerControls.DTISharedVariables.UploadFolderDefault)
                        Dim fullFName As String = Path.Combine(fpath, f.FileName)

                        exists = File.Exists(fullFName)

                        Dim buf(f.ContentLength) As Byte
                        f.InputStream.Read(buf, 0, f.ContentLength)

                        'checking to see if it is the first time this file has been uplaoded and if it exists 
                        'it skipps appending a creates a new one thus over-writting the old file.
                        'final = final piece of chunk or only peice if it wasn't chunked
                        'part = -1 means it wasn't chunked
                        'part = 1 means it is the first part of a chunked file
                        If exists AndAlso ((Not (final AndAlso part = -1)) AndAlso (Not (Not final AndAlso part = 1))) Then
                            While FileInUse(fullFName)
                                System.Threading.Thread.Sleep(3000)
                            End While
                            fs = New FileStream(fullFName, FileMode.Append, FileAccess.Write, FileShare.None)
                            fs.Seek(0, SeekOrigin.End)
                        Else
                            fs = New FileStream(fullFName, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
                        End If

                        fs.Write(buf, 0, buf.Length - 1)
                        fs.Close()

                        'If final Then _
                        '    vmdbOps.upLoadIso(f.FileName, securityHT.Item(currentguid))

                    End If
                End If
            Else
                'securityHT.Add(aguid.ToString, currentCustomer.id)
            End If
        End Sub

        Private Function FileInUse(ByVal fullPath As String) As Boolean
            Try
                Dim FS As New FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                FS.Close()
            Catch ex As Exception
                Return True
            End Try
            Return False
        End Function
    End Class