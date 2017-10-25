Imports System.Web.UI
Imports DTIMediaManager.SharedMediaVariables
Imports DTIVideoManager.dsDTIVideo
Imports DTIMediaManager.dsMedia
Imports DTIImageManager
Imports DTIVideoManager
Imports DTIGallery.DTISlideGallery

#If DEBUG Then
Partial Public Class MediaList
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class MediaList
        Inherits BaseClasses.BaseSecurityPage
#End If


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIAdminPanel/iframe-default.css", "text/css")
            'mediaGall.ShowSearching = False
            mediaGall.mediaSearcher.UseSortButtons = True
            mediaGall.mediaSearcher.Content_Types.AddRange(New String() {"Image", "Video"})
            mediaGall.Component_Type = "ContentManagement"
            mediaGall.IsInnerFrame = True
            mediaGall.ShowUpload = True
            mediaGall.ShowBorder = False
            'mediaGall.addUploadLink("Upload Media", "~/res/DTIContentManagement/UploadMedia.aspx")
            mediaGall.jsonControl.workerclass = "DTIContentManagement.MediaList+MediaListGalleryAjaxWorker"
            mediaGall.GalleryHeight = 410
        End Sub

        Private Sub Page_PreRender1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            mediaGall.mediaSearcher.UseSortButtons = True
        End Sub


        Public Class MediaListGalleryAjaxWorker
            Inherits SlideGalleryAjaxWorker

            Protected Overrides Sub getThumbnail(ByRef content_type_row As DTIMediaManager.dsMedia.DTIMediaTypesRow, ByRef media_row As DTIMediaManager.dsMedia.DTIMediaManagerRow)
                Dim desc As String = ""

                If media_row.Content_Type = "Image" Then
                    Dim insertImage As ImageInsertOptions = DirectCast(Page.LoadControl("~/res/DTIContentManagement/ImageInsertOptions.ascx"), ImageInsertOptions)
                    insertImage.MediaRow = media_row

                    Dim hsThumb As New ImageThumb
                    If RefreshImages Then hsThumb.RefreshImage = True
                    If Not media_row.IsDescriptionNull Then desc = media_row.Description
                    If Not desc Is Nothing AndAlso desc <> "" Then hsThumb.Caption = desc
                    hsThumb.MaxThumbWidth = MaximumThumbnailWidth
                    hsThumb.MaxThumbHeight = MaximumThumbnailHeight
                    hsThumb.ImageId = media_row.Content_Id

                    addThumbNail(hsThumb, insertImage)
                ElseIf media_row.Content_Type = "Video" Then
                    Dim insertVid As VideoInsertOptions = DirectCast(Page.LoadControl("~/res/DTIContentManagement/VideoInsertOptions.ascx"), VideoInsertOptions)
                    insertVid.MediaRow = media_row

                    Dim myVidRow As DTIVideoManagerRow = myVideos.FindById(media_row.Content_Id)
                    If Not myVidRow Is Nothing Then
                        insertVid.VideoHeight = myVidRow.height
                        insertVid.VideoWidth = myVidRow.width
                    End If

                    Dim hsThumb As New VideoThumb
                    If RefreshImages Then hsThumb.RefreshImage = True
                    hsThumb.MaxThumbWidth = MaximumThumbnailWidth
                    hsThumb.MaxThumbHeight = MaximumThumbnailHeight
                    hsThumb.VideoId = media_row.Content_Id

                    addThumbNail(hsThumb, insertVid)
                End If
            End Sub

            Public Sub New()
                items_per_page = 8
                ThumbSpanHeight = 200
                Me.Content_Types.AddRange(New String() {"Image", "Video"})
            End Sub
        End Class


    End Class