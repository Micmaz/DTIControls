Imports DTIMediaManager.dsMedia
Imports DTIImageManager
Imports DTIVideoManager
Imports DTIGallery.dsGallery
Imports DTIGallery.DTIGallerySharedVariables
Imports HighslideControls.SharedHighslideVariables

''' <summary>
''' media gallery with built-in editor for changing settings in-place. Available in DTIControls toolbox.
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class DTISocialGallery
    Inherits DTISlideGallery
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTISocialGallery
        Inherits DTISlideGallery
#End If
        Public DataFetched As Boolean = False
        Public dataHelper As GalleryDataHelper

        Private _show_sharing_caption As Boolean = False
        Public Property ShowSharingOnCaption() As Boolean
            Get
                Return _show_sharing_caption
            End Get
            Set(ByVal value As Boolean)
                _show_sharing_caption = value
            End Set
        End Property

        Private _show_rating_thumb As Boolean = False
        Public Property ShowRatingOnThumbnail() As Boolean
            Get
                Return _show_rating_thumb
            End Get
            Set(ByVal value As Boolean)
                _show_rating_thumb = value
            End Set
        End Property

        Private _show_rating_caption As Boolean = False
        Public Property ShowRatingOnCaption() As Boolean
            Get
                Return _show_rating_caption
            End Get
            Set(ByVal value As Boolean)
                _show_rating_caption = value
            End Set
        End Property

        Public Overrides ReadOnly Property Menu_Icon_Url() As String
            Get
                Return BaseClasses.Scripts.ScriptsURL & "DTIGallery/GalleryIcon.png"
            End Get
        End Property

        Friend _galleryEdit As GalleryEditControl
        Public ReadOnly Property GalleryEdit() As GalleryEditControl
            Get
                If _galleryEdit Is Nothing Then
                    _galleryEdit = DirectCast(Page.LoadControl("~/res/DTIGallery/GalleryEditControl.ascx"), GalleryEditControl)
                End If
                Return _galleryEdit
            End Get
        End Property

        Private Sub DTISocialGallery_Init1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Component_Type = "Gallery"
            dataHelper = New GalleryDataHelper(contentType, parallelhelper)
        End Sub

        Private Sub DTISocialGallery_DataReady() Handles Me.DataReady
            DataFetched = True
        End Sub

        Private Sub DTISocialGallery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jsonControl.workerclass = "DTIGallery.DTISocialGallery+SocialGalleryAjaxWorker"  'better way of getting this?
            addUploadLink("Upload Media", "~/res/DTIGallery/UploadMedia.aspx")
            If Not DataFetched AndAlso Not mypage.IsPostBack Then
                parallelhelper.executeParallelDBCall()
            End If
            setDataProperties()
            mediaSearcher.UseSortButtons = True
            jQueryLibrary.jQueryInclude.addScriptFile(Me.Page, "DTIGallery/SocialGallery.css", "text/css", True)
            If ShowRatingOnCaption OrElse ShowRatingOnThumbnail Then callbacks.Add("initStarRaters")
        End Sub

        Private Sub DTISocialGallery_LoadControls(ByVal modeChanged As Boolean) Handles Me.LoadControls
            If Mode = modes.Write Then
                GalleryEdit.caller = Me
				If setupPanel Is Nothing Then setupPanel = New Panel
				setupPanel.Controls.Add(GalleryEdit)
            End If
        End Sub

        Private Sub DTISocialGallery_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If ShowRatingOnCaption Or ShowRatingOnThumbnail Then
                Dim scriptLoader As New DTIMediaManager.MediaRater
                scriptLoader.ValuesPerStar = 1
                Controls.Add(scriptLoader)
                scriptLoader.Visible = False
            End If
            If ShowSharingOnCaption Then
                Dim addThisLoader As New DTIMiniControls.AddThisControl
                Controls.Add(addThisLoader)
                addThisLoader.Visible = False
            End If

            If Mode = modes.Write Then
                ShowBorder = True
            Else
                ShowBorder = False
            End If
        End Sub

        Private Sub setDataProperties()
            If dataHelper.GalleryRow IsNot Nothing Then
                With dataHelper.GalleryRow
                    If Not .IsComponent_TypeNull Then Component_Type = .Component_Type
                    If Not .IsGallery_WidthNull Then GalleryWidth = .Gallery_Width
                    If Not .IsGallery_HeightNull Then GalleryHeight = .Gallery_Height
                    If Not .IsShow_First_And_LastNull Then ShowFirstAndLastButtons = .Show_First_And_Last
                    If Not .IsShow_PagingNull Then ShowPaging = .Show_Paging
                    If Not .IsShow_SearchingNull Then ShowSearching = .Show_Searching
                    If Not .IsShow_UploadNull Then ShowUpload = .Show_Upload
                    If Not .IsShow_Rating_ThumbNull Then ShowRatingOnThumbnail = .Show_Rating_Thumb
                    If Not .IsShow_Rating_CaptionNull Then ShowRatingOnCaption = .Show_Rating_Caption
                    If Not .IsShow_Sharing_CaptionNull Then ShowSharingOnCaption = .Show_Sharing_Caption
                End With
            End If
        End Sub

        Public Class SocialGalleryAjaxWorker
            Inherits SlideGalleryAjaxWorker

            Private _show_thumb_info As Boolean = False
            Public Property ShowInfoOnThumbNail() As Boolean
                Get
                    Return _show_thumb_info
                End Get
                Set(ByVal value As Boolean)
                    _show_thumb_info = value
                End Set
            End Property

            Private _show_author_thumb As Boolean = False
            Public Property ShowAuthorOnThumbnail() As Boolean
                Get
                    Return _show_author_thumb
                End Get
                Set(ByVal value As Boolean)
                    _show_author_thumb = value
                End Set
            End Property

            Private _show_pub_date_thumb As Boolean = False
            Public Property ShowPublishDateOnThumbnail() As Boolean
                Get
                    Return _show_pub_date_thumb
                End Get
                Set(ByVal value As Boolean)
                    _show_pub_date_thumb = value
                End Set
            End Property

            Private _show_rating_thumb As Boolean = False
            Public Property ShowRatingOnThumbnail() As Boolean
                Get
                    Return _show_rating_thumb
                End Get
                Set(ByVal value As Boolean)
                    _show_rating_thumb = value
                End Set
            End Property

            Private _show_author_caption As Boolean = False
            Public Property ShowAuthorOnCaption() As Boolean
                Get
                    Return _show_author_caption
                End Get
                Set(ByVal value As Boolean)
                    _show_author_caption = value
                End Set
            End Property

            Private _show_pub_date_caption As Boolean = False
            Public Property ShowPublishDateOnCaption() As Boolean
                Get
                    Return _show_pub_date_caption
                End Get
                Set(ByVal value As Boolean)
                    _show_pub_date_caption = value
                End Set
            End Property

            Private _show_rating_caption As Boolean = False
            Public Property ShowRatingOnCaption() As Boolean
                Get
                    Return _show_rating_caption
                End Get
                Set(ByVal value As Boolean)
                    _show_rating_caption = value
                End Set
            End Property

            Private _show_sharing_caption As Boolean = False
            Public Property ShowSharingOnCaption() As Boolean
                Get
                    Return _show_sharing_caption
                End Get
                Set(ByVal value As Boolean)
                    _show_sharing_caption = value
                End Set
            End Property

            Public Overrides Sub getPage()
                setDataProperties()
                MyBase.getPage()
            End Sub

            Protected Overrides Sub getThumbnail(ByRef content_type_row As DTIMediaManager.dsMedia.DTIMediaTypesRow, ByRef media_row As DTIMediaManager.dsMedia.DTIMediaManagerRow)
                Dim captionInfo As New DTIMediaManager.MediaInfo

                Dim thumbInfo As DTIMediaManager.MediaInfo
                If ShowInfoOnThumbNail Then
                    thumbInfo = New DTIMediaManager.MediaInfo
                End If

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

                hsThumb.CaptionControls.Add(captionInfo)
                hsThumb.align = "center"
                hsThumb.EnsureHeader = False

                addThumbNail(hsThumb, thumbInfo)

                captionInfo.MediaRow = media_row
                captionInfo.ValuesPerStar = 1
                captionInfo.TitleIsLink = False
                captionInfo.ShowDescription = True
                captionInfo.ShowPubAuthor = ShowAuthorOnCaption
                captionInfo.ShowPubDate = ShowPublishDateOnCaption
                captionInfo.ShowRating = ShowRatingOnCaption
                captionInfo.ShowSharing = ShowSharingOnCaption

                If Not thumbInfo Is Nothing Then
                    thumbInfo.MediaRow = media_row
                    thumbInfo.ValuesPerStar = 1
                    thumbInfo.TitleIsLink = False
                    thumbInfo.ShowDescription = False
                    thumbInfo.ShowPubAuthor = ShowAuthorOnThumbnail
                    thumbInfo.ShowPubDate = ShowPublishDateOnThumbnail
                    thumbInfo.ShowRating = ShowRatingOnThumbnail
                    thumbInfo.ShowSharing = False
                End If
            End Sub

            Public Sub New()
                'Component_Type = "Gallery"
                Content_Types.AddRange(New String() {"Image", "Video"})
            End Sub

            Private Sub setDataProperties()
                Dim dataHelper = New GalleryDataHelper(optionHash("contentType"), parallelhelper)
                If dataHelper.GalleryRow IsNot Nothing Then
                    With dataHelper.GalleryRow
                        If Not .IsComponent_TypeNull Then Component_Type = .Component_Type
                        If Not .IsThumb_HeightNull Then MaximumThumbnailHeight = .Thumb_Height
                        If Not .IsThumb_WidthNull Then MaximumThumbnailWidth = .Thumb_Width
                        If Not .IsItems_Per_PageNull Then items_per_page = .Items_Per_Page
                        If Not .IsSpan_HeightNull Then ThumbSpanHeight = .Span_Height
                        If Not .IsSpan_WidthNull Then ThumbSpanWidth = .Span_Width
                        If Not .IsShow_Author_CaptionNull Then ShowAuthorOnCaption = .Show_Author_Caption
                        If Not .IsShow_Author_ThumbNull Then ShowAuthorOnThumbnail = .Show_Author_Thumb
                        If Not .IsShow_Publish_Date_CaptionNull Then ShowPublishDateOnCaption = .Show_Publish_Date_Caption
                        If Not .IsShow_Publish_Date_ThumbNull Then ShowPublishDateOnThumbnail = .Show_Publish_Date_Thumb
                        If Not .IsShow_Rating_CaptionNull Then ShowRatingOnCaption = .Show_Rating_Caption
                        If Not .IsShow_Rating_ThumbNull Then ShowRatingOnThumbnail = .Show_Rating_Thumb
                        If Not .IsShow_Sharing_CaptionNull Then ShowSharingOnCaption = .Show_Sharing_Caption
                        If Not .IsShow_Thumb_InfoNull Then ShowInfoOnThumbNail = .Show_Thumb_Info
                    End With
                End If

            End Sub
        End Class


    End Class
