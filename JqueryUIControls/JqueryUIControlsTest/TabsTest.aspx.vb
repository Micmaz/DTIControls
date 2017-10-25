Public Partial Class TabsTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tabs.Tabs.add(New JqueryUIControls.Tab("btn1", New Button))
        tabs.Tabs.add(New JqueryUIControls.Tab("Ars1", "/ContentFrame.aspx", True))
        tabs.Tabs.add(New JqueryUIControls.Tab("test1", "/ContentFrame2.aspx", True))
        tabs.Tabs.add(New JqueryUIControls.Tab("Ars2", "/ContentFrame.aspx", True))
        tabs.Tabs.add(New JqueryUIControls.Tab("test2", "/ContentFrame2.aspx", True))
        tabs.Tabs.add(New JqueryUIControls.Tab("Ars3", "/ContentFrame.aspx", True))
        tabs.Tabs.add(New JqueryUIControls.Tab("test3", "/ContentFrame2.aspx", True))

        tabs.Tabs(3).Enabled = False
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim i = 0
    End Sub

    Protected Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim i = 0
        tabs.SelectedTabIndex = 2
    End Sub
End Class