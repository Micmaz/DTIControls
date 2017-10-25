Public Class sourceView
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.repcontent.language = DTIMiniControls.HighlighedEditor.languageEnum.html
        repcontent.lineNumbers = True
        repcontent.Wrap = True
        'Me.repcontent.matchBrackets = True
        'Me.repcontent.theme = DTIMiniControls.HighlighedEditor.themeEnum.neat
    End Sub

End Class