Public Class UploaderTest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

	Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		For Each file As String In testul.fileList
			lblFileList.Text &= file & ",  "
		Next
	End Sub
End Class