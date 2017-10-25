Public Class WebUserControl1
    Inherits System.Web.UI.UserControl

    Public number As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        editPanel2.contentType = "NumberWhatever_" & number
        editPanel2.defaultText = "Default text for number: " & number
    End Sub

End Class