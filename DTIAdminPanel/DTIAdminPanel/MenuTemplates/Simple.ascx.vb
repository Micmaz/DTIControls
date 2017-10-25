#If DEBUG Then
Partial Public Class Simple
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class Simple
        Inherits BaseClasses.BaseSecurityUserControl
#End If

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        End Sub

    End Class