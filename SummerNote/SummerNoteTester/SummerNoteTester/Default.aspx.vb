Partial Public Class _Default
	Inherits System.Web.UI.Page

	Private WithEvents editor As SummerNote.SummerNote

	Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		'Me.EnableViewState = False
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		editor = New SummerNote.SummerNote
		editor.Text = "dynamically added"
		'Dim i As String = editor.ClientID
		Me.PlaceHolder1.Controls.Add(editor)

		'Dim dlg As New SummerNote.IframeDialog()
		'dlg.Name = "uploader"
		'dlg.IconPath = "/icon.png"
		'dlg.DialogTitle = "Image Uploader"
		'dlg.IframeURL = "/dialog/upload.html"
		'dlg.DialogHeight = 200
		'dlg.DialogWidth = 200
		'dlg.ToolTip = "Upload Images..."
		'dlg.DialogAction = "alert('hi');"
		' SummerNote1.addIframeDialog(dlg)

		'SummerNote1.ClientReady = "$(this).animate({border:null}, 100);"
		'SummerNote1.ClientDestroyed = "$(this).animate({border:'3px dashed #777777'}, 100);"
		'Dim dlg2 As New SummerNote.SummerNote.IframeDialog()
		'dlg2.Name = "uploader"
		'dlg2.IconPath = "/icon2.png"
		'dlg2.DialogTitle = "Image Uploader"
		'dlg2.IframeURL = "/upload.html"
		'dlg2.DialogHeight = 200
		'dlg2.DialogWidth = 200
		'dlg2.ToolTip = "Upload Images..."
		'SummerNote2.addIframeDialog(dlg2)
	End Sub

	Private Sub SummerNote1_Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles SummerNote1.Cancel_Clicked
		Dim i = 0
		'SummerNote1.myManager.ActiveEditor = Nothing
	End Sub

	Private Sub SummerNote1_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles SummerNote1.Save_Clicked
		Dim i = 0
	End Sub

	Private Sub SummerNote2_Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles SummerNote2.Cancel_Clicked
		Dim i = 0
	End Sub

	Private Sub SummerNote2_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles SummerNote2.Save_Clicked
		Dim i = 0
	End Sub

	Private Sub editor_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles editor.Save_Clicked
		Dim x As String
		x = editor.Text
	End Sub
End Class