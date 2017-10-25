#If DEBUG Then
Partial Public Class ManipulateImage
    Inherits System.Web.UI.Page
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class ManipulateImage
        Inherits System.Web.UI.Page
#End If
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

    End Class