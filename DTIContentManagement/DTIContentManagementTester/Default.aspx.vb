Partial Public Class _Default
    Inherits Page

    'Private Sub CkEditor1_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkEditor1.Save_Clicked
    '    Dim i As Integer = 1
    'End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim s As String = EditPanelStatic.Text
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        DTIControls.Share.siteEditMainID = 2
        DTIControls.Share.EditModeOn = True
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        DTIControls.Share.siteEditMainID = 1
        DTIControls.Share.EditModeOn = True
    End Sub

End Class