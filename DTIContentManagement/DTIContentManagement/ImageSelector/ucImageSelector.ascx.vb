Imports DTIImageManager
Imports DTIMediaManager

Public Class ucImageSelector
    Inherits BaseControl

    Public Property Category As String = "Other"
    Public Property UploadOnly As Boolean = False
    Public Property MultiSelect As Boolean = False
    'Public Property showVideos As Boolean = True
    Public Property HideButton As Boolean = False

    Private _SelectedImageIDs As New List(Of String)
    Public ReadOnly Property SelectedImageIDs As List(Of String)
        Get
            Return _SelectedImageIDs
        End Get
    End Property

    Private _VisibleCategories As New List(Of String)
    Public ReadOnly Property VisibleCategories As List(Of String)
        Get
            Return _VisibleCategories
        End Get
    End Property

    Private _onSelectCallback As String = ""
    Public Property onSelectCallback As String
        Get
            If _onSelectCallback <> "" Then
                _onSelectCallback = _onSelectCallback.Trim.TrimEnd(";") & ";"
            End If
            Return _onSelectCallback
        End Get
        Set(value As String)
            _onSelectCallback = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loadImages()
        If HideButton Then btnSelect.Visible = False
        If dsCurrent.DTIImageCategories.Count > 0 Then
            loadTabs()
        Else
            Tabs1.AddTab(_Category, "No Images uploaded")
        End If
    End Sub

    Private Sub loadImages(Optional ByVal foceCheck As Boolean = False)
        If dsCurrent.DTIImageCategories.Count = 0 OrElse foceCheck Then
            sqlHelper.checkAndCreateTable(New DTIImageManager.dsImageManager.DTIImageManagerDataTable)
            sqlHelper.checkAndCreateTable(New DTIImageManager.dsImageManager.DTIImageCategoriesDataTable)
            sqlHelper.FillDataTable("Select DTIImageCategories.*, Original_Filename   " & _
              "from DTIImageCategories left join DTIImageManager  " & _
              "on DTIImageCategories.Image_Id = DTIImageManager.Id where site_id = @id " & _
              "and DTIImageCategories.Image_Id not like 'Y%' and DTIImageCategories.Image_Id not like 'V%'  " & _
              "union  " & _
              "Select DTIImageCategories.*, null as Original_Filename from DTIImageCategories where site_id = @id " & _
              "and (DTIImageCategories.Image_Id like 'Y%' or DTIImageCategories.Image_Id like 'V%')  " & _
              "union  " & _
              "select -1*mm.Id as ID, mm.user_ID as Site_Id,mm.content_id as Image_Id,'All Images' as Category, mm.Date_Added as DateAdded,im.[Original_Filename] " & _
              "from DTIMEDIAMANAGER mm left outer join DTIIMAGEMANAGER im on im.[Id] = mm.[Content_Id] " & _
              "  where content_type = 'Image' and user_ID = @id", dsCurrent.DTIImageCategories, MainId)
        End If
    End Sub

    Private Sub loadTabs()
        Tabs1.Tabs.Clear()
        If dsCurrent.DTIImageCategories.Count > 0 Then
            dsCurrent.DTIImageCategories.DefaultView.Sort = "Category"
            dsCurrent.DTIImageCategories.CaseSensitive = False
            Dim Cats As DataTable = dsCurrent.DTIImageCategories.DefaultView.ToTable(True, "Category")
            For Each catrow As DataRow In Cats.Rows
                If catrow("Category") IsNot DBNull.Value Then
                    Dim cat As String = catrow("Category")
                    If VisibleCategories.Count = 0 OrElse VisibleCategories.Contains(cat) Then
                        Dim pnlMain As New Panel
                        pnlMain.CssClass = "innerdiv " & cat

                        Dim vwCatImgs As New DataView(dsCurrent.DTIImageCategories, "Category like '" & cat & "'", "DateAdded Desc", DataViewRowState.CurrentRows)
                        For Each imgrow As DataRowView In vwCatImgs
                            If cat <> "Videos" Then
                                Dim img As ucImage = LoadControl("~/res/DTIContentManagement/ucImage.ascx")
                                img.ImageID = imgrow("Image_Id")
                                img.UploadOnly = UploadOnly
                                img.MultiSelect = MultiSelect
                                If imgrow("Original_Filename") IsNot DBNull.Value Then
                                    img.Name = imgrow("Original_Filename").ToString
                                Else
                                    img.Name = imgrow("Image_Id").ToString
                                End If
                                If SelectedImageIDs.Contains(img.ImageID) Then
                                    img.Selected = True
                                End If
                                AddHandler img.Delete, AddressOf Delete_Click
                                pnlMain.Controls.Add(img)
                            Else
                                Dim img As ucVideo = LoadControl("~/res/DTIContentManagement/ucVideo.ascx")
                                img.ImageID = imgrow("Image_Id")
                                img.MultiSelect = MultiSelect
                                img.Name = imgrow("Image_Id").ToString.Substring(1)
                                If SelectedImageIDs.Contains(img.ImageID) Then
                                    img.Selected = True
                                End If
                                AddHandler img.Delete, AddressOf Delete_Click
                                pnlMain.Controls.Add(img)
                            End If
                        Next
                        Dim pnlClear As New Panel
                        pnlClear.CssClass = "clearfix"
                        pnlMain.Controls.Add(pnlClear)
                        Tabs1.AddTab(cat, pnlMain)
                    End If
                End If
            Next
            If Not Category Is Nothing Then
                If Tabs1.Tabs.Find(_Category) Is Nothing Then
                    Tabs1.AddTab(_Category, "<div class=""innerdiv " & _Category & """>No Images Uploaded</div>")
                End If
                Tabs1.SelectedTab = Tabs1.Tabs.Find(_Category)
            End If

            Tabs1.onShowCallback = "$('img.lazyImgLoad').lazyload();parent.calllightbox(this,$(ui.tab).text());"

            If MultiSelect Then
                btnSelect.Text = "Select Images"
            End If
        End If
        If VisibleCategories.Count = 0 OrElse VisibleCategories.Contains("Videos") Then
            divUploadLink.Visible = True
        Else
            divUploadLink.Visible = False
        End If
        If Tabs1.Tabs.Find("Videos") Is Nothing AndAlso (VisibleCategories.Count = 0 OrElse VisibleCategories.Contains("Videos")) Then
            Tabs1.AddTab("Videos", "")
        End If
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Dim cat As String = Tabs1.SelectedTab.Title
        If FileUpload1.HasFile Then
            Dim filename As String = FileUpload1.PostedFile.FileName.ToLower
            If filename.EndsWith("avi") Or filename.EndsWith("mpg") Or filename.EndsWith("mpeg") Or filename.EndsWith("wmv") Or filename.EndsWith("mov") Then
                Dim ffmpeg As New DTIVideoManager.ffmpegHelper(sqlHelper)
                Dim i As Integer = ffmpeg.convertToFLV(FileUpload1.PostedFile)
                Dim x As Integer
                x = 3
            Else
                Dim imgrow As dsImageManager.DTIImageManagerRow = SharedImageVariables.myImages.NewDTIImageManagerRow
                ImageUploaderControl.pushImage(FileUpload1.PostedFile, imgrow, MainId)
                SharedImageVariables.myImages.AddDTIImageManagerRow(imgrow)
                Try
                    sqlHelper.Update(SharedImageVariables.myImages)
                Catch ex As Exception
                    SharedImageVariables.myImages.Clear()
                    SharedImageVariables.myImages.AcceptChanges()
                    imgrow = SharedImageVariables.myImages.NewDTIImageManagerRow
                    ImageUploaderControl.pushImage(FileUpload1.PostedFile, imgrow, MainId)
                    SharedImageVariables.myImages.AddDTIImageManagerRow(imgrow)
                End Try


                Dim catrow As dsImageManager.DTIImageCategoriesRow = dsCurrent.DTIImageCategories.NewDTIImageCategoriesRow

                If cat = "Videos" Then
                    cat = "Backdrops"
                End If
                With catrow
                    .Site_Id = MainId
                    .Image_Id = imgrow.Id
                    .Category = cat
                    .DateAdded = Now
                    .Item("Original_Filename") = imgrow.Original_Filename
                End With
                dsCurrent.DTIImageCategories.AddDTIImageCategoriesRow(catrow)
                sqlHelper.Update(dsCurrent.DTIImageCategories)

                Dim medRow As dsMedia.DTIMediaManagerRow = _
                        SharedMediaVariables.myMediaTable.NewDTIMediaManagerRow

                With medRow
                    .Component_Type = "ContentManagement"
                    .Content_Id = imgrow.Id
                    .Content_Type = "Image"
                    .Date_Added = Now
                    .Published = True
                    .Removed = False
                    .User_Id = MainId
                    .Permanent_URL = "DTIImageManager/ViewImage.aspx?Id=" & imgrow.Id
                End With
                SharedMediaVariables.myMediaTable.AddDTIMediaManagerRow(medRow)
                sqlHelper.Update(SharedMediaVariables.myMediaTable)
            End If

            redirect(cat)
        End If
    End Sub

    Private Sub Delete_Click(ByVal ImageID As String)
        Dim imgID As Integer = -1
        If Integer.TryParse(ImageID, imgID) Then
            sqlHelper.SafeExecuteNonQuery("Delete from DTIImageManager where id=@id", New Object() {imgID})
            Try
                sqlHelper.SafeExecuteNonQuery("Delete from DTIMediaManager where Content_type='Image' and Content_ID=@id", New Object() {imgID})
            Catch ex As Exception
            End Try
            Try
                sqlHelper.SafeExecuteNonQuery("Delete from DTIImageCategories where Image_Id=@id", New Object() {imgID})
            Catch ex As Exception
            End Try
        End If

        'If dv.Count > 0 Then
        '    Dim cat As dsImageManager.DTIImageCategoriesRow = dsCurrent.DTIImageCategories.FindById(dv(0)("id"))

        '    If cat IsNot Nothing Then
        '        Try
        '            cat.Delete()
        '            sqlHelper.Update(dsCurrent.DTIImageCategories)
        '        Catch ex As Exception
        '        End Try

        '        Dim imgID As Integer = -1

        '        Dim doSiteUpdate As Boolean = False
        '        If Integer.TryParse(ImageID, imgID) Then
        '            Dim imgrow As DTIImageManager.dsImageManager.DTIImageManagerRow = DTIImageManager.SharedImageVariables.myImages.FindById(imgID)

        '            If imgrow IsNot Nothing Then
        '                imgrow.Delete()
        '                sqlHelper.Update(DTIImageManager.SharedImageVariables.myImages)
        '            Else
        '                sqlHelper.SafeExecuteNonQuery("Delete from DTIImageManager where id=@id", New Object() {imgID})
        '            End If

        '        End If

        '        'Dim gal As dsSites.GalleryRow '= dsCurrent.Gallery.FindByImageID(ImageID)

        '        'Dim dvGal As New DataView(dsCurrent.Gallery, "Image_Id=" & ImageID, "", DataViewRowState.CurrentRows)
        '        'If dvGal.Count > 0 Then gal = dvGal(0).Row

        '        'If gal IsNot Nothing Then
        '        '    gal.Delete()
        '        '    sqlHelper.Update(dsCurrent.Gallery)
        '        'Else
        '        '    sqlHelper.SafeExecuteNonQuery("delete from Gallery where Image_id = @img;", _
        '        '                                                     New Object() {ImageID})
        '        'End If

        '    End If

        redirect()
        'End If
    End Sub

    Private Sub btnEmbed_Click(sender As Object, e As System.EventArgs) Handles btnEmbed.Click
        If Not tbLink.Text = "" Then
            Dim url As String = tbLink.Text
            If Not url.StartsWith("http://") Then
                url = "http://" & url
            End If
            Dim vidID As String = ""
            Dim options As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.CultureInvariant Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Compiled
            Dim urlDom As New Regex(":\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*", options)
            Dim strDom As String = urlDom.Match(url).ToString().Trim

            If strDom.Contains("youtu") Then
                Dim r As New Regex("(?:youtu.be\/|embed\/|v=)(?<id>.*?)$", options)
                vidID = "Y" & r.Match(strDom).Groups("id").ToString()
            ElseIf strDom.Contains("vimeo") Then
                Dim r As New Regex(".*?vimeo\.com(?:\/video)?\/(?<id>\d*?)(?:\?.*)?$", options)
                vidID = "V" & r.Match(strDom).Groups("id").ToString()
            End If

            If vidID <> "" Then
                Dim newEmbed As dsImageManager.DTIImageCategoriesRow
                newEmbed = dsCurrent.DTIImageCategories.NewDTIImageCategoriesRow

                With newEmbed
                    .Image_Id = vidID
                    .Category = "Videos"
                    .Site_Id = MainId
                    .DateAdded = System.DateTime.Now
                End With

                dsCurrent.DTIImageCategories.Rows.Add(newEmbed)
                sqlHelper.Update(dsCurrent.DTIImageCategories)

                redirect("Videos")
            End If
        End If
    End Sub

    Private Sub redirect(Optional ByVal cat As String = Nothing)
        Dim re As New Regex("(cat\=).*?\&")
        If cat Is Nothing Then
            Response.Redirect(re.Replace(Request.Url.OriginalString, "$1" & Tabs1.SelectedTab.Title & "&"))
        Else
            Response.Redirect(re.Replace(Request.Url.OriginalString, "$1" & cat & "&"))
        End If

    End Sub
End Class