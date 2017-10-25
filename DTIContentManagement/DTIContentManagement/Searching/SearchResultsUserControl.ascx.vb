Imports DTIImageManager
Imports DTIVideoManager
Imports DTIGallery.DTISlideGallery

#If DEBUG Then
Partial Public Class SearchResultsUserControl
    Inherits System.Web.UI.UserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class SearchResultsUserControl
        Inherits System.Web.UI.UserControl
#End If
        Public ReadOnly Property SearchButtonId() As String
            Get
                If gallPages.Visible Then
                    Return gallPages.mediaSearcher.btnSearch.ClientID
                ElseIf gallMedia.Visible Then
                    Return gallMedia.mediaSearcher.btnSearch.ClientID
                End If
            End Get
        End Property

        Public ReadOnly Property SearchTextBoxId() As String
            Get
                If gallPages.Visible Then
                    Return gallPages.mediaSearcher.tbSearch.ClientID
                ElseIf gallMedia.Visible Then
                    Return gallMedia.mediaSearcher.tbSearch.ClientID
                End If
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            gallMedia.mediaSearcher.Content_Types.AddRange(New String() {"Image", "Video"})
            gallMedia.Component_Type = "ContentManagement"
            gallMedia.jsonControl.workerclass = "DTIContentManagement.SearchResultsUserControl+SearchResultMediaGalleryAjaxWorker"
            gallMedia.ReturnResultsOnEmptySearch = False
        End Sub

        Private Sub btnMedia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMedia.Click
            gallMedia.mediaSearcher.SearchText = gallPages.mediaSearcher.SearchText
            gallMedia.Visible = True
            gallPages.Visible = False
        End Sub

        Private Sub btnPages_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPages.Click
            gallPages.mediaSearcher.SearchText = gallMedia.mediaSearcher.SearchText
            gallMedia.Visible = False
            gallPages.Visible = True
        End Sub

        Public Class SearchResultMediaGalleryAjaxWorker
            Inherits SlideGalleryAjaxWorker

            Protected Overrides Sub getThumbnail(ByRef content_type_row As DTIMediaManager.dsMedia.DTIMediaTypesRow, ByRef media_row As DTIMediaManager.dsMedia.DTIMediaManagerRow)
                Dim thumbInfo As New DTIMediaManager.MediaInfo
                Dim hsThumb As HighslideControls.Highslider

                If media_row.Content_Type = "Image" Then
                    hsThumb = New ImageThumb
                    With CType(hsThumb, ImageThumb)
                        If RefreshImages Then .RefreshImage = True
                        .MaxThumbWidth = MaximumThumbnailWidth
                        .MaxThumbHeight = MaximumThumbnailHeight
                        .ImageId = media_row.Content_Id
                        .wrapperClassName = "draggable-header"
                        .Outline_Scheme = HighSlide_Color_Mode
                    End With
                ElseIf media_row.Content_Type = "Video" Then
                    hsThumb = New VideoThumb
                    With CType(hsThumb, VideoThumb)
                        If RefreshImages Then .RefreshImage = True
                        .MaxThumbWidth = MaximumThumbnailWidth
                        .MaxThumbHeight = MaximumThumbnailHeight
                        .VideoId = media_row.Content_Id
                    End With
                End If

                addThumbNail(hsThumb, thumbInfo)

                thumbInfo.MediaRow = media_row
                thumbInfo.TitleIsLink = False
                thumbInfo.ShowDescription = False
                thumbInfo.ShowPubDate = True
                thumbInfo.ShowSharing = True
            End Sub

            Public Sub New()
                items_per_page = 8
            End Sub
        End Class
    End Class