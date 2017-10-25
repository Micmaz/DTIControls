Imports DTIServerControls
Imports DTISortable
Imports DTIMiniControls

''' <summary>
''' Adds dynamic page creation and ability to switch to layout, edit or preview mode. Best used on a master page where automatic templates will take effect.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("Adds dynamic page creation and ability to switch to layout, edit or preview mode. Best used on a master page where automatic templates will take effect."),ToolboxData("<{0}:DTIAdminPanel ID=""AdminPanel"" runat=""server"" />")> _
Public Class DTIAdminPanel
    Inherits DTIServerBase

    Protected WithEvents Button1 As New Button
    Protected WithEvents btnSave As New Button
    Protected WithEvents RadioButtonList1 As New RadioButtonList
    Private men As New DTIWidgetMenu
    Private recyc As New DTIRecycleBin
    Private tsLoggedIn As New TabSlider
    Private tsRecyc As New TabSlider
    Private tsMenu As New TabSlider

#Region "Properties"

#Region "session properties"

    ''' <summary>
    ''' Sets the Admin mode to Edit.  Show all components in Admin Mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the Admin mode to Edit. Show all components in Admin Mode")> _
    Public Property EditOn() As Boolean
        Get
            Return DTISharedVariables.AdminOn
        End Get
        Set(ByVal value As Boolean)
            DTISharedVariables.AdminOn = value
        End Set
    End Property

	

    ''' <summary>
    ''' Sets the admin Mode to Layout allowing items to be re-ordered
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the admin Mode to Layout allowing items to be re-ordered")> _
    Public Property LayoutOn() As Boolean
        Get
            Return DTIServerControls.DTISharedVariables.LayoutOn
        End Get
        Set(ByVal value As Boolean)
            DTIServerControls.DTISharedVariables.LayoutOn = value
        End Set
    End Property

    ''' <summary>
    ''' Determines if the Admin panel is visible 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Determines if the Admin panel is visible")> _
    Public Property LoggedIn() As Boolean
        Get
            Return DTISharedVariables.LoggedIn
        End Get
        Set(ByVal value As Boolean)
            DTISharedVariables.LoggedIn = value
        End Set
    End Property
#End Region

#Region "Tab properties"

#Region "admin"
    Private _adminImageUrl As String = BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/admin.png"

    ''' <summary>
    ''' sets Url to override Admin Panel Image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("sets Url to override Admin Panel Image")> _
    Public Property adminImageUrl() As String
        Get
            Return _adminImageUrl
        End Get
        Set(ByVal value As String)
            _adminImageUrl = value
        End Set
    End Property

    Private _adminImageHeight As Unit = New Unit(39, UnitType.Pixel)

    ''' <summary>
    ''' Height for the Admin Panel image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Height for the Admin Panel image")> _
    Public Property adminImageHeight() As Unit
        Get
            Return _adminImageHeight
        End Get
        Set(ByVal value As Unit)
            _adminImageHeight = value
        End Set
    End Property

    Private _adminImageWidth As Unit = New Unit(161, UnitType.Pixel)

    ''' <summary>
    ''' Width of the Admin Panel Image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Width of the Admin Panel Image")> _
    Public Property adminImageWidth() As Unit
        Get
            Return _adminImageWidth
        End Get
        Set(ByVal value As Unit)
            _adminImageWidth = value
        End Set
    End Property

    Private _fixedScrollPosition As Boolean = False

    ''' <summary>
    ''' Keeps Admin panel at top of page during scrolling
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Keeps Admin panel at top of page during scrolling")> _
    Public Property FixedScrollPosition() As Boolean
        Get
            Return _fixedScrollPosition
        End Get
        Set(ByVal value As Boolean)
            _fixedScrollPosition = value
        End Set
    End Property
#End Region

#Region "Menu"
    Private _menuImageUrl As String = BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/Toolbox.png"

    ''' <summary>
    ''' Sets the Url for to override the menu image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the Url for to override the menu image")> _
    Public Property menuImageUrl() As String
        Get
            Return _menuImageUrl
        End Get
        Set(ByVal value As String)
            _menuImageUrl = value
        End Set
    End Property

    Private _menuImageHeight As Unit = New Unit(161, UnitType.Pixel)

    ''' <summary>
    ''' Height for the Admin Panel image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Height for the Admin Panel image")> _
    Public Property menuImageHeight() As Unit
        Get
            Return _menuImageHeight
        End Get
        Set(ByVal value As Unit)
            _menuImageHeight = value
        End Set
    End Property

    Private _menuImageWidth As Unit = New Unit(39, UnitType.Pixel)

    ''' <summary>
    ''' width for the Admin Panel image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("width for the Admin Panel image")> _
    Public Property menuImageWidth() As Unit
        Get
            Return _menuImageWidth
        End Get
        Set(ByVal value As Unit)
            _menuImageWidth = value
        End Set
    End Property
#End Region

#Region "recycebin"
    Private _recycImageUrl As String = BaseClasses.Scripts.ScriptsURL & "DTIAdminPanel/RecycleBin.png"

    ''' <summary>
    ''' Sets the Url for to override the menu image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets the Url for to override the menu image")> _
    Public Property recycImageUrl() As String
        Get
            Return _recycImageUrl
        End Get
        Set(ByVal value As String)
            _recycImageUrl = value
        End Set
    End Property

    Private _recycImageHeight As Unit = New Unit(161, UnitType.Pixel)

    ''' <summary>
    ''' Height for the Admin Panel image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Height for the Admin Panel image")> _
    Public Property recycImageHeight() As Unit
        Get
            Return _recycImageHeight
        End Get
        Set(ByVal value As Unit)
            _recycImageHeight = value
        End Set
    End Property

    Private _recycImageWidth As Unit = New Unit(39, UnitType.Pixel)

    ''' <summary>
    ''' width for the Admin Panel image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("width for the Admin Panel image")> _
    Public Property recycImageWidth() As Unit
        Get
            Return _recycImageWidth
        End Get
        Set(ByVal value As Unit)
            _recycImageWidth = value
        End Set
    End Property
#End Region

#End Region

    Private _cssTheme As String = "default"

    ''' <summary>
    ''' Sets theme so all css can be overridden
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Sets theme so all css can be overridden")> _
    Public Property CssTheme() As String
        Get
            Return _cssTheme
        End Get
        Set(ByVal value As String)
            _cssTheme = value
        End Set
    End Property
#End Region

   <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Overrides Function getDesignTimeHtml() As String
        Return " "
    End Function

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        loadAdminPanel()
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.Page.Request.QueryString("enablefirebug") Is Nothing AndAlso Me.Page.Request.QueryString("enablefirebug").ToLower = "y" Then
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIAdminPanel/firebug-lite.js")
        End If
        Session("DTIAdminPanel.sitemasterpage") = Page.MasterPageFile

        If Not Page.IsPostBack Then
            If EditOn Then
                RadioButtonList1.SelectedValue = "edit"
            ElseIf LayoutOn Then
                RadioButtonList1.SelectedValue = "layout"
            Else
                RadioButtonList1.SelectedValue = "preview"
            End If
        End If

        jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIAdminPanel/DTIAdminPanel.css", "text/css")
    End Sub

    Private Sub loadAdminPanel()
        Me.Controls.Clear()

        If LoggedIn Then
            Dim high As New HighslideControls.Highslider
            Dim tab As New Table
            Dim row1 As New TableRow
            Dim cell1 As New TableCell
            Dim cell2 As New TableCell
            Dim pan As New Panel

            cell1.Style.Add("padding-left", "10px")
            cell2.Style.Add("padding-right", "10px")
            cell1.Style.Add("padding-top", "5px")
            cell2.Style.Add("padding-top", "5px")
            cell1.Style.Add("padding-bottom", "5px")
            cell2.Style.Add("padding-bottom", "5px")
            With tsLoggedIn
                .Style.Add("z-index", "10000")
                .ImageUrl = adminImageUrl
                .ImageHeight = adminImageHeight
                .ImageWidth = adminImageWidth
                .TabLocation = TabSlider.Position.top
                .FixedScrollPosition = FixedScrollPosition
                .CssClass = "DTIAdminPanelSlideOut-" & CssTheme
                .Width = New Unit(100, UnitType.Percentage)
            End With

            Button1.Text = "Logout"
            btnSave.Text = "Save"

            With RadioButtonList1
                .Items.Add(New ListItem("Edit Site", "edit"))
                .Items.Add(New ListItem("Layout", "layout"))
                .Items.Add(New ListItem("Preview", "preview"))
                .TextAlign = TextAlign.Right
                .RepeatDirection = RepeatDirection.Horizontal
                .AutoPostBack = True
                .SelectedIndex = 2
                .CssClass = "DTIAdminRadioButtonList-" & CssTheme
            End With

            With high
                .HighslideDisplayMode = HighslideControls.SharedHighslideVariables.HighslideDisplayModes.Iframe
                .ExpandURL = "~/res/DTIAdminPanel/PageList.aspx?page=" & HttpUtility.UrlEncode(Page.Request.Path.Substring(1))
                .DisplayText = "Page List"
                .HighslideVariables.Add("width", 900)
                .HighslideVariables.Add("wrapperClassName", "draggable-header")
                .HighslideVariables.Add("outlineType", "rounded-white")
                .HighslideVariables.Add("align", "center")
            End With

            If MyHighslideHeader.HighslideVariables.Contains("zIndexCounter") Then
                MyHighslideHeader.HighslideVariables.Item("zIndexCounter") = 10001
            Else
                MyHighslideHeader.HighslideVariables.Add("zIndexCounter", 10001)
            End If

            tab.Width = New WebControls.Unit(100, UnitType.Percentage)
            tab.CellPadding = 0
            tab.CellSpacing = 0

            cell1.Controls.Add(high)

            cell2.Attributes.Add("Align", "right")
            cell2.Controls.Add(RadioButtonList1)
            cell2.Controls.Add(Button1)

            If LayoutOn Then cell2.Controls.Add(btnSave)

            row1.Cells.Add(cell1)
            row1.Cells.Add(cell2)

            tab.Rows.Add(row1)

            pan.Controls.Add(tab)
            tsLoggedIn.Controls.Add(pan)

            Me.Controls.Add(tsLoggedIn)
            Me.Style.Add("display", "none")
        End If

        If LayoutOn Then
            insertMenu()
        End If

    End Sub

    Private Sub insertMenu()
        'men.HandleText = "Drag Me"
        With tsMenu
            .Style.Add("z-index", "10000")
            .ImageUrl = menuImageUrl
            .ImageHeight = menuImageHeight
            .ImageWidth = menuImageWidth
            .FixedScrollPosition = FixedScrollPosition
            .TopPosition = New Unit(50, UnitType.Pixel)
            .CssClass = "DTIMenuSlideOut-" & CssTheme
            .Width = New Unit(200, UnitType.Pixel)
        End With
        tsMenu.Controls.Add(men)
        Me.Controls.Add(tsMenu)

        recyc.HandleText = "Click to Drag"
        With tsRecyc
            .Style.Add("z-index", "9999")
            .ImageUrl = recycImageUrl
            .ImageHeight = recycImageHeight
            .ImageWidth = recycImageWidth
            .FixedScrollPosition = FixedScrollPosition
            .TopPosition = New Unit(215, UnitType.Pixel)
            .CssClass = "DTIRecyceSlideOut-" & CssTheme
            .Width = New Unit(200, UnitType.Pixel)
        End With
        tsRecyc.Controls.Add(recyc)
        Me.Controls.Add(tsRecyc)
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        LoggedIn = False
        LayoutOn = False
        EditOn = False
        Page.Response.Redirect(Page.Request.Url.OriginalString, True)
    End Sub

    Private Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioButtonList1.SelectedIndexChanged
        If RadioButtonList1.SelectedValue = "edit" Then
            EditOn = True
            LayoutOn = False
        ElseIf RadioButtonList1.SelectedValue = "layout" Then
            EditOn = False
            LayoutOn = True
        Else
            EditOn = False
            LayoutOn = False
        End If

        Page.Response.Redirect(Page.Request.Url.OriginalString, True)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        recyc.Save()
        men.save(Me.Page)
    End Sub

    Private Sub DTIAdminPanelServer_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If LoggedIn Then
            Dim str As String = "<script type=""text/javascript"">" & vbCrLf
            str &= "$(document).ready(function() {" & vbCrLf
            str &= "     $(""#" & Me.ClientID & """).fadeIn(600);" & vbCrLf
            str &= "});" & vbCrLf
            str &= "function __doPostBack(eventTarget, eventArgument) { " & vbCrLf
            str &= "     if (!theForm.onsubmit || (theForm.onsubmit() != false)) { " & vbCrLf
            str &= "          theForm.__EVENTTARGET.value = eventTarget; " & vbCrLf
            str &= "          theForm.__EVENTARGUMENT.value = eventArgument; " & vbCrLf
            str &= "          $(theForm).submit(); " & vbCrLf
            str &= "      } " & vbCrLf
            str &= "} " & vbCrLf
            str &= "</script>" & vbCrLf
            writer.Write(str)
        End If
        MyBase.Render(writer)
    End Sub

    Private Sub DTIAdminPanel_typeFirstInitialized(ByVal t As System.Type) Handles Me.typeFirstInitialized
        Dim ds As New dsDTIAdminPanel
        sqlhelper.checkAndCreateTable(ds.DTIDynamicPage)
        sqlhelper.checkAndCreateTable(ds.DTIPageHeiarchy)
        sqlhelper.checkAndCreateTable(DTIMediaManager.SharedMediaVariables.myMediaTable)
    End Sub
End Class