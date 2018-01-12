Public Class ComboBox
    Inherits DropDownList

    Public Shared Sub registerControl(ByVal page As Page)
        If Not page Is Nothing Then
            jQueryLibrary.jQueryInclude.RegisterJQueryUI(page)
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/jquery.combobox.js")
            jQueryLibrary.jQueryInclude.addScriptFile(page, "JqueryUIControls/combobox.css")
        End If
    End Sub

    Private Sub ComboBox_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        registerControl(Me.Page)
    End Sub

    Private Sub ComboBox_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        Dim id As String = "" & Me.ID
        If id = "" Then id = ClientID

        Dim s As String = ""
		s &= "     $('#" & Me.ClientID & "').combobox();"
		jQueryLibrary.jQueryInclude.addScriptBlockPageLoad(Me.Page, s)
	End Sub
End Class
