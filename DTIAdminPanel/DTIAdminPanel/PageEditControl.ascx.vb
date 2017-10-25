#If DEBUG Then
Partial Public Class PageEditControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class PageEditControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If

    Public dynpage As dsDTIAdminPanel.DTIDynamicPageRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack AndAlso dynpage IsNot Nothing Then
            tbPageName.Text = dynpage.PageName
            'rblTemplate.SelectedValue = dynpage.Template

            If Not dynpage.IsLinkNull Then tbLink.Text = dynpage.Link

            cbAdmin.Checked = (Not dynpage.IsAdminOnlyNull AndAlso dynpage.AdminOnly)
            cbUsers.Checked = (Not dynpage.IsUsersOnlyNull AndAlso dynpage.UsersOnly)

            binddll()
            If dynpage.IsLinkNull Then
                ddlMasterPage.SelectedValue = dynpage.MasterPage
            Else
                ddlMasterPage.Enabled = False
                'rblTemplate.Enabled = False
            End If

            Dim link As String = "http://" & Request.Url.Authority & "/"
            If dynpage.IsLinkNull Then
                link &= "page/" & dynpage.PageName
            Else
                If dynpage.Link.StartsWith("http://") Then
                    link = dynpage.Link
                Else
                    link &= dynpage.Link
                End If
            End If
            lbl.InnerText = link
        End If
    End Sub

    Private Sub binddll()
        Dim location As String = AppDomain.CurrentDomain.BaseDirectory
        For Each masterPage As String In System.IO.Directory.GetFiles(location, "*.Master", IO.SearchOption.AllDirectories)
            masterPage = masterPage.Replace(location, "~/").Replace("\", "/")
            Dim last As Integer = masterPage.LastIndexOf("/") + 1
            ddlMasterPage.Items.Add(New ListItem(masterPage.Substring(last, masterPage.Length - last), masterPage))
        Next
    End Sub

    Public Sub save()
        If dynpage IsNot Nothing Then
            With dynpage
                If tbPageName.Text <> "" Then
                    .PageName = tbPageName.Text
                End If
                If tbLink.Text <> "" Then
                    .Link = tbLink.Text
                Else
                    .SetLinkNull()
                End If
                .AdminOnly = cbAdmin.Checked
                .UsersOnly = cbUsers.Checked
                '.Template = rblTemplate.SelectedValue
                .MasterPage = ddlMasterPage.SelectedValue
            End With
            sqlHelper.Update(dynpage.Table)
            'clears out menu items and page information from session
            Session("DTIAdminPanel.DTIPageHeiarchy") = Nothing
        End If
    End Sub
End Class