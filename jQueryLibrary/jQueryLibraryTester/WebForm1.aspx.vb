Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        jQueryLibrary.ThemeAdder.AddTheme(Me)
        JqueryUIControls.Dialog.registerControl(Me)
    End Sub

End Class