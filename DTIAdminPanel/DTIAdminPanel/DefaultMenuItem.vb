Imports DTIServerControls

#If DEBUG Then
Public Class DefaultMenuItem
    Inherits MenuItem
#Else
    <ToolboxData("<{0}:DefaultMenuItem ID=""DefaultMenuItem"" runat=""server"" Name=""PageName""> </{0}:DefaultMenuItem>"), ComponentModel.ToolboxItem(False)> _
    Public Class DefaultMenuItem
        Inherits MenuItem
#End If

#Region "properties"
        Private _name As String = ""

        ''' <summary>
        ''' Name of the page and display of menu item to add to data
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Name of the page and display of menu item to add to data")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Private _link As String = ""

        ''' <summary>
        ''' Static link to the page being added to the database.  If blank a dynamic page will be added
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Static link to the page being added to the database. If blank a dynamic page will be added")> _
        Public Property Link() As String
            Get
                Return _link
            End Get
            Set(ByVal value As String)
                _link = value
            End Set
        End Property

        Private _adminonly As Boolean = False

        ''' <summary>
        ''' sets the page added to only display when admin panel is logged in
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("sets the page added to only display when admin panel is logged in")> _
        Public Property AdminOnly() As Boolean
            Get
                Return _adminonly
            End Get
            Set(ByVal value As Boolean)
                _adminonly = value
            End Set
        End Property

        Private _overrideTemplate As Boolean = False

        ''' <summary>
        ''' Allows this specific menuitem to be rendered differently based on it's Display name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Allows this specific menuitem to be rendered differently based on it's Display name")> _
        Public Property overrideTemplate() As Boolean
            Get
                Return _overrideTemplate
            End Get
            Set(ByVal value As Boolean)
                _overrideTemplate = value
            End Set
        End Property

        Private _AddToMenu As Boolean = True

        ''' <summary>
        ''' Set false to only add page to the pages table
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.ComponentModel.Description("Set false to only add page to the pages table")> _
        Public Property AddToMenu() As Boolean
            Get
                Return _AddToMenu
            End Get
            Set(ByVal value As Boolean)
                _AddToMenu = value
            End Set
        End Property

        Private _phTable As dsDTIAdminPanel.MenuItemDataTable
        Private ReadOnly Property phTable() As dsDTIAdminPanel.MenuItemDataTable
            Get
                If _phTable Is Nothing Then
                    _phTable = CType(BaseClasses.Spider.spiderUpforType(Me, GetType(Menu)), Menu).MenuItems
                End If
                Return _phTable
            End Get
        End Property

        Private Property ds() As dsDTIAdminPanel
            Get
                If Page.Session("DTIAdminPanel.Pagelist") Is Nothing Then
                    Page.Session("DTIAdminPanel.Pagelist") = New dsDTIAdminPanel
                End If
                Return Page.Session("DTIAdminPanel.Pagelist")
            End Get
            Set(ByVal value As dsDTIAdminPanel)
                Page.Session("DTIAdminPanel.Pagelist") = value
            End Set
        End Property
#End Region

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Function addToData(ByVal item As DefaultMenuItem, Optional ByVal parentid As Integer = -1) As Integer
        Dim dv As New DataView(ds.DTIPageHeiarchy, "DisplayName = '" & item.Name & "'", "id asc", DataViewRowState.CurrentRows)
        Dim itemId As Integer = -1

        If dv.Count = 0 Then
            Dim pageHRow As dsDTIAdminPanel.DTIPageHeiarchyRow = ds.DTIPageHeiarchy.NewDTIPageHeiarchyRow
            If item.AddToMenu Then
                With pageHRow
                    .DisplayName = item.Name
                    .MainID = MainID
                    .isHidden = False
                    If parentid = -1 Then
                        .SetparentIDNull()
                    Else
                        .parentID = parentid
                    End If
                End With
                ds.DTIPageHeiarchy.AddDTIPageHeiarchyRow(pageHRow)
            End If

            'Check if Dynamic Page already exists
            Dim dpv As New DataView(ds.DTIDynamicPage, "PageName = '" & item.Name & "'", "id asc", DataViewRowState.CurrentRows)
            If dpv.Count = 0 Then
                Dim dynamicpage As dsDTIAdminPanel.DTIDynamicPageRow = ds.DTIDynamicPage.NewDTIDynamicPageRow
                With dynamicpage
                    .PageName = item.Name
                    .MainID = MainID
                    If item.Link <> "" Then
                        .Link = item.Link
                    Else
                        .SetLinkNull()
                    End If
                    .MasterPage = Page.MasterPageFile
                    .AdminOnly = item.AdminOnly
                    .UsersOnly = False
                End With
                ds.DTIDynamicPage.AddDTIDynamicPageRow(dynamicpage)
                sqlhelper.Update(ds.DTIDynamicPage)
                If item.AddToMenu Then
                    pageHRow.DTIDynamicPage = dynamicpage.id
                End If
            ElseIf item.AddToMenu Then
                pageHRow.DTIDynamicPage = dpv(0).Item("id")
            End If

            If item.AddToMenu Then
                sqlhelper.Update(ds.DTIPageHeiarchy)
                itemId = pageHRow.id

                Dim menu As Menu = BaseClasses.Spider.spiderUpforType(Me, GetType(Menu))
                menu.refreshdata = True
            End If
        Else
            itemId = dv(0).Item("Id")
        End If

        If item.overrideTemplate AndAlso itemId > -1 Then
            Dim ht As Hashtable = CType(BaseClasses.Spider.spiderUpforType(Me, GetType(Menu)), Menu).ItemToData
            If Not ht.ContainsKey(itemId) Then
                ht.Add(itemId, item)
            End If

        End If

        For Each con As Control In item.Controls
            If con.GetType Is GetType(DefaultMenuItem) Then
                addToData(con, itemId)
            End If
        Next

        Return itemId
    End Function

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        End Sub
    End Class
