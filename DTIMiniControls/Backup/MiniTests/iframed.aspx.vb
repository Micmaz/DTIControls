Partial Public Class iframed
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Threading.Thread.Sleep(3000)
    End Sub

    Protected Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Threading.Thread.Sleep(15000)
    End Sub

    Protected Sub btnAjaxFreezeExample_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Threading.Thread.Sleep(7000)
    End Sub
End Class