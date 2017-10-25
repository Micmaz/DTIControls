Public Partial Class currentTime
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Controls.Add(New LiteralControl(Date.Now.ToLocalTime))
    End Sub

End Class