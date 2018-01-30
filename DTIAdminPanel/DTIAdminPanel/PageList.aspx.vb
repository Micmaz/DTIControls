Imports DTIServerControls
Imports DTIMiniControls
Imports DTIMediaManager

#If DEBUG Then
Partial Public Class PageList
    Inherits DTIServerControlBasePage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class PageList
        Inherits DTIServerControlBasePage
#End If

#Region "properties"
        Private MainID As Long

        Public ReadOnly Property ds() As dsDTIAdminPanel
            Get
                If Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl") Is Nothing Then
                    Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl") = New dsDTIAdminPanel
                End If
                Return Session("DTIAdminPanelDataSetForUseInDTIAdminPanelServerControl")
            End Get
        End Property

        Public Property MaximumTreeDepth() As Integer
            Get
                If Page.Session("DepthOfMenuOnPageToSetDepthOfPagelistMenu") Is Nothing Then
                    Page.Session("DepthOfMenuOnPageToSetDepthOfPagelistMenu") = 10
                End If
                Return Page.Session("DepthOfMenuOnPageToSetDepthOfPagelistMenu")
            End Get
            Set(ByVal value As Integer)
                Page.Session("DepthOfMenuOnPageToSetDepthOfPagelistMenu") = value
            End Set
        End Property

        Public ReadOnly Property Sitemasterpage() As String
            Get
                Return Session("DTIAdminPanel.sitemasterpage")
            End Get
        End Property

        Private ReadOnly Property CurrentPage() As String
            Get
                Return HttpUtility.UrlDecode(Me.Page.Request.QueryString("page"))
            End Get
        End Property

        Private ReadOnly Property CurrentPageName() As String
            Get
                Dim pg As String = HttpUtility.UrlDecode(Me.Page.Request.QueryString("page"))
                If pg.Contains("/") Then
                    pg = pg.Substring(pg.LastIndexOf("/") + 1)
                End If
                Return pg.Replace(".aspx", "")
            End Get
        End Property
#End Region

        Private Sub Page_LoadNoPostBack(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadNoPostBack
            Dim location As String = AppDomain.CurrentDomain.BaseDirectory
            For Each masterPage As String In System.IO.Directory.GetFiles(location, "*.Master", IO.SearchOption.AllDirectories)
                masterPage = masterPage.Replace(location, "~/").Replace("\", "/")
                Dim last As Integer = masterPage.LastIndexOf("/") + 1
                ddlMasterPage.Items.Add(New ListItem(masterPage.Substring(last, masterPage.Length - last), masterPage))
            Next

            filltables()
            If CurrentPage IsNot Nothing AndAlso Not CurrentPage.Contains("page/") Then
                Dim dvisPageAdded As New DataView(ds.DTIDynamicPage, "Link = '" & CurrentPage & "'", "", DataViewRowState.CurrentRows)
                If dvisPageAdded.Count = 0 Then
                    btnAddStatic.Text = "Add " & CurrentPageName
                Else
                    btnAddStatic.Visible = False
                End If
            Else
                btnAddStatic.Visible = False
            End If
        End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		tooltip1.Text = "<img alt='info' src='" & BaseClasses.Scripts.ScriptsURL() & "/DTIAdminPanel/textbubble.png' />"
		tooltip2.Text = "<img alt='info' src='" & BaseClasses.Scripts.ScriptsURL() & "/DTIAdminPanel/textbubble.png' />"
		Try
		MainID = DTISharedVariables.MasterMainId
		Catch ex As Exception
			MainID = 0
		End Try

		tlMenuItems.NodeTypes.Add(New TreeNodeType("link", True, True, True, True, True, -1, -1, "[""link"", ""page"", ""item"", ""hidden""]", New NodeImageIcon(BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/Page.png")))
		tlMenuItems.NodeTypes.Add(New TreeNodeType("item", False, True, True, True, True, -1, -1, "[""link"", ""page"", ""item"", ""hidden""]"))
		tlMenuItems.NodeTypes.Add(New TreeNodeType("hidden", True, False, True, False, True, -1, -1, "[""link"", ""page"", ""hidden""]", New NodeImageIcon(BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/HPage.png")))
		'tlMenuItems.ValidChildren = "[""link"", ""page"", ""item"", ""hidden""]"
		tlMenuItems.addCustomContextMenu("Show/Hide link", BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/doc_cancel.png", "ShowHideVisible", "HideUnhide", True)

		tlPages.NodeTypes.Add(New TreeNodeType("page", True, True, True, True, True, -1, -1, "folder", New NodeImageIcon(BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/Page.png")))
		tlPages.NodeTypes.Add(New TreeNodeType("folder", False, True, True, True, True, -1, -1, "page"))
		tlPages.addCustomContextMenu("Go To Page", BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/doc_next.png", "pageVisibility", "gotoPage", True)
		tlPages.addCustomContextMenu("Edit Properties", BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/edit.png", "pageVisibility", "editPage", False)
		tlPages.addCustomContextMenu("Duplicate", BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/copy.png", "pageVisibility", "dup", True)
		bindTrees()
	End Sub

#Region "Menu Items Tree events"
	Private Sub tlMenuItems_NodeBound(ByRef node As DTIMiniControls.TreeListItem, ByVal isRoot As Boolean, ByVal hasChildren As Boolean) Handles tlMenuItems.NodeBound
            Dim phRow As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.FindByid(node.Value)
            If Not phRow.IsDTIDynamicPageNull Then
                If Not phRow.isHidden Then
                    node.NodeType = "link"
                Else
                    node.NodeType = "hidden"
                End If
            Else
                node.NodeType = "item"
            End If
        End Sub

        Private Sub tlMenuItems_NodeDeleted(ByRef node As DTIMiniControls.TreeListItem) Handles tlMenuItems.NodeDeleted
            Dim deleRow As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.FindByid(node.Value)
            ds.EnforceConstraints = False
            If deleRow IsNot Nothing Then
                deleRow.Delete()
            End If
            hfRefresh.Value = "1"
        End Sub

        Private Sub tlMenuItems_NodeDropped(ByRef node As DTIMiniControls.TreeListItem, ByRef originalTree As DTIMiniControls.TreeList) Handles tlMenuItems.NodeDropped
            Try
                Dim pageHRow As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.NewDTIPageHeiarchyRow
                With pageHRow
                    .DisplayName = node.Text
                    .isHidden = (node.NodeType = "hidden")
                    If node.ParentNode IsNot Nothing Then
                        .parentID = node.ParentNode.Value
                    End If
                    .DTIDynamicPage = node.Value
                    .MainID = MainID
                End With
                ds.DTIPageHeiarchy.AddDTIPageHeiarchyRow(pageHRow)
                node.Value = pageHRow.id
                hfRefresh.Value = "1"
            Catch ex As Exception

            End Try
        End Sub

        Private Sub tlMenuItems_NodeInserted(ByRef node As DTIMiniControls.TreeListItem) Handles tlMenuItems.NodeInserted
            Dim pageHRow As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.NewDTIPageHeiarchyRow
            Try
                If node.NodeType <> "EmptyTreeHolder" Then
                    Dim dv As New DataView(ds.DTIDynamicPage, "(PageName = '" & node.Text & "' OR ID = " & node.Value & ") AND MasterPage is not null", "", DataViewRowState.CurrentRows)
                    With pageHRow
                        .DisplayName = node.Text
                        .isHidden = (node.NodeType = "hidden")
                        If node.ParentNode IsNot Nothing Then
                            .parentID = node.ParentNode.Value
                        End If
                        If dv.Count > 0 Then
                            .DTIDynamicPage = dv(0)("Id")
                        End If
                        .MainID = MainID
                    End With
                    ds.DTIPageHeiarchy.AddDTIPageHeiarchyRow(pageHRow)
                    node.Value = pageHRow.id
                    hfRefresh.Value = "1"
                End If

            Catch ex As Exception

            End Try
        End Sub

        Private Sub tlMenuItems_NodeTypeChanged(ByRef node As DTIMiniControls.TreeListItem) Handles tlMenuItems.NodeTypeChanged
            Dim row As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.FindByid(node.Value)
            If row IsNot Nothing Then
                If node.NodeType = "hidden" Then
                    row.isHidden = True
                Else
                    row.isHidden = False
                End If
                hfRefresh.Value = "1"
            End If
        End Sub

        Private Sub tlMenuItems_NodeUpdated(ByRef node As DTIMiniControls.TreeListItem, ByVal newText As String) Handles tlMenuItems.NodeUpdated
            Dim row As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.FindByid(node.Value)
            'Dim dv As New DataView(ds.DTIDynamicPage, "PageName = '" & newText & "' AND MasterPage is not null", "", DataViewRowState.CurrentRows)
            If row IsNot Nothing Then
                row.DisplayName = newText
                'If dv.Count > 0 Then
                '    row.DTIDynamicPage = dv(0)("Id")
                'End If
                hfRefresh.Value = "1"
            End If
        End Sub

        Private Sub tlMenuItems_TreeReOrdered1(ByRef newTree() As DTIMiniControls.TreeListItem) Handles tlMenuItems.TreeReOrdered
            Dim i As Integer = 0
            For Each node As DTIMiniControls.TreeListItem In newTree
                Dim row As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.FindByid(node.Value)
                If row IsNot Nothing Then
                    If node.ParentNode IsNot Nothing Then
                        row.parentID = node.ParentNode.Value
                    Else
                        row.SetparentIDNull()
                    End If
                    row.SortOrder = i
                    i += 1
                End If
            Next
            hfRefresh.Value = "1"
        End Sub
#End Region

#Region "Pages Tree Events"
        Private Sub tlPages_NodeBound(ByRef node As DTIMiniControls.TreeListItem, ByVal isRoot As Boolean, ByVal hasChildren As Boolean) Handles tlPages.NodeBound
            Dim pgRow As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(node.Value)
            If Not pgRow.IsMasterPageNull Then
                node.NodeType = "page"
            Else
                node.NodeType = "folder"
            End If
        End Sub

        Private Sub tlPages_NodeDeleted(ByRef node As DTIMiniControls.TreeListItem) Handles tlPages.NodeDeleted
            Dim deleRow As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(node.Value)
            If deleRow IsNot Nothing Then
                Dim dv As New DataView(ds.DTIPageHeiarchy, "DTIDynamicPage = " & deleRow.id, "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    ds.DTIPageHeiarchy.FindByid(dv(0)("Id")).Delete()
                End If
                deleRow.Delete()
            End If
        End Sub

        Private Sub tlPages_NodeInserted(ByRef node As DTIMiniControls.TreeListItem) Handles tlPages.NodeInserted
            Dim pageRow As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.NewDTIDynamicPageRow
            With pageRow
                .PageName = node.Text
                .MainID = MainID
                .AdminOnly = False
                .UsersOnly = False
            End With
            ds.DTIDynamicPage.AddDTIDynamicPageRow(pageRow)
            node.Value = pageRow.id
        End Sub

        Private Sub tlPages_NodeUpdated(ByRef node As DTIMiniControls.TreeListItem, ByVal newText As String) Handles tlPages.NodeUpdated
            Dim row As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(node.Value)
            If row IsNot Nothing Then
                row.PageName = newText
            End If
        End Sub

        Private Sub tlPages_TreeReOrdered(ByRef newTree() As DTIMiniControls.TreeListItem) Handles tlPages.TreeReOrdered
            Dim i As Integer = 0
            For Each node As DTIMiniControls.TreeListItem In newTree
                Dim row As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(node.Value)
                If row IsNot Nothing Then
                    If node.ParentNode IsNot Nothing Then
                        row.ParentID = node.ParentNode.Value
                    Else
                        row.SetParentIDNull()
                    End If
                End If
                row.SortOrder = i
                i += 1
            Next
        End Sub
#End Region

#Region "Button Clicks"
    Protected Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            ds.EnforceConstraints = True
            Try
                sqlHelper.Update(ds.DTIDynamicPage)
            Catch ex As Exception
                Dim LastParent As Integer = -1
                For Each row As dsDTIAdminPanel.DTIDynamicPageRow In ds.DTIDynamicPage
                    If Not row.IsParentIDNull Then
                        Dim checkRow As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(row.ParentID)
                        If checkRow Is Nothing Then row.ParentID = LastParent
                    Else
                        LastParent = row.id
                    End If
                Next
                sqlHelper.Update(ds.DTIDynamicPage)
            End Try

            Try
                sqlHelper.Update(ds.DTIPageHeiarchy)
            Catch ex As System.Data.ConstraintException

                Dim LastRow As Integer = -1
                Dim LastOldParentId = -1
                Dim LastParentId As Integer = -1

                For Each row As dsDTIAdminPanel.DTIPageHeiarchyRow In ds.DTIPageHeiarchy
                    If Not row.IsparentIDNull Then
                        If row.parentID = LastRow Then
                            LastParentId = row.parentID
                        ElseIf row.parentID <> LastParentId Then
                            If row.parentID <> LastOldParentId Then
                                LastOldParentId = row.parentID
                                row.parentID = LastRow
                                LastParentId = row.parentID
                            Else
                                row.parentID = LastParentId
                            End If
                        End If
                    Else
                        LastParentId = -1
                        LastOldParentId = -1
                    End If
                    LastRow = row.id
                Next
                sqlHelper.Update(ds.DTIPageHeiarchy)
            End Try
        Catch ex As Exception
            Dim i As Integer = 0
            For Each row As dsDTIAdminPanel.DTIPageHeiarchyRow In ds.DTIPageHeiarchy
                If Not row.RowState = DataRowState.Deleted Then
                    If Not row.IsparentIDNull Then
                        If ds.DTIPageHeiarchy.FindByid(row.parentID) Is Nothing Then
                            row.Delete()
                        Else
                            row.SortOrder = i
                            i += 1
                        End If
                    Else
                        row.SortOrder = i
                        i += 1
                    End If
                End If
            Next
            sqlHelper.Update(ds.DTIPageHeiarchy)
        End Try
        bindTrees()
        Session("DTIAdminPanel.DTIPageHeiarchy") = Nothing
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If tbPageName.Text <> "" Then
            Dim dv As New DataView(ds.DTIDynamicPage, "PageName = '" & tbPageName.Text & "'", "", DataViewRowState.CurrentRows)
            If Not dv.Count > 0 Then
                addPage(tbPageName.Text)
            Else
                lblError.Visible = True
            End If
        End If
        tbPageName.Text = ""
        tbPageName.Focus()
    End Sub

    Protected Sub btnDuplicate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDuplicate.Click
        Try
            If HiddenField1.Value <> "" AndAlso Integer.Parse(HiddenField1.Value) > -1 Then
                duplicatePage(HiddenField1.Value)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnAddStatic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddStatic.Click
        addPage(CurrentPageName, CurrentPage)
        btnAddStatic.Visible = False
    End Sub

    'Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim sort As New DTISortable.dsDTISortable.DTISortableDataTable
    '    sqlHelper.FillDataTable("Select * from DTISortable", sort)

    '    Dim reg As New Regex("page_(.*?)_conPlaceHolder", RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase Or RegexOptions.Multiline)
    '    'Dim conReg As New Regex("(pag).*?(_conPlaceHolder.*?_\d)", RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase Or RegexOptions.Multiline)

    '    For Each row As DTISortable.dsDTISortable.DTISortableRow In sort
    '        Dim Contype As String = row.Content_Type

    '        If reg.IsMatch(Contype) Then
    '            Dim pagename As String = reg.Match(Contype).Groups(1).ToString
    '            Dim dv As New DataView(ds.DTIDynamicPage, "PageName = '" & pagename & "'", "", DataViewRowState.CurrentRows)

    '            If dv.Count > 0 Then
    '                Dim pageid = dv(0)("id")
    '                row.Content_Type = Contype.Replace(pagename, pageid)
    '            End If
    '        End If
    '    Next
    '    sqlHelper.Update(sort)
    '    Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") = Nothing
    'End Sub
#End Region

#Region "Helper Subs"
        Private Sub filltables()


            ds.EnforceConstraints = False
            ds.DTIPageHeiarchy.Clear()
            ds.DTIDynamicPage.Clear()
            ds.EnforceConstraints = True
            sqlHelper.FillDataSetMultiSelect("Select * from DTIDynamicPage where MainID = " & MainID & " Order by SortOrder, PageName;" & _
                    "select * from dtipageheiarchy where mainid = " & MainID & " order by SortOrder, displayname", ds, New String() {"DTIDynamicPage", "DTIPageHeiarchy"})


            Dim dynpageid As Integer = -1
            If ds.DTIDynamicPage.Count = 0 Then
                Dim dynPage As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.NewDTIDynamicPageRow
                With dynPage
                    .PageName = CurrentPage.Replace(".aspx", "")
                    .Link = CurrentPage
                    .MasterPage = ddlMasterPage.SelectedValue
                    .MainID = MainID
                    .AdminOnly = False
                    .UsersOnly = False
                    .Template = 1
                    .SortOrder = 0
                End With
                ds.DTIDynamicPage.AddDTIDynamicPageRow(dynPage)
                sqlHelper.Update(ds.DTIDynamicPage)
                dynpageid = dynPage.id
            End If
        End Sub

        Private Sub bindTrees()
            With tlMenuItems
                .dt = ds.DTIPageHeiarchy
                .ParentIdColumnName = "ParentID"
                .IdColumnName = "id"
                .TextColumnName = "DisplayName"
                .SortColumnName = "SortOrder"
                .DataBind()
            End With

            With tlPages
                .dt = ds.DTIDynamicPage
                .IdColumnName = "id"
                .TextColumnName = "PageName"
                .ParentIdColumnName = "ParentID"
                .SortColumnName = "SortOrder"
                .DataBind()
            End With

            hfMenus.Value = ""
            hfPages.Value = ""

            For Each menu As dsDTIAdminPanel.DTIPageHeiarchyRow In ds.DTIPageHeiarchy
                If Not menu.RowState = DataRowState.Deleted Then
                    Dim dynpage As String = "-1"
                    If Not menu.IsDTIDynamicPageNull Then
                        dynpage = menu.DTIDynamicPage
                    End If
                    hfMenus.Value &= "[" & menu.id & "," & dynpage & "]"
                End If
            Next

            For Each page As dsDTIAdminPanel.DTIDynamicPageRow In ds.DTIDynamicPage
                If Not page.RowState = DataRowState.Deleted Then
                    If Not page.IsMasterPageNull Then
                        Dim link As String = page.PageName & ",d"
                        If Not page.IsLinkNull Then
                            link = page.Link & ",s"
                        End If
                        link &= "," & page.PageName
                        hfPages.Value &= "[" & page.id & "," & link & "]"
                    End If
                End If
            Next
        End Sub

        Private Sub addPage(ByVal pagename As String, Optional ByVal link As String = Nothing)
            Dim newPage As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.NewDTIDynamicPageRow
            With newPage
                .PageName = pagename.Trim
                .MasterPage = ddlMasterPage.SelectedValue
                .MainID = MainID
                .Template = 1 'rblTemplate.SelectedValue
                .AdminOnly = False
                .UsersOnly = False
                If link IsNot Nothing Then
                    .Link = link
                End If
            End With
            ds.DTIDynamicPage.AddDTIDynamicPageRow(newPage)
            sqlHelper.Update(ds.DTIDynamicPage)
            bindTrees()
            lblError.Visible = False

            Dim medRow As dsMedia.DTIMediaManagerRow = _
                SharedMediaVariables.myMediaTable.NewDTIMediaManagerRow

            With medRow
                .Component_Type = "ContentManagement"
                .Content_Id = newPage.id
                .Content_Type = "page"
                .Date_Added = Now
                .Published = True
                .Removed = False
                .User_Id = -1 'currentUser.id
                .Permanent_URL = "/page/" & Page.Server.UrlEncode(newPage.PageName) & ".aspx"
            End With
            SharedMediaVariables.myMediaTable.AddDTIMediaManagerRow(medRow)
        End Sub

        Private Sub duplicatePage(ByVal id As Integer)
            Dim newPageName As String = ""
            Dim pageName As String = ""
            Dim page As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(id)
            Dim insertedSortables As New DTISortable.dsDTISortable.DTISortableDataTable
            Dim insertedSortableItems As New DTISortable.dsDTISortable.DTISortableItemDataTable
            Dim insertedEditPanels As New DTIContentManagement.dsEditPanel.DTIContentManagerDataTable
            Dim sortableItems As New DTISortable.dsDTISortable.DTISortableItemDataTable

            Dim dv As New DataView(ds.DTIDynamicPage, "PageName like '" & page.PageName & "%'", "", DataViewRowState.CurrentRows)
            newPageName = page.PageName & dv.Count + 1

            Dim dtSortable As New DTISortable.dsDTISortable.DTISortableDataTable
            Dim dtEditPanel As New DTIContentManagement.dsEditPanel.DTIContentManagerDataTable

            pageName = "page_" & page.id & "_conPlaceHolder%"
            sqlHelper.SafeFillTable("Select * from DTISortable where Content_Type like @pagename", dtSortable, New Object() {pageName})
            sqlHelper.SafeFillTable("Select * from DTISortableItem where DTISortable_Id in (Select ID from DTISortable where Content_Type like @pagename) and Assembly_Name like '%#DTIContentManagement%'", sortableItems, New Object() {pageName})
            sqlHelper.SafeFillTable("Select * from DTIContentManager where AreaName in (Select Content_Type from DTISortableItem where DTISortable_Id in (Select ID from DTISortable where Content_Type like @pagename) and Assembly_Name like '%#DTIContentManagement%')", dtEditPanel, New Object() {pageName})

            Dim newDynamicPagerow As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.NewDTIDynamicPageRow
            With newDynamicPagerow
                .PageName = newPageName
                If page.IsMasterPageNull Then
                    .SetMasterPageNull()
                Else
                    .MasterPage = page.MasterPage
                End If
                .MainID = MainID
                If page.IsLinkNull Then
                    .SetLinkNull()
                Else
                    .Link = page.Link
                End If
                If page.IsAdminOnlyNull Then
                    .SetAdminOnlyNull()
                Else
                    .AdminOnly = page.AdminOnly
                End If
                If page.IsUsersOnlyNull Then
                    .SetUsersOnlyNull()
                Else
                    .UsersOnly = page.UsersOnly
                End If
                If page.IsParentIDNull Then
                    .SetParentIDNull()
                Else
                    .ParentID = page.ParentID
                End If
                .SetSortOrderNull()
                If page.IsTemplateNull Then
                    .SetTemplateNull()
                Else
                    .Template = page.Template
                End If
            End With
            ds.DTIDynamicPage.AddDTIDynamicPageRow(newDynamicPagerow)
            sqlHelper.Update(ds.DTIDynamicPage)

            For Each sortable As DTISortable.dsDTISortable.DTISortableRow In dtSortable
                Dim sortRow As DTISortable.dsDTISortable.DTISortableRow = insertedSortables.NewDTISortableRow
                With sortRow
                    .Main_Id = MainID
                    .Content_Type = sortable.Content_Type.Replace(page.id, newDynamicPagerow.id)
                End With
                insertedSortables.AddDTISortableRow(sortRow)
                sqlHelper.Update(insertedSortables)

                Dim dvSortableItems As New DataView(sortableItems, "DTISortable_Id = " & sortable.Id, "", DataViewRowState.CurrentRows)

                For Each sortItem As DataRowView In dvSortableItems
                    Dim dvEdit As New DataView(dtEditPanel, "AreaName = '" & sortItem("Content_Type") & "'", "", DataViewRowState.CurrentRows)
                    Dim newcontentType As String = "DTIContentManagementEditPanel" & Guid.NewGuid.ToString.Replace("-", "")
                    If dvEdit.Count > 0 Then
                        insertedEditPanels.AddDTIContentManagerRow(dvEdit(0)("content"), MainID, newcontentType, Now)
                    End If

                    Dim insertedSortableRow As DTISortable.dsDTISortable.DTISortableItemRow = insertedSortableItems.NewDTISortableItemRow
                    With insertedSortableRow
                        .Content_Type = newcontentType
                        .Sort_Order = sortItem("Sort_Order")
                        .DTISortable_Id = sortRow.Id
                        .Assembly_Name = sortItem("Assembly_Name")
                        .isDeleted = sortItem("isDeleted")
                        .DeleteDate = sortItem("DeleteDate")
                        If sortItem("Page_Id") Is DBNull.Value Then
                            .SetPage_IdNull()
                        Else
                            .Page_Id = sortItem("Page_Id")
                        End If
                    End With
                    insertedSortableItems.AddDTISortableItemRow(insertedSortableRow)
                Next
            Next

            sqlHelper.Update(insertedEditPanels)
            sqlHelper.Update(insertedSortableItems)
            'clear out session object that holds sortables
            Session("DTISortableDataSetForUseInDTISortableAndDTIWidgetMenu") = Nothing
            bindTrees()
        End Sub
#End Region
    End Class