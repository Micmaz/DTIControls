Partial Public Class strtest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For i As Integer = 0 To 100
            Dim star As New DTIMiniControls.StarRater
            For j As Integer = 1 To 5
                star.Items.Add(New ListItem(j))
            Next
            star.SelectedValue = 3
            Me.Panel1.Controls.Add(star)
        Next

    End Sub

End Class