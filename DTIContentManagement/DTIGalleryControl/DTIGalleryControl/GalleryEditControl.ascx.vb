Imports DTIServerControls.DTISharedVariables
Imports DTIGallery.dsGallery
Imports DTIGallery.DTIGallerySharedVariables

#If DEBUG Then
Partial Public Class GalleryEditControl
    Inherits BaseClasses.BaseSecurityUserControl
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class GalleryEditControl
        Inherits BaseClasses.BaseSecurityUserControl
#End If

#Region "Properties"

        Public ReadOnly Property gallWidth() As String
            Get
                If caller.GalleryWidth.IsEmpty Then
                    Return ""
                Else
                    Return caller.GalleryWidth.Value
                End If
            End Get
        End Property

        Public ReadOnly Property gallHeight() As String
            Get
                If caller.GalleryHeight.IsEmpty Then
                    Return ""
                Else
                    Return caller.GalleryHeight.Value
                End If
            End Get
        End Property

        Private ReadOnly Property GalleryRow() As dsGallery.DTIGalleryRow
            Get
                Try
                    Return CType(caller, DTIGallery.DTISocialGallery).dataHelper.GalleryRow
                Catch ex As Exception
                    Return Nothing
                End Try
            End Get
        End Property

        Public ReadOnly Property thumbWidth() As Integer
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsThumb_WidthNull Then
                    Return 120
                Else
                    Return GalleryRow.Thumb_Width
                End If
            End Get
        End Property

        Public ReadOnly Property thumbHeight() As Integer
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsThumb_HeightNull Then
                    Return 140
                Else
                    Return GalleryRow.Thumb_Height
                End If
            End Get
        End Property

        Public ReadOnly Property spanWidth() As Integer
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsSpan_WidthNull Then
                    Return 130
                Else
                    Return GalleryRow.Span_Width
                End If
            End Get
        End Property

        Public ReadOnly Property spanHeight() As Integer
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsSpan_HeightNull Then
                    Return 150
                Else
                    Return GalleryRow.Span_Height
                End If
            End Get
        End Property

        Public ReadOnly Property items_per_page() As Integer
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsItems_Per_PageNull Then
                    Return 4
                Else
                    Return GalleryRow.Items_Per_Page
                End If
            End Get
        End Property

        Public ReadOnly Property ShowThumbInfo() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Thumb_InfoNull OrElse Not GalleryRow.Show_Thumb_Info Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowPubDateOnThumbnail() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Publish_Date_ThumbNull OrElse Not GalleryRow.Show_Publish_Date_Thumb Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowAuthorOnThumbnail() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Author_ThumbNull OrElse Not GalleryRow.Show_Author_Thumb Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowRatingOnThumbnail() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Rating_ThumbNull OrElse Not GalleryRow.Show_Rating_Thumb Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowPubDateOnCaption() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Publish_Date_CaptionNull OrElse Not GalleryRow.Show_Publish_Date_Caption Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowAuthorOnCaption() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Author_CaptionNull OrElse Not GalleryRow.Show_Author_Caption Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowRatingOnCaption() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Rating_CaptionNull OrElse Not GalleryRow.Show_Rating_Caption Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property ShowSharingOnCaption() As Boolean
            Get
                If GalleryRow Is Nothing OrElse GalleryRow.IsShow_Sharing_CaptionNull OrElse Not GalleryRow.Show_Sharing_Caption Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Private _caller As DTISlideGallery
        Public Property caller() As DTISlideGallery
            Get
                Return _caller
            End Get
            Set(ByVal value As DTISlideGallery)
                _caller = value
            End Set
        End Property

#End Region

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim newRow As Boolean = False
            Dim _gall_row As DTIGalleryRow
            If GalleryRow Is Nothing Then
                sqlHelper.SafeFillTable("select * from " & dsGall.DTIGallery.TableName & _
                                    " where MainId = @mainid and Content_Type = @contType", dsGall.DTIGallery, _
                                    New Object() {MasterMainId, caller.contentType})

                For Each row As DTIGalleryRow In dsGall.DTIGallery
                    If row.Content_Type = caller.contentType Then
                        _gall_row = row
                    End If
                Next

                If _gall_row Is Nothing Then
                    _gall_row = dsGall.DTIGallery.NewDTIGalleryRow
                    _gall_row.Content_Type = caller.contentType
                    _gall_row.MainId = MasterMainId
                    newRow = True
                End If
            Else
                _gall_row = GalleryRow
            End If

            Dim i As Integer
            With _gall_row
                If Integer.TryParse(tbGalleryHeight.Text, i) AndAlso i > 0 Then .Gallery_Height = tbGalleryHeight.Text
                If Integer.TryParse(tbGalleryWidth.Text, i) AndAlso i > 0 Then .Gallery_Width = tbGalleryWidth.Text
                If Integer.TryParse(tbItemsPerPage.Text, i) AndAlso i > 0 Then .Items_Per_Page = tbItemsPerPage.Text
                If Integer.TryParse(tbSpanHeight.Text, i) AndAlso i > 0 Then .Span_Height = tbSpanHeight.Text
                If Integer.TryParse(tbSpanWidth.Text, i) AndAlso i > 0 Then .Span_Width = tbSpanWidth.Text
                If Integer.TryParse(tbThumbHeight.Text, i) AndAlso i > 0 Then .Thumb_Height = tbThumbHeight.Text
                If Integer.TryParse(tbThumbWidth.Text, i) AndAlso i > 0 Then .Thumb_Width = tbThumbWidth.Text
                .Component_Type = tbCompType.Text
                .Show_Author_Caption = cbShowAuthorOnCaption.Checked
                .Show_Author_Thumb = cbShowAuthorOnThumbnail.Checked
                .Show_First_And_Last = cbFirstAndLast.Checked
                .Show_Paging = cbPaging.Checked
                .Show_Publish_Date_Caption = cbShowPubDateOnCaption.Checked
                .Show_Publish_Date_Thumb = cbShowPubDateOnThumbnail.Checked
                .Show_Rating_Caption = cbShowRatingOnCaption.Checked
                .Show_Rating_Thumb = cbShowRatingOnThumbnail.Checked
                .Show_Searching = cbSearching.Checked
                .Show_Sharing_Caption = cbShowSharingOnCaption.Checked
                .Show_Thumb_Info = cbShowThumbInfo.Checked
                .Show_Upload = cbUpload.Checked
            End With

            If newRow Then
                dsGall.DTIGallery.AddDTIGalleryRow(_gall_row)
            End If

            sqlHelper.Update(dsGall.DTIGallery)
            Response.Redirect(Request.Url.OriginalString)
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim i = 0
        End Sub

        Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            tbCompType.Text = caller.Component_Type
            tbGalleryHeight.Text = gallHeight
            tbGalleryWidth.Text = gallWidth
            tbItemsPerPage.Text = items_per_page
            tbSpanHeight.Text = spanHeight
            tbSpanWidth.Text = spanWidth
            tbThumbHeight.Text = thumbHeight
            tbThumbWidth.Text = thumbWidth
            cbFirstAndLast.Checked = caller.ShowFirstAndLastButtons
            cbPaging.Checked = caller.ShowPaging
            cbSearching.Checked = caller.ShowSearching
            cbShowAuthorOnCaption.Checked = ShowAuthorOnCaption
            cbShowAuthorOnThumbnail.Checked = ShowAuthorOnThumbnail
            cbShowPubDateOnCaption.Checked = ShowPubDateOnCaption
            cbShowPubDateOnThumbnail.Checked = ShowPubDateOnThumbnail
            cbShowRatingOnCaption.Checked = ShowRatingOnCaption
            cbShowRatingOnThumbnail.Checked = ShowRatingOnThumbnail
            cbShowSharingOnCaption.Checked = ShowSharingOnCaption
            cbShowThumbInfo.Checked = ShowThumbInfo
            cbUpload.Checked = caller.ShowUpload
        End Sub
    End Class