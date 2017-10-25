Imports HighslideControls.SharedHighslideVariables
Imports DTIVideoManager.dsDTIVideo

''' <summary>
''' control to preview and edit title screenshot of an uploaded video
''' </summary>
''' <remarks></remarks>
#If DEBUG Then
Public Class EditVideoControl
    Inherits VideoBase
#Else
            <ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never), ComponentModel.ToolboxItem(False)> _
            Public Class EditVideoControl
                Inherits VideoBase
#End If
    Protected _myEditor As VideoPreview

    ''' <summary>
    ''' The video preview control for this editor.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("The video preview control for this editor.")> _
    Protected ReadOnly Property myEditor() As VideoPreview
        Get
            If _myEditor Is Nothing Then
                _myEditor = DirectCast(Page.LoadControl("~/res/DTIVideoManager/VideoPreview.ascx"), VideoPreview)
            End If
            Return _myEditor
        End Get
    End Property

    ''' <summary>
    ''' highslide scheme for the video pop-up (if used)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("highslide scheme for the video pop-up (if used)")> _
    Public Property VideoOutline() As Highslide_Outline_Scheme
        Get
            Return myEditor.VideoOutline
        End Get
        Set(ByVal value As Highslide_Outline_Scheme)
            myEditor.VideoOutline = value
        End Set
    End Property

    Private _myVideoRow As DTIVideoManagerRow

    ''' <summary>
    ''' Datarow of this video 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.ComponentModel.Description("Datarow of this video")> _
    Public Property myVideoRow() As DTIVideoManagerRow
        Get
            Return _myVideoRow
        End Get
        Set(ByVal value As DTIVideoManagerRow)
            _myVideoRow = value
        End Set
    End Property


    Private Sub EditVideoControl_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        myEditor.myVideoRow = myVideoRow
        myEditor.MyFlashWrapper = MyFlashWrapper
        Controls.Add(myEditor)
    End Sub
End Class
