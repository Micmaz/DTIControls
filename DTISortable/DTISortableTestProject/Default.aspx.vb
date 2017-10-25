Imports DTISortable
Imports DTIServerControls

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private sortable3 As New DTISortable.DTISortable("thirdDiv")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DTIServerControls.DTISharedVariables.LoggedIn = True
        If Not IsPostBack Then
            cbEdit.Checked = DTIControls.EditModeOn 'Session(DTIServerControl.siteEditOnDefaultKey)
            cbReorder.Checked = SortableServer1.LayoutOn
        End If
        With sortable3
            .ID = "SortableServer3"
            .HandleText = "++Handle++"
        End With
        'PlaceHolder3.Controls.Add(sortable3)

        'Dim lit As New LiteralControl("Test dydnamic controls")
        'lit.ID = SortableServer1.ClientID & "_" & "Literal1"
        'Panel1.Controls.Add(lit)
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        DTIServerControls.DTISharedVariables.AdminOn = cbEdit.Checked
        'EditPanel1.AdminOn = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString, True)
    End Sub
   
    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Recycle1.Save()
        SortableServer1.Save()
        'SortableServer2.Save()
        'sortable3.Save()
    End Sub

    Protected Sub cbReorder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbReorder.CheckedChanged
        SortableServer1.LayoutOn = cbReorder.Checked
        Response.Redirect(Request.Url.OriginalString, True)
    End Sub

    'Protected Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    Dim i As Integer = 0
    'End Sub
End Class