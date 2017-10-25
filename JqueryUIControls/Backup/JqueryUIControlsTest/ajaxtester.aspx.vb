Public Partial Class ajaxtester
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ajax1.addControlsToWatchList(pnl1)
        addUC()
    End Sub


    Private Sub addUC()
        pnl1.Controls.Add(Me.LoadControl("currentTime.ascx"))
    End Sub

    Private Sub ajax1_callBack(ByVal sender As JqueryUIControls.AjaxCall, ByVal value As String) Handles ajax1.callBack
        addUC()
        sender.respond("ok")
    End Sub

End Class