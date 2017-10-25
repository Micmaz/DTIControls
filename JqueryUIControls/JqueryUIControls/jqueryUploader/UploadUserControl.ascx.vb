Imports System.IO
Imports BaseClasses
Imports System.Xml

Public Class UploadUserControl
    Inherits Control

#Region "Exposed Properties"

#End Region

    Public Property savePath As String = "/uploads/"

    Public Property fileList As String
        Get
            Return Session("uploadFiles")
        End Get
        Set(value As String)
            Session("uploadFiles") = value
        End Set
    End Property

    Private ReadOnly Property Session As HttpSessionState
        Get
            Return BaseClasses.DataBase.httpSession
        End Get
    End Property

    Private Function getFormattedPath() As String
        Return "/" & savePath.Trim("/") & "/"
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.Request.Params("doRemove") = "true" Then
            File.Delete(Page.Server.MapPath("~/") & getFormattedPath() & Page.Request.Params("removeFile"))
        End If
        If Page.Request.Files.Count > 0 Then
            For Each filename As String In Page.Request.Files
                Dim file As HttpPostedFile = Page.Request.Files(filename)
                file.SaveAs(Page.Server.MapPath("~/") & getFormattedPath() & file.FileName)
                fileList &= file.FileName & "," & file.InputStream.Length & "#"
            Next
            Page.Response.Clear()
            Page.Response.Write("{""status"":""success""}")
            Page.Response.End()
        End If

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        writer.Write( _
            "        <link href=""~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/style.css"" rel=""stylesheet"" /> " & vbCrLf & _
"        <div id=""upload"" class=""uploadPanel""><div id=""currentfiles"" style=""display:none"">" & fileList & "</div> " & vbCrLf & _
"			<div id=""drop"">Drop Here<a>Browse</a> " & vbCrLf & _
"                <input type=""file"" name=""upl"" multiple /> " & vbCrLf & _
"            </div> " & vbCrLf & _
"			<ul></ul> " & vbCrLf & _
"        <script src=""~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/jquery.knob.js""></script> " & vbCrLf & _
"		<script src=""~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/jquery.iframe-transport.js""></script> " & vbCrLf & _
"		<script src=""~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/jquery.fileupload.js""></script> " & vbCrLf & _
"		<script src=""~/res/BaseClasses/Scripts.aspx?f=JqueryUIControls/script.js""></script> " & vbCrLf & _
"		</div> " & vbCrLf)
    End Sub

End Class