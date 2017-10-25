Public Partial Class plumb
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.jsPlumb1.connections.Add(New JqueryUIControls.jsPlumb.Connection("div1", "div2"))
    End Sub

End Class