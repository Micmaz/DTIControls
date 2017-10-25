Partial Public Class Puzzled
    Inherits BaseMaster

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ChangeTemplate(SiteTemplate, Menu1.MenuItems)
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ToggleLoginVisibility(Menu1)
    End Sub
End Class