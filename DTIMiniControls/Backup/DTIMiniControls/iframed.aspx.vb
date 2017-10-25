#If DEBUG Then
Partial Public Class iframed
    Inherits System.Web.UI.Page
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class iframed
        Inherits System.Web.UI.Page
#End If

        Public ReadOnly Property url() As String
            Get
                If Request.QueryString("url") Is Nothing Then Return ""
                Return Request.QueryString("url")
            End Get
        End Property

        Private Sub iframed_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.RedirectLocation = url
        End Sub
    End Class