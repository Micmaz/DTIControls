''' <summary>
''' Gallery of only images.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTIImageGallery
    Inherits DTISlideGallery
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIImageGallery
        Inherits DTISlideGallery
#End If
        Private Sub DTIImageGallery_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            mediaSearcher.Content_Types.Add("Image")
        End Sub

        Private Sub DTIImageGallery_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            addUploadLink("Upload Images", "~/res/DTIGallery/UploadMedia.aspx?f=image")
        End Sub
    End Class
