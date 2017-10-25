Public Partial Class UploaderServerControlTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub DTIUploaderControl1_handleFile(ByRef file As System.Web.HttpPostedFile) Handles DTIUploaderControl1.handleFile
        'file.SaveAs(MapPath("/") & file.FileName)
    End Sub
End Class