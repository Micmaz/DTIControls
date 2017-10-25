Partial Public Class Socialists
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            For i As Integer = 1 To 20
                StarRater1.Items.Add(New ListItem(i))
            Next
        End If
        'StarRater1.Enabled = False
        StarRater1.SelectedValue = 15
        StarRater1.CallbackFunction = "function() {$('body').append($('input:radio[name=""" & StarRater1.ID & """]:checked').val());}"

    End Sub

    Private Sub b1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles b1.Click
        Dim i As Integer = StarRater1.SelectedValue

    End Sub
End Class