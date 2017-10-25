#If DEBUG Then
Partial Public Class _Default
    Inherits System.Web.UI.Page
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class _Default
        Inherits System.Web.UI.Page
#End If

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me.Page)
        End Sub

    End Class