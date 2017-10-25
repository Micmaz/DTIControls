Imports DTIServerControls

Partial Public Class GenericSortableTest
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbEdit.Checked = Session(DTIServerControl.siteEditOnDefaultKey)
        End If
        Sortable1.ControlList.Add(1, New LiteralControl("text1"))
        Sortable1.ControlList.Add(2, New LiteralControl("text2"))
        Sortable1.ControlList.Add(3, New Button)
        Sortable1.ConnectByClass = True

        Sortable2.ControlList.Add(1, New LiteralControl("text1"))
        Sortable2.ControlList.Add(2, New LiteralControl("text2"))
        Sortable2.AutoPostBack = True
        Sortable2.ConnectByClass = True
    End Sub


    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'DTIDragy.SaveList()
        'Response.Redirect("/Default.aspx")
    End Sub

    Private Sub Sortable1_OrderChanged(ByVal order() As String) Handles Sortable1.OrderChanged
        Dim s = order(0)
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        Session(DTIServerControl.siteEditOnDefaultKey) = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString)
    End Sub

    Private Sub Sortable2_OrderChanged(ByVal order() As String) Handles Sortable2.OrderChanged
        Dim s = order(0)
    End Sub
End Class