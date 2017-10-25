#If DEBUG Then
Partial Public Class UploadMedia
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class UploadMedia
        Inherits BaseClasses.BaseSecurityPage
#End If
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIAdminPanel/iframe-default.css", "text/css")
            'DTIUploader.DTIUploaderControl.correctCookie()
            MediaUploader1.Component_Type = "ContentManagement"
            MediaUploader1.User_Id = -1 'DTIServerControls.DTISharedVariables.CurrentUser.Id
            MediaUploader1.RedirectURL = "~/res/DTIContentManagement/PreviewFiles.aspx"
        End Sub

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            'DTIUploader.DTIUploaderControl.correctCookie()
        End Sub


        Public Sub New()
            DTIUploader.DTIUploaderControl.correctCookie()
        End Sub
    End Class