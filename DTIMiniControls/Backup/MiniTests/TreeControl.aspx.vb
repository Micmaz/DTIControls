Imports DTIMiniControls

Partial Public Class TreeControl
    Inherits System.Web.UI.Page

    Private ReadOnly Property dt1() As DataTable
        Get
            If Session("dt1") Is Nothing Then
                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("Id", GetType(Integer)))
                dt.Columns.Add(New DataColumn("Parent_Id", GetType(Integer)))
                dt.Columns.Add(New DataColumn("SortOrder", GetType(Integer)))
                dt.Columns.Add(New DataColumn("Text", GetType(String)))
                dt.Columns.Add(New DataColumn("NodeType", GetType(String)))
                dt.Rows.Add(New Object() {1, DBNull.Value, 0, "Root Node 1", "Folder"})
                dt.Rows.Add(New Object() {2, 1, 1, "Child Node 1-1", "Folder"})
                dt.Rows.Add(New Object() {3, 1, 2, "Child Node 1-2", "Folder"})
                dt.Rows.Add(New Object() {4, 1, 3, "Child Node 1-3", "Folder"})
                dt.Rows.Add(New Object() {7, 3, 4, "Grand Child Node 1-1-3", "File"})
                Session("dt1") = dt
            End If
            Return Session("dt1")
        End Get
    End Property

    Private ReadOnly Property dt2() As DataTable
        Get
            If Session("dt2") Is Nothing Then
                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("Id", GetType(Integer)))
                dt.Columns.Add(New DataColumn("Parent_Id", GetType(Integer)))
                dt.Columns.Add(New DataColumn("Text", GetType(String)))
                dt.Columns.Add(New DataColumn("NodeType", GetType(String)))
                dt.Rows.Add(New Object() {1, DBNull.Value, "Root Node 1", "Folder"})
                dt.Rows.Add(New Object() {2, 1, "Child Node 1-1", "Folder"})
                Session("dt2") = dt
            End If
            Return Session("dt2")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TreeList1.ParentIdColumnName = "Parent_Id"
        TreeList1.IdColumnName = "Id"
        TreeList1.TextColumnName = "Text"
        TreeList1.NodeTypeColumnName = "NodeType"
        TreeList1.dt = dt1
        TreeList1.OnDropCallBack = "function(){alert('hi');}"
        TreeList1.DataBind()

        TreeList1.NodeTypes.Add(New TreeNodeType("Folder", True, True, True, True, True, -1, -1, "all", New NodeImageIcon(BaseClasses.Scripts.ScriptsURL & "/res/DTIMiniControls/themes/default.icons.png", "0px -16px")))
        TreeList1.NodeTypes.Add(New TreeNodeType("File", True, True, True, True, True, -1, -1, "all"))

        TreeList2.ParentIdColumnName = "Parent_Id"
        TreeList2.IdColumnName = "Id"
        TreeList2.TextColumnName = "Text"
        TreeList2.NodeTypeColumnName = "NodeType"
        TreeList2.dt = dt2
        TreeList2.DataBind()
    End Sub

    Private Sub TreeList1_NodeBound(ByRef node As DTIMiniControls.TreeListItem, ByVal isRoot As Boolean, ByVal hasChildren As Boolean) Handles TreeList1.NodeBound
        'node.NodeType = "Normal"
    End Sub

    Private Sub TreeList1_NodeDeleted(ByRef node As TreeListItem) Handles TreeList1.NodeDeleted
        For Each dr As DataRow In TreeList1.dt.Rows
            If dr("Id") = node.Value Then
                dr.Delete()
                Exit For
            End If
        Next
    End Sub

    Private Sub TreeList1_NodeInserted(ByRef node As TreeListItem) Handles TreeList1.NodeInserted
        If node.ParentNode IsNot Nothing Then
            TreeList1.dt.Rows.Add(New Object() {TreeList1.dt.Rows.Count + 1, node.ParentNode.Value, node.Text})
        Else
            TreeList1.dt.Rows.Add(New Object() {TreeList1.dt.Rows.Count + 1, DBNull.Value, node.Text})
        End If
    End Sub

    Private Sub TreeList1_NodeUpdated(ByRef node As TreeListItem, ByVal newText As String) Handles TreeList1.NodeUpdated
        For Each dr As DataRow In TreeList1.dt.Rows
            If dr("Id") = node.Value Then
                dr("Text") = newText
            End If
        Next
    End Sub

    Private Sub TreeList1_TreeReOrdered1(ByRef newTree() As DTIMiniControls.TreeListItem) Handles TreeList1.TreeReOrdered
        Dim i = 0
    End Sub
End Class