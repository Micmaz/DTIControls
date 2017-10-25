Imports System.Net
Imports System.IO

#If DEBUG Then
Partial Public Class WebFileUploader
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class WebFileUploader
        Inherits BaseClasses.BaseSecurityUserControl
#End If
        Public Event handleFile(ByVal path As String)

        Private _redirectURL As String
        Public Property RedirectURL() As String
            Get
                Return _redirectURL
            End Get
            Set(ByVal value As String)
                _redirectURL = value
            End Set
        End Property

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Tagger1.CurrentTagText = "URL's to Upload:"
            Tagger1.AddTagText = "Add URL: "
            Tagger1.ShowPopularTags = False
            Tagger1.ShowSubmit = False
        End Sub

        Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
            For Each url As String In Tagger1.currentTagsList
                Dim _file As Byte()
                Try
                    Dim myReq As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)

                    Dim myResp As HttpWebResponse = myReq.GetResponse()

                    Dim stream As Stream = myResp.GetResponseStream()
                    Dim i As Integer
                    Using br As New BinaryReader(stream)
                        i = myResp.ContentLength

                        _file = br.ReadBytes(i)
                        br.Close()

                    End Using
                    myResp.Close()


                    Dim fileName As String = url.Replace("\", "/")
                    If fileName.LastIndexOf("/") > -1 Then
                        fileName = fileName.Substring(fileName.LastIndexOf("/") + 1)
                    End If
                    fileName = System.IO.Path.GetTempPath() & fileName
                    Dim fsTemp As New System.IO.FileStream(fileName, IO.FileMode.Create)
                    fsTemp.Write(_file, 0, _file.Length)
                    fsTemp.Close()

                    RaiseEvent handleFile(fileName)

                Catch ex As Exception

                End Try
            Next
            Response.Redirect(RedirectURL)
        End Sub
    End Class