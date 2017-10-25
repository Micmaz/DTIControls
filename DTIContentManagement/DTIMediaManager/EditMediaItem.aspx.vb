Imports DTIImageManager.dsImageManager
Imports DTIImageManager
Imports DTIVideoManager
Imports DTIVideoManager.dsDTIVideo
Imports DTIMediaManager.dsMedia
Imports DTIServerControls.DTISharedVariables
Imports HighslideControls.SharedHighslideVariables

Partial Public Class EditMediaItem
    Inherits BaseClasses.BaseSecurityPage

    Public ReadOnly Property myImages() As DTIImageManagerDataTable
        Get
            Return SharedMediaVariables.myImages
        End Get
    End Property

    Public ReadOnly Property myGalleryObjects() As DTIMediaManagerDataTable
        Get
            Return SharedMediaVariables.myMediaTable
        End Get
    End Property

    Public ReadOnly Property MyFlashWrapper() As ffmpegHelper
        Get
            Return SharedMediaVariables.MyFlashWrapper
        End Get
    End Property

    Public ReadOnly Property myVideos() As DTIVideoManagerDataTable
        Get
            Return SharedMediaVariables.myVideos
        End Get
    End Property

    Private _mediaID As Integer = -1
    Public ReadOnly Property mediaId() As Integer
        Get
            If _mediaID = -1 Then
                If Not Request.Params("mId") Is Nothing Then
                    _mediaID = Request.Params("mId")
                End If
                If Not Request.Params("mid") Is Nothing Then
                    Dim img As DataTable = sqlHelper.FillDataTable("select id from dtiImagemanager where id = @id", Request.Params("mid"))

                    If img.Rows.Count > 0 Then
                        Dim dt As New dsMedia.DTIMediaManagerDataTable
                        sqlHelper.FillDataTable("select * from dtiMediaManager where Content_Type='Image' and Content_id= @id", dt, Request.Params("mid"))
                        If dt.Count = 0 Then
                            dt.AddDTIMediaManagerRow("Image", Request.Params("mid"), True, False, "", 0, 0, "", "", "ContentManagement", Date.Now, "DTIImageManager/ViewImage.aspx?Id=" & Request.Params("mid"), 0, 0)
                            sqlHelper.Update(dt)
                        End If
                        _mediaID = dt(0).Id
                    End If
                End If
            End If
            If _mediaID = -1 Then
                Response.Clear()
                Response.Write("No image or media type specified.")
                Response.End()
            End If
            Return _mediaID
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        HighslideControls.HighslideHeaderControl.addToPage(Me).isInnerFrame = True
        Dim mediaRow As DTIMediaManagerRow = myGalleryObjects.FindById(mediaId)
        If mediaRow Is Nothing Then
            sqlHelper.SafeFillTable("select * from DTIMediaManager where Id = @mid", myGalleryObjects, _
                New Object() {mediaId})
            mediaRow = myGalleryObjects.FindById(mediaId)
        End If
        If Not mediaRow Is Nothing Then
            If mediaRow.Content_Type = "Video" Then
                Dim videoEdit As New EditVideoControl
                videoEdit.ID = "vidPreview_" & mediaRow.Content_Id
                videoEdit.myVideoRow = myVideos.FindById(mediaRow.Content_Id)
                EditMediaControl1.myControlHolder.Add(videoEdit)
                'addTheme()
            ElseIf mediaRow.Content_Type = "Image" Then
                Dim imageEdit As New ImageEditorControl
                imageEdit.ImageRow = myImages.FindById(mediaRow.Content_Id)
                imageEdit.ID = "imgPreview_" & mediaRow.Content_Id

                EditMediaControl1.myControlHolder.Add(imageEdit)
            End If

            EditMediaControl1.MyMediaRow = mediaRow
        End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim mediaRow As DTIMediaManagerRow = EditMediaControl1.MyMediaRow
        mediaRow.Description = EditMediaControl1.Description
        mediaRow.Title = EditMediaControl1.Title
        EditMediaControl1.saveTags()
        
        Try
            sqlHelper.Update(myVideos)
        Catch ex As Exception
        End Try
        Try
            sqlHelper.Update(myImages)
        Catch ex As Exception
        End Try

        sqlHelper.Update(mediaRow.Table)
        Me.ClientScript.RegisterStartupScript(Me.GetType, "closeMe", "parent.window.hs.getExpander().close();", True)
    End Sub
End Class