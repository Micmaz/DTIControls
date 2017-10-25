Public Partial Class Test3
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DTISortable.DTIWidgetMenu.addUserControlToMenu("/WebUserControl1.ascx", "Awesome adder control")
    End Sub

    Private Sub _Default_LoadNoPostBack(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadNoPostBack
        Dim location As String = AppDomain.CurrentDomain.BaseDirectory
        For Each masterPage As String In System.IO.Directory.GetFiles(location, "*.Master", IO.SearchOption.AllDirectories)
            masterPage = masterPage.Replace(location, "~/").Replace("\", "/")
            Dim last As Integer = masterPage.LastIndexOf("/") + 1
            ddlTemplate.Items.Add(New ListItem(masterPage.Substring(last, masterPage.Length - last).Replace(".Master", ""), masterPage))
        Next
        ddlTemplate.SelectedValue = SiteTemplate
    End Sub

    Private Sub ddlTemplate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTemplate.SelectedIndexChanged
        SiteTemplate = ddlTemplate.SelectedValue
        TemplateChanged = True
        Response.Redirect(Request.Url.OriginalString)
    End Sub

End Class