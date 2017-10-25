Imports DTIMediaManager.dsMedia
Imports DTIImageManager
Imports DTIImageManager.dsImageManager
Imports DTIVideoManager
Imports DTIVideoManager.dsDTIVideo
Imports System.Web.HttpServerUtility
Imports DTIServerControls.DTISharedVariables

#If DEBUG Then
Public Class SharedMediaVariables
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class SharedMediaVariables
#End If
        Public Shared ReadOnly Property PreviewList() As ArrayList
            Get
                If Session("DTIGalleryPreviewList") Is Nothing Then
                    Session("DTIGalleryPreviewList") = New ArrayList
                End If
                Return Session("DTIGalleryPreviewList")
            End Get
        End Property

        Public Shared ReadOnly Property myMediaTable() As DTIMediaManagerDataTable
            Get
                Return dsMedia.DTIMediaManager
            End Get
        End Property

        Private Shared _myDTIMediaTypes As DTIMediaTypesDataTable
        Public Shared ReadOnly Property myDTIMediaTypes() As DTIMediaTypesDataTable
            Get
                initDTIMediaTypes()
                Return _myDTIMediaTypes
            End Get
        End Property

        Public Shared ReadOnly Property dsMedia() As dsMedia
            Get
                If Session("DTIGalleryDataset") Is Nothing Then
                    Session("DTIGalleryDataset") = New dsMedia
                End If
                Return Session("DTIGalleryDataset")
            End Get
        End Property

        Public Shared ReadOnly Property myImages() As DTIImageManagerDataTable
            Get
                Return SharedImageVariables.myImages
            End Get
        End Property

        Public Shared ReadOnly Property myVideos() As DTIVideoManagerDataTable
            Get
                Return MyFlashWrapper.videoSaveTable
            End Get
        End Property

        Public Shared ReadOnly Property MyFlashWrapper() As ffmpegHelper
            Get
                If Session("MyFlashWrapper") Is Nothing Then
                    Session("MyFlashWrapper") = New ffmpegHelper(BaseClasses.DataBase.getHelper())
                End If
                Return Session("MyFlashWrapper")
            End Get
        End Property

        Public Shared Sub initDTIMediaTypes()
            If _myDTIMediaTypes Is Nothing Then
                _myDTIMediaTypes = New DTIMediaTypesDataTable()
            End If
            SyncLock _myDTIMediaTypes
                If _myDTIMediaTypes.Count = 0 Then
                    Dim sqlHelper As BaseClasses.BaseHelper = BaseClasses.DataBase.getHelper
                    sqlHelper.checkAndCreateTable(_myDTIMediaTypes)
                    sqlHelper.FillDataTable("select * from DTIMediaTypes", _myDTIMediaTypes)
                    If _myDTIMediaTypes.Count = 0 Then
                        _myDTIMediaTypes.AddDTIMediaTypesRow("~/res/DTIImageManager/ViewImage.aspx?Id=", _
                            "~/res/DTIImageManager/ViewImage.aspx?Id=", "Image", "Image")
                        _myDTIMediaTypes.AddDTIMediaTypesRow("~/res/DTIVideoManager/ViewVideoScreenShot.aspx?showPlayOverlay=1&Id=", _
                                       "~/res/DTIVideoManager/VideoViewer.aspx?autoPlay=1&Id=", "Video", "Iframe")
                        _myDTIMediaTypes.AddDTIMediaTypesRow(Nothing, Nothing, "page", Nothing)
                        _myDTIMediaTypes.AddDTIMediaTypesRow(Nothing, Nothing, "editPanel", Nothing)
                        sqlHelper.Update(_myDTIMediaTypes)
                    End If
                End If
            End SyncLock
        End Sub


    End Class
