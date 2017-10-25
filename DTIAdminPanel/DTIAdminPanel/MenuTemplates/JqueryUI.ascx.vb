#If DEBUG Then
Partial Public Class JqueryUI
    Inherits System.Web.UI.UserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class JqueryUI
        Inherits System.Web.UI.UserControl
#End If

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
End Class