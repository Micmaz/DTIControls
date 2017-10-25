Public Class WebForm1
    Inherits System.Web.UI.Page

    Private Sub WebForm1_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        Dim contenttyp As String = "1"
        If Request.QueryString("t") IsNot Nothing Then contenttyp = Request.QueryString("t")
        ep1.contentType = "EditPanel_" & contenttyp

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        ep1.setTextFromPage()
        Label1.Text = ep1.Text
    End Sub
End Class