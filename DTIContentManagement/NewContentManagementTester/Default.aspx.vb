Imports DTIServerControls

Partial Public Class _Default
    Inherits BaseClasses.BaseSecurityPage

    Public Overridable Sub addTheme()
		'jQueryLibrary.ThemeAdder.AddTheme(Me, jQueryLibrary.ThemeAdder.themes.overcast)
		'jQueryLibrary.ThemeAdder.AddCustomTheme(Me, "jquery-ui-1.8.22.custom.css")
		'jQueryLibrary.ThemeAdder.AddTheme(Me)
		'JqueryUIControls.Dialog.registerControl(Me)
	End Sub

    Private Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
		If Request.QueryString("m") IsNot Nothing Then DTIControls.MainId = Request.QueryString("m")
	End Sub

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		If Not IsPostBack Then
			cbEdit.Checked = DTIControls.EditModeOn
		End If
		addTheme()
		For i As Integer = 0 To 5
			Dim uc1 As WebUserControl1 = Page.LoadControl("WebUserControl1.ascx")
			uc1.number = i
			PlaceHolder1.Controls.Add(uc1)
		Next
	End Sub

	Private Sub cbEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbEdit.CheckedChanged
		DTIControls.LoggedIn = cbEdit.Checked
		DTIControls.EditModeOn = cbEdit.Checked
		Response.Redirect(Request.Url.OriginalString)
    End Sub


End Class