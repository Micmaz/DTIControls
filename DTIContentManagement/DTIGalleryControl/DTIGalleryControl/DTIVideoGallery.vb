''' <summary>
''' A gallery of only videos.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTIVideoGallery
    Inherits DTISlideGallery
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIVideoGallery
        Inherits DTISlideGallery
#End If
        Private Sub DTIImageGallery_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            mediaSearcher.Content_Types.Add("Video")
        End Sub

        Private Sub DTIVideoGallery_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            addUploadLink("Upload Videos", "~/res/DTIGallery/UploadFiles.aspx?f=video")
        End Sub
    End Class
