Public Partial Class DatePickerTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Threading.Thread.CurrentThread.SpinWait(2000)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Literal1.Text = DatePicker1.SelectedDate
    End Sub
End Class