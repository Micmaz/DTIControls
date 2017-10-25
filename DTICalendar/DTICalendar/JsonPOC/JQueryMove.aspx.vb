#If DEBUG Then
Partial Public Class JQueryMove
    Inherits System.Web.UI.Page
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class JQueryMove
        Inherits System.Web.UI.Page
#End If

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.RegisterJQuery(Me)
        End Sub

    End Class