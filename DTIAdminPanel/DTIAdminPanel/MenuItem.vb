Imports DTIServerControls

''' <summary>
''' A menu item used in a menu. Can be added in code, to the aspx page or from data.
''' </summary>
''' <remarks></remarks>
<System.ComponentModel.Description("A menu item used in a menu. Can be added in code, to the aspx page or from data."),ToolboxData("<{0}:MenuItem ID=""MenuItem"" runat=""server""> </{0}:MenuItem>"),ComponentModel.ToolboxItem(False)> _
Public Class MenuItem
    Inherits DTIServerBase

#Region "properties"
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

    Private _phTable As dsDTIAdminPanel.MenuItemDataTable
    Private ReadOnly Property phTable() As dsDTIAdminPanel.MenuItemDataTable
        Get
            If _phTable Is Nothing Then
                _phTable = CType(BaseClasses.Spider.spiderUpforType(Me, GetType(Menu)), Menu).MenuItems
            End If
            Return _phTable
        End Get
    End Property

    Private ReadOnly Property dsi() As dsDTIAdminPanel
        Get
            If Page.Session("DTIAdminPanel.Pagelist") Is Nothing Then
                Page.Session("DTIAdminPanel.Pagelist") = New dsDTIAdminPanel
            End If
            Return Page.Session("DTIAdminPanel.Pagelist")
        End Get
    End Property

    Private _DepthOfMenu As Integer = 2
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Property DepthOfMenu() As Integer
        Get
            Return _DepthOfMenu
        End Get
        Set(ByVal value As Integer)
            _DepthOfMenu = value
        End Set
    End Property

    Private ReadOnly Property loggedin() As Boolean
        Get
            Return DTISharedVariables.LoggedIn
        End Get
    End Property
#End Region

    Private currentDepth As Integer = 0

    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)> _
    Public Function RenderMenuItem(ByVal selectedCss As String, ByVal selectParent As Boolean, Optional ByVal id As Integer = -1) As LiteralControl
        Dim options As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.CultureInvariant Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled
        Dim txt As New LiteralControl
        Dim dv As New DataView(phTable, "ParentId = " & id, "SortOrder", DataViewRowState.CurrentRows)
        Dim hasChildren As Regex = New Regex("<hasChildren>(?<Name>.*?)</hasChildren>", options)
        Dim hasNoChildren As Regex = New Regex("<hasNoChildren>(?<Name>.*?)</hasNoChildren>", options)

        'Dim selectedLI As Regex = New Regex("<li>", options)
        Dim selectedLI As Regex
        Dim linkreg As Regex = New Regex("\#\#link\#\#", options)
        Dim namereg As Regex = New Regex("\#\#name\#\#", options)
        Dim idreg As Regex = New Regex("\#\#id\#\#", options)
        Dim selectedClass As Regex = New Regex("\#\#selected\#\#", options)

        Dim link As String = ""
        Dim name As String = ""

        txt.Text = ""

        For Each itm As Control In Me.Controls
            If TypeOf itm Is LiteralControl Then
                txt.Text &= CType(itm, LiteralControl).Text
            ElseIf itm.GetType Is GetType(MenuItem) Then
                Dim currentitem As MenuItem = CType(itm, MenuItem)
                currentitem.DepthOfMenu = DepthOfMenu
                currentDepth += 1
                If currentDepth < DepthOfMenu OrElse DepthOfMenu = -1 Then
                    For Each crow As DataRowView In dv
                        Dim ht As Hashtable = CType(BaseClasses.Spider.spiderUpforType(Me, GetType(Menu)), Menu).ItemToData

                        If ht.Contains(crow("id")) Then
                            txt.Text &= CType(ht(crow("id")), MenuItem).RenderMenuItem(selectedCss, selectParent, crow.Item("id")).Text
                        Else
                            txt.Text &= currentitem.RenderMenuItem(selectedCss, selectParent, crow.Item("id")).Text
                        End If
                    Next
                End If
                currentDepth -= 1
            Else
                Dim sb As New StringBuilder()
                Using sw As New IO.StringWriter(sb)
                    Using textWriter As New HtmlTextWriter(sw)
                        itm.RenderControl(textWriter)
                    End Using
                End Using
                txt.Text &= sb.ToString()
            End If
        Next

        Dim phtRow As dsDTIAdminPanel.MenuItemRow = phTable.FindByid(id)
        If phtRow Is Nothing Then
            _phTable = Nothing
            phtRow = phTable.FindByid(id)
        End If

        If phtRow IsNot Nothing Then
            If phtRow.IsAdminOnlyNull OrElse Not phtRow.AdminOnly OrElse (phtRow.AdminOnly AndAlso loggedin) Then
                link = getPageLink(phtRow)

                If phtRow.IsDisplayNameNull OrElse phtRow.DisplayName = "" Then name = "Unknown" Else name = phtRow.DisplayName

                txt.Text = txt.Text.Replace(vbCrLf, "")
                txt.Text = txt.Text.Replace(vbTab, "")


                dv.RowFilter = "ParentId = " & id & " AND isHidden = 0"
                If dv.Count = 0 OrElse currentDepth = DepthOfMenu - 1 Then
                    txt.Text = hasChildren.Replace(txt.Text, "")
                    txt.Text = hasNoChildren.Replace(txt.Text, "$1")
                Else
                    txt.Text = hasChildren.Replace(txt.Text, "$1")
                    txt.Text = hasNoChildren.Replace(txt.Text, "")
                End If

                txt.Text = linkreg.Replace(txt.Text, link)
                txt.Text = namereg.Replace(txt.Text, name)
                txt.Text = idreg.Replace(txt.Text, id)

                Dim page As String = Me.Page.Request.Url.LocalPath

                page = page.Substring(page.LastIndexOf("/"))
                page = page.TrimStart("/").Replace(".aspx", "")
                If page.ToLower = "page" Then
                    page = Me.Page.Request.QueryString("pagename")
                End If
                page = page.ToLower

                selectedLI = New Regex("<a\s+href=\""" & link.Replace(" ", "\s"), options)

                If (Not phtRow.IsPageNameNull AndAlso phtRow.PageName.ToLower = page) _
                    OrElse (Not phtRow.IsLinkNull AndAlso phtRow.Link.ToLower.Replace("/", "").Replace(".aspx", "") = page) Then
                    If selectedLI.IsMatch(txt.Text) Then
                        If selectParent AndAlso Not phtRow.IsparentIDNull Then
                            'if we wanted to select more parents like 2 parents high this is where we would do it
                            selectedParentPage = "href=""" & getPageLink(phTable.FindByid(phtRow.parentID)) & """"
                        Else
                            txt.Text = selectedLI.Replace(txt.Text, "<a href=""" & link & """ class=""" & selectedCss, 1)
                            selectedParentPage = ""
                        End If
                    End If
                End If

                If selectedParentPage <> "" AndAlso txt.Text.Contains(selectedParentPage) Then
                    txt.Text = txt.Text.Replace(selectedParentPage, selectedParentPage & " class=""" & selectedCss & """")
                    selectedParentPage = ""
                End If

                If Not phtRow.Visible Then
                    txt.Text = ""
                End If
            Else
                txt.Text = ""
            End If
        End If
        txt.Text &= vbCrLf
        Return txt
    End Function

    Private Function getPageLink(ByRef phtrow As dsDTIAdminPanel.MenuItemRow) As String
        If phtrow.IsDTIDynamicPageNull Then
            Return "#"
        ElseIf phtrow.IsLinkNull OrElse phtrow.Link = "" Then
            Return "/page/" & phtrow.PageName & ".aspx"
        ElseIf phtrow.Link.StartsWith("http://") Then
            Return phtrow.Link
        Else : Return "/" & phtrow.Link
        End If
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

    End Sub
End Class
