Imports DTIServerControls

Partial Public Class ModernElegance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbEdit.Checked = Session(DTIServerControl.siteEditOnDefaultKey)
            cbReorder.Checked = SortableServer1.LayoutOn
        End If
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        Session(DTIServerControl.siteEditOnDefaultKey) = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString, True)
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Recycle1.Save()
        DTISortable1.Save()
        SortableServer1.Save()
        DTISortable2.Save()
        DTISortable3.Save()
    End Sub

    Protected Sub cbReorder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbReorder.CheckedChanged
        SortableServer1.LayoutOn = cbReorder.Checked
        Response.Redirect(Request.Url.OriginalString, True)
    End Sub
End Class