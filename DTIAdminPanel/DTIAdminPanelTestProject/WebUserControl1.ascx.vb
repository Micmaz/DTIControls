Public Partial Class WebUserControl1
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim i As Integer = 0
        Try
            i = Integer.Parse(tb1.Text) + Integer.Parse(tb2.Text)
        Catch ex As Exception

        End Try

        Label1.Text = i
    End Sub
End Class