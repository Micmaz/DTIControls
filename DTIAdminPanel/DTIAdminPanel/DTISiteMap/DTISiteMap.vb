Imports DTIServerControls
Imports DTIMiniControls

<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
Public Class DTISiteMap
    Inherits DTIServerControl

#Region "properties"
    'Public Overrides ReadOnly Property Menu_Icon_Url() As String
    '    Get
    '        Return BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/SiteMapIcon.png"
    '    End Get
    'End Property

    Private _cssTheme As String = "default"
    Public Property CssTheme() As String
        Get
            Return _cssTheme
        End Get
        Set(ByVal value As String)
            _cssTheme = value
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

    'Private ReadOnly Property tblSitemap() As DataTable
    '    Get
    '        If Session("DTIStieMapDataTable" & contentType) Is Nothing Then
    '            Session("DTIStieMapDataTable" & contentType) = New DataTable
    '        End If
    '        Return Session("DTIStieMapDataTable" & contentType)
    '    End Get
    'End Property
#End Region

    Public DataFetched As Boolean = False

    Private Sub DTISiteMap_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not mypage.IsPostBack Then
            addSQLCall()
        End If
    End Sub

    Private Sub DTISiteMap_DataChanged() Handles Me.DataChanged
        addSQLCall()
    End Sub

    Private Sub DTISiteMap_DataReady() Handles Me.DataReady
        DataFetched = True
    End Sub

    Private Sub DTISiteMap_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not DataFetched AndAlso Not mypage.IsPostBack Then
            parallelhelper.executeParallelDBCall()
        End If
    End Sub

    Private Sub DTISiteMap_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
        Me.Controls.Clear()

        If Mode = modes.Read Then
            Dim dv As New DataView(ds.DTISiteMap, "ParentID IS null AND Contenttype='" & contentType & "'", "SortOrder", DataViewRowState.CurrentRows)

            For Each parent As DataRowView In dv
                Dim parentUl As New HTMLList
                Dim link As String = ""
                Dim page As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(parent("DTIDynamicPage"))

                parentUl.CssClass = "DTISitemapRoot-" & CssTheme

                If page IsNot Nothing Then
                    If page.IsLinkNull Then
                        link = "/page/" & page.PageName & ".aspx"
                    Else
                        link = page.Link
                    End If

                    parentUl.addListItem(parent.Item("DisplayName"), Nothing, link)

                    '===========Start Children====================

                    addChildren(parent("Id"), parentUl)
                    Me.Controls.Add(parentUl)
                End If

            Next
        Else
            Dim siteMapEditControl As DTISiteMapEditor = DirectCast(mypage.LoadControl("~/res/DTIAdminPanel/DTISiteMapEditor.ascx"), DTISiteMapEditor)
            siteMapEditControl.MainID = MainID
            siteMapEditControl.ContentType = contentType
			If setupPanel Is Nothing Then setupPanel = New Panel
            setupPanel.Controls.Add(siteMapEditControl)

            Controls.Add(siteMapEditControl)
        End If

    End Sub

    Private Sub addSQLCall()
        parallelhelper.addFillDataTable("select * from DTISiteMap where mainID = @mainId", ds.DTISiteMap, New Object() {MainID})
        If ds.DTIDynamicPage.Count = 0 Then
            parallelhelper.addFillDataTable("Select * from DTIDynamicPage where MainID = @mainId Order by SortOrder, PageName", ds.DTIDynamicPage, New Object() {MainID})
        End If
    End Sub

    Private Sub addChildren(ByVal parentID As Integer, ByRef ParentUL As HTMLList)
        Dim itemsUl As New HTMLList
        itemsUl.CssClass = "DTISitemapNested-" & CssTheme
        Dim dvItems As New DataView(ds.DTISiteMap, "ParentID = " & parentID & " AND Contenttype='" & contentType & "'", "SortOrder", DataViewRowState.CurrentRows)

        For Each item As DataRowView In dvItems
            Dim itemLink As String = ""
            Dim childPage As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.FindByid(item("DTIDynamicPage"))

            If childPage.IsLinkNull Then
                itemLink = "/page/" & childPage.PageName & ".aspx"
            Else
                itemLink = "/" & childPage.Link
            End If
            itemsUl.addListItem(item("DisplayName"), Nothing, itemLink)
            addChildren(item("ID"), itemsUl)
        Next

        If itemsUl.Controls.Count > 0 Then
            ParentUL.addListItem(itemsUl)
        End If
    End Sub


    Private Sub DTISiteMap_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Private Sub DTISiteMap_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        sqlhelper.checkAndCreateTable(New dsDTIAdminPanel.DTISiteMapDataTable)
    End Sub
End Class
