Imports DTIServerControls
Imports DTIMediaManager

#If DEBUG Then
Partial Public Class CHPage
    Inherits DTIBasePage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class CHPage
        Inherits DTIBasePage
#End If

#Region "Properties"

        Private _pagename As String = ""
        Private ReadOnly Property pagename() As String
            Get
                If _pagename = "" Then
                    If Request.Params("pagename") Is Nothing Then
                        Dim url As String = Request.Url.ToString
                        Try
                            _pagename = url.Substring(url.LastIndexOf("/") + 1).Replace(".aspx", "")
                        Catch ex As Exception
                            Return ""
                        End Try
                    Else
                        _pagename = Request.Params("pagename")
                    End If
                End If
                Return _pagename
            End Get
        End Property

        Private _pageID As Integer = -1
        Private Property PageID() As Integer
            Get
                Return _pageID
            End Get
            Set(ByVal value As Integer)
                _pageID = value
            End Set
        End Property

        Private _cphColl As Collection
        Private Property cphColl(ByVal masterpageName As String) As Collection
            Get
                If Session("DTIAdminPanel.ContentPlaceHolderCollection") Is Nothing Then
                    Session("DTIAdminPanel.ContentPlaceHolderCollection") = New Hashtable
                End If
                Dim ht As Hashtable = Session("DTIAdminPanel.ContentPlaceHolderCollection")
                If ht(masterpageName) Is Nothing Then
                    ht(masterpageName) = New Collection
                End If
                Return ht(masterpageName)
            End Get
            Set(ByVal value As Collection)
                If Session("DTIAdminPanel.ContentPlaceHolderCollection") Is Nothing Then
                    Session("DTIAdminPanel.ContentPlaceHolderCollection") = New Hashtable
                End If
                Session("DTIAdminPanel.ContentPlaceHolderCollection")(masterpageName) = value
            End Set
        End Property

        Public ReadOnly Property Sitemasterpage() As String
            Get
                Return Session("DTIAdminPanel.sitemasterpage")
            End Get
        End Property

        Private _phTable As New dsDTIAdminPanel.MenuItemDataTable
        Public ReadOnly Property menuitems() As dsDTIAdminPanel.MenuItemDataTable
            Get
                If Not DesignMode Then
                    If Page.Session("DTIAdminPanel.DTIPageHeiarchy") Is Nothing Then
                        _phTable.Clear()

                        Dim ds As New dsDTIAdminPanel
                        sqlHelper.checkAndCreateTable(ds.DTIPageHeiarchy)
                        sqlHelper.checkAndCreateTable(ds.DTIDynamicPage)

                        sqlHelper.FillDataTable("SELECT     DTIPageHeiarchy.*, DTIDynamicPage.PageName, DTIDynamicPage.Link, DTIDynamicPage.AdminOnly, DTIDynamicPage.MasterPage, DTIDynamicPage.Template " & _
                                                "FROM         DTIPageHeiarchy LEFT OUTER JOIN " & _
                                                "                      DTIDynamicPage ON DTIPageHeiarchy.DTIDynamicPage = DTIDynamicPage.id " & _
                                                "Where DTIPageHeiarchy.MainId = " & MainID & _
                                                " Order by DTIPageHeiarchy.SortOrder", _phTable)
                        For Each row As dsDTIAdminPanel.MenuItemRow In _phTable
                            row.Visible = Not row("isHidden")
                        Next
                        Page.Session("DTIAdminPanel.DTIPageHeiarchy") = _phTable
                    End If
                    Return Page.Session("DTIAdminPanel.DTIPageHeiarchy")
                Else
                    Return Nothing
                End If
            End Get
        End Property
#End Region

        Private Template As Integer = 0
        Private DTISortable1 As DTISortable.DTISortable
        Private DTISortable2 As DTISortable.DTISortable
        Private DTISortable3 As DTISortable.DTISortable
        'Private edit1 As DTIContentManagement.EditPanel
        Private dt As New dsDTIAdminPanel.DTIDynamicPageDataTable
        Private dv As DataView


        Private Sub setMasterpage(ByVal masterpage As String)
            'Me.MasterPageFile = masterpage
            Dim location As String = AppDomain.CurrentDomain.BaseDirectory
            If System.IO.File.Exists(location & masterpage.Replace("~", "")) Then
                Me.MasterPageFile = masterpage
            ElseIf System.IO.File.Exists(location & Sitemasterpage) Then
                Me.MasterPageFile = Sitemasterpage
            Else
                For Each mpfile As String In System.IO.Directory.GetFiles(location, "*.Master", IO.SearchOption.AllDirectories)
                    Me.MasterPageFile = mpfile.Replace(location, "/")
                    Exit For
                Next
            End If
        End Sub

        Private Sub CHPage_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'Setting the master page

            BaseClasses.BaseVirtualPathProvider.registerVirtualPathProvider()
            If menuitems IsNot Nothing Then
                dv = New DataView(menuitems, "pagename like '" & pagename & "'", "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    If dv(0)("link") Is DBNull.Value OrElse dv(0)("link") = "" Then
                        If dv(0)("MasterPage") IsNot DBNull.Value OrElse dv(0)("MasterPage") <> "" Then
                            setMasterpage(dv(0)("MasterPage"))
                        Else
                            setMasterpage(Sitemasterpage)
                        End If
                        If dv(0)("Template") IsNot DBNull.Value Then Template = dv(0)("Template")
                    Else
                        If Not dv(0)("link").ToString.StartsWith("/") Then
                            Response.Redirect("/" & dv(0)("link"), True)
                        Else
                            Response.Redirect(dv(0)("link"), True)
                        End If

                    End If
                    PageID = dv(0)("DTIDynamicPage")
                Else
                    'Only happens if they visit a page that isn't in the menu
                    Dim dt As New dsDTIAdminPanel.DTIDynamicPageDataTable
                    Try
                        sqlHelper.SafeFillTable("select * from DTIDynamicPage where PageName like @PageName and mainId = @mainId", dt, New Object() {pagename, MainID})
                    Catch ex As Exception
                        sqlHelper.checkAndCreateTable(dt)
                    End Try

                    If dt.Count > 0 Then
                        If dt(0).IsLinkNull OrElse dt(0).Link = "" Then
                            If Not dt(0).IsMasterPageNull Then
                                setMasterpage(dt(0).MasterPage)
                            Else
                                setMasterpage(Sitemasterpage)
                            End If

                            Me.Title = dt(0).PageName
                            If Not dt(0).IsTemplateNull Then Template = dt(0).Template
                        Else
                            If Not dt(0).Link.StartsWith("/") Then
                                Response.Redirect("/" & dt(0).Link, True)
                            Else
                                Response.Redirect(dt(0).Link, True)
                            End If
                        End If
                        PageID = dt(0).id
                    Else
                        Me.Title = "none"
                    End If
                End If
            End If

            'Setting up sortable regions
            If cphColl(Me.MasterPageFile).Count = 0 Then
                cphColl(Me.MasterPageFile) = BaseClasses.Spider.spidercontrolforTypeArray(Me.Master, GetType(ContentPlaceHolder))
                Response.Redirect(Request.Url.OriginalString, True)
            End If

            For Each cph As ContentPlaceHolder In cphColl(Me.MasterPageFile)
                MyBase.AddContentTemplate(cph.ID, New CompiledTemplateBuilder(New BuildTemplateMethod(AddressOf Me.Build)))
            Next
        End Sub


        Private Sub Build(ByVal c As System.Web.UI.Control)
            If PageID > -1 Then
                Dim conType As String = "page_" & PageID & "_conPlaceHolder_" & c.ID
                Select Case Template
                    Case 0
                        Dim div1 As New Panel
                        div1.CssClass = "whole"
                        'edit1 = New DTIContentManagement.EditPanel
                        'edit1.contentType = conType
                        'edit1.MainID = MainID
                        'div1.Controls.Add(edit1)
                        c.Controls.Add(div1)
                    Case 1
                        Dim div1 As New Panel
                        div1.CssClass = "whole"
                        DTISortable1 = New DTISortable.DTISortable(conType & "_1")
                        With DTISortable1
                            .MainID = MainID
                            .HandleText = "Drag Me"
                            .ZIndex = 10000000
                        End With
                        div1.Controls.Add(DTISortable1)
                        c.Controls.Add(div1)
                    Case 2
                        Dim div1 As New Panel
                        Dim div2 As New Panel
                        DTISortable1 = New DTISortable.DTISortable(conType & "_1")
                        With DTISortable1
                            .MainID = MainID
                            .HandleText = "Drag Me"
                            .ZIndex = 10000000
                        End With

                        DTISortable2 = New DTISortable.DTISortable(conType & "_2")
                        With DTISortable2
                            .MainID = MainID
                            .HandleText = "Drag Me"
                            .ZIndex = 10000000
                        End With
                        div1.CssClass = "left"
                        div2.CssClass = "right"
                        div1.Controls.Add(DTISortable1)
                        c.Controls.Add(div1)
                        div2.Controls.Add(DTISortable2)
                        c.Controls.Add(div2)
                End Select
            End If
        End Sub

        Private regCss As String = Nothing
        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If dt.Count > 0 Then
                MyMediaRow = getPageMediaRow(dt(0).id)
            ElseIf dv.Count > 0 Then
                MyMediaRow = getPageMediaRow(dv(0)("DTIDynamicPage"))
            Else
                MyMediaRow = getPageMediaRowFromURL(Request.Url.PathAndQuery)
            End If
            If Not regCss Is Nothing Then
                jQueryLibrary.jQueryInclude.addScriptFile(Page, regCss, "text/css", True)
            End If
        End Sub

        Protected Function getPageMediaRowFromURL(ByVal staticPageURL As String) As dsMedia.DTIMediaManagerRow
            Dim mediaRow As DTIMediaManager.dsMedia.DTIMediaManagerRow
            If DTIServerControl.initExternalType(Me.GetType) Then
                Dim dstag As New DTITagManager.dsTagger
                sqlHelper.checkAndCreateTable(dstag.DTI_Content_Tags)
                sqlHelper.checkAndCreateTable(dstag.DTI_Content_Tag_Pivot)
            End If
            parallelDataHelper.addFillDataTable("select * from DTI_Content_Tags where Id in " & _
                    "(select Tag_Id from DTI_Content_Tag_Pivot where " & _
                    "Component_Type = 'page' and Content_Id in " & _
                    "(select Id from DTIDynamicPage where Link like '%" & staticPageURL & "%'))", tagTable)
            parallelDataHelper.addFillDataTable("select * from DTIMediaManager where Content_Id in " & _
                "(select Id from DTIDynamicPage where Link like '%" & staticPageURL & "%')" & _
                " and Content_Type = 'page'", SharedMediaVariables.myMediaTable)

            Dim dt As New dsDTIAdminPanel.DTIDynamicPageDataTable
            Dim pageRow As dsDTIAdminPanel.DTIDynamicPageRow
            parallelDataHelper.addFillDataTable("select * from DTIDynamicPage where Link like '%" & staticPageURL & "%'", dt)

            parallelDataHelper.executeParallelDBCall()

            For Each row As dsDTIAdminPanel.DTIDynamicPageRow In dt
                If row.Link.Contains(staticPageURL) Then
                    pageRow = row
                    Exit For
                End If
            Next

            If Not pageRow Is Nothing Then
                Dim dv As New DataView(SharedMediaVariables.myMediaTable, "Content_Type = 'page' and Content_Id = " & _
                    pageRow.id, "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    mediaRow = dv(0).Row
                End If

                If mediaRow Is Nothing Then
                    mediaRow = SharedMediaVariables.myMediaTable.NewDTIMediaManagerRow
                    With mediaRow
                        .Component_Type = "ContentManagement"
                        .Content_Type = "page"
                        .Content_Id = pageRow.id
                        .Date_Added = Now
                        .Published = True
                        .Removed = False
                        .User_Id = -1 'currentUser.id
                        .Permanent_URL = Request.Url.PathAndQuery
                    End With
                    SharedMediaVariables.myMediaTable.AddDTIMediaManagerRow(mediaRow)
                    sqlHelper.Update(mediaRow.Table)
                End If
            End If

            Return mediaRow
        End Function

    End Class