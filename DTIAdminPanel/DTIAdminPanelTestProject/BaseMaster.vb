Public Class BaseMaster
    Inherits BaseClasses.MasterBase

    Public Property TemplateChanged() As Boolean
        Get
            If Session("HastheTemplateBeenChanged") Is Nothing Then
                Session("HastheTemplateBeenChanged") = False
            End If
            Return Session("HastheTemplateBeenChanged")
        End Get
        Set(ByVal value As Boolean)
            Session("HastheTemplateBeenChanged") = True
        End Set
    End Property

    Public Property SiteTemplate() As String
        Get
            If Session("MasterPageUsedForTemplatingWebsite") Is Nothing Then
                Session("MasterPageUsedForTemplatingWebsite") = "~/Puzzled.Master"
            End If
            Return Session("MasterPageUsedForTemplatingWebsite")
        End Get
        Set(ByVal value As String)
            Session("MasterPageUsedForTemplatingWebsite") = value
        End Set
    End Property

    Public Sub ToggleLoginVisibility(ByVal Menu As DTIAdminPanel.Menu)
        Try
            If DTIAdminPanel.LoginControl.isLoggedIn Then
                Menu.MenuItem("login").Visible = False
                Menu.MenuItem("logout").Visible = True
            Else
                Menu.MenuItem("login").Visible = True
                Menu.MenuItem("logout").Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ChangeTemplate(ByVal MasterPage As String, ByRef MenuItems As DTIAdminPanel.dsDTIAdminPanel.MenuItemDataTable)
        If TemplateChanged Then
            For Each row As DTIAdminPanel.dsDTIAdminPanel.MenuItemRow In MenuItems
                row.MasterPage = MasterPage
            Next
            TemplateChanged = False
        End If
    End Sub
End Class
