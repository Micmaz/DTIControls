Public Partial Class SliderTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Slider1.UpdateControl = TextBox1
        'Slider1.Range = JqueryUIControls.Slider.limit.true
        'Slider1.Values.Add(17)
        'Slider1.Values.Add(47)
        slHrs.SlideCallback = "var num = ui.value+'';if (num.length == 1)num = '0' + ui.value;$('#" & tbSelHrs.ClientID & "').val(num);"
        slMin.SlideCallback = "var num = ui.value+'';if (num.length == 1)num = '0' + ui.value;$('#" & tbSelMins.ClientID & "').val(num);"
    End Sub

    'Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Dim i = Slider1.Value
    'End Sub
End Class