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
			Return lastFileSize = 512000
		End Get
	End Property


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
            If fileList.Contains(Page.Request.Params("removeFile")) Then
                System.IO.File.Delete(getFormattedPath() & Page.Request.Params("removeFile"))
                fileList.Remove(Page.Request.Params("removeFile"))
                Page.Response.Clear()
                Page.Response.Write("OK")
                Page.Response.End()
            End If

        End If
        If Page.Request.Files.Count > 0 Then
            For Each f As String In Page.Request.Files
                Dim file As HttpPostedFile = Page.Request.Files(f)
                Dim localfilename As String = getFormattedPath() & file.FileName
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
					localfilename = getUniqueFilename(localfilename)
					lastFileName = localfilename
					file.SaveAs(localfilename)
					lastFileSize = file.InputStream.Length
					fileList.Add(localfilename.Substring(localfilename.LastIndexOf("/") + 1))
				End If
            Next
            Page.Response.Clear()
            Page.Response.Write("{""status"":""success""}")
            Page.Response.End()
        End If
        jQueryLibrary.jQueryInclude.RegisterJQueryUI(Page)
    End Sub

	Public Function getUniqueFilename(filename As String) As String
		If System.IO.File.Exists(filename) Then
			Dim ext As String = filename.Substring(filename.LastIndexOf("."))
			Dim fileNameDate As String = filename.Substring(0, filename.LastIndexOf(".")) & "_" & DateTime.Now.ToString("yyyyMMdd_HH_mm_ss")
			filename = fileNameDate & ext

			Dim i As Integer = 1
			While System.IO.File.Exists(filename)
				filename = fileNameDate & " (" & i & ")" & ext
				i += 1
			End While
		End If
		Return filename
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
