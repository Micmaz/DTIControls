Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private WithEvents editor As ckEditor.ckEditor

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Me.EnableViewState = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        editor = New ckEditor.ckEditor
        editor.Text = "dynamically added"
        'Dim i As String = editor.ClientID
        Me.PlaceHolder1.Controls.Add(editor)

        Dim dlg As New ckEditor.IframeDialog()
        dlg.Name = "uploader"
        dlg.IconPath = "/icon.png"
        dlg.DialogTitle = "Image Uploader"
        dlg.IframeURL = "/dialog/upload.html"
        dlg.DialogHeight = 200
        dlg.DialogWidth = 200
        dlg.ToolTip = "Upload Images..."
        dlg.DialogAction = "alert('hi');"
        CkEditor1.addIframeDialog(dlg)

        'CkEditor1.ClientReady = "$(this).animate({border:null}, 100);"
        'CkEditor1.ClientDestroyed = "$(this).animate({border:'3px dashed #777777'}, 100);"
        Dim dlg2 As New ckEditor.IframeDialog()
        dlg2.Name = "uploader"
        dlg2.IconPath = "/icon2.png"
        dlg2.DialogTitle = "Image Uploader"
        dlg2.IframeURL = "/upload.html"
        dlg2.DialogHeight = 200
        dlg2.DialogWidth = 200
        dlg2.ToolTip = "Upload Images..."
        CkEditor2.addIframeDialog(dlg2)
    End Sub

    Private Sub CkEditor1_Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkEditor1.Cancel_Clicked
        Dim i = 0
        CkEditor1.myManager.ActiveEditor = Nothing
    End Sub

    Private Sub CkEditor1_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkEditor1.Save_Clicked
        Dim i = 0
    End Sub

    Private Sub CkEditor2_Cancel_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkEditor2.Cancel_Clicked
        Dim i = 0
    End Sub

    Private Sub CkEditor2_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkEditor2.Save_Clicked
        Dim i = 0
    End Sub

    Private Sub editor_Save_Clicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles editor.Save_Clicked
        Dim x As String
        x = editor.Text
    End Sub
End Class