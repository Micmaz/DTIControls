Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.HighlighedEditor1.additionalAutocomp.Add("AAA")
        Me.HighlighedEditor1.additionalAutocomp.Add("group by")
        Dim i = 0
        ScriptBlock1.ScriptText() = "function showAlert() {alert('I worked');}"
        HiddenFieldEncoded1.ReferenceId = "hidfld"
    End Sub

    Protected Sub btnEncode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEncode.Click
        lblEncoded.Text = TextBoxEncoded1.Text
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        lblHidEncoded.Text = HiddenFieldEncoded1.Value
    End Sub

    Protected Sub btnAjaxFreezeExample_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Threading.Thread.Sleep(5000)
    End Sub

    Protected Sub btnPostbackFreezeExample_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostbackFreezeExample.Click
        Threading.Thread.Sleep(2000)
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        lblHLEditor.Text = HighlighedEditor1.Text
    End Sub
End Class