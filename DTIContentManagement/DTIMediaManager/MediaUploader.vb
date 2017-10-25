Imports DTIImageManager.dsImageManager
Imports DTIImageManager
Imports DTIVideoManager
Imports DTIVideoManager.dsDTIVideo
Imports DTIMediaManager.dsMedia
Imports DTIUploader

''' <summary>
''' control to upload DTI-managed media objects
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
    Public Class MediaUploader
        Inherits DTIUploaderControl
#Else
<ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
Public Class MediaUploader
    Inherits DTIUploaderControl
#End If
    Public Event ImageSaved(ByVal image_id As Integer)
    Public Event VideoSaved(ByVal video_id As Integer)

    Public ReadOnly Property PreviewList() As ArrayList
        Get
            Return SharedMediaVariables.PreviewList
        End Get
    End Property

    Public ReadOnly Property myImages() As DTIImageManagerDataTable
        Get
            Return SharedMediaVariables.myImages
        End Get
    End Property

    Public ReadOnly Property myMediaTable() As DTIMediaManagerDataTable
        Get
            Return SharedMediaVariables.myMediaTable
        End Get
    End Property

    Public ReadOnly Property MyFlashWrapper() As ffmpegHelper
        Get
            Dim x As ffmpegHelper = SharedMediaVariables.MyFlashWrapper
            x.OutputPath = mypage.MapPath(DTIServerControls.DTISharedVariables.UploadFolderDefault)
            'If Not System.IO.Directory.Exists(x.OutputPath) Then
            '    System.IO.Directory.CreateDirectory(x.OutputPath)
            'End If

            Return x
        End Get
    End Property

    Public ReadOnly Property myVideos() As DTIVideoManagerDataTable
        Get
            Return SharedMediaVariables.myVideos
        End Get
    End Property

    Public Property VideoOutputPath() As String
        Get
            Return MyFlashWrapper.OutputPath
        End Get
        Set(ByVal value As String)
            MyFlashWrapper.OutputPath = value
        End Set
    End Property

    Public Property VideoOutputFileNamingConvention() As String
        Get
            Return MyFlashWrapper.OutputFileNamingConvention
        End Get
        Set(ByVal value As String)
            MyFlashWrapper.OutputFileNamingConvention = value
        End Set
    End Property

    Public Property SaveVideoInDataBase() As Boolean
        Get
            Return MyFlashWrapper.SaveVideoInDataBase
        End Get
        Set(ByVal value As Boolean)
            MyFlashWrapper.SaveVideoInDataBase = value
        End Set
    End Property

    Public Property User_Id() As Integer
        Get
            Return MyFlashWrapper.User_Id
        End Get
        Set(ByVal value As Integer)
            MyFlashWrapper.User_Id = value
        End Set
    End Property

    Public Sub myUploaderControl_handleFile(ByRef file As System.Web.HttpPostedFile) Handles Me.handleFile
        If file.FileName.EndsWith(".avi", StringComparison.InvariantCultureIgnoreCase) OrElse _
           file.FileName.EndsWith(".mov", StringComparison.InvariantCultureIgnoreCase) OrElse _
           file.FileName.EndsWith(".mpg", StringComparison.InvariantCultureIgnoreCase) OrElse _
           file.FileName.EndsWith(".wmv", StringComparison.InvariantCultureIgnoreCase) Then

            Dim vid_id As Integer = MyFlashWrapper.convertToFLV(file)
            RaiseEvent VideoSaved(vid_id)

        ElseIf file.FileName.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) OrElse _
            file.FileName.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) OrElse _
            file.FileName.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase) Then

            Dim imgRow As DTIImageManager.dsImageManager.DTIImageManagerRow = myImages.NewDTIImageManagerRow
            ImageUploaderControl.pushImage(file, imgRow, User_Id)
            myImages.AddDTIImageManagerRow(imgRow)
            sqlhelper.Update(myImages)
            RaiseEvent ImageSaved(imgRow.Id)
        End If
    End Sub

    Public Sub myUploaderControl_handleWebFile(ByVal path As String) Handles Me.handleWebFile
        If path.EndsWith(".avi", StringComparison.InvariantCultureIgnoreCase) OrElse _
           path.EndsWith(".mov", StringComparison.InvariantCultureIgnoreCase) OrElse _
           path.EndsWith(".mpg", StringComparison.InvariantCultureIgnoreCase) OrElse _
           path.EndsWith(".wmv", StringComparison.InvariantCultureIgnoreCase) Then

            Dim vid_id As Integer = MyFlashWrapper.convertToFLV(path)
            RaiseEvent VideoSaved(vid_id)

        ElseIf path.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) OrElse _
            path.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase) OrElse _
            path.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase) Then

            Dim imgRow As DTIImageManager.dsImageManager.DTIImageManagerRow = myImages.NewDTIImageManagerRow
            ImageUploaderControl.pushImage(path, imgRow, User_Id)
            myImages.AddDTIImageManagerRow(imgRow)
            sqlhelper.Update(myImages)
            RaiseEvent ImageSaved(imgRow.Id)
        End If
    End Sub

    Private Sub MediaUploader_ImageSaved(ByVal image_id As Integer) Handles Me.ImageSaved
        Dim newMedObj As DTIMediaManagerRow = myMediaTable.NewDTIMediaManagerRow
        newMedObj.Content_Type = "Image"
        newMedObj.Component_Type = Component_Type
        newMedObj.Content_Id = image_id
        newMedObj.User_Id = MainID
        newMedObj.Published = True
        newMedObj.Removed = False
        newMedObj.Date_Added = Date.Now
        newMedObj.Permanent_URL = "DTIImageManager/ViewImage.aspx?Id=" & image_id
        myMediaTable.AddDTIMediaManagerRow(newMedObj)
        sqlhelper.Update(myMediaTable)
        PreviewList.Add(newMedObj)
    End Sub

    Private Sub MediaUploader_VideoSaved(ByVal video_id As Integer) Handles Me.VideoSaved
        Dim newMedObj As DTIMediaManagerRow = myMediaTable.NewDTIMediaManagerRow
        newMedObj.Content_Type = "Video"
        newMedObj.Component_Type = Component_Type
        newMedObj.Content_Id = video_id
        newMedObj.User_Id = MainID
        newMedObj.Published = True
        newMedObj.Removed = False
        newMedObj.Date_Added = Date.Now
        newMedObj.Permanent_URL = "DTIVideoManager/ViewMovie.aspx?Id=" & video_id
        myMediaTable.AddDTIMediaManagerRow(newMedObj)
        sqlhelper.Update(myMediaTable)
        PreviewList.Add(newMedObj)
    End Sub

    Private Sub MediaUploader_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        FileFilter = FileFilters.ImagesAndVideo
        If Not mypage.IsPostBack AndAlso mypage.Request.QueryString("sid") Is Nothing Then
            sqlhelper.checkAndCreateTable(myImages)
            PreviewList.Clear()
        End If
    End Sub

    Private Sub MediaUploader_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not mypage.Request.QueryString("f") Is Nothing Then
            If mypage.Request.QueryString("f") = "image" Then
                FileFilter = DTIUploader.DTIUploaderControl.FileFilters.Images
            ElseIf mypage.Request.QueryString("f") = "video" Then
                FileFilter = DTIUploader.DTIUploaderControl.FileFilters.Videos
            End If
        End If
    End Sub


End Class
