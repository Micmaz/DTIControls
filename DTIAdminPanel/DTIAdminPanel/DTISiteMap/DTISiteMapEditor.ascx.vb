Imports DTIMiniControls

<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
Partial Public Class DTISiteMapEditor
    Inherits BaseClasses.BaseSecurityUserControl

#Region "properties"
    Private _mainID As Long = 0
    Public Property MainID() As Long
        Get
            Return _mainID
        End Get
        Set(ByVal value As Long)
            _mainID = value
        End Set
    End Property

    Private _contentType As String = ""
    Public Property ContentType() As String
        Get
            Return _contentType
        End Get
        Set(ByVal value As String)
            _contentType = value
        End Set
    End Property

    Protected ReadOnly Property ds() As dsDTIAdminPanel
        Get
            If Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl") Is Nothing Then
                Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl") = New dsDTIAdminPanel
            End If
            Return Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl")
        End Get
    End Property

    Private _sitemap As DataView
    Private ReadOnly Property tblsitemap() As DataView
        Get
            If _sitemap Is Nothing OrElse _sitemap.Count = 0 Then
                _sitemap = New DataView(ds.DTISiteMap, "MainID = " & MainID & " and ContentType = '" & ContentType & "'", "SortOrder", DataViewRowState.CurrentRows)
            End If
            Return _sitemap
        End Get
    End Property

    'Private ReadOnly Property tblSitemap() As dsDTIAdminPanel.DTISiteMapDataTable
    '    Get
    '        If Session("DTIStieMapDataTable" & ContentType) Is Nothing Then
    '            Session("DTIStieMapDataTable" & ContentType) = New dsDTIAdminPanel.DTISiteMapDataTable
    '        End If
    '        Return Session("DTIStieMapDataTable" & ContentType)
    '    End Get
    'End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            MainID = DTIServerControls.DTISharedVariables.MasterMainId
        Catch ex As Exception
            MainID = 0
        End Try
        'tlMenuItems.ValidChildren = "[""link"", ""page""]"
        bindTrees()
    End Sub

    Private Sub bindTrees()
        tlMenuItems.NodeTypes.Add(New TreeNodeType("link", False, True, True, False, True, -1, -1, "[""link"", ""page""]", New NodeImageIcon(BaseClasses.Scripts.ScriptsURL & "/res/DTIAdminPanel/page.png")))

        With tlMenuItems
            .dv = tblsitemap
            .ParentIdColumnName = "ParentID"
            .IdColumnName = "Id"
            .TextColumnName = "DisplayName"
            .SortColumnName = "SortOrder"
            .DataBind()
        End With

        tlPages.NodeTypes.Add(New TreeNodeType("page", False, False, False, False, True, -1, -1, "none", New NodeImageIcon(BaseClasses.Scripts.ScriptsURL & "/res/DTIAdminPanel/page.png")))
        tlPages.NodeTypes.Add(New TreeNodeType("folder", False, False, False, False, True, -1, -1, "page"))

        With tlPages
            .dt = ds.DTIDynamicPage
            .IdColumnName = "Id"
            .TextColumnName = "PageName"
            .ParentIdColumnName = "ParentID"
            .SortColumnName = "SortOrder"
            .DataBind()
        End With
    End Sub

#Region "tree Events"
    Private Sub tlMenuItems_NodeBound(ByRef node As DTIMiniControls.TreeListItem, ByVal isRoot As Boolean, ByVal hasChildren As Boolean) Handles tlMenuItems.NodeBound
        'node.Text = ds.DTIDynamicPage.FindByid(node.Text).PageName
        node.NodeType = "link"
        Dim row As dsDTIAdminPanel.DTISiteMapRow = ds.DTISiteMap.FindById(node.Value)
        If row.ContentType <> ContentType Then
            node.Visible = False
        End If
    End Sub

    Private Sub tlMenuItems_NodeDeleted(ByRef node As DTIMiniControls.TreeListItem) Handles tlMenuItems.NodeDeleted
        Dim delRow As dsDTIAdminPanel.DTISiteMapRow = ds.DTISiteMap.FindById(node.Value)
        If delRow IsNot Nothing Then
            delRow.Delete()
        End If
    End Sub

    Private Sub tlMenuItems_NodeInserted(ByRef node As DTIMiniControls.TreeListItem) Handles tlMenuItems.NodeInserted
        Dim siteMapRow As dsDTIAdminPanel.DTISiteMapRow = ds.DTISiteMap.NewDTISiteMapRow
        Try
            If node.Text <> "Drag/Drop a Page" Then
                Dim dv As New DataView(ds.DTIDynamicPage, "PageName = '" & node.Text & "' AND MasterPage is not null", "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    With siteMapRow
                        If node.ParentNode IsNot Nothing Then
                            .ParentID = node.ParentNode.Value
                        End If
                        .DisplayName = node.Text
                        .DTIDynamicPage = dv(0)("Id")
                        .MainID = MainID
                        .ContentType = Me.ContentType
                    End With
                    ds.DTISiteMap.AddDTISiteMapRow(siteMapRow)
                    node.Value = siteMapRow.Id
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tlMenuItems_NodeUpdated(ByRef node As DTIMiniControls.TreeListItem, ByVal newText As String) Handles tlMenuItems.NodeUpdated
        Dim row As dsDTIAdminPanel.DTISiteMapRow = ds.DTISiteMap.FindById(node.Value)
        If row IsNot Nothing Then
            row.DisplayName = newText
        End If
    End Sub

    Private Sub tlMenuItems_TreeReOrdered(ByRef newTree() As DTIMiniControls.TreeListItem) Handles tlMenuItems.TreeReOrdered
        Dim i As Integer = 0
        For Each node As DTIMiniControls.TreeListItem In newTree
            Dim row As dsDTIAdminPanel.DTISiteMapRow = ds.DTISiteMap.FindById(node.Value)
            If row IsNot Nothing Then
                If node.ParentNode IsNot Nothing Then
                    row.ParentID = node.ParentNode.Value
                Else
                    row.SetParentIDNull()
                End If
                row.SortOrder = i
                i += 1
            End If
        Next
    End Sub

    Private Sub tlPages_NodeBound(ByRef node As DTIMiniControls.TreeListItem, ByVal isRoot As Boolean, ByVal hasChildren As Boolean) Handles tlPages.NodeBound
        Dim pgRow As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(node.Value)
        If Not pgRow.IsMasterPageNull Then
            node.NodeType = "page"
        Else
            node.NodeType = "folder"
        End If
        node.Text = pgRow.PageName
    End Sub
#End Region

    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ds.EnforceConstraints = True
        Try
            sqlHelper.Update(ds.DTISiteMap)
        Catch ex As System.Data.ConstraintException

            Dim LastRow As Integer = -1
            Dim LastOldParentId = -1
            Dim LastParentId As Integer = -1

            For Each row As dsDTIAdminPanel.DTISiteMapRow In ds.DTISiteMap
                If Not row.IsParentIDNull Then
                    If row.ParentID = LastRow Then
                        LastParentId = row.ParentID
                    ElseIf row.ParentID <> LastParentId Then
                        If row.ParentID <> LastOldParentId Then
                            LastOldParentId = row.ParentID
                            row.ParentID = LastRow
                            LastParentId = row.ParentID
                        Else
                            row.ParentID = LastParentId
                        End If
                    End If
                Else
                    LastParentId = -1
                    LastOldParentId = -1
                End If
                LastRow = row.Id
            Next
            sqlHelper.Update(ds.DTISiteMap)
        End Try
        'bindTrees()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        bindTrees()
    End Sub
End Class