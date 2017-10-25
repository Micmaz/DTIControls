Imports DTIImageManager.dsImageManager
Imports DTIImageManager
Imports DTIVideoManager
Imports DTIVideoManager.dsDTIVideo
Imports DTIMediaManager.dsMedia
Imports DTIServerControls.DTISharedVariables
Imports HighslideControls.SharedHighslideVariables
Imports DTIMediaManager
#If DEBUG Then
Partial Public Class PreviewFiles
    Inherits BaseClasses.BaseSecurityPage
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Partial Public Class PreviewFiles
        Inherits BaseClasses.BaseSecurityPage
#End If
        Private ThemeAdded As Boolean = False

        Public ReadOnly Property myImages() As DTIImageManagerDataTable
            Get
                Return SharedMediaVariables.myImages
            End Get
        End Property

    Public ReadOnly Property myGalleryObjects() As DTIMediaManager.dsMedia.DTIMediaManagerDataTable
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

        Public ReadOnly Property PreviewList() As ArrayList
            Get
                Return SharedMediaVariables.PreviewList
            End Get
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            jQueryLibrary.jQueryInclude.addScriptFile(Page, "/DTIAdminPanel/iframe-default.css", "text/css")
            HighslideControls.HighslideHeaderControl.addToPage(Me).isInnerFrame = True
            For Each ele As DTIMediaManagerRow In PreviewList
                Dim mediaPreview As New EditMediaControl
                phFilePreview.Controls.Add(mediaPreview)

                If ele.Content_Type = "Video" Then
                    Dim videoEdit As New EditVideoControl
                    videoEdit.ID = "vidPreview_" & ele.Content_Id
                    videoEdit.myVideoRow = myVideos.FindById(ele.Content_Id)

                    mediaPreview.myControlHolder.Add(videoEdit)
                    addTheme()
                ElseIf ele.Content_Type = "Image" Then
                    Dim imageEdit As New ImageEditorControl
                    imageEdit.ImageRow = myImages.FindById(ele.Content_Id)
                    imageEdit.ID = "imgPreview_" & ele.Content_Id

                    mediaPreview.myControlHolder.Add(imageEdit)
                End If

                mediaPreview.MyMediaRow = ele
            Next
        End Sub

        Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            For Each editor As EditMediaControl In phFilePreview.Controls
                Dim mediaRow As DTIMediaManagerRow = editor.MyMediaRow
                mediaRow.Description = editor.Description
                mediaRow.Title = editor.Title
                editor.saveTags()
            Next

            Try
                sqlHelper.Update(myVideos)
            Catch ex As Exception
            End Try
            Try
                sqlHelper.Update(myImages)
            Catch ex As Exception
            End Try
            Try
                sqlHelper.Update(myGalleryObjects)
            Catch ex As Exception
            End Try

            phFilePreview.Visible = False
            'var i; for (i = 0; i < parent.frames.length; i++) { if ($(""img[src*='id\=" & myVideoRow.Id & "'],img[src*='Id\=" & myVideoRow.Id & "'],img[src*='ID\=" & myVideoRow.Id & "']"", parent.frames[i].document).length > 0) { parent.frames[i].location.reload(true); parent.frames[i].location.hash = 'i" & myVideoRow.Id & "'; }}
            Response.Redirect("~/res/DTIContentManagement/MediaList.aspx")
        End Sub

        Private Sub PreviewFiles_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim hsHeader As HighslideControls.HighslideHeaderControl = HighslideControls.HighslideHeaderControl.addToPage(Me)
            hsHeader.isInnerFrame = True
            btnSave.OnClientClick = "prepareCurrentTags();"
        End Sub

        Private Sub addTheme()
            If Not ThemeAdded Then
                'jQueryThemes.ThemeAdder.AddTheme(Me, jQueryThemes.ThemeAdder.themes.ui_lightness)
                ThemeAdded = True
            End If
        End Sub
    End Class