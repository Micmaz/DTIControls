Public Partial Class TimerPickerTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        tpTimeIn.SelectedTime = Now.TimeOfDay
    End Sub

    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim i = 0
    End Sub
End Class