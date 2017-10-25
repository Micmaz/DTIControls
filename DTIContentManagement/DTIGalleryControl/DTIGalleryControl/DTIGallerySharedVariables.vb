Imports DTIServerControls.DTISharedVariables
Imports DTIGallery.dsGallery

#If DEBUG Then
Public Class DTIGallerySharedVariables
#Else
    <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
    Public Class DTIGallerySharedVariables
#End If

    ''' <summary>
    ''' Gallery information cached on a session level.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("Gallery information cached on a session level.")> _
        Public Shared ReadOnly Property dsGall() As dsGallery
            Get
                If Session("DTIdsGallery") Is Nothing Then
                    Session("DTIdsGallery") = New dsGallery
                End If
                Return Session("DTIdsGallery")
            End Get
        End Property

    ''' <summary>
    '''  a helper class to load gallery information from a database.
    ''' </summary>
    ''' <remarks></remarks>
        <System.ComponentModel.Description("a helper class to load gallery information from a database.")> _
        Public Class GalleryDataHelper

            Public gallery_content_type As String = ""
            Protected parallelhelper As BaseClasses.ParallelDataHelper


            Private _galleryRow As DTIGalleryRow
            Public Property GalleryRow() As DTIGalleryRow
                Get
                    If _galleryRow Is Nothing Then
                        parallelhelper.sqlHelper.SafeFillTable("select * from DTIGallery where mainID = @mainId and Content_Type = @contentType", dsGall.DTIGallery, New Object() {MasterMainId, gallery_content_type})
                        'parallelhelper.executeParallelDBCall()
                        iterateTable()
                    End If

                    Return _galleryRow
                End Get
                Set(ByVal value As DTIGalleryRow)
                    _galleryRow = value
                End Set
            End Property

            Private Sub iterateTable()
                For Each row As DTIGalleryRow In dsGall.DTIGallery
                    If row.Content_Type = gallery_content_type Then
                        _galleryRow = row
                        Exit Sub
                    End If
                Next
            End Sub

            Public Sub New(ByVal gall_content_type As String, ByRef parHelper As BaseClasses.ParallelDataHelper)
                gallery_content_type = gall_content_type
                parallelhelper = parHelper
                'addSQLCall()
            End Sub

        End Class

    End Class
