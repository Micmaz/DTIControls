Public Partial Class base64Tests
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Label1.Text = HiddenFieldEncoded1.Value
        Label2.Text = TextBoxEncoded1.Text
    End Sub
End Class