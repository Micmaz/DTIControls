Imports DTIServerControls

''' <summary>
''' Dynamic menu containing static and dynmic pages. Best used on a master page where automatic templates will take effect.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Dynamic menu containing static and dynmic pages. Best used on a master page where automatic templates will take effect."), ToolboxData("<{0}:Menu ID=""Menu"" SelectedCss=""selected"" Template=""Simple"" runat=""server""> </{0}:Menu>")> _
Public Class Menu
    Inherits DTIServerBase

#Region "Properties"

    Private _phTable As New dsDTIAdminPanel.MenuItemDataTable

    ''' <summary>
    ''' Menu Items in the menu
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Menu Items in the menu")> _
    Public ReadOnly Property MenuItems() As dsDTIAdminPanel.MenuItemDataTable
        Get
            If Not DesignMode Then
                If Page.Session("DTIAdminPanel.DTIPageHeiarchy") Is Nothing Then
                    _phTable.Clear()

                    'Dim ds As New dsDTIAdminPanel
                    Try
                        sqlhelper.FillDataTable("SELECT     DTIPageHeiarchy.*, DTIDynamicPage.PageName, DTIDynamicPage.Link, DTIDynamicPage.AdminOnly, DTIDynamicPage.MasterPage, DTIDynamicPage.Template " & _
                                                                   "FROM         DTIPageHeiarchy LEFT OUTER JOIN " & _
                                                                   "                      DTIDynamicPage ON DTIPageHeiarchy.DTIDynamicPage = DTIDynamicPage.id " & _
                                                                   "Where DTIPageHeiarchy.MainId = " & MainID & _
                                                                   " Order by DTIPageHeiarchy.SortOrder", _phTable)
                        For Each row As dsDTIAdminPanel.MenuItemRow In _phTable
                            row.Visible = Not row("isHidden")
                        Next
                    Catch ex As Exception

                    End Try

                    Page.Session("DTIAdminPanel.DTIPageHeiarchy") = _phTable
                End If
                Return Page.Session("DTIAdminPanel.DTIPageHeiarchy")
            Else
                Return New dsDTIAdminPanel.MenuItemDataTable
            End If
        End Get
    End Property

    ''' <summary>
    ''' Finds a specific page in MenuItems by pagename or display name if page name doesn't exist
    ''' </summary>
    ''' <param name="PageName"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Finds a specific page in MenuItems by pagename or display name if page name doesn't exist")> _
    Public ReadOnly Property MenuItem(ByVal PageName As String) As dsDTIAdminPanel.MenuItemRow
        Get
            Dim dv As New DataView(MenuItems, "PageName='" & PageName & "'", "", DataViewRowState.CurrentRows)
            If dv.Count > 0 Then
                Return MenuItems.FindByid(dv(0)("Id"))
            Else
                dv.RowFilter = "DisplayName='" & PageName & "'"
                If dv.Count > 0 Then
                    Return MenuItems.FindByid(dv(0)("Id"))
                Else
                    Return Nothing
                End If
            End If
        End Get
    End Property

    Public Enum TemplateType
        Simple
        ChromeMenu
        FlatMenu
        VerticalMenu
        JqueryUIHorizontal
    End Enum

    Private _DepthOfMenu As Integer = -1

    ''' <summary>
    ''' How many leves the menu actually renders on a page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("How many leves the menu actually renders on a page")> _
    Public Property DepthOfMenu() As Integer
        Get
            Return _DepthOfMenu
        End Get
        Set(ByVal value As Integer)
            _DepthOfMenu = value
        End Set
    End Property


    Private _Template As TemplateType

    ''' <summary>
    ''' Displays a pre desinged menu structure
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Displays a pre desinged menu structure")> _
    Public Property Template() As TemplateType
        Get
            Return _Template
        End Get
        Set(ByVal value As TemplateType)
            _Template = value
        End Set
    End Property

    Private _selectedCss As String = "selectedItem"

    ''' <summary>
    ''' Css Class for the currently selected tab in a menu
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Css Class for the currently selected tab in a menu")> _
    Public Property SelectedCss() As String
        Get
            Return _selectedCss
        End Get
        Set(ByVal value As String)
            _selectedCss = value
        End Set
    End Property

    Private _StartDepth As Integer = -1

    ''' <summary>
    ''' Overrides other options. Displays only a fixed depth of the menu from root plus a given depth.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Overrides other options. Displays only a fixed depth of the menu from root plus a given depth.")> _
    Public Property StartDepth() As Integer
        Get
            Return _StartDepth
        End Get
        Set(ByVal value As Integer)
            _StartDepth = value
        End Set
    End Property

    Private _FixedDepthtravel As Integer = 0

    ''' <summary>
    ''' Only used for fixed depth menus. This sets the number of nodes that will be walked to find relative nodes for display
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Only used for fixed depth menus. This sets the number of nodes that will be walked to find relative nodes for display")> _
    Public Property FixedDepthtravel() As Integer
        Get
            Return _FixedDepthtravel
        End Get
        Set(ByVal value As Integer)
            _FixedDepthtravel = value
        End Set
    End Property

    Private _SelectParentPage As Boolean = False

    ''' <summary>
    ''' If true the Anchor tag of the parent will include the class declared in selecedCSS otherwise
    ''' the class will be added to the anchor of the current page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("If true the Anchor tag of the parent will include the class declared in selecedCSS otherwise   the class will be added to the anchor of the current page")> _
    Public Property SelectParentPage() As Boolean
        Get
            Return _SelectParentPage
        End Get
        Set(ByVal value As Boolean)
            _SelectParentPage = value
        End Set
    End Property

    Private ReadOnly Property ds() As dsDTIAdminPanel
        Get
            If Page.Session("DTIAdminPanel.Pagelist") Is Nothing Then
                Page.Session("DTIAdminPanel.Pagelist") = New dsDTIAdminPanel
            End If
            Return Page.Session("DTIAdminPanel.Pagelist")
        End Get
    End Property

    Private Property selectedParentPage() As String
        Get
            If Page.Session("ParentPageOfTheCurrentlySelectedPageIfItIsAChildPage") Is Nothing Then
                Page.Session("ParentPageOfTheCurrentlySelectedPageIfItIsAChildPage") = ""
            End If
            Return Page.Session("ParentPageOfTheCurrentlySelectedPageIfItIsAChildPage")
        End Get
        Set(ByVal value As String)
            Page.Session("ParentPageOfTheCurrentlySelectedPageIfItIsAChildPage") = value
        End Set
    End Property

#End Region


    ''' <summary>
    ''' if true this refreshes the menu items from the database on load.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("if true this refreshes the menu items from the database on load."), ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Friend refreshdata As Boolean = False


    ''' <summary>
    ''' A utility hashtable to associate menu items with their data elements.
    ''' </summary>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("A utility hashtable to associate menu items with their data elements."), ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Friend ItemToData As New Hashtable

    ''' <summary>
    ''' The html rendered to the design view
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The html rendered to the design view"), ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Overrides Function getDesignTimeHtml() As String
        Return " "
    End Function

    Private TemplateControls As ControlCollection = Nothing
    Private containsDefaultMenuItems As Boolean = False
    Private ContainsOverridables As Boolean = False

    Private Sub Menu_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim ContainsMenuItems As Boolean = False

        'Load predefined templates
        For Each con As Control In Me.Controls
            If con.GetType Is GetType(MenuItem) Then
                ContainsMenuItems = True
                TemplateControls = Me.Controls
            ElseIf con.GetType Is GetType(DefaultMenuItem) Then
                containsDefaultMenuItems = True
            ElseIf con.GetType Is GetType(OverrideItem) Then
                ContainsOverridables = True
            End If
        Next
        If Template > -1 AndAlso Not ContainsMenuItems Then
            Select Case Template
                Case TemplateType.ChromeMenu
                    Dim templatecon As New ChromeMenu
                    templatecon = Me.Page.LoadControl("~/res/DTIAdminPanel/ChromeMenu.ascx")
                    Me.Controls.Add(templatecon)
                    jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/res/DTIAdminPanel/Chrome.js", Nothing, True, False)
                    TemplateControls = templatecon.Controls
                Case TemplateType.FlatMenu
                    Dim templateCon As New FlatMenu
                    templateCon = Me.Page.LoadControl("~/res/DTIAdminPanel/FlatMenu.ascx")
                    Me.Controls.Add(templateCon)
                    TemplateControls = templateCon.Controls
                Case TemplateType.Simple
                    Dim templateCon As New Simple
                    templateCon = Me.Page.LoadControl("~/res/DTIAdminPanel/Simple.ascx")
                    Me.Controls.Add(templateCon)
                    TemplateControls = templateCon.Controls
                Case TemplateType.VerticalMenu
                    Dim templateCon As New VerticalMenu
                    templateCon = Me.Page.LoadControl("~/res/DTIAdminPanel/VerticalMenu.ascx")
                    Me.Controls.Add(templateCon)
                    TemplateControls = templateCon.Controls
                    jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "~/res/DTIAdminPanel/jquery.dtimenu.js", Nothing, True, False)
                    jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "~/res/DTIAdminPanel/jquery.hoverIntent.js", Nothing, True, False)
					jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, "$('.verticalmenu').dtimenu();", False, Nothing)
				Case TemplateType.JqueryUIHorizontal
                    Dim templatecon As New JqueryUI
                    templatecon = Me.Page.LoadControl("~/res/DTIAdminPanel/JqueryUI.ascx")
                    Me.Controls.Add(templatecon)
                    jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "/res/DTIAdminPanel/jquery.ui.potato.menu.js", Nothing, True, False)
                    jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIAdminPanel/jquery.ui.potato.menu.css", "text/css")
					jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, "$('.JqueryUIHorizontal').ptMenu();", False, Nothing)
					jQueryLibrary.ThemeAdder.AddTheme(Me.Page)
                    TemplateControls = templatecon.Controls
                Case Else
                    Dim templatecon As New Control
                    TemplateControls = templatecon.Controls
            End Select
        End If
    End Sub

    Private Sub Menu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Add any additional menuitems to data
        If containsDefaultMenuItems OrElse ContainsOverridables Then
            If ds.DTIDynamicPage.Count = 0 Then
                sqlhelper.FillDataTable("select * from DTIDynamicPage where mainid = @mainid Order by SortOrder", ds.DTIDynamicPage, New Object() {Me.MainID})
            End If
            If ds.DTIPageHeiarchy.Count = 0 Then
                sqlhelper.FillDataTable("select * from DTIPageHeiarchy where mainid = @mainid Order by SortOrder", ds.DTIPageHeiarchy, New Object() {Me.MainID})
            End If
        End If

        If containsDefaultMenuItems Then
            For Each con As Control In Me.Controls
                If con.GetType Is GetType(DefaultMenuItem) Then
                    Dim item As DefaultMenuItem = CType(con, DefaultMenuItem)
                    item.addToData(item)
                End If
            Next
        End If

        If ContainsOverridables Then
            For Each con As Control In Me.Controls
                If con.GetType Is GetType(OverrideItem) Then
                    Dim item As OverrideItem = CType(con, OverrideItem)
                    item.SaveOverridableInfo(item)
                End If
            Next
        End If

        If refreshdata Then
            'reset menuitems property - want to keep it readonly
            Page.Session("DTIAdminPanel.DTIPageHeiarchy") = Nothing
        End If
       
    End Sub

    Private Function RenderMenu() As String
        Dim txt As String = ""
        Dim options As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.CultureInvariant Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled
        Dim dv As DataView = Nothing

        If StartDepth > -1 Then
            dv = buildFixedLevel(getPageID())
        Else
            dv = New DataView(MenuItems, "ParentId is null", "", DataViewRowState.CurrentRows)
        End If

        For Each ctrl As Control In TemplateControls
            If TypeOf ctrl Is LiteralControl Then
                txt &= CType(ctrl, LiteralControl).Text
            ElseIf ctrl.GetType Is GetType(MenuItem) Then
                For Each row As DataRowView In dv
                    Dim meni As MenuItem = CType(ctrl, MenuItem)
                    meni.DepthOfMenu = DepthOfMenu
                    If Not ItemToData Is Nothing AndAlso ItemToData.Contains(row("id")) Then
                        txt &= CType(ItemToData(row("id")), MenuItem).RenderMenuItem(SelectedCss, SelectParentPage, row.Item("id")).Text
                    Else
                        txt &= CType(ctrl, MenuItem).RenderMenuItem(SelectedCss, SelectParentPage, row.Item("id")).Text
                    End If
                Next
            End If
        Next

        If selectedParentPage <> "" Then
            selectedParentPage = selectedParentPage.Replace("#", "\#")
            Dim selectedLI As Regex = New Regex(selectedParentPage, options)
            If selectedLI.IsMatch(txt) Then
                txt = selectedLI.Replace(txt, "class=""" & SelectedCss & """ " & selectedParentPage)
            End If
        End If

        Return txt
    End Function

    Private Function getPageID() As Integer
        Dim page As String = Me.Page.Request.Url.LocalPath

        page = page.Substring(page.LastIndexOf("/"))
        page = page.TrimStart("/").Replace(".aspx", "")
        If page.ToLower = "page" Then
            page = Me.Page.Request.QueryString("pagename")
        End If
        page = page.ToLower
        Dim dv As New DataView(MenuItems, "PageName='" & page & "'", "", DataViewRowState.CurrentRows)
        If dv.Count > 0 Then
            Return dv(0)("id")
        Else
            Dim dv1 As New DataView(MenuItems, "Link='" & page & ".aspx'", "", DataViewRowState.CurrentRows)
            If dv1.Count > 0 Then
                Return dv1(0)("id")
            Else
                Return -1
            End If
        End If

    End Function

    Private Function buildFixedLevel(ByVal currentPageId As Integer) As DataView
        FixedDepthtravel += 1
        'Build the sort filter for either the parent nodes or a level of subnodes
        Dim currentlvl As Integer = -1
        If FixedDepthtravel > StartDepth + 1 Then FixedDepthtravel = StartDepth + 1

        Dim currentrow As dsDTIAdminPanel.MenuItemRow
        Dim row As dsDTIAdminPanel.MenuItemRow = MenuItems.FindByid(currentPageId)
        currentrow = row
        Dim filtStr As String = "1=2"
        Dim moveSteps As Integer
        If Not row Is Nothing Then
            currentlvl += 1
            While Not row.IsparentIDNull
                row = MenuItems.FindByid(row.parentID)
                currentlvl += 1
            End While
            moveSteps = currentlvl - StartDepth + FixedDepthtravel
        Else
            currentlvl = -1
            moveSteps = 0
        End If


        If currentlvl < StartDepth - FixedDepthtravel Then
            Return New DataView(MenuItems, "1=2", "sortOrder", DataViewRowState.CurrentRows)
        End If
        row = currentrow

        For i As Integer = 0 To moveSteps - 1
            If Not row Is Nothing Then
                If row.IsparentIDNull Then row = Nothing Else row = MenuItems.FindByid(row.parentID)
            End If
        Next

        If row Is Nothing Then
            filtStr = "parentID is Null   "
        Else
            filtStr = "parentID=" & row.id
        End If

        Dim dv As New DataView(MenuItems, "", "sortOrder", DataViewRowState.CurrentRows)
        If Not row Is Nothing AndAlso FixedDepthtravel = 0 Then dv.RowFilter = "id =" & row.id
        For i As Integer = 0 To FixedDepthtravel - 1
            dv.RowFilter = filtStr
            If dv.Count = 0 Then Return dv

            filtStr = "parentID in ("
            For Each rv As Data.DataRowView In dv
                filtStr &= rv("id") & " ,"
            Next
            'filtStr = filtStr.Substring(0, filtStr.Length - 2)
            filtStr = filtStr.Trim(",") & ")"
            'Itereate through all the rows in thelist at that level
        Next
        Return dv
    End Function

    Private Sub Menu_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        sqlhelper.checkAndCreateTable(ds.DTIDynamicPage)
        sqlhelper.checkAndCreateTable(ds.DTIPageHeiarchy)
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Not DesignMode Then
            Dim s As String = RenderMenu()
            Me.Controls.Clear()
            Me.Controls.Add(New LiteralControl(s))
            MyBase.Render(writer)
        End If
    End Sub
End Class
