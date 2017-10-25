Partial Public Class GenericSortableTest
    Inherits System.Web.UI.Page

    Public Property AdminOn() As Boolean
        Get
            Return Session("IsAdminModeOn")
        End Get
        Set(ByVal value As Boolean)
            Session("IsAdminModeOn") = value
        End Set
    End Property

    Private ReadOnly Property dt() As DataTable
        Get
            If Session("dt1") Is Nothing Then
                Dim dt1 As New DataTable
                dt1.Columns.Add(New DataColumn("Id", GetType(Integer)))
                dt1.Columns.Add(New DataColumn("Text", GetType(String)))
                dt1.Columns.Add(New DataColumn("SortOrder", GetType(Integer)))
                dt1.Rows.Add(New Object() {1, "item1", 0})
                dt1.Rows.Add(New Object() {2, "item2", 1})
                dt1.Rows.Add(New Object() {3, "item3", 2})
                dt1.Rows.Add(New Object() {4, "item4", 3})
                dt1.Rows.Add(New Object() {7, "item5", 4})
                Session("dt1") = dt1
            End If
            Return Session("dt1")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cbEdit.Checked = AdminOn
            loadSortables()
        End If

    End Sub

    Private Sub loadSortables()
        Sortable1.addSortableItem(New LiteralControl("text1"), "ID1")
        Sortable1.addSortableItem(New LiteralControl("text2"), "ID2")
        Sortable1.addSortableItem(New Button, "ID3")
        Sortable1.AdminOn = AdminOn

        sortable2.addSortableItem(New LiteralControl("text1"))
        sortable2.addSortableItem(New LiteralControl("text2"))
        sortable2.AutoPostBack = True
        sortable2.AdminOn = AdminOn

        With Sortable3
            .DataSource = dt
            .DataTextField = "Text"
            .DataValueField = "Id"
            .DataSortField = "SortOrder"
            .DataBind()
            .AdminOn = AdminOn
        End With
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'causes Postback
    End Sub

    Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
        AdminOn = cbEdit.Checked
        Response.Redirect(Request.Url.OriginalString)
    End Sub

    Private Sub Sortable1_OrderChanged(ByVal newOrder() As String, ByVal oldOrder() As String) Handles Sortable1.OrderChanged
        Dim i = 0
    End Sub

    Private Sub sortable2_OrderChanged(ByVal newOrder() As String, ByVal oldOrder() As String) Handles sortable2.OrderChanged
        Dim i = 0
    End Sub

    Private Sub Sortable3_DataBound(ByRef item As DTIMiniControls.SortableItem) Handles Sortable3.DataBound
        item.Controls.Add(New LiteralControl("--test DataBound"))
    End Sub

    Private Sub Sortable3_ItemReOrdered(ByRef item As DTIMiniControls.SortableItem, ByVal newIndex As Integer) Handles Sortable3.ItemReOrdered
        For Each row As DataRow In dt.Rows
            If row("ID") = item.Value Then
                row("SortOrder") = newIndex
            End If
        Next
    End Sub

    Private Sub Sortable3_OrderChanged(ByVal newOrder() As String, ByVal oldOrder() As String) Handles Sortable3.OrderChanged
        Sortable3.DataBind()
    End Sub
End Class