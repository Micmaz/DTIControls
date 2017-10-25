Imports System.IO

Public Class UploadTester
    Inherits basepage

    Private Sub UploadTester_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        If Request.QueryString("doupload") = "t" Then
            Response.Clear()
            HandleMethod(HttpContext.Current)
            Response.End()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.ThemeAdder.AddTheme(Me)
    End Sub

    Public Class FilesStatus
        Public Const HandlerPath As String = "/"

        Public Property group() As String
            Get
                Return m_group
            End Get
            Set(value As String)
                m_group = value
            End Set
        End Property
        Private m_group As String
        Public Property name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property
        Private m_name As String
        Public Property type() As String
            Get
                Return m_type
            End Get
            Set(value As String)
                m_type = value
            End Set
        End Property
        Private m_type As String
        Public Property size() As Integer
            Get
                Return m_size
            End Get
            Set(value As Integer)
                m_size = value
            End Set
        End Property
        Private m_size As Integer
        Public Property progress() As String
            Get
                Return m_progress
            End Get
            Set(value As String)
                m_progress = value
            End Set
        End Property
        Private m_progress As String
        Public Property url() As String
            Get
                Return m_url
            End Get
            Set(value As String)
                m_url = value
            End Set
        End Property
        Private m_url As String
        Public Property thumbnail_url() As String
            Get
                Return m_thumbnail_url
            End Get
            Set(value As String)
                m_thumbnail_url = value
            End Set
        End Property
        Private m_thumbnail_url As String
        Public Property delete_url() As String
            Get
                Return m_delete_url
            End Get
            Set(value As String)
                m_delete_url = value
            End Set
        End Property
        Private m_delete_url As String
        Public Property delete_type() As String
            Get
                Return m_delete_type
            End Get
            Set(value As String)
                m_delete_type = value
            End Set
        End Property
        Private m_delete_type As String
        Public Property [error]() As String
            Get
                Return m_error
            End Get
            Set(value As String)
                m_error = value
            End Set
        End Property
        Private m_error As String

        Public Sub New()
        End Sub

        Public Sub New(fileInfo As IO.FileInfo)
            SetValues(fileInfo.Name, CInt(fileInfo.Length))
        End Sub

        Public Sub New(fileName As String, fileLength As Integer)
            SetValues(fileName, fileLength)
        End Sub

        Private Sub SetValues(fileName As String, fileLength As Integer)
            name = fileName
            type = "image/png"
            size = fileLength
            progress = "1.0"
            url = HandlerPath & "FileTransferHandler.ashx?f=" & fileName
            thumbnail_url = HandlerPath & "Thumbnail.ashx?f=" & fileName
            delete_url = HandlerPath & "FileTransferHandler.ashx?f=" & fileName
            delete_type = "DELETE"
        End Sub
    End Class

#Region "Process File methods"
    'Private js As New JavaScriptSerializer()
    Public StorageRoot As String = "C:/temp"
    ' Handle request based on method
    Private Sub HandleMethod(context As HttpContext)
        Select Case context.Request.HttpMethod
            Case "HEAD", "GET"
                If GivenFilename(context) Then
                    DeliverFile(context)
                Else
                    ListCurrentFiles(context)
                End If
                Exit Select

            Case "POST", "PUT"
                UploadFile(context)
                Exit Select

            Case "DELETE"
                DeleteFile(context)
                Exit Select

            Case "OPTIONS"
                ReturnOptions(context)
                Exit Select
            Case Else

                context.Response.ClearHeaders()
                context.Response.StatusCode = 405
                Exit Select
        End Select
    End Sub

    Private Shared Sub ReturnOptions(context As HttpContext)
        context.Response.AddHeader("Allow", "DELETE,GET,HEAD,POST,PUT,OPTIONS")
        context.Response.StatusCode = 200
    End Sub

    ' Delete file from the server
    Private Sub DeleteFile(context As HttpContext)
        Dim filePath = StorageRoot + context.Request("f")
        If File.Exists(filePath) Then
            File.Delete(filePath)
        End If
    End Sub

    ' Upload file to the server
    Private Sub UploadFile(context As HttpContext)
        Dim statuses = New List(Of FilesStatus)()
        Dim headers = context.Request.Headers

        If String.IsNullOrEmpty(headers("X-File-Name")) Then
            UploadWholeFile(context, statuses)
        Else
            UploadPartialFile(headers("X-File-Name"), context, statuses)
        End If

        WriteJsonIframeSafe(context, statuses)
    End Sub

    ' Upload partial file
    Private Sub UploadPartialFile(fileName As String, context As HttpContext, statuses As List(Of FilesStatus))
        If context.Request.Files.Count <> 1 Then
            Throw New HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request")
        End If
        Dim inputStream = context.Request.Files(0).InputStream
        Dim fullName = StorageRoot + Path.GetFileName(fileName)

        Using fs = New FileStream(fullName, FileMode.Append, FileAccess.Write)
            Dim buffer = New Byte(1023) {}

            Dim l = inputStream.Read(buffer, 0, 1024)
            While l > 0
                fs.Write(buffer, 0, l)
                l = inputStream.Read(buffer, 0, 1024)
            End While
            fs.Flush()
            fs.Close()
        End Using
        statuses.Add(New FilesStatus(New FileInfo(fullName)))
    End Sub

    ' Upload entire file
    Private Sub UploadWholeFile(context As HttpContext, statuses As List(Of FilesStatus))
        For i As Integer = 0 To context.Request.Files.Count - 1
            Dim file = context.Request.Files(i)
            file.SaveAs(StorageRoot + Path.GetFileName(file.FileName))

            Dim fullName As String = Path.GetFileName(file.FileName)
            statuses.Add(New FilesStatus(fullName, file.ContentLength))
        Next
    End Sub

    Private Sub WriteJsonIframeSafe(context As HttpContext, statuses As List(Of FilesStatus))
        context.Response.AddHeader("Vary", "Accept")
        Try
            If context.Request("HTTP_ACCEPT").Contains("application/json") Then
                context.Response.ContentType = "application/json"
            Else
                context.Response.ContentType = "text/plain"
            End If
        Catch
            context.Response.ContentType = "text/plain"
        End Try

        Dim jsonObj = Json.JsonConvert.SerializeObject(statuses.ToArray())
        context.Response.Write(jsonObj)
    End Sub

    Private Shared Function GivenFilename(context As HttpContext) As Boolean
        Return Not String.IsNullOrEmpty(context.Request("f"))
    End Function

    Private Sub DeliverFile(context As HttpContext)
        Dim filename = context.Request("f")
        Dim filePath = StorageRoot + filename

        If File.Exists(filePath) Then
            context.Response.AddHeader("Content-Disposition", "attachment; filename=""" & Convert.ToString(filename) & """")
            context.Response.ContentType = "application/octet-stream"
            context.Response.ClearContent()
            context.Response.WriteFile(filePath)
        Else
            context.Response.StatusCode = 404
        End If
    End Sub

    Private Sub ListCurrentFiles(context As HttpContext)
        Dim files = New DirectoryInfo(StorageRoot).GetFiles()
        Dim jsonObj As String = Json.JsonConvert.SerializeObject(files)
        context.Response.AddHeader("Content-Disposition", "inline; filename=""files.json""")
        context.Response.Write(jsonObj)
        context.Response.ContentType = "application/json"
    End Sub


#End Region


End Class
