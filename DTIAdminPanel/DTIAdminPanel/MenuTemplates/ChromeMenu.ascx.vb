#If DEBUG Then
Partial Public Class ChromeMenu
    Inherits System.Web.UI.UserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ChromeMenu
        Inherits System.Web.UI.UserControl
#End If

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        End Sub

    End Class