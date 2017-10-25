Public Partial Class PasswordStrengthTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PasswordStrength1.PasswordTextBoxId = TextBox1.ClientID
    End Sub

End Class