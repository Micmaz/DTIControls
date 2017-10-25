Imports DTIMiniControls
Partial Public Class TreeControlTest
    Inherits System.Web.UI.Page

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    Dim liroot2_1 As New TreeListItem("Root Node 1", 1)
    '    liroot2_1.Expanded = True
    '    Dim lichild2_1 As New TreeListItem("Child Node 1-1", 2)
    '    Dim lichild2_2 As New TreeListItem("Child Node 1-2", 3)
    '    Dim lichild2_3 As New TreeListItem("Child Node 1-3", 4)
    '    Dim lichild2_11 As New TreeListItem("Grand Child Node 1-1-1", 5)
    '    Dim lichild2_12 As New TreeListItem("Grand Child Node 1-1-2", 6)
    '    Dim lichild2_13 As New TreeListItem("Grand Child Node 1-1-3", 7)
    '    lichild2_2.addItems(New TreeListItem() {lichild2_11, lichild2_12, lichild2_13})
    '    liroot2_1.addItems(New TreeListItem() {lichild2_1, lichild2_2, lichild2_3})
    '    TreeList1.addListItemRoot(liroot2_1)
    '    Dim dt As New DataTable
    '    dt.Columns.Add(New DataColumn("Id", GetType(Integer)))
    '    dt.Columns.Add(New DataColumn("Parent_Id", GetType(Integer)))
    '    dt.Columns.Add(New DataColumn("Text", GetType(String)))
    '    dt.Rows.Add(New Object() {1, DBNull.Value, "Root Node 1"})
    '    dt.Rows.Add(New Object() {2, 1, "Child Node 1-1"})
    '    dt.Rows.Add(New Object() {3, 1, "Child Node 1-2"})
    '    dt.Rows.Add(New Object() {4, 1, "Child Node 1-3"})
    '    dt.Rows.Add(New Object() {7, 3, "Grand Child Node 1-1-3"})

    '    Dim dt2 As New DataTable
    '    dt2.Columns.Add(New DataColumn("Id", GetType(Integer)))
    '    dt2.Columns.Add(New DataColumn("Parent_Id", GetType(Integer)))
    '    dt2.Columns.Add(New DataColumn("Text", GetType(String)))
    '    dt2.Rows.Add(New Object() {5, DBNull.Value, "Root Node 1"})
    '    dt2.Rows.Add(New Object() {2, 1, "Child Node 1-1"})
    '    dt2.Rows.Add(New Object() {3, 1, "Child Node 1-2"})
    '    dt2.Rows.Add(New Object() {4, 1, "Child Node 1-3"})
    '    dt2.Rows.Add(New Object() {5, 3, "Grand Child Node 1-1-1"})
    '    dt2.Rows.Add(New Object() {6, 3, "Grand Child Node 1-1-2"})
    '    dt2.Rows.Add(New Object() {7, 3, "Grand Child Node 1-1-3"})

    '    TreeList1.ParentIdColumnName = "Parent_Id"
    '    TreeList1.IdColumnName = "Id"
    '    TreeList1.TextColumnName = "Text"
    '    TreeList1.dt = dt
    '    TreeList1.DataBind()

    '    TreeList2.ParentIdColumnName = "Parent_Id"
    '    TreeList2.IdColumnName = "Id"
    '    TreeList2.TextColumnName = "Text"
    '    TreeList2.dt = dt2
    '    TreeList2.DataBind()
    'End Sub

    'Private Sub TreeList1_NodeDeleted(ByRef node As TreeListItem) Handles TreeList1.NodeDeleted
    '    For Each dr As DataRow In TreeList1.dt.Rows
    '        If dr("Id") = node.Value Then
    '            dr.Delete()
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub TreeList1_NodeInserted(ByRef node As TreeListItem) Handles TreeList1.NodeInserted
    '    If node.ParentNode IsNot Nothing Then
    '        TreeList1.dt.Rows.Add(New Object() {TreeList1.dt.Rows.Count + 1, node.ParentNode.Value, node.Text})
    '    Else
    '        TreeList1.dt.Rows.Add(New Object() {TreeList1.dt.Rows.Count + 1, DBNull.Value, node.Text})
    '    End If
    'End Sub

    'Private Sub TreeList1_NodeUpdated(ByRef node As TreeListItem, ByVal newText As String) Handles TreeList1.NodeUpdated
    '    For Each dr As DataRow In TreeList1.dt.Rows
    '        If dr("Id") = node.Value Then
    '            dr("Text") = newText
    '        End If
    '    Next
    'End Sub

    'Private Sub TreeList1_TreeReOrdered() Handles TreeList1.TreeReOrdered
    '    Dim i = 0
    'End Sub
End Class