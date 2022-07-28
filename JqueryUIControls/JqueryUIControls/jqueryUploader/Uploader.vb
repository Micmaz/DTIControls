Imports System.IO

Public Class Uploader
    Inherits Control

#Region "Exposed Properties"

#End Region

    ''' <summary>
    ''' Path can either be a relative path such as: "/uploads/"
    ''' Or it can be a fully qualified path such as: C:\website\uploads
    ''' The iis user must have write access to the folder for this to work.
    ''' savePath and uploadPath both change the same internal value.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Path can either be a relative path such as: ""/uploads/""   Or it can be a fully qualified path such as: C:\website\uploads   The iis user must have write access to the folder for this to work.   savePath and uploadPath both change the same internal value.")> _
    Public Property savePath As String = "/uploads/"

    ''' <summary>
    ''' Path can either be a relative path such as: "/uploads/"
    ''' Or it can be a fully qualified path such as: C:\website\uploads
    ''' The iis user must have write access to the folder for this to work.
    ''' savePath and uploadPath both change the same internal value.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Path can either be a relative path such as: ""/uploads/""   Or it can be a fully qualified path such as: C:\website\uploads   The iis user must have write access to the folder for this to work.   savePath and uploadPath both change the same internal value.")> _
    Public Property uploadPath As String
        Get
            Return savePath
        End Get
        Set(value As String)
            savePath = value
        End Set
    End Property


    Public Property style As String = ""

    Public Property dropAreaText As String = "Drop Here"

    Public Property buttonText As String = "Browse"

    ''' <summary>
    ''' Sets weather the camera on a smartphone can be used as an upload source.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets weather the camera on a smartphone can be used as an upload source.")> _
    Public Property cameraUpload As Boolean = True

    Public ReadOnly Property fileList As List(Of String)
		Get
			If DesignMode Then Return New List(Of String)
			If Session("uploadFiles") Is Nothing Then Session("uploadFiles") = New List(Of String)
			Return Session("uploadFiles")
		End Get
	End Property

	Public Property lastFileSize As Int32
		Get
			If DesignMode Then Return 0
			If Session("lastFileSize") Is Nothing Then Session("lastFileSize") = 0
			Return Session("lastFileSize")
		End Get
		Set(value As Int32)
			If DesignMode Then Return
			Session("lastFileSize") = value
		End Set
	End Property

	Public Property lastFileName As String
		Get
			If DesignMode Then Return ""
			If Session("lastFileName") Is Nothing Then Session("lastFileName") = 0
			Return Session("lastFileName")
		End Get
		Set(value As String)
			If DesignMode Then Return
			Session("lastFileName") = value
		End Set
	End Property
	Public ReadOnly Property continueLastFile As Boolean
		Get
            Return startByte > 0
        End Get
	End Property

    Public ReadOnly Property isEndOfFile As Boolean
        Get
            Return totalByte = -1 OrElse endByte + 1 = totalByte
        End Get
    End Property



    Private _startByte As Integer = -1
    Public ReadOnly Property startByte As Integer
        Get
            parseHeader()
            Return _startByte
        End Get
    End Property

    Private _endByte As Integer = -1
    Public ReadOnly Property endByte As Integer
        Get
            parseHeader()
            Return _endByte
        End Get
    End Property

    Private _totalByte As Integer = -1
    Public ReadOnly Property totalByte As Integer
        Get
            parseHeader()
            Return _totalByte
        End Get
    End Property

    Private headerParse As Regex = New Regex("bytes\s(?<startByte>\d+)-(?<endByte>\d+)/(?<totalByte>\d+)", RegexOptions.CultureInvariant Or RegexOptions.Compiled)
    Private Function parseHeader() As Boolean
        If Page.Request.Headers("Content-Range") Is Nothing Then Return True
        If (_startByte > -1) Then Return True
        Dim m As Match = headerParse.Match(Page.Request.Headers("Content-Range"))
        If m IsNot Nothing Then
            _startByte = 0
            Integer.TryParse(m.Groups("startByte").Value, _startByte)
            _endByte = 0
            Integer.TryParse(m.Groups("endByte").Value, _endByte)
            _totalByte = 0
            Integer.TryParse(m.Groups("totalByte").Value, _totalByte)
        Else Return False
        End If

        Return True
    End Function


    Public Property fileTypes As String = ""

    Private ReadOnly Property Session As HttpSessionState
        Get
            Return BaseClasses.DataBase.httpSession
        End Get
    End Property

    Private Function getFormattedPath() As String
        Dim path As String = savePath
        If Not path.Contains(":") Then path = Page.Server.MapPath("~/") & "/" & savePath.Trim("/") & "/"
            If Not Directory.Exists(path) Then
                Try
                    Directory.CreateDirectory(path)
                Catch ex As Exception

                End Try
            End If
            Return path
    End Function

    Private Function getListString() As String
        Dim ret As String
            For Each filename As String In fileList.ToArray
                Dim filepath As String = getFormattedPath() & filename
                If File.Exists(filepath) Then
                    ret &= filename & "," & New FileInfo(filepath).Length & "#"
                Else
                fileList.Remove(filename)
                End If
            Next

            Return ret
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.Request.Params("doRemove") = "true" Then
            If fileList.Contains(Session("ULFilename.done." & Page.Request.Params("removeFile"))) Then
                System.IO.File.Delete(getFormattedPath() & Session("ULFilename.done." & Page.Request.Params("removeFile")))
                fileList.Remove(Page.Request.Params("removeFile"))
                Page.Response.Clear()
                Page.Response.Write("OK")
                Page.Response.End()
            End If

        End If
        If Page.Request.Files.Count > 0 Then
            For Each f As String In Page.Request.Files
                Dim file As HttpPostedFile = Page.Request.Files(f)
                Dim uniqueFilename = getUniqueFilename(file.FileName)
                Dim localfilename As String = getFormattedPath() & uniqueFilename
                'If System.IO.File.Exists(localfilename) AndAlso Not fileList.Contains(file.FileName) Then
                '    System.IO.File.Delete(localfilename)
                'End If
                If System.IO.File.Exists(lastFileName) AndAlso continueLastFile Then
                    If fileList.Contains(localfilename.Substring(localfilename.LastIndexOf("/") + 1)) Then
                        Dim fs As New FileStream(lastFileName, FileMode.Append, FileAccess.Write, FileShare.None)
                        With fs
                            Dim buffer(file.InputStream.Length - 1) As Byte
                            lastFileSize = file.InputStream.Length
                            file.InputStream.Read(buffer, 0, file.InputStream.Length)
                            fs.Write(buffer, 0, buffer.Length)
                            fs.Close()
                        End With
                    End If
                Else
                    'localfilename = getUniqueFilename(localfilename)
                    lastFileName = localfilename
                    file.SaveAs(localfilename)
                    lastFileSize = file.InputStream.Length
                    fileList.Add(localfilename.Substring(localfilename.LastIndexOf("/") + 1))
                End If
                If isEndOfFile Then
                    Session.Remove("ULFilename." & file.FileName)
                    Session("ULFilename.done." & file.FileName) = uniqueFilename
                End If
            Next

            Page.Response.Clear()
            Page.Response.Write("{""status"":""success""}")
            Page.Response.End()
        End If
        jQueryLibrary.jQueryInclude.RegisterJQueryUI(Page)
    End Sub


    Public Function getUniqueFilename(filename As String) As String
        'If System.IO.File.Exists(filename) Then

        If Session("ULFilename." & filename) IsNot Nothing Then
            Return Session("ULFilename." & filename)
        End If
        Dim origFilename = filename
        Dim ext As String = filename.Substring(filename.LastIndexOf("."))
        Dim fileNameDate As String = filename.Substring(0, filename.LastIndexOf("."))
        If fileNameDate.Length > 150 Then fileNameDate = fileNameDate.Substring(0, 150)
        If ext.Length > 10 Then ext = ext.Substring(0, 10)
        'Max file name len ~185 chars.
        fileNameDate = fileNameDate & "_" & DateTime.Now.ToString("yyyyMMdd_HH_mm_ss") & "_" & GenerateRandomString(8)
        filename = fileNameDate & ext

			Dim i As Integer = 1
			While System.IO.File.Exists(filename)
				filename = fileNameDate & " (" & i & ")" & ext
				i += 1
			End While
        'End If
        Session("ULFilename." & origFilename) = filename
        Return filename
	End Function

    Public Shared Function GenerateRandomString(Optional length As Integer = 16) As String
        Const src As String = "abcdefghijklmnopqrstuvwxyz0123456789"
        Dim sb As New StringBuilder()
        Dim RNG As Random = New Random()
        For i As Integer = 0 To length - 1
            Dim c As Char = src(RNG.[Next](0, src.Length))
            sb.Append(c)
        Next

        Return sb.ToString()
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        Dim camString As String = ""
        If cameraUpload Then
            camString = "accept=""image/*;capture=camera"" capture=""camera"" "
        ElseIf Not FileTypes.Trim = "" Then
            camString = "accept=""" & FileTypes & """"
        End If

		writer.Write(
			"        <link href=""" & BaseClasses.Scripts.ScriptsURL() & "JqueryUIControls/style.css"" rel=""stylesheet"" /> " & vbCrLf &
"        <div id=""upload"" style=""" & style & """ class=""uploadPanel""><div id=""currentfiles"" style=""display:none"">" & getListString() & "</div> " & vbCrLf &
"			<div id=""drop"">" & dropAreaText & "<br/><a>" & buttonText & "</a> " & vbCrLf &
"                <input type=""file"" name=""upl"" " & camString & " multiple /> " & vbCrLf &
"            </div> " & vbCrLf &
"			<ul></ul> " & vbCrLf &
"        <script src=""" & BaseClasses.Scripts.ScriptsURL() & "JqueryUIControls/jquery.knob.js""></script> " & vbCrLf &
"		<script src=""" & BaseClasses.Scripts.ScriptsURL() & "JqueryUIControls/jquery.iframe-transport.js""></script> " & vbCrLf &
"		<script src=""" & BaseClasses.Scripts.ScriptsURL() & "JqueryUIControls/jquery.fileupload.js""></script> " & vbCrLf &
"		<script src=""" & BaseClasses.Scripts.ScriptsURL() & "JqueryUIControls/UploaderScript.js""></script> " & vbCrLf &
"		</div> " & vbCrLf)
	End Sub

End Class
