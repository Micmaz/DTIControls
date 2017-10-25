#If DEBUG Then
Partial Public Class VideoUploader
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class VideoUploader
        Inherits BaseClasses.BaseSecurityPage
#End If

        Private Sub VideoUploader_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            SecurePage = False
        End Sub
    End Class