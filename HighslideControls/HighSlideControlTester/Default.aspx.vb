Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected WithEvents btn1 As New Button
    Protected WithEvents cb1 As New CheckBox

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'highslideheader.HighslideVariables.Add("outlineType", "rounded-white")
            If Not hsGalleryImage.HighslideVariables.Contains("zIndexCounter") Then
                hsGalleryImage.HighslideVariables.Add("zIndexCounter", 200000)
            End If
            'LinkButton1.OnClientClick = Highslider1.openJSFunction
        End If

        btn1.Text = "HAAAY!"
        'btn1.ID = "btn1"
        cb1.AutoPostBack = True
        Highslider4.ContentControls.Add(cb1)
        Highslider4.ContentControls.Add(btn1)

        'pnlDynamic.Controls.Add(cb1)
        'Highslider4.maincontentId = pnlDynamic.ClientID
    End Sub

    Private Sub btn1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn1.Click
        Dim x = 1
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim x = 1
    End Sub

    Private Sub cb1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cb1.CheckedChanged
        Dim x = 1
    End Sub
End Class